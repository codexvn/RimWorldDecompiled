using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class TravellingTransporters : WorldObject, IThingHolder
{
	[CompilerGenerated]
	private sealed class _003Cget_Pawns_003Ed__30 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TravellingTransporters _003C_003E4__this;

		private int _003Ci_003E5__2;

		private ThingOwner _003Cthings_003E5__3;

		private int _003Cj_003E5__4;

		Pawn IEnumerator<Pawn>.Current
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
		public _003Cget_Pawns_003Ed__30(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cthings_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			TravellingTransporters travellingTransporters = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0080;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_00ba;
			IL_0090:
			if (_003Cj_003E5__4 < _003Cthings_003E5__3.Count)
			{
				if (_003Cthings_003E5__3[_003Cj_003E5__4] is Pawn pawn)
				{
					_003C_003E2__current = pawn;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0080;
			}
			_003Cthings_003E5__3 = null;
			_003Ci_003E5__2++;
			goto IL_00ba;
			IL_0080:
			_003Cj_003E5__4++;
			goto IL_0090;
			IL_00ba:
			if (_003Ci_003E5__2 < travellingTransporters.transporters.Count)
			{
				_003Cthings_003E5__3 = travellingTransporters.transporters[_003Ci_003E5__2].innerContainer;
				_003Cj_003E5__4 = 0;
				goto IL_0090;
			}
			return false;
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
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003Cget_Pawns_003Ed__30 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Pawns_003Ed__30(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	public PlanetTile destinationTile = PlanetTile.Invalid;

	public TransportersArrivalAction arrivalAction;

	private List<ActiveTransporterInfo> transporters = new List<ActiveTransporterInfo>();

	private bool arrived;

	private PlanetTile initialTile = PlanetTile.Invalid;

	private float traveledPct;

	private const float TravelSpeed = 0.00025f;

	public bool IsPlayerControlled => base.Faction == Faction.OfPlayer;

	private Vector3 Start => Find.WorldGrid.GetTileCenter(initialTile);

	private Vector3 End => Find.WorldGrid.GetTileCenter(destinationTile);

	public override Vector3 DrawPos => Vector3.Slerp(Start, End, traveledPct);

	public override bool ExpandingIconFlipHorizontal => GenWorldUI.WorldToUIPosition(Start).x > GenWorldUI.WorldToUIPosition(End).x;

	private Thing Shuttle => transporters.FirstOrDefault()?.GetShuttle();

	public override Color ExpandingIconColor => base.Faction.Color;

	public override float ExpandingIconRotation
	{
		get
		{
			if (!def.rotateGraphicWhenTraveling)
			{
				return base.ExpandingIconRotation;
			}
			Vector2 vector = GenWorldUI.WorldToUIPosition(Start);
			Vector2 vector2 = GenWorldUI.WorldToUIPosition(End);
			float num = Mathf.Atan2(vector2.y - vector.y, vector2.x - vector.x) * 57.29578f;
			if (num > 180f)
			{
				num -= 180f;
			}
			return num + 90f;
		}
	}

	private float TraveledPctStepPerTick
	{
		get
		{
			Vector3 start = Start;
			Vector3 end = End;
			if (start == end)
			{
				return 1f;
			}
			float num = GenMath.SphericalDistance(start.normalized, end.normalized);
			if (num == 0f)
			{
				return 1f;
			}
			return 0.00025f / num;
		}
	}

	private bool PodsHaveAnyPotentialCaravanOwner
	{
		get
		{
			for (int i = 0; i < transporters.Count; i++)
			{
				ThingOwner innerContainer = transporters[i].innerContainer;
				for (int j = 0; j < innerContainer.Count; j++)
				{
					if (innerContainer[j] is Pawn pawn && CaravanUtility.IsOwner(pawn, base.Faction))
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public bool PodsHaveAnyFreeColonist
	{
		get
		{
			for (int i = 0; i < transporters.Count; i++)
			{
				ThingOwner innerContainer = transporters[i].innerContainer;
				for (int j = 0; j < innerContainer.Count; j++)
				{
					if (innerContainer[j] is Pawn { IsColonist: not false, HostFaction: null })
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public IEnumerable<Pawn> Pawns
	{
		[IteratorStateMachine(typeof(_003Cget_Pawns_003Ed__30))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Pawns_003Ed__30(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref transporters, "pods", LookMode.Deep);
		Scribe_Values.Look(ref destinationTile, "destinationTile");
		Scribe_Values.Look(ref arrived, "arrived", defaultValue: false);
		Scribe_Values.Look(ref initialTile, "initialTile");
		Scribe_Values.Look(ref traveledPct, "traveledPct", 0f);
		Scribe_Deep.Look(ref arrivalAction, "arrivalAction");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			for (int i = 0; i < transporters.Count; i++)
			{
				transporters[i].parent = this;
			}
		}
	}

	public override void PostAdd()
	{
		base.PostAdd();
		initialTile = base.Tile;
	}

	protected override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		traveledPct += TraveledPctStepPerTick * (float)delta;
		if (traveledPct >= 1f)
		{
			traveledPct = 1f;
			Arrived();
		}
	}

	public void AddTransporter(ActiveTransporterInfo contents, bool justLeftTheMap)
	{
		contents.parent = this;
		transporters.Add(contents);
		ThingOwner innerContainer = contents.innerContainer;
		for (int i = 0; i < innerContainer.Count; i++)
		{
			if (innerContainer[i] is Pawn pawn && !pawn.IsWorldPawn())
			{
				if (!base.Spawned)
				{
					Log.Warning($"Passing pawn {pawn} to world, but the TravelingTransportPod is not spawned. This means that WorldPawns can discard this pawn which can cause bugs.");
				}
				if (justLeftTheMap)
				{
					pawn.ExitMap(allowedToJoinOrCreateCaravan: false, Rot4.Invalid);
				}
				else
				{
					Find.WorldPawns.PassToWorld(pawn);
				}
			}
		}
		contents.savePawnsWithReferenceMode = true;
	}

	public bool ContainsPawn(Pawn p)
	{
		for (int i = 0; i < transporters.Count; i++)
		{
			if (transporters[i].innerContainer.Contains(p))
			{
				return true;
			}
		}
		return false;
	}

	private void Arrived()
	{
		if (arrived)
		{
			return;
		}
		arrived = true;
		if (arrivalAction == null || !arrivalAction.StillValid(transporters, destinationTile))
		{
			arrivalAction = null;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].Tile == destinationTile)
				{
					arrivalAction = new TransportersArrivalAction_LandInSpecificCell(maps[i].Parent, DropCellFinder.RandomDropSpot(maps[i]));
					break;
				}
			}
			if (arrivalAction == null)
			{
				if (TransportersArrivalAction_FormCaravan.CanFormCaravanAt(transporters, destinationTile))
				{
					arrivalAction = new TransportersArrivalAction_FormCaravan();
				}
				else
				{
					List<Caravan> caravans = Find.WorldObjects.Caravans;
					for (int j = 0; j < caravans.Count; j++)
					{
						if (caravans[j].Tile == destinationTile && (bool)TransportersArrivalAction_GiveToCaravan.CanGiveTo(transporters, caravans[j]))
						{
							arrivalAction = new TransportersArrivalAction_GiveToCaravan(caravans[j]);
							break;
						}
					}
				}
			}
		}
		if (arrivalAction != null && arrivalAction.ShouldUseLongEvent(transporters, destinationTile))
		{
			LongEventHandler.QueueLongEvent(DoArrivalAction, "GeneratingMapForNewEncounter", doAsynchronously: false, null);
		}
		else
		{
			DoArrivalAction();
		}
	}

	private void DoArrivalAction()
	{
		for (int i = 0; i < transporters.Count; i++)
		{
			transporters[i].savePawnsWithReferenceMode = false;
			transporters[i].parent = null;
		}
		if (arrivalAction != null)
		{
			try
			{
				arrivalAction.Arrived(transporters, destinationTile);
			}
			catch (Exception ex)
			{
				Log.Error("Exception in transport pods arrival action: " + ex);
			}
			arrivalAction = null;
		}
		else
		{
			for (int j = 0; j < transporters.Count; j++)
			{
				for (int k = 0; k < transporters[j].innerContainer.Count; k++)
				{
					if (transporters[j].innerContainer[k] is Pawn pawn && (pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer))
					{
						PawnBanishUtility.Banish(pawn, destinationTile);
					}
				}
			}
			bool flag = true;
			if (ModsConfig.BiotechActive)
			{
				flag = false;
				for (int l = 0; l < transporters.Count; l++)
				{
					if (flag)
					{
						break;
					}
					for (int m = 0; m < transporters[l].innerContainer.Count; m++)
					{
						if (transporters[l].innerContainer[m].def != ThingDefOf.Wastepack)
						{
							flag = true;
							break;
						}
					}
				}
			}
			for (int n = 0; n < transporters.Count; n++)
			{
				for (int num = 0; num < transporters[n].innerContainer.Count; num++)
				{
					transporters[n].innerContainer[num].Notify_AbandonedAtTile(destinationTile);
				}
			}
			for (int num2 = 0; num2 < transporters.Count; num2++)
			{
				transporters[num2].innerContainer.ClearAndDestroyContentsOrPassToWorld();
			}
			if (flag)
			{
				string key = "MessageTransportPodsArrivedAndLost";
				if (def == WorldObjectDefOf.TravelingShuttle)
				{
					key = "MessageShuttleArrivedContentsLost";
				}
				Messages.Message(key.Translate(), new GlobalTargetInfo(destinationTile), MessageTypeDefOf.NegativeEvent);
			}
		}
		transporters.Clear();
		Destroy();
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return null;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
		for (int i = 0; i < transporters.Count; i++)
		{
			outChildren.Add(transporters[i]);
		}
	}
}
