using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003043 RID: 12355
	public class Alert_SubjectHasNowOverseer : Alert
	{
		// Token: 0x17002CF7 RID: 11511
		// (get) Token: 0x060115CB RID: 71115 RVA: 0x004FB30C File Offset: 0x004F950C
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.tmpMechsCanGoFeralNow.Clear();
				this.tmpMechsCanGoFeralSoon.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
				{
					if (pawn.IsColonyMechRequiringMechanitor())
					{
						this.targets.Add(pawn);
						if (pawn.OverseerSubject.DelayUntilFeralCheckTicks > 0)
						{
							this.tmpMechsCanGoFeralSoon.Add(pawn);
						}
						else
						{
							this.tmpMechsCanGoFeralNow.Add(pawn);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x060115CC RID: 71116 RVA: 0x004FB3C4 File Offset: 0x004F95C4
		public Alert_SubjectHasNowOverseer()
		{
			this.defaultLabel = "AlertMechLacksOverseer".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x060115CD RID: 71117 RVA: 0x004FB414 File Offset: 0x004F9614
		public override TaggedString GetExplanation()
		{
			TaggedString taggedString = "AlertMechLacksOverseerDesc".Translate(Faction.OfMechanoids);
			if (this.tmpMechsCanGoFeralSoon.Count > 0)
			{
				taggedString += "\n\n" + "AlertMechLacksOverseerUncontrolled".Translate() + ":\n" + (from m in this.tmpMechsCanGoFeralSoon
				select m.LabelCap).ToLineList("  - ", false);
			}
			if (this.tmpMechsCanGoFeralNow.Count > 0)
			{
				taggedString += "\n\n" + "AlertMechLacksOverseerMayGoFeral".Translate() + ":\n" + (from m in this.tmpMechsCanGoFeralNow
				select m.LabelCap).ToLineList("  - ", false);
			}
			return taggedString;
		}

		// Token: 0x060115CE RID: 71118 RVA: 0x004FB511 File Offset: 0x004F9711
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0400B3CA RID: 46026
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B3CB RID: 46027
		private List<Pawn> tmpMechsCanGoFeralNow = new List<Pawn>();

		// Token: 0x0400B3CC RID: 46028
		private List<Pawn> tmpMechsCanGoFeralSoon = new List<Pawn>();
	}
}
