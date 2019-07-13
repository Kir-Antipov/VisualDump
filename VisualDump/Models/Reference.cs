using System;
using System.Linq;

namespace VisualDump.Models
{
    public class Reference
    {
        #region Var
        public string Name { get; }
        public Version Version { get; }
        public string Culture { get; }
        public string PublicKeyToken { get; }
        public string ProcessorArchitecture { get; }
        #endregion

        #region Init
        public Reference(string Name, Version Version, string Culture, string PublicKeyToken, string ProcessorArchitecture)
        {
            this.Name = Name;
            this.Version = Version ?? new Version(0, 0, 0, 0);
            this.Culture = Culture;
            this.PublicKeyToken = PublicKeyToken;
            this.ProcessorArchitecture = ProcessorArchitecture;
        }
        public Reference(string Name, string Version, string Culture, string PublicKeyToken, string ProcessorArchitecture) : this(Name, System.Version.TryParse(Version, out var version) ? version : null, Culture, PublicKeyToken, ProcessorArchitecture) { }

        public static bool TryParse(string Include, out Reference Reference)
        {
            string name = null;
            string culture = "neutral";
            string version = "0.0.0.0";
            string publicKeyToken = null;
            string processorArchitecture = null;
            foreach (string part in Include.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
            {
                int equalIndex = part.IndexOf('=');
                if (equalIndex == -1)
                    name = part;
                else
                {
                    string propertyName = part.Substring(0, equalIndex).ToLower();
                    string propertyValue = part.Substring(equalIndex + 1);
                    switch (propertyName)
                    {
                        case "version":
                            version = propertyValue;
                            break;
                        case "culture":
                            culture = propertyValue;
                            break;
                        case "publickeytoken":
                            publicKeyToken = propertyValue;
                            break;
                        case "processorarchitecture":
                            processorArchitecture = propertyValue;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (string.IsNullOrEmpty(name))
            {
                Reference = null;
                return false;
            }
            else
            {
                Reference = new Reference(name, version, culture, publicKeyToken, processorArchitecture);
                return true;
            }
        }
        #endregion

        #region Functions
        public override string ToString() => Name;
        public override int GetHashCode() => Name.GetHashCode();
        public override bool Equals(object obj) => obj is Reference r && r.Name == Name && r.Version == Version && r.Culture == Culture && r.ProcessorArchitecture == ProcessorArchitecture && r.PublicKeyToken == PublicKeyToken;
        #endregion
    }
}
