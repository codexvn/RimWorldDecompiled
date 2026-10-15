using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompCascadeOnDestroyed : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CAdjacentCascadableThings_003Ed__3 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing mass;

		public Thing _003C_003E3__mass;

		private Map map;

		public Map _003C_003E3__map;

		public CompCascadeOnDestroyed _003C_003E4__this;

		private int _003Ci_003E5__2;

		Thing IEnumerator<Thing>.Current
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
		public _003CAdjacentCascadableThings_003Ed__3(int _003C_003E1__state)
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
			CompCascadeOnDestroyed compCascadeOnDestroyed = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_00f3;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00e1;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_00e1;
				}
				IL_00f3:
				if (_003Ci_003E5__2 < 4)
				{
					IntVec3 c = mass.Position + GenAdj.CardinalDirections[_003Ci_003E5__2];
					if (c.InBounds(map))
					{
						Building edifice = c.GetEdifice(map);
						if (edifice != null)
						{
							if (compCascadeOnDestroyed.Props.cascadeThingDefs.NullOrEmpty() && edifice.def == compCascadeOnDestroyed.parent.def)
							{
								_003C_003E2__current = edifice;
								_003C_003E1__state = 1;
								return true;
							}
							if (compCascadeOnDestroyed.Props.cascadeThingDefs.NotNullAndContains(edifice.def))
							{
								_003C_003E2__current = edifice;
								_003C_003E1__state = 2;
								return true;
							}
						}
					}
					goto IL_00e1;
				}
				return false;
				IL_00e1:
				_003Ci_003E5__2++;
				goto IL_00f3;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CAdjacentCascadableThings_003Ed__3 _003CAdjacentCascadableThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAdjacentCascadableThings_003Ed__ = this;
			}
			else
			{
				_003CAdjacentCascadableThings_003Ed__ = new _003CAdjacentCascadableThings_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAdjacentCascadableThings_003Ed__.mass = _003C_003E3__mass;
			_003CAdjacentCascadableThings_003Ed__.map = _003C_003E3__map;
			return _003CAdjacentCascadableThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	private CompProperties_CascadeOnDestroyed Props => (CompProperties_CascadeOnDestroyed)props;

	public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
	{
		if (dinfo.HasValue && dinfo.Value.PreventCascade)
		{
			return;
		}
		int randomInRange = Props.cascadeCountRange.RandomInRange;
		int i = 0;
		Queue<Thing> queue = new Queue<Thing>();
		foreach (Thing item in AdjacentCascadableThings(parent, prevMap))
		{
			queue.Enqueue(item);
		}
		for (; i < randomInRange; i++)
		{
			if (queue.Empty())
			{
				break;
			}
			Thing thing = queue.Dequeue();
			foreach (Thing item2 in AdjacentCascadableThings(thing, prevMap))
			{
				if (!queue.Contains(item2))
				{
					queue.Enqueue(item2);
				}
			}
			thing.Kill(new DamageInfo(null, 99999f, 0f, -1f, dinfo?.Instigator, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, instigatorGuilty: true, spawnFilth: true, QualityCategory.Normal, checkForJobOverride: true, preventCascade: true));
		}
	}

	[IteratorStateMachine(typeof(_003CAdjacentCascadableThings_003Ed__3))]
	private IEnumerable<Thing> AdjacentCascadableThings(Thing mass, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAdjacentCascadableThings_003Ed__3(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__mass = mass,
			_003C_003E3__map = map
		};
	}
}
