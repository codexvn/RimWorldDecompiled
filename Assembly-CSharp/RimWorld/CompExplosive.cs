using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompExplosive : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__32 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompExplosive _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCompGetGizmosExtra_003Ed__32(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompExplosive CS_0024_003C_003E8__locals2 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals2.countdownTicksLeft > 0)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Trigger countdown",
						action = delegate
						{
							CS_0024_003C_003E8__locals2.countdownTicksLeft = 1;
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__32 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__32(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public bool wickStarted;

	public int wickTicksLeft;

	private Thing instigator;

	private int countdownTicksLeft = -1;

	public bool destroyedThroughDetonation;

	private List<Thing> thingsIgnoredByExplosion;

	public float? customExplosiveRadius;

	protected Sustainer wickSoundSustainer;

	private OverlayHandle? overlayBurningWick;

	public CompProperties_Explosive Props => (CompProperties_Explosive)props;

	protected int StartWickThreshold => Mathf.RoundToInt(Props.startWickHitPointsPercent * (float)parent.MaxHitPoints);

	protected virtual bool CanEverExplodeFromDamage
	{
		get
		{
			if (Props.chanceNeverExplodeFromDamage < 1E-05f)
			{
				return true;
			}
			Rand.PushState();
			Rand.Seed = parent.thingIDNumber.GetHashCode();
			bool result = Rand.Value > Props.chanceNeverExplodeFromDamage;
			Rand.PopState();
			return result;
		}
	}

	public void AddThingsIgnoredByExplosion(List<Thing> things)
	{
		if (thingsIgnoredByExplosion == null)
		{
			thingsIgnoredByExplosion = new List<Thing>();
		}
		thingsIgnoredByExplosion.AddRange(things);
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_References.Look(ref instigator, "instigator");
		Scribe_Collections.Look(ref thingsIgnoredByExplosion, "thingsIgnoredByExplosion", LookMode.Reference);
		Scribe_Values.Look(ref wickStarted, "wickStarted", defaultValue: false);
		Scribe_Values.Look(ref wickTicksLeft, "wickTicksLeft", 0);
		Scribe_Values.Look(ref destroyedThroughDetonation, "destroyedThroughDetonation", defaultValue: false);
		Scribe_Values.Look(ref countdownTicksLeft, "countdownTicksLeft", 0);
		Scribe_Values.Look(ref customExplosiveRadius, "explosiveRadius", null);
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (Props.countdownTicks.HasValue)
		{
			countdownTicksLeft = Props.countdownTicks.Value.RandomInRange;
		}
		UpdateOverlays();
	}

	public override void CompTick()
	{
		if (countdownTicksLeft > 0)
		{
			countdownTicksLeft--;
			if (countdownTicksLeft == 0)
			{
				StartWick();
				countdownTicksLeft = -1;
			}
		}
		if (!wickStarted)
		{
			return;
		}
		if (wickSoundSustainer == null)
		{
			StartWickSustainer();
		}
		else
		{
			wickSoundSustainer.Maintain();
		}
		if (Props.wickMessages != null)
		{
			foreach (WickMessage wickMessage in Props.wickMessages)
			{
				if (wickMessage.ticksLeft == wickTicksLeft && wickMessage.wickMessagekey != null)
				{
					Messages.Message(wickMessage.wickMessagekey.Translate(parent.GetCustomLabelNoCount(includeHp: false), wickTicksLeft.ToStringSecondsFromTicks()), parent, wickMessage.messageType ?? MessageTypeDefOf.NeutralEvent, historical: false);
				}
			}
		}
		wickTicksLeft--;
		if (wickTicksLeft <= 0)
		{
			Detonate(parent.MapHeld);
		}
	}

	private void StartWickSustainer()
	{
		SoundDefOf.MetalHitImportant.PlayOneShot(new TargetInfo(parent.PositionHeld, parent.MapHeld));
		SoundInfo info = SoundInfo.InMap(parent, MaintenanceType.PerTick);
		wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
	}

	private void EndWickSustainer()
	{
		if (wickSoundSustainer != null)
		{
			wickSoundSustainer.End();
			wickSoundSustainer = null;
		}
	}

	private void UpdateOverlays()
	{
		if (parent.Spawned && Props.drawWick)
		{
			parent.Map.overlayDrawer.Disable(parent, ref overlayBurningWick);
			if (wickStarted)
			{
				overlayBurningWick = parent.Map.overlayDrawer.Enable(parent, OverlayTypes.BurningWick);
			}
		}
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		if (!destroyedThroughDetonation && ((mode == DestroyMode.KillFinalize && Props.explodeOnKilled) || Props.explodeOnDestroyed))
		{
			Detonate(previousMap, ignoreUnspawned: true);
		}
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		base.PostDeSpawn(map, mode);
		EndWickSustainer();
		if (!parent.BeingTransportedOnGravship)
		{
			StopWick();
		}
	}

	public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
	{
		absorbed = false;
		if (!CanEverExplodeFromDamage)
		{
			return;
		}
		if (dinfo.Def.ExternalViolenceFor(parent) && dinfo.Amount >= (float)parent.HitPoints && CanExplodeFromDamageType(dinfo.Def))
		{
			if (parent.MapHeld != null)
			{
				instigator = dinfo.Instigator;
				Detonate(parent.MapHeld);
				if (parent.Destroyed)
				{
					absorbed = true;
				}
			}
		}
		else if (!wickStarted && Props.startWickOnDamageTaken != null && Props.startWickOnDamageTaken.Contains(dinfo.Def))
		{
			StartWick(dinfo.Instigator);
		}
	}

	public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
	{
		if (CanEverExplodeFromDamage && CanExplodeFromDamageType(dinfo.Def) && !parent.Destroyed)
		{
			if (wickStarted && dinfo.Def == DamageDefOf.Stun)
			{
				StopWick();
			}
			else if (!wickStarted && parent.HitPoints <= StartWickThreshold && (dinfo.Def.ExternalViolenceFor(parent) || (!Props.startWickOnInternalDamageTaken.NullOrEmpty() && Props.startWickOnInternalDamageTaken.Contains(dinfo.Def))))
			{
				StartWick(dinfo.Instigator);
			}
		}
	}

	public void StartWick(Thing instigator = null)
	{
		if (!wickStarted && !(ExplosiveRadius() <= 0f))
		{
			this.instigator = instigator;
			wickStarted = true;
			wickTicksLeft = Props.wickTicks.RandomInRange;
			StartWickSustainer();
			GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(parent, Props.explosiveDamageType, null, instigator);
			UpdateOverlays();
		}
	}

	public void StopWick()
	{
		wickStarted = false;
		instigator = null;
		UpdateOverlays();
		EndWickSustainer();
	}

	public float ExplosiveRadius()
	{
		CompProperties_Explosive compProperties_Explosive = Props;
		float num = customExplosiveRadius ?? Props.explosiveRadius;
		if (parent.stackCount > 1 && compProperties_Explosive.explosiveExpandPerStackcount > 0f)
		{
			num += Mathf.Sqrt((float)(parent.stackCount - 1) * compProperties_Explosive.explosiveExpandPerStackcount);
		}
		if (compProperties_Explosive.explosiveExpandPerFuel > 0f && parent.GetComp<CompRefuelable>() != null)
		{
			num += Mathf.Sqrt(parent.GetComp<CompRefuelable>().Fuel * compProperties_Explosive.explosiveExpandPerFuel);
		}
		return num;
	}

	protected void Detonate(Map map, bool ignoreUnspawned = false)
	{
		if (!ignoreUnspawned && !parent.SpawnedOrAnyParentSpawned)
		{
			return;
		}
		CompProperties_Explosive compProperties_Explosive = Props;
		float num = ExplosiveRadius();
		if (num <= 0f)
		{
			return;
		}
		Thing thing = ((instigator == null || (instigator.HostileTo(parent.Faction) && parent.Faction != Faction.OfPlayer)) ? parent : instigator);
		if (compProperties_Explosive.explosiveExpandPerFuel > 0f && parent.GetComp<CompRefuelable>() != null)
		{
			parent.GetComp<CompRefuelable>().ConsumeFuel(parent.GetComp<CompRefuelable>().Fuel);
		}
		if (compProperties_Explosive.destroyThingOnExplosionSize <= num && !parent.Destroyed)
		{
			destroyedThroughDetonation = true;
			parent.Kill(null);
		}
		EndWickSustainer();
		wickStarted = false;
		UpdateOverlays();
		if (map == null)
		{
			Log.Warning("Tried to detonate CompExplosive in a null map.");
			return;
		}
		if (compProperties_Explosive.explosionEffect != null)
		{
			Effecter effecter = compProperties_Explosive.explosionEffect.Spawn();
			effecter.Trigger(new TargetInfo(parent.PositionHeld, map), new TargetInfo(parent.PositionHeld, map));
			effecter.Cleanup();
		}
		IntVec3 positionHeld = parent.PositionHeld;
		DamageDef explosiveDamageType = compProperties_Explosive.explosiveDamageType;
		int damageAmountBase = compProperties_Explosive.damageAmountBase;
		float armorPenetrationBase = compProperties_Explosive.armorPenetrationBase;
		SoundDef explosionSound = compProperties_Explosive.explosionSound;
		ThingDef postExplosionSpawnThingDef = compProperties_Explosive.postExplosionSpawnThingDef;
		float postExplosionSpawnChance = compProperties_Explosive.postExplosionSpawnChance;
		int postExplosionSpawnThingCount = compProperties_Explosive.postExplosionSpawnThingCount;
		GasType? postExplosionGasType = Props.postExplosionGasType;
		float? postExplosionGasRadiusOverride = Props.postExplosionGasRadiusOverride;
		int postExplosionGasAmount = Props.postExplosionGasAmount;
		bool applyDamageToExplosionCellsNeighbors = compProperties_Explosive.applyDamageToExplosionCellsNeighbors;
		ThingDef preExplosionSpawnThingDef = compProperties_Explosive.preExplosionSpawnThingDef;
		float preExplosionSpawnChance = compProperties_Explosive.preExplosionSpawnChance;
		int preExplosionSpawnThingCount = compProperties_Explosive.preExplosionSpawnThingCount;
		float chanceToStartFire = compProperties_Explosive.chanceToStartFire;
		bool damageFalloff = compProperties_Explosive.damageFalloff;
		List<Thing> ignoredThings = thingsIgnoredByExplosion;
		bool doVisualEffects = compProperties_Explosive.doVisualEffects;
		bool doSoundEffects = compProperties_Explosive.doSoundEffects;
		float propagationSpeed = compProperties_Explosive.propagationSpeed;
		ThingDef preExplosionSpawnSingleThingDef = compProperties_Explosive.preExplosionSpawnSingleThingDef;
		ThingDef postExplosionSpawnSingleThingDef = compProperties_Explosive.postExplosionSpawnSingleThingDef;
		GenExplosion.DoExplosion(positionHeld, map, num, explosiveDamageType, thing, damageAmountBase, armorPenetrationBase, explosionSound, null, null, null, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, postExplosionGasType, postExplosionGasRadiusOverride, postExplosionGasAmount, applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, preExplosionSpawnChance, preExplosionSpawnThingCount, chanceToStartFire, damageFalloff, null, ignoredThings, null, doVisualEffects, propagationSpeed, 0f, doSoundEffects, null, 1f, null, null, postExplosionSpawnSingleThingDef, preExplosionSpawnSingleThingDef);
	}

	private bool CanExplodeFromDamageType(DamageDef damage)
	{
		if (Props.requiredDamageTypeToExplode != null)
		{
			return Props.requiredDamageTypeToExplode == damage;
		}
		return true;
	}

	public override string CompInspectStringExtra()
	{
		string text = "";
		if (countdownTicksLeft != -1)
		{
			text += "DetonationCountdown".Translate(countdownTicksLeft.TicksToDays().ToString("0.0"));
		}
		if (Props.extraInspectStringKey != null)
		{
			text += ((text != "") ? "\n" : "") + Props.extraInspectStringKey.Translate();
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__32))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__32(-2)
		{
			_003C_003E4__this = this
		};
	}
}
