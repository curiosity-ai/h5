using H5.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ZLogger;

namespace H5.Translator
{
    public partial class Translator
    {
        private FileVersionInfo _compilerVersionInfo;
        private FileVersionInfo _assemblyVersionInfo;
        private FileVersionInfo _h5VersionInfo;
        private VersionContext _versionContext;

        private string GetProductVersionFromVersionInfo(FileVersionInfo versionInfo)
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

        private FileVersionInfo GetCompilerVersion()
        {
            if (_compilerVersionInfo == null)
            {
                try
                {
                    var compilerAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                    _compilerVersionInfo = FileVersionInfo.GetVersionInfo(compilerAssembly.Location);
                }
                catch (Exception ex)
                {
                    Logger.ZLogError(ex, "Could not load executing assembly to get assembly info");
                }
            }

            return _compilerVersionInfo;
        }
        
        private FileVersionInfo GetAssemblyVersion()
        {
            if (_assemblyVersionInfo == null)
            {
                _assemblyVersionInfo = GetAssemblyVersionByPath(AssemblyLocation);
            }

            return _assemblyVersionInfo;
        }

        private FileVersionInfo GetH5AssemblyVersion()
        {
            if (_h5VersionInfo == null)
            {
                _h5VersionInfo = GetAssemblyVersionByPath(H5Location);
            }

            return _h5VersionInfo;
        }

        private FileVersionInfo GetAssemblyVersionByPath(string path)
        {
            FileVersionInfo fileVerionInfo = null;
            try
            {
                fileVerionInfo = FileVersionInfo.GetVersionInfo(path);
            }
            catch (Exception ex)
            {
                Logger.ZLogError(ex, "Could not load {0} to get the assembly info", path);
            }

            return fileVerionInfo;
        }

        public VersionContext GetVersionContext()
        {
            if (_versionContext == null)
            {
                _versionContext = new VersionContext();

                _versionContext.Assembly = GetVersionFromFileVersionInfo(GetAssemblyVersion());
                _versionContext.Assembly.Description = GetAssemblyDescription();

                _versionContext.H5 = GetVersionFromFileVersionInfo(GetH5AssemblyVersion());

                _versionContext.Compiler = GetVersionFromFileVersionInfo(GetCompilerVersion());
            }

            return _versionContext;
        }

        private VersionContext.AssemblyVersion GetVersionFromFileVersionInfo(FileVersionInfo versionInfo)
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

            if (compilerInfo != null)
            {
                Logger.ZLogInformation("Running compiler version {0}:",compilerInfo.ProductVersion);
            }
        }
    }
}