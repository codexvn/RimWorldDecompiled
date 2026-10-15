using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using Steamworks;
using UnityStubs;
using Waylith.AssetBake;

namespace Verse.Steam;

public static class Workshop
{
	[CompilerGenerated]
	private sealed class _003CAllSubscribedItems_003Ed__34 : IEnumerable<PublishedFileId_t>, IEnumerable, IEnumerator<PublishedFileId_t>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PublishedFileId_t _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private PublishedFileId_t[] _003CsubbedItems_003E5__2;

		private uint _003Ccount_003E5__3;

		private int _003Ci_003E5__4;

		PublishedFileId_t IEnumerator<PublishedFileId_t>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CAllSubscribedItems_003Ed__34(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CsubbedItems_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
				_003CsubbedItems_003E5__2 = new PublishedFileId_t[numSubscribedItems];
				_003Ccount_003E5__3 = SteamUGC.GetSubscribedItems(_003CsubbedItems_003E5__2, numSubscribedItems);
				_003Ci_003E5__4 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__4++;
				break;
			}
			if (_003Ci_003E5__4 < _003Ccount_003E5__3)
			{
				PublishedFileId_t publishedFileId_t = _003CsubbedItems_003E5__2[_003Ci_003E5__4];
				_003C_003E2__current = publishedFileId_t;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<PublishedFileId_t> IEnumerable<PublishedFileId_t>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CAllSubscribedItems_003Ed__34(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PublishedFileId_t>)this).GetEnumerator();
		}
	}

	private static WorkshopItemHook uploadingHook;

	private static ModMetaData stagedMod;

	private static UGCUpdateHandle_t curUpdateHandle;

	private static WorkshopInteractStage curStage = WorkshopInteractStage.None;

	private static Callback<RemoteStoragePublishedFileSubscribed_t> subscribedCallback;

	private static Callback<RemoteStoragePublishedFileUnsubscribed_t> unsubscribedCallback;

	private static Callback<ItemInstalled_t> installedCallback;

	private static CallResult<SubmitItemUpdateResult_t> submitResult;

	private static CallResult<CreateItemResult_t> createResult;

	private static CallResult<SteamUGCRequestUGCDetailsResult_t> requestDetailsResult;

	private static UGCQueryHandle_t detailsQueryHandle;

	private static int detailsQueryCount = -1;

	public const uint InstallInfoFolderNameMaxLength = 257u;

	public static WorkshopInteractStage CurStage => curStage;

	internal static void Init()
	{
		subscribedCallback = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(OnItemSubscribed);
		installedCallback = Callback<ItemInstalled_t>.Create(OnItemInstalled);
		unsubscribedCallback = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(OnItemUnsubscribed);
	}

	internal static void Upload(WorkshopUploadable item)
	{
		if (curStage != 0)
		{
			Messages.Message("UploadAlreadyInProgress".Translate(), MessageTypeDefOf.RejectInput, historical: false);
			return;
		}
		uploadingHook = item.GetWorkshopItemHook();
		if (uploadingHook.PublishedFileId != PublishedFileId_t.Invalid)
		{
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Starting item update for mod '" + uploadingHook.Name + "' with PublishedFileId " + uploadingHook.PublishedFileId.ToString());
			}
			curStage = WorkshopInteractStage.SubmittingItem;
			curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), uploadingHook.PublishedFileId);
			SetWorkshopItemDataFrom(curUpdateHandle, uploadingHook, creating: false);
			SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(curUpdateHandle, "[Auto-generated text]: Update on " + DateTime.Now.ToString() + ".");
			submitResult = CallResult<SubmitItemUpdateResult_t>.Create(OnItemSubmitted);
			submitResult.Set(hAPICall);
		}
		else
		{
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Starting item creation for mod '" + uploadingHook.Name + "'.");
			}
			curStage = WorkshopInteractStage.CreatingItem;
			SteamAPICall_t hAPICall2 = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeFirst);
			createResult = CallResult<CreateItemResult_t>.Create(OnItemCreated);
			createResult.Set(hAPICall2);
		}
		Find.WindowStack.Add(new Dialog_WorkshopOperationInProgress());
	}

	internal static void BakeAndUpload(ModMetaData mod)
	{
		if (curStage != 0)
		{
			Messages.Message("UploadAlreadyInProgress".Translate(), MessageTypeDefOf.RejectInput, historical: false);
			return;
		}
		List<string> loadFolders = (mod.uploadBuildAssetPack ? ModContentPack.AllLoadFolders(mod).Where(ModAssetPacks.HasBakeableContent).ToList() : null);
		if (loadFolders.NullOrEmpty())
		{
			Upload(mod);
			return;
		}
		bool skipSourceTextures = mod.uploadSkipSourceTextures;
		string progressPrefix = "BakingModAssetPacksFor".Translate(mod.Name);
		List<string> errors = new List<string>();
		DirectoryInfo staging = null;
		LongEventHandler.QueueLongEvent(delegate
		{
			foreach (string item in loadFolders)
			{
				ContentBakeResult contentBakeResult = ModAssetPacks.BakeForUpload(item, delegate(string file)
				{
					LongEventHandler.SetCurrentEventText(progressPrefix + ": " + file);
				});
				if (!contentBakeResult.Succeeded)
				{
					errors.AddRange(contentBakeResult.Errors);
				}
			}
			if (errors.Count == 0)
			{
				staging = new DirectoryInfo(Path.Combine(GenFilePaths.SaveDataFolderPath, "WorkshopUpload", mod.PackageIdPlayerFacing));
				BuildStagingDirectory(mod, staging, skipSourceTextures);
			}
		}, "BakingModAssetPacks", doAsynchronously: true, delegate(Exception e)
		{
			errors.Add(e.Message);
		}, showExtraUIInfo: true, forceHideUI: false, delegate
		{
			if (errors.Count > 0)
			{
				TryDeleteStaging(staging);
				Find.WindowStack.Add(new Dialog_MessageBox("ModAssetPackBakeFailed".Translate(mod.Name) + "\n" + errors.ToLineList("  - ")));
			}
			else if (curStage != 0)
			{
				TryDeleteStaging(staging);
				Messages.Message("UploadAlreadyInProgress".Translate(), MessageTypeDefOf.RejectInput, historical: false);
			}
			else
			{
				mod.workshopUploadDirectoryOverride = staging;
				stagedMod = ((staging != null) ? mod : null);
				Upload(mod);
			}
		});
	}

	private static void BuildStagingDirectory(ModMetaData mod, DirectoryInfo staging, bool skipSourceTextures)
	{
		string fullName = mod.RootDir.FullName;
		if (staging.Exists)
		{
			staging.Delete(recursive: true);
		}
		staging.Create();
		foreach (string item in Directory.EnumerateFiles(fullName, "*", SearchOption.AllDirectories))
		{
			if (!ContentPacks.IsPackSidecar(item) && (!skipSourceTextures || !ContentPacks.IsPackedSource(item)))
			{
				string text = Path.Combine(staging.FullName, Path.GetRelativePath(fullName, item));
				Directory.CreateDirectory(Path.GetDirectoryName(text));
				File.Copy(item, text);
			}
		}
	}

	private static void CopyPublishedFileIdToStaging()
	{
		if (stagedMod != null)
		{
			string text = Path.Combine(stagedMod.workshopUploadDirectoryOverride.FullName, "About/PublishedFileId.txt");
			Directory.CreateDirectory(Path.GetDirectoryName(text));
			File.Copy(Path.Combine(stagedMod.RootDir.FullName, "About/PublishedFileId.txt"), text, overwrite: true);
		}
	}

	private static void ClearStaging()
	{
		if (stagedMod != null)
		{
			TryDeleteStaging(stagedMod.workshopUploadDirectoryOverride);
			stagedMod.workshopUploadDirectoryOverride = null;
			stagedMod = null;
		}
	}

	private static void TryDeleteStaging(DirectoryInfo staging)
	{
		if (staging == null)
		{
			return;
		}
		try
		{
			staging.Refresh();
			if (staging.Exists)
			{
				staging.Delete(recursive: true);
			}
		}
		catch (Exception ex)
		{
			Log.Warning("Workshop: Could not delete upload staging directory " + staging.FullName + ": " + ex.Message);
		}
	}

	internal static void Unsubscribe(PublishedFileId_t pfid)
	{
		SteamUGC.UnsubscribeItem(pfid);
	}

	internal static void Unsubscribe(WorkshopUploadable item)
	{
		SteamUGC.UnsubscribeItem(item.GetPublishedFileId());
	}

	internal static void RequestItemsDetails(PublishedFileId_t[] publishedFileIds)
	{
		if (detailsQueryCount >= 0)
		{
			Log.Error("Requested Workshop item details while a details request was already pending.");
			return;
		}
		detailsQueryCount = publishedFileIds.Length;
		detailsQueryHandle = SteamUGC.CreateQueryUGCDetailsRequest(publishedFileIds, (uint)detailsQueryCount);
		SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(detailsQueryHandle);
		requestDetailsResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(OnGotItemDetails);
		requestDetailsResult.Set(hAPICall);
	}

	internal static void OnItemSubscribed(RemoteStoragePublishedFileSubscribed_t result)
	{
		if (IsOurAppId(result.m_nAppID))
		{
			if (Prefs.LogVerbose)
			{
				PublishedFileId_t nPublishedFileId = result.m_nPublishedFileId;
				Log.Message("Workshop: Item subscribed: " + nPublishedFileId.ToString());
			}
			Find.WindowStack.WindowOfType<Page_ModsConfig>()?.Notify_SteamItemSubscribed(result.m_nPublishedFileId);
			WorkshopItems.Notify_Subscribed(result.m_nPublishedFileId);
		}
	}

	internal static void OnItemInstalled(ItemInstalled_t result)
	{
		if (IsOurAppId(result.m_unAppID))
		{
			if (Prefs.LogVerbose)
			{
				PublishedFileId_t nPublishedFileId = result.m_nPublishedFileId;
				Log.Message("Workshop: Item installed: " + nPublishedFileId.ToString());
			}
			Find.WindowStack.WindowOfType<Page_ModsConfig>()?.Notify_SteamItemInstalled(result.m_nPublishedFileId);
			WorkshopItems.Notify_Installed(result.m_nPublishedFileId);
		}
	}

	internal static void OnItemUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t result)
	{
		if (IsOurAppId(result.m_nAppID))
		{
			if (Prefs.LogVerbose)
			{
				PublishedFileId_t nPublishedFileId = result.m_nPublishedFileId;
				Log.Message("Workshop: Item unsubscribed: " + nPublishedFileId.ToString());
			}
			Find.WindowStack.WindowOfType<Page_ModsConfig>()?.Notify_SteamItemUnsubscribed(result.m_nPublishedFileId);
			Find.WindowStack.WindowOfType<Page_SelectScenario>()?.Notify_SteamItemUnsubscribed(result.m_nPublishedFileId);
			WorkshopItems.Notify_Unsubscribed(result.m_nPublishedFileId);
		}
	}

	private static void OnItemCreated(CreateItemResult_t result, bool IOFailure)
	{
		if (IOFailure || result.m_eResult != EResult.k_EResultOK)
		{
			uploadingHook = null;
			Dialog_WorkshopOperationInProgress.CloseAll();
			Log.Error("Workshop: OnItemCreated failure. Result: " + result.m_eResult.GetLabel());
			Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(GenText.SplitCamelCase(result.m_eResult.GetLabel()))));
			ClearStaging();
			curStage = WorkshopInteractStage.None;
			createResult = null;
			return;
		}
		uploadingHook.PublishedFileId = result.m_nPublishedFileId;
		CopyPublishedFileIdToStaging();
		if (Prefs.LogVerbose)
		{
			Log.Message("Workshop: Item created. PublishedFileId: " + uploadingHook.PublishedFileId.ToString());
		}
		curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), uploadingHook.PublishedFileId);
		SetWorkshopItemDataFrom(curUpdateHandle, uploadingHook, creating: true);
		curStage = WorkshopInteractStage.SubmittingItem;
		if (Prefs.LogVerbose)
		{
			Log.Message("Workshop: Submitting item.");
		}
		SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(curUpdateHandle, "[Auto-generated text]: Initial upload.");
		submitResult = CallResult<SubmitItemUpdateResult_t>.Create(OnItemSubmitted);
		submitResult.Set(hAPICall);
		createResult = null;
	}

	private static void OnItemSubmitted(SubmitItemUpdateResult_t result, bool IOFailure)
	{
		if (IOFailure || result.m_eResult != EResult.k_EResultOK)
		{
			uploadingHook = null;
			Dialog_WorkshopOperationInProgress.CloseAll();
			Log.Error("Workshop: OnItemSubmitted failure. Result: " + result.m_eResult.GetLabel());
			Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(GenText.SplitCamelCase(result.m_eResult.GetLabel()))));
		}
		else
		{
			SteamUtility.OpenWorkshopPage(uploadingHook.PublishedFileId);
			Messages.Message("WorkshopUploadSucceeded".Translate(uploadingHook.Name), MessageTypeDefOf.TaskCompletion, historical: false);
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item submit result: " + result.m_eResult);
			}
		}
		ClearStaging();
		curStage = WorkshopInteractStage.None;
		submitResult = null;
	}

	private static void OnGotItemDetails(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
	{
		if (IOFailure)
		{
			Log.Error("Workshop: OnGotItemDetails IOFailure.");
			detailsQueryCount = -1;
			return;
		}
		if (detailsQueryCount < 0)
		{
			Log.Warning("Got unexpected Steam Workshop item details response.");
		}
		string text = "Steam Workshop Item details received:";
		for (int i = 0; i < detailsQueryCount; i++)
		{
			SteamUGC.GetQueryUGCResult(detailsQueryHandle, (uint)i, out var pDetails);
			if (pDetails.m_eResult != EResult.k_EResultOK)
			{
				text = text + "\n  Query result: " + pDetails.m_eResult;
			}
			else
			{
				text = text + "\n  Title: " + pDetails.m_rgchTitle;
				string text2 = text;
				PublishedFileId_t nPublishedFileId = pDetails.m_nPublishedFileId;
				text = text2 + "\n  PublishedFileId: " + nPublishedFileId.ToString();
				text = text + "\n  Created: " + DateTime.FromFileTimeUtc(pDetails.m_rtimeCreated);
				text = text + "\n  Updated: " + DateTime.FromFileTimeUtc(pDetails.m_rtimeUpdated);
				text = text + "\n  Added to list: " + DateTime.FromFileTimeUtc(pDetails.m_rtimeAddedToUserList);
				text = text + "\n  File size: " + pDetails.m_nFileSize.ToStringKilobytes();
				text = text + "\n  Preview size: " + pDetails.m_nPreviewFileSize.ToStringKilobytes();
				text = text + "\n  File name: " + pDetails.m_pchFileName;
				string text3 = text;
				AppId_t nCreatorAppID = pDetails.m_nCreatorAppID;
				text = text3 + "\n  CreatorAppID: " + nCreatorAppID.ToString();
				string text4 = text;
				nCreatorAppID = pDetails.m_nConsumerAppID;
				text = text4 + "\n  ConsumerAppID: " + nCreatorAppID.ToString();
				text = text + "\n  Visibiliy: " + pDetails.m_eVisibility;
				text = text + "\n  FileType: " + pDetails.m_eFileType;
				text = text + "\n  Owner: " + pDetails.m_ulSteamIDOwner;
			}
			text += "\n";
		}
		Log.Message(text.TrimEndNewlines());
		detailsQueryCount = -1;
	}

	public static void GetUpdateStatus(out EItemUpdateStatus updateStatus, out float progPercent)
	{
		updateStatus = SteamUGC.GetItemUpdateProgress(curUpdateHandle, out var punBytesProcessed, out var punBytesTotal);
		progPercent = (float)punBytesProcessed / (float)punBytesTotal;
	}

	public static string UploadButtonLabel(PublishedFileId_t pfid)
	{
		return (pfid != PublishedFileId_t.Invalid) ? "UpdateOnSteamWorkshop".Translate() : "UploadToSteamWorkshop".Translate();
	}

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
		foreach (System.Version supportedVersion in hook.SupportedVersions)
		{
			tags.Add(supportedVersion.Major + "." + supportedVersion.Minor);
		}
		SteamUGC.SetItemTags(updateHandle, tags);
		SteamUGC.SetItemContent(updateHandle, hook.Directory.FullName);
	}

	[IteratorStateMachine(typeof(_003CAllSubscribedItems_003Ed__34))]
	internal static IEnumerable<PublishedFileId_t> AllSubscribedItems()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllSubscribedItems_003Ed__34(-2);
	}

	[DebugOutput("System", false)]
	internal static void SteamWorkshopStatus()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("All subscribed items (" + SteamUGC.GetNumSubscribedItems() + " total):");
		List<PublishedFileId_t> list = AllSubscribedItems().ToList();
		for (int i = 0; i < list.Count; i++)
		{
			stringBuilder.AppendLine("   " + ItemStatusString(list[i]));
		}
		stringBuilder.AppendLine("All installed mods:");
		foreach (ModMetaData allInstalledMod in ModLister.AllInstalledMods)
		{
			stringBuilder.AppendLine("   " + allInstalledMod.PackageIdPlayerFacing + ": " + ItemStatusString(allInstalledMod.GetPublishedFileId()));
		}
		Log.Message(stringBuilder.ToString());
		List<PublishedFileId_t> list2 = AllSubscribedItems().ToList();
		PublishedFileId_t[] array = new PublishedFileId_t[list2.Count];
		for (int j = 0; j < list2.Count; j++)
		{
			array[j] = (PublishedFileId_t)list2[j].m_PublishedFileId;
		}
		RequestItemsDetails(array);
	}

	private static string ItemStatusString(PublishedFileId_t pfid)
	{
		if (pfid == PublishedFileId_t.Invalid)
		{
			return "[unpublished]";
		}
		PublishedFileId_t publishedFileId_t = pfid;
		string text = "[" + publishedFileId_t.ToString() + "] ";
		if (SteamUGC.GetItemInstallInfo(pfid, out var punSizeOnDisk, out var pchFolder, 257u, out var _))
		{
			text += "\n      installed";
			text = text + "\n      folder=" + pchFolder;
			return text + "\n      sizeOnDisk=" + ((float)punSizeOnDisk / 1024f).ToString("F2") + "Kb";
		}
		return text + "\n      not installed";
	}

	private static bool IsOurAppId(AppId_t appId)
	{
		if (appId != SteamUtils.GetAppID())
		{
			return false;
		}
		return true;
	}
}
