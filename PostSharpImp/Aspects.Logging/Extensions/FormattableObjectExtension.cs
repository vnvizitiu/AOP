namespace Aspects.Logging.Extensions
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///  An extension method meant to present an object states using a string format
    /// </summary>
    /// <remarks>
    ///  http://haacked.com/archive/2009/01/04/fun-with-named-formats-string-parsing-and-edge-cases.aspx/
    /// </remarks>
    /// <remarks>
    ///  http://www.hanselman.com/blog/ASmarterOrPureEvilToStringWithExtensionMethods.aspx
    /// </remarks>
    public static class FormattableObjectExtension
    {
        /// <summary>
        /// The regex format pattern.
        /// </summary>
        private static readonly Regex RegexFormatPattern = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">instance</exception>
        public static string ToString(this object instance, string format, IFormatProvider formatProvider = null)
        { 
            if (instance == null) throw new ArgumentNullException("instance");

            StringBuilder sb = new StringBuilder();
            Type type = instance.GetType();
            MatchCollection matches = RegexFormatPattern.Matches(format);
            int startIndex = 0;

            foreach (Match match in matches)
            {
                Group propertyNameGroup = match.Groups[2]; // it's second in match between { and }
                int length = propertyNameGroup.Index - startIndex - 1;
                sb.Append(format.Substring(startIndex, length));

                string toGet;
                string toFormat = string.Empty;

                int formatIndex = propertyNameGroup.Value.IndexOf(":", StringComparison.Ordinal); // formatting would be to the right of the colon

                if (formatIndex == -1)
                {
                    // no formatting, no worries
                    toGet = propertyNameGroup.Value;
                }
                else
                {
                    // pickup the formatting
                    toGet = propertyNameGroup.Value.Substring(0, formatIndex);
                    toFormat = propertyNameGroup.Value.Substring(formatIndex + 1);
                }

                // first try properties
                PropertyInfo retrievedPropertyInfo = type.GetProperty(toGet);
                Type retrievedType = null;
                object retrievedObject = null;
                if (retrievedPropertyInfo != null)
                {
                    retrievedType = retrievedPropertyInfo.PropertyType;
                    retrievedObject = retrievedPropertyInfo.GetValue(instance, null);
                }
                else
                {
                    // try fields
                    FieldInfo retrievedField = type.GetField(toGet);
                    if (retrievedField != null)
                    {
                        retrievedType = retrievedField.FieldType;
                        retrievedObject = retrievedField.GetValue(instance);
                    }
                }

                if (retrievedType != null)
                {
                    // Cool, we found something
                    string result;
                    const BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase;
                    if (string.IsNullOrWhiteSpace(toFormat))
                    {
                        // no format info
                        result =
                            retrievedType.InvokeMember(
                                "ToString",
                                BindingFlags,
                                null, 
                                retrievedObject, 
                                null, 
                                CultureInfo.InvariantCulture) as string;
                    }
                    else
                    {
                        // format info
                        result =
                            retrievedType.InvokeMember(
                                "ToString", 
                                BindingFlags, 
                                null, 
                                retrievedObject, 
                                new object[] { toFormat, formatProvider }, 
                                CultureInfo.InvariantCulture) as string;
                    }

                    sb.Append(result);
                }
                else
                {
                    // didn't find a property with that name, so be gracious and put it back
                    sb.Append("{");
                    sb.Append(propertyNameGroup.Value);
                    sb.Append("}");
                }

                startIndex = propertyNameGroup.Index + propertyNameGroup.Length + 1;
            }

            if (startIndex < format.Length)
            {
                // include the rest (end) of the string
                sb.Append(format.Substring(startIndex));
            }

            return sb.ToString();
        }
    }
}