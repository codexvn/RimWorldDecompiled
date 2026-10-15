using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class WorldRenderer
{
	[CompilerGenerated]
	private sealed class _003CRegenerateDirtyLayersNow_Async_003Ed__17 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldRenderer _003C_003E4__this;

		private IEnumerator<WorldDrawLayerBase> _003C_003E7__wrap1;

		private IEnumerator _003Cenumerator_003E5__3;

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
		public _003CRegenerateDirtyLayersNow_Async_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || (uint)(num - 1) <= 1u)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003Cenumerator_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldRenderer worldRenderer = _003C_003E4__this;
				WorldDrawLayerBase current;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = worldRenderer.AllDrawLayers.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0115;
				case 1:
					_003C_003E1__state = -4;
					break;
				case 2:
					{
						_003C_003E1__state = -3;
						_003Cenumerator_003E5__3 = null;
						goto IL_0115;
					}
					IL_0115:
					do
					{
						if (_003C_003E7__wrap1.MoveNext())
						{
							current = _003C_003E7__wrap1.Current;
							continue;
						}
						_003C_003Em__Finally1();
						_003C_003E7__wrap1 = null;
						worldRenderer.asynchronousRegenerationActive = false;
						return false;
					}
					while (!current.Dirty || !current.Visible);
					_003Cenumerator_003E5__3 = current.Regenerate().GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				try
				{
					if (!_003Cenumerator_003E5__3.MoveNext())
					{
						goto IL_00ee;
					}
				}
				catch (Exception value)
				{
					Log.Error($"Could not regenerate WorldLayer: {value}");
					goto IL_00ee;
				}
				_003C_003E2__current = _003Cenumerator_003E5__3.Current;
				_003C_003E1__state = 1;
				return true;
				IL_00ee:
				_003C_003Em__Finally2();
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003Cenumerator_003E5__3 is IDisposable disposable)
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
			_003CRegenerateDirtyLayersNow_Async_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerateDirtyLayersNow_Async_003Ed__17(0)
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
	private sealed class _003Cget_AllDrawLayers_003Ed__5 : IEnumerable<WorldDrawLayerBase>, IEnumerable, IEnumerator<WorldDrawLayerBase>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private WorldDrawLayerBase _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerator<WorldDrawLayerBase> _003C_003E7__wrap1;

		private IEnumerator<KeyValuePair<int, PlanetLayer>> _003C_003E7__wrap2;

		private List<WorldDrawLayer>.Enumerator _003C_003E7__wrap3;

		WorldDrawLayerBase IEnumerator<WorldDrawLayerBase>.Current
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
		public _003Cget_AllDrawLayers_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
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
			case -5:
			case -4:
			case 2:
				try
				{
					if (num == -5 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = default(List<WorldDrawLayer>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = Find.WorldGrid.GlobalLayers.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -5;
						goto IL_0106;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						WorldDrawLayerBase current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = Find.WorldGrid.PlanetLayers.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0125;
					IL_0125:
					if (_003C_003E7__wrap2.MoveNext())
					{
						_003C_003E7__wrap2.Current.Deconstruct(out var _, out var value);
						PlanetLayer planetLayer = value;
						_003C_003E7__wrap3 = planetLayer.WorldDrawLayers.GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_0106;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					return false;
					IL_0106:
					if (_003C_003E7__wrap3.MoveNext())
					{
						WorldDrawLayer current2 = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = default(List<WorldDrawLayer>.Enumerator);
					goto IL_0125;
				}
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<WorldDrawLayerBase> IEnumerable<WorldDrawLayerBase>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_AllDrawLayers_003Ed__5(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<WorldDrawLayerBase>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllVisibleDrawLayers_003Ed__7 : IEnumerable<WorldDrawLayerBase>, IEnumerable, IEnumerator<WorldDrawLayerBase>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private WorldDrawLayerBase _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerator<WorldDrawLayerBase> _003C_003E7__wrap1;

		private IEnumerator<KeyValuePair<int, PlanetLayer>> _003C_003E7__wrap2;

		private List<WorldDrawLayer>.Enumerator _003C_003E7__wrap3;

		WorldDrawLayerBase IEnumerator<WorldDrawLayerBase>.Current
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
		public _003Cget_AllVisibleDrawLayers_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
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
			case -5:
			case -4:
			case 2:
				try
				{
					if (num == -5 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = default(List<WorldDrawLayer>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				PlanetLayer planetLayer2;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = Find.WorldGrid.GlobalLayers.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0077;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0077;
				case 2:
					{
						_003C_003E1__state = -5;
						goto IL_0119;
					}
					IL_0077:
					while (_003C_003E7__wrap1.MoveNext())
					{
						WorldDrawLayerBase current = _003C_003E7__wrap1.Current;
						if (current.Visible)
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = Find.WorldGrid.PlanetLayers.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0138;
					IL_0119:
					if (_003C_003E7__wrap3.MoveNext())
					{
						WorldDrawLayer current2 = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = default(List<WorldDrawLayer>.Enumerator);
					goto IL_0138;
					IL_0138:
					do
					{
						if (!_003C_003E7__wrap2.MoveNext())
						{
							_003C_003Em__Finally2();
							_003C_003E7__wrap2 = null;
							return false;
						}
						(_, planetLayer2) = (KeyValuePair<int, PlanetLayer>)(ref _003C_003E7__wrap2.Current);
					}
					while (!planetLayer2.Visible);
					_003C_003E7__wrap3 = planetLayer2.WorldDrawLayers.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0119;
				}
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<WorldDrawLayerBase> IEnumerable<WorldDrawLayerBase>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_AllVisibleDrawLayers_003Ed__7(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<WorldDrawLayerBase>)this).GetEnumerator();
		}
	}

	public WorldRenderMode wantedMode;

	private bool asynchronousRegenerationActive;

	private bool ShouldRegenerateDirtyLayersInLongEvent
	{
		get
		{
			foreach (var (_, planetLayer2) in Find.WorldGrid.PlanetLayers)
			{
				if (!planetLayer2.Visible)
				{
					continue;
				}
				foreach (WorldDrawLayer worldDrawLayer in planetLayer2.WorldDrawLayers)
				{
					if (worldDrawLayer.Dirty && worldDrawLayer is WorldDrawLayer_Terrain)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public IEnumerable<WorldDrawLayerBase> AllDrawLayers
	{
		[IteratorStateMachine(typeof(_003Cget_AllDrawLayers_003Ed__5))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllDrawLayers_003Ed__5(-2);
		}
	}

	public IEnumerable<WorldDrawLayerBase> AllVisibleDrawLayers
	{
		[IteratorStateMachine(typeof(_003Cget_AllVisibleDrawLayers_003Ed__7))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllVisibleDrawLayers_003Ed__7(-2);
		}
	}

	public void SetAllLayersDirty()
	{
		foreach (WorldDrawLayerBase allDrawLayer in AllDrawLayers)
		{
			allDrawLayer.SetDirty();
		}
	}

	public void SetDirty<T>(PlanetLayer planetLayer) where T : WorldDrawLayer
	{
		foreach (WorldDrawLayerBase allDrawLayer in AllDrawLayers)
		{
			if (allDrawLayer is WorldDrawLayer worldDrawLayer && planetLayer == worldDrawLayer.planetLayer && allDrawLayer is T)
			{
				allDrawLayer.SetDirty();
			}
		}
	}

	public bool HasGlobalLayer<T>() where T : WorldDrawLayer
	{
		T layer;
		return TryGetGlobalLayer<T>(out layer);
	}

	public bool HasLayer<T>(PlanetLayer planetLayer) where T : WorldDrawLayer
	{
		T layer;
		return TryGetLayer<T>(planetLayer, out layer);
	}

	public bool TryGetGlobalLayer<T>(out T layer) where T : WorldDrawLayer
	{
		layer = GetGlobalLayer<T>();
		return layer != null;
	}

	public bool TryGetLayer<T>(PlanetLayer planetLayer, out T layer) where T : WorldDrawLayer
	{
		layer = GetLayer<T>(planetLayer);
		return layer != null;
	}

	public T GetGlobalLayer<T>() where T : WorldDrawLayer
	{
		foreach (WorldDrawLayerBase globalLayer in Find.WorldGrid.GlobalLayers)
		{
			if (globalLayer is T result)
			{
				return result;
			}
		}
		return null;
	}

	public T GetLayer<T>(PlanetLayer planetLayer) where T : WorldDrawLayer
	{
		foreach (WorldDrawLayerBase allDrawLayer in AllDrawLayers)
		{
			if (allDrawLayer is WorldDrawLayer worldDrawLayer && planetLayer == worldDrawLayer.planetLayer && allDrawLayer is T result)
			{
				return result;
			}
		}
		return null;
	}

	public void RegenerateAllLayersNow()
	{
		foreach (WorldDrawLayerBase allDrawLayer in AllDrawLayers)
		{
			if (allDrawLayer.Visible)
			{
				allDrawLayer.RegenerateNow();
			}
		}
	}

	[IteratorStateMachine(typeof(_003CRegenerateDirtyLayersNow_Async_003Ed__17))]
	private IEnumerable RegenerateDirtyLayersNow_Async()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerateDirtyLayersNow_Async_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void Notify_StaticWorldObjectPosChanged()
	{
		foreach (WorldDrawLayerBase allDrawLayer in AllDrawLayers)
		{
			if (allDrawLayer is WorldDrawLayer_WorldObjects worldDrawLayer_WorldObjects)
			{
				worldDrawLayer_WorldObjects.SetDirty();
			}
		}
	}

	public void Notify_TilePollutionChanged(PlanetTile tileId)
	{
		foreach (WorldDrawLayerBase allDrawLayer in AllDrawLayers)
		{
			if (allDrawLayer is WorldDrawLayer_Pollution worldDrawLayer_Pollution)
			{
				worldDrawLayer_Pollution.Notify_TilePollutionChanged(tileId);
			}
		}
	}

	public void CheckActivateWorldCamera()
	{
		Find.WorldCamera.gameObject.SetActive(WorldRendererUtility.WorldRendered);
	}

	public bool RegenerateLayersIfDirtyInLongEvent()
	{
		if (ShouldRegenerateDirtyLayersInLongEvent)
		{
			asynchronousRegenerationActive = true;
			LongEventHandler.QueueLongEvent(RegenerateDirtyLayersNow_Async(), "GeneratingPlanet", null, showExtraUIInfo: false);
			return true;
		}
		return false;
	}

	public void DrawWorldLayers()
	{
		if (asynchronousRegenerationActive)
		{
			Log.Error("Called DrawWorldLayers() but already regenerating. This shouldn't ever happen because LongEventHandler should have stopped us.");
		}
		else
		{
			if (RegenerateLayersIfDirtyInLongEvent())
			{
				return;
			}
			GlobalRendererUtility.UpdateGlobalShadersParams();
			WorldRendererUtility.UpdateGlobalShadersParams();
			foreach (WorldDrawLayerBase globalLayer in Find.WorldGrid.GlobalLayers)
			{
				if (globalLayer.Visible)
				{
					try
					{
						globalLayer.Render();
					}
					catch (Exception value)
					{
						Log.Error($"Error drawing global WorldLayer: {value}");
					}
				}
			}
			foreach (var (_, planetLayer2) in Find.WorldGrid.PlanetLayers)
			{
				if (!planetLayer2.Visible)
				{
					continue;
				}
				foreach (WorldDrawLayer worldDrawLayer in planetLayer2.WorldDrawLayers)
				{
					try
					{
						worldDrawLayer.Render();
					}
					catch (Exception value2)
					{
						Log.Error($"Error drawing planet layer WorldLayer {worldDrawLayer.GetType().Name}: {value2} on layer {planetLayer2.Def.label}");
					}
				}
			}
		}
	}

	public PlanetTile GetTileFromRayHit(RaycastHit hit)
	{
		foreach (WorldDrawLayerBase allVisibleDrawLayer in AllVisibleDrawLayers)
		{
			if (allVisibleDrawLayer is WorldDrawLayer { Raycastable: not false } worldDrawLayer && worldDrawLayer.TryGetTileFromRayHit(hit, out var id))
			{
				return id;
			}
		}
		return PlanetTile.Invalid;
	}
}
