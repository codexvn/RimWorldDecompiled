using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002284 RID: 8836
	public class CompAbilityEffect_StartAnimaLinking : CompAbilityEffect_StartRitual
	{
		// Token: 0x17001D6A RID: 7530
		// (get) Token: 0x0600C074 RID: 49268 RVA: 0x0037BD40 File Offset: 0x00379F40
		public override bool ShouldHideGizmo
		{
			get
			{
				return this.parent.pawn.GetPsylinkLevel() >= this.parent.pawn.GetMaxPsylinkLevel() || !MeditationFocusDefOf.Natural.CanPawnUse(this.parent.pawn) || !this.parent.pawn.psychicEntropy.IsPsychicallySensitive;
			}
		}

		// Token: 0x0600C075 RID: 49269 RVA: 0x0037BDA4 File Offset: 0x00379FA4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = this.parent.pawn;
			Thing thing = target.Thing;
			CompPsylinkable compPsylinkable = (thing != null) ? thing.TryGetComp<CompPsylinkable>() : null;
			if (compPsylinkable == null)
			{
				return false;
			}
			int requiredPlantCount = compPsylinkable.GetRequiredPlantCount(pawn);
			if (requiredPlantCount == -1)
			{
				return false;
			}
			if (!pawn.CanReserve(target.Thing, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(target.Thing, pawn, null);
				if (throwMessages)
				{
					Messages.Message((pawn2 == null) ? "Reserved".Translate() : "ReservedBy".Translate(pawn.LabelShort, pawn2), target.Thing, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			CompSpawnSubplant compSpawnSubplant = target.Thing.TryGetComp<CompSpawnSubplant>();
			if (compSpawnSubplant.SubplantsForReading.Count < requiredPlantCount)
			{
				if (throwMessages)
				{
					Messages.Message("BeginLinkingRitualNeedSubplants".Translate(requiredPlantCount.ToString(), compSpawnSubplant.Props.subplant.label, compSpawnSubplant.SubplantsForReading.Count.ToString()), target.Thing, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			LocalTargetInfo localTargetInfo;
			if (!compPsylinkable.TryFindLinkSpot(pawn, out localTargetInfo) && throwMessages)
			{
				Messages.Message("BeginLinkingRitualNeedLinkSpot".Translate(), target.Thing, MessageTypeDefOf.RejectInput, false);
			}
			return base.Valid(target, throwMessages);
		}

		// Token: 0x0600C076 RID: 49270 RVA: 0x0037BF28 File Offset: 0x0037A128
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			TargetInfo targetInfo = TargetInfo.Invalid;
			if (base.Ritual.targetFilter != null)
			{
				targetInfo = base.Ritual.targetFilter.BestTarget(this.parent.pawn, target.ToTargetInfo(this.parent.pawn.MapHeld));
			}
			return base.Ritual.GetRitualBeginWindow(targetInfo, null, confirmAction, null, null, this.parent.pawn);
		}
	}
}
