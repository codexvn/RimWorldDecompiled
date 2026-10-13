using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003031 RID: 12337
	public class Alert_AnimalRoaming : Alert
	{
		// Token: 0x06011582 RID: 71042 RVA: 0x004F9B2E File Offset: 0x004F7D2E
		public Alert_AnimalRoaming()
		{
			this.defaultLabel = "AlertAnimalIsRoaming".Translate();
		}

		// Token: 0x06011583 RID: 71043 RVA: 0x004F9B64 File Offset: 0x004F7D64
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.pawnNames.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn.MentalStateDef == MentalStateDefOf.Roaming)
				{
					this.targets.Add(pawn);
					this.pawnNames.Add(pawn.NameShortColored.Resolve());
				}
			}
		}

		// Token: 0x06011584 RID: 71044 RVA: 0x004F9C04 File Offset: 0x004F7E04
		public override TaggedString GetExplanation()
		{
			return "AlertAnimalIsRoamingDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x06011585 RID: 71045 RVA: 0x004F9C25 File Offset: 0x004F7E25
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3A1 RID: 45985
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B3A2 RID: 45986
		private List<string> pawnNames = new List<string>();
	}
}
