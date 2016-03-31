using System;
using System.IO;
using System.Reflection;
using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Localization
{
    public class Container : WindsorContainer
    {
        private const string ConfigSuffix = ".Container.config";
        private static volatile WindsorContainer m_current;
        private static readonly object m_lock = new object();

        private Container()
        {
            InstallComponents();
        }

        public static WindsorContainer Current
        {
            get
            {
                if (m_current == null)
                {
                    lock (m_lock)
                    {
                        if (m_current == null)
                        {
                            m_current = new Container();
                        }
                    }
                }
                return m_current;
            }
        }

        private void InstallComponents()
        {
            Install(FromAssembly.InThisApplication());

            Install(Configuration.FromXml(GetConfigResource()));
        }

        private static IResource GetConfigResource()
        {
            var assembly = GetAssembly();

            var mainAssemblyPath = Path.GetDirectoryName(new Uri(assembly.EscapedCodeBase).LocalPath);

            if (mainAssemblyPath != null)
            {
                var externalConfigPath = Path.Combine(mainAssemblyPath, GetConfigName(assembly));
                if (File.Exists(externalConfigPath))
                    return new FileResource(externalConfigPath);
            }

            var fileConfigPath = GetFileConfigPath(assembly);
            if (File.Exists(fileConfigPath))
                return new FileResource(fileConfigPath);

            return new AssemblyResource(GetEmbeddedConfigPath(assembly));
        }

        private static string GetFileConfigPath(Assembly assembly)
        {
            var directory = Path.GetDirectoryName(assembly.Location);
            var configName = GetConfigName(assembly);
            return string.Format(@"{0}\{1}", directory, configName);
        }

        private static string GetEmbeddedConfigPath(Assembly assembly)
        {
            var configName = string.Format(@"assembly://{0}/{1}", assembly.GetName().Name, GetConfigName(assembly));
            return configName;
        }

        private static string GetConfigName(Assembly assembly)
        {
            return string.Format(@"{0}{1}", assembly.GetName().Name, ConfigSuffix);
        }


        private static Assembly GetAssembly()
        {
            return Assembly.GetCallingAssembly();
        }
    }
}