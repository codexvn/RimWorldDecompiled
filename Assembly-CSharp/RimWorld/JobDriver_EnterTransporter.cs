using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001267 RID: 4711
	public class JobDriver_EnterTransporter : JobDriver
	{
		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x0600733B RID: 29499 RVA: 0x00237168 File Offset: 0x00235368
		public CompTransporter Transporter
		{
			get
			{
				Thing thing = this.job.GetTarget(this.TransporterInd).Thing;
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x0600733C RID: 29500 RVA: 0x0023719C File Offset: 0x0023539C
		public CompShuttle Shuttle
		{
			get
			{
				Thing thing = this.job.GetTarget(this.TransporterInd).Thing;
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompShuttle>();
			}
		}

		// Token: 0x0600733D RID: 29501 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600733E RID: 29502 RVA: 0x002371CE File Offset: 0x002353CE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.TransporterInd);
			this.FailOn(() => this.Shuttle != null && !this.Shuttle.IsAllowed(this.pawn));
			yield return Toils_Goto.GotoThing(this.TransporterInd, PathEndMode.Touch, false);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				if (!this.job.playerForced && LoadTransportersJobUtility.HasJobOnTransporter(this.pawn, this.Transporter))
				{
					return;
				}
				if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
				}
				CompTransporter transporter = this.Transporter;
				bool flag = this.pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
				transporter.GetDirectlyHeldThings().TryAdd(this.pawn, true);
				if (flag)
				{
					Find.Selector.Select(this.pawn, false, false);
				}
			};
			yield return toil;
			yield break;
		}

		// Token: 0x04004F7F RID: 20351
		private TargetIndex TransporterInd = TargetIndex.A;
	}
}
