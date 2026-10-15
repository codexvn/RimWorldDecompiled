using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RoyalTitlePermitWorker_CallAid : RoyalTitlePermitWorker_Targeted
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public RoyalTitlePermitWorker_CallAid _003C_003E4__this;

		public Pawn pawn;

		public Map map;

		public Faction faction;

		public bool free;

		internal void _003CGetRoyalAidOptions_003Eb__0()
		{
			_003C_003E4__this.BeginCallAid(pawn, map, faction, free);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetRoyalAidOptions_003Ed__2 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitlePermitWorker_CallAid _003C_003E4__this;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private Map map;

		public Map _003C_003E3__map;

		private Faction faction;

		public Faction _003C_003E3__faction;

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
		public _003CGetRoyalAidOptions_003Ed__2(int _003C_003E1__state)
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
			RoyalTitlePermitWorker_CallAid royalTitlePermitWorker_CallAid = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals15 = new _003C_003Ec__DisplayClass2_0
				{
					_003C_003E4__this = _003C_003E4__this,
					pawn = pawn,
					map = map,
					faction = faction
				};
				if (royalTitlePermitWorker_CallAid.AidDisabled_NewTemp(CS_0024_003C_003E8__locals15.map, CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.faction, out var reason))
				{
					_003C_003E2__current = new FloatMenuOption(royalTitlePermitWorker_CallAid.def.LabelCap + ": " + reason, null);
					_003C_003E1__state = 1;
					return true;
				}
				if (NeutralGroupIncidentUtility.AnyBlockingHostileLord(CS_0024_003C_003E8__locals15.pawn.MapHeld, CS_0024_003C_003E8__locals15.faction))
				{
					_003C_003E2__current = new FloatMenuOption(royalTitlePermitWorker_CallAid.def.LabelCap + ": " + "HostileVisitorsPresent".Translate(), null);
					_003C_003E1__state = 2;
					return true;
				}
				Action action = null;
				string description = royalTitlePermitWorker_CallAid.def.LabelCap + ": ";
				if (royalTitlePermitWorker_CallAid.FillAidOption(CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.faction, ref description, out CS_0024_003C_003E8__locals15.free))
				{
					action = delegate
					{
						CS_0024_003C_003E8__locals15._003C_003E4__this.BeginCallAid(CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.map, CS_0024_003C_003E8__locals15.faction, CS_0024_003C_003E8__locals15.free);
					};
				}
				_003C_003E2__current = new FloatMenuOption(description, action, CS_0024_003C_003E8__locals15.faction.def.FactionIcon, CS_0024_003C_003E8__locals15.faction.Color);
				_003C_003E1__state = 3;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				return false;
			case 3:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetRoyalAidOptions_003Ed__2 _003CGetRoyalAidOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRoyalAidOptions_003Ed__ = this;
			}
			else
			{
				_003CGetRoyalAidOptions_003Ed__ = new _003CGetRoyalAidOptions_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetRoyalAidOptions_003Ed__.map = _003C_003E3__map;
			_003CGetRoyalAidOptions_003Ed__.pawn = _003C_003E3__pawn;
			_003CGetRoyalAidOptions_003Ed__.faction = _003C_003E3__faction;
			return _003CGetRoyalAidOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private Faction calledFaction;

	private float biocodeChance;

	[IteratorStateMachine(typeof(_003CGetRoyalAidOptions_003Ed__2))]
	public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRoyalAidOptions_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__map = map,
			_003C_003E3__pawn = pawn,
			_003C_003E3__faction = faction
		};
	}

	private void BeginCallAid(Pawn caller, Map map, Faction faction, bool free, float biocodeChance = 1f)
	{
		IEnumerable<Faction> source = from f in (from p in map.mapPawns.AllPawnsSpawned
				where p.Faction != null && !p.Faction.IsPlayer && p.Faction != faction && !p.IsPrisonerOfColony
				select p.Faction).Distinct()
			where f.HostileTo(Faction.OfPlayer) && !faction.HostileTo(f)
			select f;
		if (source.Any())
		{
			Find.WindowStack.Add(new Dialog_MessageBox("CommandCallRoyalAidWarningNonHostileFactions".Translate(faction, source.Select((Faction f) => f.NameColored.Resolve()).ToCommaList()), "Confirm".Translate(), Call, "GoBack".Translate()));
		}
		else
		{
			Call();
		}
		void Call()
		{
			targetingParameters = new TargetingParameters();
			targetingParameters.canTargetLocations = true;
			targetingParameters.canTargetSelf = false;
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetFires = false;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.canTargetItems = false;
			base.caller = caller;
			base.map = map;
			calledFaction = faction;
			base.free = free;
			this.biocodeChance = biocodeChance;
			float rangeActual = base.RangeClamped;
			targetingParameters.validator = delegate(TargetInfo target)
			{
				if (rangeActual > 0f && target.Cell.DistanceTo(caller.Position) > rangeActual)
				{
					return false;
				}
				if (target.Cell.Fogged(map) || !DropCellFinder.CanPhysicallyDropInto(target.Cell, map, canRoofPunch: true))
				{
					return false;
				}
				return target.Cell.GetEdifice(map) == null && !target.Cell.Impassable(map);
			};
			Find.Targeter.BeginTargeting(this);
		}
	}

	public override void OrderForceTarget(LocalTargetInfo target)
	{
		CallAid(caller, map, target.Cell, calledFaction, free, biocodeChance);
	}

	private void CallAid(Pawn caller, Map map, IntVec3 spawnPos, Faction faction, bool free, float biocodeChance = 1f)
	{
		IncidentParms incidentParms = new IncidentParms();
		incidentParms.target = map;
		incidentParms.faction = faction;
		incidentParms.raidArrivalModeForQuickMilitaryAid = true;
		incidentParms.biocodeApparelChance = biocodeChance;
		incidentParms.biocodeWeaponsChance = biocodeChance;
		incidentParms.spawnCenter = spawnPos;
		if (def.royalAid.pawnKindDef != null)
		{
			incidentParms.pawnKind = def.royalAid.pawnKindDef;
			incidentParms.pawnCount = def.royalAid.pawnCount;
		}
		else
		{
			incidentParms.points = def.royalAid.points;
		}
		faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
		if (IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms))
		{
			if (!free)
			{
				caller.royalty.TryRemoveFavor(faction, def.royalAid.favorCost);
			}
			caller.royalty.GetPermit(def, faction).Notify_Used();
		}
		else
		{
			Log.Error("Could not send aid to map " + map?.ToString() + " from faction " + faction);
		}
	}

	protected override bool TemperatureIsAcceptable(Map map, Faction faction)
	{
		if (def.royalAid.pawnKindDef == null)
		{
			return base.TemperatureIsAcceptable(map, faction);
		}
		if (!def.royalAid.overrideAcceptableTemperatureRange.HasValue)
		{
			return base.TemperatureIsAcceptable(map, faction);
		}
		return def.royalAid.overrideAcceptableTemperatureRange.Value.Includes(map.mapTemperature.SeasonalTemp);
	}
}
