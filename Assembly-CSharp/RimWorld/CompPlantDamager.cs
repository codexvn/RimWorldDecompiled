using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompPlantDamager : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__10 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompPlantDamager _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__10(int _003C_003E1__state)
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
			CompPlantDamager @object = _003C_003E4__this;
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
						defaultLabel = "DEV: Plant kill cycle",
						action = @object.DamageCycle
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
			_003CCompGetGizmosExtra_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__10(0)
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

	private int ticksToPlantHarm;

	private int queuedCycles;

	private static readonly SimpleCurve DamageMultiplierOverDistance = new SimpleCurve
	{
		new CurvePoint(0f, 1f),
		new CurvePoint(1f, 0.2f)
	};

	public CompProperties_PlantDamager Props => (CompProperties_PlantDamager)props;

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		ticksToPlantHarm = Props.ticksBetweenDamage;
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref queuedCycles, "queuedCycles", 0);
		Scribe_Values.Look(ref ticksToPlantHarm, "ticksToPlantHarm", 0);
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!respawningAfterLoad)
		{
			queuedCycles = Props.cycleCountOnSpawn;
		}
	}

	public override void CompTick()
	{
		if (parent.Spawned)
		{
			while (queuedCycles > 0)
			{
				DamageCycle();
				queuedCycles--;
			}
			ticksToPlantHarm--;
			if (ticksToPlantHarm <= 0)
			{
				ticksToPlantHarm = Props.ticksBetweenDamage;
				DamageCycle();
			}
		}
	}

	public void DamageCycle()
	{
		foreach (IntVec3 item in GenRadial.RadialCellsAround(parent.Position, Props.radius, useCenter: true))
		{
			if (item.InBounds(parent.Map))
			{
				float distance = item.DistanceTo(parent.Position);
				float amount = ComputeDamage(distance);
				Plant plant = item.GetPlant(parent.Map);
				if (plant != null && (!Props.ignoreAnima || (plant.def != ThingDefOf.Plant_GrassAnima && plant.def != ThingDefOf.Plant_TreeAnima)))
				{
					plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, amount));
				}
			}
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__10))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	private float ComputeDamage(float distance)
	{
		return Props.damagePerCycle * DamageMultiplierOverDistance.Evaluate(distance / Props.radius);
	}
}
