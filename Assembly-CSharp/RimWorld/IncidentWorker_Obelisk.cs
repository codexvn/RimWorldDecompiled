using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE6 RID: 7654
	public abstract class IncidentWorker_Obelisk : IncidentWorker
	{
		// Token: 0x17001A1D RID: 6685
		// (get) Token: 0x0600AA9A RID: 43674
		public abstract ThingDef ObeliskDef { get; }

		// Token: 0x0600AA9B RID: 43675 RVA: 0x0030C880 File Offset: 0x0030AA80
		public override float ChanceFactorNow(IIncidentTarget target)
		{
			Map map = target as Map;
			if (map == null)
			{
				return base.ChanceFactorNow(target);
			}
			int num = map.listerBuildings.allBuildingsNonColonist.Count((Building b) => b.def.GetCompProperties<CompProperties_Obelisk>() != null);
			return ((num > 0) ? ((float)num * 0.7f) : 1f) * base.ChanceFactorNow(target);
		}

		// Token: 0x0600AA9C RID: 43676 RVA: 0x0030C8EC File Offset: 0x0030AAEC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return this.TryFindCell(out intVec, map, this.ObeliskDef);
		}

		// Token: 0x0600AA9D RID: 43677 RVA: 0x0030C914 File Offset: 0x0030AB14
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Skyfaller skyfaller = this.SpawnObeliskIncoming(map);
			if (skyfaller == null)
			{
				return false;
			}
			skyfaller.impactLetter = LetterMaker.MakeLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef ?? LetterDefOf.NeutralEvent, new TargetInfo(skyfaller.Position, map, false), null, null, null);
			return true;
		}

		// Token: 0x0600AA9E RID: 43678 RVA: 0x0030C990 File Offset: 0x0030AB90
		private Skyfaller SpawnObeliskIncoming(Map map)
		{
			IntVec3 pos;
			if (!this.TryFindCell(out pos, map, this.ObeliskDef))
			{
				return null;
			}
			return SkyfallerMaker.SpawnSkyfaller(ThingDefOf.WarpedObeliskIncoming, ThingMaker.MakeThing(this.ObeliskDef, null), pos, map);
		}

		// Token: 0x0600AA9F RID: 43679 RVA: 0x0030C9C8 File Offset: 0x0030ABC8
		private bool TryFindCell(out IntVec3 cell, Map map, ThingDef obeliskDef)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.WarpedObeliskIncoming, map, this.ObeliskDef.terrainAffordanceNeeded, out cell, 10, default(IntVec3), -1, true, false, false, false, true, true, delegate(IntVec3 x)
			{
				if ((float)x.DistanceToEdge(map) < 20f + (float)map.Size.x * 0.1f)
				{
					return false;
				}
				foreach (IntVec3 c in CellRect.CenteredOn(x, obeliskDef.Size.x, obeliskDef.Size.z))
				{
					if (!c.InBounds(map) || !c.Standable(map) || !c.GetAffordances(map).Contains(obeliskDef.terrainAffordanceNeeded))
					{
						return false;
					}
				}
				return true;
			});
		}
	}
}
