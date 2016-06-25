namespace Aspects.Logging.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Extensions;
    using Models;

    using PostSharp.Aspects;

    /// <summary>
    /// The an utility class used to formate the output message.
    /// </summary>
    internal class MessageFormatter
    {
        /// <summary>
        /// The default separator.
        /// </summary>
        private const string DefaultSeparator = ";";

        /// <summary>
        /// The date time message element.
        /// </summary>
        private const string DateTimeElement = "{datetime}";

        /// <summary>
        /// The action message element.
        /// </summary>
        private const string ActionElement = "{action}";

        /// <summary>
        /// The declaring type message element.
        /// </summary>
        private const string DeclaringTypeElement = "{declaringType}";

        /// <summary>
        /// The method name message element.
        /// </summary>
        private const string MethodNameElement = "{MethodName}";

        /// <summary>
        /// The arguments message element.
        /// </summary>
        private const string ArgumentsElement = "{Arguments}";

        /// <summary>
        /// The return value message element.
        /// </summary>
        private const string ReturnValueElement = "{ReturnObject}";

        /// <summary>
        /// The elapsed time message element.
        /// </summary>
        private const string ElapsedTimeElement = "{ElapsedTime}";

        /// <summary>
        /// The full type signature.
        /// </summary>
        private static readonly string FullTypeSignature = string.Format(CultureInfo.InvariantCulture, "{0}.{1}({2})", DeclaringTypeElement, MethodNameElement, ArgumentsElement);

        /// <summary>
        /// The default output format.
        /// </summary>
        private static readonly string DefaultOutputFormat = string.Join(DefaultSeparator, DateTimeElement, ActionElement, FullTypeSignature, ReturnValueElement, ElapsedTimeElement);

        /// <summary>
        /// Formats the output message.
        /// </summary>
        /// <param name="output">The output pattern.</param>
        /// <param name="args">The method execution arguments.</param>
        /// <param name="action">The logging action.</param>
        /// <param name="info">The logging options.</param>
        /// <returns>Retuns a formate output message</returns>
        /// <exception cref="System.ArgumentNullException"> args or info</exception>
        /// <exception cref="System.ArgumentException">Value for "action" cannot be null or whitespace.</exception>
        public static string FormatMessage(string output, MethodExecutionArgs args, string action, LoggingInfo info)
        {
            if (args == null) throw new ArgumentNullException("args");
            if (info == null) throw new ArgumentNullException("info");
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Value cannot be null or whitespace.", "action");

            string message = string.IsNullOrWhiteSpace(output) ? DefaultOutputFormat : output;
            message = message.Replace(DateTimeElement, DateTime.Now.ToString("u", CultureInfo.InvariantCulture));
            message = message.Replace(ActionElement, action);
            message = message.Replace(DeclaringTypeElement, info.DeclaringType.FullName);
            message = message.Replace(MethodNameElement, info.MethodName);

            if (info.LogParameters)
            {
                IDictionary<string, object> arguments = GetArguments(args);
                string formatArguments = FormatArguments(arguments);
                string value = formatArguments.Equals("NULL", StringComparison.OrdinalIgnoreCase) ? string.Empty : formatArguments;
                message = message.Replace(ArgumentsElement, value);
            }
            else
            {
                message = message.Replace(ArgumentsElement, string.Empty);
            }

            if (info.LogReturnValue)
            {
                string returnValue = FormatObject(args.ReturnValue);
                message = message.Replace(ReturnValueElement, returnValue);
            }
            else
            {
                message = message.Replace(ReturnValueElement, string.Empty);
            }

            string elapsedTimeString = info.LogExecutionTime ? info.Stopwatch.Elapsed.ToString() : string.Empty;
            message = message.Replace(ElapsedTimeElement, elapsedTimeString);

            return message;
        }

        /// <summary>Gets the arguments.</summary>
        /// <param name="args">The method execution arguments.</param>
        /// <returns>A dictionary containing the name of the argument and it's value</returns>
        private static IDictionary<string, object> GetArguments(MethodExecutionArgs args)
        {
            Dictionary<string, object> arguments = new Dictionary<string, object>();
            ParameterInfo[] parameters = args.Method.GetParameters();
            for (int i = 0; i < args.Arguments.Count; i++)
            {
                arguments.Add(parameters[i].Name, args.Arguments[i]);
            }

            return arguments;
        }

        /// <summary>Formats the arguments.</summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The formated arguments.</returns>
        private static string FormatArguments(IDictionary<string, object> arguments)
        {
            List<string> formatedArguments = new List<string>();
            foreach (KeyValuePair<string, object> argument in arguments)
            {
                if (argument.Value == null)
                {
                    formatedArguments.Add("NULL");
                    continue;
                }

                Type type = argument.Value.GetType();
                if (type.IsValueType)
                {
                    formatedArguments.Add(string.Format(CultureInfo.InvariantCulture, "{0} = {1}", argument.Key, argument.Value));
                }
                else if (type.IsClass)
                {
                    string formatedObject = FormatObject(argument.Value);
                    formatedArguments.Add(string.Format(CultureInfo.InvariantCulture, "{0} = {{ {1} }}", argument.Key, formatedObject));
                }
            }

            string formatedArgumentString = string.Join(" , ", formatedArguments.ToArray());
            return string.IsNullOrWhiteSpace(formatedArgumentString) ? "NULL" : formatedArgumentString;
        }

        /// <summary>Formats the object.</summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The objects string form.</returns>
        private static string FormatObject(object argument)
        {
            if (argument == null)
                return "NULL";

            string formatedObject;
            List<DebuggerDisplayAttribute> arrDda = Attribute.GetCustomAttributes(argument.GetType(), typeof(DebuggerDisplayAttribute)).OfType<DebuggerDisplayAttribute>().ToList();
            if (arrDda.Count == 1)
            {
                DebuggerDisplayAttribute dda = arrDda.First();
                string val = dda.Value;
                formatedObject = argument.ToString(val);
            }
            else
            {
                formatedObject = argument.ToString();
            }

            return formatedObject;
        }
    }
}