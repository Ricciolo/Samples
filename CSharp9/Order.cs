using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp9
{
    public class Order
    {
        private Person _person = new();

        private List<string> Items { get; } = new();

        public Order()
        {
            Init(new());
        }

        private void Init(Address address)
        {
        }
    }

    public class Address
    {

    }
}
