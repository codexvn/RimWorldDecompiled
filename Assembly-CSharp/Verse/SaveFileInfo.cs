using System;
using System.IO;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200084A RID: 2122
	public class SaveFileInfo
	{
		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x060035B7 RID: 13751 RVA: 0x0011DE80 File Offset: 0x0011C080
		private bool Valid
		{
			get
			{
				object obj = this.lockObject;
				bool result;
				lock (obj)
				{
					result = (this.gameVersion != null);
				}
				return result;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x060035B8 RID: 13752 RVA: 0x0011DEC8 File Offset: 0x0011C0C8
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x060035B9 RID: 13753 RVA: 0x0011DED0 File Offset: 0x0011C0D0
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x060035BA RID: 13754 RVA: 0x0011DED8 File Offset: 0x0011C0D8
		public DateTime LastWriteTime
		{
			get
			{
				return this.lastWriteTime;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x060035BB RID: 13755 RVA: 0x0011DEE0 File Offset: 0x0011C0E0
		public string GameVersion
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return "LoadingVersionInfo".Translate();
						}
						if (!this.Valid)
						{
							return "???";
						}
						return this.gameVersion;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return "LoadingVersionInfo".Translate();
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x060035BC RID: 13756 RVA: 0x0011DF60 File Offset: 0x0011C160
		public Color VersionColor
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return Color.gray;
						}
						if (!this.Valid)
						{
							return ColorLibrary.RedReadable;
						}
						if (VersionControl.MajorFromVersionString(this.gameVersion) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(this.gameVersion) == VersionControl.CurrentMinor)
						{
							return SaveFileInfo.UnimportantTextColor;
						}
						if (BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
						{
							return Color.yellow;
						}
						return ColorLibrary.RedReadable;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return Color.gray;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x060035BD RID: 13757 RVA: 0x0011E00C File Offset: 0x0011C20C
		public TipSignal CompatibilityTip
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return "LoadingVersionInfo".Translate();
						}
						if (!this.Valid)
						{
							return "SaveIsUnknownFormat".Translate();
						}
						if ((VersionControl.MajorFromVersionString(this.gameVersion) != VersionControl.CurrentMajor || VersionControl.MinorFromVersionString(this.gameVersion) != VersionControl.CurrentMinor) && !BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
						{
							return "SaveIsFromDifferentGameVersion".Translate(VersionControl.CurrentVersionString, this.gameVersion);
						}
						if (VersionControl.BuildFromVersionString(this.gameVersion) != VersionControl.CurrentBuild)
						{
							return "SaveIsFromDifferentGameBuild".Translate(VersionControl.CurrentVersionString, this.gameVersion);
						}
						return "SaveIsFromThisGameBuild".Translate();
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return "LoadingVersionInfo".Translate();
			}
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x0011E138 File Offset: 0x0011C338
		private bool TryLock(int timeoutMilliseconds)
		{
			return Monitor.TryEnter(this.lockObject, timeoutMilliseconds);
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x0011E146 File Offset: 0x0011C346
		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.fileName = fileInfo.Name;
			this.lastWriteTime = fileInfo.LastWriteTime;
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x0011E178 File Offset: 0x0011C378
		public void LoadData()
		{
			object obj = this.lockObject;
			lock (obj)
			{
				this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(this.fileInfo);
				this.loaded = true;
			}
		}

		// Token: 0x0400287A RID: 10362
		private FileInfo fileInfo;

		// Token: 0x0400287B RID: 10363
		private string gameVersion;

		// Token: 0x0400287C RID: 10364
		private DateTime lastWriteTime;

		// Token: 0x0400287D RID: 10365
		private string fileName;

		// Token: 0x0400287E RID: 10366
		private bool loaded;

		// Token: 0x0400287F RID: 10367
		private object lockObject = new object();

		// Token: 0x04002880 RID: 10368
		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);
	}
}
