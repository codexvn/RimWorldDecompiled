using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompDeepDrill : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__18 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompDeepDrill _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CCompGetGizmosExtra_003Ed__18(int _003C_003E1__state)
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
				CompDeepDrill CS_0024_003C_003E8__locals2 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals2).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Produce portion (100% yield)",
							action = delegate
							{
								CS_0024_003C_003E8__locals2.TryProducePortion(1f);
							}
						};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__18(0)
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

	private float portionProgress;

	private float portionYieldPct;

	private int lastUsedTick = -99999;

	private const float WorkPerPortionBase = 10000f;

	[Obsolete("Use WorkPerPortionBase constant directly.")]
	public static float WorkPerPortionCurrentDifficulty => 10000f;

	public float ProgressToNextPortionPercent => portionProgress / 10000f;

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		powerComp = parent.TryGetComp<CompPowerTrader>();
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref portionProgress, "portionProgress", 0f);
		Scribe_Values.Look(ref portionYieldPct, "portionYieldPct", 0f);
		Scribe_Values.Look(ref lastUsedTick, "lastUsedTick", 0);
	}

	public void DrillWorkDone(Pawn driller, int delta)
	{
		float num = driller.GetStatValue(StatDefOf.DeepDrillingSpeed) * (float)delta;
		portionProgress += num;
		portionYieldPct += num * driller.GetStatValue(StatDefOf.MiningYield) / 10000f;
		lastUsedTick = Find.TickManager.TicksGame;
		if (portionProgress > 10000f)
		{
			TryProducePortion(portionYieldPct, driller);
			portionProgress = 0f;
			portionYieldPct = 0f;
		}
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		portionProgress = 0f;
		portionYieldPct = 0f;
		lastUsedTick = -99999;
	}

	private void TryProducePortion(float yieldPct, Pawn driller = null)
	{
		ThingDef resDef;
		int countPresent;
		IntVec3 cell;
		bool nextResource = GetNextResource(out resDef, out countPresent, out cell);
		if (resDef == null)
		{
			return;
		}
		int num = Mathf.Min(countPresent, resDef.deepCountPerPortion);
		if (nextResource)
		{
			parent.Map.deepResourceGrid.SetAt(cell, resDef, countPresent - num);
		}
		int stackCount = Mathf.Max(1, GenMath.RoundRandom((float)num * yieldPct));
		Thing thing = ThingMaker.MakeThing(resDef);
		thing.stackCount = stackCount;
		GenPlace.TryPlaceThing(thing, parent.InteractionCell, parent.Map, ThingPlaceMode.Near, null, (IntVec3 p) => p != parent.Position && p != parent.InteractionCell, null);
		if (driller != null)
		{
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.Mined, driller.Named(HistoryEventArgsNames.Doer)));
		}
		if (!nextResource || ValuableResourcesPresent())
		{
			return;
		}
		if (DeepDrillUtility.GetBaseResource(parent.Map, parent.Position) == null)
		{
			Messages.Message("DeepDrillExhaustedNoFallback".Translate(), parent, MessageTypeDefOf.TaskCompletion);
			return;
		}
		Messages.Message("DeepDrillExhausted".Translate(Find.ActiveLanguageWorker.Pluralize(DeepDrillUtility.GetBaseResource(parent.Map, parent.Position).label)), parent, MessageTypeDefOf.TaskCompletion);
		for (int i = 0; i < 21; i++)
		{
			IntVec3 c = cell + GenRadial.RadialPattern[i];
			if (c.InBounds(parent.Map))
			{
				ThingWithComps firstThingWithComp = c.GetFirstThingWithComp<CompDeepDrill>(parent.Map);
				if (firstThingWithComp != null && !firstThingWithComp.GetComp<CompDeepDrill>().ValuableResourcesPresent())
				{
					firstThingWithComp.SetForbidden(value: true);
				}
			}
		}
	}

	private bool GetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
	{
		return DeepDrillUtility.GetNextResource(parent.Position, parent.Map, out resDef, out countPresent, out cell);
	}

	public bool CanDrillNow()
	{
		if (powerComp != null && !powerComp.PowerOn)
		{
			return false;
		}
		if (DeepDrillUtility.GetBaseResource(parent.Map, parent.Position) != null)
		{
			return true;
		}
		return ValuableResourcesPresent();
	}

	public bool ValuableResourcesPresent()
	{
		ThingDef resDef;
		int countPresent;
		IntVec3 cell;
		return GetNextResource(out resDef, out countPresent, out cell);
	}

	public bool UsedLastTick()
	{
		return lastUsedTick >= Find.TickManager.TicksGame - 1;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__18))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__18(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		if (parent.Spawned)
		{
			GetNextResource(out var resDef, out var _, out var _);
			if (resDef == null)
			{
				return "DeepDrillNoResources".Translate();
			}
			return "ResourceBelow".Translate() + ": " + resDef.LabelCap + "\n" + "ProgressToNextPortion".Translate() + ": " + ProgressToNextPortionPercent.ToStringPercent("F0");
		}
		return null;
	}
}
