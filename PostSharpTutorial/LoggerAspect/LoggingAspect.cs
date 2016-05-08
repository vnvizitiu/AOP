using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        // The declaring type of the method provided at compile time
        private Type _declaringType;


        // The method name provided at compile time
        private string _methodName;

        [NonSerialized]
        private Stopwatch _stopwatch;

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
        /// Gets or sets a value indicating whether to log the parameters.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the parameters should be logged; otherwise, <c>false</c>.
        /// </value>
        public bool LogParameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to log the execution time of a method.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the execution time should be logged; otherwise, <c>false</c>.
        /// </value>
        public bool LogExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to log the return value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if return value should be logged; otherwise, <c>false</c>.
        /// </value>
        public bool LogReturnValue { get; set; }

        /// <summary>
        /// Gets or sets the exclude flags.
        /// </summary>
        /// <value>
        /// The exclude flags.
        /// </value>
        public ExclusionFlags Exclude { get; set; }

        private Stopwatch Stopwatch
        {
            get
            {
                if (_stopwatch == null)
                    _stopwatch = new Stopwatch();
                return _stopwatch;
            }
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
                _declaringType = method.DeclaringType;
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
            if (method.Name.Contains("ToString"))
                return false;
            if (typeof(ILogger).IsAssignableFrom(_declaringType))
                return false;
            if ((Exclude & ExclusionFlags.StaticConstructor) == ExclusionFlags.StaticConstructor && method.Name.StartsWith(".cctor"))
                return false;
            if ((Exclude & ExclusionFlags.InstanceConstructors) == ExclusionFlags.InstanceConstructors && method.Name.StartsWith(".ctor"))
                return false;
            if ((Exclude & ExclusionFlags.PropertyGetters) == ExclusionFlags.PropertyGetters && method.Name.StartsWith("get_"))
                return false;
            if ((Exclude & ExclusionFlags.PropertySetters) == ExclusionFlags.PropertySetters && method.Name.StartsWith("set_"))
                return false;
            return true;
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
            var message = string.Format("Entering method {0}.{1}", _declaringType.FullName, _methodName);

            if (LogExecutionTime)
            {
                Stopwatch.Start();
            }
            if (LogParameters)
            {
                var arguments = GetArguments(args);
                message = string.Format("{0} with ({1})", message, FormatAguments(arguments));
            }
            Logger.Debug(message);
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// in case that the method resulted with an exception.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.RethrowException;
            var message = string.Format("An exception occured in method {0}.{1}", _declaringType.FullName, _methodName);

            if (LogParameters)
            {
                var arguments = GetArguments(args);
                message = string.Format("{0} with ({1})", message, FormatAguments(arguments));
            }
            Logger.Error(message, args.Exception);
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
            var message = string.Format("Exiting method {0}.{1}", _declaringType.FullName, _methodName);
            if (LogParameters)
            {
                var arguments = GetArguments(args);
                message = string.Format("{0} with ({1})", message, FormatAguments(arguments));
            }
            if (LogExecutionTime)
            {
                message = string.Format("{0} and lasted ({1})", message, Stopwatch.Elapsed);
                Stopwatch.Stop();
            }
            Logger.Debug(message);
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
            var message = string.Format("Successfully finished method {0}.{1}", _declaringType.FullName, _methodName);
            if (LogParameters)
            {
                var arguments = GetArguments(args);
                message = string.Format("{0} with ({1})", message, FormatAguments(arguments));
            }
            if (LogReturnValue)
            {
                var returnValue = FormatObject(args.ReturnValue);
                message = string.Format("{0} returning {1}", message, returnValue.Equals("NULL") ? "VOID" : returnValue);
            }
            Logger.Debug(message);
        }

        /// <summary>
        /// Formats the aguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
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
            var formatedArgumentString = string.Join(" , ", formatedArguments.ToArray());
            return string.IsNullOrWhiteSpace(formatedArgumentString) ? "NULL" : formatedArgumentString;
        }

        /// <summary>
        /// Formats the object.
        /// </summary>
        /// <param name="argument">The argument.</param>
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

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
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
