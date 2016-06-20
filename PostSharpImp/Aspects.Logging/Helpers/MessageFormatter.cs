using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Aspects.Logging.Models;
using PostSharp.Aspects;

namespace Aspects.Logging.Helpers
{
    public class MessageFormatter : IMessageFormatter
    {
        private const string DefaultSeparator = ";";
        private const string DateTimeElement = "{datetime}";
        private const string ActionElement = "{action}";
        private const string DeclaringTypeElement = "{declaringType}";
        private const string MethodNameElement = "{MethodName}";
        private const string ArgumentsElement = "{Arguments}";
        private const string ReturnValueElement = "{ReturnObject}";
        private const string ElapsedTimeElement = "{ElapsedTime}";

        private static readonly string FullTypeSignature = string.Format(CultureInfo.InvariantCulture, "{0}.{1}({2})", DeclaringTypeElement, MethodNameElement, ArgumentsElement);
        private static readonly string DefaultOutputFormat = string.Join(DefaultSeparator, DateTimeElement, ActionElement, FullTypeSignature, ReturnValueElement, ElapsedTimeElement);

        public string FormatMessage(string output, MethodExecutionArgs args, string action, LoggingInfo info)
        {
            if (args == null) throw new ArgumentNullException("args");
            if (info == null) throw new ArgumentNullException("info");
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Value cannot be null or whitespace.", "action");

            string message = String.IsNullOrWhiteSpace(output) ? DefaultOutputFormat : output;
            message = message.Replace(DateTimeElement, DateTime.Now.ToString("u", CultureInfo.InvariantCulture));
            message = message.Replace(ActionElement, action);
            message = message.Replace(DeclaringTypeElement, info.DeclaringType.FullName);
            message = message.Replace(MethodNameElement, info.MethodName);

            if (info.LogParameters)
            {
                IDictionary<string, object> arguments = GetArguments(args);
                string formatAguments = FormatAguments(arguments);
                string value = formatAguments.Equals("NULL", StringComparison.OrdinalIgnoreCase) ? String.Empty : formatAguments;
                message = message.Replace(ArgumentsElement, value);
            }
            else
            {
                message = message.Replace(ArgumentsElement, String.Empty);
            }

            if (info.LogReturnValue)
            {
                var returnValue = FormatObject(args.ReturnValue);
                message = message.Replace(ReturnValueElement, returnValue);
            }
            else
            {
                message = message.Replace(ReturnValueElement, String.Empty);
            }

            if (info.LogExecutionTime)
            {
                message = message.Replace(ElapsedTimeElement, info.Stopwatch.Elapsed.ToString());
            }
            else
            {
                message = message.Replace(ElapsedTimeElement, String.Empty);
            }

            return message;
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
                    formatedArguments.Add(string.Format(CultureInfo.InvariantCulture, "{0} = {1}", argument.Key, argument.Value));
                }
                else if (type.IsClass)
                {
                    var formatedObject = FormatObject(argument.Value);
                    formatedArguments.Add(string.Format(CultureInfo.InvariantCulture, "{0} = {{ {1} }}", argument.Key, formatedObject));
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
            var arrDda = Attribute.GetCustomAttributes(argument.GetType(), typeof(DebuggerDisplayAttribute)).OfType<DebuggerDisplayAttribute>().ToList();
            if (arrDda.Count == 1)
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
    }
}