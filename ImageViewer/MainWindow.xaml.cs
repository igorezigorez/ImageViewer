using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
		private string[] allImagesPath;
		private bool isPictureOpened = false;
		private string currentImagePath;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void OpenButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();



			if (dlg.ShowDialog() == true)
			{

				currentImage = new Image(dlg.FileName);
				Picture.Source = currentImage.Img;
				isPictureOpened = true;

				string fileDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
				allImagesPath = Directory.GetFiles(fileDirectory);
				
				currentImagePath = dlg.FileName;
			}
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			PictureGrid.Margin = new Thickness(0, 20, 300, 0);
			SettingsGrid.Width = 300;
		}

		private void MoveImage(string imagePath, string directoryPath)
		{
			if(!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			if (!File.Exists(System.IO.Path.Combine(
				directoryPath, Path.GetFileName(Picture.Source.ToString()))))
			{
				File.Copy((Picture.Source as BitmapImage).UriSource.LocalPath,
					System.IO.Path.Combine(
						directoryPath,
						System.IO.Path.GetFileName(Picture.Source.ToString())));
			}
			if(MoveRadioButton.IsChecked == true)
			{
				currentImagePath = (Picture.Source as BitmapImage).UriSource.LocalPath;
				ShowNextImage();
				File.Delete(currentImagePath);
			}
			
		}

		private void MainGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Right)
				ShowNextImage();
			if (e.Key == Key.Left)
				ShowPreviousImage();
			if (e.Key == Key.F5)
				MoveImage(currentImagePath, F5DirectoryPath.Text);
			if (e.Key == Key.F6)
				MoveImage(currentImagePath, F6DirectoryPath.Text);
			if (e.Key == Key.F7)
				MoveImage(currentImagePath, F7DirectoryPath.Text);
		}

		private void ShowNextImage()
		{
			if (isPictureOpened == false)
				return;

			string currentImagePath = currentImage.FullPath;
			int index = Array.IndexOf(allImagesPath, currentImagePath);
			if (index != allImagesPath.Count() - 1)
				index++;
			else
				index = 0;

			currentImage = new Image(allImagesPath[index]);
			Picture.Source = currentImage.Img;
			currentImagePath = currentImage.FullPath;
		}

		private void ShowPreviousImage()
		{
			if (isPictureOpened == false)
				return;

			string currentImagePath = currentImage.FullPath;
			int index = Array.IndexOf(allImagesPath, currentImagePath);
			if (index != 0)
				index--;
			else
				index = allImagesPath.Count() - 1;

			currentImage = new Image(allImagesPath[index]);
			Picture.Source = currentImage.Img;
			currentImagePath = currentImage.FullPath;
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			PictureGrid.Margin = new Thickness(0, 20, 0, 0);
			SettingsGrid.Width = 0;
		}

		private void F5SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
			{
				System.Windows.Forms.DialogResult result = dlg.ShowDialog();
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					F5DirectoryPath.Text = dlg.SelectedPath;
				}
			}
		}

		private void F6SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
			{
				System.Windows.Forms.DialogResult result = dlg.ShowDialog();
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					F6DirectoryPath.Text = dlg.SelectedPath;
				}
			}
		}

		private void F7SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
			{
				System.Windows.Forms.DialogResult result = dlg.ShowDialog();
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					F7DirectoryPath.Text = dlg.SelectedPath;
				}
			}
		}
	}
}

