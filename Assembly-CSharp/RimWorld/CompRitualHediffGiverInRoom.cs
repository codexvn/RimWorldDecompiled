using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003866 RID: 14438
	public class CompRitualHediffGiverInRoom : ThingComp
	{
		// Token: 0x170032FB RID: 13051
		// (get) Token: 0x06013F59 RID: 81753 RVA: 0x005EBFEC File Offset: 0x005EA1EC
		private CompProperties_RitualHediffGiverInRoom Props
		{
			get
			{
				return (CompProperties_RitualHediffGiverInRoom)this.props;
			}
		}

		// Token: 0x06013F5A RID: 81754 RVA: 0x005EBFFC File Offset: 0x005EA1FC
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(60) && this.parent.IsRitualTarget())
			{
				Room room = this.parent.GetRoom(RegionType.Set_All);
				if (room == null || room.TouchesMapEdge)
				{
					return;
				}
				foreach (Pawn pawn in this.parent.Map.mapPawns.AllPawnsSpawned)
				{
					if (pawn.RaceProps.IsFlesh && pawn.GetRoom(RegionType.Set_All) == room && this.parent.Position.InHorDistOf(pawn.Position, this.Props.minRadius))
					{
						Hediff hediff = HediffMaker.MakeHediff(this.Props.hediff, pawn, null);
						if (this.Props.severity > 0f)
						{
							hediff.Severity = this.Props.severity;
						}
						pawn.health.AddHediff(hediff, null, null, null);
						if (this.Props.resetLastRecreationalDrugTick && pawn.mindState != null)
						{
							pawn.mindState.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame;
						}
					}
				}
			}
		}

		// Token: 0x0400DC67 RID: 56423
		private const int CheckInterval = 60;
	}
}
