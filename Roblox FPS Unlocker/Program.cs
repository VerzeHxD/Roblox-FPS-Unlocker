using System;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace RobloxFFlagUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine("Starting Roblox FFlag Updater...");
                Console.WriteLine("-------------------------------------------------");
                Thread.Sleep(3000);
                ClearConsole();

                string? newestVersion = GetNewestRobloxVersion();
                if (newestVersion == null)
                {
                    Console.WriteLine("Error: Unable to find the newest Roblox version directory.");
                    return;
                }

                string username = Environment.UserName;
                string clientSettingsDir = GetClientSettingsDirectory(username, newestVersion);
                string filePath = Path.Combine(clientSettingsDir, "ClientAppSettings.json");

                EnsureClientSettingsDirectoryExists(clientSettingsDir);
                string jsonContent = ReadExistingSettings(filePath);

                JObject settingsObject = JObject.Parse(jsonContent);
                int newFFlagValue = GetNewFFlagValue();
                UpdateFFlag(settingsObject, newFFlagValue);

                WriteSettingsToFile(filePath, settingsObject);

                Console.WriteLine("------------------------------------");
                Console.WriteLine("Roblox FFlag Updater Finished Successfully!");
                Console.WriteLine("------------------------------------");
                Thread.Sleep(3000);
                ClearConsole();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Updating FFlag: {ex.Message}");
            }

            Console.ReadLine();
        }

        static void ClearConsole()
        {
            Console.Clear();
        }

        static string? GetNewestRobloxVersion()
        {
            string username = Environment.UserName;
            string versionsDirectory = $@"C:\Users\{username}\AppData\Local\Roblox\Versions";

            DirectoryInfo directoryInfo = new DirectoryInfo(versionsDirectory);
            DirectoryInfo[] versionDirectories = directoryInfo.GetDirectories("version-*", SearchOption.TopDirectoryOnly);

            DirectoryInfo? newestVersionDirectory = versionDirectories
                .OrderByDescending(d => d.Name)
                .FirstOrDefault();

            return newestVersionDirectory?.Name;
        }

        static string GetClientSettingsDirectory(string username, string? version)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            return $@"C:\Users\{username}\AppData\Local\Roblox\Versions\{version}\ClientSettings";
        }

        static void EnsureClientSettingsDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine("------------------------------------");
                Console.WriteLine("ClientSettings Directory Created.");
                Console.WriteLine("------------------------------------");
                Thread.Sleep(3000);
                ClearConsole();
            }
            else
            {
                Console.WriteLine("------------------------------------");
                Console.WriteLine("ClientSettings Directory Already Exists.");
                Console.WriteLine("------------------------------------");
                Thread.Sleep(3000);
                ClearConsole();
            }
        }

        static string ReadExistingSettings(string filePath)
        {
            string jsonContent = "{}";
            if (File.Exists(filePath))
            {
                jsonContent = File.ReadAllText(filePath);
                Console.WriteLine("------------------------------------");
                Console.WriteLine("Existing ClientAppSettings.json Found!");
                Console.WriteLine("------------------------------------");
                Thread.Sleep(3000);
                ClearConsole();
            }
            else
            {
                Console.WriteLine("------------------------------------");
                Console.WriteLine("No Existing ClientAppSettings.json Found. Creating New File...");
                Console.WriteLine("------------------------------------");
                Thread.Sleep(3000);
                ClearConsole();
            }
            return jsonContent;
        }

        static int GetNewFFlagValue()
        {
            int newValue = -1;
            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("Enter The Number Of FPS You Want Your Game To Be Capped At (0-1000): ");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
                if (int.TryParse(Console.ReadLine(), out newValue) && newValue >= 0 && newValue <= 1000)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("-----------------------------------------------------------------------------------------");
                    Console.WriteLine("Invalid input! Please enter a number between 0 And 1000.");
                    Console.WriteLine("-----------------------------------------------------------------------------------------");
                    Thread.Sleep(3000);
                    ClearConsole();
                }
            }
            return newValue;
        }

        static void UpdateFFlag(JObject settingsObject, int newValue)
        {
            settingsObject["DFIntTaskSchedulerTargetFps"] = newValue;
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine($"FFlag Value Updated To {newValue}.");
            Console.WriteLine("----------------------------------------------------------");
            Thread.Sleep(3000);
            ClearConsole();
        }

        static void WriteSettingsToFile(string filePath, JObject settingsObject)
        {
            File.WriteAllText(filePath, settingsObject.ToString());
            Console.WriteLine("------------------------------------");
            Console.WriteLine("ClientAppSettings.json Updated Successfully!");
            Console.WriteLine("------------------------------------");
            Thread.Sleep(3000);
            ClearConsole();
        }
    }
}
