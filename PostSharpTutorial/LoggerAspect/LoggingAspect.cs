using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using LoggerAspect.Concrete;
using LoggerAspect.Enums;
using LoggerAspect.Extensions;
using LoggerAspect.Interfaces;
using PostSharp.Aspects;

namespace LoggerAspect
{
    /// <summary>
    /// The implementation of the <see cref="OnMethodBoundaryAspect"/>
    /// </summary>
    /// <seealso cref="PostSharp.Aspects.OnMethodBoundaryAspect" />
    [Serializable]
    [LoggingAspect(AttributeExclude = true)]
    public class LoggingAspect : OnMethodBoundaryAspect
    {
        // Logger implementation provided via injection
        private static ILogger _logger;

        // The class name provided at compile time
        private string _className;

        // The method name provided at compile time
        private string _methodName;
        [NonSerialized]
        private Action<string> _entryLogger;
        [NonSerialized]
        private Action<string, Exception> _exceptionLogger;
        [NonSerialized]
        private Action<string> _exitLogger;
        [NonSerialized]
        private Action<string> _resumeLogger;
        [NonSerialized]
        private Action<string> _successLogger;
        [NonSerialized]
        private Action<string> _yieldLogger;

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public static ILogger Logger
        {
            get
            {
                // If no logger implementation is provided then default to a logger which does nothing
                if (_logger == null)
                    _logger = new NullLogger();
                return _logger;
            }
            set { _logger = value; }
        }

        /// <summary>
        /// Gets or sets the exclude flags.
        /// </summary>
        /// <value>
        /// The exclude flags.
        /// </value>
        public ExclusionFlags Exclude { get; set; }

        private Action<string> EntryLogger
        {
            get
            {
                if (_entryLogger == null)
                    _entryLogger = Logger.Debug;
                return _entryLogger;
            }
            set { _entryLogger = value; }
        }

        private Action<string, Exception> ExceptionLogger
        {
            get
            {
                if (_exceptionLogger == null)
                    _exceptionLogger = Logger.Error;
                return _exceptionLogger;
            }
            set { _exceptionLogger = value; }
        }

        private Action<string> ExitLogger
        {
            get
            {
                if (_exitLogger == null)
                    _exitLogger = Logger.Debug;
                return _exitLogger;
            }
            set { _exitLogger = value; }
        }

        private Action<string> ResumeLogger
        {
            get
            {
                if (_resumeLogger == null)
                    _resumeLogger = Logger.Debug;
                return _resumeLogger;
            }
            set { _resumeLogger = value; }
        }

        private Action<string> SuccessLogger
        {
            get
            {
                if (_successLogger == null)
                    _successLogger = Logger.Debug;
                return _successLogger;
            }
            set { _successLogger = value; }
        }

        private Action<string> YieldLogger
        {
            get
            {
                if (_yieldLogger == null)
                    _yieldLogger = Logger.Debug;
                return _yieldLogger;
            }
            set { _yieldLogger = value; }
        }

        /// <summary>
        /// Method invoked at build time to initialize the instance fields of the current aspect. This method is invoked
        /// before any other build-time method.
        /// </summary>
        /// <param name="method">Method to which the current aspect is applied</param>
        /// <param name="aspectInfo">Reserved for future usage.</param>
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            _methodName = method.Name;
            if (method.DeclaringType != null)
                _className = method.DeclaringType.FullName;
        }

        /// <summary>
        /// Method invoked at build time to ensure that the aspect has been applied to the right target.
        /// </summary>
        /// <param name="method">Method to which the aspect has been applied</param>
        /// <returns>
        /// <c>true</c> if the aspect was applied to an acceptable field, otherwise
        /// <c>false</c>.
        /// </returns>
        public override bool CompileTimeValidate(MethodBase method)
        {
            if ((Exclude & ExclusionFlags.StaticConstructor) == ExclusionFlags.StaticConstructor && method.Name.Contains(".cctor"))
                return false;
            if ((Exclude & ExclusionFlags.InstanceConstructors) == ExclusionFlags.InstanceConstructors && method.Name.Contains(".ctor"))
                return false;
            if ((Exclude & ExclusionFlags.PropertyGetters) == ExclusionFlags.PropertyGetters && method.Name.Contains("get_"))
                return false;
            if ((Exclude & ExclusionFlags.PropertySetters) == ExclusionFlags.PropertySetters && method.Name.Contains("set_"))
                return false;
            return !method.Name.Contains("ToString");
        }

        /// <summary>
        /// Method executed <b>before</b> the body of methods to which this aspect is applied.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed, which are its arguments, and how should the execution continue
        /// after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)" />.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override void OnEntry(MethodExecutionArgs args)
        {
            var arguments = GetArguments(args);
            var message = string.Format("Entering method {0}.{1} with ({2})", _className, _methodName,
                FormatAguments(arguments));
            EntryLogger(message);
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// in case that the method resulted with an exception.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            var arguments = GetArguments(args);
            args.FlowBehavior = FlowBehavior.RethrowException;
            var message = string.Format("An exception occured in method {0}.{1} with ({2})", _className, _methodName, FormatAguments(arguments));
            ExceptionLogger(message, args.Exception);
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// even when the method exists with an exception (this method is invoked from
        /// the <c>finally</c> block).
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnExit(MethodExecutionArgs args)
        {
            var arguments = GetArguments(args);
            var message = string.Format("Exiting method {0}.{1} with ({2})", _className, _methodName,
                FormatAguments(arguments));
            ExitLogger(message);
        }

        /// <summary>
        /// Method executed when a state machine resumes execution after an <c>yield return</c> or
        /// <c>await</c> statement.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnResume(MethodExecutionArgs args)
        {
            var arguments = GetArguments(args);
            var message = string.Format("Resuming method {0}.{1} with ({2})", _className, _methodName,
                FormatAguments(arguments));
            ResumeLogger(message);
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// but only when the method successfully returns (i.e. when no exception flies out
        /// the method.).
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnSuccess(MethodExecutionArgs args)
        {
            var arguments = GetArguments(args);
            var returnValue = FormatObject(args.ReturnValue);
            var message = string.Format("Successfully finished method {0}.{1} with ({2}) retuning {3}", _className, _methodName,
                FormatAguments(arguments), returnValue.Equals("NULL") ? "VOID" : returnValue);
            SuccessLogger(message);
        }

        /// <summary>
        /// Method executed when a state machine yields, as the result of an <c>yield return</c> or
        /// <c>await</c> statement.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments. In iterator methods, the <see cref="P:PostSharp.Aspects.MethodExecutionArgs.YieldValue" />
        /// property gives access to the operand of the <c>yield return</c> statement.</param>
        public override void OnYield(MethodExecutionArgs args)
        {
            var arguments = GetArguments(args);
            var message = string.Format("Yielding result from method {0}.{1} with ({2})", _className, _methodName,
                FormatAguments(arguments));
            YieldLogger(message);
        }

        /// <summary>
        /// Sets the on entry log level.
        /// </summary>
        /// <param name="logLevel">The level.</param>
        public void SetOnEntryLevel(LoggingLevel logLevel)
        {
            switch (logLevel)
            {
                case LoggingLevel.Debug:
                    EntryLogger = Logger.Debug;
                    break;
                case LoggingLevel.Info:
                    EntryLogger = Logger.Info;
                    break;
                case LoggingLevel.Trace:
                    EntryLogger = Logger.Trace;
                    break;
                case LoggingLevel.Warn:
                    EntryLogger = Logger.Warn;
                    break;

            }

        }

        /// <summary>
        /// Sets the on exception level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        public void SetOnExceptionLevel(LoggingLevel logLevel)
        {
            switch (logLevel)
            {
                case LoggingLevel.Error:
                    ExceptionLogger = Logger.Error;
                    break;
                case LoggingLevel.Fatal:
                    ExceptionLogger = Logger.Fatal;
                    break;
            }
        }

        /// <summary>
        /// Sets the on exit level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        public void SetOnExitLevel(LoggingLevel logLevel)
        {
            switch (logLevel)
            {
                case LoggingLevel.Debug:
                    ExitLogger = Logger.Debug;
                    break;
                case LoggingLevel.Info:
                    ExitLogger = Logger.Info;
                    break;
                case LoggingLevel.Trace:
                    ExitLogger = Logger.Trace;
                    break;
                case LoggingLevel.Warn:
                    ExitLogger = Logger.Warn;
                    break;

            }
        }

        /// <summary>
        /// Sets the on resume level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        public void SetOnResumeLevel(LoggingLevel logLevel)
        {
            switch (logLevel)
            {
                case LoggingLevel.Debug:
                    ResumeLogger = Logger.Debug;
                    break;
                case LoggingLevel.Info:
                    ResumeLogger = Logger.Info;
                    break;
                case LoggingLevel.Trace:
                    ResumeLogger = Logger.Trace;
                    break;
                case LoggingLevel.Warn:
                    ResumeLogger = Logger.Warn;
                    break;

            }
        }

        /// <summary>
        /// Sets the on success level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        public void SetOnSuccessLevel(LoggingLevel logLevel)
        {
            switch (logLevel)
            {
                case LoggingLevel.Debug:
                    SuccessLogger = Logger.Debug;
                    break;
                case LoggingLevel.Info:
                    SuccessLogger = Logger.Info;
                    break;
                case LoggingLevel.Trace:
                    SuccessLogger = Logger.Trace;
                    break;
                case LoggingLevel.Warn:
                    SuccessLogger = Logger.Warn;
                    break;

            }
        }

        /// <summary>
        /// Sets the on yield level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        public void SetOnYieldLevel(LoggingLevel logLevel)
        {
            switch (logLevel)
            {
                case LoggingLevel.Debug:
                    YieldLogger = Logger.Debug;
                    break;
                case LoggingLevel.Info:
                    YieldLogger = Logger.Info;
                    break;
                case LoggingLevel.Trace:
                    YieldLogger = Logger.Trace;
                    break;
                case LoggingLevel.Warn:
                    YieldLogger = Logger.Warn;
                    break;

            }
        }

        private static string FormatAguments(IDictionary<string, object> arguments)
        {
            var formatedArguments = new List<string>();
            foreach (var argument in arguments)
            {
                if (argument.Value == null)
                {
                    formatedArguments.Add("NULL");
                    continue;
                }
                var type = argument.Value.GetType();
                if (type.IsValueType)
                {
                    formatedArguments.Add(string.Format("{0} = {1}", argument.Key, argument.Value));
                }
                else if (type.IsClass)
                {
                    var formatedObject = FormatObject(argument.Value);
                    formatedArguments.Add(string.Format("{0} = {{ {1} }}", argument.Key, formatedObject));
                }
            }
            var formatedArgumentString = string.Join(" , ", formatedArguments);
            return string.IsNullOrWhiteSpace(formatedArgumentString) ? "NULL" : formatedArgumentString;
        }

        private static string FormatObject(object argument)
        {
            if (argument == null)
                return "NULL";

            string formatedObject;
            var arrDda = GetCustomAttributes(argument.GetType(), typeof(DebuggerDisplayAttribute));
            if (arrDda.Length == 1)
            {
                var dda = arrDda[0] as DebuggerDisplayAttribute;
                if (dda == null)
                    return null;
                var val = dda.Value;
                formatedObject = argument.ToString(val);
            }
            else
            {
                formatedObject = argument.ToString();
            }
            return formatedObject;
        }

        private static IDictionary<string, object> GetArguments(MethodExecutionArgs args)
        {
            var arguments = new Dictionary<string, object>();
            var parameters = args.Method.GetParameters();
            for (var i = 0; i < args.Arguments.Count; i++)
            {
                arguments.Add(parameters[i].Name, args.Arguments[i]);
            }
            return arguments;
        }
    }
}