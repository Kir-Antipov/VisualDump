using EnvDTE;
using System.Linq;
using VisualDump.VSHelpers;
using Thread = System.Threading.Thread;


namespace VisualDump.Models
{
    public static class NuGetWatcher
    {
        #region Var
        public const string NuGet = "DumpExtensions";
        public const string NuGetVersion = "1.0.0";
        public const string NuGetAPI = "https://api.nuget.org/v3/index.json";
        #endregion

        #region Init
        public static void Initialize() => ProjectListener.OnProjectOpened += ProjectListener_OnProjectOpened;
        #endregion

        #region Functions
        private static void ProjectListener_OnProjectOpened(Project Project)
        {
            ProjectExplorer explorer = new ProjectExplorer(Project);
            if (explorer.Language != Languages.Undefined)
            {
                string packageName = NuGet.ToLower();
                if (!explorer.References.Any(x => x.ToLower().Contains(packageName)))
                    AddReference(Project);
            }
        }

        private static void AddReference(Project Project)
        {
            try
            {
                AddOnlineReferece(Project);
            } 
            catch // Greetings from .NET Core (or there's no internet connection)
            {
                Thread t = new Thread(() => {
                    Thread.Sleep(15000);
                    try
                    {
                        AddOnlineReferece(Project);
                    }
                    catch
                    {
                        try
                        {
                            AddOfflineReferece(Project);
                        }
                        catch
                        {
                            // Sorry, I'm powerless)
                        }
                    }
                });
                t.Start();
            }
        }

        private static void AddOnlineReferece(Project Project) => VSPackage.PackageInstaller.InstallPackage(NuGetAPI, Project, NuGet, NuGetVersion, false);
        private static void AddOfflineReferece(Project Project) => VSPackage.PackageInstaller.InstallPackage(VSPackage.AssemblyPath, Project, NuGet, NuGetVersion, false);
        #endregion
    }
}
