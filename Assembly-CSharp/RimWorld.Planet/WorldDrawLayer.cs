using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public abstract class WorldDrawLayer : WorldDrawLayerBase
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__10 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer _003C_003E4__this;

		private IEnumerator _003C_003E7__wrap1;

		object IEnumerator<object>.Current
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
		public _003CRegenerate_003Ed__10(int _003C_003E1__state)
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
				WorldDrawLayer worldDrawLayer = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					worldDrawLayer.triangleIndexToTileID?.Clear();
					_003C_003E7__wrap1 = ((WorldDrawLayerBase)worldDrawLayer).Regenerate().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					object current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
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
			if (_003C_003E7__wrap1 is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			_003CRegenerate_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRegenerateWorldMeshColliders_003Ed__13 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer _003C_003E4__this;

		private GameObject _003CgameObject_003E5__2;

		private int _003Ci_003E5__3;

		object IEnumerator<object>.Current
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
		public _003CRegenerateWorldMeshColliders_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CgameObject_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			WorldDrawLayer worldDrawLayer = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (worldDrawLayer.meshCollidersInOrder == null)
				{
					worldDrawLayer.meshCollidersInOrder = new List<MeshCollider>();
				}
				else
				{
					worldDrawLayer.meshCollidersInOrder.Clear();
				}
				_003CgameObject_003E5__2 = WorldTerrainColliderManager.Get(worldDrawLayer.planetLayer, WorldCameraManager.WorldLayer);
				_003CgameObject_003E5__2.transform.position = worldDrawLayer.Position;
				MeshCollider[] components = _003CgameObject_003E5__2.GetComponents<MeshCollider>();
				for (int i = 0; i < components.Length; i++)
				{
					UnityEngine.Object.Destroy(components[i]);
				}
				_003Ci_003E5__3 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < worldDrawLayer.subMeshes.Count)
			{
				MeshCollider meshCollider = _003CgameObject_003E5__2.AddComponent<MeshCollider>();
				meshCollider.sharedMesh = worldDrawLayer.subMeshes[_003Ci_003E5__3].mesh;
				worldDrawLayer.meshCollidersInOrder.Add(meshCollider);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			_003CRegenerateWorldMeshColliders_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerateWorldMeshColliders_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}
	}

	public PlanetLayer planetLayer;

	private List<MeshCollider> meshCollidersInOrder;

	private List<List<int>> triangleIndexToTileID;

	public override bool Visible
	{
		get
		{
			if (base.Visible)
			{
				if (PlanetLayer.Selected != planetLayer)
				{
					return VisibleWhenLayerNotSelected;
				}
				return true;
			}
			return false;
		}
	}

	public override Vector3 Position
	{
		get
		{
			if (CameraIsOrigin)
			{
				return Find.WorldCameraDriver.CameraPosition;
			}
			return planetLayer.Origin;
		}
	}

	public bool Raycastable
	{
		get
		{
			if (!meshCollidersInOrder.NullOrEmpty())
			{
				return PlanetLayer.Selected == planetLayer;
			}
			return false;
		}
	}

	public void Initialize(PlanetLayer layer)
	{
		planetLayer = layer;
	}

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__10))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	protected void AppendRaycastableTriangle(int subMeshIndex, int tileIndex)
	{
		if (triangleIndexToTileID == null)
		{
			triangleIndexToTileID = new List<List<int>>();
		}
		while (triangleIndexToTileID.Count < subMeshes.Count)
		{
			triangleIndexToTileID.Add(new List<int>());
		}
		triangleIndexToTileID[subMeshIndex].Add(tileIndex);
	}

	public bool TryGetTileFromRayHit(RaycastHit hit, out PlanetTile id)
	{
		id = PlanetTile.Invalid;
		for (int i = 0; i < meshCollidersInOrder.Count; i++)
		{
			if (meshCollidersInOrder[i] == hit.collider)
			{
				id = new PlanetTile(triangleIndexToTileID[i][hit.triangleIndex], planetLayer);
				return true;
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CRegenerateWorldMeshColliders_003Ed__13))]
	protected IEnumerable RegenerateWorldMeshColliders()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerateWorldMeshColliders_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}
}
