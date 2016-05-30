using System.Configuration;

namespace Aspects.Logging.Configuration
{
    public class TagElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = false, IsRequired = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("includeTag", DefaultValue = false, IsRequired = false)]
        public bool IncludeTag
        {
            get
            {
                bool includeTag = (bool) this["includeTag"];
                if (includeTag && ExcludeTag)
                    throw new ConfigurationErrorsException("Cannot have a tag be include and excluded at the same time");
                return includeTag;
            }
            set
            {
                bool includeTag = value;
                if (includeTag && ExcludeTag)
                    throw new ConfigurationErrorsException("Cannot have a tag be include and excluded at the same time");
                this["includeTag"] = includeTag;
            }
        }

        [ConfigurationProperty("excludeTag", DefaultValue = false, IsRequired = false)]
        public bool ExcludeTag
        {
            get
            {
                bool excludeTag = (bool) this["excludeTag"];
                if (excludeTag && IncludeTag)
                    throw new ConfigurationErrorsException("Cannot have a tag be include and excluded at the same time");
                return excludeTag;
            }
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