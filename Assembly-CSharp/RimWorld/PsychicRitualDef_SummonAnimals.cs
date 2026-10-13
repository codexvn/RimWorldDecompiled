using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016A5 RID: 5797
	public class PsychicRitualDef_SummonAnimals : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x060088A7 RID: 34983 RVA: 0x0028C661 File Offset: 0x0028A861
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SummonAnimals(this.InvokerRole));
			return list;
		}

		// Token: 0x060088A8 RID: 34984 RVA: 0x0028C67C File Offset: 0x0028A87C
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(this.manhunterSpawnChanceFromQualityCurve.Evaluate(qualityRange.min).ToStringPercent());
		}

		// Token: 0x060088A9 RID: 34985 RVA: 0x0028C6A4 File Offset: 0x0028A8A4
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			foreach (string text in base.BlockingIssues(assignments, map))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!this.animalsAllowed)
			{
				yield return "NotEnoughAnimals".Translate();
			}
			yield break;
			yield break;
		}

		// Token: 0x060088AA RID: 34986 RVA: 0x0028C6C4 File Offset: 0x0028A8C4
		public override void InitializeCast(Map map)
		{
			base.InitializeCast(map);
			IntVec3 intVec;
			this.animalsAllowed = (map.Biome.AllWildAnimals.EnumerableCount() > 0 && map.Biome.wildAnimalsCanWanderInto && RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null));
		}

		// Token: 0x04005924 RID: 22820
		public SimpleCurve manhunterSpawnChanceFromQualityCurve;

		// Token: 0x04005925 RID: 22821
		private bool animalsAllowed;
	}
}
