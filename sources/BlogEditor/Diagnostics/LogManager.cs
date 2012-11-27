// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Diagnostics
{
    public class LogManager : ILogManager
    {
        public ILogger CreateLogger(string context)
        {
            return new Logger(context);
        }
    }
}