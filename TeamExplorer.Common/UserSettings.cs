using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using System;

namespace TeamExplorer.Common
{
    public static class UserSettings
    {
        private const string CollectionPath = "GitFlowVSWithPR";
        public static IServiceProvider ServiceProvider { get; set; }


        public static string UserId
        {
            get
            {
                SettingsManager settingsManager = new ShellSettingsManager(UserSettings.ServiceProvider);
                var settings = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

                if (!settings.CollectionExists(CollectionPath))
                {
                    settings.CreateCollection(CollectionPath);
                }

                var userId = settings.GetString(CollectionPath, "UserId", null);
                if (String.IsNullOrEmpty(userId))
                {
                    userId = Guid.NewGuid().ToString();
                    settings.SetString(CollectionPath, "UserId", userId);
                }
                return userId;
            }
        }
    }
}