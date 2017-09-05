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
using ImageViewer;

namespace ImageViewer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Image currentImage;
		private List<string> allImagesPath;
		private bool isPictureOpened = false;
		private string currentImagePath;
		private KeyManager keyManager = new KeyManager();
		private Interface interfaceSettings = new Interface();

		public MainWindow()
		{
			InitializeComponent();

			//Button closeSettingsButton = new Button();
			//closeSettingsButton.Name = "CloseSettingsButton";
			//closeSettingsButton.HorizontalAlignment = HorizontalAlignment.Right;
			//closeSettingsButton.VerticalAlignment = VerticalAlignment.Top;
			//closeSettingsButton.Width = Double.NaN;
			//closeSettingsButton.Content = "Close";
			//closeSettingsButton.Margin = new Thickness(0, 5, 5, 0);
			//closeSettingsButton.Click += CloseSettingsButton_Click;
			//Grid.SetRow(closeSettingsButton, 0);
			//SettingsGrid.Children.Add(closeSettingsButton);
		}



		private void AddControlRow()
		{
			RowDefinition rd = new RowDefinition();
			rd.Height = new GridLength(60, GridUnitType.Pixel);
			int rdCount = SettingsGrid.RowDefinitions.Count;
			rd.Name = String.Concat("RowDefenition", rdCount);
			SettingsGrid.RowDefinitions.Add(rd);
			interfaceSettings.AddRow(rd);
			rdCount++;
			int marginRatio = rdCount - 1;

			#region
			Button deleteKeyButton = interfaceSettings.CreateButton(String.Concat("DeleteKeyButton", marginRatio), Interface.ButtonType.DeleteKeyButton);
			deleteKeyButton.Margin = new Thickness(83, 4, 0, 0);
			Grid.SetRow(deleteKeyButton, marginRatio);
			deleteKeyButton.Click += DeleteKeyButton_Click;
			SettingsGrid.Children.Add(deleteKeyButton);
			interfaceSettings.AddButton(deleteKeyButton);

			Button selectFolderButton = interfaceSettings.CreateButton(String.Concat("SelectFolderButton", marginRatio), Interface.ButtonType.SelectButton);
			selectFolderButton.Click += SelectFolder_Click;
			selectFolderButton.Margin = new Thickness(148, 29, 0, 0);
			Grid.SetRow(selectFolderButton, marginRatio);
			SettingsGrid.Children.Add(selectFolderButton);
			interfaceSettings.AddButton(selectFolderButton);

			TextBox fullPathTextBox = interfaceSettings.CreateTextBox(String.Concat("FullPathTextBox", marginRatio), Interface.TextBoxType.FullPathTextBox);
			fullPathTextBox.Margin = new Thickness(5, 30, 5, 0);
			fullPathTextBox.TextChanged += TextBox_TextChanged;
			Grid.SetRow(fullPathTextBox, marginRatio);
			SettingsGrid.Children.Add(fullPathTextBox);
			interfaceSettings.AddTextBox(fullPathTextBox);

			TextBox directoryNameTextBox = interfaceSettings.CreateTextBox(String.Concat("DirectoryNameTextBox", marginRatio), Interface.TextBoxType.DirectoryNameTextBox);
			directoryNameTextBox.Margin = new Thickness(200, 30, 5, 0);
			directoryNameTextBox.TextChanged += TextBox_TextChanged;
			Grid.SetRow(directoryNameTextBox, marginRatio);
			SettingsGrid.Children.Add(directoryNameTextBox);
			interfaceSettings.AddTextBox(directoryNameTextBox);

			TextBox keyTextBox = interfaceSettings.CreateTextBox(String.Concat("KeyTextBox", marginRatio), Interface.TextBoxType.KeyTextBox);
			keyTextBox.Margin = new Thickness(38, 4, 0, 0);
			keyTextBox.PreviewKeyDown += Bind_KeyDown;
			Grid.SetRow(keyTextBox, marginRatio);
			SettingsGrid.Children.Add(keyTextBox);
			interfaceSettings.AddTextBox(keyTextBox);

			Label keyLabel = interfaceSettings.CreateLabel(String.Concat("KeyLabel", marginRatio), Interface.LabelType.KeyLabel);
			keyLabel.Margin = new Thickness(5, 0, 0, 0);
			Grid.SetRow(keyLabel, marginRatio);
			SettingsGrid.Children.Add(keyLabel);
			interfaceSettings.AddLabel(keyLabel);

			Label orLabel = interfaceSettings.CreateLabel(String.Concat("OrLabel", marginRatio), Interface.LabelType.OrLabel);
			orLabel.Margin = new Thickness(180, 25, 0, 0);
			Grid.SetRow(orLabel, marginRatio);
			SettingsGrid.Children.Add(orLabel);
			interfaceSettings.AddLabel(orLabel);
			#endregion
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

		private void DeleteKeyButton_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;
			string index = btn.Name.Remove(0, btn.Name.Length - 1);

			IEnumerable<UIElement> uiButtons = interfaceSettings.buttons.
				Where(element => element.Name.Remove(0, element.Name.Length - 1) == index).
				Select(element => element);

			foreach (UIElement element in uiButtons)
			{
				SettingsGrid.Children.Remove(element);
			}

			IEnumerable<UIElement> uiTextBoxes = interfaceSettings.textBoxes.
				Where(element => element.Name.Remove(0, element.Name.Length - 1) == index).
				Select(element => element);

			foreach (UIElement element in uiTextBoxes)
			{
				SettingsGrid.Children.Remove(element);
			}

			IEnumerable<UIElement> uiLabels = interfaceSettings.labels.
				Where(element => element.Name.Remove(0, element.Name.Length - 1) == index).
				Select(element => element);
			foreach (UIElement element in uiLabels)
			{
				SettingsGrid.Children.Remove(element);
			}

			IEnumerable<RowDefinition> uiRows = interfaceSettings.rows.
				Where(element => element.Name.Remove(0, element.Name.Length - 1) == index).
				Select(element => element);
			foreach (RowDefinition element in uiRows)
			{
				SettingsGrid.RowDefinitions.Remove(element);
			}
			//interfaceSettings.DeleteButton("SelectFolderButton" + index);

		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			PictureGrid.Margin = new Thickness(0, 20, 300, 0);
			SettingsGrid.Width = 340;
			SettingsScrollViewer.Width = 360;
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

		private void TextBox_TextChanged(object sender, RoutedEventArgs e)
		{
			TextBox txtBox = (TextBox)sender;
			string index = txtBox.Name.Remove(0, txtBox.Name.Length - 1);

			if (txtBox.Name == String.Concat("FullPathTextBox", index))
			{
				TextBox directoryNameTextBox = GetTextBox(index, "DirectoryNameTextBox");
				directoryNameTextBox.Text = "";
			}

			if (txtBox.Name.Contains("DirectoryNameTextBox"))
			{
				TextBox fullPathTextBox = GetTextBox(index, "FullPathTextBox");
				fullPathTextBox.Text = "";
			}
		}

		private void MainGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Right)
				ShowNextImage();
			if (e.Key == Key.Left)
				ShowPreviousImage();
			if (keyManager.IsKeyBusy(e.Key.ToString()))
			{
				string index = GetKeyIndex(e.Key.ToString());
				TextBox fullPathTextBox = GetTextBox(index, "FullPathTextBox");
				TextBox directoryNameTextBox = GetTextBox(index, "DirectoryNameTextBox");
				string directoryPath = CreateDirectoryPath(fullPathTextBox.Text, directoryNameTextBox.Text);
				MoveImage(currentImage.FullPath, directoryPath);
			}
		}

		private string CreateDirectoryPath(string fullPath, string directoryName)
		{
			if (fullPath != "")
			{
				return fullPath;
			}

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

		private string GetKeyIndex(string key)
		{
			IEnumerable<TextBox> textBoxes = interfaceSettings.textBoxes.
				Where(txtBox => txtBox.Name.Contains("KeyTextBox") == true && txtBox.Text == key).
				Select(txtBox => txtBox);

			foreach (TextBox txtBox in textBoxes)
			{
				return txtBox.Name.Remove(0, txtBox.Name.Length - 1);
			}
			return "";
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
			AddControlRow();
		}
	}
}