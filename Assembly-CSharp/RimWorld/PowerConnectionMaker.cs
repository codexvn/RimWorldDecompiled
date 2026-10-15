using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class PowerConnectionMaker
{
	[CompilerGenerated]
	private sealed class _003CPotentialConnectorsForTransmitter_003Ed__5 : IEnumerable<CompPower>, IEnumerable, IEnumerator<CompPower>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private CompPower _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private CompPower b;

		public CompPower _003C_003E3__b;

		private CellRect _003Crect_003E5__2;

		private int _003Cz_003E5__3;

		private int _003Cx_003E5__4;

		private List<Thing> _003CthingList_003E5__5;

		private int _003Cj_003E5__6;

		CompPower IEnumerator<CompPower>.Current
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
		public _003CPotentialConnectorsForTransmitter_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CthingList_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0143;
			}
			_003C_003E1__state = -1;
			if (!b.parent.Spawned)
			{
				Log.Warning("Can't check potential connectors for " + b?.ToString() + " because it's unspawned.");
				return false;
			}
			_003Crect_003E5__2 = b.parent.OccupiedRect().ExpandedBy(6).ClipInsideMap(b.parent.Map);
			_003Cz_003E5__3 = _003Crect_003E5__2.minZ;
			goto IL_01a3;
			IL_017d:
			if (_003Cx_003E5__4 <= _003Crect_003E5__2.maxX)
			{
				IntVec3 c = new IntVec3(_003Cx_003E5__4, 0, _003Cz_003E5__3);
				_003CthingList_003E5__5 = b.parent.Map.thingGrid.ThingsListAt(c);
				_003Cj_003E5__6 = 0;
				goto IL_0153;
			}
			_003Cz_003E5__3++;
			goto IL_01a3;
			IL_0153:
			if (_003Cj_003E5__6 < _003CthingList_003E5__5.Count)
			{
				if (_003CthingList_003E5__5[_003Cj_003E5__6].def.ConnectToPower)
				{
					_003C_003E2__current = ((Building)_003CthingList_003E5__5[_003Cj_003E5__6]).PowerComp;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0143;
			}
			_003CthingList_003E5__5 = null;
			_003Cx_003E5__4++;
			goto IL_017d;
			IL_0143:
			_003Cj_003E5__6++;
			goto IL_0153;
			IL_01a3:
			if (_003Cz_003E5__3 <= _003Crect_003E5__2.maxZ)
			{
				_003Cx_003E5__4 = _003Crect_003E5__2.minX;
				goto IL_017d;
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
		IEnumerator<CompPower> IEnumerable<CompPower>.GetEnumerator()
		{
			_003CPotentialConnectorsForTransmitter_003Ed__5 _003CPotentialConnectorsForTransmitter_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialConnectorsForTransmitter_003Ed__ = this;
			}
			else
			{
				_003CPotentialConnectorsForTransmitter_003Ed__ = new _003CPotentialConnectorsForTransmitter_003Ed__5(0);
			}
			_003CPotentialConnectorsForTransmitter_003Ed__.b = _003C_003E3__b;
			return _003CPotentialConnectorsForTransmitter_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CompPower>)this).GetEnumerator();
		}
	}

	private const int ConnectMaxDist = 6;

	public static void ConnectAllConnectorsToTransmitter(CompPower newTransmitter)
	{
		foreach (CompPower item in PotentialConnectorsForTransmitter(newTransmitter))
		{
			if (item.connectParent == null)
			{
				item.ConnectToTransmitter(newTransmitter);
			}
		}
	}

	public static void DisconnectAllFromTransmitterAndSetWantConnect(CompPower deadPc, Map map)
	{
		if (deadPc.connectChildren == null)
		{
			return;
		}
		for (int i = 0; i < deadPc.connectChildren.Count; i++)
		{
			CompPower compPower = deadPc.connectChildren[i];
			compPower.connectParent = null;
			if (compPower is CompPowerTrader compPowerTrader)
			{
				compPowerTrader.PowerOn = false;
			}
			map.powerNetManager.Notify_ConnectorWantsConnect(compPower);
		}
	}

	public static void TryConnectToAnyPowerNet(CompPower pc, List<PowerNet> disallowedNets = null)
	{
		if (pc.connectParent == null && pc.parent.Spawned)
		{
			CompPower compPower = BestTransmitterForConnector(pc.parent.def.building.isAttachment ? GenConstruct.GetWallAttachedTo(pc.parent).Position : pc.parent.Position, pc.parent.Map, disallowedNets);
			if (compPower != null)
			{
				pc.ConnectToTransmitter(compPower);
			}
			else
			{
				pc.connectParent = null;
			}
		}
	}

	public static void DisconnectFromPowerNet(CompPower pc)
	{
		if (pc.connectParent == null)
		{
			return;
		}
		if (pc.PowerNet != null)
		{
			pc.PowerNet.DeregisterConnector(pc);
		}
		if (pc.connectParent.connectChildren != null)
		{
			pc.connectParent.connectChildren.Remove(pc);
			if (pc.connectParent.connectChildren.Count == 0)
			{
				pc.connectParent.connectChildren = null;
			}
		}
		pc.connectParent = null;
	}

	[IteratorStateMachine(typeof(_003CPotentialConnectorsForTransmitter_003Ed__5))]
	private static IEnumerable<CompPower> PotentialConnectorsForTransmitter(CompPower b)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialConnectorsForTransmitter_003Ed__5(-2)
		{
			_003C_003E3__b = b
		};
	}

	public static CompPower BestTransmitterForConnector(IntVec3 connectorPos, Map map, List<PowerNet> disallowedNets = null)
	{
		CellRect cellRect = CellRect.SingleCell(connectorPos).ExpandedBy(6).ClipInsideMap(map);
		cellRect.ClipInsideMap(map);
		float num = 999999f;
		CompPower result = null;
		for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
		{
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				Building transmitter = new IntVec3(j, 0, i).GetTransmitter(map);
				if (transmitter == null || transmitter.Destroyed)
				{
					continue;
				}
				CompPower powerComp = transmitter.PowerComp;
				if (powerComp != null && powerComp.TransmitsPowerNow && (transmitter.def.building == null || transmitter.def.building.allowWireConnection) && (disallowedNets == null || !disallowedNets.Contains(powerComp.transNet)))
				{
					float num2 = (transmitter.Position - connectorPos).LengthHorizontalSquared;
					if (num2 < num)
					{
						num = num2;
						result = powerComp;
					}
				}
			}
		}
		return result;
	}
}
