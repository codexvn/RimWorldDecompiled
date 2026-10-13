using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002735 RID: 10037
	public class RitualOutcomeComp_BuildingOfDefUsed : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x170021BE RID: 8638
		// (get) Token: 0x0600DC4C RID: 56396 RVA: 0x0040DD7E File Offset: 0x0040BF7E
		protected override string LabelForDesc
		{
			get
			{
				return "Used".Translate(this.def.LabelCap);
			}
		}

		// Token: 0x0600DC4D RID: 56397 RVA: 0x0040DD9F File Offset: 0x0040BF9F
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.usedThings.Any((Thing t) => t.def == this.def);
		}

		// Token: 0x0600DC4E RID: 56398 RVA: 0x0040DDB8 File Offset: 0x0040BFB8
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			bool flag = false;
			foreach (Thing thing in ritualTarget.Map.listerThings.ThingsOfDef(this.def))
			{
				if (GatheringsUtility.InGatheringArea(thing.Position, ritualTarget.Cell, ritualTarget.Map) && (this.maxHorDistFromTarget == 0 || thing.Position.InHorDistOf(ritualTarget.Cell, (float)this.maxHorDistFromTarget)))
				{
					flag = true;
					break;
				}
			}
			return new QualityFactor
			{
				label = this.def.LabelCap,
				present = flag,
				qualityChange = this.ExpectedOffsetDesc(flag, -1f),
				quality = (flag ? this.qualityOffset : 0f),
				positive = flag,
				priority = 1f
			};
		}

		// Token: 0x040094BF RID: 38079
		public ThingDef def;

		// Token: 0x040094C0 RID: 38080
		public int maxHorDistFromTarget;
	}
}
