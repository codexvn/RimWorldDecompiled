using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C7B RID: 11387
	public class CompExplosive : ThingComp
	{
		// Token: 0x1700284F RID: 10319
		// (get) Token: 0x0600FF7F RID: 65407 RVA: 0x004A05EF File Offset: 0x0049E7EF
		public CompProperties_Explosive Props
		{
			get
			{
				return (CompProperties_Explosive)this.props;
			}
		}

		// Token: 0x17002850 RID: 10320
		// (get) Token: 0x0600FF80 RID: 65408 RVA: 0x004A05FC File Offset: 0x0049E7FC
		protected int StartWickThreshold
		{
			get
			{
				return Mathf.RoundToInt(this.Props.startWickHitPointsPercent * (float)this.parent.MaxHitPoints);
			}
		}

		// Token: 0x17002851 RID: 10321
		// (get) Token: 0x0600FF81 RID: 65409 RVA: 0x004A061C File Offset: 0x0049E81C
		protected virtual bool CanEverExplodeFromDamage
		{
			get
			{
				if (this.Props.chanceNeverExplodeFromDamage < 1E-05f)
				{
					return true;
				}
				Rand.PushState();
				Rand.Seed = this.parent.thingIDNumber.GetHashCode();
				bool result = Rand.Value > this.Props.chanceNeverExplodeFromDamage;
				Rand.PopState();
				return result;
			}
		}

		// Token: 0x0600FF82 RID: 65410 RVA: 0x004A066E File Offset: 0x0049E86E
		public void AddThingsIgnoredByExplosion(List<Thing> things)
		{
			if (this.thingsIgnoredByExplosion == null)
			{
				this.thingsIgnoredByExplosion = new List<Thing>();
			}
			this.thingsIgnoredByExplosion.AddRange(things);
		}

		// Token: 0x0600FF83 RID: 65411 RVA: 0x004A0690 File Offset: 0x0049E890
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Collections.Look<Thing>(ref this.thingsIgnoredByExplosion, "thingsIgnoredByExplosion", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.wickStarted, "wickStarted", false, false);
			Scribe_Values.Look<int>(ref this.wickTicksLeft, "wickTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.destroyedThroughDetonation, "destroyedThroughDetonation", false, false);
			Scribe_Values.Look<int>(ref this.countdownTicksLeft, "countdownTicksLeft", 0, false);
			Scribe_Values.Look<float?>(ref this.customExplosiveRadius, "explosiveRadius", null, false);
		}

		// Token: 0x0600FF84 RID: 65412 RVA: 0x004A072C File Offset: 0x0049E92C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.Props.countdownTicks != null)
			{
				this.countdownTicksLeft = this.Props.countdownTicks.Value.RandomInRange;
			}
			this.UpdateOverlays();
		}

		// Token: 0x0600FF85 RID: 65413 RVA: 0x004A0770 File Offset: 0x0049E970
		public override void CompTick()
		{
			if (this.countdownTicksLeft > 0)
			{
				this.countdownTicksLeft--;
				if (this.countdownTicksLeft == 0)
				{
					this.StartWick(null);
					this.countdownTicksLeft = -1;
				}
			}
			if (this.wickStarted)
			{
				if (this.wickSoundSustainer == null)
				{
					this.StartWickSustainer();
				}
				else
				{
					this.wickSoundSustainer.Maintain();
				}
				if (this.Props.wickMessages != null)
				{
					foreach (WickMessage wickMessage in this.Props.wickMessages)
					{
						if (wickMessage.ticksLeft == this.wickTicksLeft && wickMessage.wickMessagekey != null)
						{
							Messages.Message(wickMessage.wickMessagekey.Translate(this.parent.GetCustomLabelNoCount(false), this.wickTicksLeft.ToStringSecondsFromTicks()), this.parent, wickMessage.messageType ?? MessageTypeDefOf.NeutralEvent, false);
						}
					}
				}
				this.wickTicksLeft--;
				if (this.wickTicksLeft <= 0)
				{
					this.Detonate(this.parent.MapHeld, false);
				}
			}
		}

		// Token: 0x0600FF86 RID: 65414 RVA: 0x004A08B4 File Offset: 0x0049EAB4
		private void StartWickSustainer()
		{
			SoundDefOf.MetalHitImportant.PlayOneShot(new TargetInfo(this.parent.PositionHeld, this.parent.MapHeld, false));
			SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.PerTick);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		// Token: 0x0600FF87 RID: 65415 RVA: 0x004A090F File Offset: 0x0049EB0F
		private void EndWickSustainer()
		{
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
		}

		// Token: 0x0600FF88 RID: 65416 RVA: 0x004A092C File Offset: 0x0049EB2C
		private void UpdateOverlays()
		{
			if (!this.parent.Spawned || !this.Props.drawWick)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayBurningWick);
			if (this.wickStarted)
			{
				this.overlayBurningWick = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.BurningWick));
			}
		}

		// Token: 0x0600FF89 RID: 65417 RVA: 0x004A09A4 File Offset: 0x0049EBA4
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if ((mode == DestroyMode.KillFinalize && this.Props.explodeOnKilled) || this.Props.explodeOnDestroyed)
			{
				this.Detonate(previousMap, true);
			}
		}

		// Token: 0x0600FF8A RID: 65418 RVA: 0x004A09CC File Offset: 0x0049EBCC
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			this.EndWickSustainer();
			if (this.parent.BeingTransportedOnGravship)
			{
				return;
			}
			this.StopWick();
		}

		// Token: 0x0600FF8B RID: 65419 RVA: 0x004A09F0 File Offset: 0x0049EBF0
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			if (this.CanEverExplodeFromDamage)
			{
				if (dinfo.Def.ExternalViolenceFor(this.parent) && dinfo.Amount >= (float)this.parent.HitPoints && this.CanExplodeFromDamageType(dinfo.Def))
				{
					if (this.parent.MapHeld != null)
					{
						this.instigator = dinfo.Instigator;
						this.Detonate(this.parent.MapHeld, false);
						if (this.parent.Destroyed)
						{
							absorbed = true;
							return;
						}
					}
				}
				else if (!this.wickStarted && this.Props.startWickOnDamageTaken != null && this.Props.startWickOnDamageTaken.Contains(dinfo.Def))
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		// Token: 0x0600FF8C RID: 65420 RVA: 0x004A0AB8 File Offset: 0x0049ECB8
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!this.CanEverExplodeFromDamage)
			{
				return;
			}
			if (!this.CanExplodeFromDamageType(dinfo.Def))
			{
				return;
			}
			if (!this.parent.Destroyed)
			{
				if (this.wickStarted && dinfo.Def == DamageDefOf.Stun)
				{
					this.StopWick();
					return;
				}
				if (!this.wickStarted && this.parent.HitPoints <= this.StartWickThreshold && (dinfo.Def.ExternalViolenceFor(this.parent) || (!this.Props.startWickOnInternalDamageTaken.NullOrEmpty<DamageDef>() && this.Props.startWickOnInternalDamageTaken.Contains(dinfo.Def))))
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		// Token: 0x0600FF8D RID: 65421 RVA: 0x004A0B74 File Offset: 0x0049ED74
		public void StartWick(Thing instigator = null)
		{
			if (this.wickStarted)
			{
				return;
			}
			if (this.ExplosiveRadius() <= 0f)
			{
				return;
			}
			this.instigator = instigator;
			this.wickStarted = true;
			this.wickTicksLeft = this.Props.wickTicks.RandomInRange;
			this.StartWickSustainer();
			GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this.parent, this.Props.explosiveDamageType, null, instigator);
			this.UpdateOverlays();
		}

		// Token: 0x0600FF8E RID: 65422 RVA: 0x004A0BE0 File Offset: 0x0049EDE0
		public void StopWick()
		{
			this.wickStarted = false;
			this.instigator = null;
			this.UpdateOverlays();
			this.EndWickSustainer();
		}

		// Token: 0x0600FF8F RID: 65423 RVA: 0x004A0BFC File Offset: 0x0049EDFC
		public float ExplosiveRadius()
		{
			CompProperties_Explosive props = this.Props;
			float num = this.customExplosiveRadius ?? this.Props.explosiveRadius;
			if (this.parent.stackCount > 1 && props.explosiveExpandPerStackcount > 0f)
			{
				num += Mathf.Sqrt((float)(this.parent.stackCount - 1) * props.explosiveExpandPerStackcount);
			}
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				num += Mathf.Sqrt(this.parent.GetComp<CompRefuelable>().Fuel * props.explosiveExpandPerFuel);
			}
			return num;
		}

		// Token: 0x0600FF90 RID: 65424 RVA: 0x004A0CA8 File Offset: 0x0049EEA8
		protected void Detonate(Map map, bool ignoreUnspawned = false)
		{
			if (!ignoreUnspawned && !this.parent.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			CompProperties_Explosive props = this.Props;
			float num = this.ExplosiveRadius();
			if (num <= 0f)
			{
				return;
			}
			Thing parent;
			if (this.instigator != null && (!this.instigator.HostileTo(this.parent.Faction) || this.parent.Faction == Faction.OfPlayer))
			{
				parent = this.instigator;
			}
			else
			{
				parent = this.parent;
			}
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				this.parent.GetComp<CompRefuelable>().ConsumeFuel(this.parent.GetComp<CompRefuelable>().Fuel);
			}
			if (props.destroyThingOnExplosionSize <= num && !this.parent.Destroyed)
			{
				this.destroyedThroughDetonation = true;
				this.parent.Kill(null, null);
			}
			this.EndWickSustainer();
			this.wickStarted = false;
			this.UpdateOverlays();
			if (map == null)
			{
				Log.Warning("Tried to detonate CompExplosive in a null map.");
				return;
			}
			if (props.explosionEffect != null)
			{
				Effecter effecter = props.explosionEffect.Spawn();
				effecter.Trigger(new TargetInfo(this.parent.PositionHeld, map, false), new TargetInfo(this.parent.PositionHeld, map, false), -1);
				effecter.Cleanup();
			}
			IntVec3 positionHeld = this.parent.PositionHeld;
			float radius = num;
			DamageDef explosiveDamageType = props.explosiveDamageType;
			Thing thing = parent;
			int damageAmountBase = props.damageAmountBase;
			float armorPenetrationBase = props.armorPenetrationBase;
			SoundDef explosionSound = props.explosionSound;
			ThingDef weapon = null;
			ThingDef projectile = null;
			Thing intendedTarget = null;
			ThingDef postExplosionSpawnThingDef = props.postExplosionSpawnThingDef;
			float postExplosionSpawnChance = props.postExplosionSpawnChance;
			int postExplosionSpawnThingCount = props.postExplosionSpawnThingCount;
			GasType? postExplosionGasType = this.Props.postExplosionGasType;
			float? postExplosionGasRadiusOverride = this.Props.postExplosionGasRadiusOverride;
			int postExplosionGasAmount = this.Props.postExplosionGasAmount;
			bool applyDamageToExplosionCellsNeighbors = props.applyDamageToExplosionCellsNeighbors;
			ThingDef preExplosionSpawnThingDef = props.preExplosionSpawnThingDef;
			float preExplosionSpawnChance = props.preExplosionSpawnChance;
			int preExplosionSpawnThingCount = props.preExplosionSpawnThingCount;
			float chanceToStartFire = props.chanceToStartFire;
			bool damageFalloff = props.damageFalloff;
			List<Thing> ignoredThings = this.thingsIgnoredByExplosion;
			bool doVisualEffects = props.doVisualEffects;
			bool doSoundEffects = props.doSoundEffects;
			float propagationSpeed = props.propagationSpeed;
			ThingDef preExplosionSpawnSingleThingDef = props.preExplosionSpawnSingleThingDef;
			ThingDef postExplosionSpawnSingleThingDef = props.postExplosionSpawnSingleThingDef;
			GenExplosion.DoExplosion(positionHeld, map, radius, explosiveDamageType, thing, damageAmountBase, armorPenetrationBase, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, postExplosionGasType, postExplosionGasRadiusOverride, postExplosionGasAmount, applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, preExplosionSpawnChance, preExplosionSpawnThingCount, chanceToStartFire, damageFalloff, null, ignoredThings, null, doVisualEffects, propagationSpeed, 0f, doSoundEffects, null, 1f, null, null, postExplosionSpawnSingleThingDef, preExplosionSpawnSingleThingDef);
		}

		// Token: 0x0600FF91 RID: 65425 RVA: 0x004A0ED2 File Offset: 0x0049F0D2
		private bool CanExplodeFromDamageType(DamageDef damage)
		{
			return this.Props.requiredDamageTypeToExplode == null || this.Props.requiredDamageTypeToExplode == damage;
		}

		// Token: 0x0600FF92 RID: 65426 RVA: 0x004A0EF4 File Offset: 0x0049F0F4
		public override string CompInspectStringExtra()
		{
			string text = "";
			if (this.countdownTicksLeft != -1)
			{
				text += "DetonationCountdown".Translate(this.countdownTicksLeft.TicksToDays().ToString("0.0"));
			}
			if (this.Props.extraInspectStringKey != null)
			{
				text += ((text != "") ? "\n" : "") + this.Props.extraInspectStringKey.Translate();
			}
			return text;
		}

		// Token: 0x0600FF93 RID: 65427 RVA: 0x004A0F8B File Offset: 0x0049F18B
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.countdownTicksLeft > 0)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Trigger countdown",
					action = delegate()
					{
						this.countdownTicksLeft = 1;
					}
				};
			}
			yield break;
		}

		// Token: 0x0400A730 RID: 42800
		public bool wickStarted;

		// Token: 0x0400A731 RID: 42801
		public int wickTicksLeft;

		// Token: 0x0400A732 RID: 42802
		private Thing instigator;

		// Token: 0x0400A733 RID: 42803
		private int countdownTicksLeft = -1;

		// Token: 0x0400A734 RID: 42804
		public bool destroyedThroughDetonation;

		// Token: 0x0400A735 RID: 42805
		private List<Thing> thingsIgnoredByExplosion;

		// Token: 0x0400A736 RID: 42806
		public float? customExplosiveRadius;

		// Token: 0x0400A737 RID: 42807
		protected Sustainer wickSoundSustainer;

		// Token: 0x0400A738 RID: 42808
		private OverlayHandle? overlayBurningWick;
	}
}
