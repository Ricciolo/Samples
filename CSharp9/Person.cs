using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp9
{
    //public record Person2(string FirstName, string LastName);

    public record Person
    {
        public string FirstName { get; init; }

        public string LastName { get; init; }
        
        public string Address { get; set; }

        public Person(Person original)
        {

        }


        //public virtual Person Validate()
        //{
        //    return this;
        //}
    }
}
