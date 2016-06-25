namespace Aspects.Logging.Configuration.Concrete
{
    using Abstract;
    using Infrastructure;

    /// <summary>
    /// The config file source.
    /// </summary>
    internal class ConfigFileSource : IConfigFileSource
    {
        /// <summary>
        /// The embedded reference to the config section.
        /// </summary>
        private readonly LogAspectConfig _instance = LogAspectConfig.Open();

        /// <summary>
        /// Gets the tags.
        /// </summary>
        public TagCollection Tags
        {
            get { return _instance.Tags; }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public string Logger
        {
            get { return _instance.Logger; }
        }

        /// <summary>
        /// Gets a value indicating whether use console logger.
        /// </summary>
        public bool UseConsoleLogger
        {
            get { return _instance.UseConsoleLogger; }
        }

        /// <summary>
        /// Gets a value indicating whether is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _instance != null && _instance.IsEnabled;
            }
        }
    }
}