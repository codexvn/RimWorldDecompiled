using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000D8F RID: 3471
	public class WorkshopItem
	{
		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x060058A2 RID: 22690 RVA: 0x001C993B File Offset: 0x001C7B3B
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x060058A3 RID: 22691 RVA: 0x001C9943 File Offset: 0x001C7B43
		// (set) Token: 0x060058A4 RID: 22692 RVA: 0x001C994B File Offset: 0x001C7B4B
		public virtual PublishedFileId_t PublishedFileId
		{
			get
			{
				return this.pfidInt;
			}
			set
			{
				this.pfidInt = value;
			}
		}

		// Token: 0x060058A5 RID: 22693 RVA: 0x001C9954 File Offset: 0x001C7B54
		public static WorkshopItem MakeFrom(PublishedFileId_t pfid)
		{
			ulong num;
			string path;
			uint num2;
			bool itemInstallInfo = SteamUGC.GetItemInstallInfo(pfid, out num, out path, 257U, out num2);
			WorkshopItem workshopItem = null;
			if (!itemInstallInfo)
			{
				workshopItem = new WorkshopItem_Downloading();
			}
			else
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				if (!directoryInfo.Exists)
				{
					string str = "Created WorkshopItem for ";
					PublishedFileId_t publishedFileId_t = pfid;
					Log.Error(str + publishedFileId_t.ToString() + " but there is no folder for it.");
					return new WorkshopItem_NotInstalled();
				}
				FileInfo[] files = directoryInfo.GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].Extension == ".rsc")
					{
						workshopItem = new WorkshopItem_Scenario();
						break;
					}
				}
				if (workshopItem == null)
				{
					workshopItem = new WorkshopItem_Mod();
				}
				workshopItem.directoryInt = directoryInfo;
			}
			workshopItem.PublishedFileId = pfid;
			return workshopItem;
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x001C9A10 File Offset: 0x001C7C10
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId.ToString();
		}

		// Token: 0x04004056 RID: 16470
		protected DirectoryInfo directoryInt;

		// Token: 0x04004057 RID: 16471
		private PublishedFileId_t pfidInt;
	}
}
