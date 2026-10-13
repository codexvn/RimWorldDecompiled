using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011D3 RID: 4563
	public class JobDriver_BuildSnowman : JobDriver
	{
		// Token: 0x06006F86 RID: 28550 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006F87 RID: 28551 RVA: 0x0022B69B File Offset: 0x0022989B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = ToilMaker.MakeToil("MakeNewToils");
			doWork.initAction = delegate()
			{
				this.workLeft = 2300f;
			};
			doWork.tickIntervalAction = delegate(int delta)
			{
				this.workLeft -= doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true, -1) * 1.7f * (float)delta;
				if (this.workLeft <= 0f)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Snowman, null);
					thing.SetFaction(this.pawn.Faction, null);
					GenSpawn.Spawn(thing, this.TargetLocA, this.Map, WipeMode.Vanish);
					this.ReadyForNextToil();
					return;
				}
				JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, 1f, null);
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x06006F88 RID: 28552 RVA: 0x0022B6AB File Offset: 0x002298AB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04004DC1 RID: 19905
		private float workLeft = -1000f;

		// Token: 0x04004DC2 RID: 19906
		protected const int BaseWorkAmount = 2300;
	}
}
