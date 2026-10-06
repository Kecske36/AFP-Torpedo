using Godot;

public partial class Hajo_Interakcio : Control
{
	[Export] public Hajo_lehelyez Tabla { get; set; }
	[Export(PropertyHint.Range, "1,4,1")]
	public int Hossz { get; set; } = 1;

	public int RacsX { get; private set; } = -1;
	public int RacsY { get; private set; } = -1;

	private bool fuggoleges = false;
	private bool huzasban;
	private bool lehelyezve;

	private Vector2 fogasiEltolas;
	private Vector2 elozoPozicio;
	private float elozoForgatas;
	private bool elozoIrany;

	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Stop;
		PivotOffset = Vector2.Zero;
	}
	private void FrissitElojelzest()
	{
		if (Tabla == null)
			return;

		if (Tabla.TryGetCellAtGlobalPosition(GlobalPosition, out int x, out int y))
		{
			Tabla.MutatElohelyezest(x, y, Hossz, fuggoleges);
		}
		else
		{
			Tabla.ElojelzesTorlese();
		}
	}
	public override void _GuiInput(InputEvent @event)
	{
		if (lehelyezve)
			return;

		if (@event is InputEventMouseButton mouse &&
			mouse.ButtonIndex == MouseButton.Left &&
			mouse.Pressed)
		{
			huzasban = true;
			fogasiEltolas = GetGlobalMousePosition() - GlobalPosition;

			elozoPozicio = GlobalPosition;
			elozoForgatas = Rotation;
			elozoIrany = fuggoleges;

			FrissitElojelzest();
			AcceptEvent();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (!huzasban)
			return;

		if (@event is InputEventMouseMotion)
		{
			GlobalPosition = GetGlobalMousePosition() - fogasiEltolas;
			FrissitElojelzest();
			GetViewport().SetInputAsHandled();
			return;
		}

		if (@event is InputEventKey key &&
			key.Pressed && !key.Echo && key.Keycode == Key.R)
		{
			fuggoleges = !fuggoleges;
			Rotation = fuggoleges ? Mathf.Pi / 2 : 0;

			// itt a Control bal felső sarka marad a kezdőpont
			// ne a sprite közepét használd
			FrissitElojelzest();
			GetViewport().SetInputAsHandled();
			return;
		}

		if (@event is InputEventMouseButton mouse &&
			mouse.ButtonIndex == MouseButton.Left &&
			!mouse.Pressed)
		{
			huzasban = false;

			if (Tabla != null &&
				Tabla.TryGetCellAtGlobalPosition(GlobalPosition, out int x, out int y) &&
				Tabla.Hajo_lehelyezes(x, y, Hossz, fuggoleges))
			{
				GlobalPosition = Tabla.GetCellGlobalPosition(x, y);
				lehelyezve = true;
			}
			else
			{
				GlobalPosition = elozoPozicio;
				Rotation = elozoForgatas;
				fuggoleges = elozoIrany;
			}
		}
	}
}