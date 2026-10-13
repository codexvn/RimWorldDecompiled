using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026ED RID: 9965
	public class RitualObligationTargetWorker_AnyEmptyGrave : RitualObligationTargetFilter
	{
		// Token: 0x0600DA7E RID: 55934 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_AnyEmptyGrave()
		{
		}

		// Token: 0x0600DA7F RID: 55935 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_AnyEmptyGrave(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DA80 RID: 55936 RVA: 0x0040943F File Offset: 0x0040763F
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			Thing thing = map.listerThings.ThingsInGroup(ThingRequestGroup.Grave).FirstOrDefault((Thing t) => ((Building_Grave)t).Corpse == null);
			if (thing != null)
			{
				yield return thing;
			}
			yield break;
		}

		// Token: 0x0600DA81 RID: 55937 RVA: 0x00409450 File Offset: 0x00407650
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			bool canUse;
			if (target.HasThing)
			{
				Building_Grave building_Grave = target.Thing as Building_Grave;
				if (building_Grave != null)
				{
					canUse = (building_Grave.Corpse == null);
					goto IL_25;
				}
			}
			canUse = false;
			IL_25:
			return canUse;
		}

		// Token: 0x0600DA82 RID: 55938 RVA: 0x00409488 File Offset: 0x00407688
		public override bool ObligationTargetsValid(RitualObligation obligation)
		{
			Pawn pawn = obligation.targetA.Thing as Pawn;
			Corpse corpse = (pawn != null) ? pawn.Corpse : null;
			return obligation.targetA.HasThing && (corpse == null || !corpse.Destroyed);
		}

		// Token: 0x0600DA83 RID: 55939 RVA: 0x004094CF File Offset: 0x004076CF
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			if (obligation == null)
			{
				yield return "RitualTargetEmptyGraveInfoAbstract".Translate(this.parent.ideo.Named("IDEO"));
				yield break;
			}
			Pawn arg = (Pawn)obligation.targetA.Thing;
			yield return "RitualTargetEmptyGraveInfo".Translate(arg.Named("PAWN"));
			yield break;
		}

		// Token: 0x0600DA84 RID: 55940 RVA: 0x004094E6 File Offset: 0x004076E6
		public override string LabelExtraPart(RitualObligation obligation)
		{
			return ((Pawn)obligation.targetA.Thing).LabelShort;
		}
	}
}
