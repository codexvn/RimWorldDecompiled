using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026E9 RID: 9961
	public class RitualObligationTargetWorker_GraveWithTarget : RitualObligationTargetFilter
	{
		// Token: 0x0600DA65 RID: 55909 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_GraveWithTarget()
		{
		}

		// Token: 0x0600DA66 RID: 55910 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_GraveWithTarget(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DA67 RID: 55911 RVA: 0x0040906B File Offset: 0x0040726B
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			Thing thing = map.listerThings.ThingsInGroup(ThingRequestGroup.Grave).FirstOrDefault((Thing t) => ((Building_Grave)t).Corpse == obligation.targetA.Thing);
			if (thing != null)
			{
				yield return thing;
			}
			yield break;
		}

		// Token: 0x0600DA68 RID: 55912 RVA: 0x00409084 File Offset: 0x00407284
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			bool canUse;
			if (target.HasThing)
			{
				Building_Grave building_Grave = target.Thing as Building_Grave;
				if (building_Grave != null)
				{
					if (building_Grave.Corpse != obligation.targetA.Thing)
					{
						Corpse corpse = building_Grave.Corpse;
						canUse = (((corpse != null) ? corpse.InnerPawn : null) == obligation.targetA.Thing);
						goto IL_51;
					}
					canUse = true;
					goto IL_51;
				}
			}
			canUse = false;
			IL_51:
			return canUse;
		}

		// Token: 0x0600DA69 RID: 55913 RVA: 0x004090E8 File Offset: 0x004072E8
		public override bool ObligationTargetsValid(RitualObligation obligation)
		{
			Pawn pawn = obligation.targetA.Thing as Pawn;
			Corpse corpse = (pawn != null) ? pawn.Corpse : null;
			return obligation.targetA.HasThing && (corpse == null || !corpse.Destroyed);
		}

		// Token: 0x0600DA6A RID: 55914 RVA: 0x0040912F File Offset: 0x0040732F
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			if (obligation == null)
			{
				if (Find.IdeoManager.classicMode)
				{
					yield return "RitualTargetGraveInfoAbstractNoIdeo".Translate();
				}
				else
				{
					yield return "RitualTargetGraveInfoAbstract".Translate(this.parent.ideo.Named("IDEO"));
				}
				yield break;
			}
			Pawn pawn = obligation.targetA.Thing as Pawn;
			if (pawn == null)
			{
				pawn = ((Corpse)obligation.targetA.Thing).InnerPawn;
			}
			yield return "RitualTargetGraveInfo".Translate(pawn.Named("PAWN"));
			yield break;
		}

		// Token: 0x0600DA6B RID: 55915 RVA: 0x00409148 File Offset: 0x00407348
		public override string LabelExtraPart(RitualObligation obligation)
		{
			Pawn pawn = obligation.targetA.Thing as Pawn;
			if (pawn != null)
			{
				return pawn.LabelShort;
			}
			return ((Corpse)obligation.targetA.Thing).InnerPawn.LabelShort;
		}
	}
}
