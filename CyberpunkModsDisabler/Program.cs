namespace CyberpunkModsDisabler;

internal class Program
{
    public static void Main()
    {
        ModsManagerUi modsManagerUi = new();

        try
        {
            modsManagerUi.Init();
        }
        catch(Exception e)
        {
            Console.WriteLine($"An error has occurred while moving the mods: {e.Message}");
        }
    }
}