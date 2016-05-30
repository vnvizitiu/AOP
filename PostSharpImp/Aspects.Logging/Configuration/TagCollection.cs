using System;
using System.Collections.Generic;
using System.Configuration;

namespace Aspects.Logging.Configuration
{
    /// <summary>
    ///  Configuration section containing the tags which should be included or excluded
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElementCollection" />
    public class TagCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A newly created <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TagElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override object GetElementKey(ConfigurationElement element)
        {
            throw new NotImplementedException();
        }
    }
}