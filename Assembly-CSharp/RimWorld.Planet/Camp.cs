using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class Camp : MapParent
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__6 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Camp _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__6(int _003C_003E1__state)
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
				Camp camp = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)camp).GetGizmos().GetEnumerator();
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
					if (Find.WorldSelector.SingleSelectedObject == camp)
					{
						Command command = SettleInExistingMapUtility.SettleCommand(camp.Map, requiresNoEnemies: true);
						if (!TileFinder.IsValidTileForNewSettlement(camp.Tile, sb))
						{
							command.Disable(sb.ToString());
						}
						_003C_003E2__current = command;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				sb.Clear();
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
			_003CGetGizmos_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__6(0)
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

	private Material cachedMat;

	private static readonly StringBuilder sb = new StringBuilder();

	public override bool CanReformFoggedEnemies => true;

	public override Material Material
	{
		get
		{
			if (cachedMat != null)
			{
				return cachedMat;
			}
			Color color = base.Faction?.Color ?? Color.white;
			cachedMat = MaterialPool.MatFrom(def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, 3550);
			return cachedMat;
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__6))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
	{
		alsoRemoveWorldObject = false;
		if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
		{
			return false;
		}
		if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
		{
			return false;
		}
		alsoRemoveWorldObject = true;
		return true;
	}

	public override void Notify_MyMapRemoved(Map map)
	{
		base.Notify_MyMapRemoved(map);
		if (ModsConfig.OdysseyActive && map.TileInfo.Landmark != null)
		{
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedLandmark);
			worldObject.Tile = base.Tile;
			worldObject.SetFaction(base.Faction);
			Find.WorldObjects.Add(worldObject);
		}
		else
		{
			WorldObject worldObject2 = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedCamp);
			worldObject2.Tile = base.Tile;
			worldObject2.SetFaction(base.Faction);
			worldObject2.GetComponent<TimeoutComp>().StartTimeout(1800000);
			Find.WorldObjects.Add(worldObject2);
		}
	}
}
