using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace CyberpunkModsDisabler
{
    internal class ModsManager
    {
        private readonly string _hiddenModsDirName = "HiddenMods";
        private readonly string _modsDirName = "archive/pc/mod";
        private readonly string _patchDirName = "archive/pc/patch";
        private readonly string _pluginsDirName = "bin/x64/plugins";
        private readonly string _scriptsDirName = "r6/scripts";

        public ModsStatus ModsStatus { get; private set; }

        public ModsManager() => UpdateModsStatus();

        public void MoveToOriginal()
        {
            UpdateModsStatus();

            switch(ModsStatus)
            {
                case ModsStatus.Enabled:
                    throw new InvalidOperationException("Mods are already enabled.");
                case ModsStatus.NoMods:
                    throw new InvalidOperationException("There are no mods to move.");
                default:
                    MoveDirectory($"{_hiddenModsDirName}/{_modsDirName}", $"../{_modsDirName}");
                    MoveDirectory($"{_hiddenModsDirName}/{_patchDirName}", $"../{_patchDirName}");
                    MoveDirectory($"{_hiddenModsDirName}/{_pluginsDirName}", $"../{_pluginsDirName}");
                    MoveDirectory($"{_hiddenModsDirName}/{_scriptsDirName}", $"../{_scriptsDirName}");
                    break;
            }
        }

        public void MoveToHidden()
        {
            UpdateModsStatus();

            switch (ModsStatus)
            {
                case ModsStatus.Enabled:
                    throw new InvalidOperationException("Mods are already disabled.");
                case ModsStatus.NoMods:
                    throw new InvalidOperationException("There are no mods to move.");
                default:
                    MoveDirectory($"../{_modsDirName}", $"{_hiddenModsDirName}/{_modsDirName}");
                    MoveDirectory($"../{_patchDirName}", $"{_hiddenModsDirName}/{_patchDirName}");
                    MoveDirectory($"../{_pluginsDirName}", $"{_hiddenModsDirName}/{_pluginsDirName}");
                    MoveDirectory($"../{_scriptsDirName}", $"{_hiddenModsDirName}/{_scriptsDirName}");
                    break;
            }
        }

        private static void MoveDirectory(string source, string destination)
        {
            if(Directory.Exists(source)) Directory.Move(source, destination);
        }

        private void UpdateModsStatus()
        {
            bool emptyHiddenMods = Directory.GetFiles(_hiddenModsDirName).Length == 0;
            bool emptyMods = Directory.GetFiles(_modsDirName).Length == 0
                && Directory.GetFiles(_patchDirName).Length == 0
                && Directory.GetFiles(_pluginsDirName).Length == 0
                && Directory.GetFiles(_scriptsDirName).Length == 0;

            if (emptyHiddenMods && emptyMods) ModsStatus = ModsStatus.Corrupted;
            else if (emptyHiddenMods) ModsStatus = ModsStatus.Enabled;
            else if (emptyMods) ModsStatus = ModsStatus.Disabled;
            else ModsStatus = ModsStatus.NoMods;
        }
    }

    enum ModsStatus
    {
        Enabled,
        Disabled,
        Corrupted,
        NoMods,
    }
}
