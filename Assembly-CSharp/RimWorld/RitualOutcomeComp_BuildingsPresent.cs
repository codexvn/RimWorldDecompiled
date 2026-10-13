using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200273E RID: 10046
	public abstract class RitualOutcomeComp_BuildingsPresent : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x0600DC79 RID: 56441 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual int RequiredAmount(RitualRoleAssignments assignments)
		{
			return 1;
		}

		// Token: 0x0600DC7A RID: 56442 RVA: 0x0040E517 File Offset: 0x0040C717
		protected virtual string LabelForPredictedOutcomeDesc(Precept_Ritual ritual)
		{
			return this.LabelForDesc;
		}

		// Token: 0x0600DC7B RID: 56443
		protected abstract Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual);

		// Token: 0x0600DC7C RID: 56444 RVA: 0x0040E520 File Offset: 0x0040C720
		protected virtual int CountAvailable(Precept_Ritual ritual, TargetInfo ritualTarget)
		{
			RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.ritualTarget = ritualTarget;
			CS$<>8__locals1.ritual = ritual;
			int result;
			try
			{
				bool usesLectern = CS$<>8__locals1.ritual.behavior.def.UsesLectern;
				RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_1 CS$<>8__locals2;
				CS$<>8__locals2.lecternPos = CS$<>8__locals1.ritual.behavior.def.FirstLecternPosition;
				CS$<>8__locals2.allLecterns = (usesLectern ? CS$<>8__locals1.ritualTarget.Map.listerThings.ThingsOfDef(ThingDefOf.Lectern) : null);
				int num = 0;
				if (GatheringsUtility.UseWholeRoomAsGatheringArea(CS$<>8__locals1.ritualTarget.Cell, CS$<>8__locals1.ritualTarget.Map))
				{
					using (IEnumerator<IntVec3> enumerator = CS$<>8__locals1.ritualTarget.Cell.GetRoom(CS$<>8__locals1.ritualTarget.Map).Cells.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntVec3 cell = enumerator.Current;
							if (this.<CountAvailable>g__Check|4_0(cell, ref CS$<>8__locals1, ref CS$<>8__locals2))
							{
								num++;
							}
						}
						goto IL_144;
					}
				}
				foreach (IntVec3 cell2 in CellRect.CenteredOn(CS$<>8__locals1.ritualTarget.Cell, 18))
				{
					if (this.<CountAvailable>g__Check|4_0(cell2, ref CS$<>8__locals1, ref CS$<>8__locals2))
					{
						num++;
					}
				}
				IL_144:
				result = num;
			}
			finally
			{
				RitualOutcomeComp_BuildingsPresent.tmpThingsAlreadyCounted.Clear();
			}
			return result;
		}

		// Token: 0x0600DC7D RID: 56445 RVA: 0x0040E6D0 File Offset: 0x0040C8D0
		private float CalcQualityOffset(int available, int required)
		{
			return this.qualityOffset * Mathf.Clamp01((float)available / (float)required);
		}

		// Token: 0x0600DC7E RID: 56446 RVA: 0x0040E6E3 File Offset: 0x0040C8E3
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.CalcQualityOffset(this.CountAvailable(ritual.Ritual, ritual.selectedTarget), this.RequiredAmount(ritual.assignments));
		}

		// Token: 0x0600DC7F RID: 56447 RVA: 0x0040E709 File Offset: 0x0040C909
		public override bool Applies(LordJob_Ritual ritual)
		{
			return this.CountAvailable(ritual.Ritual, ritual.selectedTarget) > 0;
		}

		// Token: 0x0600DC80 RID: 56448 RVA: 0x0040E720 File Offset: 0x0040C920
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int num = this.RequiredAmount(assignments);
			if (num <= 0)
			{
				return null;
			}
			int num2 = this.CountAvailable(ritual, ritualTarget);
			string count = null;
			if (num != 1)
			{
				count = num2.ToString() + " / " + num.ToString();
			}
			float quality = this.CalcQualityOffset(num2, num);
			return new QualityFactor
			{
				label = this.LabelForPredictedOutcomeDesc(ritual),
				qualityChange = this.ExpectedOffsetDesc(num2 > 0, quality),
				present = (num2 > 0),
				quality = quality,
				positive = (num2 > 0),
				priority = 1f,
				count = count
			};
		}

		// Token: 0x0600DC83 RID: 56451 RVA: 0x0040E7CC File Offset: 0x0040C9CC
		[CompilerGenerated]
		private bool <CountAvailable>g__Check|4_0(IntVec3 cell, ref RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_0 A_2, ref RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_1 A_3)
		{
			Thing thing = this.LookForBuilding(cell, A_2.ritualTarget.Map, A_2.ritual);
			if (thing != null && GatheringsUtility.InGatheringArea(cell, A_2.ritualTarget.Cell, A_2.ritualTarget.Map))
			{
				IntVec3? intVec = null;
				if (A_3.allLecterns != null)
				{
					foreach (Thing thing2 in A_3.allLecterns)
					{
						if (GatheringsUtility.InGatheringArea(thing2.Position, A_2.ritualTarget.Cell, A_2.ritualTarget.Map) && A_3.lecternPos.IsUsableThing(thing2, A_2.ritualTarget.Cell, A_2.ritualTarget))
						{
							intVec = new IntVec3?(thing2.Position);
							break;
						}
					}
				}
				if (intVec == null)
				{
					intVec = new IntVec3?(A_2.ritualTarget.Thing.Position);
				}
				if (thing.def.building == null || !thing.def.building.isSittable || SpectatorCellFinder.CorrectlyRotatedChairAt(thing.Position, thing.Map, CellRect.CenteredOn(intVec.Value, 1)))
				{
					return RitualOutcomeComp_BuildingsPresent.tmpThingsAlreadyCounted.Add(thing);
				}
			}
			return false;
		}

		// Token: 0x040094C9 RID: 38089
		private static HashSet<Thing> tmpThingsAlreadyCounted = new HashSet<Thing>();
	}
}
