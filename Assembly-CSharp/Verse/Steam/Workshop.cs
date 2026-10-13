using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000D8D RID: 3469
	public static class Workshop
	{
		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06005886 RID: 22662 RVA: 0x001C8D0F File Offset: 0x001C6F0F
		public static WorkshopInteractStage CurStage
		{
			get
			{
				return Workshop.curStage;
			}
		}

		// Token: 0x06005887 RID: 22663 RVA: 0x001C8D18 File Offset: 0x001C6F18
		internal static void Init()
		{
			Workshop.subscribedCallback = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(new Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate(Workshop.OnItemSubscribed));
			Workshop.installedCallback = Callback<ItemInstalled_t>.Create(new Callback<ItemInstalled_t>.DispatchDelegate(Workshop.OnItemInstalled));
			Workshop.unsubscribedCallback = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(new Callback<RemoteStoragePublishedFileUnsubscribed_t>.DispatchDelegate(Workshop.OnItemUnsubscribed));
		}

		// Token: 0x06005888 RID: 22664 RVA: 0x001C8D68 File Offset: 0x001C6F68
		internal static void Upload(WorkshopUploadable item)
		{
			if (Workshop.curStage != WorkshopInteractStage.None)
			{
				Messages.Message("UploadAlreadyInProgress".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			Workshop.uploadingHook = item.GetWorkshopItemHook();
			if (Workshop.uploadingHook.PublishedFileId != PublishedFileId_t.Invalid)
			{
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Starting item update for mod '" + Workshop.uploadingHook.Name + "' with PublishedFileId " + Workshop.uploadingHook.PublishedFileId.ToString());
				}
				Workshop.curStage = WorkshopInteractStage.SubmittingItem;
				Workshop.curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), Workshop.uploadingHook.PublishedFileId);
				Workshop.SetWorkshopItemDataFrom(Workshop.curUpdateHandle, Workshop.uploadingHook, false);
				SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(Workshop.curUpdateHandle, "[Auto-generated text]: Update on " + DateTime.Now.ToString() + ".");
				Workshop.submitResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(Workshop.OnItemSubmitted));
				Workshop.submitResult.Set(hAPICall, null);
			}
			else
			{
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Starting item creation for mod '" + Workshop.uploadingHook.Name + "'.");
				}
				Workshop.curStage = WorkshopInteractStage.CreatingItem;
				SteamAPICall_t hAPICall2 = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeFirst);
				Workshop.createResult = CallResult<CreateItemResult_t>.Create(new CallResult<CreateItemResult_t>.APIDispatchDelegate(Workshop.OnItemCreated));
				Workshop.createResult.Set(hAPICall2, null);
			}
			Find.WindowStack.Add(new Dialog_WorkshopOperationInProgress());
		}

		// Token: 0x06005889 RID: 22665 RVA: 0x001C8EDA File Offset: 0x001C70DA
		internal static void Unsubscribe(PublishedFileId_t pfid)
		{
			SteamUGC.UnsubscribeItem(pfid);
		}

		// Token: 0x0600588A RID: 22666 RVA: 0x001C8EE3 File Offset: 0x001C70E3
		internal static void Unsubscribe(WorkshopUploadable item)
		{
			SteamUGC.UnsubscribeItem(item.GetPublishedFileId());
		}

		// Token: 0x0600588B RID: 22667 RVA: 0x001C8EF4 File Offset: 0x001C70F4
		internal static void RequestItemsDetails(PublishedFileId_t[] publishedFileIds)
		{
			if (Workshop.detailsQueryCount >= 0)
			{
				Log.Error("Requested Workshop item details while a details request was already pending.");
				return;
			}
			Workshop.detailsQueryCount = publishedFileIds.Length;
			Workshop.detailsQueryHandle = SteamUGC.CreateQueryUGCDetailsRequest(publishedFileIds, (uint)Workshop.detailsQueryCount);
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(Workshop.detailsQueryHandle);
			Workshop.requestDetailsResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(Workshop.OnGotItemDetails));
			Workshop.requestDetailsResult.Set(hAPICall, null);
		}

		// Token: 0x0600588C RID: 22668 RVA: 0x001C8F5C File Offset: 0x001C715C
		internal static void OnItemSubscribed(RemoteStoragePublishedFileSubscribed_t result)
		{
			if (!Workshop.IsOurAppId(result.m_nAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				string str = "Workshop: Item subscribed: ";
				PublishedFileId_t nPublishedFileId = result.m_nPublishedFileId;
				Log.Message(str + nPublishedFileId.ToString());
			}
			Page_ModsConfig page_ModsConfig = Find.WindowStack.WindowOfType<Page_ModsConfig>();
			if (page_ModsConfig != null)
			{
				page_ModsConfig.Notify_SteamItemSubscribed(result.m_nPublishedFileId);
			}
			WorkshopItems.Notify_Subscribed(result.m_nPublishedFileId);
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x001C8FC8 File Offset: 0x001C71C8
		internal static void OnItemInstalled(ItemInstalled_t result)
		{
			if (!Workshop.IsOurAppId(result.m_unAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				string str = "Workshop: Item installed: ";
				PublishedFileId_t nPublishedFileId = result.m_nPublishedFileId;
				Log.Message(str + nPublishedFileId.ToString());
			}
			Page_ModsConfig page_ModsConfig = Find.WindowStack.WindowOfType<Page_ModsConfig>();
			if (page_ModsConfig != null)
			{
				page_ModsConfig.Notify_SteamItemInstalled(result.m_nPublishedFileId);
			}
			WorkshopItems.Notify_Installed(result.m_nPublishedFileId);
		}

		// Token: 0x0600588E RID: 22670 RVA: 0x001C9034 File Offset: 0x001C7234
		internal static void OnItemUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t result)
		{
			if (!Workshop.IsOurAppId(result.m_nAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				string str = "Workshop: Item unsubscribed: ";
				PublishedFileId_t nPublishedFileId = result.m_nPublishedFileId;
				Log.Message(str + nPublishedFileId.ToString());
			}
			Page_ModsConfig page_ModsConfig = Find.WindowStack.WindowOfType<Page_ModsConfig>();
			if (page_ModsConfig != null)
			{
				page_ModsConfig.Notify_SteamItemUnsubscribed(result.m_nPublishedFileId);
			}
			Page_SelectScenario page_SelectScenario = Find.WindowStack.WindowOfType<Page_SelectScenario>();
			if (page_SelectScenario != null)
			{
				page_SelectScenario.Notify_SteamItemUnsubscribed(result.m_nPublishedFileId);
			}
			WorkshopItems.Notify_Unsubscribed(result.m_nPublishedFileId);
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x001C90B8 File Offset: 0x001C72B8
		private static void OnItemCreated(CreateItemResult_t result, bool IOFailure)
		{
			if (IOFailure || result.m_eResult != EResult.k_EResultOK)
			{
				Workshop.uploadingHook = null;
				Dialog_WorkshopOperationInProgress.CloseAll();
				Log.Error("Workshop: OnItemCreated failure. Result: " + result.m_eResult.GetLabel());
				Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(GenText.SplitCamelCase(result.m_eResult.GetLabel())), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				return;
			}
			Workshop.uploadingHook.PublishedFileId = result.m_nPublishedFileId;
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item created. PublishedFileId: " + Workshop.uploadingHook.PublishedFileId.ToString());
			}
			Workshop.curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), Workshop.uploadingHook.PublishedFileId);
			Workshop.SetWorkshopItemDataFrom(Workshop.curUpdateHandle, Workshop.uploadingHook, true);
			Workshop.curStage = WorkshopInteractStage.SubmittingItem;
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Submitting item.");
			}
			SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(Workshop.curUpdateHandle, "[Auto-generated text]: Initial upload.");
			Workshop.submitResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(Workshop.OnItemSubmitted));
			Workshop.submitResult.Set(hAPICall, null);
			Workshop.createResult = null;
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x001C91E4 File Offset: 0x001C73E4
		private static void OnItemSubmitted(SubmitItemUpdateResult_t result, bool IOFailure)
		{
			if (IOFailure || result.m_eResult != EResult.k_EResultOK)
			{
				Workshop.uploadingHook = null;
				Dialog_WorkshopOperationInProgress.CloseAll();
				Log.Error("Workshop: OnItemSubmitted failure. Result: " + result.m_eResult.GetLabel());
				Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(GenText.SplitCamelCase(result.m_eResult.GetLabel())), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			}
			else
			{
				SteamUtility.OpenWorkshopPage(Workshop.uploadingHook.PublishedFileId);
				Messages.Message("WorkshopUploadSucceeded".Translate(Workshop.uploadingHook.Name), MessageTypeDefOf.TaskCompletion, false);
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Item submit result: " + result.m_eResult.ToString());
				}
			}
			Workshop.curStage = WorkshopInteractStage.None;
			Workshop.submitResult = null;
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x001C92C8 File Offset: 0x001C74C8
		private static void OnGotItemDetails(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			if (IOFailure)
			{
				Log.Error("Workshop: OnGotItemDetails IOFailure.");
				Workshop.detailsQueryCount = -1;
				return;
			}
			if (Workshop.detailsQueryCount < 0)
			{
				Log.Warning("Got unexpected Steam Workshop item details response.");
			}
			string text = "Steam Workshop Item details received:";
			for (int i = 0; i < Workshop.detailsQueryCount; i++)
			{
				SteamUGCDetails_t steamUGCDetails_t;
				SteamUGC.GetQueryUGCResult(Workshop.detailsQueryHandle, (uint)i, out steamUGCDetails_t);
				if (steamUGCDetails_t.m_eResult != EResult.k_EResultOK)
				{
					text = text + "\n  Query result: " + steamUGCDetails_t.m_eResult.ToString();
				}
				else
				{
					text = text + "\n  Title: " + steamUGCDetails_t.m_rgchTitle;
					string str = text;
					string str2 = "\n  PublishedFileId: ";
					PublishedFileId_t nPublishedFileId = steamUGCDetails_t.m_nPublishedFileId;
					text = str + str2 + nPublishedFileId.ToString();
					text = text + "\n  Created: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeCreated)).ToString();
					text = text + "\n  Updated: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeUpdated)).ToString();
					text = text + "\n  Added to list: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeAddedToUserList)).ToString();
					text = text + "\n  File size: " + steamUGCDetails_t.m_nFileSize.ToStringKilobytes("F2");
					text = text + "\n  Preview size: " + steamUGCDetails_t.m_nPreviewFileSize.ToStringKilobytes("F2");
					text = text + "\n  File name: " + steamUGCDetails_t.m_pchFileName;
					string str3 = text;
					string str4 = "\n  CreatorAppID: ";
					AppId_t appId_t = steamUGCDetails_t.m_nCreatorAppID;
					text = str3 + str4 + appId_t.ToString();
					string str5 = text;
					string str6 = "\n  ConsumerAppID: ";
					appId_t = steamUGCDetails_t.m_nConsumerAppID;
					text = str5 + str6 + appId_t.ToString();
					text = text + "\n  Visibiliy: " + steamUGCDetails_t.m_eVisibility.ToString();
					text = text + "\n  FileType: " + steamUGCDetails_t.m_eFileType.ToString();
					text = text + "\n  Owner: " + steamUGCDetails_t.m_ulSteamIDOwner.ToString();
				}
				text += "\n";
			}
			Log.Message(text.TrimEndNewlines());
			Workshop.detailsQueryCount = -1;
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x001C94E8 File Offset: 0x001C76E8
		public static void GetUpdateStatus(out EItemUpdateStatus updateStatus, out float progPercent)
		{
			ulong num;
			ulong num2;
			updateStatus = SteamUGC.GetItemUpdateProgress(Workshop.curUpdateHandle, out num, out num2);
			progPercent = num / num2;
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x001C950E File Offset: 0x001C770E
		public static string UploadButtonLabel(PublishedFileId_t pfid)
		{
			return (pfid != PublishedFileId_t.Invalid) ? "UpdateOnSteamWorkshop".Translate() : "UploadToSteamWorkshop".Translate();
		}

		// Token: 0x06005894 RID: 22676 RVA: 0x001C9538 File Offset: 0x001C7738
		private static void SetWorkshopItemDataFrom(UGCUpdateHandle_t updateHandle, WorkshopItemHook hook, bool creating)
		{
			hook.PrepareForWorkshopUpload();
			SteamUGC.SetItemTitle(updateHandle, hook.Name);
			if (creating)
			{
				SteamUGC.SetItemDescription(updateHandle, hook.Description);
			}
			if (!File.Exists(hook.PreviewImagePath))
			{
				Log.Warning("Missing preview file at " + hook.PreviewImagePath);
			}
			else
			{
				SteamUGC.SetItemPreview(updateHandle, hook.PreviewImagePath);
			}
			IList<string> tags = hook.Tags;
			foreach (System.Version version in hook.SupportedVersions)
			{
				tags.Add(version.Major.ToString() + "." + version.Minor.ToString());
			}
			SteamUGC.SetItemTags(updateHandle, tags);
			SteamUGC.SetItemContent(updateHandle, hook.Directory.FullName);
		}

		// Token: 0x06005895 RID: 22677 RVA: 0x001C9620 File Offset: 0x001C7820
		internal static IEnumerable<PublishedFileId_t> AllSubscribedItems()
		{
			uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
			PublishedFileId_t[] subbedItems = new PublishedFileId_t[numSubscribedItems];
			uint count = SteamUGC.GetSubscribedItems(subbedItems, numSubscribedItems);
			int i = 0;
			while ((long)i < (long)((ulong)count))
			{
				PublishedFileId_t publishedFileId_t = subbedItems[i];
				yield return publishedFileId_t;
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x001C962C File Offset: 0x001C782C
		[DebugOutput("System", false)]
		internal static void SteamWorkshopStatus()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All subscribed items (" + SteamUGC.GetNumSubscribedItems().ToString() + " total):");
			List<PublishedFileId_t> list = Workshop.AllSubscribedItems().ToList<PublishedFileId_t>();
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.AppendLine("   " + Workshop.ItemStatusString(list[i]));
			}
			stringBuilder.AppendLine("All installed mods:");
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				stringBuilder.AppendLine("   " + modMetaData.PackageIdPlayerFacing + ": " + Workshop.ItemStatusString(modMetaData.GetPublishedFileId()));
			}
			Log.Message(stringBuilder.ToString());
			List<PublishedFileId_t> list2 = Workshop.AllSubscribedItems().ToList<PublishedFileId_t>();
			PublishedFileId_t[] array = new PublishedFileId_t[list2.Count];
			for (int j = 0; j < list2.Count; j++)
			{
				array[j] = (PublishedFileId_t)list2[j].m_PublishedFileId;
			}
			Workshop.RequestItemsDetails(array);
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x001C976C File Offset: 0x001C796C
		private static string ItemStatusString(PublishedFileId_t pfid)
		{
			if (pfid == PublishedFileId_t.Invalid)
			{
				return "[unpublished]";
			}
			string str = "[";
			PublishedFileId_t publishedFileId_t = pfid;
			string text = str + publishedFileId_t.ToString() + "] ";
			ulong num;
			string str2;
			uint num2;
			if (SteamUGC.GetItemInstallInfo(pfid, out num, out str2, 257U, out num2))
			{
				text += "\n      installed";
				text = text + "\n      folder=" + str2;
				text = text + "\n      sizeOnDisk=" + (num / 1024f).ToString("F2") + "Kb";
			}
			else
			{
				text += "\n      not installed";
			}
			return text;
		}

		// Token: 0x06005898 RID: 22680 RVA: 0x001C980F File Offset: 0x001C7A0F
		private static bool IsOurAppId(AppId_t appId)
		{
			return !(appId != SteamUtils.GetAppID());
		}

		// Token: 0x04004044 RID: 16452
		private static WorkshopItemHook uploadingHook;

		// Token: 0x04004045 RID: 16453
		private static UGCUpdateHandle_t curUpdateHandle;

		// Token: 0x04004046 RID: 16454
		private static WorkshopInteractStage curStage = WorkshopInteractStage.None;

		// Token: 0x04004047 RID: 16455
		private static Callback<RemoteStoragePublishedFileSubscribed_t> subscribedCallback;

		// Token: 0x04004048 RID: 16456
		private static Callback<RemoteStoragePublishedFileUnsubscribed_t> unsubscribedCallback;

		// Token: 0x04004049 RID: 16457
		private static Callback<ItemInstalled_t> installedCallback;

		// Token: 0x0400404A RID: 16458
		private static CallResult<SubmitItemUpdateResult_t> submitResult;

		// Token: 0x0400404B RID: 16459
		private static CallResult<CreateItemResult_t> createResult;

		// Token: 0x0400404C RID: 16460
		private static CallResult<SteamUGCRequestUGCDetailsResult_t> requestDetailsResult;

		// Token: 0x0400404D RID: 16461
		private static UGCQueryHandle_t detailsQueryHandle;

		// Token: 0x0400404E RID: 16462
		private static int detailsQueryCount = -1;

		// Token: 0x0400404F RID: 16463
		public const uint InstallInfoFolderNameMaxLength = 257U;
	}
}
