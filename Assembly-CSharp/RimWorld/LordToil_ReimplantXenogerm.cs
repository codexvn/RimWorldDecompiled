using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001715 RID: 5909
	public class LordToil_ReimplantXenogerm : LordToil
	{
		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x06008B26 RID: 35622 RVA: 0x0029757C File Offset: 0x0029577C
		public override bool ShouldFail
		{
			get
			{
				return this.Data.target == null;
			}
		}

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x06008B27 RID: 35623 RVA: 0x0029758C File Offset: 0x0029578C
		public LordToilData_ReimplantXenogerm Data
		{
			get
			{
				return (LordToilData_ReimplantXenogerm)this.data;
			}
		}

		// Token: 0x06008B28 RID: 35624 RVA: 0x00297599 File Offset: 0x00295799
		public LordToil_ReimplantXenogerm(IntVec3 spot)
		{
			this.data = new LordToilData_ReimplantXenogerm
			{
				waitSpot = spot
			};
		}

		// Token: 0x06008B29 RID: 35625 RVA: 0x002975B3 File Offset: 0x002957B3
		public override void Init()
		{
			this.SetTarget();
		}

		// Token: 0x06008B2A RID: 35626 RVA: 0x002975BC File Offset: 0x002957BC
		private void SetTarget()
		{
			LordToilData_ReimplantXenogerm data = this.Data;
			if (!this.lord.ownedPawns.TryRandomElementByWeight((Pawn x) => (float)((!x.Destroyed && !x.Downed && !x.health.hediffSet.HasHediff(HediffDefOf.XenogermReplicating, false)) ? 1 : 0), out data.target))
			{
				this.lord.ownedPawns.TryRandomElementByWeight((Pawn x) => (float)((!x.Destroyed && !x.Downed) ? 1 : 0), out data.target);
			}
			if (data.target != null)
			{
				Hediff firstHediffOfDef = data.target.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.XenogermReplicating, false);
				if (firstHediffOfDef != null)
				{
					data.target.health.RemoveHediff(firstHediffOfDef);
				}
				Find.LetterStack.ReceiveLetter("LetterLabelSanguophageWaitingToReimplant".Translate(), "LetterTextSanguophageWaitingToReimplant".Translate(data.target.Named("PAWN"), 15000.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")), LetterDefOf.PositiveEvent, data.target, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06008B2B RID: 35627 RVA: 0x002976D8 File Offset: 0x002958D8
		public override void UpdateAllDuties()
		{
			LordToilData_ReimplantXenogerm data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i] == data.target)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Idle, data.waitSpot, -1f);
				}
				else
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.WanderClose_NoNeeds, data.waitSpot, 10f);
				}
			}
		}

		// Token: 0x06008B2C RID: 35628 RVA: 0x00297792 File Offset: 0x00295992
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			if (pawn == this.Data.target)
			{
				pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
			}
		}

		// Token: 0x06008B2D RID: 35629 RVA: 0x002977B5 File Offset: 0x002959B5
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (p == this.Data.target)
			{
				this.SetTarget();
			}
		}

		// Token: 0x06008B2E RID: 35630 RVA: 0x002977CB File Offset: 0x002959CB
		public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn requester, Pawn current)
		{
			LordToilData_ReimplantXenogerm data = this.Data;
			if (data.target == requester)
			{
				if (requester.genes == null || current.genes == null)
				{
					yield break;
				}
				string text = "AcceptXenogerm".Translate(requester);
				if (!current.CanReach(data.target, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					yield return new FloatMenuOption(text + ": " + "CannotReach".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else if (current.IsQuestLodger())
				{
					yield return new FloatMenuOption(text + ": " + "MessageCannotImplantInTempFactionMembers".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else if (GeneUtility.SameXenotype(current, data.target))
				{
					yield return new FloatMenuOption(text + ": " + "MessageCannotUseOnSameXenotype".Translate(current), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					yield return new FloatMenuOption(text, delegate()
					{
						current.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.GetReimplanted, requester), new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			yield break;
		}

		// Token: 0x04005A6C RID: 23148
		private const float WanderRadius = 10f;
	}
}
