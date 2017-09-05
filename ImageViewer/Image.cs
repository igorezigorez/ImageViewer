using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace ImageViewer
{
	class Image
	{
		public BitmapImage Img { get; private set; }

		public string FullPath
		{
			get
			{
				if (Img != null)
					return Img.UriSource.LocalPath.ToString();
				return null;
			}
			private set
			{
				FullPath = value;
			}
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
