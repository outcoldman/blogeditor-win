// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.BlogEditor.Diagnostics
{
    public interface ILogManager
    {
        ILogger CreateLogger(string context);
    }
}