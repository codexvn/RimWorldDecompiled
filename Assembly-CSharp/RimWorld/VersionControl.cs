using System;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x020010F3 RID: 4339
	public static class VersionControl
	{
		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06006970 RID: 26992 RVA: 0x00213665 File Offset: 0x00211865
		public static Version CurrentVersion
		{
			get
			{
				return VersionControl.version;
			}
		}

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06006971 RID: 26993 RVA: 0x0021366C File Offset: 0x0021186C
		public static string CurrentVersionString
		{
			get
			{
				return VersionControl.versionString;
			}
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06006972 RID: 26994 RVA: 0x00213673 File Offset: 0x00211873
		public static string CurrentVersionStringWithoutBuild
		{
			get
			{
				return VersionControl.versionStringWithoutBuild;
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06006973 RID: 26995 RVA: 0x0021367A File Offset: 0x0021187A
		public static string CurrentVersionStringWithRev
		{
			get
			{
				return VersionControl.versionStringWithRev;
			}
		}

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06006974 RID: 26996 RVA: 0x00213681 File Offset: 0x00211881
		public static int CurrentMajor
		{
			get
			{
				return VersionControl.version.Major;
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06006975 RID: 26997 RVA: 0x0021368D File Offset: 0x0021188D
		public static int CurrentMinor
		{
			get
			{
				return VersionControl.version.Minor;
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06006976 RID: 26998 RVA: 0x00213699 File Offset: 0x00211899
		public static int CurrentBuild
		{
			get
			{
				return VersionControl.version.Build;
			}
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06006977 RID: 26999 RVA: 0x002136A5 File Offset: 0x002118A5
		public static int CurrentRevision
		{
			get
			{
				return VersionControl.version.Revision;
			}
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06006978 RID: 27000 RVA: 0x002136B1 File Offset: 0x002118B1
		public static DateTime CurrentBuildDate
		{
			get
			{
				return VersionControl.buildDate;
			}
		}

		// Token: 0x06006979 RID: 27001 RVA: 0x002136B8 File Offset: 0x002118B8
		static VersionControl()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			VersionControl.buildDate = new DateTime(2000, 1, 1).AddDays((double)version.Build);
			int build = version.Build - 4805;
			int revision = version.Revision * 2 / 60;
			VersionControl.version = new Version(version.Major, version.Minor, build, revision);
			VersionControl.versionStringWithRev = string.Concat(new string[]
			{
				VersionControl.version.Major.ToString(),
				".",
				VersionControl.version.Minor.ToString(),
				".",
				VersionControl.version.Build.ToString(),
				" rev",
				VersionControl.version.Revision.ToString()
			});
			VersionControl.versionString = string.Concat(new string[]
			{
				VersionControl.version.Major.ToString(),
				".",
				VersionControl.version.Minor.ToString(),
				".",
				VersionControl.version.Build.ToString()
			});
			VersionControl.versionStringWithoutBuild = VersionControl.version.Major.ToString() + "." + VersionControl.version.Minor.ToString();
		}

		// Token: 0x0600697A RID: 27002 RVA: 0x00213840 File Offset: 0x00211A40
		public static void DrawInfoInCorner()
		{
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			string text = "VersionIndicator".Translate(VersionControl.versionString);
			string versionExtraInfo = VersionControl.GetVersionExtraInfo();
			if (!versionExtraInfo.NullOrEmpty())
			{
				text = text + " (" + versionExtraInfo + ")";
			}
			text += "\n" + "CompiledOn".Translate(VersionControl.buildDate.ToString("MMM d yyyy"));
			if (SteamManager.Initialized)
			{
				text += "\n" + "LoggedIntoSteamAs".Translate(SteamUtility.SteamPersonaName);
			}
			Rect rect = new Rect(10f, 10f, 330f, Text.CalcHeight(text, 330f));
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			LatestVersionGetter component = Current.Root.gameObject.GetComponent<LatestVersionGetter>();
			Rect rect2 = new Rect(10f, rect.yMax - 5f, 330f, 999f);
			component.DrawAt(rect2);
		}

		// Token: 0x0600697B RID: 27003 RVA: 0x0021397C File Offset: 0x00211B7C
		private static string GetVersionExtraInfo()
		{
			string text = "";
			if (UnityData.Is32BitBuild)
			{
				text += "32-bit";
			}
			else if (UnityData.Is64BitBuild)
			{
				text += "64-bit";
			}
			return text;
		}

		// Token: 0x0600697C RID: 27004 RVA: 0x002139B8 File Offset: 0x00211BB8
		public static void LogVersionNumber()
		{
			Log.Message("RimWorld " + VersionControl.versionStringWithRev);
		}

		// Token: 0x0600697D RID: 27005 RVA: 0x002139CE File Offset: 0x00211BCE
		public static bool IsCompatible(Version v)
		{
			return v.Major == VersionControl.CurrentMajor && v.Minor == VersionControl.CurrentMinor;
		}

		// Token: 0x0600697E RID: 27006 RVA: 0x002139EC File Offset: 0x00211BEC
		public static bool TryParseVersionString(string str, out Version version)
		{
			version = null;
			if (str == null)
			{
				return false;
			}
			string[] array = str.Split('.', StringSplitOptions.None);
			if (array.Length < 2)
			{
				return false;
			}
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					return false;
				}
				if (num < 0)
				{
					return false;
				}
			}
			version = new Version(int.Parse(array[0]), int.Parse(array[1]));
			return true;
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x00213A4C File Offset: 0x00211C4C
		public static int BuildFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			string[] array = str.Split('.', StringSplitOptions.None);
			if (array.Length < 3 || !int.TryParse(array[2], out result))
			{
				Log.Warning("Could not get build from version string " + str);
			}
			return result;
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x00213A90 File Offset: 0x00211C90
		public static int MinorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			string[] array = str.Split('.', StringSplitOptions.None);
			if (array.Length < 2 || !int.TryParse(array[1], out result))
			{
				Log.Warning("Could not get minor version from version string " + str);
			}
			return result;
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x00213AD4 File Offset: 0x00211CD4
		public static int MajorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			if (!int.TryParse(str.Split('.', StringSplitOptions.None)[0], out result))
			{
				Log.Warning("Could not get major version from version string " + str);
			}
			return result;
		}

		// Token: 0x06006982 RID: 27010 RVA: 0x00213B10 File Offset: 0x00211D10
		public static string VersionStringWithoutRev(string str)
		{
			return str.Split(' ', StringSplitOptions.None)[0];
		}

		// Token: 0x06006983 RID: 27011 RVA: 0x00213B20 File Offset: 0x00211D20
		public static Version VersionFromString(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException("str");
			}
			string[] array = str.Split('.', StringSplitOptions.None);
			if (array.Length > 3)
			{
				throw new ArgumentException("str");
			}
			int major = 0;
			int minor = 0;
			int build = 0;
			for (int i = 0; i < array.Length; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					throw new ArgumentException("str");
				}
				if (num < 0)
				{
					throw new ArgumentException("str");
				}
				switch (i)
				{
				case 0:
					major = num;
					break;
				case 1:
					minor = num;
					break;
				case 2:
					build = num;
					break;
				}
			}
			return new Version(major, minor, build);
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x00213BC8 File Offset: 0x00211DC8
		public static bool IsWellFormattedVersionString(string str)
		{
			string[] array = str.Split('.', StringSplitOptions.None);
			if (array.Length != 2)
			{
				return false;
			}
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					return false;
				}
				if (num < 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04004AA6 RID: 19110
		private static Version version;

		// Token: 0x04004AA7 RID: 19111
		private static string versionStringWithoutBuild;

		// Token: 0x04004AA8 RID: 19112
		private static string versionString;

		// Token: 0x04004AA9 RID: 19113
		private static string versionStringWithRev;

		// Token: 0x04004AAA RID: 19114
		private static DateTime buildDate;
	}
}
