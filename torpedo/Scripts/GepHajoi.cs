using Godot;
using System;
using System.Collections.Generic;

public partial class GepHajoi : Node
{
    private const int RacsMeret = 10;

    // A gép hajóinak elfoglalt mezői
    private readonly HashSet<Vector2I> hajok = new();

    private readonly Random random = new();

    // A gép hajói:
    // 5, 4, 3, 3, 2 mező hosszúak
    private readonly int[] hajokHosszai = { 5, 4, 3, 3, 2 };


    public override void _Ready()
    {
        HajokRandomElhelyezese();
    }


    /// <summary>
    /// A gép összes hajóját véletlenszerűen elhelyezi a 10x10-es rácson.
    /// </summary>
    public void HajokRandomElhelyezese()
    {
        hajok.Clear();

        foreach (int hossz in hajokHosszai)
        {
            bool sikerult = false;

            while (!sikerult)
            {
                bool fuggoleges = random.Next(0, 2) == 1;

                int x = random.Next(0, RacsMeret);
                int y = random.Next(0, RacsMeret);

                if (SzabadE(x, y, hossz, fuggoleges))
                {
                    HajoElhelyezese(x, y, hossz, fuggoleges);
                    sikerult = true;
                }
            }
        }

        GD.Print("A gép hajói véletlenszerűen elhelyezve.");
    }


    /// <summary>
    /// Megvizsgálja, hogy a hajó elfér-e az adott pozícióban.
    /// </summary>
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
        }

        return true;
    }


    /// <summary>
    /// A hajó mezőit hozzáadja a gép foglalt mezőihez.
    /// </summary>
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


    /// <summary>
    /// Megvizsgálja, hogy a megadott mezőn van-e gépi hajó.
    /// </summary>
    public bool TalalatE(int x, int y)
    {
        return hajok.Contains(new Vector2I(x, y));
    }


    /// <summary>
    /// Visszaadja, hogy a gép valamelyik hajója elfoglalja-e
    /// a megadott mezőt.
    /// </summary>
    public bool VanEHajo(int x, int y)
    {
        return hajok.Contains(new Vector2I(x, y));
    }
}