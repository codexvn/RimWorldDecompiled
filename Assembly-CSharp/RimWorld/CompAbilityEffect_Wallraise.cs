using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompAbilityEffect_Wallraise : CompAbilityEffect
{
	[CompilerGenerated]
	private sealed class _003CAffectedCells_003Ed__6 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAbilityEffect_Wallraise _003C_003E4__this;

		private LocalTargetInfo target;

		public LocalTargetInfo _003C_003E3__target;

		private Map map;

		public Map _003C_003E3__map;

		private List<IntVec2>.Enumerator _003C_003E7__wrap1;

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
		public _003CAffectedCells_003Ed__6(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<IntVec2>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompAbilityEffect_Wallraise compAbilityEffect_Wallraise = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = compAbilityEffect_Wallraise.Props.pattern.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					IntVec2 current = _003C_003E7__wrap1.Current;
					IntVec3 c = target.Cell + new IntVec3(current.x, 0, current.z);
					if (c.InBounds(map))
					{
						_003C_003E2__current = c;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<IntVec2>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CAffectedCells_003Ed__6 _003CAffectedCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAffectedCells_003Ed__ = this;
			}
			else
			{
				_003CAffectedCells_003Ed__ = new _003CAffectedCells_003Ed__6(0)
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

	public static Color DustColor = new Color(0.55f, 0.55f, 0.55f, 4f);

	public new CompProperties_AbilityWallraise Props => (CompProperties_AbilityWallraise)props;

	public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
	{
		base.Apply(target, dest);
		Map map = parent.pawn.Map;
		List<Thing> list = new List<Thing>();
		list.AddRange(AffectedCells(target, map).SelectMany((IntVec3 c) => from t in c.GetThingList(map)
			where t.def.category == ThingCategory.Item
			select t));
		foreach (Thing item in list)
		{
			item.DeSpawn();
		}
		foreach (IntVec3 item2 in AffectedCells(target, map))
		{
			GenSpawn.Spawn(ThingDefOf.RaisedRocks, item2, map);
			FleckMaker.ThrowDustPuffThick(item2.ToVector3Shifted(), map, Rand.Range(1.5f, 3f), DustColor);
			CompAbilityEffect_Teleport.SendSkipUsedSignal(item2, parent.pawn);
		}
		foreach (Thing item3 in list)
		{
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec2 = item3.Position + GenRadial.RadialPattern[i];
				if (intVec2.InBounds(map) && intVec2.Walkable(map) && map.thingGrid.ThingsListAtFast(intVec2).Count <= 0)
				{
					intVec = intVec2;
					break;
				}
			}
			if (intVec != IntVec3.Invalid)
			{
				GenSpawn.Spawn(item3, intVec, map);
			}
			else
			{
				GenPlace.TryPlaceThing(item3, item3.Position, map, ThingPlaceMode.Near, null, null, null);
			}
		}
	}

	public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
	{
		return Valid(target, throwMessages: true);
	}

	public override void DrawEffectPreview(LocalTargetInfo target)
	{
		GenDraw.DrawFieldEdges(AffectedCells(target, parent.pawn.Map).ToList(), Valid(target) ? Color.white : Color.red, null);
	}

	[IteratorStateMachine(typeof(_003CAffectedCells_003Ed__6))]
	private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAffectedCells_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target,
			_003C_003E3__map = map
		};
	}

	public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
	{
		if (AffectedCells(target, parent.pawn.Map).Any((IntVec3 c) => c.Filled(parent.pawn.Map)))
		{
			if (throwMessages)
			{
				Messages.Message("CannotUseAbility".Translate(parent.def.label) + ": " + "AbilityOccupiedCells".Translate(), target.ToTargetInfo(parent.pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		if (AffectedCells(target, parent.pawn.Map).Any((IntVec3 c) => !c.Standable(parent.pawn.Map)))
		{
			if (throwMessages)
			{
				Messages.Message("CannotUseAbility".Translate(parent.def.label) + ": " + "AbilityUnwalkable".Translate(), target.ToTargetInfo(parent.pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		if (AffectedCells(target, parent.pawn.Map).Any((IntVec3 c) => c.GetThingList(parent.pawn.Map).Any((Thing t) => !t.def.destroyable)))
		{
			if (throwMessages)
			{
				Messages.Message("CannotUseAbility".Translate(parent.def.label) + ": " + "AbilityNotEnoughFreeSpace".Translate(), target.ToTargetInfo(parent.pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		return true;
	}
}
