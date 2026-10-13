using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02001085 RID: 4229
	public class PsychicRitualToil_SummonFleshbeastsCultist : PsychicRitualToil
	{
		// Token: 0x0600677F RID: 26495 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_SummonFleshbeastsCultist()
		{
		}

		// Token: 0x06006780 RID: 26496 RVA: 0x0020C988 File Offset: 0x0020AB88
		public PsychicRitualToil_SummonFleshbeastsCultist(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x06006781 RID: 26497 RVA: 0x0020C997 File Offset: 0x0020AB97
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			this.ApplyOutcome(psychicRitual);
		}

		// Token: 0x06006782 RID: 26498 RVA: 0x0020C9A8 File Offset: 0x0020ABA8
		private void ApplyOutcome(PsychicRitual psychicRitual)
		{
			float x2;
			psychicRitual.def.CalculateMaxPower(psychicRitual.assignments, null, out x2);
			float num = psychicRitual.power = ((PsychicRitualDef_SummonFleshbeasts)psychicRitual.def).fleshbeastPointsFromThreatPointsCurve.Evaluate(x2);
			int num2 = 0;
			List<Thing> list = new List<Thing>();
			Predicate<IntVec3> <>9__0;
			while (num > 0f)
			{
				IntVec3 cell;
				if (!this.TryFindBurrowCell(psychicRitual.Map, out cell))
				{
					int minEdgeDistance = 10;
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((IntVec3 x) => x.Walkable(psychicRitual.Map)));
					}
					if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(minEdgeDistance, validator, psychicRitual.Map, out cell))
					{
						num2++;
						if (num2 > 200)
						{
							break;
						}
						continue;
					}
				}
				float num3 = Mathf.Min(num, 500f);
				num -= num3;
				list.Add(FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(cell, psychicRitual.Map, num3, PsychicRitualToil_SummonFleshbeastsCultist.PitBurrowEmergenceDelayRangeTicks, PsychicRitualToil_SummonFleshbeastsCultist.FleshbeastSpawnDelayTicks, true));
				num2++;
				if (num2 > 200)
				{
					break;
				}
			}
			if (list.Count > 0 && !psychicRitual.def.letterAICompleteLabel.NullOrEmpty() && !psychicRitual.def.letterAICompleteText.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(psychicRitual.def.letterAICompleteLabel, psychicRitual.def.letterAICompleteText, LetterDefOf.ThreatBig, list, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06006783 RID: 26499 RVA: 0x0020CB44 File Offset: 0x0020AD44
		private bool TryFindBurrowCell(Map map, out IntVec3 cell)
		{
			PsychicRitualToil_SummonFleshbeastsCultist.tmpPawns.Clear();
			PsychicRitualToil_SummonFleshbeastsCultist.tmpPawns.AddRange(map.mapPawns.FreeColonistsSpawned);
			PsychicRitualToil_SummonFleshbeastsCultist.tmpPawns.Shuffle<Pawn>();
			LargeBuildingSpawnParms parms = PsychicRitualToil_SummonFleshbeastsCultist.BurrowSpawnParms.ForThing(ThingDefOf.PitBurrow);
			using (List<Pawn>.Enumerator enumerator = PsychicRitualToil_SummonFleshbeastsCultist.tmpPawns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (LargeBuildingCellFinder.TryFindCellNear(enumerator.Current.Position, map, 6, parms, out cell, false))
					{
						return true;
					}
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06006784 RID: 26500 RVA: 0x0020CBF0 File Offset: 0x0020ADF0
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			foreach (Pawn pawn in psychicRitual.assignments.AllAssignedPawns)
			{
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Idle, null, null, null, null, 0f, null);
			}
		}

		// Token: 0x06006785 RID: 26501 RVA: 0x0020CC74 File Offset: 0x0020AE74
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x040049BD RID: 18877
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x040049BE RID: 18878
		private static readonly IntRange FleshbeastSpawnDelayTicks = new IntRange(180, 180);

		// Token: 0x040049BF RID: 18879
		private static readonly IntRange PitBurrowEmergenceDelayRangeTicks = new IntRange(420, 420);

		// Token: 0x040049C0 RID: 18880
		private const int MaxIterationsGuard = 200;

		// Token: 0x040049C1 RID: 18881
		private static readonly LargeBuildingSpawnParms BurrowSpawnParms = new LargeBuildingSpawnParms
		{
			maxDistanceToColonyBuilding = -1f,
			minDistToEdge = 10,
			canSpawnOnImpassable = false
		};

		// Token: 0x040049C2 RID: 18882
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
