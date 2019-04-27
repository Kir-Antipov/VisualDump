using System;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace VisualDump.VSHelpers
{
    public class ProjectListener
    {
        #region Var
        private static SolutionEvents Events { get; set; }
        public static event Action<Project> OnProjectOpened;
        #endregion

        #region Functions
        public static void Initialize()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Events = VSPackage.DTE.Events.SolutionEvents;
            Events.Opened += Events_Opened;
            Events.ProjectAdded += Events_ProjectAdded;
        }

        private static void Events_ProjectAdded(Project Project) => OnProjectOpened?.Invoke(Project);

        private static void Events_Opened()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Projects projects = VSPackage.DTE.Solution.Projects;
            foreach (Project p in projects)
                OnProjectOpened?.Invoke(p);
        }
        #endregion
    }
}
