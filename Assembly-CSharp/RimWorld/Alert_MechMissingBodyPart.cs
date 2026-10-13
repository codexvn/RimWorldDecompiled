using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200304D RID: 12365
	public class Alert_MechMissingBodyPart : Alert
	{
		// Token: 0x060115F5 RID: 71157 RVA: 0x004FBEB0 File Offset: 0x004FA0B0
		public Alert_MechMissingBodyPart()
		{
			this.defaultLabel = "AlertMechMissingBodyPart".Translate();
			this.labelPlural = "AlertMechsMissingBodyPart".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x060115F6 RID: 71158 RVA: 0x004FBF00 File Offset: 0x004FA100
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].IsColonyMech && this.HasMissingBodyPart(list[j]))
					{
						this.targets.Add(list[j]);
					}
				}
			}
		}

		// Token: 0x060115F7 RID: 71159 RVA: 0x004FBF88 File Offset: 0x004FA188
		private bool HasMissingBodyPart(Pawn pawn)
		{
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = hediffs[i] as Hediff_MissingPart;
				if (hediff_MissingPart != null && hediffs[i].ageTicks > 240000 && allParts.Contains(hediff_MissingPart.Part))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060115F8 RID: 71160 RVA: 0x004FC001 File Offset: 0x004FA201
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x060115F9 RID: 71161 RVA: 0x004FC014 File Offset: 0x004FA214
		public override string GetLabel()
		{
			if (this.targets.Count != 1)
			{
				return this.labelPlural;
			}
			return this.defaultLabel;
		}

		// Token: 0x060115FA RID: 71162 RVA: 0x004FC034 File Offset: 0x004FA234
		public override TaggedString GetExplanation()
		{
			return "AlertMechMissingBodyPartDesc".Translate() + ":\n" + (from p in this.targets
			select p.LabelCap).ToLineList("  - ", false);
		}

		// Token: 0x0400B3DE RID: 46046
		private const int MinAgeTicks = 240000;

		// Token: 0x0400B3DF RID: 46047
		private List<Pawn> targets = new List<Pawn>();

		// Token: 0x0400B3E0 RID: 46048
		private string labelPlural;
	}
}
