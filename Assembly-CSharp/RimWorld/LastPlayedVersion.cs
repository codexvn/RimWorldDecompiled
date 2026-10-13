using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02003775 RID: 14197
	public static class LastPlayedVersion
	{
		// Token: 0x17003250 RID: 12880
		// (get) Token: 0x06013AC9 RID: 80585 RVA: 0x005D86B5 File Offset: 0x005D68B5
		public static Version Version
		{
			get
			{
				LastPlayedVersion.InitializeIfNeeded();
				return LastPlayedVersion.lastPlayedVersionInt;
			}
		}

		// Token: 0x06013ACA RID: 80586 RVA: 0x005D86C4 File Offset: 0x005D68C4
		public static void InitializeIfNeeded()
		{
			if (LastPlayedVersion.initialized)
			{
				return;
			}
			try
			{
				string text = null;
				if (File.Exists(GenFilePaths.LastPlayedVersionFilePath))
				{
					try
					{
						text = File.ReadAllText(GenFilePaths.LastPlayedVersionFilePath);
					}
					catch (Exception ex)
					{
						Log.Error("Exception getting last played version data. Path: " + GenFilePaths.LastPlayedVersionFilePath + ". Exception: " + ex.ToString());
					}
				}
				if (text != null)
				{
					try
					{
						LastPlayedVersion.lastPlayedVersionInt = VersionControl.VersionFromString(text);
					}
					catch (Exception ex2)
					{
						Log.Error("Exception parsing last version from string '" + text + "': " + ex2.ToString());
					}
				}
				if (LastPlayedVersion.lastPlayedVersionInt != VersionControl.CurrentVersion)
				{
					File.WriteAllText(GenFilePaths.LastPlayedVersionFilePath, VersionControl.CurrentVersionString);
				}
			}
			finally
			{
				LastPlayedVersion.initialized = true;
			}
		}

		// Token: 0x0400D9FF RID: 55807
		private static bool initialized;

		// Token: 0x0400DA00 RID: 55808
		private static Version lastPlayedVersionInt;
	}
}
