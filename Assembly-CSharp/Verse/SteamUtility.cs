using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000D62 RID: 3426
	public static class SteamUtility
	{
		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06005725 RID: 22309 RVA: 0x001C0EF3 File Offset: 0x001BF0F3
		public static string SteamPersonaName
		{
			get
			{
				if (SteamManager.Initialized && SteamUtility.cachedPersonaName == null)
				{
					SteamUtility.cachedPersonaName = SteamFriends.GetPersonaName();
				}
				if (SteamUtility.cachedPersonaName == null)
				{
					return "???";
				}
				return SteamUtility.cachedPersonaName;
			}
		}

		// Token: 0x06005726 RID: 22310 RVA: 0x001C0F1F File Offset: 0x001BF11F
		public static void OpenUrl(string url)
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
				return;
			}
			Application.OpenURL(url);
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x001C0F3D File Offset: 0x001BF13D
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x001C0F4C File Offset: 0x001BF14C
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("http://steamcommunity.com/workshop/browse/?appid=" + SteamUtils.GetAppID().ToString());
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x001C0F7C File Offset: 0x001BF17C
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			string str = "steam://url/CommunityFilePage/";
			PublishedFileId_t publishedFileId_t = pfid;
			return str + publishedFileId_t.ToString();
		}

		// Token: 0x04003F85 RID: 16261
		private static string cachedPersonaName;
	}
}
