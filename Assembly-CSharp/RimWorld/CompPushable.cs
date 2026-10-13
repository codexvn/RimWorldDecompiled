using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002DD0 RID: 11728
	public class CompPushable : ThingComp
	{
		// Token: 0x17002A15 RID: 10773
		// (get) Token: 0x060107E0 RID: 67552 RVA: 0x004C1F46 File Offset: 0x004C0146
		public CompProperties_Pushable Props
		{
			get
			{
				return (CompProperties_Pushable)this.props;
			}
		}

		// Token: 0x17002A16 RID: 10774
		// (get) Token: 0x060107E1 RID: 67553 RVA: 0x0049B545 File Offset: 0x00499745
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x060107E2 RID: 67554 RVA: 0x000026EA File Offset: 0x000008EA
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
		}

		// Token: 0x060107E3 RID: 67555 RVA: 0x004C1F53 File Offset: 0x004C0153
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!this.canBePushed || !this.Props.givePushOption)
			{
				yield break;
			}
			Action<LocalTargetInfo> <>9__1;
			FloatMenuOption floatMenuOption = new FloatMenuOption("CommandPushTo".Translate(), delegate()
			{
				Targeter targeter = Find.Targeter;
				TargetingParameters targetParams = TargetingParameters.ForCell();
				Action<LocalTargetInfo> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(LocalTargetInfo target)
					{
						Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, this.Pawn, target.Cell);
						job.count = 1;
						selPawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
					});
				}
				targeter.BeginTargeting(targetParams, action, null, null, null, true);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			if (!selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				floatMenuOption.Label = string.Format("{0} ({1})", floatMenuOption.Label, "RequiredCapacity".Translate(PawnCapacityDefOf.Manipulation.label));
				floatMenuOption.Disabled = true;
			}
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x060107E4 RID: 67556 RVA: 0x004C1F6C File Offset: 0x004C016C
		public void OnStartedCarrying(Pawn pawn)
		{
			Vector3 v = (pawn.Position - this.Pawn.Position).ToVector3();
			Vector2 vector = new Vector2(0f, -this.Props.offsetDistance).RotatedBy(v.AngleFlat());
			this.drawPos = new Vector3(vector.x, 0f, -vector.y);
			this.drawVel = Vector3.zero;
		}

		// Token: 0x060107E5 RID: 67557 RVA: 0x004C1FE2 File Offset: 0x004C01E2
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			bool devMode = Prefs.DevMode;
			yield break;
		}

		// Token: 0x060107E6 RID: 67558 RVA: 0x004C1FEC File Offset: 0x004C01EC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.canBePushed, "canBePushed", false, false);
			Scribe_Values.Look<Vector3>(ref this.drawPos, "drawPos", default(Vector3), false);
			Scribe_Values.Look<Vector3>(ref this.drawVel, "drawVel", default(Vector3), false);
		}

		// Token: 0x0400AC01 RID: 44033
		public bool canBePushed = true;

		// Token: 0x0400AC02 RID: 44034
		public Vector3 drawPos;

		// Token: 0x0400AC03 RID: 44035
		public Vector3 drawVel;
	}
}
