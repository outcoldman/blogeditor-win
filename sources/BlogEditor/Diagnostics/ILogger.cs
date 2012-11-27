﻿// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Diagnostics
{
    public interface ILogger
    {
        bool IsInfoEnabled { get; }

        bool IsDebugEnabled { get; }

        bool IsWarningEnabled { get; }

        bool IsErrorEnabled { get; }

        void Info(string message, params object[] parameters);

        void Debug(string message, params object[] parameters);

        void Warning(string message, params object[] parameters);

        void Error(string message, params object[] parameters);
    }
}