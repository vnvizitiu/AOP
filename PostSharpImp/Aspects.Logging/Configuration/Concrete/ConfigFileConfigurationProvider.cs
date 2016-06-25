namespace Aspects.Logging.Configuration.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Abstract;
    using Infrastructure;
    using Loggers;

    /// <summary>
    /// The config file configuration provider.
    /// </summary>
    internal class ConfigFileConfigurationProvider : IConfigurationProvider
    {
        /// <summary>
        /// The default separators for splitting the tags
        /// </summary>
        private readonly char[] _separators = { ',', ';', ' ' };

        /// <summary>
        /// The config file source implementation.
        /// </summary>
        private IConfigFileSource _configFileSource;

        /// <summary>
        /// Gets or sets the config file source.
        /// </summary>
        internal IConfigFileSource ConfigFileSource
        {
            private get
            {
                return _configFileSource ?? (_configFileSource = new ConfigFileSource());
            }

            set
            {
                _configFileSource = value;
            }
        }

        /// <summary>
        /// The should log.
        /// </summary>
        /// <param name="logAttribute">
        /// The log attribute.
        /// </param>
        /// <returns>
        /// Whether the logger should be activated or not.
        /// </returns>
        public bool ShouldLog(LogAttribute logAttribute)
        {
            if (!ConfigFileSource.IsEnabled) return false;
            ICollection<TagElement> tags = ConfigFileSource.Tags.OfType<TagElement>().ToList();
            List<string> includedTags =
                tags.Where(element => element.IncludeTag).Select(element => element.Name).ToList();
            List<string> excludedTags =
                tags.Where(element => element.ExcludeTag).Select(element => element.Name).ToList();

            if (includedTags.Any(tag => logAttribute.CurrentMethodFullName.StartsWith(tag, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            if (excludedTags.Any(tag => logAttribute.CurrentMethodFullName.StartsWith(tag, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(logAttribute.Tags))
            {
                ICollection<string> currentTags = logAttribute.Tags.Split(_separators, StringSplitOptions.RemoveEmptyEntries).Select(tag => tag.Trim()).ToList();

                if (currentTags.Any(currentTag => includedTags.Any(includedTag => currentTag.Equals(includedTag, StringComparison.OrdinalIgnoreCase))))
                {
                    return true;
                }

                if (currentTags.Any(currentTag => excludedTags.Any(excludedTag => currentTag.Equals(excludedTag, StringComparison.OrdinalIgnoreCase))))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <exception cref="ConfigurationErrorsException">
        /// The UseConsoleLogger and the Logger config cannot both be filled in at the same time
        /// </exception>
        /// <returns>
        /// The and instance of <see cref="ILogger"/>.
        /// </returns>
        public ILogger GetLogger()
        {
            if (!ConfigFileSource.IsEnabled)
            {
                return new NullLogger();
            }

            if (!string.IsNullOrWhiteSpace(ConfigFileSource.Logger) && ConfigFileSource.UseConsoleLogger)
            {
                throw new ConfigurationErrorsException(
                    "The UseConsoleLogger and the Logger config cannot both be filled in at the same time");
            }

            if (ConfigFileSource.UseConsoleLogger)
            {
                return new ConsoleLogger();
            }

            if (!string.IsNullOrWhiteSpace(ConfigFileSource.Logger))
            {
                Type type = Type.GetType(ConfigFileSource.Logger);
                if (type != null)
                {
                    return (ILogger)Activator.CreateInstance(type);
                }
            }

            throw new ConfigurationErrorsException("Could not find a proper configuration");
        }
    }
}