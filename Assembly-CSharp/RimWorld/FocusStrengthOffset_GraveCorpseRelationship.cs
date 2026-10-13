using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D54 RID: 11604
	public class FocusStrengthOffset_GraveCorpseRelationship : FocusStrengthOffset
	{
		// Token: 0x17002978 RID: 10616
		// (get) Token: 0x060104E0 RID: 66784 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DependsOnPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060104E1 RID: 66785 RVA: 0x004B56BA File Offset: 0x004B38BA
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveCorpseRelatedAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x060104E2 RID: 66786 RVA: 0x004B537B File Offset: 0x004B357B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x060104E3 RID: 66787 RVA: 0x004B56EC File Offset: 0x004B38EC
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			Building_Grave building_Grave = parent as Building_Grave;
			return parent.Spawned && building_Grave != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike && building_Grave.Corpse.InnerPawn.relations.PotentiallyRelatedPawns.Contains(user);
		}
	}
}
