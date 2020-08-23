#nullable disable
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo
{
    class Product
    {
        private string _name;
        private string _description;

        public string Name
        {
            get
            {
                if (_name == null) _name = String.Empty;
                
                return _name;
            }
            set => _name = value;
        }

        public string Description
        {
            get => _description ??= String.Empty;
            set => _name = value;
        }


        public string Description2
        {
            get => _description ??= String.Empty;
            set => _name = value;
        }
    }
}
