using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001644 RID: 5700
	public class Command_BestowerCeremony : Command
	{
		// Token: 0x0600866C RID: 34412 RVA: 0x002824A4 File Offset: 0x002806A4
		public Command_BestowerCeremony(LordJob_BestowingCeremony job, Pawn bestower, Pawn forPawn, Action<List<Pawn>> action)
		{
			this.bestower = bestower;
			this.forPawn = forPawn;
			this.action = action;
			this.job = job;
			this.defaultLabel = "BeginCeremony".Translate(this.forPawn);
			this.icon = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/BestowCeremony", true);
		}

		// Token: 0x0600866D RID: 34413 RVA: 0x00282508 File Offset: 0x00280708
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!JobDriver_BestowingCeremony.AnalyzeThroneRoom(this.bestower, this.forPawn))
			{
				this.disabledReason = "BestowingCeremonyThroneroomRequirementsNotSatisfiedShort".Translate(this.forPawn.Named("PAWN"), this.forPawn.royalty.GetTitleAwardedWhenUpdating(this.bestower.Faction, this.forPawn.royalty.GetFavor(this.bestower.Faction)).label.Named("TITLE"));
				this.disabled = true;
			}
			else if (!this.job.GetSpot().IsValid)
			{
				this.disabledReason = "MessageBestowerUnreachable".Translate();
				this.disabled = true;
			}
			else
			{
				Lord lord = this.forPawn.GetLord();
				if (lord != null)
				{
					if (lord.LordJob is LordJob_Ritual)
					{
						this.disabledReason = "CantStartRitualTargetIsAlreadyInRitual".Translate(this.forPawn.LabelShort);
						this.disabled = true;
					}
					else
					{
						this.disabledReason = "MessageBestowingTargetIsBusy".Translate(this.forPawn.LabelShort);
						this.disabled = true;
					}
				}
			}
			return base.GizmoOnGUI(topLeft, maxWidth, parms);
		}

		// Token: 0x0600866E RID: 34414 RVA: 0x00282650 File Offset: 0x00280850
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			Find.WindowStack.Add(new Dialog_BeginRitual("RitualBestowingCeremony".Translate(), null, this.job.targetSpot.ToTargetInfo(this.bestower.Map), this.bestower.Map, delegate(RitualRoleAssignments assignments)
			{
				this.action((from p in assignments.Participants
				where p != this.bestower
				select p).ToList<Pawn>());
				return true;
			}, this.bestower, null, delegate(Pawn pawn, bool voluntary, bool allowOtherIdeos)
			{
				Lord lord = pawn.GetLord();
				return !(((lord != null) ? lord.LordJob : null) is LordJob_Ritual) && !pawn.IsSubhuman && !pawn.IsPrisonerOfColony && !pawn.RaceProps.Animal;
			}, "Begin".Translate(), new List<Pawn>
			{
				this.bestower,
				this.forPawn
			}, null, RitualOutcomeEffectDefOf.BestowingCeremony, null, null));
		}

		// Token: 0x040057D5 RID: 22485
		private Pawn bestower;

		// Token: 0x040057D6 RID: 22486
		private Pawn forPawn;

		// Token: 0x040057D7 RID: 22487
		private Action<List<Pawn>> action;

		// Token: 0x040057D8 RID: 22488
		private LordJob_BestowingCeremony job;
	}
}
