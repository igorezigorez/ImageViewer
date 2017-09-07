using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ImageViewer;

namespace ImageViewer
{
	public delegate void AddControlPanelDelegate();

	public partial class MainWindow : Window
	{
		private Image currentImage;
		private List<string> allImagesPath;
		private bool isPictureOpened = false;
		private string currentImagePath;
		private KeyManager keyManager = new KeyManager();
		private Settings settings = new Settings();
		private Interface interfaceSettings = new Interface();
		private List<ControlPanel> controlPanels = new List<ControlPanel>();

		public MainWindow()
		{
			InitializeComponent();
			AddControlPanelDelegate addControlPanel = new AddControlPanelDelegate(AddControlPanel);
			LoadSettingsFromFile();
		}

		private void SaveSettings(object sender)
		{
			BinaryFormatter binFormat = new BinaryFormatter();
			using (Stream fStream = new FileStream("settings.dat", FileMode.Create, FileAccess.Write, FileShare.None))
			{
				binFormat.Serialize(fStream, sender);
			}
		}

		private void LoadSettingsFromFile()
		{
			BinaryFormatter binFormat = new BinaryFormatter();
			Settings settingsFromFile;

			using (Stream fStream = File.OpenRead("settings.dat"))
			{
				if (fStream.Length != 0)
				{
					settingsFromFile = (Settings)binFormat.Deserialize(fStream);
				}
				else
				{
					settingsFromFile = new Settings();
				}
			}

			SettingsManager.LoadSettings(controlPanels, settingsFromFile, AddControlPanel);
		}

		private void AddControlPanel()
		{
			ControlPanel cp = new ControlPanel(controlPanels.Count + 1);
			cp.KeyTextBox.PreviewKeyDown += Bind_KeyDown;
			cp.KeyTextBox.TextChanged += KeyTextBox_TextChanged;
			cp.DeleteButton.Click += DeleteKeyButton_Click;
			cp.DirectoryNameTextBox.TextChanged += TextBox_TextChanged;

			SettingsGrid.RowDefinitions.Add(cp.ControlRow);
			SettingsGrid.Children.Add(cp.DeleteButton);
			SettingsGrid.Children.Add(cp.KeyLabel);
			SettingsGrid.Children.Add(cp.DirectoryNameTextBox);
			SettingsGrid.Children.Add(cp.KeyTextBox);
			SettingsGrid.Children.Add(cp.SubdirectoryLabel);
			controlPanels.Add(cp);
		}

		private void AddControlPanel(int index)
		{
			RowDefinition row = new RowDefinition();
			row.Height = new GridLength(45, GridUnitType.Pixel);
			SettingsGrid.RowDefinitions.Add(row);
			int newIndex = 1;

			ControlPanel cpToDelete = new ControlPanel(controlPanels.Count + 1);

			foreach(ControlPanel cp in controlPanels)
			{
				if (cp.Index != index)
				{
					cp.ChangeIndex(newIndex);
					SettingsGrid.RowDefinitions.Add(cp.ControlRow);
					SettingsGrid.Children.Add(cp.DeleteButton);
					SettingsGrid.Children.Add(cp.KeyLabel);
					SettingsGrid.Children.Add(cp.KeyTextBox);
					SettingsGrid.Children.Add(cp.DirectoryNameTextBox);
					SettingsGrid.Children.Add(cp.SubdirectoryLabel);
					newIndex++;
				}
				else if (cp.Index == index)
				{
					cpToDelete = cp;
				}
			}
			controlPanels.Remove(cpToDelete);
		}

		private void OpenFileButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();

			if (dlg.ShowDialog() == true)
			{
				currentImage = new Image(dlg.FileName);
				Picture.Source = currentImage.Img;
				isPictureOpened = true;

				string fileDirectory = Path.GetDirectoryName(dlg.FileName);
				allImagesPath = Directory.GetFiles(fileDirectory).ToList();

				currentImagePath = dlg.FileName;
			}
		}

		private void ChangePositionsInRows(int index)
		{
			IEnumerable<UIElement> uiButtons = interfaceSettings.buttons.
				Where(element => Convert.ToInt32(element.Name.Remove(0, element.Name.Length - 1)) > index).
				Select(element => element);
			IEnumerable<UIElement> uiLabels = interfaceSettings.labels.
				Where(element => Convert.ToInt32(element.Name.Remove(0, element.Name.Length - 1)) > index).
				Select(element => element);
			IEnumerable<UIElement> uiTextBoxes = interfaceSettings.textBoxes.
				Where(element => Convert.ToInt32(element.Name.Remove(0, element.Name.Length - 1)) > index).
				Select(element => element);

			foreach (UIElement button in uiButtons)
				Grid.SetRow(button, Grid.GetRow(button) - 1);
			foreach (UIElement label in uiLabels)
				Grid.SetRow(label, Grid.GetRow(label) - 1);
			foreach (UIElement textBox in uiTextBoxes)
				Grid.SetRow(textBox, Grid.GetRow(textBox) - 1);
		}

		private void DeleteKeyButton_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;
			int index = Convert.ToInt32(btn.Name.Remove(0, btn.Name.Length - 1));
			SettingsGrid.Children.Clear();
			SettingsGrid.RowDefinitions.Clear();
			AddControlPanel(index);
			SettingsManager.RefreshSettings(controlPanels, settings);
			SaveSettings(settings);
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			PictureGrid.Margin = new Thickness(0, 20, 300, 0);
			SettingsGrid.Width = 340;
			SettingsScrollViewer.Width = 360;
			ControlPanelButtons.Width = 320;
			SettingsManager.RefreshSettings(controlPanels, settings);
			SaveSettings(settings);
		}

		private void MoveImage(string imagePath, string directoryPath)
		{
			if (Picture.Source == null)
				return;

			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			if (!File.Exists(Path.Combine(directoryPath, currentImage.Name)))
			{
				File.Copy(currentImage.FullPath, Path.Combine(directoryPath, currentImage.Name));
			}
			if (MoveRadioButton.IsChecked == true)
			{
				currentImagePath = currentImage.FullPath;
				allImagesPath.Remove(currentImagePath);
				ShowNextImage();
				File.Delete(currentImagePath);
			}

		}

		private void Bind_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox txtBox = (TextBox)sender;

			if (txtBox.Text == "")
			{
				if (!keyManager.IsKeyBusy(e.Key.ToString()))
				{
					txtBox.Text = e.Key.ToString();
					keyManager.AddKey(e.Key.ToString());
				}
			}
			else
			{
				if (!keyManager.IsKeyBusy(e.Key.ToString()))
				{
					keyManager.RemoveKey(txtBox.Text);
					keyManager.AddKey(e.Key.ToString());
					txtBox.Text = e.Key.ToString();
				}
			}
		}

		private void MainGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Right || e.Key == Key.Space)
				ShowNextImage();
			if (e.Key == Key.Left || e.Key == Key.Back)
				ShowPreviousImage();
			if (keyManager.IsKeyBusy(e.Key.ToString()))
			{
				int index = GetKeyIndex(e.Key.ToString());
				string directoryName = GetTextBoxText(index);
				if (currentImage != null)
				{
					string directoryPath = Path.Combine(Path.GetDirectoryName(currentImage.FullPath), directoryName);
					if (!Directory.Exists(directoryPath))
						Directory.CreateDirectory(directoryPath);
					MoveImage(currentImage.FullPath, directoryPath);
				}
			}

			SettingsManager.RefreshSettings(controlPanels, settings);
			SaveSettings(settings);
		}

		private void KeyTextBox_TextChanged(object sender, RoutedEventArgs e)
		{
			TextBox txtBox = (TextBox)sender;
			if(!keyManager.IsKeyBusy(txtBox.Text))
			keyManager.AddKey(txtBox.Text);
		}

		private void TextBox_TextChanged(object sender, RoutedEventArgs e)
		{
			TextBox txtBox = (TextBox)sender;
			int index = Convert.ToInt32(txtBox.Name.Remove(0, txtBox.Name.Length - 1));
			string keyTextBoxContent = String.Empty;

			foreach (ControlPanel cp in controlPanels)
			{
				if (cp.Index == index 
					&& cp.KeyTextBox.Text == String.Empty 
					&& txtBox.Text.Length == 1
					&& !keyManager.IsKeyBusy(txtBox.Text))
				{
					cp.KeyTextBox.Text = txtBox.Text;
				}
			}
		}

		private string GetTextBoxText(int index)
		{
			foreach(ControlPanel cp in controlPanels)
			{
				if (cp.Index == index)
					return cp.DirectoryNameTextBox.Text;
			}
			return String.Empty;
		}

		private string CreateDirectoryPath(string fullPath, string directoryName)
		{
			if (fullPath != "")
			{
				return fullPath;
			}

			if(currentImage != null)
				fullPath = Path.Combine(Path.GetDirectoryName(currentImage.FullPath), directoryName);
			if (!Directory.Exists(fullPath))
				Directory.CreateDirectory(fullPath);
			return fullPath;
		}

		private TextBox GetTextBox(string index, string name)
		{
			IEnumerable<TextBox> textBoxes = interfaceSettings.textBoxes.
				Where(txtBox => txtBox.Name == String.Concat(name, index)).
				Select(txtBox => txtBox);
			foreach (TextBox txtBox in textBoxes)
			{
				return txtBox;
			}
			return null;
		}

		private int GetKeyIndex(string key)
		{
			foreach(ControlPanel cp in controlPanels)
			{
				if (cp.KeyTextBox.Text == key)
					return cp.Index;
			}
			return 0;
		}

		private void ShowNextImage()
		{
			if (isPictureOpened == false)
				return;

			string currentImagePath = currentImage.FullPath;
			//int currentImageIndex = Array.IndexOf(allImagesPath, currentImagePath);
			int currentImageIndex = allImagesPath.IndexOf(currentImagePath);
			if (currentImageIndex != allImagesPath.Count() - 1)
				currentImageIndex++;
			else
				currentImageIndex = 0;
			if (allImagesPath.Count != 0)
			{
				currentImage = new Image(allImagesPath[currentImageIndex]);
				Picture.Source = currentImage.Img;
				currentImagePath = currentImage.FullPath;
			}
			else
			{
				Picture.Source = null;
			}
		}

		private void ShowPreviousImage()
		{
			if (isPictureOpened == false)
				return;

			string currentImagePath = currentImage.FullPath;
			//int currentImageIndex = Array.IndexOf(allImagesPath, currentImagePath);
			int currentImageIndex = allImagesPath.IndexOf(currentImagePath);
			if (currentImageIndex != 0)
				currentImageIndex--;
			else
				currentImageIndex = allImagesPath.Count() - 1;

			if (allImagesPath.Count != 0)
			{
				currentImage = new Image(allImagesPath[currentImageIndex]);
				Picture.Source = currentImage.Img;
				currentImagePath = currentImage.FullPath;
			}
			else
				Picture.Source = null;
		}

		private void CloseSettingsButton_Click(object sender, RoutedEventArgs e)
		{
			PictureGrid.Margin = new Thickness(0, 20, 0, 0);
			SettingsGrid.Width = 0;
			SettingsScrollViewer.Width = 0;
			ControlPanelButtons.Width = 0;

			SettingsManager.RefreshSettings(controlPanels, settings);
			SaveSettings(settings);
		}

		private void SelectFolder_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;
			string index = btn.Name.Remove(0, btn.Name.Length - 1);
			using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
			{
				System.Windows.Forms.DialogResult result = dlg.ShowDialog();
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					TextBox txtBox = GetTextBox(index, "FullPathTextBox");
					txtBox.Text = dlg.SelectedPath;
				}
			}
		}

		private void AddControlRowButton_Click(object sender, RoutedEventArgs e)
		{
			//AddControlRow();
			AddControlPanel();

			SettingsManager.RefreshSettings(controlPanels, settings);
			SaveSettings(settings);
		}
	}
}