using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000D97 RID: 3479
	public static class WorkshopItems
	{
		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x060058CB RID: 22731 RVA: 0x001C9C41 File Offset: 0x001C7E41
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x060058CC RID: 22732 RVA: 0x001C9C48 File Offset: 0x001C7E48
		public static IEnumerable<WorkshopItem_Downloading> AllDownloadingItems
		{
			get
			{
				return WorkshopItems.downloadingItems;
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x060058CD RID: 22733 RVA: 0x001C9C50 File Offset: 0x001C7E50
		public static int DownloadingItemsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < WorkshopItems.subbedItems.Count; i++)
				{
					if (WorkshopItems.subbedItems[i] is WorkshopItem_Downloading)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x060058CE RID: 22734 RVA: 0x001C9C8B File Offset: 0x001C7E8B
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060058CF RID: 22735 RVA: 0x000026EA File Offset: 0x000008EA
		public static void EnsureInit()
		{
		}

		// Token: 0x060058D0 RID: 22736 RVA: 0x001C9CA8 File Offset: 0x001C7EA8
		public static WorkshopItem GetItem(PublishedFileId_t pfid)
		{
			for (int i = 0; i < WorkshopItems.subbedItems.Count; i++)
			{
				if (WorkshopItems.subbedItems[i].PublishedFileId == pfid)
				{
					return WorkshopItems.subbedItems[i];
				}
			}
			return null;
		}

		// Token: 0x060058D1 RID: 22737 RVA: 0x001C9CEF File Offset: 0x001C7EEF
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		// Token: 0x060058D2 RID: 22738 RVA: 0x001C9CFC File Offset: 0x001C7EFC
		private static void RebuildItemsList()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			WorkshopItems.subbedItems.Clear();
			WorkshopItems.downloadingItems.Clear();
			foreach (PublishedFileId_t pfid in Workshop.AllSubscribedItems())
			{
				WorkshopItem workshopItem = WorkshopItem.MakeFrom(pfid);
				if (workshopItem != null)
				{
					WorkshopItems.subbedItems.Add(workshopItem);
					if (workshopItem is WorkshopItem_Downloading)
					{
						WorkshopItems.downloadingItems.Add(workshopItem as WorkshopItem_Downloading);
					}
				}
			}
			ScenarioLister.MarkDirty();
		}

		// Token: 0x060058D3 RID: 22739 RVA: 0x001C9D90 File Offset: 0x001C7F90
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060058D4 RID: 22740 RVA: 0x001C9D90 File Offset: 0x001C7F90
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060058D5 RID: 22741 RVA: 0x001C9D90 File Offset: 0x001C7F90
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060058D6 RID: 22742 RVA: 0x001C9D98 File Offset: 0x001C7F98
		public static string DebugOutput()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Subscribed items:");
			foreach (WorkshopItem workshopItem in WorkshopItems.subbedItems)
			{
				stringBuilder.AppendLine("  " + workshopItem.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400405E RID: 16478
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();

		// Token: 0x0400405F RID: 16479
		private static List<WorkshopItem_Downloading> downloadingItems = new List<WorkshopItem_Downloading>();
	}
}
