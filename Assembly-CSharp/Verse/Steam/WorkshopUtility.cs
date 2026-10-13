using System;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000D99 RID: 3481
	internal static class WorkshopUtility
	{
		// Token: 0x060058D7 RID: 22743 RVA: 0x001C9E14 File Offset: 0x001C8014
		public static string GetLabel(this WorkshopInteractStage stage)
		{
			if (stage == WorkshopInteractStage.None)
			{
				return "None".Translate();
			}
			return ("WorkshopInteractStage_" + stage.ToString()).Translate();
		}

		// Token: 0x060058D8 RID: 22744 RVA: 0x001C9E4A File Offset: 0x001C804A
		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + status.ToString()).Translate();
		}

		// Token: 0x060058D9 RID: 22745 RVA: 0x001C9E6D File Offset: 0x001C806D
		public static string GetLabel(this EResult result)
		{
			return result.ToString().Substring(9);
		}
	}
}
