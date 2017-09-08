using System;
using System.Windows.Media.Imaging;
using System.IO;

namespace ImageViewer
{
	[Serializable]
	class Image
	{
		public BitmapImage Img { get; }

		public string FullPath
		{
			get => Img?.UriSource.LocalPath;
			private set => FullPath = value;
		}
		public string Name
		{
			get
			{
				if (File.Exists(FullPath))
					return Path.GetFileName(FullPath);
				return null;
			}
			private set
			{
				Name = value;
			}
		}

		public Image(string path)
		{
			if (path != null && File.Exists(path))
			{
				Img = new BitmapImage();
				Img.BeginInit();
				Img.CacheOption = BitmapCacheOption.OnLoad;
				Img.UriSource = new Uri(path);
				Img.DecodePixelWidth = 1280;
				Img.EndInit();
			}
		}
	}
}
