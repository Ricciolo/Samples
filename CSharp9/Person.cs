using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp9
{
    public record Person
    {
        public string FirstName { get; init; }

        public string LastName { get; init; }


        public virtual Person Validate()
        {
            return this;
        }
    }
}
