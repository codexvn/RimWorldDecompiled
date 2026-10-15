using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompDamageOnInterval : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__8 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompDamageOnInterval _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__8(int _003C_003E1__state)
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
			CompDamageOnInterval @object = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Damage",
						action = @object.Damage
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
			_003CCompGetGizmosExtra_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__8(0)
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

	private int ticksToDamage;

	private Effecter effecter;

	public CompProperties_DamageOnInterval Props => (CompProperties_DamageOnInterval)props;

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		ticksToDamage = Props.ticksBetweenDamage;
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref ticksToDamage, "ticksToPlantHarm", 0);
	}

	public override void CompTick()
	{
		if (!parent.Spawned || (Props.startHitPointsPercent > 0f && (float)parent.HitPoints > Props.startHitPointsPercent * (float)parent.MaxHitPoints))
		{
			return;
		}
		ticksToDamage--;
		if (ticksToDamage <= 0)
		{
			ticksToDamage = Props.ticksBetweenDamage;
			Damage();
		}
		EffecterDef effecterDef = null;
		foreach (DamageEffectStage effectStage in Props.effectStages)
		{
			if (effectStage.minHitPointsPercent * (float)parent.MaxHitPoints > (float)parent.HitPoints)
			{
				effecterDef = effectStage.effecterDef;
			}
		}
		if (effecterDef != null && effecterDef != effecter?.def)
		{
			effecter?.Cleanup();
			effecter = effecterDef.SpawnAttached(parent, parent.MapHeld);
		}
		effecter?.EffectTick(parent, parent);
	}

	public void Damage()
	{
		parent.TakeDamage(new DamageInfo(Props.damageDef ?? DamageDefOf.Deterioration, Props.damage));
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__8))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
