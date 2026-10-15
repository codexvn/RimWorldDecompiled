using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompProperties_Explosive : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__41 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_Explosive _003C_003E4__this;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__41(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompProperties_Explosive compProperties_Explosive = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_Explosive).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0075;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0075;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0075:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (parentDef.tickerType != TickerType.Normal)
					{
						_003C_003E2__current = "CompExplosive requires Normal ticker type";
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__41 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__41(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public float explosiveRadius = 1.9f;

	public DamageDef explosiveDamageType;

	public int damageAmountBase = -1;

	public float armorPenetrationBase = -1f;

	public ThingDef postExplosionSpawnThingDef;

	public float postExplosionSpawnChance;

	public int postExplosionSpawnThingCount = 1;

	public bool applyDamageToExplosionCellsNeighbors;

	public ThingDef preExplosionSpawnThingDef;

	public float preExplosionSpawnChance;

	public int preExplosionSpawnThingCount = 1;

	public float chanceToStartFire;

	public bool damageFalloff;

	public bool explodeOnKilled;

	public bool explodeOnDestroyed;

	public GasType? postExplosionGasType;

	public float? postExplosionGasRadiusOverride;

	public int postExplosionGasAmount = 255;

	public bool doVisualEffects = true;

	public bool doSoundEffects = true;

	public float propagationSpeed = 1f;

	public ThingDef postExplosionSpawnSingleThingDef;

	public ThingDef preExplosionSpawnSingleThingDef;

	public float explosiveExpandPerStackcount;

	public float explosiveExpandPerFuel;

	public EffecterDef explosionEffect;

	public SoundDef explosionSound;

	public List<DamageDef> startWickOnDamageTaken;

	public float startWickHitPointsPercent = 0.2f;

	public IntRange wickTicks = new IntRange(140, 150);

	public float wickScale = 1f;

	public List<DamageDef> startWickOnInternalDamageTaken;

	public bool drawWick = true;

	public float chanceNeverExplodeFromDamage;

	public float destroyThingOnExplosionSize;

	public DamageDef requiredDamageTypeToExplode;

	public IntRange? countdownTicks;

	public string extraInspectStringKey;

	public List<WickMessage> wickMessages;

	public CompProperties_Explosive()
	{
		compClass = typeof(CompExplosive);
	}

	public override void ResolveReferences(ThingDef parentDef)
	{
		base.ResolveReferences(parentDef);
		if (explosiveDamageType == null)
		{
			explosiveDamageType = DamageDefOf.Bomb;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__41))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__41(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
