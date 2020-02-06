using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public sealed class ConnectionStrings
    {
        public ConnectionStrings(string value) => Value = value;

        public string Value { get; }
    }
}
