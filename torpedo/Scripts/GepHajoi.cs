using Godot;
using System;
using System.Collections.Generic;

public partial class GepHajoi : Node
{
    // A tábla mérete
    private const int RacsMeret = 10;

    // A gép hajóinak elfoglalt mezői
    private readonly HashSet<Vector2I> hajok = new();

    // Az gép hajóinak eltalált mezői 
    private readonly HashSet<Vector2I> talalatok = new();

    private readonly Random random = new();

    // A gép hajói:
    // 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 mező hosszúak
    private readonly int[] hajokHosszai = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

    // Ellenőrzi, hogy a gépnak van e még hajója
    public bool MindenHajoElsullyedt =>
        hajok.Count > 0 && talalatok.Count == hajok.Count;

    // A gép összes hajóját véletlenszerűen elhelyezi a 10x10-es rácson.
    public void HajokRandomElhelyezese()
    {
        hajok.Clear();
        talalatok.Clear();

        if (!HajokElhelyezese(0))
        {
            hajok.Clear();
            GD.PushError("A gép hajóit nem sikerült elhelyezni.");
            return;
        }

        GD.Print($"A gép hajói elhelyezve. Foglalt mezők: {hajok.Count}");
    }

    // Hajók helyének ellenőrzése 
    private bool HajokElhelyezese(int hajoIndex)
    {
        if (hajoIndex == hajokHosszai.Length)
            return true;

        int hossz = hajokHosszai[hajoIndex];
        var lehetosegek = new List<(int x, int y, bool fuggoleges)>();

        for (int y = 0; y < RacsMeret; y++)
        {
            for (int x = 0; x < RacsMeret; x++)
            {
                lehetosegek.Add((x, y, false));
                lehetosegek.Add((x, y, true));
            }
        }

        for (int i = lehetosegek.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (lehetosegek[i], lehetosegek[j]) = (lehetosegek[j], lehetosegek[i]);
        }

        foreach (var lehetoseg in lehetosegek)
        {
            if (!SzabadE(lehetoseg.x, lehetoseg.y, hossz, lehetoseg.fuggoleges))
                continue;

            HajoElhelyezese(lehetoseg.x, lehetoseg.y, hossz, lehetoseg.fuggoleges);

            if (HajokElhelyezese(hajoIndex + 1))
                return true;

            HajoEltavolitasa(lehetoseg.x, lehetoseg.y, hossz, lehetoseg.fuggoleges);
        }

        return false;
    }



    // Megvizsgálja, hogy a hajó elfér-e az adott pozícióban.

    private bool SzabadE(int x, int y, int hossz, bool fuggoleges)
    {
        for (int i = 0; i < hossz; i++)
        {
            int cellaX = x + (fuggoleges ? 0 : i);
            int cellaY = y + (fuggoleges ? i : 0);

            // Kilógna a rácsból
            if (cellaX < 0 || cellaX >= RacsMeret ||
                cellaY < 0 || cellaY >= RacsMeret)
            {
                return false;
            }

            // Már foglalja egy másik hajó
            if (hajok.Contains(new Vector2I(cellaX, cellaY)))
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
                    if (hajok.Contains(new Vector2I(szomszedX, szomszedY)))
                        return false;
                }
            }
        }

        return true;
    }


    // A hajó mezőit hozzáadja a gép foglalt mezőihez.
    private void HajoElhelyezese(
        int x,
        int y,
        int hossz,
        bool fuggoleges)
    {
        for (int i = 0; i < hossz; i++)
        {
            int cellaX = x + (fuggoleges ? 0 : i);
            int cellaY = y + (fuggoleges ? i : 0);

            hajok.Add(new Vector2I(cellaX, cellaY));
        }
    }

    private void HajoEltavolitasa(int x, int y, int hossz, bool fuggoleges)
    {
        for (int i = 0; i < hossz; i++)
        {
            int cellaX = x + (fuggoleges ? 0 : i);
            int cellaY = y + (fuggoleges ? i : 0);

            hajok.Remove(new Vector2I(cellaX, cellaY));
        }
    }


    // Megvizsgálja, hogy a megadott mezőn van-e gépi hajó.
    public bool TalalatE(int x, int y)
    {
        if (x < 0 || x >= RacsMeret || y < 0 || y >= RacsMeret)
            return false;

        var koordinata = new Vector2I(x, y);
        bool talalat = hajok.Contains(koordinata);
        if (talalat)
            talalatok.Add(koordinata);

        return talalat;
    }


    // Visszaadja, hogy a gép valamelyik hajója elfoglalja-e
    // a megadott mezőt.
    public bool VanEHajo(int x, int y)
    {
        return TalalatE(x, y);
    }
}