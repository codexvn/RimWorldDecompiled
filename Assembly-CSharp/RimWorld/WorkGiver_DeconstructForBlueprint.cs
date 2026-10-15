using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class WorkGiver_DeconstructForBlueprint : WorkGiver_Scanner
{
	[CompilerGenerated]
	private sealed class _003CBuildableBlueprintsAt_003Ed__4 : IEnumerable<Blueprint>, IEnumerable, IEnumerator<Blueprint>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Blueprint _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private IntVec3 cell;

		public IntVec3 _003C_003E3__cell;

		private List<Blueprint>.Enumerator _003C_003E7__wrap1;

		Blueprint IEnumerator<Blueprint>.Current
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
		public _003CBuildableBlueprintsAt_003Ed__4(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Blueprint>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					List<Blueprint> list = pawn.Map.blueprintGrid[cell];
					if (list.NullOrEmpty())
					{
						return false;
					}
					_003C_003E7__wrap1 = list.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Blueprint current = _003C_003E7__wrap1.Current;
					if (current.Faction == pawn.Faction)
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Blueprint>.Enumerator);
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
		IEnumerator<Blueprint> IEnumerable<Blueprint>.GetEnumerator()
		{
			_003CBuildableBlueprintsAt_003Ed__4 _003CBuildableBlueprintsAt_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CBuildableBlueprintsAt_003Ed__ = this;
			}
			else
			{
				_003CBuildableBlueprintsAt_003Ed__ = new _003CBuildableBlueprintsAt_003Ed__4(0);
			}
			_003CBuildableBlueprintsAt_003Ed__.pawn = _003C_003E3__pawn;
			_003CBuildableBlueprintsAt_003Ed__.cell = _003C_003E3__cell;
			return _003CBuildableBlueprintsAt_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Blueprint>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPotentialWorkThingsGlobal_003Ed__2 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private List<Building>.Enumerator _003C_003E7__wrap1;

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
		public _003CPotentialWorkThingsGlobal_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<Building>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = pawn.Map.listerBuildings.allBuildingsColonist.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00bf;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00bf;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_00bf:
					while (_003C_003E7__wrap1.MoveNext())
					{
						Building current = _003C_003E7__wrap1.Current;
						if (current.def != null && !pawn.Map.blueprintGrid[current.Position].NullOrEmpty() && current.DeconstructibleBy(pawn.Faction).Accepted)
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Building>.Enumerator);
					_003C_003E7__wrap1 = pawn.Map.listerBuildings.allBuildingsNonColonist.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Building current2 = _003C_003E7__wrap1.Current;
					if (current2.def != null && !pawn.Map.blueprintGrid[current2.Position].NullOrEmpty() && current2.DeconstructibleBy(pawn.Faction).Accepted)
					{
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = default(List<Building>.Enumerator);
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

		private void _003C_003Em__Finally2()
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CPotentialWorkThingsGlobal_003Ed__2 _003CPotentialWorkThingsGlobal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialWorkThingsGlobal_003Ed__ = this;
			}
			else
			{
				_003CPotentialWorkThingsGlobal_003Ed__ = new _003CPotentialWorkThingsGlobal_003Ed__2(0);
			}
			_003CPotentialWorkThingsGlobal_003Ed__.pawn = _003C_003E3__pawn;
			return _003CPotentialWorkThingsGlobal_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	public override PathEndMode PathEndMode => PathEndMode.Touch;

	[IteratorStateMachine(typeof(_003CPotentialWorkThingsGlobal_003Ed__2))]
	public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialWorkThingsGlobal_003Ed__2(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		if (!pawn.CanReserve(t, 1, -1, null, forced))
		{
			return false;
		}
		IEnumerable<Blueprint> enumerable = BuildableBlueprintsAt(pawn, t.Position);
		if (enumerable.EnumerableNullOrEmpty())
		{
			return false;
		}
		if (t.TryGetComp(out CompExplosive comp) && comp.wickStarted)
		{
			JobFailReason.Is("AboutToExplode".Translate());
			return false;
		}
		if (t is Building building)
		{
			AcceptanceReport acceptanceReport = building.DeconstructibleBy(pawn.Faction);
			if (!acceptanceReport.Accepted)
			{
				JobFailReason.Is(acceptanceReport.Reason);
				return false;
			}
			if (building.IsForbidden(pawn))
			{
				JobFailReason.Is("ForbiddenLower".Translate());
				return false;
			}
		}
		foreach (Blueprint item in enumerable)
		{
			if (item is Blueprint_Install blueprint_Install && blueprint_Install.ThingToInstall == t)
			{
				return false;
			}
			if (item.IsForbidden(pawn))
			{
				JobFailReason.Is("ForbiddenLower".Translate());
				return false;
			}
			if (GenConstruct.CanReplace(item.EntityToBuild(), t.def, item.Stuff, t.Stuff))
			{
				return true;
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CBuildableBlueprintsAt_003Ed__4))]
	private static IEnumerable<Blueprint> BuildableBlueprintsAt(Pawn pawn, IntVec3 cell)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBuildableBlueprintsAt_003Ed__4(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__cell = cell
		};
	}

	public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		return JobMaker.MakeJob(JobDefOf.DeconstructForBlueprint, t);
	}
}
