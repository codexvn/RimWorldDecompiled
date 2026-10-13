using System;
using System.Linq;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020006AD RID: 1709
	public class HediffComp_Disorientation : HediffComp
	{
		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002AE0 RID: 10976 RVA: 0x000E0E4F File Offset: 0x000DF04F
		private HediffCompProperties_Disorientation Props
		{
			get
			{
				return (HediffCompProperties_Disorientation)this.props;
			}
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000E0E5C File Offset: 0x000DF05C
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.Props.wanderMtbHours > 0f && base.Pawn.Spawned && !base.Pawn.Downed && base.Pawn.Awake() && base.Pawn.CurJobDef.suspendable && this.Props.wanderMtbHours > 0f && base.Pawn.IsHashIntervalTick(60, delta) && Rand.MTBEventOccurs(this.Props.wanderMtbHours, 2500f, 60f) && base.Pawn.CurJob.def != JobDefOf.GotoMindControlled)
			{
				IntVec3 c2 = (from c in GenRadial.RadialCellsAround(base.Pawn.Position, this.Props.wanderRadius, false)
				where c.Standable(base.Pawn.MapHeld) && base.Pawn.CanReach(c, PathEndMode.OnCell, Danger.Unspecified, false, false, TraverseMode.ByPawn)
				select c).RandomElementWithFallback(IntVec3.Invalid);
				if (c2.IsValid)
				{
					MoteMaker.MakeThoughtBubble(base.Pawn, "Things/Mote/Disoriented", false);
					Job job = JobMaker.MakeJob(JobDefOf.GotoMindControlled, c2);
					job.expiryInterval = this.Props.singleWanderDurationTicks;
					base.Pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
				}
			}
		}

		// Token: 0x040022A4 RID: 8868
		private const string MoteTexPath = "Things/Mote/Disoriented";
	}
}
