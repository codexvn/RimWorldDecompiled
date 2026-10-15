using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class Building_Crate : Building_Casket, IThingGlower
{
	[CompilerGenerated]
	private sealed class _003CGetMultiSelectFloatMenuOptions_003Ed__13 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Crate _003C_003E4__this;

		private IEnumerable<Pawn> selPawns;

		public IEnumerable<Pawn> _003C_003E3__selPawns;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetMultiSelectFloatMenuOptions_003Ed__13(int _003C_003E1__state)
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
				Building_Crate CS_0024_003C_003E8__locals12 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)CS_0024_003C_003E8__locals12).GetMultiSelectFloatMenuOptions(selPawns).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0080;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0080;
				case 2:
					_003C_003E1__state = -1;
					return false;
				case 3:
					_003C_003E1__state = -1;
					return false;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0080:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!CS_0024_003C_003E8__locals12.CanOpen)
					{
						return false;
					}
					tmpAllowedPawns.Clear();
					foreach (Pawn item in selPawns)
					{
						if (item.RaceProps.Humanlike && item.CanReach(CS_0024_003C_003E8__locals12, PathEndMode.InteractionCell, Danger.Deadly))
						{
							tmpAllowedPawns.Add(item);
						}
					}
					if (tmpAllowedPawns.Count <= 0)
					{
						_003C_003E2__current = new FloatMenuOption("CannotOpen".Translate(CS_0024_003C_003E8__locals12) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 2;
						return true;
					}
					tmpAllowedPawns.Clear();
					foreach (Pawn item2 in selPawns)
					{
						if (item2.RaceProps.Humanlike && CS_0024_003C_003E8__locals12.IsCapableOfOpening(item2))
						{
							tmpAllowedPawns.Add(item2);
						}
					}
					if (tmpAllowedPawns.Count <= 0)
					{
						_003C_003E2__current = new FloatMenuOption("CannotOpen".Translate(CS_0024_003C_003E8__locals12.Label) + ": " + "Incapable".Translate(), null);
						_003C_003E1__state = 3;
						return true;
					}
					tmpAllowedPawns.Clear();
					foreach (Pawn item3 in selPawns)
					{
						if (item3.RaceProps.Humanlike && CS_0024_003C_003E8__locals12.IsCapableOfOpening(item3) && item3.CanReach(CS_0024_003C_003E8__locals12, PathEndMode.InteractionCell, Danger.Deadly))
						{
							tmpAllowedPawns.Add(item3);
						}
					}
					if (tmpAllowedPawns.Count <= 0)
					{
						break;
					}
					_003C_003E2__current = new FloatMenuOption("Open".Translate(CS_0024_003C_003E8__locals12), delegate
					{
						tmpAllowedPawns[0].jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Open, CS_0024_003C_003E8__locals12), JobTag.Misc);
						for (int i = 1; i < tmpAllowedPawns.Count; i++)
						{
							FloatMenuOptionProvider_DraftedMove.PawnGotoAction(CS_0024_003C_003E8__locals12.Position, tmpAllowedPawns[i], RCellFinder.BestOrderedGotoDestNear(CS_0024_003C_003E8__locals12.Position, tmpAllowedPawns[i]));
						}
					});
					_003C_003E1__state = 4;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetMultiSelectFloatMenuOptions_003Ed__13 _003CGetMultiSelectFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetMultiSelectFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetMultiSelectFloatMenuOptions_003Ed__ = new _003CGetMultiSelectFloatMenuOptions_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetMultiSelectFloatMenuOptions_003Ed__.selPawns = _003C_003E3__selPawns;
			return _003CGetMultiSelectFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	public const string CrateContentsChanged = "CrateContentsChanged";

	private CompHackable hackable;

	private static List<Pawn> tmpAllowedPawns = new List<Pawn>();

	public override int OpenTicks => 100;

	public override bool CanOpen
	{
		get
		{
			if (hackable == null)
			{
				hackable = GetComp<CompHackable>();
			}
			if (hackable != null && !hackable.IsHacked)
			{
				return false;
			}
			return base.CanOpen;
		}
	}

	public bool ShouldBeLitNow()
	{
		return base.HasAnyContents;
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		if (def != ThingDefOf.AncientSecurityCrate || ModLister.CheckIdeology("Ancient security crate"))
		{
			base.SpawnSetup(map, respawningAfterLoad);
		}
	}

	public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
	{
		if (base.TryAcceptThing(thing, allowSpecialEffects))
		{
			BroadcastCompSignal("CrateContentsChanged");
			return true;
		}
		return false;
	}

	public override void EjectContents()
	{
		innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, (IntVec3 c) => c.GetEdifice(base.Map) == null);
		contentsKnown = true;
		if (def.building.openingEffect != null)
		{
			Effecter effecter = def.building.openingEffect.Spawn();
			effecter.Trigger(new TargetInfo(base.Position, base.Map), null);
			effecter.Cleanup();
		}
		BroadcastCompSignal("CrateContentsChanged");
	}

	protected override void ReceiveCompSignal(string signal)
	{
		base.ReceiveCompSignal(signal);
		if (signal == "Hacked")
		{
			Open();
		}
	}

	public override void Open()
	{
		contentsKnown = true;
		if (CanOpen)
		{
			base.Open();
		}
	}

	[IteratorStateMachine(typeof(_003CGetMultiSelectFloatMenuOptions_003Ed__13))]
	public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetMultiSelectFloatMenuOptions_003Ed__13(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawns = selPawns
		};
	}

	private bool IsCapableOfOpening(Pawn pawn)
	{
		if (!pawn.IsSubhuman)
		{
			return pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}
		return false;
	}
}
