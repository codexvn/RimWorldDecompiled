using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200385F RID: 14431
	public class Pawn_FlightTracker : IExposable
	{
		// Token: 0x170032F0 RID: 13040
		// (get) Token: 0x06013F36 RID: 81718 RVA: 0x005EB4AD File Offset: 0x005E96AD
		public bool Flying
		{
			get
			{
				return this.flightState > Pawn_FlightTracker.FlightState.Grounded;
			}
		}

		// Token: 0x170032F1 RID: 13041
		// (get) Token: 0x06013F37 RID: 81719 RVA: 0x005EB4B8 File Offset: 0x005E96B8
		public bool CanEverFly
		{
			get
			{
				return (!this.pawn.IsMutant || !this.pawn.mutant.Def.disableFlying) && (this.pawn.RaceProps.canFlyInVacuum || !this.pawn.MapHeld.Biome.inVacuum) && this.pawn.GetStatValue(StatDefOf.MaxFlightTime, true, 300) > 0f;
			}
		}

		// Token: 0x170032F2 RID: 13042
		// (get) Token: 0x06013F38 RID: 81720 RVA: 0x005EB533 File Offset: 0x005E9733
		public bool CanFlyNow
		{
			get
			{
				return !this.Flying && this.CanEverFly && this.flightCooldownTicks <= 0;
			}
		}

		// Token: 0x170032F3 RID: 13043
		// (get) Token: 0x06013F39 RID: 81721 RVA: 0x005EB553 File Offset: 0x005E9753
		public int MaxFlightTicks
		{
			get
			{
				return Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.MaxFlightTime, true, 300) * 60f);
			}
		}

		// Token: 0x170032F4 RID: 13044
		// (get) Token: 0x06013F3A RID: 81722 RVA: 0x005EB578 File Offset: 0x005E9778
		public float PositionOffsetFactor
		{
			get
			{
				switch (this.flightState)
				{
				case Pawn_FlightTracker.FlightState.Flying:
					return 1f;
				case Pawn_FlightTracker.FlightState.TakingOff:
					return Pawn_FlightTracker.TakeoffCurve.Evaluate((float)this.lerpTick / 50f);
				case Pawn_FlightTracker.FlightState.Landing:
					return Pawn_FlightTracker.LandingCurve.Evaluate((float)this.lerpTick / 50f);
				}
				return 0f;
			}
		}

		// Token: 0x06013F3B RID: 81723 RVA: 0x005EB5DF File Offset: 0x005E97DF
		public Pawn_FlightTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06013F3C RID: 81724 RVA: 0x005EB5F8 File Offset: 0x005E97F8
		public void FlightTick()
		{
			this.CheckFlyAnimation();
			switch (this.flightState)
			{
			case Pawn_FlightTracker.FlightState.Grounded:
				if (this.flightCooldownTicks > 0)
				{
					this.flightCooldownTicks--;
				}
				if (this.pawn.CurJob != null && this.pawn.CurJob.flying)
				{
					this.pawn.CurJob.flying = false;
				}
				break;
			case Pawn_FlightTracker.FlightState.Flying:
				this.flyingTicks++;
				if (this.flyingTicks >= this.MaxFlightTicks)
				{
					this.flightState = Pawn_FlightTracker.FlightState.Landing;
					return;
				}
				break;
			case Pawn_FlightTracker.FlightState.TakingOff:
				this.lerpTick++;
				if (this.lerpTick >= 50)
				{
					this.flightState = Pawn_FlightTracker.FlightState.Flying;
					this.lerpTick = 0;
					return;
				}
				break;
			case Pawn_FlightTracker.FlightState.Landing:
				this.lerpTick++;
				if (this.lerpTick >= 50)
				{
					this.flightState = Pawn_FlightTracker.FlightState.Grounded;
					this.lerpTick = 0;
					this.flightCooldownTicks = Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.FlightCooldown, true, -1) * 60f);
					this.flyingTicks = -1;
					if (this.pawn.Downed && !this.pawn.Position.WalkableBy(this.pawn.Map, this.pawn))
					{
						this.pawn.Kill(null, null);
						Corpse corpse = this.pawn.Corpse;
						if (corpse != null && !corpse.Destroyed)
						{
							corpse.Destroy(DestroyMode.Vanish);
							return;
						}
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06013F3D RID: 81725 RVA: 0x005EB77C File Offset: 0x005E997C
		public void Notify_JobStarted(Job job)
		{
			if (!this.CanEverFly)
			{
				return;
			}
			if ((job.def.tryStartFlying || job.def.ifFlyingKeepFlying) && job.def.tryStartFlying && this.CanFlyNow && Rand.Chance((job.def.overrideFlyChance >= 0f) ? job.def.overrideFlyChance : this.pawn.RaceProps.flightStartChanceOnJobStart))
			{
				this.StartFlyingInternal();
				job.flying = true;
				return;
			}
			job.flying = false;
			if (this.Flying)
			{
				this.ForceLand();
			}
		}

		// Token: 0x06013F3E RID: 81726 RVA: 0x005EB81A File Offset: 0x005E9A1A
		public void StartFlying()
		{
			if (!this.CanFlyNow)
			{
				return;
			}
			this.StartFlyingInternal();
		}

		// Token: 0x06013F3F RID: 81727 RVA: 0x005EB82B File Offset: 0x005E9A2B
		private void StartFlyingInternal()
		{
			this.flightState = Pawn_FlightTracker.FlightState.TakingOff;
			this.lerpTick = 0;
			this.flyingTicks = -1;
			this.CheckFlyAnimation();
		}

		// Token: 0x06013F40 RID: 81728 RVA: 0x005EB848 File Offset: 0x005E9A48
		private void CheckFlyAnimation()
		{
			bool flying = this.Flying;
			if (!flying && this.playing != null)
			{
				if (this.pawn.Drawer.renderer.CurAnimation == this.playing)
				{
					this.pawn.Drawer.renderer.SetAnimation(null);
				}
				this.playing = null;
				return;
			}
			if (flying)
			{
				AnimationDef bestFlyAnimation = Pawn_FlightTracker.GetBestFlyAnimation(this.pawn, null);
				if (this.pawn.Drawer.renderer.CurAnimation == bestFlyAnimation)
				{
					return;
				}
				this.pawn.Drawer.renderer.SetAnimation(bestFlyAnimation);
				this.playing = bestFlyAnimation;
			}
		}

		// Token: 0x06013F41 RID: 81729 RVA: 0x005EB8F0 File Offset: 0x005E9AF0
		public void ForceLand()
		{
			if (this.flightState != Pawn_FlightTracker.FlightState.TakingOff && this.flightState != Pawn_FlightTracker.FlightState.Flying)
			{
				return;
			}
			this.flightState = Pawn_FlightTracker.FlightState.Landing;
			this.lerpTick = 25;
			this.flightCooldownTicks = Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.FlightCooldown, true, -1) * 60f) + 50;
		}

		// Token: 0x06013F42 RID: 81730 RVA: 0x005EB948 File Offset: 0x005E9B48
		public string GetStatusString()
		{
			if (this.Flying)
			{
				return "Flying".Translate().CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x06013F43 RID: 81731 RVA: 0x005EB978 File Offset: 0x005E9B78
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.flyingTicks, "flyingTicks", -1, false);
			Scribe_Values.Look<int>(ref this.flightCooldownTicks, "flightCooldownTicks", 0, false);
			Scribe_Values.Look<int>(ref this.lerpTick, "lerpTick", 0, false);
			Scribe_Values.Look<Pawn_FlightTracker.FlightState>(ref this.flightState, "flightState", Pawn_FlightTracker.FlightState.Grounded, false);
			Scribe_Defs.Look<AnimationDef>(ref this.playing, "playing");
		}

		// Token: 0x06013F44 RID: 81732 RVA: 0x005EB9E0 File Offset: 0x005E9BE0
		public static AnimationDef GetBestFlyAnimation(Pawn pawn, Rot4? facingOverride = null)
		{
			if (((pawn != null) ? pawn.kindDef : null) == null)
			{
				return null;
			}
			if (pawn.RaceProps.Humanlike)
			{
				return null;
			}
			Rot4 a = facingOverride ?? pawn.Rotation;
			bool flag = a == Rot4.South;
			bool flag2 = a == Rot4.North;
			bool flag3 = pawn.gender == Gender.Female;
			Pawn_AgeTracker ageTracker = pawn.ageTracker;
			if (ageTracker != null)
			{
				PawnKindLifeStage pawnKindLifeStage = ageTracker.CurKindLifeStage;
				if (pawnKindLifeStage != null)
				{
					if (flag && pawnKindLifeStage.flyingAnimationSouth != null)
					{
						if (flag3 && pawnKindLifeStage.flyingAnimationSouthFemale != null)
						{
							return pawnKindLifeStage.flyingAnimationSouthFemale;
						}
						return pawnKindLifeStage.flyingAnimationSouth;
					}
					else if ((flag2 || flag) && pawnKindLifeStage.flyingAnimationNorth != null)
					{
						if (flag3 && pawnKindLifeStage.flyingAnimationNorthFemale != null)
						{
							return pawnKindLifeStage.flyingAnimationNorthFemale;
						}
						return pawnKindLifeStage.flyingAnimationNorth;
					}
					else if (pawnKindLifeStage.flyingAnimationEast != null)
					{
						if (flag3 && pawnKindLifeStage.flyingAnimationEastFemale != null)
						{
							return pawnKindLifeStage.flyingAnimationEastFemale;
						}
						return pawnKindLifeStage.flyingAnimationEast;
					}
					else if (pawnKindLifeStage.flyingAnimationNorth != null)
					{
						if (flag3 && pawnKindLifeStage.flyingAnimationNorthFemale != null)
						{
							return pawnKindLifeStage.flyingAnimationNorthFemale;
						}
						return pawnKindLifeStage.flyingAnimationNorth;
					}
					else if (pawnKindLifeStage.flyingAnimationSouth != null)
					{
						if (flag3 && pawnKindLifeStage.flyingAnimationSouthFemale != null)
						{
							return pawnKindLifeStage.flyingAnimationSouthFemale;
						}
						return pawnKindLifeStage.flyingAnimationSouth;
					}
				}
			}
			if (pawn.ageTracker == null)
			{
				return null;
			}
			int i = pawn.ageTracker.CurLifeStageIndex;
			while (i > 0)
			{
				i--;
				PawnKindLifeStage pawnKindLifeStage = pawn.kindDef.lifeStages[i];
				if (flag && pawnKindLifeStage.flyingAnimationSouth != null)
				{
					if (flag3 && pawnKindLifeStage.flyingAnimationSouthFemale != null)
					{
						return pawnKindLifeStage.flyingAnimationSouthFemale;
					}
					return pawnKindLifeStage.flyingAnimationSouth;
				}
				else if ((flag2 || flag) && pawnKindLifeStage.flyingAnimationNorth != null)
				{
					if (flag3 && pawnKindLifeStage.flyingAnimationNorthFemale != null)
					{
						return pawnKindLifeStage.flyingAnimationNorthFemale;
					}
					return pawnKindLifeStage.flyingAnimationNorth;
				}
				else if (pawnKindLifeStage.flyingAnimationEast != null)
				{
					if (flag3 && pawnKindLifeStage.flyingAnimationEastFemale != null)
					{
						return pawnKindLifeStage.flyingAnimationEastFemale;
					}
					return pawnKindLifeStage.flyingAnimationEast;
				}
				else if (pawnKindLifeStage.flyingAnimationNorth != null)
				{
					if (flag3 && pawnKindLifeStage.flyingAnimationNorthFemale != null)
					{
						return pawnKindLifeStage.flyingAnimationNorthFemale;
					}
					return pawnKindLifeStage.flyingAnimationNorth;
				}
				else if (pawnKindLifeStage.flyingAnimationSouth != null)
				{
					if (flag3 && pawnKindLifeStage.flyingAnimationSouthFemale != null)
					{
						return pawnKindLifeStage.flyingAnimationSouthFemale;
					}
					return pawnKindLifeStage.flyingAnimationSouth;
				}
			}
			return null;
		}

		// Token: 0x0400DC49 RID: 56393
		private Pawn pawn;

		// Token: 0x0400DC4A RID: 56394
		private int flyingTicks = -1;

		// Token: 0x0400DC4B RID: 56395
		private int flightCooldownTicks;

		// Token: 0x0400DC4C RID: 56396
		private int lerpTick;

		// Token: 0x0400DC4D RID: 56397
		private AnimationDef playing;

		// Token: 0x0400DC4E RID: 56398
		private Pawn_FlightTracker.FlightState flightState;

		// Token: 0x0400DC4F RID: 56399
		private const int TakeoffDurationTicks = 50;

		// Token: 0x0400DC50 RID: 56400
		private const int LandingDurationTicks = 50;

		// Token: 0x0400DC51 RID: 56401
		private static readonly SimpleCurve TakeoffCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 0.6f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x0400DC52 RID: 56402
		private static readonly SimpleCurve LandingCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(0.5f, 0.4f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			}
		};

		// Token: 0x02003860 RID: 14432
		private enum FlightState
		{
			// Token: 0x0400DC54 RID: 56404
			Grounded,
			// Token: 0x0400DC55 RID: 56405
			Flying,
			// Token: 0x0400DC56 RID: 56406
			TakingOff,
			// Token: 0x0400DC57 RID: 56407
			Landing
		}
	}
}
