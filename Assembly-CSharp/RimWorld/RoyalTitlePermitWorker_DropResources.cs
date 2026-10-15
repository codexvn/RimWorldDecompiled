using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class RoyalTitlePermitWorker_DropResources : RoyalTitlePermitWorker_Targeted
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public RoyalTitlePermitWorker_DropResources _003C_003E4__this;

		public Pawn pawn;

		public Faction faction;

		public Map map;

		public bool free;

		internal void _003CGetRoyalAidOptions_003Eb__0()
		{
			_003C_003E4__this.BeginCallResources(pawn, faction, map, free);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public Pawn pawn;

		public RoyalTitlePermitWorker_DropResources _003C_003E4__this;

		public Faction faction;

		public Action _003C_003E9__1;

		internal void _003CGetCaravanGizmos_003Eb__0()
		{
			Caravan caravan = pawn.GetCaravan();
			float num = caravan.MassUsage;
			List<ThingDefCountClass> itemsToDrop = _003C_003E4__this.def.royalAid.itemsToDrop;
			for (int i = 0; i < itemsToDrop.Count; i++)
			{
				num += itemsToDrop[i].thingDef.BaseMass * (float)itemsToDrop[i].count;
			}
			if (num > caravan.MassCapacity)
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("DropResourcesOverweightConfirm".Translate(), delegate
				{
					_003C_003E4__this.CallResourcesToCaravan(pawn, faction, _003C_003E4__this.free);
				}, destructive: true));
			}
			else
			{
				_003C_003E4__this.CallResourcesToCaravan(pawn, faction, _003C_003E4__this.free);
			}
		}

		internal void _003CGetCaravanGizmos_003Eb__1()
		{
			_003C_003E4__this.CallResourcesToCaravan(pawn, faction, _003C_003E4__this.free);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetCaravanGizmos_003Ed__4 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		public RoyalTitlePermitWorker_DropResources _003C_003E4__this;

		private Faction faction;

		public Faction _003C_003E3__faction;

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
		public _003CGetCaravanGizmos_003Ed__4(int _003C_003E1__state)
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
			RoyalTitlePermitWorker_DropResources royalTitlePermitWorker_DropResources = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass4_0 CS_0024_003C_003E8__locals18 = new _003C_003Ec__DisplayClass4_0
				{
					pawn = pawn,
					_003C_003E4__this = _003C_003E4__this,
					faction = faction
				};
				if (!royalTitlePermitWorker_DropResources.FillCaravanAidOption(CS_0024_003C_003E8__locals18.pawn, CS_0024_003C_003E8__locals18.faction, out var description, out royalTitlePermitWorker_DropResources.free, out var disableNotEnoughFavor))
				{
					return false;
				}
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = royalTitlePermitWorker_DropResources.def.LabelCap + " (" + CS_0024_003C_003E8__locals18.pawn.LabelShort + ")",
					defaultDesc = description,
					icon = CommandTex,
					action = delegate
					{
						Caravan caravan = CS_0024_003C_003E8__locals18.pawn.GetCaravan();
						float num2 = caravan.MassUsage;
						List<ThingDefCountClass> itemsToDrop = CS_0024_003C_003E8__locals18._003C_003E4__this.def.royalAid.itemsToDrop;
						for (int i = 0; i < itemsToDrop.Count; i++)
						{
							num2 += itemsToDrop[i].thingDef.BaseMass * (float)itemsToDrop[i].count;
						}
						if (num2 > caravan.MassCapacity)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("DropResourcesOverweightConfirm".Translate(), delegate
							{
								CS_0024_003C_003E8__locals18._003C_003E4__this.CallResourcesToCaravan(CS_0024_003C_003E8__locals18.pawn, CS_0024_003C_003E8__locals18.faction, CS_0024_003C_003E8__locals18._003C_003E4__this.free);
							}, destructive: true));
						}
						else
						{
							CS_0024_003C_003E8__locals18._003C_003E4__this.CallResourcesToCaravan(CS_0024_003C_003E8__locals18.pawn, CS_0024_003C_003E8__locals18.faction, CS_0024_003C_003E8__locals18._003C_003E4__this.free);
						}
					}
				};
				if (CS_0024_003C_003E8__locals18.pawn.MapHeld != null && CS_0024_003C_003E8__locals18.pawn.MapHeld.generatorDef.isUnderground)
				{
					command_Action.Disable("CommandCallRoyalAidMapUnreachable".Translate(CS_0024_003C_003E8__locals18.faction.Named("FACTION")));
				}
				if (CS_0024_003C_003E8__locals18.faction.HostileTo(Faction.OfPlayer))
				{
					command_Action.Disable("CommandCallRoyalAidFactionHostile".Translate(CS_0024_003C_003E8__locals18.faction.Named("FACTION")));
				}
				if (disableNotEnoughFavor)
				{
					command_Action.Disable("CommandCallRoyalAidNotEnoughFavor".Translate());
				}
				_003C_003E2__current = command_Action;
				_003C_003E1__state = 1;
				return true;
			}
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetCaravanGizmos_003Ed__4 _003CGetCaravanGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetCaravanGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetCaravanGizmos_003Ed__ = new _003CGetCaravanGizmos_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetCaravanGizmos_003Ed__.pawn = _003C_003E3__pawn;
			_003CGetCaravanGizmos_003Ed__.faction = _003C_003E3__faction;
			return _003CGetCaravanGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetRoyalAidOptions_003Ed__3 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitlePermitWorker_DropResources _003C_003E4__this;

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
		public _003CGetRoyalAidOptions_003Ed__3(int _003C_003E1__state)
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
			RoyalTitlePermitWorker_DropResources royalTitlePermitWorker_DropResources = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals14 = new _003C_003Ec__DisplayClass3_0
				{
					_003C_003E4__this = _003C_003E4__this,
					pawn = pawn,
					faction = faction,
					map = map
				};
				if (CS_0024_003C_003E8__locals14.map.generatorDef.isUnderground)
				{
					_003C_003E2__current = new FloatMenuOption(royalTitlePermitWorker_DropResources.def.LabelCap + ": " + "CommandCallRoyalAidMapUnreachable".Translate(CS_0024_003C_003E8__locals14.faction.Named("FACTION")), null);
					_003C_003E1__state = 1;
					return true;
				}
				if (CS_0024_003C_003E8__locals14.faction.HostileTo(Faction.OfPlayer))
				{
					_003C_003E2__current = new FloatMenuOption("CommandCallRoyalAidFactionHostile".Translate(CS_0024_003C_003E8__locals14.faction.Named("FACTION")), null);
					_003C_003E1__state = 2;
					return true;
				}
				Action action = null;
				string description = royalTitlePermitWorker_DropResources.def.LabelCap + ": ";
				if (royalTitlePermitWorker_DropResources.FillAidOption(CS_0024_003C_003E8__locals14.pawn, CS_0024_003C_003E8__locals14.faction, ref description, out CS_0024_003C_003E8__locals14.free))
				{
					action = delegate
					{
						CS_0024_003C_003E8__locals14._003C_003E4__this.BeginCallResources(CS_0024_003C_003E8__locals14.pawn, CS_0024_003C_003E8__locals14.faction, CS_0024_003C_003E8__locals14.map, CS_0024_003C_003E8__locals14.free);
					};
				}
				_003C_003E2__current = new FloatMenuOption(description, action, CS_0024_003C_003E8__locals14.faction.def.FactionIcon, CS_0024_003C_003E8__locals14.faction.Color);
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
			_003CGetRoyalAidOptions_003Ed__3 _003CGetRoyalAidOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRoyalAidOptions_003Ed__ = this;
			}
			else
			{
				_003CGetRoyalAidOptions_003Ed__ = new _003CGetRoyalAidOptions_003Ed__3(0)
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

	private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallAid");

	public override void OrderForceTarget(LocalTargetInfo target)
	{
		CallResources(target.Cell);
	}

	[IteratorStateMachine(typeof(_003CGetRoyalAidOptions_003Ed__3))]
	public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRoyalAidOptions_003Ed__3(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__map = map,
			_003C_003E3__pawn = pawn,
			_003C_003E3__faction = faction
		};
	}

	[IteratorStateMachine(typeof(_003CGetCaravanGizmos_003Ed__4))]
	public override IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCaravanGizmos_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn,
			_003C_003E3__faction = faction
		};
	}

	private void BeginCallResources(Pawn caller, Faction faction, Map map, bool free)
	{
		targetingParameters = new TargetingParameters();
		targetingParameters.canTargetLocations = true;
		targetingParameters.canTargetBuildings = false;
		targetingParameters.canTargetPawns = false;
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
			return DropCellFinder.CanPhysicallyDropInto(target.Cell, map, canRoofPunch: true) ? true : false;
		};
		Find.Targeter.BeginTargeting(this);
	}

	private void CallResources(IntVec3 cell)
	{
		List<Thing> list = new List<Thing>();
		for (int i = 0; i < def.royalAid.itemsToDrop.Count; i++)
		{
			Thing thing = ThingMaker.MakeThing(def.royalAid.itemsToDrop[i].thingDef);
			thing.stackCount = def.royalAid.itemsToDrop[i].count;
			list.Add(thing);
		}
		if (list.Any())
		{
			ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
			activeTransporterInfo.innerContainer.TryAddRangeOrTransfer(list);
			DropPodUtility.MakeDropPodAt(cell, map, activeTransporterInfo);
			Messages.Message("MessagePermitTransportDrop".Translate(faction.Named("FACTION")), new LookTargets(cell, map), MessageTypeDefOf.NeutralEvent);
			caller.royalty.GetPermit(def, faction).Notify_Used();
			if (!free)
			{
				caller.royalty.TryRemoveFavor(faction, def.royalAid.favorCost);
			}
		}
	}

	private void CallResourcesToCaravan(Pawn caller, Faction faction, bool free)
	{
		Caravan caravan = caller.GetCaravan();
		for (int i = 0; i < def.royalAid.itemsToDrop.Count; i++)
		{
			Thing thing = ThingMaker.MakeThing(def.royalAid.itemsToDrop[i].thingDef);
			thing.stackCount = def.royalAid.itemsToDrop[i].count;
			CaravanInventoryUtility.GiveThing(caravan, thing);
		}
		Messages.Message("MessagePermitTransportDropCaravan".Translate(faction.Named("FACTION"), caller.Named("PAWN")), caravan, MessageTypeDefOf.NeutralEvent);
		caller.royalty.GetPermit(def, faction).Notify_Used();
		if (!free)
		{
			caller.royalty.TryRemoveFavor(faction, def.royalAid.favorCost);
		}
	}
}
