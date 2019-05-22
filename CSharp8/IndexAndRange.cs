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

            Index i = ^2;
            Range r = 0..2;
        }
    }
}
