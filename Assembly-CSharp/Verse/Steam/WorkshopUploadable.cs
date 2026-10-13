using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000D95 RID: 3477
	public interface WorkshopUploadable
	{
		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x060058B3 RID: 22707
		IEnumerable<System.Version> SupportedVersions { get; }

		// Token: 0x060058B4 RID: 22708
		bool CanToUploadToWorkshop();

		// Token: 0x060058B5 RID: 22709
		void PrepareForWorkshopUpload();

		// Token: 0x060058B6 RID: 22710
		PublishedFileId_t GetPublishedFileId();

		// Token: 0x060058B7 RID: 22711
		void SetPublishedFileId(PublishedFileId_t pfid);

		// Token: 0x060058B8 RID: 22712
		string GetWorkshopName();

		// Token: 0x060058B9 RID: 22713
		string GetWorkshopDescription();

		// Token: 0x060058BA RID: 22714
		string GetWorkshopPreviewImagePath();

		// Token: 0x060058BB RID: 22715
		IList<string> GetWorkshopTags();

		// Token: 0x060058BC RID: 22716
		DirectoryInfo GetWorkshopUploadDirectory();

		// Token: 0x060058BD RID: 22717
		WorkshopItemHook GetWorkshopItemHook();
	}
}
