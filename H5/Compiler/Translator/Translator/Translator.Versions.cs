using H5.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZLogger;

namespace H5.Translator
{
    public partial class Translator
    {
        private string GetProductVersionFromVersionInfo(System.Diagnostics.FileVersionInfo versionInfo)
        {
            string version = null;

            if (versionInfo != null && versionInfo.ProductVersion != null)
            {
                version = versionInfo.ProductVersion.Trim();
            }

            // If version contains only 0 and dots like 0.0.0.0 then set it to default string.Empty
            // This helps get compatibility with Mono when it returns empty (whitespace) when AssemblyVersion is not set
            if (version == null || version.All(x => x == '0' || x == '.'))
            {
                version = Contract.Constants.JS.Types.System.Reflection.Assembly.Config.DEFAULT_VERSION;
            }

            return version;
        }

        System.Diagnostics.FileVersionInfo compilerVersionInfo;
        private System.Diagnostics.FileVersionInfo GetCompilerVersion()
        {
            if (compilerVersionInfo == null)
            {
                try
                {
                    var compilerAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                    compilerVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(compilerAssembly.Location);
                }
                catch (Exception ex)
                {
                    Logger.ZLogError(ex, "Could not load executing assembly to get assembly info");
                }
            }

            return compilerVersionInfo;
        }

        System.Diagnostics.FileVersionInfo assemblyVersionInfo;
        private System.Diagnostics.FileVersionInfo GetAssemblyVersion()
        {
            if (assemblyVersionInfo == null)
            {
                assemblyVersionInfo = GetAssemblyVersionByPath(AssemblyLocation);
            }

            return assemblyVersionInfo;
        }

        System.Diagnostics.FileVersionInfo h5VersionInfo;
        private System.Diagnostics.FileVersionInfo GetH5AssemblyVersion()
        {
            if (h5VersionInfo == null)
            {
                h5VersionInfo = GetAssemblyVersionByPath(H5Location);
            }

            return h5VersionInfo;
        }

        private System.Diagnostics.FileVersionInfo GetAssemblyVersionByPath(string path)
        {
            System.Diagnostics.FileVersionInfo fileVerionInfo = null;
            try
            {
                fileVerionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
            }
            catch (System.Exception ex)
            {
                Logger.ZLogError(ex, "Could not load {0} to get the assembly info", path);
            }

            return fileVerionInfo;
        }

        private VersionContext versionContext;

        public VersionContext GetVersionContext()
        {
            if (versionContext == null)
            {
                versionContext = new VersionContext();

                versionContext.Assembly = GetVersionFromFileVersionInfo(GetAssemblyVersion());
                versionContext.Assembly.Description = GetAssemblyDescription();

                versionContext.H5 = GetVersionFromFileVersionInfo(GetH5AssemblyVersion());

                versionContext.Compiler = GetVersionFromFileVersionInfo(GetCompilerVersion());
            }

            return versionContext;
        }

        private VersionContext.AssemblyVersion GetVersionFromFileVersionInfo(System.Diagnostics.FileVersionInfo versionInfo)
        {
            return versionInfo == null
                    ? new VersionContext.AssemblyVersion()
                    : new VersionContext.AssemblyVersion()
                    {
                        CompanyName = versionInfo.CompanyName?.Trim(),
                        Copyright = versionInfo.LegalCopyright?.Trim(),
                        Version = GetProductVersionFromVersionInfo(versionInfo),
                        Name = versionInfo.ProductName?.Trim()
                    };
        }

        private string GetAssemblyDescription()
        {
            string assemblyDescription = null;

            var assemblyDescriptionAttribute = AssemblyDefinition.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == "System.Reflection.AssemblyDescriptionAttribute");

            if (assemblyDescriptionAttribute != null
                && assemblyDescriptionAttribute.HasConstructorArguments)
            {
                assemblyDescription = assemblyDescriptionAttribute.ConstructorArguments[0].Value as string;
            }

            if (assemblyDescription != null)
            {
                assemblyDescription = assemblyDescription.Trim();
            }

            return assemblyDescription;
        }

        internal string GetAssemblyTitle()
        {
            string assemblyTitle = null;

            var assemblyTitleAttribute = AssemblyDefinition.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == "System.Reflection.AssemblyTitleAttribute");

            if (assemblyTitleAttribute != null
                && assemblyTitleAttribute.HasConstructorArguments)
            {
                assemblyTitle = assemblyTitleAttribute.ConstructorArguments[0].Value as string;
            }

            if (assemblyTitle != null)
            {
                assemblyTitle = assemblyTitle.Trim();
            }

            return assemblyTitle ?? AssemblyDefinition.Name.Name;
        }

        private void LogProductInfo()
        {
            var compilerInfo = GetCompilerVersion();

            var h5Info = GetH5AssemblyVersion();

            Logger.ZLogInformation("Product info:");
            if (compilerInfo != null)
            {
                Logger.ZLogInformation("\t{0} version {1}", compilerInfo.ProductName, compilerInfo.ProductVersion);
            }
            else
            {
                Logger.ZLogInformation("Not found");
            }

            if (h5Info != null)
            {
                Logger.ZLogInformation("\t[{0} Framework, version {1}]", h5Info.ProductName, h5Info.ProductVersion);
            }

            if (compilerInfo != null)
            {
                Logger.ZLogInformation("\t" + compilerInfo.LegalCopyright);
            }
        }
    }
}