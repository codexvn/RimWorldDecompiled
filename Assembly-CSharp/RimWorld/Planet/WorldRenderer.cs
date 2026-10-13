using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CA7 RID: 15527
	public class WorldRenderer
	{
		// Token: 0x17003496 RID: 13462
		// (get) Token: 0x060151A6 RID: 86438 RVA: 0x00650FB4 File Offset: 0x0064F1B4
		private bool ShouldRegenerateDirtyLayersInLongEvent
		{
			get
			{
				foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
				{
					int num;
					PlanetLayer planetLayer;
					keyValuePair.Deconstruct(out num, out planetLayer);
					PlanetLayer planetLayer2 = planetLayer;
					if (planetLayer2.Visible)
					{
						foreach (WorldDrawLayer worldDrawLayer in planetLayer2.WorldDrawLayers)
						{
							if (worldDrawLayer.Dirty && worldDrawLayer is WorldDrawLayer_Terrain)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17003497 RID: 13463
		// (get) Token: 0x060151A7 RID: 86439 RVA: 0x0065106C File Offset: 0x0064F26C
		public IEnumerable<WorldDrawLayerBase> AllDrawLayers
		{
			get
			{
				foreach (WorldDrawLayerBase worldDrawLayerBase in Find.WorldGrid.GlobalLayers)
				{
					yield return worldDrawLayerBase;
				}
				IEnumerator<WorldDrawLayerBase> enumerator = null;
				foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
				{
					int num;
					PlanetLayer planetLayer;
					keyValuePair.Deconstruct(out num, out planetLayer);
					PlanetLayer planetLayer2 = planetLayer;
					foreach (WorldDrawLayer worldDrawLayer in planetLayer2.WorldDrawLayers)
					{
						yield return worldDrawLayer;
					}
					List<WorldDrawLayer>.Enumerator enumerator3 = default(List<WorldDrawLayer>.Enumerator);
				}
				IEnumerator<KeyValuePair<int, PlanetLayer>> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17003498 RID: 13464
		// (get) Token: 0x060151A8 RID: 86440 RVA: 0x00651075 File Offset: 0x0064F275
		public IEnumerable<WorldDrawLayerBase> AllVisibleDrawLayers
		{
			get
			{
				foreach (WorldDrawLayerBase worldDrawLayerBase in Find.WorldGrid.GlobalLayers)
				{
					if (worldDrawLayerBase.Visible)
					{
						yield return worldDrawLayerBase;
					}
				}
				IEnumerator<WorldDrawLayerBase> enumerator = null;
				foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
				{
					int num;
					PlanetLayer planetLayer;
					keyValuePair.Deconstruct(out num, out planetLayer);
					PlanetLayer planetLayer2 = planetLayer;
					if (planetLayer2.Visible)
					{
						foreach (WorldDrawLayer worldDrawLayer in planetLayer2.WorldDrawLayers)
						{
							yield return worldDrawLayer;
						}
						List<WorldDrawLayer>.Enumerator enumerator3 = default(List<WorldDrawLayer>.Enumerator);
					}
				}
				IEnumerator<KeyValuePair<int, PlanetLayer>> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x060151A9 RID: 86441 RVA: 0x00651080 File Offset: 0x0064F280
		public void SetAllLayersDirty()
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				worldDrawLayerBase.SetDirty();
			}
		}

		// Token: 0x060151AA RID: 86442 RVA: 0x006510CC File Offset: 0x0064F2CC
		public void SetDirty<T>(PlanetLayer planetLayer) where T : WorldDrawLayer
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				WorldDrawLayer worldDrawLayer = worldDrawLayerBase as WorldDrawLayer;
				if (worldDrawLayer != null && planetLayer == worldDrawLayer.planetLayer && worldDrawLayerBase is T)
				{
					worldDrawLayerBase.SetDirty();
				}
			}
		}

		// Token: 0x060151AB RID: 86443 RVA: 0x00651134 File Offset: 0x0064F334
		public bool HasGlobalLayer<T>() where T : WorldDrawLayer
		{
			T t;
			return this.TryGetGlobalLayer<T>(out t);
		}

		// Token: 0x060151AC RID: 86444 RVA: 0x0065114C File Offset: 0x0064F34C
		public bool HasLayer<T>(PlanetLayer planetLayer) where T : WorldDrawLayer
		{
			T t;
			return this.TryGetLayer<T>(planetLayer, out t);
		}

		// Token: 0x060151AD RID: 86445 RVA: 0x00651162 File Offset: 0x0064F362
		public bool TryGetGlobalLayer<T>(out T layer) where T : WorldDrawLayer
		{
			layer = this.GetGlobalLayer<T>();
			return layer != null;
		}

		// Token: 0x060151AE RID: 86446 RVA: 0x0065117E File Offset: 0x0064F37E
		public bool TryGetLayer<T>(PlanetLayer planetLayer, out T layer) where T : WorldDrawLayer
		{
			layer = this.GetLayer<T>(planetLayer);
			return layer != null;
		}

		// Token: 0x060151AF RID: 86447 RVA: 0x0065119C File Offset: 0x0064F39C
		public T GetGlobalLayer<T>() where T : WorldDrawLayer
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in Find.WorldGrid.GlobalLayers)
			{
				T t = worldDrawLayerBase as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060151B0 RID: 86448 RVA: 0x00651208 File Offset: 0x0064F408
		public T GetLayer<T>(PlanetLayer planetLayer) where T : WorldDrawLayer
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				WorldDrawLayer worldDrawLayer = worldDrawLayerBase as WorldDrawLayer;
				if (worldDrawLayer != null && planetLayer == worldDrawLayer.planetLayer)
				{
					T t = worldDrawLayerBase as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x060151B1 RID: 86449 RVA: 0x00651288 File Offset: 0x0064F488
		public void RegenerateAllLayersNow()
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				if (worldDrawLayerBase.Visible)
				{
					worldDrawLayerBase.RegenerateNow();
				}
			}
		}

		// Token: 0x060151B2 RID: 86450 RVA: 0x006512DC File Offset: 0x0064F4DC
		private IEnumerable RegenerateDirtyLayersNow_Async()
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				if (worldDrawLayerBase.Dirty && worldDrawLayerBase.Visible)
				{
					using (IEnumerator enumerator = worldDrawLayerBase.Regenerate().GetEnumerator())
					{
						for (;;)
						{
							try
							{
								if (!enumerator.MoveNext())
								{
									break;
								}
							}
							catch (Exception arg)
							{
								Log.Error(string.Format("Could not regenerate WorldLayer: {0}", arg));
								break;
							}
							yield return enumerator.Current;
						}
					}
					yield return null;
					IEnumerator enumerator = null;
				}
			}
			IEnumerator<WorldDrawLayerBase> enumerator2 = null;
			this.asynchronousRegenerationActive = false;
			yield break;
			yield break;
		}

		// Token: 0x060151B3 RID: 86451 RVA: 0x006512EC File Offset: 0x0064F4EC
		public void Notify_StaticWorldObjectPosChanged()
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				WorldDrawLayer_WorldObjects worldDrawLayer_WorldObjects = worldDrawLayerBase as WorldDrawLayer_WorldObjects;
				if (worldDrawLayer_WorldObjects != null)
				{
					worldDrawLayer_WorldObjects.SetDirty();
				}
			}
		}

		// Token: 0x060151B4 RID: 86452 RVA: 0x00651340 File Offset: 0x0064F540
		public void Notify_TilePollutionChanged(PlanetTile tileId)
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				WorldDrawLayer_Pollution worldDrawLayer_Pollution = worldDrawLayerBase as WorldDrawLayer_Pollution;
				if (worldDrawLayer_Pollution != null)
				{
					worldDrawLayer_Pollution.Notify_TilePollutionChanged(tileId);
				}
			}
		}

		// Token: 0x060151B5 RID: 86453 RVA: 0x00651398 File Offset: 0x0064F598
		public void CheckActivateWorldCamera()
		{
			Find.WorldCamera.gameObject.SetActive(WorldRendererUtility.WorldRendered);
		}

		// Token: 0x060151B6 RID: 86454 RVA: 0x006513AE File Offset: 0x0064F5AE
		public bool RegenerateLayersIfDirtyInLongEvent()
		{
			if (this.ShouldRegenerateDirtyLayersInLongEvent)
			{
				this.asynchronousRegenerationActive = true;
				LongEventHandler.QueueLongEvent(this.RegenerateDirtyLayersNow_Async(), "GeneratingPlanet", null, false, false);
				return true;
			}
			return false;
		}

		// Token: 0x060151B7 RID: 86455 RVA: 0x006513D8 File Offset: 0x0064F5D8
		public void DrawWorldLayers()
		{
			if (this.asynchronousRegenerationActive)
			{
				Log.Error("Called DrawWorldLayers() but already regenerating. This shouldn't ever happen because LongEventHandler should have stopped us.");
				return;
			}
			if (this.RegenerateLayersIfDirtyInLongEvent())
			{
				return;
			}
			GlobalRendererUtility.UpdateGlobalShadersParams();
			WorldRendererUtility.UpdateGlobalShadersParams();
			foreach (WorldDrawLayerBase worldDrawLayerBase in Find.WorldGrid.GlobalLayers)
			{
				if (worldDrawLayerBase.Visible)
				{
					try
					{
						worldDrawLayerBase.Render();
					}
					catch (Exception arg)
					{
						Log.Error(string.Format("Error drawing global WorldLayer: {0}", arg));
					}
				}
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				if (planetLayer2.Visible)
				{
					foreach (WorldDrawLayer worldDrawLayer in planetLayer2.WorldDrawLayers)
					{
						try
						{
							worldDrawLayer.Render();
						}
						catch (Exception arg2)
						{
							Log.Error(string.Format("Error drawing planet layer WorldLayer {0}: {1} on layer {2}", worldDrawLayer.GetType().Name, arg2, planetLayer2.Def.label));
						}
					}
				}
			}
		}

		// Token: 0x060151B8 RID: 86456 RVA: 0x00651550 File Offset: 0x0064F750
		public PlanetTile GetTileFromRayHit(RaycastHit hit)
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllVisibleDrawLayers)
			{
				WorldDrawLayer worldDrawLayer = worldDrawLayerBase as WorldDrawLayer;
				PlanetTile result;
				if (worldDrawLayer != null && worldDrawLayer.Raycastable && worldDrawLayer.TryGetTileFromRayHit(hit, out result))
				{
					return result;
				}
			}
			return PlanetTile.Invalid;
		}

		// Token: 0x0400EB81 RID: 60289
		public WorldRenderMode wantedMode;

		// Token: 0x0400EB82 RID: 60290
		private bool asynchronousRegenerationActive;
	}
}
