using System;
using Xunit;

namespace Digst.Nemlogin.LookupService.Wsc.Test
{
    public class RunLookupTest
    {
        [Fact]
        public void RunRestSample()
        {
            Rest.Program.Main(Array.Empty<string>());
        }

        [Fact]
        public void RunSoapSample()
        {
            Soap.Program.Main(Array.Empty<string>());
        }
    }
}
