using Godot;
using System;

public partial class TorpedoRacs : GridContainer
{
	[Export] public bool IsEnemyBoard = false;
	[Export] public JatekKezelo JatekVezerlo;
	[Export] public Label label;

	public override void _Ready()
	{
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
	}

	private void OnMezoGuiInput(InputEvent @event, int x, int y, Control mezo)
	{
		if (@event is InputEventMouseButton mouseEvent
			&& mouseEvent.Pressed
			&& mouseEvent.ButtonIndex == MouseButton.Left)
		{
			if (JatekVezerlo != null)
			{
				JatekVezerlo.TalaltE(IsEnemyBoard, x, y, mezo, label);
			}
			else
			{
				GD.PrintErr("A JatekVezarlo nincs beállítva az Inspectorban!");
			}
		}
	}
}
