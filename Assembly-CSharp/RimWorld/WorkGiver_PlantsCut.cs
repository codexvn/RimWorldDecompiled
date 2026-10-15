using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class WorkGiver_PlantsCut : WorkGiver_Scanner
{
	[CompilerGenerated]
	private sealed class _003CPotentialWorkThingsGlobal_003Ed__1 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private List<Designation>.Enumerator _003C_003E7__wrap1;

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
		public _003CPotentialWorkThingsGlobal_003Ed__1(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Designation>.Enumerator);
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
					_003C_003E7__wrap1 = pawn.Map.designationManager.designationsByDef[DesignationDefOf.CutPlant].GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_008e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_008e;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_008e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Designation current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current.target.Thing;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Designation>.Enumerator);
					_003C_003E7__wrap1 = pawn.Map.designationManager.designationsByDef[DesignationDefOf.HarvestPlant].GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Designation current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2.target.Thing;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = default(List<Designation>.Enumerator);
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
			_003CPotentialWorkThingsGlobal_003Ed__1 _003CPotentialWorkThingsGlobal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialWorkThingsGlobal_003Ed__ = this;
			}
			else
			{
				_003CPotentialWorkThingsGlobal_003Ed__ = new _003CPotentialWorkThingsGlobal_003Ed__1(0);
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

	public override Danger MaxPathDanger(Pawn pawn)
	{
		return Danger.Deadly;
	}

	[IteratorStateMachine(typeof(_003CPotentialWorkThingsGlobal_003Ed__1))]
	public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialWorkThingsGlobal_003Ed__1(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override bool ShouldSkip(Pawn pawn, bool forced = false)
	{
		if (!pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.CutPlant))
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.HarvestPlant);
		}
		return false;
	}

	public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		if (t.def.category != ThingCategory.Plant)
		{
			return null;
		}
		if (!pawn.CanReserve(t, 1, -1, null, forced))
		{
			return null;
		}
		if (t.IsBurning())
		{
			return null;
		}
		if (!PlantUtility.PawnWillingToCutPlant_Job(t, pawn))
		{
			return null;
		}
		if (!forced && t.TryGetComp(out CompPlantPreventCutting comp) && comp.PreventCutting)
		{
			return null;
		}
		foreach (Designation item in pawn.Map.designationManager.AllDesignationsOn(t))
		{
			if (item.def == DesignationDefOf.HarvestPlant)
			{
				if (!((Plant)t).HarvestableNow)
				{
					return null;
				}
				return JobMaker.MakeJob(JobDefOf.HarvestDesignated, t);
			}
			if (item.def == DesignationDefOf.CutPlant)
			{
				return JobMaker.MakeJob(JobDefOf.CutPlantDesignated, t);
			}
		}
		return null;
	}

	public override string PostProcessedGerund(Job job)
	{
		if (job.def == JobDefOf.HarvestDesignated)
		{
			return "HarvestGerund".Translate();
		}
		if (job.def == JobDefOf.CutPlantDesignated)
		{
			return "CutGerund".Translate();
		}
		return def.gerund;
	}
}
