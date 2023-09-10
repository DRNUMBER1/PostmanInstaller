using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostmanInstaller
{
    public partial class Options : Form
    {
        public static string path;
        public Options(string path_)
        {
            InitializeComponent();
            path = path_;
        }

            public async Task DownloadVersion(string Adress, string Version)
            {
            panel2.Visible = true;
                StatusLable.Text = "Downloading... " + Version;
                WebClient webClient = new WebClient();
                Uri url = new Uri(Adress);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                if (Directory.Exists(path))
                {
                    await webClient.DownloadFileTaskAsync(url, path + Version + ".jar");
                }
                else
                {
                    Directory.CreateDirectory(path);
                    await webClient.DownloadFileTaskAsync(url, path + Version + ".jar");
                }
                StatusLable.Text = "Finished!";
            MessageBox.Show(Version + " Successfully downloaded!");
        }
        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            
            await DownloadVersion("https://maven.minecraftforge.net/net/minecraftforge/forge/1.12.2-14.23.5.2860/forge-1.12.2-14.23.5.2860-installer.jar", "forge-1.12.2-14.23.5.2860-installer");
            try
            {
                Process forge = new Process();
                forge.StartInfo.FileName = string.Concat(JavaInstallation(),"bin\\java.exe");
                Console.WriteLine(forge.StartInfo.FileName);
                string p = path + "forge-1.12.2-14.23.5.2860-installer.jar";
                forge.StartInfo.Arguments = "-jar " + p;
                forge.Start();
                forge.WaitForExit();
                FileInfo fi1 = new FileInfo(p);
                fi1.Delete();
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
                Console.WriteLine(ex.Message);
            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
           await DownloadVersion("https://github.com/cabaletta/baritone/releases/download/v1.2.19/baritone-api-forge-1.2.19.jar", "baritone-api-forge-1.2.19");
        }

        private async void button3_Click(object sender, EventArgs e)
        {
           await DownloadVersion("https://github.com/DRNUMBER1/PostmanInstaller/raw/master/OptiFine_1.12.1_HD_U_G5.jar", "OptiFine_1.12.2_HD_U_E3");
        }

        static string JavaInstallation()
        {
            string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrEmpty(environmentPath))
            {
                return environmentPath;
            }

            const string JAVA_KEY = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";

            var localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
            using (var rk = localKey.OpenSubKey(JAVA_KEY))
            {
                if (rk != null)
                {
                    string currentVersion = rk.GetValue("CurrentVersion").ToString();
                    using (var key = rk.OpenSubKey(currentVersion))
                    {
                        return key.GetValue("JavaHome").ToString();
                    }
                }
            }

            localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            using (var rk = localKey.OpenSubKey(JAVA_KEY))
            {
                if (rk != null)
                {
                    string currentVersion = rk.GetValue("CurrentVersion").ToString();
                    using (var key = rk.OpenSubKey(currentVersion))
                    {
                        return key.GetValue("JavaHome").ToString();
                    }
                }
            }

            return null;
        }
    }
}
