using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002B1E RID: 11038
	public class PawnFlyer : Thing, IThingHolderTickable, IThingHolder, ISearchableContents
	{
		// Token: 0x1700269A RID: 9882
		// (get) Token: 0x0600F69F RID: 63135 RVA: 0x00479DF1 File Offset: 0x00477FF1
		public bool ShouldTickContents
		{
			get
			{
				return this.ticksFlying < this.ticksFlightTime;
			}
		}

		// Token: 0x1700269B RID: 9883
		// (get) Token: 0x0600F6A0 RID: 63136 RVA: 0x00479E01 File Offset: 0x00478001
		protected Thing FlyingThing
		{
			get
			{
				if (this.innerContainer.InnerListForReading.Count <= 0)
				{
					return null;
				}
				return this.innerContainer.InnerListForReading[0];
			}
		}

		// Token: 0x1700269C RID: 9884
		// (get) Token: 0x0600F6A1 RID: 63137 RVA: 0x00479E29 File Offset: 0x00478029
		public Pawn FlyingPawn
		{
			get
			{
				return this.FlyingThing as Pawn;
			}
		}

		// Token: 0x1700269D RID: 9885
		// (get) Token: 0x0600F6A2 RID: 63138 RVA: 0x00479E36 File Offset: 0x00478036
		public Thing CarriedThing
		{
			get
			{
				return this.carriedThing;
			}
		}

		// Token: 0x1700269E RID: 9886
		// (get) Token: 0x0600F6A3 RID: 63139 RVA: 0x00479E3E File Offset: 0x0047803E
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x1700269F RID: 9887
		// (get) Token: 0x0600F6A4 RID: 63140 RVA: 0x00479E46 File Offset: 0x00478046
		public override Vector3 DrawPos
		{
			get
			{
				this.RecomputePosition();
				return this.effectivePos;
			}
		}

		// Token: 0x170026A0 RID: 9888
		// (get) Token: 0x0600F6A5 RID: 63141 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override int UpdateRateTicks
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600F6A6 RID: 63142 RVA: 0x00479E54 File Offset: 0x00478054
		private void RecomputePosition()
		{
			if (this.positionLastComputedTick == this.ticksFlying)
			{
				return;
			}
			this.positionLastComputedTick = this.ticksFlying;
			float t = (float)this.ticksFlying / (float)this.ticksFlightTime;
			float t2 = this.def.pawnFlyer.Worker.AdjustedProgress(t);
			this.effectiveHeight = this.def.pawnFlyer.Worker.GetHeight(t2);
			this.groundPos = Vector3.Lerp(this.startVec, this.DestinationPos, t2);
			Vector3 b = Altitudes.AltIncVect * this.effectiveHeight;
			Vector3 b2 = Vector3.forward * (this.def.pawnFlyer.heightFactor * this.effectiveHeight);
			this.effectivePos = this.groundPos + b + b2;
			base.Position = this.groundPos.ToIntVec3();
		}

		// Token: 0x0600F6A7 RID: 63143 RVA: 0x00479E3E File Offset: 0x0047803E
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600F6A8 RID: 63144 RVA: 0x00479F33 File Offset: 0x00478133
		public PawnFlyer()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600F6A9 RID: 63145 RVA: 0x00479F5D File Offset: 0x0047815D
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Effecter effecter = this.flightEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			base.Destroy(mode);
		}

		// Token: 0x0600F6AA RID: 63146 RVA: 0x00479F77 File Offset: 0x00478177
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x170026A1 RID: 9889
		// (get) Token: 0x0600F6AB RID: 63147 RVA: 0x00479F88 File Offset: 0x00478188
		public Vector3 DestinationPos
		{
			get
			{
				Thing flyingThing = this.FlyingThing;
				return GenThing.TrueCenter(this.destCell, flyingThing.Rotation, flyingThing.def.size, flyingThing.def.Altitude);
			}
		}

		// Token: 0x0600F6AC RID: 63148 RVA: 0x00479FC4 File Offset: 0x004781C4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				float num = Mathf.Max(this.flightDistance, 1f) / this.def.pawnFlyer.flightSpeed;
				num = Mathf.Max(num, this.def.pawnFlyer.flightDurationMin);
				this.ticksFlightTime = num.SecondsToTicks();
				this.ticksFlying = 0;
			}
		}

		// Token: 0x0600F6AD RID: 63149 RVA: 0x0047A030 File Offset: 0x00478230
		protected virtual void RespawnPawn()
		{
			Thing flyingThing = this.FlyingThing;
			this.LandingEffects();
			Thing thing;
			this.innerContainer.TryDrop(flyingThing, this.destCell, flyingThing.MapHeld, ThingPlaceMode.Direct, out thing, null, null, false);
			Pawn pawn = flyingThing as Pawn;
			if (((pawn != null) ? pawn.drafter : null) != null)
			{
				pawn.drafter.Drafted = this.pawnWasDrafted;
				pawn.drafter.FireAtWill = this.pawnCanFireAtWill;
			}
			flyingThing.Rotation = base.Rotation;
			if (this.carriedThing != null && this.innerContainer.TryDrop(this.carriedThing, this.destCell, flyingThing.MapHeld, ThingPlaceMode.Direct, out thing, null, null, false) && pawn != null)
			{
				this.carriedThing.DeSpawn(DestroyMode.Vanish);
				if (!pawn.carryTracker.TryStartCarry(this.carriedThing))
				{
					Log.Error("Could not carry " + this.carriedThing.ToStringSafe<Thing>() + " after respawning flyer pawn.");
				}
			}
			if (pawn != null)
			{
				if (this.jobQueue != null)
				{
					pawn.jobs.RestoreCapturedJobs(this.jobQueue, true);
				}
				pawn.jobs.CheckForJobOverride(0f, true);
				if (this.def.pawnFlyer.stunDurationTicksRange != IntRange.Zero)
				{
					pawn.stances.stunner.StunFor(this.def.pawnFlyer.stunDurationTicksRange.RandomInRange, null, false, false, false);
				}
				if (this.triggeringAbility != null)
				{
					Ability ability = pawn.abilities.GetAbility(this.triggeringAbility, false);
					if (((ability != null) ? ability.comps : null) != null)
					{
						foreach (AbilityComp abilityComp in ability.comps)
						{
							ICompAbilityEffectOnJumpCompleted compAbilityEffectOnJumpCompleted = abilityComp as ICompAbilityEffectOnJumpCompleted;
							if (compAbilityEffectOnJumpCompleted != null)
							{
								compAbilityEffectOnJumpCompleted.OnJumpCompleted(this.startVec.ToIntVec3(), this.target);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600F6AE RID: 63150 RVA: 0x0047A21C File Offset: 0x0047841C
		private void LandingEffects()
		{
			SoundDef soundDef = this.soundLanding;
			if (soundDef != null)
			{
				soundDef.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			FleckMaker.ThrowDustPuff(this.DestinationPos + Gen.RandomHorizontalVector(0.5f), base.Map, 2f);
		}

		// Token: 0x0600F6AF RID: 63151 RVA: 0x0047A278 File Offset: 0x00478478
		protected override void TickInterval(int delta)
		{
			if (this.flightEffecter == null && this.flightEffecterDef != null)
			{
				this.flightEffecter = this.flightEffecterDef.Spawn();
				this.flightEffecter.Trigger(this, TargetInfo.Invalid, -1);
			}
			else
			{
				Effecter effecter = this.flightEffecter;
				if (effecter != null)
				{
					effecter.EffectTick(this, TargetInfo.Invalid);
				}
			}
			if (this.ticksFlying >= this.ticksFlightTime)
			{
				this.RespawnPawn();
				this.Destroy(DestroyMode.Vanish);
			}
			else if (this.IsHashIntervalTick(15, delta))
			{
				this.CheckDestination();
			}
			this.ticksFlying += delta;
		}

		// Token: 0x0600F6B0 RID: 63152 RVA: 0x0047A318 File Offset: 0x00478518
		private void CheckDestination()
		{
			if (JumpUtility.ValidJumpTarget(this.FlyingThing, base.Map, this.destCell))
			{
				return;
			}
			int num = GenRadial.NumCellsInRadius(3.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 cell = this.destCell + GenRadial.RadialPattern[i];
				if (JumpUtility.ValidJumpTarget(this.FlyingThing, base.Map, cell))
				{
					this.destCell = cell;
					return;
				}
			}
		}

		// Token: 0x0600F6B1 RID: 63153 RVA: 0x0047A38C File Offset: 0x0047858C
		public void Notify_TransportedOnGravship(Gravship gravship)
		{
			IntVec3 b = gravship.Engine.Position - gravship.originalPosition;
			this.startVec += b.ToVector3();
			this.destCell += b;
			this.positionLastComputedTick = -1;
		}

		// Token: 0x0600F6B2 RID: 63154 RVA: 0x0047A3E4 File Offset: 0x004785E4
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			this.RecomputePosition();
			if (this.FlyingPawn != null)
			{
				this.FlyingPawn.DynamicDrawPhaseAt(phase, this.effectivePos, false);
			}
			else
			{
				Thing flyingThing = this.FlyingThing;
				if (flyingThing != null)
				{
					flyingThing.DynamicDrawPhaseAt(phase, this.effectivePos, false);
				}
			}
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
		}

		// Token: 0x0600F6B3 RID: 63155 RVA: 0x0047A436 File Offset: 0x00478636
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DrawShadow(this.groundPos, this.effectiveHeight);
			if (this.CarriedThing != null && this.FlyingPawn != null)
			{
				PawnRenderUtility.DrawCarriedThing(this.FlyingPawn, this.effectivePos, this.CarriedThing);
			}
		}

		// Token: 0x0600F6B4 RID: 63156 RVA: 0x0047A474 File Offset: 0x00478674
		private void DrawShadow(Vector3 drawLoc, float height)
		{
			Material shadowMaterial = this.def.pawnFlyer.ShadowMaterial;
			if (shadowMaterial == null)
			{
				return;
			}
			float num = Mathf.Lerp(1f, 0.6f, height);
			Vector3 s = new Vector3(num, 1f, num);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawLoc, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, shadowMaterial, 0);
		}

		// Token: 0x0600F6B5 RID: 63157 RVA: 0x0047A4E0 File Offset: 0x004786E0
		public static PawnFlyer MakeFlyer(ThingDef flyingDef, Pawn pawn, IntVec3 destCell, EffecterDef flightEffecterDef, SoundDef landingSound, bool flyWithCarriedThing = false, Vector3? overrideStartVec = null, Ability triggeringAbility = null, LocalTargetInfo target = default(LocalTargetInfo))
		{
			PawnFlyer pawnFlyer = (PawnFlyer)ThingMaker.MakeThing(flyingDef, null);
			pawnFlyer.startVec = (overrideStartVec ?? pawn.TrueCenter());
			pawnFlyer.Rotation = pawn.Rotation;
			pawnFlyer.flightDistance = pawn.Position.DistanceTo(destCell);
			pawnFlyer.destCell = destCell;
			pawnFlyer.pawnWasDrafted = pawn.Drafted;
			pawnFlyer.flightEffecterDef = flightEffecterDef;
			pawnFlyer.soundLanding = landingSound;
			pawnFlyer.triggeringAbility = ((triggeringAbility != null) ? triggeringAbility.def : null);
			pawnFlyer.target = target;
			if (pawn.drafter != null)
			{
				pawnFlyer.pawnCanFireAtWill = pawn.drafter.FireAtWill;
			}
			if (pawn.CurJob != null)
			{
				if (pawn.CurJob.def == JobDefOf.CastJump)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
				else
				{
					pawn.jobs.SuspendCurrentJob(JobCondition.InterruptForced, true, null);
				}
			}
			pawnFlyer.jobQueue = pawn.jobs.CaptureAndClearJobQueue();
			if (flyWithCarriedThing && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Direct, out pawnFlyer.carriedThing, null))
			{
				if (pawnFlyer.carriedThing.holdingOwner != null)
				{
					pawnFlyer.carriedThing.holdingOwner.Remove(pawnFlyer.carriedThing);
				}
				pawnFlyer.carriedThing.DeSpawn(DestroyMode.Vanish);
			}
			if (pawn.Spawned)
			{
				pawn.DeSpawn(DestroyMode.WillReplace);
			}
			if (!pawnFlyer.innerContainer.TryAdd(pawn, true))
			{
				Log.Error("Could not add " + pawn.ToStringSafe<Pawn>() + " to a flyer.");
				pawn.Destroy(DestroyMode.Vanish);
			}
			if (pawnFlyer.carriedThing != null && !pawnFlyer.innerContainer.TryAdd(pawnFlyer.carriedThing, true))
			{
				Log.Error("Could not add " + pawnFlyer.carriedThing.ToStringSafe<Thing>() + " to a flyer.");
			}
			return pawnFlyer;
		}

		// Token: 0x0600F6B6 RID: 63158 RVA: 0x0047A6BC File Offset: 0x004788BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector3>(ref this.startVec, "startVec", default(Vector3), false);
			Scribe_Values.Look<IntVec3>(ref this.destCell, "destCell", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.flightDistance, "flightDistance", 0f, false);
			Scribe_Values.Look<bool>(ref this.pawnWasDrafted, "pawnWasDrafted", false, false);
			Scribe_Values.Look<bool>(ref this.pawnCanFireAtWill, "pawnCanFireAtWill", true, false);
			Scribe_Values.Look<int>(ref this.ticksFlightTime, "ticksFlightTime", 0, false);
			Scribe_Values.Look<int>(ref this.ticksFlying, "ticksFlying", 0, false);
			Scribe_Defs.Look<EffecterDef>(ref this.flightEffecterDef, "flightEffecterDef");
			Scribe_Defs.Look<SoundDef>(ref this.soundLanding, "soundLanding");
			Scribe_Defs.Look<AbilityDef>(ref this.triggeringAbility, "triggeringAbility");
			Scribe_References.Look<Thing>(ref this.carriedThing, "carriedThing", false);
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Deep.Look<JobQueue>(ref this.jobQueue, "jobQueue", Array.Empty<object>());
			Scribe_TargetInfo.Look(ref this.target, "target");
		}

		// Token: 0x0400A24C RID: 41548
		private ThingOwner<Thing> innerContainer;

		// Token: 0x0400A24D RID: 41549
		protected Vector3 startVec;

		// Token: 0x0400A24E RID: 41550
		private IntVec3 destCell;

		// Token: 0x0400A24F RID: 41551
		private float flightDistance;

		// Token: 0x0400A250 RID: 41552
		private bool pawnWasDrafted;

		// Token: 0x0400A251 RID: 41553
		private bool pawnCanFireAtWill = true;

		// Token: 0x0400A252 RID: 41554
		protected int ticksFlightTime = 120;

		// Token: 0x0400A253 RID: 41555
		protected int ticksFlying;

		// Token: 0x0400A254 RID: 41556
		private JobQueue jobQueue;

		// Token: 0x0400A255 RID: 41557
		protected EffecterDef flightEffecterDef;

		// Token: 0x0400A256 RID: 41558
		protected SoundDef soundLanding;

		// Token: 0x0400A257 RID: 41559
		private Thing carriedThing;

		// Token: 0x0400A258 RID: 41560
		private LocalTargetInfo target;

		// Token: 0x0400A259 RID: 41561
		private AbilityDef triggeringAbility;

		// Token: 0x0400A25A RID: 41562
		private Effecter flightEffecter;

		// Token: 0x0400A25B RID: 41563
		private int positionLastComputedTick = -1;

		// Token: 0x0400A25C RID: 41564
		private Vector3 groundPos;

		// Token: 0x0400A25D RID: 41565
		private Vector3 effectivePos;

		// Token: 0x0400A25E RID: 41566
		private float effectiveHeight;

		// Token: 0x0400A25F RID: 41567
		private const int CheckDestinationInterval = 15;
	}
}
