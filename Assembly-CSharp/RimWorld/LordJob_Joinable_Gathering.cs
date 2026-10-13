using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016CE RID: 5838
	public abstract class LordJob_Joinable_Gathering : LordJob_VoluntarilyJoinable
	{
		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x060089C9 RID: 35273 RVA: 0x00291E6C File Offset: 0x0029006C
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x060089CA RID: 35274 RVA: 0x00291E74 File Offset: 0x00290074
		public int DurationTicks
		{
			get
			{
				return this.durationTicks;
			}
		}

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x060089CB RID: 35275 RVA: 0x00291E7C File Offset: 0x0029007C
		public virtual int TicksLeft
		{
			get
			{
				return this.timeoutTrigger.TicksLeft;
			}
		}

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x060089CC RID: 35276 RVA: 0x00291E89 File Offset: 0x00290089
		public virtual IntVec3 Spot
		{
			get
			{
				return this.spot;
			}
		}

		// Token: 0x060089CD RID: 35277 RVA: 0x00291E91 File Offset: 0x00290091
		public LordJob_Joinable_Gathering()
		{
		}

		// Token: 0x060089CE RID: 35278 RVA: 0x00291E99 File Offset: 0x00290099
		public LordJob_Joinable_Gathering(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.organizer = organizer;
			this.gatheringDef = gatheringDef;
		}

		// Token: 0x060089CF RID: 35279
		protected abstract LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef);

		// Token: 0x060089D0 RID: 35280 RVA: 0x00291EB6 File Offset: 0x002900B6
		protected virtual bool ShouldBeCalledOff()
		{
			return (this.organizer != null && !GatheringsUtility.PawnCanStartOrContinueGathering(this.organizer)) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		// Token: 0x060089D1 RID: 35281 RVA: 0x00291EE0 File Offset: 0x002900E0
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (!this.IsInvited(p))
			{
				return 0f;
			}
			if (!GatheringsUtility.ShouldPawnKeepGathering(p, this.gatheringDef))
			{
				return 0f;
			}
			if (this.spot.IsForbidden(p))
			{
				return 0f;
			}
			if (!this.lord.ownedPawns.Contains(p) && this.IsGatheringAboutToEnd())
			{
				return 0f;
			}
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.blocksSocialInteraction)
				{
					return 0f;
				}
			}
			return VoluntarilyJoinableLordJobJoinPriorities.SocialGathering;
		}

		// Token: 0x060089D2 RID: 35282 RVA: 0x00291F88 File Offset: 0x00290188
		protected virtual Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassed(this.durationTicks);
		}

		// Token: 0x060089D3 RID: 35283 RVA: 0x00291F98 File Offset: 0x00290198
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
			Scribe_References.Look<Pawn>(ref this.organizer, "organizer", false);
			Scribe_Defs.Look<GatheringDef>(ref this.gatheringDef, "gatheringDef");
		}

		// Token: 0x060089D4 RID: 35284 RVA: 0x00291FF2 File Offset: 0x002901F2
		protected bool IsGatheringAboutToEnd()
		{
			return this.TicksLeft < 1200;
		}

		// Token: 0x060089D5 RID: 35285 RVA: 0x00292004 File Offset: 0x00290204
		protected virtual bool IsInvited(Pawn p)
		{
			return this.lord.faction != null && p.Faction == this.lord.faction;
		}

		// Token: 0x04005A01 RID: 23041
		protected IntVec3 spot;

		// Token: 0x04005A02 RID: 23042
		protected Pawn organizer;

		// Token: 0x04005A03 RID: 23043
		protected GatheringDef gatheringDef;

		// Token: 0x04005A04 RID: 23044
		protected int durationTicks;

		// Token: 0x04005A05 RID: 23045
		protected Trigger_TicksPassed timeoutTrigger;
	}
}
