using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001314 RID: 4884
	public class JobDriver_ShieldMech : JobDriver_CastAbility
	{
		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x060077DD RID: 30685 RVA: 0x00245DA7 File Offset: 0x00243FA7
		private CompProjectileInterceptor ProjectileInterceptor
		{
			get
			{
				if (this.projectileInterceptor == null && this.mechShield != null)
				{
					this.projectileInterceptor = this.mechShield.GetComp<CompProjectileInterceptor>();
				}
				return this.projectileInterceptor;
			}
		}

		// Token: 0x060077DE RID: 30686 RVA: 0x00245DD0 File Offset: 0x00243FD0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			base.AddFinishAction(delegate(JobCondition jobCondition)
			{
				if (this.mechShield != null)
				{
					if (!this.mechShield.Destroyed)
					{
						this.mechShield.Destroy(DestroyMode.Vanish);
					}
					this.mechShield = null;
				}
			});
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				this.pawn.pather.StopDead();
				this.mechShield = (MechShield)GenSpawn.Spawn(ThingDefOf.MechShield, base.TargetThingA.Position, this.pawn.Map, WipeMode.Vanish);
				this.mechShield.SetTarget(base.TargetThingA);
				int num = (int)this.pawn.GetStatValue(StatDefOf.MechRemoteShieldEnergy, true, -1);
				this.ProjectileInterceptor.maxHitPointsOverride = new int?(num);
				this.ProjectileInterceptor.currentHitPoints = num;
			};
			toil2.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(base.TargetThingA);
			};
			toil2.defaultCompleteMode = ToilCompleteMode.Delay;
			toil2.defaultDuration = 1800;
			toil2.handlingFacing = true;
			toil2.PlaySustainerOrSound(SoundDefOf.ShieldMech, 1f);
			toil2.AddFailCondition(delegate
			{
				if (this.mechShield != null)
				{
					if (!this.job.ability.verb.CanHitTargetFrom(this.pawn.Position, this.mechShield))
					{
						return true;
					}
					if (this.ProjectileInterceptor != null && !this.ProjectileInterceptor.Active)
					{
						return true;
					}
				}
				return false;
			});
			yield return toil2;
			yield break;
			yield break;
		}

		// Token: 0x060077DF RID: 30687 RVA: 0x00245DE0 File Offset: 0x00243FE0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MechShield>(ref this.mechShield, "mechShield", false);
		}

		// Token: 0x04005199 RID: 20889
		private const int DurationTicks = 1800;

		// Token: 0x0400519A RID: 20890
		private MechShield mechShield;

		// Token: 0x0400519B RID: 20891
		private CompProjectileInterceptor projectileInterceptor;
	}
}
