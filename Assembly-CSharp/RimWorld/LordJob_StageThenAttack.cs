using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200167A RID: 5754
	public class LordJob_StageThenAttack : LordJob
	{
		// Token: 0x170014C3 RID: 5315
		// (get) Token: 0x0600879D RID: 34717 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600879E RID: 34718 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_StageThenAttack()
		{
		}

		// Token: 0x0600879F RID: 34719 RVA: 0x00287EF0 File Offset: 0x002860F0
		public LordJob_StageThenAttack(Faction faction, IntVec3 stageLoc, int raidSeed, bool canTimeoutFlee = true, bool canKidnap = true, bool canSteal = true, IntRange? delay = null)
		{
			this.faction = faction;
			this.stageLoc = stageLoc;
			this.raidSeed = raidSeed;
			this.canTimeoutFlee = canTimeoutFlee;
			this.canKidnap = canKidnap;
			this.canSteal = canSteal;
			this.delay = (delay ?? new IntRange(5000, 15000));
		}

		// Token: 0x060087A0 RID: 34720 RVA: 0x00287F5C File Offset: 0x0028615C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Stage lordToil_Stage = new LordToil_Stage(this.stageLoc);
			stateGraph.StartingToil = lordToil_Stage;
			Faction assaulterFaction = this.faction;
			bool canTimeoutOrFlee = this.canTimeoutFlee;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(assaulterFaction, this.canKidnap, canTimeoutOrFlee, false, false, this.canSteal, false, false).CreateGraph()).StartingToil;
			int tickLimit = Rand.RangeSeeded(this.delay.min, this.delay.max, this.raidSeed);
			Transition transition = new Transition(lordToil_Stage, startingToil, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(tickLimit));
			transition.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			transition.AddPreAction(new TransitionAction_Message("MessageRaidersBeginningAssault".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), MessageTypeDefOf.ThreatBig, "MessageRaidersBeginningAssault-" + this.raidSeed.ToString(), 1f, null));
			transition.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition, false);
			stateGraph.transitions.Find((Transition x) => x.triggers.Any((Trigger y) => y is Trigger_BecameNonHostileToPlayer)).AddSource(lordToil_Stage);
			return stateGraph;
		}

		// Token: 0x060087A1 RID: 34721 RVA: 0x002880AC File Offset: 0x002862AC
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.stageLoc, "stageLoc", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.raidSeed, "raidSeed", 0, false);
			Scribe_Values.Look<bool>(ref this.canTimeoutFlee, "canTimeoutFlee", false, false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", false, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", false, false);
			Scribe_Values.Look<IntRange>(ref this.delay, "delay", default(IntRange), false);
		}

		// Token: 0x04005885 RID: 22661
		private Faction faction;

		// Token: 0x04005886 RID: 22662
		private IntVec3 stageLoc;

		// Token: 0x04005887 RID: 22663
		private int raidSeed;

		// Token: 0x04005888 RID: 22664
		private bool canTimeoutFlee;

		// Token: 0x04005889 RID: 22665
		private bool canKidnap;

		// Token: 0x0400588A RID: 22666
		private bool canSteal;

		// Token: 0x0400588B RID: 22667
		private IntRange delay;
	}
}
