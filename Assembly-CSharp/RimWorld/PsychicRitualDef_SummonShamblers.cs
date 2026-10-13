using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016AB RID: 5803
	public class PsychicRitualDef_SummonShamblers : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x060088C8 RID: 35016 RVA: 0x0028CB9F File Offset: 0x0028AD9F
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SummonShamblers(this.InvokerRole, this.combatPointsFromQualityRange));
			return list;
		}

		// Token: 0x060088C9 RID: 35017 RVA: 0x0028CBC0 File Offset: 0x0028ADC0
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			foreach (string text in base.BlockingIssues(assignments, map))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!this.shamblersAllowed)
			{
				yield return "NoNearbyShamblers".Translate();
			}
			yield break;
			yield break;
		}

		// Token: 0x060088CA RID: 35018 RVA: 0x0028CBE0 File Offset: 0x0028ADE0
		public override void InitializeCast(Map map)
		{
			base.InitializeCast(map);
			int num = 0;
			bool flag = false;
			while (!flag && num < 5)
			{
				IntVec3 root;
				RCellFinder.TryFindRandomPawnEntryCell(out root, map, CellFinder.EdgeRoadChance_Hostile, false, null);
				IntVec3 intVec;
				flag = RCellFinder.TryFindTravelDestFrom(root, map, out intVec);
				num++;
			}
			this.shamblersAllowed = (map.Biome != BiomeDefOf.Undercave && flag);
		}

		// Token: 0x0400593B RID: 22843
		private FloatRange combatPointsFromQualityRange;

		// Token: 0x0400593C RID: 22844
		private bool shamblersAllowed;
	}
}
