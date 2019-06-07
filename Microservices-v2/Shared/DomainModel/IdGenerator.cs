using System;
using System.Collections.Generic;
using System.Text;
using MassTransit;

namespace Industria4.DomainModel
{
    public static class IdGenerator
    {
        /// <summary>
        /// Returns a new id
        /// </summary>
        /// <returns></returns>
        public static string New()
        {
			return NewId.Next().ToString();
        }
    }
}
