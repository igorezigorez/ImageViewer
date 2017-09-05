using System;
using System.Collections.Generic;

namespace ImageViewer
{
	class KeyManager
	{
		private List<string> busyKeys;

		public KeyManager()
		{
			busyKeys = new List<string>();
		}

		public bool IsKeyBusy(string key)
		{
			int index = busyKeys.IndexOf(key);
			if (index == -1)
				return false;
			return true;
		}

		public void AddKey(string key)
		{
			busyKeys.Add(key);
		}

		public void RemoveKey(string key)
		{
			busyKeys.Remove(key);
		}
	}
}