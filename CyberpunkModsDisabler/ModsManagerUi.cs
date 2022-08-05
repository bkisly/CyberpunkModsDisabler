using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberpunkModsDisabler
{
    internal class ModsManagerUi
    {
        private readonly ModsManager _modsManager;

        public ModsManagerUi() => _modsManager = new();

        public void Init()
        {
            Console.WriteLine("----------\nCYBERPUNK 2077 MODS DISABLER\n----------");
            Console.WriteLine($"Current mods status: {_modsManager.ModsStatus}");

            switch(_modsManager.ModsStatus)
            {
                case ModsStatus.NoMods:
                    CloseMessage();
                    return;
                case ModsStatus.Corrupted:
                    MoveModsToSelectedDest();
                    return;
            }

            Console.WriteLine("Press q to close the program or any other key to move the mods: ");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if(keyInfo.KeyChar != 'q')
            {
                if (_modsManager.ModsStatus == ModsStatus.Enabled) _modsManager.MoveToHidden();
                else if (_modsManager.ModsStatus == ModsStatus.Disabled) _modsManager.MoveToOriginal();

                Console.WriteLine("The operation has been performed successfully.");
                CloseMessage();
            }
        }

        private void MoveModsToSelectedDest()
        {
            Console.WriteLine("This situation happens when there are mods either in the original location and in the hidden folder.");
            Console.WriteLine("Select the action to perform:");
            Console.WriteLine("1 - move mods from the original location to the hidden folder");
            Console.WriteLine("2 - move mods from the hidden folder to the original location");
            Console.WriteLine("any other key - close the program");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.KeyChar)
            {
                case '1':
                    _modsManager.MoveToHidden();
                    break;
                case '2':
                    _modsManager.MoveToOriginal();
                    break;
            }

            Console.WriteLine("The operation has been performed successfully.");
        }

        private static void CloseMessage()
        {
            Console.WriteLine("Press any key to close the program...");
            Console.ReadKey(true);
        }
    }
}
