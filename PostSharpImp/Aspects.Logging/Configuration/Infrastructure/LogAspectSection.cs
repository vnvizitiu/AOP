namespace Aspects.Logging.Configuration.Infrastructure
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using Loggers;

    /// <summary>
    ///  Represents the configuration section for the logging aspect
    /// </summary>
    /// <remarks>
    /// See also base object <seealso cref="System.Configuration.ConfigurationSection" />
    /// </remarks>
    /// <remarks>
    /// http://www.codeproject.com/Articles/32490/Custom-Configuration-Sections-for-Lazy-Coders
    /// </remarks>
    public class LogAspectConfig : ConfigurationSection
    {
        /// <summary>
        /// The log aspect section name.
        /// </summary>
        private const string LogAspectSectionName = "LogAspectConfig";

        /// <summary>
        /// The logging aspect config section instance.
        /// </summary>
        private static LogAspectConfig instance;

        /// <summary>
        /// The original config path.
        /// </summary>
        private static string originalConfigPath;

        // ReSharper disable once StyleCop.SA1600
        private LogAspectConfig()
        {
        }

        /// <summary>
        /// Gets the default configuration without any changes made.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static LogAspectConfig Default
        {
            get
            {
                return new LogAspectConfig();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether use console logger.
        /// </summary>
        [ConfigurationProperty("useConsoleLogger", DefaultValue = false, IsRequired = false)]
        public bool UseConsoleLogger
        {
            get
            {
                return (bool)this["useConsoleLogger"];
            }

            // ReSharper disable once MemberCanBePrivate.Global
            set
            {
                this["useConsoleLogger"] = value;
            }
        }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        [ConfigurationProperty("tags", DefaultValue = null, IsRequired = false, IsDefaultCollection = false)]
        public TagCollection Tags
        {
            get
            {
                return (TagCollection)this["tags"];
            }
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>  The logger. </value>
        /// <exception cref="ConfigurationErrorsException"> The provided logger type is not of type Aspects.Logging.Loggers.ILogger </exception>
        [ConfigurationProperty("logger", IsRequired = false, DefaultValue = null)]
        public string Logger
        {
            get
            {
                string typeName = (string)this["logger"];
                Type loggerType = Type.GetType(typeName);
                if (loggerType != null && typeof(ILogger).IsAssignableFrom(loggerType)) return typeName;
                if (loggerType == null) return null;
                throw new ConfigurationErrorsException(
                    "The provided logger type is not of type Aspects.Logging.Loggers.ILogger");
            }

            // ReSharper disable once MemberCanBePrivate.Global
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Type loggerType = Type.GetType(value);
                    if (!typeof(ILogger).IsAssignableFrom(loggerType))
                        throw new ConfigurationErrorsException(
                            "The provided logger type is not of type Aspects.Logging.Loggers.ILogger");
                }

                this["logger"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return (bool)this["isEnabled"];
            }

            // ReSharper disable once UnusedMember.Global
            set
            {
                this["isEnabled"] = value;
            }
        }

        /// <summary>
        /// Get this configuration set from the application's default config file
        /// </summary>
        /// <returns> The <see cref="LogAspectConfig"/>. </returns>
        public static LogAspectConfig Open()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly != null) return Open(assembly.Location);
            return null;
        }

        /// <summary>
        /// Get this configuration set from a specific config file
        /// </summary>
        /// <param name="path"> The path. </param>
        /// <returns> The <see cref="LogAspectConfig"/>. </returns>
        public static LogAspectConfig Open(string path)
        {
            if (path == null) throw new ArgumentNullException("path");

            if (instance == null)
            {
                originalConfigPath = path.EndsWith(".config", StringComparison.OrdinalIgnoreCase)
                                         ? path.Remove(path.Length - 7)
                                         : path;

                Configuration config = ConfigurationManager.OpenExeConfiguration(originalConfigPath);

                if (config.Sections[LogAspectSectionName] == null)
                {
                    instance = new LogAspectConfig();
                    config.Sections.Add(LogAspectSectionName, instance);
                    config.Save(ConfigurationSaveMode.Modified);
                }
                else
                {
                    instance = (LogAspectConfig)config.Sections[LogAspectSectionName];
                }
            }

            return instance;
        }

        /// <summary>
        /// The copy.
        /// </summary>
        /// <returns> The <see cref="LogAspectConfig"/>. </returns>
        // ReSharper disable once UnusedMember.Global
        public LogAspectConfig Copy()
        {
            LogAspectConfig copy = new LogAspectConfig();
            string xml = SerializeSection(this, LogAspectSectionName, ConfigurationSaveMode.Full);
            using (StringReader stringReader = new StringReader(xml))
            {
                XmlReader reader = new XmlTextReader(stringReader);
                copy.DeserializeSection(reader);
                return copy;
            }
        }

        /// <summary>
        /// Save the current property values to the config file
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void Save()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(originalConfigPath);
            LogAspectConfig section = Open(originalConfigPath);

            section.UseConsoleLogger = UseConsoleLogger;
            section.Logger = Logger;

            config.Save(ConfigurationSaveMode.Full);
        }
    }
}