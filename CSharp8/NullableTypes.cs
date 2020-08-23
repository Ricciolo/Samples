//#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Demo
{
    class NullableTypes
    {
        public NullableTypes()
        {
            // Creo un nuovo ordine
            var order = new Order();
            Console.WriteLine($"Order {order.Id.ToUpper()}");

            // Aggiungo una riga
            var item = new OrderItem(12);
            order.Items.Add(item);
            Console.WriteLine(item.GetFormattedAmount()?.ToString());

            // Provo ad ottenere la stringa formattata
            if (item.TryGetFormattedAmount(out string formatted))
            {
                Console.WriteLine(formatted.ToUpper());
            }

            // Accesso a dizionario
            var sampleDictionary = new Dictionary<string, string>();
            if (sampleDictionary.TryGetValue("test", out string? n))
            {
                n.ToString();
            }

            string type = DemoLibrary.Algorithm.Type;
            
            string result = DemoLibrary.Algorithm.Run();
            Console.WriteLine(result.ToUpper());
        }
    }

    public class Order
    {
        public string Id { get; set; } = String.Empty;

        public List<OrderItem> Items { get; } = new List<OrderItem>();
    }

    public class OrderItem
    {
        public OrderItem(int quantity)
        {
            Quantity = quantity;
        }

        public int Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public string? GetFormattedAmount()
        {
            if (!UnitPrice.HasValue) return null;

            return (UnitPrice.Value * Quantity).ToString("C");
        }

        public bool TryGetFormattedAmount(out string value)
        {
            value = null;
            if (!UnitPrice.HasValue) return false;

            value = (UnitPrice.Value * Quantity).ToString("C");

            return true;
        }
    }



    public class Order2
    {
        public string Id { get; set; } = String.Empty;

        public List<OrderItem2> Items { get; } = new List<OrderItem2>();
    }

    public class OrderItem2
    {
        public OrderItem2(int quantity)
        {
            Quantity = quantity;
        }

        public int Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public string? GetFormattedAmount()
        {
            if (!UnitPrice.HasValue) return null;

            return (UnitPrice.Value * Quantity).ToString("C");
        }

        public bool TryGetFormattedAmount([NotNullWhen(true)]out string? value)
        {
            value = null;
            if (!UnitPrice.HasValue) return false;

            value = (UnitPrice.Value * Quantity).ToString("C");

            return true;
        }
    }

}
