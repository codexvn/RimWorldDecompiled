using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

[StaticConstructorOnStartup]
public class Settlement : MapParent, ITrader, ITraderRestockingInfoProvider, INameableWorldObject
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass66_0
	{
		public Caravan caravan;

		public Settlement _003C_003E4__this;

		internal void _003CGetCaravanGizmos_003Eb__0()
		{
			SettlementUtility.Attack(caravan, _003C_003E4__this);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass69_0
	{
		public Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Settlement _003C_003E4__this;

		public IEnumerable<IThingHolder> pods;

		public CompTransporter firstPod;

		public Func<FloatMenuAcceptanceReport> _003C_003E9__2;

		public Func<TransportersArrivalAction_TransportShip> _003C_003E9__3;

		internal void _003CGetShuttleFloatMenuOptions_003Eb__0()
		{
			launchAction(_003C_003E4__this.Tile, new TransportersArrivalAction_VisitSettlement(_003C_003E4__this, "MessageShuttleArrived"));
		}

		internal void _003CGetShuttleFloatMenuOptions_003Eb__1()
		{
			launchAction(_003C_003E4__this.Tile, new TransportersArrivalAction_Trade(_003C_003E4__this, "MessageShuttleArrived"));
		}

		internal FloatMenuAcceptanceReport _003CGetShuttleFloatMenuOptions_003Eb__2()
		{
			return TransportersArrivalAction_AttackSettlement.CanAttack(pods, _003C_003E4__this);
		}

		internal TransportersArrivalAction_TransportShip _003CGetShuttleFloatMenuOptions_003Eb__3()
		{
			return new TransportersArrivalAction_TransportShip(_003C_003E4__this, firstPod.Shuttle.shipParent);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass69_1
	{
		public TaggedString message;

		public _003C_003Ec__DisplayClass69_0 CS_0024_003C_003E8__locals1;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E9__4;

		internal void _003CGetShuttleFloatMenuOptions_003Eb__4(PlanetTile t, TransportersArrivalAction s)
		{
			_003C_003Ec__DisplayClass69_2 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass69_2
			{
				CS_0024_003C_003E8__locals2 = this,
				t = t,
				s = s
			};
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(message, delegate
			{
				CS_0024_003C_003E8__locals3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.launchAction(CS_0024_003C_003E8__locals3.t, CS_0024_003C_003E8__locals3.s);
			}));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass69_2
	{
		public PlanetTile t;

		public TransportersArrivalAction s;

		public _003C_003Ec__DisplayClass69_1 CS_0024_003C_003E8__locals2;

		internal void _003CGetShuttleFloatMenuOptions_003Eb__5()
		{
			CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.launchAction(t, s);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetCaravanGizmos_003Ed__66 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Caravan caravan;

		public Caravan _003C_003E3__caravan;

		public Settlement _003C_003E4__this;

		private _003C_003Ec__DisplayClass66_0 _003C_003E8__1;

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
		public _003CGetCaravanGizmos_003Ed__66(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 3)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Settlement settlement = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass66_0();
					_003C_003E8__1.caravan = caravan;
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					if (settlement.CanTradeNow && CaravanVisitUtility.SettlementVisitedNow(_003C_003E8__1.caravan) == settlement)
					{
						_003C_003E2__current = CaravanVisitUtility.TradeCommand(_003C_003E8__1.caravan, settlement.Faction, settlement.TraderKind);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00b5;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00b5;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00f9;
				case 3:
					_003C_003E1__state = -3;
					goto IL_0148;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00f9:
					_003C_003E7__wrap1 = ((WorldObject)settlement).GetCaravanGizmos(_003C_003E8__1.caravan).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0148;
					IL_0148:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (settlement.Attackable)
					{
						_003C_003E2__current = new Command_Action
						{
							icon = AttackCommand,
							defaultLabel = "CommandAttackSettlement".Translate(),
							defaultDesc = "CommandAttackSettlementDesc".Translate(),
							action = delegate
							{
								SettlementUtility.Attack(_003C_003E8__1.caravan, _003C_003E8__1._003C_003E4__this);
							}
						};
						_003C_003E1__state = 4;
						return true;
					}
					break;
					IL_00b5:
					if ((bool)CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(_003C_003E8__1.caravan, settlement))
					{
						_003C_003E2__current = FactionGiftUtility.OfferGiftsCommand(_003C_003E8__1.caravan, settlement);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00f9;
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
			_003CGetCaravanGizmos_003Ed__66 _003CGetCaravanGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetCaravanGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetCaravanGizmos_003Ed__ = new _003CGetCaravanGizmos_003Ed__66(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetCaravanGizmos_003Ed__.caravan = _003C_003E3__caravan;
			return _003CGetCaravanGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__67 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Settlement _003C_003E4__this;

		private Caravan caravan;

		public Caravan _003C_003E3__caravan;

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
		public _003CGetFloatMenuOptions_003Ed__67(int _003C_003E1__state)
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
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			case -7:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally5();
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
				Settlement settlement = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)settlement).GetFloatMenuOptions(caravan).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0084;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0084;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00f8;
				case 3:
					_003C_003E1__state = -5;
					goto IL_015e;
				case 4:
					_003C_003E1__state = -6;
					goto IL_01c4;
				case 5:
					{
						_003C_003E1__state = -7;
						break;
					}
					IL_0084:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CaravanVisitUtility.SettlementVisitedNow(caravan) != settlement)
					{
						_003C_003E7__wrap1 = CaravanArrivalAction_VisitSettlement.GetFloatMenuOptions(caravan, settlement).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00f8;
					}
					goto IL_0112;
					IL_0112:
					_003C_003E7__wrap1 = CaravanArrivalAction_Trade.GetFloatMenuOptions(caravan, settlement).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_015e;
					IL_00f8:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_0112;
					IL_01c4:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = CaravanArrivalAction_AttackSettlement.GetFloatMenuOptions(caravan, settlement).GetEnumerator();
					_003C_003E1__state = -7;
					break;
					IL_015e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current4 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = CaravanArrivalAction_OfferGifts.GetFloatMenuOptions(caravan, settlement).GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_01c4;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current5 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current5;
					_003C_003E1__state = 5;
					return true;
				}
				_003C_003Em__Finally5();
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
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
			_003CGetFloatMenuOptions_003Ed__67 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__67(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.caravan = _003C_003E3__caravan;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__65 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Settlement _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__65(int _003C_003E1__state)
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
				Settlement CS_0024_003C_003E8__locals7 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)CS_0024_003C_003E8__locals7).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0111;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals7.TraderKind != null && !CS_0024_003C_003E8__locals7.Faction.def.permanentEnemy)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandShowSellableItems".Translate(),
							defaultDesc = "CommandShowSellableItemsDesc".Translate(),
							icon = ShowSellableItemsCommand,
							action = delegate
							{
								Find.WindowStack.Add(new Dialog_SellableItems(CS_0024_003C_003E8__locals7));
								RoyalTitleDef titleRequiredToTrade = CS_0024_003C_003E8__locals7.TraderKind.TitleRequiredToTrade;
								if (titleRequiredToTrade != null)
								{
									TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.TradingRequiresPermit, titleRequiredToTrade.GetLabelCapForBothGenders());
								}
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0111;
					IL_0111:
					if (CS_0024_003C_003E8__locals7.Faction != Faction.OfPlayer && !PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.FormCaravan) && CS_0024_003C_003E8__locals7.Tile.LayerDef.canFormCaravans)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandFormCaravan".Translate(),
							defaultDesc = "CommandFormCaravanDesc".Translate(),
							icon = FormCaravanCommand,
							action = delegate
							{
								Find.Tutor.learningReadout.TryActivateConcept(ConceptDefOf.FormCaravan);
								Messages.Message("MessageSelectOwnBaseToFormCaravan".Translate(), MessageTypeDefOf.RejectInput, historical: false);
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__65 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__65(0)
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
	private sealed class _003CGetShuttleFloatMenuOptions_003Ed__69 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

		public Settlement _003C_003E4__this;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		private _003C_003Ec__DisplayClass69_0 _003C_003E8__1;

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
		public _003CGetShuttleFloatMenuOptions_003Ed__69(int _003C_003E1__state)
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
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Settlement settlement = _003C_003E4__this;
				IThingHolder thingHolder;
				_003C_003Ec__DisplayClass69_1 CS_0024_003C_003E8__locals7;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass69_0();
					_003C_003E8__1.launchAction = launchAction;
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.pods = pods;
					_003C_003E7__wrap1 = ((MapParent)settlement).GetShuttleFloatMenuOptions(_003C_003E8__1.pods, _003C_003E8__1.launchAction).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00ce;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00ce;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0157;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01c6;
				case 4:
					{
						_003C_003E1__state = -4;
						goto IL_0365;
					}
					IL_00ce:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if ((bool)TransportersArrivalAction_VisitSettlement.CanVisit(_003C_003E8__1.pods, settlement))
					{
						_003C_003E2__current = new FloatMenuOption("VisitSettlement".Translate(settlement.Label), delegate
						{
							_003C_003E8__1.launchAction(_003C_003E8__1._003C_003E4__this.Tile, new TransportersArrivalAction_VisitSettlement(_003C_003E8__1._003C_003E4__this, "MessageShuttleArrived"));
						});
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0157;
					IL_01c6:
					if (settlement.HasMap)
					{
						break;
					}
					thingHolder = _003C_003E8__1.pods.FirstOrDefault();
					_003C_003E8__1.firstPod = thingHolder as CompTransporter;
					if (_003C_003E8__1.firstPod == null || _003C_003E8__1.firstPod.Shuttle.shipParent == null)
					{
						break;
					}
					CS_0024_003C_003E8__locals7 = new _003C_003Ec__DisplayClass69_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						message = (settlement.Faction.HostileTo(Faction.OfPlayer) ? "ConfirmLandOnHostileFactionBase".Translate(settlement.Faction) : "ConfirmLandOnNeutralFactionBase".Translate(settlement.Faction))
					};
					_003C_003E7__wrap1 = TransportersArrivalActionUtility.GetFloatMenuOptions(() => TransportersArrivalAction_AttackSettlement.CanAttack(CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1.pods, CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1._003C_003E4__this), () => new TransportersArrivalAction_TransportShip(CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1._003C_003E4__this, CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1.firstPod.Shuttle.shipParent), "AttackShuttle".Translate(settlement.Label), delegate(PlanetTile t, TransportersArrivalAction s)
					{
						_003C_003Ec__DisplayClass69_2 CS_0024_003C_003E8__locals10 = new _003C_003Ec__DisplayClass69_2
						{
							CS_0024_003C_003E8__locals2 = CS_0024_003C_003E8__locals7,
							t = t,
							s = s
						};
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(CS_0024_003C_003E8__locals7.message, delegate
						{
							CS_0024_003C_003E8__locals10.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.launchAction(CS_0024_003C_003E8__locals10.t, CS_0024_003C_003E8__locals10.s);
						}));
					}, settlement.Tile).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0365;
					IL_0365:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					break;
					IL_0157:
					if ((bool)TransportersArrivalAction_Trade.CanTradeWith(_003C_003E8__1.pods, settlement))
					{
						_003C_003E2__current = new FloatMenuOption("TradeWithSettlement".Translate(settlement.Label), delegate
						{
							_003C_003E8__1.launchAction(_003C_003E8__1._003C_003E4__this.Tile, new TransportersArrivalAction_Trade(_003C_003E8__1._003C_003E4__this, "MessageShuttleArrived"));
						});
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01c6;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetShuttleFloatMenuOptions_003Ed__69 _003CGetShuttleFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetShuttleFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetShuttleFloatMenuOptions_003Ed__ = new _003CGetShuttleFloatMenuOptions_003Ed__69(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetShuttleFloatMenuOptions_003Ed__.pods = _003C_003E3__pods;
			_003CGetShuttleFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			return _003CGetShuttleFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTransportersFloatMenuOptions_003Ed__68 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Settlement _003C_003E4__this;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

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
		public _003CGetTransportersFloatMenuOptions_003Ed__68(int _003C_003E1__state)
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
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
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
				Settlement settlement = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)settlement).GetTransportersFloatMenuOptions(pods, launchAction).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0086;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0086;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00f3;
				case 3:
					_003C_003E1__state = -5;
					goto IL_015f;
				case 4:
					{
						_003C_003E1__state = -6;
						goto IL_01d0;
					}
					IL_0086:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = TransportersArrivalAction_VisitSettlement.GetFloatMenuOptions(launchAction, pods, settlement, isShuttle: false).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00f3;
					IL_00f3:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = TransportersArrivalAction_GiveGift.GetFloatMenuOptions(launchAction, pods, settlement).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_015f;
					IL_01d0:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					break;
					IL_015f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current4 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					if (settlement.HasMap)
					{
						break;
					}
					_003C_003E7__wrap1 = TransportersArrivalAction_AttackSettlement.GetFloatMenuOptions(launchAction, pods, settlement).GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_01d0;
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

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
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
			_003CGetTransportersFloatMenuOptions_003Ed__68 _003CGetTransportersFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTransportersFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetTransportersFloatMenuOptions_003Ed__ = new _003CGetTransportersFloatMenuOptions_003Ed__68(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetTransportersFloatMenuOptions_003Ed__.pods = _003C_003E3__pods;
			_003CGetTransportersFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			return _003CGetTransportersFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CIncidentTargetTags_003Ed__57 : IEnumerable<IncidentTargetTagDef>, IEnumerable, IEnumerator<IncidentTargetTagDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IncidentTargetTagDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Settlement _003C_003E4__this;

		private IEnumerator<IncidentTargetTagDef> _003C_003E7__wrap1;

		IncidentTargetTagDef IEnumerator<IncidentTargetTagDef>.Current
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
		public _003CIncidentTargetTags_003Ed__57(int _003C_003E1__state)
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
				Settlement settlement = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)settlement).IncidentTargetTags().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					break;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						IncidentTargetTagDef current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (settlement.Faction == null || settlement.Faction == Faction.OfPlayer || SettlementDefeatUtility.IsDefeated(settlement.Map, settlement.Faction))
					{
						_003C_003E2__current = IncidentTargetTagDefOf.Map_PlayerHome;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E2__current = IncidentTargetTagDefOf.Map_Misc;
					_003C_003E1__state = 3;
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
		IEnumerator<IncidentTargetTagDef> IEnumerable<IncidentTargetTagDef>.GetEnumerator()
		{
			_003CIncidentTargetTags_003Ed__57 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CIncidentTargetTags_003Ed__57(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IncidentTargetTagDef>)this).GetEnumerator();
		}
	}

	public Settlement_TraderTracker trader;

	public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

	private string nameInt;

	public bool namedByPlayer;

	private Material cachedMat;

	public static readonly Texture2D ShowSellableItemsCommand = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems");

	public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan");

	public static readonly Texture2D AttackCommand = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement");

	public string Name
	{
		get
		{
			return nameInt;
		}
		set
		{
			nameInt = value;
		}
	}

	public override Texture2D ExpandingIcon => base.Faction.def.FactionIcon;

	public override string Label => nameInt ?? base.Label;

	public override bool HasName => !nameInt.NullOrEmpty();

	protected override bool UseGenericEnterMapFloatMenuOption => !Attackable;

	public virtual bool Visitable
	{
		get
		{
			if (base.Faction != Faction.OfPlayer && (base.Faction == null || !base.Faction.HostileTo(Faction.OfPlayer)))
			{
				return !base.Tile.LayerDef.isSpace;
			}
			return false;
		}
	}

	public virtual bool Attackable => base.Faction != Faction.OfPlayer;

	public override bool ShowRelatedQuests => base.Faction != Faction.OfPlayer;

	public override bool GravShipCanLandOn => base.Faction != Faction.OfPlayer;

	public override AcceptanceReport CanBeSettled => false;

	public override Material Material
	{
		get
		{
			if (cachedMat == null)
			{
				cachedMat = MaterialPool.MatFrom(base.Faction.def.settlementTexturePath, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, 3550);
			}
			return cachedMat;
		}
	}

	public override MapGeneratorDef MapGeneratorDef
	{
		get
		{
			if (def.mapGenerator != null)
			{
				return def.mapGenerator;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				return MapGeneratorDefOf.Base_Player;
			}
			return MapGeneratorDefOf.Base_Faction;
		}
	}

	public TraderKindDef TraderKind => trader?.TraderKind;

	public IEnumerable<Thing> Goods => trader?.StockListForReading;

	public int RandomPriceFactorSeed => trader?.RandomPriceFactorSeed ?? 0;

	public string TraderName => trader?.TraderName;

	public bool CanTradeNow
	{
		get
		{
			if (trader != null)
			{
				return trader.CanTradeNow;
			}
			return false;
		}
	}

	public float TradePriceImprovementOffsetForPlayer => trader?.TradePriceImprovementOffsetForPlayer ?? 0f;

	public TradeCurrency TradeCurrency => TraderKind.tradeCurrency;

	public bool EverVisited => trader.EverVisited;

	public bool RestockedSinceLastVisit => trader.RestockedSinceLastVisit;

	public int NextRestockTick => trader.NextRestockTick;

	public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
	{
		return trader?.ColonyThingsWillingToBuy(playerNegotiator);
	}

	public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
	}

	public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
	}

	public Settlement()
	{
		trader = new Settlement_TraderTracker(this);
	}

	[IteratorStateMachine(typeof(_003CIncidentTargetTags_003Ed__57))]
	public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CIncidentTargetTags_003Ed__57(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref previouslyGeneratedInhabitants, "previouslyGeneratedInhabitants", LookMode.Reference);
		Scribe_Deep.Look(ref trader, "trader", this);
		Scribe_Values.Look(ref nameInt, "nameInt");
		Scribe_Values.Look(ref namedByPlayer, "namedByPlayer", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			previouslyGeneratedInhabitants.RemoveAll((Pawn x) => x == null);
			if (base.Faction == null)
			{
				Log.Warning("Settlement '" + Name + "' had null faction on load - destroying.");
				Destroy();
			}
		}
	}

	public override void PostMapGenerate()
	{
		base.PostMapGenerate();
		if (!base.Map.IsPlayerHome && TryGetComponent<TimedDetectionRaids>(out var comp))
		{
			comp.StartDetectionCountdown(240000);
		}
	}

	protected override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		if (trader != null)
		{
			trader.TraderTrackerTick();
		}
		SettlementDefeatUtility.CheckDefeated(this);
	}

	public override void Notify_MyMapRemoved(Map map)
	{
		base.Notify_MyMapRemoved(map);
		for (int num = previouslyGeneratedInhabitants.Count - 1; num >= 0; num--)
		{
			Pawn pawn = previouslyGeneratedInhabitants[num];
			if (pawn.DestroyedOrNull() || !pawn.IsWorldPawn())
			{
				previouslyGeneratedInhabitants.RemoveAt(num);
			}
		}
	}

	public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
	{
		alsoRemoveWorldObject = false;
		if (base.Map.AnyBuildingBlockingMapRemoval)
		{
			return false;
		}
		if (base.Map.IsPlayerHome || base.Map.mapPawns.AnyPawnBlockingMapRemoval)
		{
			return false;
		}
		if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
		{
			return false;
		}
		return true;
	}

	public override void PostRemove()
	{
		base.PostRemove();
		if (trader != null)
		{
			trader.TryDestroyStock();
		}
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (base.Faction != Faction.OfPlayer)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += base.Faction.PlayerRelationKind.GetLabelCap();
			if (!base.Faction.Hidden)
			{
				text = text + " (" + base.Faction.PlayerGoodwill.ToStringWithSign() + ")";
			}
			RoyalTitleDef royalTitleDef = TraderKind?.TitleRequiredToTrade;
			if (royalTitleDef != null)
			{
				text += "\n" + "RequiresTradePermission".Translate(royalTitleDef.GetLabelCapForBothGenders());
			}
		}
		else
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "Settled".Translate() + ": " + GenDate.DateShortStringAt(GenDate.TickGameToAbs(creationGameTicks), Find.WorldGrid.LongLatOf(base.Tile));
			text += " (";
			text += "TimeAgo".Translate((Find.TickManager.TicksGame - creationGameTicks).ToStringTicksToPeriodVague());
			text += ")";
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__65))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__65(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetCaravanGizmos_003Ed__66))]
	public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCaravanGizmos_003Ed__66(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__caravan = caravan
		};
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__67))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__67(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__caravan = caravan
		};
	}

	[IteratorStateMachine(typeof(_003CGetTransportersFloatMenuOptions_003Ed__68))]
	public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTransportersFloatMenuOptions_003Ed__68(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pods = pods,
			_003C_003E3__launchAction = launchAction
		};
	}

	[IteratorStateMachine(typeof(_003CGetShuttleFloatMenuOptions_003Ed__69))]
	public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetShuttleFloatMenuOptions_003Ed__69(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pods = pods,
			_003C_003E3__launchAction = launchAction
		};
	}

	public override void GetChildHolders(List<IThingHolder> outChildren)
	{
		base.GetChildHolders(outChildren);
		if (trader != null)
		{
			outChildren.Add(trader);
		}
	}

	public override void Abandon(bool wasGravshipLaunch)
	{
		base.Abandon(wasGravshipLaunch);
		if (!wasGravshipLaunch && base.Tile.LayerDef == PlanetLayerDefOf.Surface)
		{
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedSettlement);
			worldObject.Tile = base.Tile;
			worldObject.SetFaction(base.Faction);
			Find.WorldObjects.Add(worldObject);
		}
	}
}
