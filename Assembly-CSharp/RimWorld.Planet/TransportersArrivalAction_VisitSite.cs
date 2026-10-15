using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet;

public class TransportersArrivalAction_VisitSite : TransportersArrivalAction
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass11_0
	{
		public Site site;

		public IEnumerable<IThingHolder> pods;

		public Func<FloatMenuAcceptanceReport> _003C_003E9__2;

		public Func<TransportersArrivalAction_VisitSite> _003C_003E9__3;

		public Func<FloatMenuAcceptanceReport> _003C_003E9__4;

		public Func<TransportersArrivalAction_VisitSite> _003C_003E9__5;

		internal FloatMenuAcceptanceReport _003CGetFloatMenuOptions_003Eb__2()
		{
			return CanVisit(pods, site);
		}

		internal TransportersArrivalAction_VisitSite _003CGetFloatMenuOptions_003Eb__3()
		{
			return new TransportersArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.EdgeDrop);
		}

		internal FloatMenuAcceptanceReport _003CGetFloatMenuOptions_003Eb__4()
		{
			return CanVisit(pods, site);
		}

		internal TransportersArrivalAction_VisitSite _003CGetFloatMenuOptions_003Eb__5()
		{
			return new TransportersArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.CenterDrop);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__11 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Site site;

		public Site _003C_003E3__site;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

		private _003C_003Ec__DisplayClass11_0 _003C_003E8__1;

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
		public _003CGetFloatMenuOptions_003Ed__11(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
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
					_003C_003E8__1 = new _003C_003Ec__DisplayClass11_0();
					_003C_003E8__1.site = site;
					_003C_003E8__1.pods = pods;
					_003C_003E7__wrap1 = TransportersArrivalActionUtility.GetFloatMenuOptions(() => CanVisit(_003C_003E8__1.pods, _003C_003E8__1.site), () => new TransportersArrivalAction_VisitSite(_003C_003E8__1.site, PawnsArrivalModeDefOf.EdgeDrop), "DropAtEdge".Translate(), launchAction, _003C_003E8__1.site.Tile, UIConfirmationCallback).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_012b;
				case 1:
					_003C_003E1__state = -3;
					goto IL_012b;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_012b:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = TransportersArrivalActionUtility.GetFloatMenuOptions(() => CanVisit(_003C_003E8__1.pods, _003C_003E8__1.site), () => new TransportersArrivalAction_VisitSite(_003C_003E8__1.site, PawnsArrivalModeDefOf.CenterDrop), "DropInCenter".Translate(), launchAction, _003C_003E8__1.site.Tile, UIConfirmationCallback).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			void UIConfirmationCallback(Action action)
			{
				if (ModsConfig.OdysseyActive && ((_003C_003Ec__DisplayClass11_0)this).site.Tile.LayerDef == PlanetLayerDefOf.Orbit)
				{
					TaggedString text = "OrbitalWarning".Translate();
					text += $"\n\n{"LaunchToConfirmation".Translate()}";
					Find.WindowStack.Add(new Dialog_MessageBox(text, null, action, "Cancel".Translate(), delegate
					{
					}, null, buttonADestructive: true));
				}
				else
				{
					action();
				}
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
			_003CGetFloatMenuOptions_003Ed__11 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__11(0);
			}
			_003CGetFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			_003CGetFloatMenuOptions_003Ed__.pods = _003C_003E3__pods;
			_003CGetFloatMenuOptions_003Ed__.site = _003C_003E3__site;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private Site site;

	private PawnsArrivalModeDef arrivalMode;

	public override bool GeneratesMap => true;

	public TransportersArrivalAction_VisitSite()
	{
	}

	public TransportersArrivalAction_VisitSite(Site site, PawnsArrivalModeDef arrivalMode)
	{
		this.site = site;
		this.arrivalMode = arrivalMode;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref site, "site");
		Scribe_Defs.Look(ref arrivalMode, "arrivalMode");
	}

	public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
	{
		FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
		if (!floatMenuAcceptanceReport)
		{
			return floatMenuAcceptanceReport;
		}
		if (site != null && site.Tile != destinationTile)
		{
			return false;
		}
		return CanVisit(pods, site);
	}

	public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
	{
		return !site.HasMap;
	}

	public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
	{
		Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
		bool num = !site.HasMap;
		Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(site.Tile, site.PreferredMapSize, null);
		if (num)
		{
			Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, informEvenIfSeenBefore: true);
		}
		if (site.Faction != null && site.Faction != Faction.OfPlayer && site.MainSitePartDef.considerEnteringAsAttack)
		{
			Faction.OfPlayer.TryAffectGoodwillWith(site.Faction, Faction.OfPlayer.GoodwillToMakeHostile(site.Faction), canSendMessage: true, canSendHostilityLetter: true, HistoryEventDefOf.AttackedSettlement, null);
		}
		if (transporters.IsShuttle())
		{
			Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion);
		}
		else
		{
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion);
		}
		arrivalMode.Worker.TravellingTransportersArrived(transporters, orGenerateMap);
	}

	public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Site site)
	{
		if (site == null || !site.Spawned)
		{
			return false;
		}
		if (!TransportersArrivalActionUtility.AnyNonDownedColonist(pods))
		{
			return false;
		}
		if (site.EnterCooldownBlocksEntering())
		{
			return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(site.EnterCooldownTicksLeft().ToStringTicksToPeriod()));
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__11))]
	public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Site site)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__11(-2)
		{
			_003C_003E3__launchAction = launchAction,
			_003C_003E3__pods = pods,
			_003C_003E3__site = site
		};
	}
}
