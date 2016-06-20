using System;
using Aspects.Logging.Models;
using PostSharp.Aspects;

namespace Aspects.Logging.Helpers
{
    public interface IMessageFormatter
    {
        string FormatMessage(string output, MethodExecutionArgs args, string action, LoggingInfo info);
    }
}