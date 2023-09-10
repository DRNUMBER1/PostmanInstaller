/*
 * Created by SharpDevelop.
 * User: RECIOR
 * Date: 9/8/2023
 * Time: 8:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Microsoft.WindowsAPICodePack.Dialogs;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostmanInstaller
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            LoadData();

			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		public static string Moddir = @"%AppData%\.minecraft\mods\";
        public static string path = Environment.ExpandEnvironmentVariables(Moddir);
		public static List<Release> releasesf; 

        private async void LoadData()
        {
			dir.Text = path;
            var releases = await GetAllReleasesAsync();
			releasesf = releases;

            foreach (Release release in releases)
            {
                comboBox1.Items.Add(release.Name);
            }
            comboBox1.SelectedIndex = 0;
        }
        void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://discord.gg/sYw7x5rHXf");
		}
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{

		}
        void Button1Click(object sender, EventArgs e)
        {

			panel2.Visible = true;
			int sel = comboBox1.SelectedIndex;
			string url = releasesf[sel].Assets[0].BrowserDownloadUrl;
			Console.WriteLine(url);
			DownloadVersion(url, releasesf[sel].Name);

        }

        static async Task<List<Release>> GetAllReleasesAsync()
		{
			var github = new GitHubClient(new ProductHeaderValue("PostmanInstaller"));
			string owner = "srgantmoomoo";
			string repository = "postman";

			try
			{
				IReadOnlyList<Release> releases = await github.Repository.Release.GetAll(owner, repository);
				return releases.ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching releases: {ex.Message}");
				return new List<Release>();
			}
		}

		public async void DownloadVersion(string Adress, string Version) {
            StatusLable.Text = "Downloading...";
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

        private void button2_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = Environment.ExpandEnvironmentVariables("%AppData%");
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = dialog.FileName + "\\";
				dir.Text = path;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/srgantmoomoo/postman");
        }

        private void button3_Click(object sender, EventArgs e)
        {
			Options o = new Options(path);
			o.ShowDialog();
        }
    }
}
