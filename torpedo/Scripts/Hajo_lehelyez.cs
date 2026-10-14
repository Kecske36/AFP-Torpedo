using Godot;
using System;
using System.Collections.Generic;
using Torpedo.Scripts;


public partial class Hajo_lehelyez : Tabla
{
    [Export] public JatekKezelo JatekVezerlo;

    private const int RacsMeret = 10;

    private readonly Control[,] mezok = new Control[RacsMeret, RacsMeret];

    private readonly bool[,] foglalt = new bool[RacsMeret, RacsMeret];

    private readonly Dictionary<Vector2I, Color> elozoSzinek = new();
    private readonly HashSet<Hajo_Interakcio> osszesHajo = new();
    private readonly HashSet<Hajo_Interakcio> lerakottHajok = new();
    private readonly HashSet<Vector2I> talalatok = new();

    public bool MindenHajoLerakva =>
        osszesHajo.Count > 0 && lerakottHajok.Count == osszesHajo.Count;

    public bool MindenHajoElsullyedt
    {
        get
        {
            int hajomezokSzama = 0;
            foreach (bool foglaltMezo in foglalt)
            {
                if (foglaltMezo)
                    hajomezokSzama++;
            }

            return hajomezokSzama > 0 && talalatok.Count == hajomezokSzama;
        }
    }


    public override void _Ready()
    {
        isEnemyBoard = false;
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

    // Játékos hajóinak elhelyezésének ellenörzése
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

            for (int szomszedY = Math.Max(0, cellaY - 1);
                 szomszedY <= Math.Min(RacsMeret - 1, cellaY + 1);
                 szomszedY++)
            {
                for (int szomszedX = Math.Max(0, cellaX - 1);
                     szomszedX <= Math.Min(RacsMeret - 1, cellaX + 1);
                     szomszedX++)
                {
                    if (foglalt[szomszedX, szomszedY])
                        return false;
                }
            }
        }

        return true;
    }

    // Előrejelzés szín törlése
    public void ElojelzesTorlese()
    {
        foreach (var par in elozoSzinek)
        {
            Vector2I koordinata = par.Key;
            mezok[koordinata.X, koordinata.Y].SelfModulate = par.Value;
        }

        elozoSzinek.Clear();
    }

    // Előrejelzés szín létrehozása
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

    public void HajoRegisztralasa(Hajo_Interakcio hajo)
    {
        osszesHajo.Add(hajo);
    }

    public void HajoLerakva(Hajo_Interakcio hajo)
    {
        if (!osszesHajo.Contains(hajo))
        {
            GD.PushError("A lerakott hajó nincs regisztrálva a játékos tábláján.");
            return;
        }

        lerakottHajok.Add(hajo);
    }

    public bool GepLoves(int x, int y)
    {
        if (x < 0 || x >= RacsMeret || y < 0 || y >= RacsMeret ||
            mezok[x, y] == null)
        {
            return false;
        }

        var koordinata = new Vector2I(x, y);
        bool talalat = foglalt[x, y];
        if (talalat)
            talalatok.Add(koordinata);

        mezok[x, y].SelfModulate = talalat ? Colors.Red : Colors.Blue;
        mezok[x, y].MouseFilter = Control.MouseFilterEnum.Ignore;
        return talalat;
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