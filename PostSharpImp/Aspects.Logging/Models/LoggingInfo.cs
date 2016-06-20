using System;
using System.Diagnostics;

namespace Aspects.Logging.Models
{
    public class LoggingInfo
    {
        public bool LogParameters { get; set; }
        public bool LogReturnValue { get; set; }
        public bool LogExecutionTime { get; set; }
        public Type DeclaringType { get; set; }
        public string MethodName { get; set; }
        public Stopwatch Stopwatch { get; set; }
    }
}