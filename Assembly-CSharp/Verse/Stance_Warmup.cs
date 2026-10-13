using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020007F1 RID: 2033
	public class Stance_Warmup : Stance_Busy
	{
		// Token: 0x060033F2 RID: 13298 RVA: 0x0011074B File Offset: 0x0010E94B
		public Stance_Warmup()
		{
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x0011075C File Offset: 0x0010E95C
		public Stance_Warmup(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
			if (focusTarg.HasThing)
			{
				Pawn pawn = focusTarg.Thing as Pawn;
				if (pawn != null)
				{
					this.targetStartedDowned = pawn.Downed;
					if (pawn.apparel != null)
					{
						Verb_CastAbility verb_CastAbility = verb as Verb_CastAbility;
						if (verb_CastAbility == null || verb_CastAbility.Ability.def.hostile)
						{
							for (int i = 0; i < pawn.apparel.WornApparelCount; i++)
							{
								List<ThingComp> allComps = pawn.apparel.WornApparel[i].AllComps;
								for (int j = 0; j < allComps.Count; j++)
								{
									CompShield compShield = allComps[j] as CompShield;
									if (compShield != null)
									{
										compShield.KeepDisplaying();
									}
								}
							}
						}
					}
				}
			}
			this.InitEffects(false);
			this.drawAimPie = (verb != null && verb.verbProps.drawAimPie);
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x00110844 File Offset: 0x0010EA44
		public void InitEffects(bool afterReload = false)
		{
			if (this.verb == null)
			{
				return;
			}
			VerbProperties verbProps = this.verb.verbProps;
			if (verbProps.soundAiming != null)
			{
				SoundInfo info = SoundInfo.InMap(this.verb.caster, MaintenanceType.PerTick);
				if (this.verb.CasterIsPawn)
				{
					info.pitchFactor = 1f / this.verb.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true, -1);
				}
				this.sustainer = verbProps.soundAiming.TrySpawnSustainer(info);
			}
			if (verbProps.warmupEffecter != null && this.verb.Caster != null)
			{
				this.effecter = verbProps.warmupEffecter.Spawn(this.verb.Caster, this.verb.Caster.Map, 1f);
				this.effecter.Trigger(this.verb.Caster, this.focusTarg.ToTargetInfo(this.verb.Caster.Map), -1);
			}
			if (this.verb.Caster != null)
			{
				Map map = this.verb.Caster.Map;
				if (verbProps.aimingLineMote != null)
				{
					Vector3 vector = this.TargetPos();
					IntVec3 cell = vector.ToIntVec3();
					this.aimLineMote = MoteMaker.MakeInteractionOverlay(verbProps.aimingLineMote, this.verb.Caster, new TargetInfo(cell, map, false), Vector3.zero, vector - cell.ToVector3Shifted());
					if (afterReload)
					{
						Mote mote = this.aimLineMote;
						if (mote != null)
						{
							mote.ForceSpawnTick(this.startedTick);
						}
					}
				}
				if (verbProps.aimingChargeMote != null)
				{
					this.aimChargeMote = MoteMaker.MakeStaticMote(this.verb.Caster.DrawPos, map, verbProps.aimingChargeMote, 1f, true, 0f);
					if (afterReload)
					{
						Mote mote2 = this.aimChargeMote;
						if (mote2 != null)
						{
							mote2.ForceSpawnTick(this.startedTick);
						}
					}
				}
				if (verbProps.aimingTargetMote != null)
				{
					this.aimTargetMote = MoteMaker.MakeStaticMote(this.focusTarg.CenterVector3, map, verbProps.aimingTargetMote, 1f, true, 0f);
					if (this.aimTargetMote != null)
					{
						this.aimTargetMote.exactRotation = this.AimDir().ToAngleFlat();
						if (afterReload)
						{
							this.aimTargetMote.ForceSpawnTick(this.startedTick);
						}
					}
				}
				if (verbProps.aimingTargetEffecter != null)
				{
					this.effecter = verbProps.aimingTargetEffecter.Spawn(new TargetInfo(this.verb.Caster), this.focusTarg.ToTargetInfo(map), 1f);
				}
			}
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x00110AC0 File Offset: 0x0010ECC0
		private Vector3 TargetPos()
		{
			VerbProperties verbProps = this.verb.verbProps;
			Vector3 result = this.focusTarg.CenterVector3;
			if (verbProps.aimingLineMoteFixedLength != null)
			{
				result = this.verb.Caster.DrawPos + this.AimDir() * verbProps.aimingLineMoteFixedLength.Value;
			}
			return result;
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x00110B20 File Offset: 0x0010ED20
		private Vector3 AimDir()
		{
			Vector3 result = this.focusTarg.CenterVector3 - this.verb.Caster.DrawPos;
			result.y = 0f;
			result.Normalize();
			return result;
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x00110B62 File Offset: 0x0010ED62
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.targetStartedDowned, "targetStartDowned", false, false);
			Scribe_Values.Look<bool>(ref this.drawAimPie, "drawAimPie", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.needsReInitAfterLoad = true;
			}
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x00110BA0 File Offset: 0x0010EDA0
		public override void StanceDraw()
		{
			if (this.drawAimPie && Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				GenDraw.DrawAimPie(this.stanceTracker.pawn, this.focusTarg, (int)((float)this.ticksLeft * this.pieSizeFactor), 0.2f);
			}
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x00110BF8 File Offset: 0x0010EDF8
		public override void StanceTick()
		{
			if (this.needsReInitAfterLoad)
			{
				this.InitEffects(true);
				this.needsReInitAfterLoad = false;
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.EffectTick(this.verb.Caster, this.focusTarg.ToTargetInfo(this.verb.Caster.Map));
			}
			Vector3 vector = this.AimDir();
			float exactRotation = vector.AngleFlat();
			bool stunned = this.stanceTracker.stunner.Stunned;
			if (this.aimLineMote != null)
			{
				this.aimLineMote.paused = stunned;
				this.aimLineMote.Maintain();
				Vector3 vector2 = this.TargetPos();
				IntVec3 cell = vector2.ToIntVec3();
				((MoteDualAttached)this.aimLineMote).UpdateTargets(this.verb.Caster, new TargetInfo(cell, this.verb.Caster.Map, false), Vector3.zero, vector2 - cell.ToVector3Shifted());
			}
			if (this.aimTargetMote != null)
			{
				this.aimTargetMote.paused = stunned;
				this.aimTargetMote.exactPosition = this.focusTarg.CenterVector3;
				this.aimTargetMote.exactRotation = exactRotation;
				Mote mote = this.aimTargetMote;
				if (mote != null)
				{
					mote.Maintain();
				}
			}
			if (this.aimChargeMote != null)
			{
				this.aimChargeMote.paused = stunned;
				this.aimChargeMote.exactRotation = exactRotation;
				this.aimChargeMote.exactPosition = this.verb.Caster.Position.ToVector3Shifted() + vector * this.verb.verbProps.aimingChargeMoteOffset;
				Mote mote2 = this.aimChargeMote;
				if (mote2 != null)
				{
					mote2.Maintain();
				}
			}
			if (!this.stanceTracker.stunner.Stunned)
			{
				if (!this.targetStartedDowned && this.focusTarg.HasThing && this.focusTarg.Thing is Pawn && ((Pawn)this.focusTarg.Thing).Downed)
				{
					this.stanceTracker.SetStance(new Stance_Mobile());
					return;
				}
				if (this.focusTarg.HasThing && (!this.focusTarg.Thing.Spawned || this.verb == null || !this.verb.CanHitTargetFrom(base.Pawn.Position, this.focusTarg)))
				{
					this.stanceTracker.SetStance(new Stance_Mobile());
					return;
				}
				if (this.focusTarg == base.Pawn.mindState.enemyTarget)
				{
					base.Pawn.mindState.Notify_EngagedTarget();
				}
			}
			base.StanceTick();
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x00110EB7 File Offset: 0x0010F0B7
		public void Interrupt()
		{
			base.Expire();
			Effecter effecter = this.effecter;
			if (effecter == null)
			{
				return;
			}
			effecter.Cleanup();
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x00110ECF File Offset: 0x0010F0CF
		protected override void Expire()
		{
			Verb verb = this.verb;
			if (verb != null)
			{
				verb.WarmupComplete();
			}
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			base.Expire();
		}

		// Token: 0x04002733 RID: 10035
		private Sustainer sustainer;

		// Token: 0x04002734 RID: 10036
		protected Effecter effecter;

		// Token: 0x04002735 RID: 10037
		private Mote aimLineMote;

		// Token: 0x04002736 RID: 10038
		private Mote aimChargeMote;

		// Token: 0x04002737 RID: 10039
		private Mote aimTargetMote;

		// Token: 0x04002738 RID: 10040
		private bool targetStartedDowned;

		// Token: 0x04002739 RID: 10041
		private bool drawAimPie = true;

		// Token: 0x0400273A RID: 10042
		private bool needsReInitAfterLoad;
	}
}
