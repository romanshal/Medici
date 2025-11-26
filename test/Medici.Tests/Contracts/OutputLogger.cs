using System;
using System.Collections.Generic;
using System.Text;

namespace Medici.Tests.Contracts
{
    public class OutputLogger
    {
        public IList<string> Messages { get; } = [];
    }
}
