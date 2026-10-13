using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003817 RID: 14359
	public class PlaceWorker_AnimalPenMarker : PlaceWorker
	{
		// Token: 0x06013DCB RID: 81355 RVA: 0x005E58E0 File Offset: 0x005E3AE0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (thing == null)
			{
				PlaceWorker_AnimalPenMarker.tmpPlacingBlueprintPainter.Paint(center, Find.CurrentMap);
				return;
			}
			if (thing is Blueprint || thing is Frame)
			{
				PlaceWorker_AnimalPenMarker.tmpPlacedBlueprintPainter.Paint(center, Find.CurrentMap);
				return;
			}
			PlaceWorker_AnimalPenMarker.tmpPainter.Paint(center, Find.CurrentMap);
		}

		// Token: 0x06013DCC RID: 81356 RVA: 0x005E5935 File Offset: 0x005E3B35
		public override void DrawMouseAttachments(BuildableDef def)
		{
			Find.CurrentMap.animalPenManager.DrawPlacingMouseAttachments(def);
		}

		// Token: 0x0400DBB8 RID: 56248
		private static readonly AnimalPenGUI.PenPainter tmpPainter = new AnimalPenGUI.PenPainter();

		// Token: 0x0400DBB9 RID: 56249
		private static readonly AnimalPenGUI.PenBlueprintPainter tmpPlacingBlueprintPainter = new AnimalPenGUI.PenBlueprintPainter();

		// Token: 0x0400DBBA RID: 56250
		private static readonly AnimalPenGUI.PenBlueprintPainter tmpPlacedBlueprintPainter = new AnimalPenGUI.PenBlueprintPainter();
	}
}
