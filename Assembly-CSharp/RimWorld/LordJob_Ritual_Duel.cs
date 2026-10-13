using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016CB RID: 5835
	public class LordJob_Ritual_Duel : LordJob_Ritual
	{
		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x060089A7 RID: 35239 RVA: 0x00291904 File Offset: 0x0028FB04
		public DuelBehaviorStage CurrentDuelStage
		{
			get
			{
				if (this.attacksThisStage <= 0)
				{
					return DuelBehaviorStage.Move;
				}
				return DuelBehaviorStage.Attack;
			}
		}

		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x060089A8 RID: 35240 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060089A9 RID: 35241 RVA: 0x00291912 File Offset: 0x0028FB12
		public LordJob_Ritual_Duel()
		{
		}

		// Token: 0x060089AA RID: 35242 RVA: 0x00291928 File Offset: 0x0028FB28
		public LordJob_Ritual_Duel(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer, null)
		{
			foreach (RitualRole ritualRole in assignments.AllRolesForReading)
			{
				if (ritualRole != null && ritualRole.id.Contains("duelist"))
				{
					Pawn item = assignments.FirstAssignedPawn(ritualRole);
					this.duelists.Add(item);
					this.pawnsDeathIgnored.Add(item);
				}
			}
		}

		// Token: 0x060089AB RID: 35243 RVA: 0x002919D0 File Offset: 0x0028FBD0
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped && base.ShouldRemovePawn(p, reason);
		}

		// Token: 0x060089AC RID: 35244 RVA: 0x002919E0 File Offset: 0x0028FBE0
		public void StartDuelIfNotStartedYet()
		{
			if (!this.duelStarted)
			{
				this.duelStarted = true;
				this.StartDuel();
			}
		}

		// Token: 0x060089AD RID: 35245 RVA: 0x002919F7 File Offset: 0x0028FBF7
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (this.duelists.Contains(p))
			{
				return 1f;
			}
			return base.VoluntaryJoinPriorityFor(p);
		}

		// Token: 0x060089AE RID: 35246 RVA: 0x00291A14 File Offset: 0x0028FC14
		private void InterruptDuelistJobs()
		{
			foreach (Pawn pawn in this.duelists)
			{
				Pawn_JobTracker jobs = pawn.jobs;
				if (jobs != null)
				{
					jobs.CheckForJobOverride(0f, true);
				}
			}
		}

		// Token: 0x060089AF RID: 35247 RVA: 0x00291A78 File Offset: 0x0028FC78
		private void StartDuel()
		{
			this.StartMoving();
		}

		// Token: 0x060089B0 RID: 35248 RVA: 0x00291A80 File Offset: 0x0028FC80
		private void StartMoving()
		{
			this.attacksThisStage = 0;
			this.movingTicks = LordJob_Ritual_Duel.MovingTicksPerStage.RandomInRange;
			this.InterruptDuelistJobs();
		}

		// Token: 0x060089B1 RID: 35249 RVA: 0x00291AB0 File Offset: 0x0028FCB0
		private void StartAttacking()
		{
			this.movingTicks = 0;
			this.attacksThisStage = LordJob_Ritual_Duel.AttacksPerStage.RandomInRange;
			this.InterruptDuelistJobs();
		}

		// Token: 0x060089B2 RID: 35250 RVA: 0x00291ADD File Offset: 0x0028FCDD
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.movingTicks > 0)
			{
				this.movingTicks--;
				if (this.movingTicks <= 0)
				{
					this.StartAttacking();
				}
			}
		}

		// Token: 0x060089B3 RID: 35251 RVA: 0x00291B0C File Offset: 0x0028FD0C
		public override void ApplyOutcome(float progress, bool showFinishedMessage = true, bool showFailedMessage = true, bool cancelled = false)
		{
			foreach (Pawn pawn in this.duelists)
			{
				if (pawn.equipment.Primary != null)
				{
					ThingWithComps thingWithComps;
					pawn.equipment.TryDropEquipment(pawn.equipment.Primary, out thingWithComps, pawn.PositionHeld, true);
				}
			}
			base.ApplyOutcome(progress, showFinishedMessage, showFailedMessage, cancelled);
		}

		// Token: 0x060089B4 RID: 35252 RVA: 0x00291B90 File Offset: 0x0028FD90
		public void Notify_MeleeAttack(Pawn duelist, Thing victim)
		{
			this.attacksThisStage--;
			if (this.attacksThisStage <= 0)
			{
				this.StartMoving();
			}
			if (this.usedWeapon)
			{
				return;
			}
			Verb verb = duelist.meleeVerbs.TryGetMeleeVerb(victim);
			if (verb != null && verb.EquipmentSource != null && verb.EquipmentSource.def.IsMeleeWeapon)
			{
				this.usedWeapon = true;
			}
		}

		// Token: 0x060089B5 RID: 35253 RVA: 0x00291BF4 File Offset: 0x0028FDF4
		protected override bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
		{
			int num = 0;
			using (List<Pawn>.Enumerator enumerator = this.duelists.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.DeadOrDowned)
					{
						num++;
					}
				}
			}
			return num <= 1;
		}

		// Token: 0x060089B6 RID: 35254 RVA: 0x00291C54 File Offset: 0x0028FE54
		public Pawn Opponent(Pawn duelist)
		{
			return this.duelists[(this.duelists.IndexOf(duelist) == 0) ? 1 : 0];
		}

		// Token: 0x060089B7 RID: 35255 RVA: 0x00291C73 File Offset: 0x0028FE73
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (!p.DeadOrDowned)
			{
				base.Cancel();
			}
			if (p.Dead && this.duelists.Contains(p))
			{
				p.health.killedByRitual = true;
			}
		}

		// Token: 0x060089B8 RID: 35256 RVA: 0x00291CA5 File Offset: 0x0028FEA5
		public override bool BlocksSocialInteraction(Pawn pawn)
		{
			return this.duelists.Contains(pawn);
		}

		// Token: 0x060089B9 RID: 35257 RVA: 0x00291CB4 File Offset: 0x0028FEB4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.duelists, "duelists", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.usedWeapon, "usedWeapon", false, false);
			Scribe_Values.Look<int>(ref this.movingTicks, "movingTicks", 0, false);
			Scribe_Values.Look<int>(ref this.attacksThisStage, "attacksThisStage", 0, false);
			Scribe_Values.Look<bool>(ref this.duelStarted, "duelStarted", false, false);
		}

		// Token: 0x040059F9 RID: 23033
		public List<Pawn> duelists = new List<Pawn>();

		// Token: 0x040059FA RID: 23034
		public bool usedWeapon;

		// Token: 0x040059FB RID: 23035
		private bool duelStarted;

		// Token: 0x040059FC RID: 23036
		private int attacksThisStage;

		// Token: 0x040059FD RID: 23037
		private int movingTicks;

		// Token: 0x040059FE RID: 23038
		private static readonly IntRange AttacksPerStage = new IntRange(4, 8);

		// Token: 0x040059FF RID: 23039
		private static readonly IntRange MovingTicksPerStage = new IntRange(360, 600);
	}
}
