using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse;

public class ProjectileProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__58 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ProjectileProperties _003C_003E4__this;

		private ThingDef parent;

		public ThingDef _003C_003E3__parent;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__58(int _003C_003E1__state)
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
			ProjectileProperties projectileProperties = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (projectileProperties.alwaysFreeIntercept && projectileProperties.flyOverhead)
				{
					_003C_003E2__current = "alwaysFreeIntercept and flyOverhead are both true";
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_005c;
			case 1:
				_003C_003E1__state = -1;
				goto IL_005c;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0096;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00c7;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00c7:
				if (parent.thingClass == typeof(Projectile_SpawnsThing) && projectileProperties.spawnsThingDef == null)
				{
					_003C_003E2__current = "Projectile_SpawnsThing has no spawnsThingDef";
					_003C_003E1__state = 4;
					return true;
				}
				break;
				IL_005c:
				if (projectileProperties.damageAmountBase == -1 && projectileProperties.damageDef != null && projectileProperties.damageDef.defaultDamage == -1)
				{
					_003C_003E2__current = "no damage amount specified for projectile";
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0096;
				IL_0096:
				if (projectileProperties.filth != null && projectileProperties.filthCount.TrueMax <= 0)
				{
					_003C_003E2__current = "has filth but filthCount <= 0.";
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_00c7;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__58 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__58(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parent = _003C_003E3__parent;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public float speed = 5f;

	public bool flyOverhead;

	public bool alwaysFreeIntercept;

	public DamageDef damageDef;

	private int damageAmountBase = -1;

	private float armorPenetrationBase = -1f;

	public float stoppingPower = 0.5f;

	public List<ExtraDamage> extraDamages;

	public float arcHeightFactor;

	public float shadowSize;

	public EffecterDef landedEffecter;

	public float spinRate;

	public bool useGraphicClass;

	public SoundDef soundHitThickRoof;

	public SoundDef soundExplode;

	public SoundDef soundImpactAnticipate;

	public SoundDef soundAmbient;

	public SoundDef soundImpact;

	public float explosionRadius;

	public float explosionRadiusDisplayPadding;

	public int explosionDelay;

	public bool doExplosionVFX = true;

	public ThingDef preExplosionSpawnThingDef;

	public float preExplosionSpawnChance = 1f;

	public int preExplosionSpawnThingCount = 1;

	public ThingDef postExplosionSpawnThingDef;

	public ThingDef postExplosionSpawnThingDefWater;

	public float postExplosionSpawnChance = 1f;

	public int postExplosionSpawnThingCount = 1;

	public GasType? postExplosionGasType;

	public bool applyDamageToExplosionCellsNeighbors;

	public float explosionChanceToStartFire;

	public bool explosionDamageFalloff;

	public float screenShakeFactor = 1f;

	public ThingDef preExplosionSpawnSingleThingDef;

	public ThingDef postExplosionSpawnSingleThingDef;

	public ThingDef filth;

	public float filthChance = 1f;

	public IntRange filthCount;

	public int numExtraHitCells;

	public bool explosionSpawnsSingleFilth;

	public TerrainDef spawnTerrain;

	public float terrainChance = 1f;

	public bool terrainReplacesFloors;

	public EffecterDef explosionEffect;

	public int explosionEffectLifetimeTicks;

	public ThingDef spawnsThingDef;

	public bool tryAdjacentFreeSpaces;

	public PawnKindDef spawnsPawnKind;

	public ThingDef beamMoteDef;

	public float beamStartOffset;

	public bool ai_IsIncendiary;

	public float SpeedTilesPerTick => speed / 100f;

	public int GetDamageAmount(Thing weapon, StringBuilder explanation = null)
	{
		float damageMultiplier = weapon?.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier) ?? 1f;
		return GetDamageAmount(damageMultiplier, weapon, explanation);
	}

	public int GetDamageAmount(ThingDef weapon, ThingDef weaponStuff, StringBuilder explanation = null)
	{
		float damageMultiplier = weapon?.GetStatValueAbstract(StatDefOf.RangedWeapon_DamageMultiplier, weaponStuff) ?? 1f;
		return GetDamageAmount(damageMultiplier, null, explanation);
	}

	public int GetDamageAmount(float damageMultiplier, Thing weapon, StringBuilder explanation = null)
	{
		int num = 0;
		if (damageAmountBase != -1)
		{
			num = damageAmountBase;
		}
		else
		{
			if (damageDef == null)
			{
				Log.ErrorOnce("Failed to find sane damage amount", 91094882);
				return 1;
			}
			num = damageDef.defaultDamage;
		}
		explanation?.AppendLine(string.Concat("StatsReport_BaseValue".Translate() + ": ", num.ToString()));
		num = Mathf.RoundToInt((float)num * damageMultiplier);
		if (!Mathf.Approximately(damageMultiplier, 1f))
		{
			explanation?.AppendLine();
			explanation?.AppendLine(StatDefOf.RangedWeapon_DamageMultiplier.LabelCap + ": " + damageMultiplier.ToStringPercent());
			if (weapon != null)
			{
				explanation?.Append(StatUtility.GetOffsetsAndFactorsFor(StatDefOf.RangedWeapon_DamageMultiplier, weapon));
			}
		}
		explanation?.AppendLine();
		explanation?.Append(string.Concat("StatsReport_FinalValue".Translate() + ": ", num.ToString()));
		return num;
	}

	public float GetArmorPenetration(Thing weapon = null, StringBuilder explanation = null)
	{
		if (damageDef.armorCategory == null)
		{
			return 0f;
		}
		float num;
		if (damageAmountBase != -1 || armorPenetrationBase >= 0f)
		{
			num = armorPenetrationBase;
		}
		else
		{
			if (damageDef == null)
			{
				return 0f;
			}
			num = damageDef.defaultArmorPenetration;
		}
		if (num < 0f)
		{
			num = (float)GetDamageAmount(null) * 0.015f;
		}
		explanation?.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToStringPercent());
		if (weapon != null)
		{
			float statValue = weapon.GetStatValue(StatDefOf.RangedWeapon_ArmorPenetrationMultiplier);
			num *= statValue;
			if (!Mathf.Approximately(statValue, 1f))
			{
				explanation?.AppendLine();
				explanation?.AppendLine(StatDefOf.RangedWeapon_ArmorPenetrationMultiplier.LabelCap + ": " + statValue.ToStringPercent());
				explanation?.Append(StatUtility.GetOffsetsAndFactorsFor(StatDefOf.RangedWeapon_ArmorPenetrationMultiplier, weapon));
			}
		}
		explanation?.AppendLine();
		explanation?.AppendLine("StatsReport_FinalValue".Translate() + ": " + num.ToStringPercent());
		return num;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__58))]
	public IEnumerable<string> ConfigErrors(ThingDef parent)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__58(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parent = parent
		};
	}
}
