using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000E7C RID: 3708
	public class JobDriver_Equip : JobDriver
	{
		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x06005D6D RID: 23917 RVA: 0x001DD17C File Offset: 0x001DB37C
		private Thing Target
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06005D6E RID: 23918 RVA: 0x001DD1A0 File Offset: 0x001DB3A0
		private bool TargetIsOnOutfitStand
		{
			get
			{
				Thing target = this.Target;
				return target != null && !target.Spawned && target.ParentHolder is Building_OutfitStand;
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06005D6F RID: 23919 RVA: 0x001DD1D0 File Offset: 0x001DB3D0
		private Building_OutfitStand OutfitStand
		{
			get
			{
				return (Building_OutfitStand)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06005D70 RID: 23920 RVA: 0x001DD1F6 File Offset: 0x001DB3F6
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.TargetIsOnOutfitStand)
			{
				this.job.targetB = (Building_OutfitStand)this.Target.ParentHolder;
			}
		}

		// Token: 0x06005D71 RID: 23921 RVA: 0x001DD228 File Offset: 0x001DB428
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (EquipmentUtility.AlreadyBondedToWeapon(this.Target, this.pawn))
			{
				return false;
			}
			int maxPawns = 1;
			int stackCount = -1;
			bool targetIsOnOutfitStand = this.TargetIsOnOutfitStand;
			if (this.job.targetA.HasThing && this.job.targetA.Thing.Spawned && this.job.targetA.Thing.def.IsIngestible)
			{
				maxPawns = 10;
				stackCount = 1;
			}
			if (this.TargetIsOnOutfitStand)
			{
				return this.pawn.Reserve((Building_OutfitStand)this.job.targetA.Thing.ParentHolder, this.job, maxPawns, stackCount, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(this.job.targetA, this.job, maxPawns, stackCount, null, errorOnFailed, false);
		}

		// Token: 0x06005D72 RID: 23922 RVA: 0x001DD2FE File Offset: 0x001DB4FE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				this.pawn.mindState.droppedWeapon = null;
			});
			bool targetOnStand = this.TargetIsOnOutfitStand;
			Toil f = Toils_Goto.GotoThing(targetOnStand ? TargetIndex.B : TargetIndex.A, PathEndMode.ClosestTouch, false);
			if (this.job.ignoreForbidden)
			{
				yield return f.FailOnDespawnedOrNull(targetOnStand ? TargetIndex.B : TargetIndex.A);
			}
			else
			{
				yield return f.FailOnDespawnedNullOrForbidden(targetOnStand ? TargetIndex.B : TargetIndex.A);
			}
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			if (targetOnStand)
			{
				toil.initAction = delegate()
				{
					Building_OutfitStand outfitStand = this.OutfitStand;
					ThingWithComps heldWeapon = outfitStand.HeldWeapon;
					if (heldWeapon == null || heldWeapon != this.Target)
					{
						base.EndJobWith(JobCondition.Errored);
						return;
					}
					if (!outfitStand.RemoveHeldWeapon(heldWeapon))
					{
						base.EndJobWith(JobCondition.Errored);
						return;
					}
					this.pawn.equipment.MakeRoomFor(heldWeapon);
					this.pawn.equipment.AddEquipment(heldWeapon);
					SoundDef soundInteract = heldWeapon.def.soundInteract;
					if (soundInteract == null)
					{
						return;
					}
					soundInteract.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
				};
			}
			else
			{
				toil.initAction = delegate()
				{
					ThingWithComps thingWithComps = (ThingWithComps)this.job.targetA.Thing;
					ThingWithComps thingWithComps2;
					if (thingWithComps.def.stackLimit > 1 && thingWithComps.stackCount > 1)
					{
						thingWithComps2 = (ThingWithComps)thingWithComps.SplitOff(1);
					}
					else
					{
						thingWithComps2 = thingWithComps;
						thingWithComps2.DeSpawn(DestroyMode.Vanish);
					}
					this.pawn.equipment.MakeRoomFor(thingWithComps2);
					this.pawn.equipment.AddEquipment(thingWithComps2);
					SoundDef soundInteract = thingWithComps.def.soundInteract;
					if (soundInteract == null)
					{
						return;
					}
					soundInteract.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
				};
			}
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}

		// Token: 0x040043BC RID: 17340
		private const TargetIndex EquippableIndex = TargetIndex.A;

		// Token: 0x040043BD RID: 17341
		private const TargetIndex OutfitStandIndex = TargetIndex.B;
	}
}
