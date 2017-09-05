using System;
using System.IO;
using ImageViewer;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ImageViewer
{
	class SettingsManager
	{
		Properties.Settings sett = new Properties.Settings();
		
		

		private List<string> CurrentSettings { get; set; }
		private int ControlRowsCount { get; set; }
		private List<string> FullPaths { get; set; }
		private List<string> DirectoryNames { get; set; }
		private List<string> Keys { get; set; }

		public SettingsManager()
		{
			CurrentSettings = new List<string>();
			ControlRowsCount = 0;
			FullPaths = new List<string>();
			DirectoryNames = new List<string>();
			Keys = new List<string>();
			if (!File.Exists("settings.txt"))
				File.Create("settings.txt");
		}

		public void SaveSettings(Interface settings)
		{
			ControlRowsCount = settings.rows.Count - 1;

			for(int i = 0; i < ControlRowsCount; i++)
			{
				if (settings.textBoxes[i].Name.Contains("KeyTextBox"))
					Keys.Add(settings.textBoxes[i].Text);
				if (settings.textBoxes[i].Name.Contains("FullPathTextBox"))
					FullPaths.Add(settings.textBoxes[i].Text);
				if (settings.textBoxes[i].Name.Contains("DirectoryNameTextBox"))
					DirectoryNames.Add(settings.textBoxes[i].Text);
			}
			BuildSettings();
			WriteSettingsInFile();

		}

		private void BuildSettings()
		{
			string settings;
			for(int i = 0; i < ControlRowsCount; i++)
			{
				settings = String.Empty;
				if (FullPaths[i] == "")
					settings += "none|";
				else
					settings += FullPaths[i] + '|';
				if (DirectoryNames[i] == "")
					settings += "none|";
				else
					settings += DirectoryNames[i] + '|';
				if (Keys[i] == "")
					settings += "none";
				else
					settings += Keys[i] + '|';
				CurrentSettings.Add(settings);
			}

			#region
			//CurrentSettings = String.Empty;
			//CurrentSettings += String.Concat("ControlRowsCount=", ControlRowsCount, '\n');
			//for (int i = 0; i < ControlRowsCount; i++)
			//{
			//	if (FullPaths[i] != "")
			//		CurrentSettings += FullPaths[i] + '\n';
			//	else
			//		CurrentSettings += "none\n";
			//}
			//for (int i = 0; i < ControlRowsCount; i++)
			//{
			//	if (DirectoryNames[i] != "")
			//		CurrentSettings += FullPaths[i] + '\n';
			//	else
			//		CurrentSettings += "none\n";
			//}
			//for (int i = 0; i < ControlRowsCount; i++)
			//{
			//	if (Keys[i] != "")
			//		CurrentSettings += FullPaths[i] + '\n';
			//	else
			//		CurrentSettings += "none\n";
			//}
			//foreach (string path in FullPaths)
			//{
			//	if (path != "")
			//		CurrentSettings += String.Concat(path, '\n');
			//	else
			//		CurrentSettings += String.Concat("none", '\n');
			//}
			//foreach (string directoryName in DirectoryNames)
			//{
			//	if (directoryName != "")
			//		CurrentSettings += String.Concat(directoryName, '\n');
			//	else
			//		CurrentSettings += String.Concat("none", '\n');
			//}
			//foreach (string key in Keys)
			//{
			//	if (key != "")
			//		CurrentSettings += String.Concat(key, '\n');
			//	else
			//		CurrentSettings += String.Concat("none", '\n');
			//}
#endregion
		}

		private void WriteSettingsInFile()
		{
			if (File.Exists("settings.txt"))
				File.Delete("settings.txt");
			File.WriteAllLines("settings.txt", CurrentSettings);
		}
	}
}