using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public abstract class WorldDrawLayerBase
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__31 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayerBase _003C_003E4__this;

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
		public _003CRegenerate_003Ed__31(int _003C_003E1__state)
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
			WorldDrawLayerBase worldDrawLayerBase = _003C_003E4__this;
			if (num != 0)
			{
				return false;
			}
			_003C_003E1__state = -1;
			worldDrawLayerBase.dirty = false;
			worldDrawLayerBase.ClearSubMeshes(MeshParts.All);
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
			_003CRegenerate_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__31(0)
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

	protected bool dirty = true;

	private MaterialPropertyBlock propertyBlock;

	protected readonly List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

	private const int MaxVerticesPerMesh = 40000;

	public virtual bool ShouldRegenerate
	{
		get
		{
			if (dirty)
			{
				return Visible;
			}
			return false;
		}
	}

	protected virtual int RenderLayer => WorldCameraManager.WorldLayer;

	protected virtual Quaternion Rotation => Quaternion.identity;

	protected virtual float Alpha => 1f;

	public bool Dirty => dirty;

	public virtual bool Visible
	{
		get
		{
			if (WorldRendererUtility.WorldBackgroundNow)
			{
				return VisibleInBackground;
			}
			return true;
		}
	}

	public virtual bool VisibleWhenLayerNotSelected => true;

	public virtual bool VisibleInBackground => true;

	public virtual Vector3 Position
	{
		get
		{
			if (CameraIsOrigin)
			{
				return Find.WorldCameraDriver.CameraPosition;
			}
			return Vector3.zero;
		}
	}

	public virtual bool CameraIsOrigin => false;

	public MaterialPropertyBlock MatPropBlock => propertyBlock ?? (propertyBlock = new MaterialPropertyBlock());

	protected LayerSubMesh GetSubMesh(Material material)
	{
		int subMeshIndex;
		return GetSubMesh(material, out subMeshIndex);
	}

	protected LayerSubMesh GetSubMesh(Material material, out int subMeshIndex)
	{
		for (int i = 0; i < subMeshes.Count; i++)
		{
			LayerSubMesh layerSubMesh = subMeshes[i];
			if (layerSubMesh.material == material && layerSubMesh.verts.Count < 40000)
			{
				subMeshIndex = i;
				return layerSubMesh;
			}
		}
		Mesh mesh = new Mesh();
		if (UnityData.isEditor)
		{
			mesh.name = "WorldLayerSubMesh_" + GetType().Name + "_" + Find.World.info.seedString;
		}
		LayerSubMesh layerSubMesh2 = new LayerSubMesh(mesh, material, null);
		subMeshIndex = subMeshes.Count;
		subMeshes.Add(layerSubMesh2);
		return layerSubMesh2;
	}

	protected void FinalizeMesh(MeshParts tags)
	{
		for (int i = 0; i < subMeshes.Count; i++)
		{
			if (subMeshes[i].verts.Count > 0)
			{
				subMeshes[i].FinalizeMesh(tags);
			}
		}
	}

	public void RegenerateNow()
	{
		dirty = false;
		Regenerate().ExecuteEnumerable();
	}

	public virtual void Render()
	{
		if (!Visible)
		{
			return;
		}
		if (ShouldRegenerate)
		{
			RegenerateNow();
		}
		int renderLayer = RenderLayer;
		Vector3 position = Position;
		Quaternion rotation = Rotation;
		float alpha = Alpha;
		for (int i = 0; i < subMeshes.Count; i++)
		{
			if (subMeshes[i].finalized)
			{
				if (!Mathf.Approximately(alpha, 1f))
				{
					Color color = subMeshes[i].material.color;
					MatPropBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * alpha));
					Graphics.DrawMesh(subMeshes[i].mesh, position, rotation, subMeshes[i].material, renderLayer, null, 0, MatPropBlock);
				}
				else
				{
					Graphics.DrawMesh(subMeshes[i].mesh, position, rotation, subMeshes[i].material, renderLayer);
				}
			}
		}
	}

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__31))]
	public virtual IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__31(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void SetDirty()
	{
		dirty = true;
	}

	private void ClearSubMeshes(MeshParts parts)
	{
		for (int i = 0; i < subMeshes.Count; i++)
		{
			subMeshes[i].Clear(parts);
		}
	}

	public void Dispose()
	{
		foreach (LayerSubMesh subMesh in subMeshes)
		{
			UnityEngine.Object.Destroy(subMesh.mesh);
		}
		subMeshes.Clear();
	}
}
