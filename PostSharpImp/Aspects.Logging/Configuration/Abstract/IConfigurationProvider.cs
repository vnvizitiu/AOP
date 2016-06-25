namespace Aspects.Logging.Configuration.Abstract
{
    using Aspects.Logging.Loggers;

    /// <summary>
    /// The ConfigurationProvider interface.
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary> Determines if it should log .</summary>
        /// <param name="logAttribute"> The log attribute. </param>
        /// <returns> True if it should log </returns>
        bool ShouldLog(LogAttribute logAttribute);

        /// <summary> The get logger. </summary>
        /// <returns> The Logger implementation. </returns>
        ILogger GetLogger();
    }
}