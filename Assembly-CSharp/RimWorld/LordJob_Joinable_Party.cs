using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016D1 RID: 5841
	public class LordJob_Joinable_Party : LordJob_Joinable_Gathering
	{
		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x060089EF RID: 35311 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x060089F0 RID: 35312 RVA: 0x00292DA3 File Offset: 0x00290FA3
		protected virtual ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x060089F1 RID: 35313 RVA: 0x00292DAA File Offset: 0x00290FAA
		protected virtual TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x060089F2 RID: 35314 RVA: 0x00292DA3 File Offset: 0x00290FA3
		protected virtual ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x060089F3 RID: 35315 RVA: 0x00292DAA File Offset: 0x00290FAA
		protected virtual TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		// Token: 0x060089F4 RID: 35316 RVA: 0x00292DB1 File Offset: 0x00290FB1
		public LordJob_Joinable_Party()
		{
		}

		// Token: 0x060089F5 RID: 35317 RVA: 0x00292DB9 File Offset: 0x00290FB9
		public LordJob_Joinable_Party(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
			this.durationTicks = Rand.RangeInclusive(5000, 15000);
		}

		// Token: 0x060089F6 RID: 35318 RVA: 0x00292DD9 File Offset: 0x00290FD9
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingParty".Translate();
		}

		// Token: 0x060089F7 RID: 35319 RVA: 0x00292DEA File Offset: 0x00290FEA
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Party(spot, gatheringDef, 3.5E-05f);
		}

		// Token: 0x060089F8 RID: 35320 RVA: 0x00292DF8 File Offset: 0x00290FF8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil party = this.CreateGatheringToil(this.spot, this.organizer, this.gatheringDef);
			stateGraph.AddToil(party);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(party, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_TickCondition(new Func<bool>(this.ShouldBeCalledOff), 1));
			transition.AddTrigger(new Trigger_PawnKilled());
			transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.LeftVoluntarily, this.organizer));
			transition.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.ApplyOutcome((LordToil_Party)party);
			}));
			transition.AddPreAction(new TransitionAction_Message(this.gatheringDef.calledOffMessage, MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition, false);
			this.timeoutTrigger = this.GetTimeoutTrigger();
			Transition transition2 = new Transition(party, lordToil_End, false, true);
			transition2.AddTrigger(this.timeoutTrigger);
			transition2.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.ApplyOutcome((LordToil_Party)party);
			}));
			transition2.AddPreAction(new TransitionAction_Message(this.gatheringDef.finishedMessage, MessageTypeDefOf.SituationResolved, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x060089F9 RID: 35321 RVA: 0x00292F60 File Offset: 0x00291160
		private void ApplyOutcome(LordToil_Party toil)
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			LordToilData_Gathering lordToilData_Gathering = (LordToilData_Gathering)toil.data;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				bool flag = pawn == this.organizer;
				int num;
				if (lordToilData_Gathering.presentForTicks.TryGetValue(pawn, out num) && num > 0)
				{
					if (ownedPawns[i].needs.mood != null)
					{
						ThoughtDef thoughtDef = flag ? this.OrganizerThought : this.AttendeeThought;
						float num2 = 0.5f / thoughtDef.stages[0].baseMoodEffect;
						float moodPowerFactor = Mathf.Min((float)num / (float)this.durationTicks + num2, 1f);
						Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
						thought_Memory.moodPowerFactor = moodPowerFactor;
						ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
					}
					TaleRecorder.RecordTale(flag ? this.OrganizerTale : this.AttendeeTale, new object[]
					{
						ownedPawns[i],
						this.organizer
					});
				}
			}
		}

		// Token: 0x060089FA RID: 35322 RVA: 0x00293093 File Offset: 0x00291293
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.gatheringDef == null)
			{
				this.gatheringDef = GatheringDefOf.Party;
			}
		}
	}
}
