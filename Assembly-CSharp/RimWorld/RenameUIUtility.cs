using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200388F RID: 14479
	public static class RenameUIUtility
	{
		// Token: 0x06014023 RID: 81955 RVA: 0x005F0F38 File Offset: 0x005EF138
		public static void DrawRenameButton(Rect rect, Pawn pawn)
		{
			TooltipHandler.TipRegionByKey(rect, pawn.RaceProps.IsMechanoid ? "RenameMech" : "RenameAnimal");
			if (Widgets.ButtonImage(rect, TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(pawn.NamePawnDialog(null));
			}
		}

		// Token: 0x06014024 RID: 81956 RVA: 0x005F0F84 File Offset: 0x005EF184
		public static void DrawRenameButton(Rect rect, IRenameable renamable)
		{
			TooltipHandler.TipRegionByKey(rect, "Rename");
			if (Widgets.ButtonImage(rect, TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(new Dialog_RenameBuildingStorage(renamable));
			}
		}

		// Token: 0x06014025 RID: 81957 RVA: 0x005F0FB0 File Offset: 0x005EF1B0
		public static void DrawRenameButton(Rect rect, IStorageGroupMember building)
		{
			TooltipHandler.TipRegionByKey(rect, "Rename");
			if (Widgets.ButtonImage(rect, TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(new Dialog_RenameBuildingStorage_CreateNew(building));
			}
		}

		// Token: 0x06014026 RID: 81958 RVA: 0x005F0FDC File Offset: 0x005EF1DC
		public static void DrawRenameButton(Rect rect, CompAnimalPenMarker marker)
		{
			TooltipHandler.TipRegionByKey(rect, "Rename");
			if (Widgets.ButtonImage(rect, TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(new Dialog_RenameAnimalPen(marker.parent.Map, marker));
			}
		}

		// Token: 0x06014027 RID: 81959 RVA: 0x005F1013 File Offset: 0x005EF213
		public static void DrawRenameButton(Rect rect, Building_GravEngine engine)
		{
			TooltipHandler.TipRegionByKey(rect, "Rename");
			if (Widgets.ButtonImage(rect, TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(new Dialog_RenameGravship(engine));
			}
		}
	}
}
