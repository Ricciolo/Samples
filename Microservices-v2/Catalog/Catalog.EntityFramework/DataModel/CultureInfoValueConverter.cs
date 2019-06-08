using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Muuvis.Catalog.EntityFramework.DataModel
{
    class CultureInfoValueConverter : ValueConverter<CultureInfo, string>
    {
        public CultureInfoValueConverter() : base(c => c.ToString(), c => new CultureInfo(c))
        {
        }
    }
}
