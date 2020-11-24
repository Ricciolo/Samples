using CSharp9;
using System;
using System.Threading.Tasks;

// 01. automatic properties
// 02. init set
// 03. record and equality
// 04. positional record
// 05. inheritance with records and equality
// 06. with keyword e custom ctor
// 07. top-level statement
// 08. pattern matching: underscore, property matching, logical operator, is/not, minor/major
// 09. target-typed new expression
// 10. covariant returns


var p1 = new Person { FirstName = "Cristian", LastName = "Civera" };
var p2 = new Company { FirstName = "Cristian", LastName = "Civera", Vat = "123" };
//var p1 = new Person("Cristian", "Civera");
//var p2 = new Company("Cristian", "Civera", "01234");

Person p4 = new();

var p3 = p1 with { FirstName = "Pippo" };

Console.WriteLine(p1.ToString());
Console.WriteLine($"Equals {p1 == p2}");
Console.WriteLine($"GetHashCode {p1.GetHashCode() == p2.GetHashCode()}");
Console.WriteLine($"ReferenceEquals {Object.ReferenceEquals(p1, p2)}");

object n = 3;
string desc = n switch
{
    int => "int", // C# 9
    string s => "string",
    double _ => "double",
    _ => throw new NotImplementedException()
};

switch (p1)
{
    case { FirstName: "a" }:
        Console.WriteLine(p1.FirstName);
        break;
}

if (p1 is ({ FirstName: "a" } or { FirstName: "b" }) and ({ LastName: "a" } or { LastName: "b" }))
{
}

if (p1 is not Company)
{
    // Non è una company
}
if (p2 is not null)
{
    // Non è null
}
if (p1 is not { FirstName: "a" })
{
    // Non contiene FirstName = a
}

if (p1.Address.Length is > 5 and < 10)
{
    // Indirizzo della dimensione dai 5 ai 10 caratteri
}