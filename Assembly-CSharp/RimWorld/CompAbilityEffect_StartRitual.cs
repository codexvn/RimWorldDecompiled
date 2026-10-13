using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002288 RID: 8840
	public class CompAbilityEffect_StartRitual : CompAbilityEffect
	{
		// Token: 0x17001D6C RID: 7532
		// (get) Token: 0x0600C07D RID: 49277 RVA: 0x0037C015 File Offset: 0x0037A215
		public new CompProperties_AbilityStartRitual Props
		{
			get
			{
				return (CompProperties_AbilityStartRitual)this.props;
			}
		}

		// Token: 0x17001D6D RID: 7533
		// (get) Token: 0x0600C07E RID: 49278 RVA: 0x0037C024 File Offset: 0x0037A224
		public Precept_Ritual Ritual
		{
			get
			{
				if (this.ritualCached == null && this.parent.pawn.Ideo != null)
				{
					for (int i = 0; i < this.parent.pawn.Ideo.PreceptsListForReading.Count; i++)
					{
						if (this.parent.pawn.Ideo.PreceptsListForReading[i].def == this.Props.ritualDef)
						{
							this.ritualCached = (Precept_Ritual)this.parent.pawn.Ideo.PreceptsListForReading[i];
							break;
						}
					}
				}
				return this.ritualCached;
			}
		}

		// Token: 0x0600C07F RID: 49279 RVA: 0x000026EA File Offset: 0x000008EA
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
		}

		// Token: 0x0600C080 RID: 49280 RVA: 0x0037C0D0 File Offset: 0x0037A2D0
		public override bool GizmoDisabled(out string reason)
		{
			Pawn initiator = this.GetInitiator();
			if (initiator == null)
			{
				reason = "AbilityStartRitualNoMembersOfIdeo".Translate(this.Ritual.ideo.memberName);
				return true;
			}
			if (ModsConfig.BiotechActive && !this.Props.allowedForChild && initiator.DevelopmentalStage.Juvenile())
			{
				reason = "AbilityStartRitualDisabledChild".Translate();
				return true;
			}
			if (initiator.Spawned && GatheringsUtility.AnyLordJobPreventsNewRituals(initiator.Map))
			{
				reason = "AbilitySpeechDisabledAnotherGatheringInProgress".Translate();
				return true;
			}
			string text;
			if (this.Ritual != null && this.Ritual.targetFilter != null && !this.Ritual.targetFilter.CanStart(initiator, TargetInfo.Invalid, out text))
			{
				reason = text;
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x0600C081 RID: 49281 RVA: 0x0037C1A8 File Offset: 0x0037A3A8
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			TargetInfo targetInfo = TargetInfo.Invalid;
			Pawn initiator = this.GetInitiator();
			if (this.Ritual.targetFilter != null)
			{
				targetInfo = this.Ritual.targetFilter.BestTarget(initiator, target.ToTargetInfo(initiator.Map));
			}
			return this.Ritual.GetRitualBeginWindow(targetInfo, null, confirmAction, this.parent.pawn, null, null);
		}

		// Token: 0x0600C082 RID: 49282 RVA: 0x0037C210 File Offset: 0x0037A410
		public Pawn GetInitiator()
		{
			if (this.parent.pawn != null && this.parent.pawn.Map == Find.CurrentMap)
			{
				return this.parent.pawn;
			}
			return Find.CurrentMap.mapPawns.FreeColonistsSpawned.FirstOrDefault((Pawn c) => c.Ideo == this.Ritual.ideo);
		}

		// Token: 0x040083D7 RID: 33751
		private Precept_Ritual ritualCached;
	}
}
