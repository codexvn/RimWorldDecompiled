using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CAF RID: 7343
	public class QuestPart_SpawnSpaceDrone : QuestPart_SpawnThing
	{
		// Token: 0x0600A394 RID: 41876 RVA: 0x002EEBC4 File Offset: 0x002ECDC4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				Map map = QuestGen_Get.GetMap(false, null, false);
				this.mapParent = ((map != null) ? map.Parent : null);
				if (this.mapParent != null)
				{
					QuestPart_SpawnSpaceDrone.TryFindSpacedronePosition(this.mapParent.Map, out this.cell);
				}
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0600A395 RID: 41877 RVA: 0x002EEC30 File Offset: 0x002ECE30
		public static bool TryFindSpacedronePosition(Map map, out IntVec3 spot)
		{
			IntVec2 size = ThingDefOf.Spacedrone.size;
			CellRect rect = GenAdj.OccupiedRect(IntVec3.Zero, ThingDefOf.Spacedrone.defaultPlacingRot, ThingDefOf.Spacedrone.size);
			IntVec3 position = rect.CenterCell + ThingDefOf.Spacedrone.interactionCellOffset;
			rect = rect.ExpandToFit(position);
			return DropCellFinder.FindSafeLandingSpot(out spot, null, map, 35, 15, 25, new IntVec2?(new IntVec2(rect.Width, rect.Height)), new IntVec3?(ThingDefOf.Spacedrone.interactionCellOffset));
		}
	}
}
