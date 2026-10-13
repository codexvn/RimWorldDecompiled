using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FD3 RID: 12243
	public class Alert_InsufficientContainmentStrength : Alert_Critical
	{
		// Token: 0x06011411 RID: 70673 RVA: 0x004F1B8D File Offset: 0x004EFD8D
		public Alert_InsufficientContainmentStrength()
		{
			this.defaultLabel = "Alert_InsufficientContainment".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x06011412 RID: 70674 RVA: 0x004F1BBC File Offset: 0x004EFDBC
		public override AlertReport GetReport()
		{
			if (Find.CurrentMap == null)
			{
				return false;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x06011413 RID: 70675 RVA: 0x004F1BE0 File Offset: 0x004EFDE0
		public override TaggedString GetExplanation()
		{
			string arg = (from x in this.targets
			select x.NameShortColored.Resolve()).ToLineList("  - ", true);
			return string.Format("{0}:\n{1}\n\n{2}", "Alert_InsufficientContainmentDesc".Translate(), arg, "Alert_InsufficientContainmentDescAppended".Translate());
		}

		// Token: 0x06011414 RID: 70676 RVA: 0x004F1C54 File Offset: 0x004EFE54
		private void GetTargets()
		{
			this.targets.Clear();
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder);
			for (int i = 0; i < list.Count; i++)
			{
				Building_HoldingPlatform building_HoldingPlatform = list[i] as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.HeldPawn != null && !building_HoldingPlatform.SafelyContains(building_HoldingPlatform.HeldPawn) && ContainmentUtility.InitiateEscapeMtbDays(building_HoldingPlatform.HeldPawn, null) > 0f)
				{
					this.targets.Add(building_HoldingPlatform.HeldPawn);
				}
			}
		}

		// Token: 0x0400B302 RID: 45826
		private List<Pawn> targets = new List<Pawn>();
	}
}
