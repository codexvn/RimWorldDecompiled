using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001639 RID: 5689
	public class LordToil_BestowingCeremony_Wait : LordToil_Wait
	{
		// Token: 0x0600860D RID: 34317 RVA: 0x0028087E File Offset: 0x0027EA7E
		public LordToil_BestowingCeremony_Wait(Pawn target, Pawn bestower) : base(true)
		{
			this.target = target;
			this.bestower = bestower;
		}

		// Token: 0x0600860E RID: 34318 RVA: 0x00280898 File Offset: 0x0027EA98
		public override void Init()
		{
			Messages.Message("MessageBestowerWaiting".Translate(this.target.Named("TARGET"), this.lord.ownedPawns[0].Named("BESTOWER")), new LookTargets(new Pawn[]
			{
				this.target,
				this.lord.ownedPawns[0]
			}), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600860F RID: 34319 RVA: 0x00280912 File Offset: 0x0027EB12
		protected override void DecoratePawnDuty(PawnDuty duty)
		{
			duty.focus = this.target;
		}

		// Token: 0x06008610 RID: 34320 RVA: 0x00280925 File Offset: 0x0027EB25
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
		}

		// Token: 0x06008611 RID: 34321 RVA: 0x0028093A File Offset: 0x0027EB3A
		public override IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			if (p == this.bestower)
			{
				LordJob_BestowingCeremony job = (LordJob_BestowingCeremony)this.lord.LordJob;
				yield return new Command_BestowerCeremony(job, this.bestower, this.target, new Action<List<Pawn>>(this.StartRitual));
			}
			yield break;
		}

		// Token: 0x06008612 RID: 34322 RVA: 0x00280951 File Offset: 0x0027EB51
		public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn target, Pawn forPawn)
		{
			LordJob_BestowingCeremony lordJob = (LordJob_BestowingCeremony)this.lord.LordJob;
			if (target == this.bestower)
			{
				Func<Pawn, bool> <>9__3;
				Dialog_BeginRitual.ActionCallback <>9__1;
				FloatMenuOption floatMenuOption = new FloatMenuOption("BeginRitual".Translate("RitualBestowingCeremony".Translate()), delegate()
				{
					WindowStack windowStack = Find.WindowStack;
					string ritualLabel = "RitualBestowingCeremony".Translate();
					Precept_Ritual ritual = null;
					TargetInfo targetInfo = lordJob.targetSpot.ToTargetInfo(this.bestower.Map);
					Map map = this.bestower.Map;
					Dialog_BeginRitual.ActionCallback action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(RitualRoleAssignments assignments)
						{
							LordToil_BestowingCeremony_Wait <>4__this = this;
							IEnumerable<Pawn> participants = assignments.Participants;
							Func<Pawn, bool> predicate;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((Pawn p) => p != this.bestower));
							}
							<>4__this.StartRitual(participants.Where(predicate).ToList<Pawn>());
							return true;
						});
					}
					windowStack.Add(new Dialog_BeginRitual(ritualLabel, ritual, targetInfo, map, action, this.bestower, null, delegate(Pawn pawn, bool voluntary, bool allowOtherIdeos)
					{
						Lord lord = pawn.GetLord();
						return !(((lord != null) ? lord.LordJob : null) is LordJob_Ritual) && !pawn.IsSubhuman && !pawn.IsPrisonerOfColony && !pawn.RaceProps.Animal;
					}, "Begin".Translate(), new List<Pawn>
					{
						this.bestower,
						this.target
					}, null, RitualOutcomeEffectDefOf.BestowingCeremony, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				if (!LordJob_BestowingCeremony.TryGetUsableSpotAdjacentToBestower(this.bestower).IsValid)
				{
					floatMenuOption.Disabled = true;
					FloatMenuOption floatMenuOption2 = floatMenuOption;
					floatMenuOption2.Label += " (" + "BestowingSpotUnavailable".Translate() + ")";
				}
				yield return floatMenuOption;
			}
			yield break;
		}

		// Token: 0x06008613 RID: 34323 RVA: 0x00280968 File Offset: 0x0027EB68
		private void StartRitual(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Lord lord = pawn.GetLord();
				if (((lord != null) ? lord.LordJob : null) is LordJob_VoluntarilyJoinable)
				{
					pawn.GetLord().Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
				}
			}
			this.lord.AddPawns(pawns, true);
			((LordJob_BestowingCeremony)this.lord.LordJob).colonistParticipants.AddRange(pawns);
			this.lord.ReceiveMemo(LordJob_BestowingCeremony.MemoCeremonyStarted);
			foreach (Pawn pawn2 in pawns)
			{
				if (pawn2.drafter != null)
				{
					pawn2.drafter.Drafted = false;
				}
				if (!pawn2.Awake())
				{
					RestUtility.WakeUp(pawn2, true);
				}
			}
		}

		// Token: 0x0400579B RID: 22427
		public Pawn target;

		// Token: 0x0400579C RID: 22428
		public Pawn bestower;
	}
}
