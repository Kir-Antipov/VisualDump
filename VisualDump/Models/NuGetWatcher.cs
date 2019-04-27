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

        private static void AddOnlineReferece(Project Project) => VSPackage.PackageInstaller.InstallPackage(NuGetAPI, Project, NuGet, NuGetVersion, false);
        private static void AddOfflineReferece(Project Project) => VSPackage.PackageInstaller.InstallPackage(VSPackage.AssemblyPath, Project, NuGet, NuGetVersion, false);
        #endregion
    }
}
