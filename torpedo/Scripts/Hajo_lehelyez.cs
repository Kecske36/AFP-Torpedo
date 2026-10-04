using Godot;
using System;
using System.Collections.Generic;


public partial class Hajo_lehelyez : GridContainer
{
    [Export] public JatekKezelo JatekVezerlo;

    private const int RacsMeret = 10;

    private readonly Control[,] mezok = new Control[RacsMeret, RacsMeret];

    private readonly bool[,] foglalt = new bool[RacsMeret, RacsMeret];

    private readonly Dictionary<Vector2I, Color> elozoSzinek = new();



    public override void _Ready()
    {
        int index = 0;
        foreach (Node child in GetChildren())
        {
            if (child is not Panel mezo)
            {
                continue;
            }

            if (index >= RacsMeret * RacsMeret)
            {
                GD.PrintErr("Túl sok mező van a rácsban!");
                return;
            }
            int x = index % RacsMeret;
            int y = index / RacsMeret;

            mezok[x, y] = mezo;
            foglalt[x, y] = false;

            index++;
        }
    }

    private bool SzabadE(int x, int y, int hossz, bool fuggoleges)
    {
        if (hossz < 1 || x < 0 || y < 0 ||
            x >= RacsMeret || y >= RacsMeret)
        {
            return false;
        }

        for (int i = 0; i < hossz; i++)
        {
            int cellaX = x + (fuggoleges ? 0 : i);
            int cellaY = y + (fuggoleges ? i : 0);

            if (cellaX >= RacsMeret || cellaY >= RacsMeret ||
                mezok[cellaX, cellaY] == null ||
                foglalt[cellaX, cellaY])
            {
                return false;
            }
        }

        return true;
    }

    public void ElojelzesTorlese()
    {
        foreach (var par in elozoSzinek)
        {
            Vector2I koordinata = par.Key;
            mezok[koordinata.X, koordinata.Y].SelfModulate = par.Value;
        }

        elozoSzinek.Clear();
    }


    public void MutatElohelyezest(int x, int y, int hossz, bool fuggoleges)
    {
        ElojelzesTorlese();
        bool ervenyes = SzabadE(x, y, hossz, fuggoleges);

        for (int i = 0; i < hossz; i++)
        {
            int cellaX = x + (fuggoleges ? 0 : i);
            int cellaY = y + (fuggoleges ? i : 0);

            if (cellaX < 0 || cellaY < 0 ||
                cellaX >= RacsMeret || cellaY >= RacsMeret ||
                mezok[cellaX, cellaY] == null)
            {
                continue;
            }

            var koordinata = new Vector2I(cellaX, cellaY);
            elozoSzinek[koordinata] = mezok[cellaX, cellaY].SelfModulate;
            mezok[cellaX, cellaY].SelfModulate =
                ervenyes ? Colors.Green : Colors.Red;
        }
    }


    public bool Hajo_lehelyezes(int x, int y, int hossz, bool fuggoleges)
    {
        if (!SzabadE(x, y, hossz, fuggoleges))
        {
            return false;
        }

        ElojelzesTorlese();

        for (int i = 0; i < hossz; i++)
        {
            int cellaX = x + (fuggoleges ? 0 : i);
            int cellaY = y + (fuggoleges ? i : 0);

            foglalt[cellaX, cellaY] = true;
            mezok[cellaX, cellaY].SelfModulate = Colors.Green;
        }

        return true;
    }

    public bool TryGetCellAtGlobalPosition(Vector2 globalPosition, out int x, out int y)
    {
        for (int cellaY = 0; cellaY < RacsMeret; cellaY++)
        {
            for (int cellaX = 0; cellaX < RacsMeret; cellaX++)
            {
                Control mezo = mezok[cellaX, cellaY];

                if (mezo != null && mezo.GetGlobalRect().HasPoint(globalPosition))
                {
                    x = cellaX;
                    y = cellaY;
                    return true;
                }
            }
        }

        x = -1;
        y = -1;
        return false;
    }

    public Vector2 GetCellGlobalPosition(int x, int y)
    {
        return mezok[x, y].GetGlobalRect().Position;
    }
}