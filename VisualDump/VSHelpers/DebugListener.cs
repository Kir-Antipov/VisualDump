using System;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace VisualDump.VSHelpers
{
    public class DebugListener
    {
        #region Var
        private static DebuggerEvents Events { get; set; }
        public static event Action<Project> OnProjectDebuggingStop;
        public static event Action<Project> OnProjectDebuggingStart;

        private static bool Debugging { get; set; }
        private static readonly object _sync = new object();
        private static DateTime LastQuery = DateTime.MinValue;
        #endregion

        #region Functions
        public static void Initialize()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Events = VSPackage.DTE.Events.DebuggerEvents;
            Events.OnEnterRunMode += Events_OnEnterRunMode;
            Events.OnEnterDesignMode += Events_OnEnterDesignMode;
        }

        private static void Events_OnEnterRunMode(dbgEventReason Reason)
        {
            lock (_sync)
            {
                switch (Reason)
                {
                    case dbgEventReason.dbgEventReasonGo:
                    case dbgEventReason.dbgEventReasonAttachProgram:
                    case dbgEventReason.dbgEventReasonLaunchProgram:
                        Project proj = VSPackage.GetActiveProject();
                        if (proj != null && !Debugging && (DateTime.Now - LastQuery).TotalSeconds > 1)
                        {
                            Debugging = true;
                            LastQuery = DateTime.Now;
                            OnProjectDebuggingStart?.Invoke(proj);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void Events_OnEnterDesignMode(dbgEventReason Reason)
        {
            lock (_sync)
            {
                switch (Reason)
                {
                    case dbgEventReason.dbgEventReasonEndProgram:
                    case dbgEventReason.dbgEventReasonLaunchProgram:
                    case dbgEventReason.dbgEventReasonStopDebugging:
                    case dbgEventReason.dbgEventReasonDetachProgram:
                    case dbgEventReason.dbgEventReasonExceptionThrown:
                    case dbgEventReason.dbgEventReasonExceptionNotHandled:
                        Project proj = VSPackage.GetActiveProject();
                        if (proj != null && Debugging && (DateTime.Now - LastQuery).TotalSeconds > 1)
                        {
                            Debugging = false;
                            LastQuery = DateTime.Now;
                            OnProjectDebuggingStop?.Invoke(proj);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}
