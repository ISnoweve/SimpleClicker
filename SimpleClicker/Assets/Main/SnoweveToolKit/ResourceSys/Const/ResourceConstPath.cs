using System.Runtime.CompilerServices;

namespace _Main.ResourceSys.Const
{
    public static class ResourceConstPath
    {
        private const string GameSettingPath = "GameSetting/";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetGameSettingDataPath(in string settingName)
        {
            return string.Concat(GameSettingPath, settingName);
        }
    }
}