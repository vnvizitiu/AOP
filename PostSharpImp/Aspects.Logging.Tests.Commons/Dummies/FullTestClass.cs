using System;
using System.Diagnostics;
using System.Threading;

namespace Aspects.Logging.Tests.Commons.Dummies
{
    [Log(LogParameters = true, LogExecutionTime = true, LogReturnValue = true)]
    [DebuggerDisplay("{Value}")]
    public class FullTestClass
    {
        public string Value { get; set; }

        public void EmbeddedMethods()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            InnerMethod();
        }

        private void InnerMethod()
        {
            Thread.Sleep(TimeSpan.FromSeconds(.5));
        }
    }
}