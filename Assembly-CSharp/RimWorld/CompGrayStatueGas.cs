using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompGrayStatueGas : CompGrayStatue
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__7 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompGrayStatueGas _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__7(int _003C_003E1__state)
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
			CompGrayStatueGas CS_0024_003C_003E8__locals4 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Test gas spread",
					action = delegate
					{
						CS_0024_003C_003E8__locals4.parent.Map.gasGrid.EstimateGasDiffusion(CS_0024_003C_003E8__locals4.parent.Position, GasType.DeadlifeDust, CS_0024_003C_003E8__locals4.GasAmount, delegate(IntVec3 c)
						{
							GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.PowerConduit), c, CS_0024_003C_003E8__locals4.parent.Map);
						});
					}
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
			_003CCompGetGizmosExtra_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__7(0)
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

	private const float RoomCellGasFactor = 0.15f;

	private const int MaxGas = 12750;

	private CompProperties_GrayStatueGas Props => (CompProperties_GrayStatueGas)props;

	private int GasAmount => Mathf.Min(Mathf.CeilToInt((float)parent.GetRoom().CellCount * 0.15f * 255f), 12750);

	protected override void Trigger(Pawn target)
	{
		if (Props.gas == GasType.DeadlifeDust)
		{
			GasUtility.AddDeadifeGas(parent.Position, parent.Map, Faction.OfEntities, GasAmount);
		}
		else
		{
			GasUtility.AddGas(parent.Position, parent.Map, Props.gas, GasAmount);
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__7))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
