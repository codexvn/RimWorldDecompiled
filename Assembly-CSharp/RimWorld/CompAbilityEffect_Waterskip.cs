using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompAbilityEffect_Waterskip : CompAbilityEffect
{
	[CompilerGenerated]
	private sealed class _003CAffectedCells_003Ed__1 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LocalTargetInfo target;

		public LocalTargetInfo _003C_003E3__target;

		private Map map;

		public Map _003C_003E3__map;

		public CompAbilityEffect_Waterskip _003C_003E4__this;

		private IEnumerator<IntVec3> _003C_003E7__wrap1;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CAffectedCells_003Ed__1(int _003C_003E1__state)
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
				CompAbilityEffect_Waterskip compAbilityEffect_Waterskip = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!target.Cell.InBounds(map) || target.Cell.Filled(compAbilityEffect_Waterskip.parent.pawn.Map))
					{
						return false;
					}
					_003C_003E7__wrap1 = GenRadial.RadialCellsAround(target.Cell, compAbilityEffect_Waterskip.parent.def.EffectRadius, useCenter: true).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap1.Current;
					if (current.InBounds(map) && GenSight.LineOfSightToEdges(target.Cell, current, map, skipFirstCell: true))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CAffectedCells_003Ed__1 _003CAffectedCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAffectedCells_003Ed__ = this;
			}
			else
			{
				_003CAffectedCells_003Ed__ = new _003CAffectedCells_003Ed__1(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAffectedCells_003Ed__.target = _003C_003E3__target;
			_003CAffectedCells_003Ed__.map = _003C_003E3__map;
			return _003CAffectedCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
	{
		base.Apply(target, dest);
		Map map = parent.pawn.Map;
		foreach (IntVec3 item in AffectedCells(target, map))
		{
			if (!item.InBounds(map))
			{
				continue;
			}
			List<Thing> thingList = item.GetThingList(map);
			for (int num = thingList.Count - 1; num >= 0; num--)
			{
				if (thingList[num] is Fire)
				{
					thingList[num].Destroy();
				}
				else if (thingList[num] is Pawn pawn)
				{
					pawn.GetInvisibilityComp()?.DisruptInvisibility();
				}
			}
			if (!item.Filled(map))
			{
				FilthMaker.TryMakeFilth(item, map, ThingDefOf.Filth_Water);
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(item.ToVector3Shifted(), map, FleckDefOf.WaterskipSplashParticles);
			dataStatic.rotationRate = Rand.Range(-30, 30);
			dataStatic.rotation = 90 * Rand.RangeInclusive(0, 3);
			map.flecks.CreateFleck(dataStatic);
			CompAbilityEffect_Teleport.SendSkipUsedSignal(item, parent.pawn);
		}
	}

	[IteratorStateMachine(typeof(_003CAffectedCells_003Ed__1))]
	private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAffectedCells_003Ed__1(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target,
			_003C_003E3__map = map
		};
	}

	public override void DrawEffectPreview(LocalTargetInfo target)
	{
		GenDraw.DrawFieldEdges(AffectedCells(target, parent.pawn.Map).ToList(), Valid(target) ? Color.white : Color.red, null);
	}

	public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
	{
		if (target.Cell.Filled(parent.pawn.Map))
		{
			if (throwMessages)
			{
				Messages.Message("AbilityOccupiedCells".Translate(parent.def.LabelCap), target.ToTargetInfo(parent.pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		return true;
	}
}
