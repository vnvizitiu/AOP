using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Aspects.Logging.Configuration;
using Aspects.Logging.Helpers;
using Aspects.Logging.Loggers;
using Aspects.Logging.Models;
using PostSharp.Aspects;

namespace Aspects.Logging
{
    /// <summary>
    /// The implementation of the <see cref="OnMethodBoundaryAspect"/>
    /// </summary>
    /// <seealso cref="PostSharp.Aspects.OnMethodBoundaryAspect" />
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public sealed class LogAttribute : OnMethodBoundaryAspect
    {
        private bool _shouldLog = true;

        /// The method name provided at compile time
        private string _methodName;

        /// The declaring type of the method provided at compile time
        private Type _declaringType;

        /// Logger implementation via injection
        private static ILogger _logger;

        // The stopwatch instance used for measuring execution time
        [NonSerialized]
        private Stopwatch _stopwatch;

        private IMessageFormatter _messageFormatter;

        /// <summary>
        /// Gets or sets the exclude flags.
        /// </summary>
        public Excludes Excludes { get; set; }


        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    InstantiateLogger();
                }
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
        /// Gets or sets the output fomat.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Method invoked at build time to initialize the instance fields of the current aspect. This method is invoked
        /// before any other build-time method.
        /// </summary>
        /// <param name="method">Method to which the current aspect is applied</param>
        /// <param name="aspectInfo">Reserved for future usage.</param>
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            if (method != null)
            {
                _methodName = method.Name;
                if (method.DeclaringType != null)
                {
                    _declaringType = method.DeclaringType;
                }
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
            if (method == null) throw new ArgumentNullException("method");

            return CheckIfMethodIsValid(method);
        }

        /// <summary>
        /// Method executed <b>before</b> the body of methods to which this aspect is applied.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed, which are its arguments, and how should the execution continue
        /// after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)" />.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (args == null) throw new ArgumentNullException("args");

            if (!_shouldLog)
                return;

            LoggingInfo info = GetLoggingInfo();

            string message = MessageFormatter.FormatMessage(Output, args, "Entered", info);

            if (LogExecutionTime)
            {
                Stopwatch.Start();
            }

            Logger.Debug(message);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (args == null) throw new ArgumentNullException("args");

            if (!_shouldLog)
                return;

            LoggingInfo info = GetLoggingInfo();

            string message = MessageFormatter.FormatMessage(Output, args, "Success", info);

            Logger.Debug(message);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            if (args == null) throw new ArgumentNullException("args");

            if (!_shouldLog)
                return;

            args.FlowBehavior = FlowBehavior.RethrowException;

            LoggingInfo info = GetLoggingInfo();

            string message = MessageFormatter.FormatMessage(Output, args, "Exception", info);

            Logger.Error(message, args.Exception);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            if (args == null) throw new ArgumentNullException("args");

            if (!_shouldLog)
                return;

            LoggingInfo info = GetLoggingInfo();

            string message = MessageFormatter.FormatMessage(Output, args, "Exited", info);

            if (LogExecutionTime)
            {
                Stopwatch.Stop();
            }

            Logger.Debug(message);
        }

        public override void RuntimeInitialize(MethodBase method)
        {
            var config = LogAspectConfig.Open();
            if (config != null)
            {
                var tags = config.Tags.OfType<TagElement>().ToList();
                var includedTags = tags.Where(element => element.IncludeTag).Select(element => element.Name).ToList();
                var excludedTags = tags.Where(element => element.ExcludeTag).Select(element => element.Name).ToList();

                if (includedTags.Any(tag => CurrentMethodFullName.StartsWith(tag, StringComparison.OrdinalIgnoreCase)))
                {
                    _shouldLog = true;
                    return;
                }

                if (excludedTags.Any(tag => CurrentMethodFullName.StartsWith(tag, StringComparison.OrdinalIgnoreCase)))
                {
                    _shouldLog = false;
                    return;
                }

                if (!string.IsNullOrWhiteSpace(Tags))
                {
                    var currentTags = Tags.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(tag => tag.Trim()).ToList();
                    if (currentTags.Any(currentTag => includedTags.Any(includedTag => currentTag.Equals(includedTag, StringComparison.OrdinalIgnoreCase))))
                    {
                        _shouldLog = true;
                        return;
                    }
                    if (currentTags.Any(currentTag => excludedTags.Any(excludedTag => currentTag.Equals(excludedTag, StringComparison.OrdinalIgnoreCase))))
                    {
                        _shouldLog = false;
                    }
                }
            }
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
                    Type type = Type.GetType(config.Logger);
                    if (type != null)
                        _logger = (ILogger)Activator.CreateInstance(type);
                    else
                        _logger = new NullLogger();
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

        private string CurrentMethodFullName
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", _declaringType.FullName, _methodName); }
        }

        private bool CheckIfMethodIsValid(MethodBase method)
        {
            if (method.Name.Contains("ToString"))
                return false;
            if (typeof(ILogger).IsAssignableFrom(_declaringType))
                return false;
            if (method.DeclaringType == GetType())
                return false;
            if ((Excludes & Excludes.StaticConstructor) == Excludes.StaticConstructor && method.Name.StartsWith(".cctor", StringComparison.OrdinalIgnoreCase))
                return false;
            if ((Excludes & Excludes.InstanceConstructors) == Excludes.InstanceConstructors && method.Name.StartsWith(".ctor", StringComparison.OrdinalIgnoreCase))
                return false;
            if ((Excludes & Excludes.PropertyGetters) == Excludes.PropertyGetters && method.Name.StartsWith("get_", StringComparison.OrdinalIgnoreCase))
                return false;
            if ((Excludes & Excludes.PropertySetters) == Excludes.PropertySetters && method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase))
                return false;
            return true;
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

        private IMessageFormatter MessageFormatter
        {
            get
            {
                if (_messageFormatter == null)
                {
                    InitializeMessageFormatter();
                }
                return _messageFormatter;
            }
        }

        private void InitializeMessageFormatter()
        {
            _messageFormatter = new MessageFormatter();
        }

        private LoggingInfo GetLoggingInfo()
        {
            LoggingInfo info = new LoggingInfo
            {
                LogParameters = LogParameters,
                LogReturnValue = LogReturnValue,
                LogExecutionTime = LogExecutionTime,
                MethodName = _methodName,
                DeclaringType = _declaringType,
                Stopwatch = Stopwatch
            };
            return info;
        }
    }
}