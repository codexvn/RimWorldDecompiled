using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld;

public class RoyalTitlePermitWorker_CallLaborers : RoyalTitlePermitWorker_Targeted
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public RoyalTitlePermitWorker_CallLaborers _003C_003E4__this;

		public Pawn pawn;

		public Map map;

		public Faction faction;

		public bool free;

		internal void _003CGetRoyalAidOptions_003Eb__0()
		{
			_003C_003E4__this.BeginCallLaborers(pawn, map, faction, free);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetRoyalAidOptions_003Ed__5 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitlePermitWorker_CallLaborers _003C_003E4__this;

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
		public _003CGetRoyalAidOptions_003Ed__5(int _003C_003E1__state)
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
			RoyalTitlePermitWorker_CallLaborers royalTitlePermitWorker_CallLaborers = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass5_0 CS_0024_003C_003E8__locals13 = new _003C_003Ec__DisplayClass5_0
				{
					_003C_003E4__this = _003C_003E4__this,
					pawn = pawn,
					map = map,
					faction = faction
				};
				if (royalTitlePermitWorker_CallLaborers.AidDisabled_NewTemp(CS_0024_003C_003E8__locals13.map, CS_0024_003C_003E8__locals13.pawn, CS_0024_003C_003E8__locals13.faction, out var reason))
				{
					_003C_003E2__current = new FloatMenuOption(royalTitlePermitWorker_CallLaborers.def.LabelCap + ": " + reason, null);
					_003C_003E1__state = 1;
					return true;
				}
				Action action = null;
				string description = royalTitlePermitWorker_CallLaborers.def.LabelCap + " (" + "CommandCallLaborersNumLaborers".Translate(royalTitlePermitWorker_CallLaborers.def.royalAid.pawnCount) + "): ";
				if (royalTitlePermitWorker_CallLaborers.FillAidOption(CS_0024_003C_003E8__locals13.pawn, CS_0024_003C_003E8__locals13.faction, ref description, out CS_0024_003C_003E8__locals13.free))
				{
					action = delegate
					{
						CS_0024_003C_003E8__locals13._003C_003E4__this.BeginCallLaborers(CS_0024_003C_003E8__locals13.pawn, CS_0024_003C_003E8__locals13.map, CS_0024_003C_003E8__locals13.faction, CS_0024_003C_003E8__locals13.free);
					};
				}
				_003C_003E2__current = new FloatMenuOption(description, action, CS_0024_003C_003E8__locals13.faction.def.FactionIcon, CS_0024_003C_003E8__locals13.faction.Color);
				_003C_003E1__state = 2;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
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
			_003CGetRoyalAidOptions_003Ed__5 _003CGetRoyalAidOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRoyalAidOptions_003Ed__ = this;
			}
			else
			{
				_003CGetRoyalAidOptions_003Ed__ = new _003CGetRoyalAidOptions_003Ed__5(0)
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

	public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
	{
		if (!CanHitTarget(target))
		{
			if (target.IsValid && showMessages)
			{
				Messages.Message(def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput);
			}
			return false;
		}
		AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, map, null, null);
		if (!acceptanceReport.Accepted)
		{
			Messages.Message(acceptanceReport.Reason, new LookTargets(target.Cell, map), MessageTypeDefOf.RejectInput, historical: false);
		}
		return acceptanceReport.Accepted;
	}

	public override void DrawHighlight(LocalTargetInfo target)
	{
		GenDraw.DrawRadiusRing(caller.Position, base.RangeClamped, Color.white);
		RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(target, map, ThingDefOf.Shuttle, ThingDefOf.Shuttle.defaultPlacingRot);
	}

	public override void OrderForceTarget(LocalTargetInfo target)
	{
		CallLaborers(target.Cell);
	}

	public override void OnGUI(LocalTargetInfo target)
	{
		if (!target.IsValid || !RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, map, null, null).Accepted)
		{
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}
	}

	[IteratorStateMachine(typeof(_003CGetRoyalAidOptions_003Ed__5))]
	public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRoyalAidOptions_003Ed__5(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__map = map,
			_003C_003E3__pawn = pawn,
			_003C_003E3__faction = faction
		};
	}

	private void BeginCallLaborers(Pawn pawn, Map map, Faction faction, bool free)
	{
		if (!faction.HostileTo(Faction.OfPlayer))
		{
			targetingParameters = new TargetingParameters();
			targetingParameters.canTargetLocations = true;
			targetingParameters.canTargetSelf = false;
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetFires = false;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.canTargetItems = true;
			caller = pawn;
			base.map = map;
			calledFaction = faction;
			base.free = free;
			float rangeActual = base.RangeClamped;
			targetingParameters.validator = (TargetInfo target) => (!(rangeActual > 0f) || !(target.Cell.DistanceTo(caller.Position) > rangeActual)) ? true : false;
			Find.Targeter.BeginTargeting(this);
		}
	}

	private void CallLaborers(IntVec3 landingCell)
	{
		QuestScriptDef permit_CallLaborers = QuestScriptDefOf.Permit_CallLaborers;
		Slate slate = new Slate();
		slate.Set("map", map);
		slate.Set("laborersCount", def.royalAid.pawnCount);
		slate.Set("permitFaction", calledFaction);
		slate.Set("laborersPawnKind", def.royalAid.pawnKindDef);
		slate.Set("laborersDurationDays", def.royalAid.aidDurationDays);
		slate.Set("landingCell", landingCell);
		QuestUtility.GenerateQuestAndMakeAvailable(permit_CallLaborers, slate);
		caller.royalty.GetPermit(def, calledFaction).Notify_Used();
		if (!free)
		{
			caller.royalty.TryRemoveFavor(calledFaction, def.royalAid.favorCost);
		}
	}
}
