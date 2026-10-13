using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200302D RID: 12333
	public class Alert_AnimalFilth : Alert
	{
		// Token: 0x06011572 RID: 71026 RVA: 0x004F9325 File Offset: 0x004F7525
		public Alert_AnimalFilth()
		{
			this.defaultLabel = "AlertAnimalFilth".Translate();
		}

		// Token: 0x06011573 RID: 71027 RVA: 0x004F9358 File Offset: 0x004F7558
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.pawnEntries.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn.HostFaction == null && !pawn.RaceProps.Humanlike && (!ModsConfig.AnomalyActive || !pawn.IsShambler) && pawn.GetStatValue(StatDefOf.FilthRate, true, -1) >= 4f)
				{
					IntVec3 position = pawn.Position;
					Map map = pawn.Map;
					TerrainDef terrain = position.GetTerrain(map);
					if (terrain.GetStatValueAbstract(StatDefOf.FilthMultiplier, null) > 0.5f && FilthMaker.TerrainAcceptsFilth(terrain, ThingDefOf.Filth_AnimalFilth, FilthSourceFlags.Pawn) && position.GetRoof(map) != null)
					{
						Room room = position.GetRoom(map);
						if (room != null && !room.Fogged && !room.TouchesMapEdge && !room.IsDoorway && pawn.Map.areaManager.Home[pawn.Position])
						{
							this.targets.Add(pawn);
							this.pawnEntries.Add(pawn.NameShortColored.Resolve());
						}
					}
				}
			}
		}

		// Token: 0x06011574 RID: 71028 RVA: 0x004F94CC File Offset: 0x004F76CC
		public override TaggedString GetExplanation()
		{
			return "AlertAnimalFilthDesc".Translate(this.pawnEntries.ToLineList("  - "));
		}

		// Token: 0x06011575 RID: 71029 RVA: 0x004F94ED File Offset: 0x004F76ED
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B398 RID: 45976
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B399 RID: 45977
		private List<string> pawnEntries = new List<string>();

		// Token: 0x0400B39A RID: 45978
		private const float MinFilthRate = 4f;

		// Token: 0x0400B39B RID: 45979
		private const float MinFilthMultiplier = 0.5f;
	}
}
