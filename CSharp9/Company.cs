using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp9
{
    public record Company : Person
    {
        //public Company(string first, string last, string vat) : base(first, last)
        //{
        //    Vat = vat;
        //}

        public string Vat { get; init; }

        //public override Person Validate()
        //{
        //    return this;
        //}
    }
}
