using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Aspects.Logging.Concrete;
using Aspects.Logging.Configuration;
using Aspects.Logging.Enums;
using Aspects.Logging.Extensions;
using Aspects.Logging.Interfaces;
using PostSharp.Aspects;

namespace Aspects.Logging
{
    /// <summary>
    /// The implementation of the <see cref="OnMethodBoundaryAspect"/>
    /// </summary>
    /// <seealso cref="PostSharp.Aspects.OnMethodBoundaryAspect" />
    [Serializable]
    public class LogAttribute : OnMethodBoundaryAspect
    {
        private const string SeparatorElement = ";";
        private const string DateTimeElement = "{datetime}";
        private const string ActionElement = "{action}";
        private const string DeclaringTypeElement = "{declaringType}";
        private const string MethodNameElement = "{MethodName}";
        private const string ArgumentsElement = "{Arguments}";
        private const string ReturnValueElement = "{ReturnObject}";
        private const string ElapsedTimeElement = "{ElapsedTime}";

        private static readonly string FullTypeSignature =
            string.Format("{0}.{1}({2})", DeclaringTypeElement, MethodNameElement, ArgumentsElement);
        private static readonly string DefaultOutputFormat = string.Join(SeparatorElement, DateTimeElement,ActionElement, FullTypeSignature, ReturnValueElement, ElapsedTimeElement);

        /// The method name provided at compile time
        private string _methodName;

        /// The declaring type of the method provided at compile time
        private Type _declaringType;

        /// Logger implementation via injection
        private static ILogger _logger;

        // The stopwatch instance used for measuring execution time
        [NonSerialized]
        private Stopwatch _stopwatch;

        /// <summary>
        /// Gets or sets the exclude flags.
        /// </summary>
        public ExclusionFlags Exclude { get; set; }


        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public static ILogger Logger
        {
            private get
            {
                if (_logger == null)
                {
                    InstantiateLogger();
                }
                return _logger;
            }
            set { _logger = value; }
        }

        private Stopwatch Stopwatch
        {
            get
            {
                if (_stopwatch == null)
                {
                    _stopwatch = new Stopwatch();
                }
                return _stopwatch;
            }
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
        /// Gets or sets the output fomat.
        /// </summary>
        public string OutputFomat { get; set; }

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
            {
                _declaringType = method.DeclaringType;
            }
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
            if (method.DeclaringType == GetType())
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
        public override void OnEntry(MethodExecutionArgs args)
        {
            string message = FillInMessage(args, "Entered");

            if (LogExecutionTime)
            {
                Stopwatch.Start();
            }

            Logger.Debug(message);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            string message = FillInMessage(args, "Success");
            Logger.Debug(message);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.RethrowException;
            string message = FillInMessage(args, "Exception");
            Logger.Error(message, args.Exception);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            string message = FillInMessage(args, "Exited");

            if (LogExecutionTime)
            {
                Stopwatch.Stop();
            }

            Logger.Debug(message);
        }

        public override void RuntimeInitialize(MethodBase method)
        {
            base.RuntimeInitialize(method);
        }

        private static void InstantiateLogger()
        {
            LogAspectConfig config = LogAspectConfig.Open();
            if (config != null)
            {
                if (!string.IsNullOrWhiteSpace(config.Logger) && config.UseConsoleLogger)
                    throw new ConfigurationErrorsException(
                        "The UseConsoleLogger and the Logger config cannot both be filled in at the same time");
                if (config.UseConsoleLogger)
                {
                    _logger = new ConsoleLogger();
                }
                else if (!string.IsNullOrWhiteSpace(config.Logger))
                {
                    _logger = (ILogger) Activator.CreateInstance(Type.GetType(config.Logger));
                }
                else
                {
                    _logger = new NullLogger();
                }
            }
            else
            {
                _logger = new NullLogger();
            }
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
            var arrDda = GetCustomAttributes(argument.GetType(), typeof(DebuggerDisplayAttribute)).OfType<DebuggerDisplayAttribute>().ToList();
            if (arrDda.Count() == 1)
            {
                var dda = arrDda.First();
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

        private string FillInMessage(MethodExecutionArgs args, string actionName)
        {
            string message = string.IsNullOrWhiteSpace(OutputFomat) ? DefaultOutputFormat : OutputFomat;
            message = message.Replace(DateTimeElement, DateTime.Now.ToString("u"));
            message = message.Replace(ActionElement, actionName);
            message = message.Replace(DeclaringTypeElement, _declaringType.FullName);
            message = message.Replace(MethodNameElement, _methodName);

            if (LogParameters)
            {
                IDictionary<string, object> arguments = GetArguments(args);
                string formatAguments = FormatAguments(arguments);
                string value = formatAguments.Equals("NULL",StringComparison.InvariantCultureIgnoreCase) ? string.Empty : formatAguments;
                message = message.Replace(ArgumentsElement, value);
            }
            else
            {
                message = message.Replace(ArgumentsElement, string.Empty);
            }

            if (LogReturnValue)
            {
                var returnValue = FormatObject(args.ReturnValue);
                message = message.Replace(ReturnValueElement, returnValue);
            }
            else
            {
                message = message.Replace(ReturnValueElement, string.Empty);
            }

            if (LogExecutionTime)
            {
                message = message.Replace(ElapsedTimeElement, Stopwatch.Elapsed.ToString());
            }
            else
            {
                message = message.Replace(ElapsedTimeElement, string.Empty);
            }

            return message;
        }
    }
}