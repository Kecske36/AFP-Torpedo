using Godot;
using System;
using System.Collections.Generic;

public partial class JatekKezelo : Control
{
	[Export] public Hajo_lehelyez JatekosTabla;
	[Export] public Label label;

	private readonly Random random = new();
	private readonly HashSet<Vector2I> gepTippelesei = new();

	public bool JatekVege { get; private set; }

	// Ellenörzés, hogy csak a saját táblájára tudja helyezni a játkos a hajóit
	public void Hajo_lehelyez(bool isEnemyBoard, int x, int y, Control mezo, Control hajo, Label label)
	{
		if (isEnemyBoard)
		{
			GD.Print("Ellenfél táblájára nem rakhatsz hajót");
			return;
		}

	}

	// Ellenörzés, hogy csak az ellenfél táblájára tudjon lőni
	public void TalaltE(bool isEnemyBoard, int x, int y, Control mezo, Label label, GepHajoi gepHajoi)
	{
		if (JatekVege)
			return;

		if (!isEnemyBoard)
		{
			GD.Print("A saját tábládra nem lőhetsz!");
			return;
		}

		if (JatekosTabla == null || !JatekosTabla.MindenHajoLerakva)
		{
			const string uzenet = "Előbb rakd le az összes hajódat!";
			GD.Print(uzenet);
			if (label != null)
				label.Text = uzenet;
			return;
		}

		if (gepHajoi == null)
		{
			GD.PushError("A gép hajói nincsenek beállítva az ellenfél tábláján.");
			return;
		}

		if (mezo.MouseFilter == Control.MouseFilterEnum.Ignore)
			return;

		bool talalat = gepHajoi.TalalatE(x, y);

		if (talalat)
		{
			GD.Print("Találat!");
			label.Text = "Találat!";
			mezo.SelfModulate = Colors.Green;
		}
		else
		{
			GD.Print("Mellé!");
			label.Text = "Mellé!";
			mezo.SelfModulate = Colors.Red;
		}

		mezo.MouseFilter = Control.MouseFilterEnum.Ignore;
		if (gepHajoi.MindenHajoElsullyedt)
		{
			JatekLezar("A játékos nyert!");
			return;
		}

		GepTippel();
	}

	// Kiírja a nyertest
	private void JatekLezar(string uzenet)
	{
		JatekVege = true;
		GD.Print(uzenet);
		if (label != null)
			label.Text = uzenet;
	}

	// Kiírja hova tippel a gép és, hogy talált-e vagy sem
	private void GepTippel()
	{
		if (JatekVege)
			return;

		if (JatekosTabla == null)
		{
			GD.PushError("A játékos táblája nincs hozzárendelve a JátékKezelőhöz.");
			return;
		}

		if (!JatekosTabla.MindenHajoLerakva)
			return;

		var lehetsegesTippek = new List<Vector2I>();
		for (int y = 0; y < 10; y++)
		{
			for (int x = 0; x < 10; x++)
			{
				var koordinata = new Vector2I(x, y);
				if (!gepTippelesei.Contains(koordinata))
					lehetsegesTippek.Add(koordinata);
			}
		}

		if (lehetsegesTippek.Count == 0)
		{
			GD.Print("A gép már minden mezőre tippelt.");
			return;
		}

		Vector2I tipp = lehetsegesTippek[random.Next(lehetsegesTippek.Count)];
		gepTippelesei.Add(tipp);
		bool talalat = JatekosTabla.GepLoves(tipp.X, tipp.Y);

		string eredmeny = talalat ? "találat" : "mellé";
		GD.Print($"A gép tippje: ({tipp.X}, {tipp.Y}) - {eredmeny}.");
		if (label != null)
			label.Text = $"A gép tippje: ({tipp.X}, {tipp.Y}) - {eredmeny}.";

		if (JatekosTabla.MindenHajoElsullyedt)
			JatekLezar("A gép nyert!");
	}
}
