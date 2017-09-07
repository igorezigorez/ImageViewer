using System;
using System.IO;
using System.Collections.Generic;
using ImageViewer;

namespace ImageViewer
{
	[Serializable]
	class Settings
	{
		public List<string> DirectoryNames;
		public List<string> Keys;

		public Settings()
		{
			DirectoryNames = new List<string>();
			Keys = new List<string>();
		}
	}

	static class SettingsManager
	{
		public static void RefreshSettings(List<ControlPanel> controlPanels, Settings settings)
		{
			settings.DirectoryNames.Clear();
			settings.Keys.Clear();
			string dnContent = String.Empty;
			string kContent = String.Empty;
			foreach (ControlPanel cp in controlPanels)
			{
				dnContent = cp.DirectoryNameTextBox.Text;
				kContent = cp.KeyTextBox.Text;
				if (dnContent != String.Empty && kContent != String.Empty)
				{
					settings.DirectoryNames.Add(cp.DirectoryNameTextBox.Text);
					settings.Keys.Add(cp.KeyTextBox.Text);
				}
			}
		}

		public static void LoadSettings(List<ControlPanel> controlPanels, Settings settings, AddControlPanelDelegate addCP)
		{
			controlPanels.Clear();

			for(int i = 0; i < settings.Keys.Count; i++)
			{
				addCP();
			}

			for(int i = 0; i < controlPanels.Count; i++)
			{
				controlPanels[i].KeyTextBox.Text = settings.Keys[i];
				controlPanels[i].DirectoryNameTextBox.Text = settings.DirectoryNames[i];
			}
		}
	}
}