using Godot;
using System;

public partial class JatekKezelo : Control
{
	public void LovesEsemeny(bool isEnemyBoard, int x, int y, Control mezo, Label label)
	{
		if (!isEnemyBoard)
		{
			GD.Print("A saját tábládra nem lőhetsz!");
			return;
		}

		GD.Print($"Lövés az ellenfél táblájára -> X: {x}, Y: {y}");
		label.Text = $"Lövés az ellenfél táblájára -> X: {x}, Y: {y}";

		// Teszt színezés:
		mezo.SelfModulate = Colors.Red;
		mezo.MouseFilter = Control.MouseFilterEnum.Ignore;
	}

	public void Hajo_lehelyez(bool isEnemyBoard, int x, int y, Control mezo, Control hajo, Label label)
	{
		if (isEnemyBoard)
		{
			GD.Print("Ellenfél táblájára nem rakhatsz hajót");
			return;
		}

	}
}
