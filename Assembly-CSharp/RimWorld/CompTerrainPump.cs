using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public abstract class CompTerrainPump : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__19 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompTerrainPump _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__19(int _003C_003E1__state)
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
			CompTerrainPump CS_0024_003C_003E8__locals1 = _003C_003E4__this;
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
						defaultLabel = "DEV: Progress 1 day",
						action = delegate
						{
							CS_0024_003C_003E8__locals1.progressTicks += 60000;
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
			_003CCompGetGizmosExtra_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__19(0)
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

	private CompPowerTrader powerComp;

	private int progressTicks;

	private CompProperties_TerrainPump Props => (CompProperties_TerrainPump)props;

	private float ProgressDays => (float)progressTicks / 60000f;

	protected float CurrentRadius => Mathf.Min(Props.radius, ProgressDays / Props.daysToRadius * Props.radius);

	protected bool Working
	{
		get
		{
			if (powerComp != null)
			{
				return powerComp.PowerOn;
			}
			return true;
		}
	}

	private int TicksUntilRadiusInteger
	{
		get
		{
			float num = Mathf.Ceil(CurrentRadius) - CurrentRadius;
			if (num < 1E-05f)
			{
				num = 1f;
			}
			float num2 = Props.radius / Props.daysToRadius;
			return (int)(num / num2 * 60000f);
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		powerComp = parent.TryGetComp<CompPowerTrader>();
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		progressTicks = 0;
	}

	public override void CompTickRare()
	{
		if (Working)
		{
			progressTicks += 250;
			int num = GenRadial.NumCellsInRadius(CurrentRadius);
			for (int i = 0; i < num; i++)
			{
				AffectCell(parent.Position + GenRadial.RadialPattern[i]);
			}
		}
	}

	protected abstract void AffectCell(IntVec3 c);

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref progressTicks, "progressTicks", 0);
	}

	public override void PostDrawExtraSelectionOverlays()
	{
		if (CurrentRadius < Props.radius - 0.0001f)
		{
			GenDraw.DrawRadiusRing(parent.Position, CurrentRadius);
		}
	}

	public override string CompInspectStringExtra()
	{
		string text = "TimePassed".Translate().CapitalizeFirst() + ": " + progressTicks.ToStringTicksToPeriod() + "\n" + "CurrentRadius".Translate().CapitalizeFirst() + ": " + CurrentRadius.ToString("F1");
		if (ProgressDays < Props.daysToRadius && Working)
		{
			text += "\n" + "RadiusExpandsIn".Translate().CapitalizeFirst() + ": " + TicksUntilRadiusInteger.ToStringTicksToPeriod();
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__19))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__19(-2)
		{
			_003C_003E4__this = this
		};
	}
}
