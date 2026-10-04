using Godot;
using System;


public partial class Hajo_lehelyez : GridContainer
{
	[Export] public JatekKezelo JatekVezerlo;

	private const int RacsMeret = 10;

	private readonly Control[,] mezok = new Control[RacsMeret, RacsMeret];

	private readonly bool[,] foglalt = new bool[RacsMeret, RacsMeret];

	
	public override void _Ready()
	{
		int index = 0;
		foreach (Node child in GetChildren())
		{
			if (child is Control mezo)
			{
				int x = index % RacsMeret;
				int y = index / RacsMeret;

				mezok[x, y] = mezo;
				foglalt[x, y] = false;

				index++;
			}
		}
	}

	public void Hajo_lehelyezes(int x, int y, int hossz, bool irany)
	{
		if (irany == false) //vízszintes
		{
			for (int i = 0; i < hossz; i++)
			{
				if (x + i >= RacsMeret || foglalt[x + i, y])
				{
					GD.Print("A hajó nem helyezhető el!");
					return;
				}
			}

			for (int i = 0; i < hossz; i++)
			{
				foglalt[x + i, y] = true;
				mezok[x + i, y].SelfModulate = Colors.Gray;
			}
		}

		else //függőleges
		{
			for (int i = 0; i < hossz; i++)
			{
				if (y + i >= RacsMeret || foglalt[x, y + i])
				{
					GD.Print("A hajó nem helyezhető el!");
					return;
				}
			}

			for (int i = 0; i < hossz; i++)
			{
				foglalt[x, y + i] = true;
				mezok[x, y + i].SelfModulate = Colors.Green;
			}


		}

	}
}
