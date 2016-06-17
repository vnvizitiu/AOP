using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using Aspects.Logging.Loggers;

namespace Aspects.Logging.Configuration
{
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
        private const string LogAspectSectionName = "LogAspectConfig";

        private static LogAspectConfig _instance;
        private static string _originalConfigPath;
        private LogAspectConfig() { }

        ///<summary>
        ///Get this configuration set from the application's default config file
        ///</summary>
        public static LogAspectConfig Open()
        {
            Assembly assy = Assembly.GetEntryAssembly();
            if (assy != null)
                return Open(assy.Location);
            return null;
        }

        /// <summary>
		/// Get this configuration set from a specific config file
        /// </summary>
        /// <param name="path">The path.</param>
        public static LogAspectConfig Open(string path)
        {
            if (path == null) throw new ArgumentNullException("path");

            if (_instance == null)
            {
                if (path.EndsWith(".config", StringComparison.OrdinalIgnoreCase))
                {
                    _originalConfigPath = path.Remove(path.Length - 7);
                }
                else
                {
                    _originalConfigPath = path;
                }

                System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(_originalConfigPath);

                if (config.Sections[LogAspectSectionName] == null)
                {
                    _instance = new LogAspectConfig();
                    config.Sections.Add(LogAspectSectionName, _instance);
                    config.Save(ConfigurationSaveMode.Modified);
                }
                else
                {
                    _instance = (LogAspectConfig)config.Sections[LogAspectSectionName];
                }
            }
            return _instance;
        }

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

        ///<summary>
        ///Save the current property values to the config file
        ///</summary>
        public void Save()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(_originalConfigPath);
            LogAspectConfig section = Open(_originalConfigPath);

            section.UseConsoleLogger = UseConsoleLogger;
            section.Logger = Logger;

            config.Save(ConfigurationSaveMode.Full);
        }

        /// <summary>
        /// Gets the default configuration without any changes made.
        /// </summary>
        public static LogAspectConfig Default
        {
            get { return new LogAspectConfig(); }
        }

        [ConfigurationProperty("useConsoleLogger", DefaultValue = false, IsRequired = false)]
        public bool UseConsoleLogger
        {
            get { return (bool)this["useConsoleLogger"]; }
            set { this["useConsoleLogger"] = value; }
        }

        [ConfigurationProperty("tags", DefaultValue = null, IsRequired = false, IsDefaultCollection = false)]
        public TagCollection Tags
        {
            get { return (TagCollection)this["tags"]; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        [ConfigurationProperty("logger", IsRequired = false, DefaultValue = null)]
        public string Logger
        {
            get
            {
                string typeName = (string)this["logger"];
                Type loggerType = Type.GetType(typeName);
                if (loggerType != null && typeof(ILogger).IsAssignableFrom(loggerType))
                    return typeName;
                if (loggerType == null)
                    return null;
                throw new ConfigurationErrorsException("The provided logger type is not of type Aspects.Logging.Loggers.ILogger");
            }
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
    }
}