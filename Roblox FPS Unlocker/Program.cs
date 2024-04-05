using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;

namespace Roblox_FPS_Unlocker
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Display starting message
                Console.WriteLine("--------------------------------");
                Console.WriteLine("Starting Roblox FFlag Updater...");
                Console.WriteLine("--------------------------------");
                Thread.Sleep(3000);       // Wait For 3 Seconds
                ClearConsole();                                             // Clear Console Screen
                
                // Get the new FFlag value from user input
                int newFFlagValue = GetNewFFlagValue();

                // Get all Roblox versions
                string[] versions = GetAllRobloxVersions();
                if (versions.Length == 0)
                {
                    // Display Error Message If No Roblox Versions Found
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine("Error: Unable To Find Any Roblox Version Directory, Please Open A Issue About This On My Github");
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    Thread.Sleep(3000);   // Wait For 3 Seconds
                    ClearConsole();                                         // Clear Console Screen
                    return;
                }

                // Iterate over each version and create ClientSettings folder and ClientAppSettings.json file
                foreach (string version in versions)
                {
                    string username = Environment.UserName;
                    string clientSettingsDir = GetClientSettingsDirectory(username, version);
                    string filePath = Path.Combine(clientSettingsDir, "ClientAppSettings.json");

                    EnsureClientSettingsDirectoryExists(clientSettingsDir);
                    string jsonContent = ReadExistingSettings(filePath);

                    JObject settingsObject = JObject.Parse(jsonContent);
                    UpdateFFlag(settingsObject, newFFlagValue);

                    WriteSettingsToFile(filePath, settingsObject);
                }

                // Display Success Message
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine("Roblox FFlag Updater Finished Successfully!");
                Console.WriteLine("--------------------------------------------------------------------");
                Thread.Sleep(3000);       // Wait For 3 Seconds
                ClearConsole();                                             // Clear Console Screen
            }
            catch (Exception ex)
            {
                // Display error Message If An Exception Occurs
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine($"Error Updating FFlag: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("-------------------------------------------------------");
                Thread.Sleep(3000);       // Wait For 3 Seconds
                ClearConsole();                                             // Clear Console Screen
            }

            Console.ReadLine();                       // Wait For User Input Before Exiting
        }

        static void ClearConsole()
        {
            Console.Clear();
        }

        // Get all Roblox versions
        static string[] GetAllRobloxVersions()
        {
            string username = Environment.UserName;
            string versionsDirectory = $@"C:\Users\{username}\AppData\Local\Roblox\Versions";

            if (!Directory.Exists(versionsDirectory))
            {
                throw new DirectoryNotFoundException($"Versions Directory Not Found: {versionsDirectory}");
            }

            return Directory.GetDirectories(versionsDirectory, "version-*");
        }

        // Get The Directory Path For Client Settings
        static string GetClientSettingsDirectory(string username, string version)
        {
            return Path.Combine(version, "ClientSettings");
        }

        // Ensure That The Client Settings Directory Exists
        static void EnsureClientSettingsDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine("------------------------------------");
                Console.WriteLine("ClientSettings Directory Created.");
                Console.WriteLine("------------------------------------");
                Thread.Sleep(1000);          // Wait For 1 Seconds
                ClearConsole();                                                // Clear Console Screen
            }
            else
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("ClientSettings Directory Already Exists.");
                Console.WriteLine("----------------------------------------");
                Thread.Sleep(1000);        // Wait For 1 Seconds
                ClearConsole();                                             // Clear Console Screen
            }
        }

        // Read existing settings from a file
        static string ReadExistingSettings(string filePath)
        {
            string jsonContent = "{}";
            if (File.Exists(filePath))
            {
                jsonContent = File.ReadAllText(filePath);
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Existing ClientAppSettings.json Found!");
                Console.WriteLine("--------------------------------------");
                Thread.Sleep(1000);        // Wait For 1 Seconds
                ClearConsole();                                              // Clear Console Screen
            }
            else
            {
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine("No Existing ClientAppSettings.json Found. Creating New File...");
                Console.WriteLine("--------------------------------------------------------------");
                Thread.Sleep(2000);          // Wait For 2 Seconds
                ClearConsole();                                                // Clear Console Screen
            }
            return jsonContent;
        }

        // Get the new FFlag value from user input
        static int GetNewFFlagValue()
        {
            int newValue = -1;
            while (true)
            {
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine("Enter The Number Of FPS You Want Your Game To Be Capped At (0-1000): ");
                Console.WriteLine("--------------------------------------------------------------------");
                if (int.TryParse(Console.ReadLine(), out newValue) && newValue >= 0 && newValue <= 1000)
                {
                    Thread.Sleep(2000);      // Wait For 2 Seconds
                    ClearConsole();                                           // Clear Console Screen
                    break; // Break the loop after successful input
                }
                else
                {
                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine("Invalid input! Please enter a number between 0 And 1000.");
                    Console.WriteLine("--------------------------------------------------------");
                    Thread.Sleep(3000);      // Wait For 3 Seconds
                    ClearConsole();                                           // Clear Console Screen
                }
            }
            return newValue;
        }

        // Update the FFlag value in the settings object
        static void UpdateFFlag(JObject settingsObject, int newValue)
        {
            settingsObject["DFIntTaskSchedulerTargetFps"] = newValue;
        }

        // Write the updated settings to the file
        static void WriteSettingsToFile(string filePath, JObject settingsObject)
        {
            try
            {
                File.WriteAllText(filePath, settingsObject.ToString());
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("ClientAppSettings.json Updated Successfully!");
                Console.WriteLine("--------------------------------------------");
                Thread.Sleep(1000);      // Wait For 1 Seconds
                ClearConsole();                                           // Clear Console Screen
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing settings to file: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("--------------------------------------------");
                Thread.Sleep(3000);      // Wait For 3 Seconds
                ClearConsole();                                           // Clear Console Screen
            }
            
        }
    }
}
