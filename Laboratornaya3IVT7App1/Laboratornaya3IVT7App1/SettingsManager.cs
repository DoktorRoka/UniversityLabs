using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Laboratornaya3IVT7App1;



namespace Laboratornaya3IVT7App1
{
    public class AppSettings
    {
        public string SaveFolder { get; set; }
        public string CopyrightText { get; set; }
    }

    public static class SettingsManager
    {
        private const string SettingsFilePath = "settings.json";

        public static AppSettings LoadSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                try
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    return JsonConvert.DeserializeObject<AppSettings>(json);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error reading settings: " + ex.Message);
                }
            }
            return new AppSettings()
            {
                SaveFolder = "C:\\DefaultSaveFolder",
                CopyrightText = "© MyCompany"
            };
        }

        public static void SaveSettings(AppSettings settings)
        {
            try
            {
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error saving settings: " + ex.Message);
            }
        }
    }
}
