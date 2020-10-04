namespace AdaptiveRoads.LifeCycle
{
    using System;
    using JetBrains.Annotations;
    using ICities;
    using CitiesHarmony.API;
    using KianCommons;

    public class UserMod : IUserMod
    {
        public static Version ModVersion => typeof(UserMod).Assembly.GetName().Version;
        public static string VersionString => ModVersion.ToString(2);
        public string Name => "Adaptive roads    Roads" + VersionString;
        public string Description => "fundation for roads with extra flexibality and variablity.";

        [UsedImplicitly]
        public void OnEnabled()
        {
            HelpersExtensions.VERBOSE = false;
            HarmonyHelper.EnsureHarmonyInstalled();   
            if (HelpersExtensions.InGame || HelpersExtensions.InAssetEditor)
                LifeCycle.Load();
        }

        [UsedImplicitly]
        public void OnDisabled()
        {
            LifeCycle.Release();
        }

        [UsedImplicitly]
        public void OnSettingsUI(UIHelperBase helper) {
            UI.ModSettings.OnSettingsUI(helper);
        }

    }
}