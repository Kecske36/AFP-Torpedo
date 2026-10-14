using Godot;
using System;
using Torpedo.Scripts;

public partial class TorpedoRacs : Tabla
{
	[Export] public JatekKezelo JatekVezerlo;
	[Export] public Label label;
	[Export] public GepHajoi gepHajoi;

	// Tábla mezői kattintás érzékelés beállítása
	public override void _Ready()
	{
		isEnemyBoard = true;
		int index = 0;
		foreach (Node child in GetChildren())
		{
			if (child is Control mezo)
			{
				int x = index % Columns;
				int y = index / Columns;

				mezo.GuiInput += (@event) => OnMezoGuiInput(@event, x, y, mezo);

				index++;
			}
		}
		gepHajoi.HajokRandomElhelyezese();
	}

	// Játékvezérlő objektum ellenőrzése
	private void OnMezoGuiInput(InputEvent @event, int x, int y, Control mezo)
	{
		if (@event is InputEventMouseButton mouseEvent
			&& mouseEvent.Pressed
			&& mouseEvent.ButtonIndex == MouseButton.Left)
		{

			if (JatekVezerlo != null)
			{
				JatekVezerlo.TalaltE(isEnemyBoard, x, y, mezo, label, gepHajoi);
			}
			else
			{
				GD.PrintErr("A JatekVezarlo nincs beállítva az Inspectorban!");
			}
		}
	}
}
