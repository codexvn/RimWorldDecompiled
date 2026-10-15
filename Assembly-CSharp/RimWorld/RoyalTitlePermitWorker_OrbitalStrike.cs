using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class RoyalTitlePermitWorker_OrbitalStrike : RoyalTitlePermitWorker_Targeted
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public RoyalTitlePermitWorker_OrbitalStrike _003C_003E4__this;

		public Pawn pawn;

		public Faction faction;

		public Map map;

		public bool free;

		internal void _003CGetRoyalAidOptions_003Eb__0()
		{
			_003C_003E4__this.BeginCallBombardment(pawn, faction, map, free);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetRoyalAidOptions_003Ed__4 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitlePermitWorker_OrbitalStrike _003C_003E4__this;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private Faction faction;

		public Faction _003C_003E3__faction;

		private Map map;

		public Map _003C_003E3__map;

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
		public _003CGetRoyalAidOptions_003Ed__4(int _003C_003E1__state)
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
			RoyalTitlePermitWorker_OrbitalStrike royalTitlePermitWorker_OrbitalStrike = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass4_0 CS_0024_003C_003E8__locals15 = new _003C_003Ec__DisplayClass4_0
				{
					_003C_003E4__this = _003C_003E4__this,
					pawn = pawn,
					faction = faction,
					map = map
				};
				if (royalTitlePermitWorker_OrbitalStrike.AidDisabled_NewTemp(CS_0024_003C_003E8__locals15.map, CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.faction, out var reason, temperatureMatters: false))
				{
					_003C_003E2__current = new FloatMenuOption(royalTitlePermitWorker_OrbitalStrike.def.LabelCap + ": " + reason, null);
					_003C_003E1__state = 1;
					return true;
				}
				if (CS_0024_003C_003E8__locals15.faction.HostileTo(Faction.OfPlayer))
				{
					_003C_003E2__current = new FloatMenuOption(royalTitlePermitWorker_OrbitalStrike.def.LabelCap + ": " + "CommandCallRoyalAidFactionHostile".Translate(CS_0024_003C_003E8__locals15.faction.Named("FACTION")), null);
					_003C_003E1__state = 2;
					return true;
				}
				string description = royalTitlePermitWorker_OrbitalStrike.def.LabelCap + ": ";
				Action action = null;
				if (royalTitlePermitWorker_OrbitalStrike.FillAidOption(CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.faction, ref description, out CS_0024_003C_003E8__locals15.free))
				{
					action = delegate
					{
						CS_0024_003C_003E8__locals15._003C_003E4__this.BeginCallBombardment(CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.faction, CS_0024_003C_003E8__locals15.map, CS_0024_003C_003E8__locals15.free);
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
			_003CGetRoyalAidOptions_003Ed__4 _003CGetRoyalAidOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRoyalAidOptions_003Ed__ = this;
			}
			else
			{
				_003CGetRoyalAidOptions_003Ed__ = new _003CGetRoyalAidOptions_003Ed__4(0)
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

	private Faction faction;

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
		return true;
	}

	public override void DrawHighlight(LocalTargetInfo target)
	{
		GenDraw.DrawRadiusRing(caller.Position, base.RangeClamped, Color.white);
		GenDraw.DrawRadiusRing(target.Cell, def.royalAid.radius + def.royalAid.explosionRadiusRange.max, Color.white);
		if (target.IsValid)
		{
			GenDraw.DrawTargetHighlight(target);
		}
	}

	public override void OrderForceTarget(LocalTargetInfo target)
	{
		CallBombardment(target.Cell);
	}

	[IteratorStateMachine(typeof(_003CGetRoyalAidOptions_003Ed__4))]
	public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRoyalAidOptions_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__map = map,
			_003C_003E3__pawn = pawn,
			_003C_003E3__faction = faction
		};
	}

	private void BeginCallBombardment(Pawn caller, Faction faction, Map map, bool free)
	{
		targetingParameters = new TargetingParameters();
		targetingParameters.canTargetLocations = true;
		targetingParameters.canTargetSelf = true;
		targetingParameters.canTargetFires = true;
		targetingParameters.canTargetItems = true;
		base.caller = caller;
		base.map = map;
		this.faction = faction;
		base.free = free;
		float rangeActual = base.RangeClamped;
		targetingParameters.validator = delegate(TargetInfo target)
		{
			if (rangeActual > 0f && target.Cell.DistanceTo(caller.Position) > rangeActual)
			{
				return false;
			}
			if (target.Cell.Fogged(map))
			{
				return false;
			}
			RoofDef roof = target.Cell.GetRoof(map);
			return (roof == null || !roof.isThickRoof) ? true : false;
		};
		Find.Targeter.BeginTargeting(this);
	}

	private void CallBombardment(IntVec3 targetCell)
	{
		Bombardment obj = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, targetCell, map);
		obj.impactAreaRadius = def.royalAid.radius;
		obj.explosionRadiusRange = def.royalAid.explosionRadiusRange;
		obj.bombIntervalTicks = def.royalAid.intervalTicks;
		obj.randomFireRadius = 1;
		obj.explosionCount = def.royalAid.explosionCount;
		obj.warmupTicks = def.royalAid.warmupTicks;
		obj.instigator = caller;
		SoundDefOf.OrbitalStrike_Ordered.PlayOneShotOnCamera();
		caller.royalty.GetPermit(def, faction).Notify_Used();
		if (!free)
		{
			caller.royalty.TryRemoveFavor(faction, def.royalAid.favorCost);
		}
	}
}
