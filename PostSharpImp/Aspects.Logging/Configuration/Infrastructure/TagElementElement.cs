namespace Aspects.Logging.Configuration.Infrastructure
{
    using System.Configuration;

    /// <summary>
    /// The tag element.
    /// </summary>
    public class TagElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ConfigurationProperty("name", DefaultValue = false, IsRequired = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            // ReSharper disable once UnusedMember.Global
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [include tag].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [include tag]; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="ConfigurationErrorsException"> Cannot have a tag be include and excluded at the same time </exception>
        [ConfigurationProperty("includeTag", DefaultValue = false, IsRequired = false)]
        public bool IncludeTag
        {
            get
            {
                bool includeTag = (bool)this["includeTag"];
                if (includeTag && ExcludeTag) throw new ConfigurationErrorsException("Cannot have a tag be include and excluded at the same time");
                return includeTag;
            }

            // ReSharper disable once UnusedMember.Global
            set
            {
                bool includeTag = value;
                if (includeTag && ExcludeTag)
                    throw new ConfigurationErrorsException("Cannot have a tag be include and excluded at the same time");
                this["includeTag"] = includeTag;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [exclude tag].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [exclude tag]; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="ConfigurationErrorsException"> Cannot have a tag be include and excluded at the same time </exception>
        [ConfigurationProperty("excludeTag", DefaultValue = false, IsRequired = false)]
        public bool ExcludeTag
        {
            get
            {
                bool excludeTag = (bool)this["excludeTag"];
                if (excludeTag && IncludeTag) throw new ConfigurationErrorsException("Cannot have a tag be include and excluded at the same time");
                return excludeTag;
            }

            // ReSharper disable once UnusedMember.Global
            set
            {
                bool excludeTag = value;
                if (excludeTag && IncludeTag)
                    throw new ConfigurationErrorsException("Cannot have a tag be include and excluded at the same time");
                this["excludeTag"] = excludeTag;
            }
        }
    }
}