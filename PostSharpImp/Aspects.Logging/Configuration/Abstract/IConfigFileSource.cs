namespace Aspects.Logging.Configuration.Abstract
{
    using Infrastructure;

    /// <summary>
    ///  An interface used to abstract the source of the config file.
    /// </summary>
    internal interface IConfigFileSource
    {
        /// <summary>
        /// Gets the tags.
        /// </summary>
        TagCollection Tags { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        string Logger { get; }

        /// <summary>
        /// Gets a value indicating whether use console logger.
        /// </summary>
        bool UseConsoleLogger { get; }

        /// <summary>
        /// Gets a value indicating whether is enabled.
        /// </summary>
        bool IsEnabled { get; }
    }
}