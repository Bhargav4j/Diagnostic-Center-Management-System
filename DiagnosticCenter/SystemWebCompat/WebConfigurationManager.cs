using System.Configuration;

namespace System.Web.Configuration
{
    public static class WebConfigurationManager
    {
        public static ConnectionStringSettingsCollection ConnectionStrings => 
            ConfigurationManager.ConnectionStrings;

        public static System.Collections.Specialized.NameValueCollection AppSettings => 
            ConfigurationManager.AppSettings;
    }
}
