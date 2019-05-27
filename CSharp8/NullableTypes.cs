#nullable disable

using System;
using System.Collections.Generic;
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
            Console.WriteLine(item.GetFormattedAmount());

            string type = DemoLibrary.Algorithm.Type;
            
            string result = DemoLibrary.Algorithm.Run();
            Console.WriteLine(result.ToUpper());
        }
    }

    public class Order
    {
        public string Id { get; set; }

        public List<OrderItem> Items { get; set; }
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
    }

}
