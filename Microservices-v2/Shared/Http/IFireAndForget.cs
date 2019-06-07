using System;
using System.Collections.Generic;
using System.Text;

namespace Industria4.Http
{
    /// <summary>
    /// Allows the control of the operation called
    /// </summary>
    public interface IFireAndForget
    {
        /// <summary>
        /// Gets or sets if the HTTP call must be asynchronous or not
        /// </summary>
        bool Enabled { get; set; }
    }
}
