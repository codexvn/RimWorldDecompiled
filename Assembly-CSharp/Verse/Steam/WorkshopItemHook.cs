using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000D96 RID: 3478
	public class WorkshopItemHook
	{
		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x060058BE RID: 22718 RVA: 0x001C9AFD File Offset: 0x001C7CFD
		// (set) Token: 0x060058BF RID: 22719 RVA: 0x001C9B0A File Offset: 0x001C7D0A
		public PublishedFileId_t PublishedFileId
		{
			get
			{
				return this.owner.GetPublishedFileId();
			}
			set
			{
				this.owner.SetPublishedFileId(value);
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x060058C0 RID: 22720 RVA: 0x001C9B18 File Offset: 0x001C7D18
		public string Name
		{
			get
			{
				return this.owner.GetWorkshopName();
			}
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x060058C1 RID: 22721 RVA: 0x001C9B25 File Offset: 0x001C7D25
		public string Description
		{
			get
			{
				return this.owner.GetWorkshopDescription();
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x060058C2 RID: 22722 RVA: 0x001C9B32 File Offset: 0x001C7D32
		public string PreviewImagePath
		{
			get
			{
				return this.owner.GetWorkshopPreviewImagePath();
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x060058C3 RID: 22723 RVA: 0x001C9B3F File Offset: 0x001C7D3F
		public IList<string> Tags
		{
			get
			{
				return this.owner.GetWorkshopTags();
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x060058C4 RID: 22724 RVA: 0x001C9B4C File Offset: 0x001C7D4C
		public DirectoryInfo Directory
		{
			get
			{
				return this.owner.GetWorkshopUploadDirectory();
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x060058C5 RID: 22725 RVA: 0x001C9B59 File Offset: 0x001C7D59
		public IEnumerable<System.Version> SupportedVersions
		{
			get
			{
				return this.owner.SupportedVersions;
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x060058C6 RID: 22726 RVA: 0x001C9B66 File Offset: 0x001C7D66
		public bool MayHaveAuthorNotCurrentUser
		{
			get
			{
				return !(this.PublishedFileId == PublishedFileId_t.Invalid) && (this.steamAuthor == CSteamID.Nil || this.steamAuthor != SteamUser.GetSteamID());
			}
		}

		// Token: 0x060058C7 RID: 22727 RVA: 0x001C9BA0 File Offset: 0x001C7DA0
		public WorkshopItemHook(WorkshopUploadable owner)
		{
			this.owner = owner;
			if (owner.GetPublishedFileId() != PublishedFileId_t.Invalid)
			{
				this.SendSteamDetailsQuery();
			}
		}

		// Token: 0x060058C8 RID: 22728 RVA: 0x001C9BD2 File Offset: 0x001C7DD2
		public void PrepareForWorkshopUpload()
		{
			this.owner.PrepareForWorkshopUpload();
		}

		// Token: 0x060058C9 RID: 22729 RVA: 0x001C9BE0 File Offset: 0x001C7DE0
		private void SendSteamDetailsQuery()
		{
			if (SteamManager.Initialized)
			{
				SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(this.PublishedFileId, 999999U);
				this.queryResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnDetailsQueryReturned));
				this.queryResult.Set(hAPICall, null);
			}
		}

		// Token: 0x060058CA RID: 22730 RVA: 0x001C9C29 File Offset: 0x001C7E29
		private void OnDetailsQueryReturned(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			this.steamAuthor = (CSteamID)result.m_details.m_ulSteamIDOwner;
		}

		// Token: 0x0400405B RID: 16475
		private WorkshopUploadable owner;

		// Token: 0x0400405C RID: 16476
		private CSteamID steamAuthor = CSteamID.Nil;

		// Token: 0x0400405D RID: 16477
		private CallResult<SteamUGCRequestUGCDetailsResult_t> queryResult;
	}
}
