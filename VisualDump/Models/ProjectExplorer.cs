using EnvDTE;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell;
using System.Text.RegularExpressions;

namespace VisualDump.Models
{
    public class ProjectExplorer
    {
        #region Var
        public string Name { get; }
        public Project Project { get; }
        public Languages Language { get; }

        private readonly Lazy<string> _asm;
        public string AssemblyName => _asm.Value;

        private readonly Lazy<XDocument> _proj;
        public XDocument Configuration => _proj.Value;

        private readonly Lazy<IEnumerable<Reference>> _ref;
        public IEnumerable<Reference> References => _ref.Value;
        #endregion

        #region Init
        public ProjectExplorer(Project Project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.Project = Project;
            Name = Project.Name;
            string fileName = string.Empty;
            try
            {
                fileName = Project.FileName; // Will throw if project's unloaded
                Language = RecognizeLanguage(fileName);
                _proj = new Lazy<XDocument>(() => XDocument.Parse(File.ReadAllText(fileName)));
            }
            catch
            {
                Language = Languages.Undefined;
                _proj = new Lazy<XDocument>(() => new XDocument());
            }
            _ref = new Lazy<IEnumerable<Reference>>(() => GetNodes(Configuration).Where(x => x.Name.LocalName == "Reference").Select(x => Reference.TryParse(x.Attribute("Include").Value, out Reference reference) ? reference : null).Where(x => x != null).ToArray());
            _asm = new Lazy<string>(() => {
                // Get AssemblyName from proj-file
                string result = GetNodes(Configuration).FirstOrDefault(x => x.Name.LocalName == "AssemblyName")?.Value;
                // Get AssemblyTitle from AssemblyInfo
                if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(fileName))
                {
                    string asmInfoPath = Directory.EnumerateFiles(Path.GetDirectoryName(fileName), "AssemblyInfo.*", SearchOption.AllDirectories).FirstOrDefault();
                    if (!string.IsNullOrEmpty(asmInfoPath) && File.Exists(asmInfoPath))
                    {
                        string asmInfo = File.ReadAllText(asmInfoPath);
                        Match match = Regex.Match(asmInfo, @"AssemblyTitle\(""(.*)""\)");
                        if (match.Success)
                            result = match.Groups[match.Groups.Count - 1].Value;
                    }
                }
                // If result still empty - return project's name
                return string.IsNullOrEmpty(result) ? Name : result;
            });
        }
        #endregion

        #region Functions
        private static Languages RecognizeLanguage(string ProjFile)
        {
            string ext = Path.GetExtension(ProjFile).ToLower().Replace(".", string.Empty).Replace("proj", string.Empty);
            return Enum.TryParse(ext, true, out Languages lang) ? lang : Languages.Undefined;
        }

        private static IEnumerable<XElement> GetNodes(XDocument Document) => GetNodes(Document.Root);
        private static IEnumerable<XElement> GetNodes(XElement Element)
        {
            yield return Element;
            foreach (XElement x in Element.Nodes().OfType<XElement>())
                foreach (XElement y in GetNodes(x))
                    yield return y;
        }

        public override string ToString() => Name;
        #endregion
    }
}
