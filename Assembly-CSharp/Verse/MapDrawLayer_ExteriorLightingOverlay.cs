using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

[StaticConstructorOnStartup]
public class MapDrawLayer_ExteriorLightingOverlay : MapDrawLayer
{
	[CompilerGenerated]
	private sealed class _003CGetClipperRects_003Ed__6 : IEnumerable<Rect>, IEnumerable, IEnumerator<Rect>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rect _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		Rect IEnumerator<Rect>.Current
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
		public _003CGetClipperRects_003Ed__6(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rect(-200f, 0f, 200f, map.Size.z);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rect(map.Size.x, 0f, 200f, map.Size.z);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rect(-200f, map.Size.z, map.Size.x + 400, 200f);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rect(-200f, -200f, map.Size.x + 400, 200f);
				_003C_003E1__state = 4;
				return true;
			case 4:
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
		IEnumerator<Rect> IEnumerable<Rect>.GetEnumerator()
		{
			_003CGetClipperRects_003Ed__6 _003CGetClipperRects_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetClipperRects_003Ed__ = this;
			}
			else
			{
				_003CGetClipperRects_003Ed__ = new _003CGetClipperRects_003Ed__6(0);
			}
			_003CGetClipperRects_003Ed__.map = _003C_003E3__map;
			return _003CGetClipperRects_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rect>)this).GetEnumerator();
		}
	}

	private static readonly int RenderLayer = LayerMask.NameToLayer("GravshipExclude");

	private const int Size = 200;

	public override bool Visible
	{
		get
		{
			if (DebugViewSettings.drawLightingOverlay)
			{
				return !base.Map.DrawMapClippers;
			}
			return false;
		}
	}

	public MapDrawLayer_ExteriorLightingOverlay(Map map)
		: base(map)
	{
	}

	public override void Regenerate()
	{
		LayerSubMesh subMesh = GetSubMesh(MatBases.LightOverlay);
		LayerSubMesh subMesh2 = GetSubMesh(MatBases.ShadowMask);
		subMesh.renderLayer = RenderLayer;
		subMesh2.renderLayer = RenderLayer;
		MakeBaseGeometry(base.Map, subMesh);
		MakeBaseGeometry(base.Map, subMesh2);
	}

	[IteratorStateMachine(typeof(_003CGetClipperRects_003Ed__6))]
	private static IEnumerable<Rect> GetClipperRects(Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetClipperRects_003Ed__6(-2)
		{
			_003C_003E3__map = map
		};
	}

	private static void MakeBaseGeometry(Map map, LayerSubMesh sm)
	{
		float y = AltitudeLayer.LightingOverlay.AltitudeFor();
		sm.Clear(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
		foreach (Rect clipperRect in GetClipperRects(map))
		{
			sm.verts.Add(new Vector3(clipperRect.x, y, clipperRect.y));
			sm.verts.Add(new Vector3(clipperRect.x, y, clipperRect.yMax));
			sm.verts.Add(new Vector3(clipperRect.xMax, y, clipperRect.yMax));
			sm.verts.Add(new Vector3(clipperRect.xMax, y, clipperRect.y));
			sm.colors.Add(new Color32(0, 0, 0, 0));
			sm.colors.Add(new Color32(0, 0, 0, 0));
			sm.colors.Add(new Color32(0, 0, 0, 0));
			sm.colors.Add(new Color32(0, 0, 0, 0));
			sm.tris.Add(sm.verts.Count - 4);
			sm.tris.Add(sm.verts.Count - 3);
			sm.tris.Add(sm.verts.Count - 2);
			sm.tris.Add(sm.verts.Count - 4);
			sm.tris.Add(sm.verts.Count - 2);
			sm.tris.Add(sm.verts.Count - 1);
		}
		sm.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
	}
}
