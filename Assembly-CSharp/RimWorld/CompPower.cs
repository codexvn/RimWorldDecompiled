using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public abstract class CompPower : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__21 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompPower _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__21(int _003C_003E1__state)
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
				CompPower CS_0024_003C_003E8__locals4 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals4).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals4.connectParent != null && CS_0024_003C_003E8__locals4.parent.Faction == Faction.OfPlayer)
					{
						_003C_003E2__current = new Command_Action
						{
							action = delegate
							{
								SoundDefOf.Tick_Tiny.PlayOneShotOnCamera();
								CS_0024_003C_003E8__locals4.TryManualReconnect();
							},
							hotKey = KeyBindingDefOf.Misc2,
							defaultDesc = "CommandTryReconnectDesc".Translate(),
							icon = ContentFinder<Texture2D>.Get("UI/Commands/TryReconnect"),
							defaultLabel = "CommandTryReconnectLabel".Translate()
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
			_003CCompGetGizmosExtra_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__21(0)
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

	public PowerNet transNet;

	public CompPower connectParent;

	public List<CompPower> connectChildren;

	private static List<PowerNet> recentlyConnectedNets = new List<PowerNet>();

	private static CompPower lastManualReconnector = null;

	public static readonly float WattsToWattDaysPerTick = 1.6666667E-05f;

	public bool TransmitsPowerNow => ((Building)parent).TransmitsPowerNow;

	public PowerNet PowerNet
	{
		get
		{
			if (transNet != null)
			{
				return transNet;
			}
			if (connectParent != null)
			{
				return connectParent.transNet;
			}
			return null;
		}
	}

	public CompProperties_Power Props => (CompProperties_Power)props;

	public virtual void ResetPowerVars()
	{
		transNet = null;
		connectParent = null;
		connectChildren = null;
		recentlyConnectedNets.Clear();
		lastManualReconnector = null;
	}

	public virtual void SetUpPowerVars()
	{
	}

	public override void PostExposeData()
	{
		Thing refee = null;
		if (Scribe.mode == LoadSaveMode.Saving && connectParent != null)
		{
			refee = connectParent.parent;
		}
		Scribe_References.Look(ref refee, "parentThing");
		if (refee != null)
		{
			connectParent = ((ThingWithComps)refee).GetComp<CompPower>();
		}
		if (Scribe.mode == LoadSaveMode.PostLoadInit && connectParent != null)
		{
			ConnectToTransmitter(connectParent, reconnectingAfterLoading: true);
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		if (Props.transmitsPower || parent.def.ConnectToPower)
		{
			parent.Map.mapDrawer.MapMeshDirty(parent.Position, MapMeshFlagDefOf.PowerGrid, regenAdjacentCells: true, regenAdjacentSections: false);
			if (Props.transmitsPower)
			{
				parent.Map.powerNetManager.Notify_TransmitterSpawned(this);
			}
			if (parent.def.ConnectToPower)
			{
				parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
			}
			SetUpPowerVars();
		}
	}

	public override void PostSwapMap()
	{
		if (connectParent != null && !connectParent.parent.Spawned)
		{
			LostConnectParent();
		}
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		if (!Props.transmitsPower && !parent.def.ConnectToPower)
		{
			return;
		}
		if (mode != DestroyMode.WillReplace || parent.BeingTransportedOnGravship)
		{
			if (Props.transmitsPower)
			{
				if (connectChildren != null)
				{
					for (int i = 0; i < connectChildren.Count; i++)
					{
						connectChildren[i].LostConnectParent();
					}
				}
				map.powerNetManager.Notify_TransmitterDespawned(this);
			}
			if (parent.def.ConnectToPower)
			{
				map.powerNetManager.Notify_ConnectorDespawned(this);
			}
		}
		map.mapDrawer.MapMeshDirty(parent.Position, MapMeshFlagDefOf.PowerGrid, regenAdjacentCells: true, regenAdjacentSections: false);
	}

	public virtual void LostConnectParent()
	{
		connectParent = null;
		if (parent.Spawned)
		{
			parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
		}
	}

	public override void PostPrintOnto(SectionLayer layer)
	{
		base.PostPrintOnto(layer);
		if (connectParent != null && connectParent.parent.def != ThingDefOf.HiddenConduit)
		{
			PowerNetGraphics.PrintWirePieceConnecting(layer, parent, connectParent.parent, forPowerOverlay: false);
		}
	}

	public override void CompPrintForPowerGrid(SectionLayer layer)
	{
		if (TransmitsPowerNow)
		{
			PowerOverlayMats.LinkedOverlayGraphic.Print(layer, parent, 0f);
		}
		Thing thing = parent;
		if (parent is Building building && building.def.building.isAttachment)
		{
			thing = GenConstruct.GetWallAttachedTo(parent) ?? parent;
		}
		if (parent.def.ConnectToPower)
		{
			PowerNetGraphics.PrintOverlayConnectorBaseFor(layer, thing);
		}
		if (connectParent != null)
		{
			PowerNetGraphics.PrintWirePieceConnecting(layer, thing, connectParent.parent, forPowerOverlay: true);
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__21))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__21(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void TryManualReconnect(bool dispayVisuals = true)
	{
		if (lastManualReconnector != this)
		{
			recentlyConnectedNets.Clear();
			lastManualReconnector = this;
		}
		if (PowerNet != null)
		{
			recentlyConnectedNets.Add(PowerNet);
		}
		IntVec3 connectorPos = (parent.def.building.isAttachment ? GenConstruct.GetWallAttachedTo(parent).Position : parent.Position);
		CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(connectorPos, parent.Map, recentlyConnectedNets);
		if (compPower == null)
		{
			recentlyConnectedNets.Clear();
			compPower = PowerConnectionMaker.BestTransmitterForConnector(connectorPos, parent.Map);
		}
		if (compPower == null)
		{
			return;
		}
		PowerConnectionMaker.DisconnectFromPowerNet(this);
		ConnectToTransmitter(compPower);
		if (dispayVisuals)
		{
			for (int i = 0; i < 5; i++)
			{
				FleckMaker.ThrowMetaPuff(compPower.parent.Position.ToVector3Shifted(), compPower.parent.Map);
			}
		}
		parent.Map.mapDrawer.MapMeshDirty(parent.Position, MapMeshFlagDefOf.PowerGrid);
		parent.Map.mapDrawer.MapMeshDirty(parent.Position, MapMeshFlagDefOf.Things);
	}

	public void ConnectToTransmitter(CompPower transmitter, bool reconnectingAfterLoading = false)
	{
		if (connectParent != null && (!reconnectingAfterLoading || connectParent != transmitter))
		{
			Log.Error("Tried to connect " + this?.ToString() + " to transmitter " + transmitter?.ToString() + " but it's already connected to " + connectParent?.ToString() + ".");
		}
		else
		{
			connectParent = transmitter;
			if (connectParent.connectChildren == null)
			{
				connectParent.connectChildren = new List<CompPower>();
			}
			transmitter.connectChildren.Add(this);
			PowerNet?.RegisterConnector(this);
		}
	}

	public override string CompInspectStringExtra()
	{
		if (PowerNet == null)
		{
			return "PowerNotConnected".Translate();
		}
		string text = (PowerNet.CurrentEnergyGainRate() / WattsToWattDaysPerTick).ToString("F0");
		string text2 = PowerNet.CurrentStoredEnergy().ToString("F0");
		return "PowerConnectedRateStored".Translate(text, text2);
	}
}
