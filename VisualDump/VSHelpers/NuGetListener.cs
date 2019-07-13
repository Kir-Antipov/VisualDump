using EnvDTE;
using System;
using System.Linq;
using VisualDump.Models;
using Thread = System.Threading.Thread;

namespace VisualDump.VSHelpers
{
    public static class NuGetListener
    {
        #region Var
        public const string NuGet = "DumpExtensions";
        public const string NuGetVersionString = "2.0.0";
        public static readonly Version NuGetVersion = new Version(NuGetVersionString);
        public const string NuGetAPI = "https://api.nuget.org/v3/index.json";
        #endregion

        #region Init
        public static void Initialize() => ProjectListener.OnProjectOpened += ProjectListener_OnProjectOpened;
        #endregion

        #region Functions
        private static void ProjectListener_OnProjectOpened(Project Project)
        {
            ProjectExplorer explorer = new ProjectExplorer(Project);
            if (explorer.Language != Models.Languages.Undefined)
                if (!explorer.References.Any(x => x.Name == NuGet && x.Version >= NuGetVersion))
                    AddReference(Project);
        }

        private static void AddReference(Project Project)
        {
            try
            {
                AddOnlineReferece(Project);
            } 
            catch // Greetings from .NET Core (or there's no internet connection)
            {
                AddReference(Project, 3000, 0);
            }
        }
        private static void AddReference(Project Project, int Wait, int TryIndex)
        {
            if (TryIndex > 5)
                try
                {
                    AddOfflineReferece(Project);
                } 
                catch
                {
                    // Sorry, I'm powerless)
                }
            else
            {
                new Thread(() => 
                {
                    Thread.Sleep(Wait);
                    try
                    {
                        AddOnlineReferece(Project);
                    } 
                    catch
                    {
                        AddReference(Project, Wait, ++TryIndex);
                    }
                }).Start();
            }
        }

        private static void AddOnlineReferece(Project Project) => VSPackage.PackageInstaller.InstallPackage(NuGetAPI, Project, NuGet, NuGetVersionString, false);
        private static void AddOfflineReferece(Project Project) => VSPackage.PackageInstaller.InstallPackage(VSPackage.AssemblyPath, Project, NuGet, NuGetVersionString, false);
        #endregion
    }
}
