namespace Aspects.Logging
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    using Configuration.Abstract;
    using Configuration.Concrete;
    using Helpers;
    using Loggers;
    using Models;

    using PostSharp.Aspects;

    /// <summary>
    /// The implementation of the <see cref="OnMethodBoundaryAspect"/>
    /// </summary>
    /// <seealso cref="PostSharp.Aspects.OnMethodBoundaryAspect" />
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public sealed class LogAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Logger implementation via injection
        /// </summary>
        private static ILogger logger;

        /// <summary>
        /// The configuration provider.
        /// </summary>
        private static IConfigurationProvider configurationProvider;

        /// <summary>
        /// The a flag indication wether to log or not.
        /// </summary>
        private bool _shouldLog = true;

        /// <summary>
        /// The method name provided at compile time
        /// </summary>
        private string _methodName;

        /// <summary>
        /// The declaring type of the method provided at compile time
        /// </summary>
        private Type _declaringType;

        /// <summary>
        /// The stopwatch instance used for measuring execution time
        /// </summary>
        [NonSerialized]
        private Stopwatch _stopwatch;

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public static ILogger Logger
        {
            private get
            {
                return logger ?? (logger = ConfigurationProvider.GetLogger());
            }

            set
            {
                logger = value;
            }
        }

        /// <summary>
        /// Gets or sets the configuration provider.
        /// </summary>
        /// <value>
        /// The configuration provider.
        /// </value>
        public static IConfigurationProvider ConfigurationProvider
        {
            private get
            {
                return configurationProvider ?? (configurationProvider = new ConfigFileConfigurationProvider());
            }

            set
            {
                configurationProvider = value;
            }
        }

        /// <summary>
        /// Gets or sets the exclude flags.
        /// </summary>
        public Excludes Excludes { get; set; }

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
        /// Gets or sets the output.
        /// </summary>
        /// <value> The output. </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Tags { get; set; }

        /// <summary>
        /// Gets the full name of the current method.
        /// </summary>
        /// <value>
        /// The full name of the current method.
        /// </value>
        public string CurrentMethodFullName
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", _declaringType.FullName, _methodName); }
        }

        /// <summary>
        /// Gets the stopwatch.
        /// </summary>
        /// <value>
        /// The stopwatch.
        /// </value>
        private Stopwatch Stopwatch
        {
            get
            {
                return _stopwatch ?? (_stopwatch = new Stopwatch());
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
            if (method == null) return;
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
            if (method == null) throw new ArgumentNullException("method");

            if (method.Name.Contains("ToString"))
                return false;
            if (typeof(ILogger).IsAssignableFrom(_declaringType))
                return false;
            if (typeof(IConfigurationProvider).IsAssignableFrom(_declaringType))
                return false;
            if (typeof(MessageFormatter).IsAssignableFrom(_declaringType))
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

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// but only when the method successfully returns (i.e. when no exception flies out
        /// the method.).
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        /// <exception cref="System.ArgumentNullException">args</exception>
        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (args == null) throw new ArgumentNullException("args");

            if (!_shouldLog)
                return;

            LoggingInfo info = GetLoggingInfo();

            string message = MessageFormatter.FormatMessage(Output, args, "Success", info);

            Logger.Debug(message);
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// in case that the method resulted with an exception.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        /// <exception cref="System.ArgumentNullException">args</exception>
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

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// even when the method exists with an exception (this method is invoked from
        /// the <c>finally</c> block).
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        /// <exception cref="System.ArgumentNullException">args</exception>
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

        /// <summary>
        /// Initializes the current aspect.
        /// </summary>
        /// <param name="method">Method to which the current aspect is applied.</param>
        public override void RuntimeInitialize(MethodBase method)
        {
            _shouldLog = ConfigurationProvider.ShouldLog(this);
        }

        /// <summary>
        /// Gets the logging information.
        /// </summary>
        /// <returns>Returns a instace of the formating options</returns>
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