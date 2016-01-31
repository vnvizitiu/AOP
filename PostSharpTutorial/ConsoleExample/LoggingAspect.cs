using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;

namespace ConsoleExample
{
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
        /// Method executed <b>before</b> the body of methods to which this aspect is applied.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed, which are its arguments, and how should the execution continue
        /// after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)" />.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            var message = string.Format("Entering method {0}.{1} with ({2})", _className, _methodName,
                string.Join(",", args.Arguments));
            Logger.Debug(message);
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
            var message = string.Format("Exiting method {0}.{1} with ({2})", _className, _methodName,
                string.Join(",", args.Arguments));
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
            var message = string.Format("Successfully finished method {0}.{1} with ({2})", _className, _methodName,
                string.Join(",", args.Arguments));
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
            var message = string.Format("An exception occured in method {0}.{1} with ({2})", _className, _methodName, string.Join(",", args.Arguments));
            Logger.Error(message, args.Exception);
        }

        /// <summary>
        /// Method executed when a state machine resumes execution after an <c>yield return</c> or
        /// <c>await</c> statement.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnResume(MethodExecutionArgs args)
        {
            var message = string.Format("Resuming method {0}.{1} with ({2})", _className, _methodName,
                string.Join(",", args.Arguments));
            Logger.Debug(message);
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
            var message = string.Format("Yielding result from method {0}.{1} with ({2})", _className, _methodName,
                string.Join(",", args.Arguments));
            Logger.Debug(message);
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
            return !method.Name.Contains("ToString");
        }

    }
}