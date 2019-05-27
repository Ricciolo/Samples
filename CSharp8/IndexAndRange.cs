using System;

namespace Demo
{
    class IndexAndRange
    {
        public IndexAndRange()
        {
            string[] words = new string[]
               {
                    "Forza",
                    "Inter",
                    "Abbasso",
                    "Milan"
               };

            // Abbasso, Milan
            string[] a = words[^2..];

            // Inter, Abbasso
            string[] b = words[^3..^1];

            // Forza, Inter
            string[] c = words[..2];

            // Posizione 2
            Index i1 = 2;
            // Posizione Fine-1
            Index i2 = ^1;

            // Da 1 a 3
            Range r1 = 1..3;
            // Da 0 a 3
            Range r2 = ..2;
            // Da 2 fino alla fine
            Range r3 = 2..;
        }
    }
}
