using Newtonsoft.Json.Linq;

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

                // Get the newest Roblox version
                string newestVersion = GetNewestRobloxVersion(); // Change return type to non-nullable
                if (newestVersion == null)
                {
                    // Display Error Message If Newest Version Not Found
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine("Error: Unable To Find The Newest Roblox Version Directory, Please Open A Issue About This On My Github");
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    Thread.Sleep(3000);   // Wait For 3 Seconds
                    ClearConsole();                                         // Clear Console Screen
                    return;
                }

                // Get The Username Of The Current Windows User  (To Locate Your "%AppData%")
                string username = Environment.UserName;
                // Gets The Directory Path For Client Settings
                string clientSettingsDir = GetClientSettingsDirectory(username, newestVersion);
                // Construct the file path for client settings
                string filePath = Path.Combine(clientSettingsDir, "ClientAppSettings.json");

                // Ensure That The Client Settings Directory Exists
                EnsureClientSettingsDirectoryExists(clientSettingsDir);
                // Read Existing Settings From The File
                string jsonContent = ReadExistingSettings(filePath);

                // Parse The JSON Content Into A JObject
                JObject settingsObject = JObject.Parse(jsonContent);
                // Get the new FFlag value from user input
                int newFFlagValue = GetNewFFlagValue();
                // Update The FFlag Value In The Settings Object
                UpdateFFlag(settingsObject, newFFlagValue);

                // Write The Updated Settings To The File
                WriteSettingsToFile(filePath, settingsObject);

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

        // Get the newest Roblox version
        static string GetNewestRobloxVersion() // Changed return type to non-nullable
        {
            // Get The Username Of The Current Windows User  (To Locate Your "%AppData%")
            string username = Environment.UserName;
            // Construct The Directory Path For Roblox Versions
            string versionsDirectory = $@"C:\Users\{username}\AppData\Local\Roblox\Versions";

            // Check If The Versions Directory Exists
            if (!Directory.Exists(versionsDirectory))
            {
                // Throw An Exception If The Directory Does Not Exist
                throw new DirectoryNotFoundException($"Versions Directory Not Found: {versionsDirectory}, Please Create A New Issue And Copy And Paste This Into The New Issue Created So I Can Help.");
            }

            // Get The List Of Version Directories
            DirectoryInfo directoryInfo = new DirectoryInfo(versionsDirectory);
            DirectoryInfo[] versionDirectories = directoryInfo.GetDirectories("version-*", SearchOption.TopDirectoryOnly);

            // Throw An Exception If No Version Directories Are Found
            if (versionDirectories.Length == 0)
            {
                throw new InvalidOperationException("No Version Directories Found.");
            }

            // Initialize newestVersion with a non-null value
            string newestVersion = versionDirectories[0].Name;

            // Find The Newest Version Among The Directories
            foreach (DirectoryInfo versionDirectory in versionDirectories)
            {
                string versionName = versionDirectory.Name;
                if (IsVersionNewer(versionName, newestVersion))
                {
                    newestVersion = versionName;
                }
            }

            return newestVersion;
        }

        // Check If One Version Is Newer Than Another
        static bool IsVersionNewer(string version1, string version2)
        {
            // Remove The "version-" Prefix Before Splitting
            version1 = version1.Replace("version-", "");
            version2 = version2.Replace("version-", "");
            
            // Split The Version Strings Into Arrays Of Parts
            string[] version1Parts = version1.Split('.', '-');
            string[] version2Parts = version2.Split('.', '-');
            
            // Compare The Corresponding Parts Of The Version Strings
            for (int i = 0; i < Math.Min(version1Parts.Length, version2Parts.Length); i++)
            {
                if (int.TryParse(version1Parts[i], out int version1Part) &&
                    int.TryParse(version2Parts[i], out int version2Part))
                {
                    if (version1Part > version2Part)
                    {
                        return true;
                    }
                    else if (version1Part < version2Part)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            
            // If We Reach This Point, Versions Are Equal Up To The Shortest Length
            // Longer Version Is Considered Newer
            return version1Parts.Length > version2Parts.Length;
        }

        // Get The Directory Path For Client Settings
        static string GetClientSettingsDirectory(string username, string version)
        {
            return $@"C:\Users\{username}\AppData\Local\Roblox\Versions\{version}\ClientSettings";
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
                Thread.Sleep(3000);          // Wait For 3 Seconds
                ClearConsole();                                                // Clear Console Screen
            }
            else
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("ClientSettings Directory Already Exists.");
                Console.WriteLine("----------------------------------------");
                // Wait for 3 seconds
                Thread.Sleep(3000);        // Wait For 3 Seconds
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
                // Wait for 3 seconds
                Thread.Sleep(3000);        // Wait For 3 Seconds
                ClearConsole();                                              // Clear Console Screen
            }
            else
            {
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine("No Existing ClientAppSettings.json Found. Creating New File...");
                Console.WriteLine("--------------------------------------------------------------");
                Thread.Sleep(3000);          // Wait For 3 Seconds
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
            ClearConsole();
            Console.WriteLine("----------------------------");
            Console.WriteLine($"FFlag Value Updated To {newValue}.");
            Console.WriteLine("----------------------------");
            Thread.Sleep(3000);              // Wait For 3 Seconds
            ClearConsole();                                                   // Clear Console Screen
        }

        // Write the updated settings to the file
        static void WriteSettingsToFile(string filePath, JObject settingsObject)
        {
            File.WriteAllText(filePath, settingsObject.ToString());
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("ClientAppSettings.json Updated Successfully!");
            Console.WriteLine("--------------------------------------------");
            // Wait for 3 seconds
            Thread.Sleep(3000);             // Wait For 3 Seconds
            ClearConsole();                                                   // Clear Console Screen
            Environment.Exit(0);                                           // Exit the application
        }
    }
}
