using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016A9 RID: 5801
	public class PsychicRitualDef_SummonPitGate : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x060088BB RID: 35003 RVA: 0x0028C963 File Offset: 0x0028AB63
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SummonPitGate(this.InvokerRole, this.combatPointMultiplierFromQualityRange));
			return list;
		}

		// Token: 0x060088BC RID: 35004 RVA: 0x0028C984 File Offset: 0x0028AB84
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			foreach (string text in base.BlockingIssues(assignments, map))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (map.listerThings.ThingsOfDef(ThingDefOf.PitGate).Count > 0 || map.listerThings.ThingsOfDef(ThingDefOf.PitGateSpawner).Count > 0)
			{
				yield return "PitGateAlreadyExists".Translate();
			}
			yield break;
			yield break;
		}

		// Token: 0x04005931 RID: 22833
		private FloatRange combatPointMultiplierFromQualityRange;
	}
}
