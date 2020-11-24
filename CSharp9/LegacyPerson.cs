using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp9
{

    //public record LegacyPerson
    //{
    //    private string _firstName;

    //    public string FirstName {
    //        get => _firstName;
    //        init
    //        {
    //            _firstName = value;
    //        }
    //    }

    //    public string LastName { get; set; }
    //}

    public record LegacyPerson(string FirstName, string LastName);
}
