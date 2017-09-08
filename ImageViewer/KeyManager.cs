using System;
using System.Collections.Generic;

namespace ImageViewer
{
	class KeyManager
	{
		private List<string> usedKeys;

		public KeyManager()
		{
			usedKeys = new List<string>();
		}

		public bool IsKeyUsed(string key)
		{
			int index = usedKeys.IndexOf(key);
			if (index == -1)
				return false;
			return true;
		}

		public void AddKey(string key)
		{
			usedKeys.Add(key);
		}

		public void RemoveKey(string key)
		{
			usedKeys.Remove(key);
		}
	}
}