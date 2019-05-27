using System;
using System.Collections.Generic;
using VS2019.Components;

// TODO: 1 - namespace alignment
namespace VS2019.WrongNamespace
{
    public class Component2 : IComponent
    {
        private int _id;

        // TODO: 2 - unused field
        private string _notUsed;

        public Component2(int id)
        {
            _id = id;
        }

        // TODO: 3 - wrap parameter
        // TODO: 4 - pull to interface
        public void FunctionWithManyParameters(int parameter1, int parameter2, string parameter3, DateTime parameter4)
        {
            var result = new List<char>();
            // TODO: 5 - convert to LINQ
            foreach (char c in parameter3)
            {
                if (Char.IsUpper(c))
                {
                    result.Add(c);
                }
            }
        }
    }
}
