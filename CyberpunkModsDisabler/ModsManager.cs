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
        private static readonly string[] _originalModPaths = new string[4]
        {
            "../archive/pc/mod",
            "../archive/pc/patch",
            "../bin/x64/plugins",
            "../r6/scripts"
        };

        private static readonly string[] _hiddenModPaths = new string[4]
        {
            "HiddenMods/mod",
            "HiddenMods/patch",
            "HiddenMods/plugins",
            "HiddenMods/scripts"
        };

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
                    foreach ((string original, string hidden) in _originalModPaths.Zip(_hiddenModPaths))
                        MoveDirectory(hidden, original);
                    break;
            }
        }

        public void MoveToHidden()
        {
            UpdateModsStatus();

            switch (ModsStatus)
            {
                case ModsStatus.Disabled:
                    throw new InvalidOperationException("Mods are already disabled.");
                case ModsStatus.NoMods:
                    throw new InvalidOperationException("There are no mods to move.");
                default:
                    foreach ((string original, string hidden) in _originalModPaths.Zip(_hiddenModPaths))
                        MoveDirectory(original, hidden);
                    break;
            }
        }

        private void UpdateModsStatus()
        {
            (bool containsHiddenMods, bool containsOriginalMods) = (false, false);

            foreach((string original, string hidden) in _originalModPaths.Zip(_hiddenModPaths))
            {
                if (Directory.Exists(original))
                    containsOriginalMods = containsOriginalMods || Directory.GetFiles(original).Any();

                if (Directory.Exists(hidden))
                    containsHiddenMods = containsHiddenMods || Directory.GetFiles(hidden).Any();
            }

            if (containsHiddenMods && containsOriginalMods) ModsStatus = ModsStatus.Corrupted;
            else if (containsHiddenMods) ModsStatus = ModsStatus.Disabled;
            else if (containsOriginalMods) ModsStatus = ModsStatus.Enabled;
            else ModsStatus = ModsStatus.NoMods;
        }
        private static void MoveDirectory(string source, string destination)
        {
            if (Directory.Exists(source))
            {
                if (Directory.Exists(destination)) Directory.Delete(destination, true);
                Directory.Move(source, destination);
            }
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
