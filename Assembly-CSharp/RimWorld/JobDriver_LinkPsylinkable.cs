using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012B2 RID: 4786
	public class JobDriver_LinkPsylinkable : JobDriver
	{
		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06007541 RID: 30017 RVA: 0x0023D050 File Offset: 0x0023B250
		private Thing PsylinkableThing
		{
			get
			{
				return base.TargetA.Thing;
			}
		}

		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06007542 RID: 30018 RVA: 0x0023D06B File Offset: 0x0023B26B
		private CompPsylinkable Psylinkable
		{
			get
			{
				return this.PsylinkableThing.TryGetComp<CompPsylinkable>();
			}
		}

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06007543 RID: 30019 RVA: 0x001E5D15 File Offset: 0x001E3F15
		private LocalTargetInfo LinkSpot
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x06007544 RID: 30020 RVA: 0x0023D078 File Offset: 0x0023B278
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.PsylinkableThing, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.LinkSpot, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007545 RID: 30021 RVA: 0x0023D0C6 File Offset: 0x0023B2C6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckRoyalty("Psylinkable"))
			{
				yield break;
			}
			base.AddFailCondition(() => !this.Psylinkable.CanPsylink(this.pawn, new LocalTargetInfo?(this.LinkSpot), true).Accepted);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil toil = Toils_General.Wait(15000, TargetIndex.None);
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(this.PsylinkableThing);
				if (!this.pawn.IsHashIntervalTick(720, delta))
				{
					return;
				}
				Vector3 vector = this.pawn.TrueCenter();
				vector += (this.PsylinkableThing.TrueCenter() - vector) * Rand.Value;
				FleckMaker.Static(vector, this.pawn.Map, FleckDefOf.PsycastAreaEffect, 0.5f);
				this.Psylinkable.Props.linkSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.PsylinkableThing), MaintenanceType.None));
			};
			toil.handlingFacing = false;
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield break;
		}

		// Token: 0x04005050 RID: 20560
		public const int LinkTimeTicks = 15000;

		// Token: 0x04005051 RID: 20561
		public const int EffectsTickInterval = 720;

		// Token: 0x04005052 RID: 20562
		protected const TargetIndex PsylinkableInd = TargetIndex.A;

		// Token: 0x04005053 RID: 20563
		protected const TargetIndex LinkSpotInd = TargetIndex.B;
	}
}
