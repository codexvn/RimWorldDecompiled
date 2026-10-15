using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class WorldDrawLayer_RaycastableGrid : WorldDrawLayer
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__5 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_RaycastableGrid _003C_003E4__this;

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
		public _003CRegenerate_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldDrawLayer_RaycastableGrid worldDrawLayer_RaycastableGrid = _003C_003E4__this;
				NativeArray<int> unsafeTileIDToVerts_offsets;
				NativeArray<Vector3> unsafeVerts;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (worldDrawLayer_RaycastableGrid.material == null)
					{
						worldDrawLayer_RaycastableGrid.material = new Material(WorldMaterials.VertexColorTransparent);
					}
					_003C_003E7__wrap1 = ((WorldDrawLayer)worldDrawLayer_RaycastableGrid).Regenerate().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0092;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0092;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0092:
					if (_003C_003E7__wrap1.MoveNext())
					{
						object current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					unsafeTileIDToVerts_offsets = worldDrawLayer_RaycastableGrid.planetLayer.UnsafeTileIDToVerts_offsets;
					unsafeVerts = worldDrawLayer_RaycastableGrid.planetLayer.UnsafeVerts;
					for (int i = 0; i < worldDrawLayer_RaycastableGrid.planetLayer.TilesCount; i++)
					{
						int subMeshIndex;
						LayerSubMesh subMesh = worldDrawLayer_RaycastableGrid.GetSubMesh(worldDrawLayer_RaycastableGrid.material, out subMeshIndex);
						int count = subMesh.verts.Count;
						int num2 = 0;
						int num3 = ((i + 1 < unsafeTileIDToVerts_offsets.Length) ? unsafeTileIDToVerts_offsets[i + 1] : unsafeVerts.Length);
						_ = unsafeTileIDToVerts_offsets[i];
						for (int j = unsafeTileIDToVerts_offsets[i]; j < num3; j++)
						{
							subMesh.verts.Add(unsafeVerts[j]);
							subMesh.colors.Add(new Color32(200, 200, 200, 10));
							switch (num2)
							{
							case 0:
								subMesh.uvs.Add(new Vector3(1f, 0f, 0f));
								break;
							case 1:
								subMesh.uvs.Add(new Vector3(0f, 1f, 0f));
								break;
							default:
								subMesh.uvs.Add(new Vector3(0f, 0f, 1f));
								break;
							}
							if (j < num3 - 2)
							{
								subMesh.tris.Add(count + num2 + 2);
								subMesh.tris.Add(count + num2 + 1);
								subMesh.tris.Add(count);
								worldDrawLayer_RaycastableGrid.AppendRaycastableTriangle(subMeshIndex, i);
							}
							num2++;
						}
					}
					worldDrawLayer_RaycastableGrid.FinalizeMesh(MeshParts.All);
					_003C_003E7__wrap1 = worldDrawLayer_RaycastableGrid.RegenerateWorldMeshColliders().GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					object current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
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

		private void _003C_003Em__Finally2()
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
			_003CRegenerate_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__5(0)
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

	private Material material;

	public override bool VisibleWhenLayerNotSelected => false;

	public override bool VisibleInBackground => false;

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__5))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void Render()
	{
		if (ShouldRegenerate)
		{
			RegenerateNow();
		}
	}
}
