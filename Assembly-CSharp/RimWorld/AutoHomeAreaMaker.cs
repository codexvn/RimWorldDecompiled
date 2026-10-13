using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034FD RID: 13565
	public static class AutoHomeAreaMaker
	{
		// Token: 0x060130F4 RID: 78068 RVA: 0x005A72DE File Offset: 0x005A54DE
		private static bool ShouldAdd()
		{
			return Find.PlaySettings.autoHomeArea && Current.ProgramState == ProgramState.Playing;
		}

		// Token: 0x060130F5 RID: 78069 RVA: 0x005A72F6 File Offset: 0x005A54F6
		public static void Notify_BuildingSpawned(Thing b)
		{
			if (!AutoHomeAreaMaker.ShouldAdd() || !b.def.building.expandHomeArea || b.Faction != Faction.OfPlayer)
			{
				return;
			}
			AutoHomeAreaMaker.MarkHomeAroundThing(b);
		}

		// Token: 0x060130F6 RID: 78070 RVA: 0x005A72F6 File Offset: 0x005A54F6
		public static void Notify_BuildingClaimed(Thing b)
		{
			if (!AutoHomeAreaMaker.ShouldAdd() || !b.def.building.expandHomeArea || b.Faction != Faction.OfPlayer)
			{
				return;
			}
			AutoHomeAreaMaker.MarkHomeAroundThing(b);
		}

		// Token: 0x060130F7 RID: 78071 RVA: 0x005A7328 File Offset: 0x005A5528
		public static void MarkHomeAroundThing(Thing t)
		{
			if (!AutoHomeAreaMaker.ShouldAdd())
			{
				return;
			}
			CellRect cellRect = new CellRect(t.Position.x - t.RotatedSize.x / 2 - 4, t.Position.z - t.RotatedSize.z / 2 - 4, t.RotatedSize.x + 8, t.RotatedSize.z + 8);
			cellRect.ClipInsideMap(t.Map);
			foreach (IntVec3 c in cellRect)
			{
				t.Map.areaManager.Home[c] = true;
			}
		}

		// Token: 0x060130F8 RID: 78072 RVA: 0x005A73F4 File Offset: 0x005A55F4
		public static void Notify_ZoneCellAdded(IntVec3 c, Zone zone)
		{
			if (!AutoHomeAreaMaker.ShouldAdd())
			{
				return;
			}
			foreach (IntVec3 c2 in CellRect.CenteredOn(c, 4).ClipInsideMap(zone.Map))
			{
				zone.Map.areaManager.Home[c2] = true;
			}
		}

		// Token: 0x0400C5A0 RID: 50592
		private const int BorderWidth = 4;
	}
}
