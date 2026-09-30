using Godot;
using System;

public partial class TorpedoRacs : GridContainer
{

	private const int Racsmeret = 10;
	private Button[,] racsGombok = new Button[Racsmeret, Racsmeret];


	// Called when the node enters the scene tree for the first time.
	private Label CimkeLetrehozas(string szoveg)
	{
		Label cimke = new Label();
		cimke.Text = szoveg;
		cimke.HorizontalAlignment = HorizontalAlignment.Center;
		cimke.VerticalAlignment = VerticalAlignment.Center;
		return cimke;
	}
	public override void _Ready()
	{
		this.Columns = 11; //11 mert vannak labelek is

		AddChild(CimkeLetrehozas(""));

		for (int x = 0; x< Racsmeret; x++)
		{
			string oszlopnev = ((char)('A' + x)).ToString();
			AddChild(CimkeLetrehozas(oszlopnev));
		}

		for (int y = 0; y < Racsmeret; y++)
		{
			string oszlopnev = ((char)('A' + y)).ToString();
			AddChild(CimkeLetrehozas((y+1).ToString()));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
