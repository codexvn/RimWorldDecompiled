using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse;

public class Corpse : ThingWithComps, IThingHolder, IThingHolderTickable, IStrippable, IBillGiver, IObservedThoughtGiver
{
	[CompilerGenerated]
	private sealed class _003CButcherProducts_003Ed__45 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Corpse _003C_003E4__this;

		private Pawn butcher;

		public Pawn _003C_003E3__butcher;

		private float efficiency;

		public float _003C_003E3__efficiency;

		private IEnumerator<Thing> _003C_003E7__wrap1;

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
		public _003CButcherProducts_003Ed__45(int _003C_003E1__state)
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
				Corpse corpse = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = corpse.InnerPawn.ButcherProducts(butcher, efficiency).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Thing current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				if (corpse.InnerPawn.RaceProps.BloodDef != null)
				{
					FilthMaker.TryMakeFilth(butcher.Position, butcher.Map, corpse.InnerPawn.RaceProps.BloodDef, corpse.InnerPawn.LabelIndefinite());
				}
				if (corpse.InnerPawn.RaceProps.Humanlike)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ButcheredHuman, new SignalArgs(butcher.Named(HistoryEventArgsNames.Doer), corpse.InnerPawn.Named(HistoryEventArgsNames.Victim))));
					TaleRecorder.RecordTale(TaleDefOf.ButcheredHumanlikeCorpse, butcher);
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CButcherProducts_003Ed__45 _003CButcherProducts_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CButcherProducts_003Ed__ = this;
			}
			else
			{
				_003CButcherProducts_003Ed__ = new _003CButcherProducts_003Ed__45(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CButcherProducts_003Ed__.butcher = _003C_003E3__butcher;
			_003CButcherProducts_003Ed__.efficiency = _003C_003E3__efficiency;
			return _003CButcherProducts_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__57 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Corpse _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__57(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Corpse CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)CS_0024_003C_003E8__locals5).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00ec;
				case 3:
					_003C_003E1__state = -1;
					if (ModsConfig.AnomalyActive)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Resurrect as shambler",
							action = delegate
							{
								MutantUtility.ResurrectAsShambler(CS_0024_003C_003E8__locals5.InnerPawn);
							}
						};
						_003C_003E1__state = 4;
						return true;
					}
					break;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00ec:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_0106;
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals5.InnerPawn.HasShowGizmosOnCorpseHediff)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals5.InnerPawn.GetGizmos().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00ec;
					}
					goto IL_0106;
					IL_0106:
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Resurrect",
							action = delegate
							{
								ResurrectionUtility.TryResurrect(CS_0024_003C_003E8__locals5.InnerPawn);
							}
						};
						_003C_003E1__state = 3;
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

		private void _003C_003Em__Finally2()
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
			_003CGetGizmos_003Ed__57 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__57(0)
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

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__52 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Corpse _003C_003E4__this;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__52(int _003C_003E1__state)
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
				Corpse corpse = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)corpse).SpecialDisplayStats().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -1;
					if (corpse.GetRotStage() == RotStage.Fresh)
					{
						StatDef meatAmount = StatDefOf.MeatAmount;
						_003C_003E2__current = new StatDrawEntry(meatAmount.category, meatAmount, corpse.InnerPawn.GetStatValue(meatAmount), StatRequest.For(corpse.InnerPawn), ToStringNumberSense.Undefined, null);
						_003C_003E1__state = 3;
						return true;
					}
					break;
				case 3:
				{
					_003C_003E1__state = -1;
					StatDef leatherAmount = StatDefOf.LeatherAmount;
					_003C_003E2__current = new StatDrawEntry(leatherAmount.category, leatherAmount, corpse.InnerPawn.GetStatValue(leatherAmount), StatRequest.For(corpse.InnerPawn), ToStringNumberSense.Undefined, null);
					_003C_003E1__state = 4;
					return true;
				}
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "BodySize".Translate(), corpse.InnerPawn.BodySize.ToString("F2"), "Stat_Race_BodySize_Desc".Translate(), 4195);
					_003C_003E1__state = 2;
					return true;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__52 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__52(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_IngredientStackCells_003Ed__25 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Corpse _003C_003E4__this;

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
		public _003Cget_IngredientStackCells_003Ed__25(int _003C_003E1__state)
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
			Corpse corpse = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = corpse.InteractionCell;
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003Cget_IngredientStackCells_003Ed__25 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_IngredientStackCells_003Ed__25(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	protected ThingOwner<Pawn> innerContainer;

	public int timeOfDeath = -1;

	private int vanishAfterTimestamp = -1;

	private BillStack operationsBillStack;

	public bool everBuriedInSarcophagus;

	[Unsaved(false)]
	private string cachedLabel;

	private const int DontCauseObservedCorpseThoughtAfterRitualExecutionTicks = 60000;

	private static readonly IntRange ExplodeFilthCountRange = new IntRange(2, 5);

	public Pawn InnerPawn
	{
		get
		{
			if (innerContainer.Count <= 0)
			{
				return null;
			}
			return innerContainer[0];
		}
		set
		{
			if (value == null)
			{
				innerContainer.Clear();
				return;
			}
			if (innerContainer.Count > 0)
			{
				Log.Error("Setting InnerPawn in corpse that already has one.");
				innerContainer.Clear();
			}
			innerContainer.TryAdd(value);
		}
	}

	public int Age
	{
		get
		{
			return Find.TickManager.TicksGame - timeOfDeath;
		}
		set
		{
			timeOfDeath = Find.TickManager.TicksGame - value;
		}
	}

	public override string LabelNoCount
	{
		get
		{
			if (cachedLabel == null)
			{
				if (Bugged)
				{
					Log.Error("LabelNoCount on Corpse while Bugged.");
					return string.Empty;
				}
				cachedLabel = "DeadLabel".Translate(InnerPawn.Label, InnerPawn);
			}
			return cachedLabel;
		}
	}

	public override bool IngestibleNow
	{
		get
		{
			if (Bugged)
			{
				Log.Error("IngestibleNow on Corpse while Bugged.");
				return false;
			}
			if (!base.IngestibleNow)
			{
				return false;
			}
			if (!InnerPawn.RaceProps.IsFlesh)
			{
				return false;
			}
			if (this.GetRotStage() != 0)
			{
				return false;
			}
			return true;
		}
	}

	public RotDrawMode CurRotDrawMode
	{
		get
		{
			CompRottable comp = GetComp<CompRottable>();
			if (comp != null)
			{
				return comp.Stage switch
				{
					RotStage.Dessicated => RotDrawMode.Dessicated, 
					RotStage.Rotting => RotDrawMode.Rotting, 
					_ => RotDrawMode.Fresh, 
				};
			}
			return RotDrawMode.Fresh;
		}
	}

	private bool ShouldVanish
	{
		get
		{
			if (InnerPawn.RaceProps.Animal && vanishAfterTimestamp > 0 && Age >= vanishAfterTimestamp && base.Spawned && this.GetRoom() != null && this.GetRoom().TouchesMapEdge)
			{
				return !base.Map.roofGrid.Roofed(base.Position);
			}
			return false;
		}
	}

	public BillStack BillStack => operationsBillStack;

	public IEnumerable<IntVec3> IngredientStackCells
	{
		[IteratorStateMachine(typeof(_003Cget_IngredientStackCells_003Ed__25))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_IngredientStackCells_003Ed__25(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public bool ShouldTickContents => false;

	public bool Bugged
	{
		get
		{
			if (innerContainer.Count != 0 && innerContainer[0]?.def != null)
			{
				return innerContainer[0].kindDef == null;
			}
			return true;
		}
	}

	public Corpse()
	{
		operationsBillStack = new BillStack(this);
		innerContainer = new ThingOwner<Pawn>(this, oneStackOnly: true, LookMode.Reference, removeContentsIfDestroyed: false);
	}

	public bool CurrentlyUsableForBills()
	{
		return InteractionCell.IsValid;
	}

	public bool UsableForBillsAfterFueling()
	{
		return CurrentlyUsableForBills();
	}

	public bool AnythingToStrip()
	{
		return InnerPawn.AnythingToStrip();
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public override void PostMake()
	{
		base.PostMake();
		timeOfDeath = Find.TickManager.TicksGame;
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		if (Bugged)
		{
			Log.Error(this?.ToString() + " spawned in bugged state.");
			return;
		}
		base.SpawnSetup(map, respawningAfterLoad);
		InnerPawn.Rotation = Rot4.South;
		List<Hediff> hediffs = InnerPawn.health.hediffSet.hediffs;
		for (int i = 0; i < hediffs.Count; i++)
		{
			hediffs[i].Notify_PawnCorpseSpawned();
		}
		NotifyColonistBar();
	}

	public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
	{
		if (dinfo.HasValue && dinfo.Value.Def == DamageDefOf.Bomb)
		{
			CompRottable comp = GetComp<CompRottable>();
			ThingDef thingDef = ((comp == null || comp.Stage != RotStage.Rotting) ? InnerPawn.RaceProps.BloodDef : ThingDefOf.Filth_CorpseBile);
			if (thingDef != null)
			{
				int randomInRange = ExplodeFilthCountRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					FilthMaker.TryMakeFilth(base.PositionHeld, base.MapHeld, thingDef);
				}
			}
		}
		base.Kill(dinfo, exactCulprit);
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		base.DeSpawn(mode);
		if (!Bugged)
		{
			NotifyColonistBar();
		}
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		Pawn pawn = null;
		if (!Bugged)
		{
			pawn = InnerPawn;
			NotifyColonistBar();
			innerContainer.Clear();
		}
		this.GetLord()?.Notify_CorpseLost(this, null);
		base.Destroy(mode);
		if (pawn != null)
		{
			PostCorpseDestroy(pawn);
		}
	}

	public static void PostCorpseDestroy(Pawn pawn, bool discarded = false)
	{
		pawn.ownership?.UnclaimAll();
		pawn.equipment?.DestroyAllEquipment();
		pawn.inventory.DestroyAll();
		pawn.health.Notify_PawnCorpseDestroyed();
		pawn.apparel?.DestroyAll();
		if (!PawnGenerator.IsBeingGenerated(pawn) && !discarded)
		{
			pawn.Ideo?.Notify_MemberCorpseDestroyed(pawn);
		}
	}

	public override void TickRare()
	{
		TickRareInt();
		if (base.Destroyed)
		{
			return;
		}
		if (Bugged)
		{
			Log.Error(this?.ToString() + " has null innerPawn. Destroying.");
			Destroy();
			return;
		}
		Hediff_DeathRefusal firstHediff = InnerPawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
		if (firstHediff != null)
		{
			firstHediff.TickRare();
			if (base.Destroyed)
			{
				return;
			}
		}
		if (ModsConfig.AnomalyActive && InnerPawn.kindDef.IsFleshBeast() && this.GetRotStage() == RotStage.Dessicated)
		{
			FilthMaker.TryMakeFilth(base.PositionHeld, base.MapHeld, ThingDefOf.Filth_TwistedFlesh);
			Destroy();
		}
		else if (ShouldVanish)
		{
			Destroy();
		}
	}

	protected void TickRareInt()
	{
		if (base.AllComps != null)
		{
			int i = 0;
			for (int count = base.AllComps.Count; i < count; i++)
			{
				base.AllComps[i].CompTickRare();
			}
		}
		if (!base.Destroyed)
		{
			InnerPawn.TickRare();
			GasUtility.CorpseGasEffectsTickRare(this);
		}
	}

	protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
	{
		BodyPartRecord bodyPartRecord = GetBestBodyPartToEat(nutritionWanted);
		if (bodyPartRecord == null)
		{
			Log.Error(ingester?.ToString() + " ate " + this?.ToString() + " but no body part was found. Replacing with core part.");
			bodyPartRecord = InnerPawn.RaceProps.body.corePart;
		}
		float bodyPartNutrition = FoodUtility.GetBodyPartNutrition(this, bodyPartRecord);
		if (bodyPartRecord == InnerPawn.RaceProps.body.corePart)
		{
			if (ingester != null && PawnUtility.ShouldSendNotificationAbout(InnerPawn) && InnerPawn.RaceProps.Humanlike)
			{
				Messages.Message("MessageEatenByPredator".Translate(InnerPawn.LabelShort, ingester.Named("PREDATOR"), InnerPawn.Named("EATEN")).CapitalizeFirst(), ingester, MessageTypeDefOf.NegativeEvent);
			}
			numTaken = 1;
		}
		else
		{
			Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, InnerPawn, bodyPartRecord);
			if (ingester != null)
			{
				hediff_MissingPart.lastInjury = HediffDefOf.Bite;
			}
			hediff_MissingPart.IsFresh = true;
			InnerPawn.health.AddHediff(hediff_MissingPart, null, null);
			numTaken = 0;
		}
		nutritionIngested = bodyPartNutrition;
	}

	[IteratorStateMachine(typeof(_003CButcherProducts_003Ed__45))]
	public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CButcherProducts_003Ed__45(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__butcher = butcher,
			_003C_003E3__efficiency = efficiency
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref timeOfDeath, "timeOfDeath", 0);
		Scribe_Values.Look(ref vanishAfterTimestamp, "vanishAfterTimestamp", 0);
		Scribe_Values.Look(ref everBuriedInSarcophagus, "everBuriedInSarcophagus", defaultValue: false);
		Scribe_Deep.Look(ref operationsBillStack, "operationsBillStack", this);
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && innerContainer.removeContentsIfDestroyed)
		{
			innerContainer.removeContentsIfDestroyed = false;
		}
	}

	public void Strip(bool notifyFaction = true)
	{
		InnerPawn.Strip(notifyFaction);
	}

	public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
	{
		InnerPawn.DynamicDrawPhaseAt(phase, drawLoc.WithYOffset(InnerPawn.Drawer.SeededYOffset));
	}

	public Thought_Memory GiveObservedThought(Pawn observer)
	{
		return null;
	}

	public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
	{
		if (!InnerPawn.RaceProps.Humanlike)
		{
			return null;
		}
		if (InnerPawn.health.killedByRitual && Find.TickManager.TicksGame - timeOfDeath < 60000)
		{
			return null;
		}
		if (this.StoringThing() == null)
		{
			if (this.IsNotFresh())
			{
				return HistoryEventDefOf.ObservedLayingRottingCorpse;
			}
			return HistoryEventDefOf.ObservedLayingCorpse;
		}
		return null;
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (InnerPawn.Faction != null && !InnerPawn.Faction.Hidden)
		{
			stringBuilder.AppendLineTagged("Faction".Translate() + ": " + InnerPawn.Faction.NameColored);
		}
		stringBuilder.AppendLine("DeadTime".Translate(Age.ToStringTicksToPeriodVague(vagueMin: true, vagueMax: false)));
		float num = 1f - InnerPawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(InnerPawn.RaceProps.body.corePart);
		if (num >= 0.01f)
		{
			stringBuilder.AppendLine("CorpsePercentMissing".Translate() + ": " + num.ToStringPercent());
		}
		Hediff_DeathRefusal firstHediff = InnerPawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
		if (firstHediff != null && firstHediff.InProgress)
		{
			stringBuilder.AppendLine("SelfResurrecting".Translate());
		}
		stringBuilder.AppendLine(base.GetInspectString());
		return stringBuilder.ToString().TrimEndNewlines();
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__52))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__52(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void RotStageChanged()
	{
		InnerPawn.Drawer.renderer.SetAllGraphicsDirty();
		InnerPawn.Drawer.renderer.WoundOverlays.ClearCache();
		NotifyColonistBar();
	}

	public BodyPartRecord GetBestBodyPartToEat(float nutritionWanted)
	{
		IEnumerable<BodyPartRecord> source = from x in InnerPawn.health.hediffSet.GetNotMissingParts()
			where x.depth == BodyPartDepth.Outside && FoodUtility.GetBodyPartNutrition(this, x) > 0.001f
			select x;
		if (!source.Any())
		{
			return null;
		}
		return source.MinBy((BodyPartRecord x) => Mathf.Abs(FoodUtility.GetBodyPartNutrition(this, x) - nutritionWanted));
	}

	private void NotifyColonistBar()
	{
		if (InnerPawn.Faction == Faction.OfPlayer && Current.ProgramState == ProgramState.Playing)
		{
			Find.ColonistBar.MarkColonistsDirty();
		}
	}

	public void Notify_BillDeleted(Bill bill)
	{
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__57))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__57(-2)
		{
			_003C_003E4__this = this
		};
	}
}
