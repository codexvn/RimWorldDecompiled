using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200046F RID: 1135
	public class Section
	{
		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x0009811B File Offset: 0x0009631B
		public CellRect Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001DD7 RID: 7639 RVA: 0x00098124 File Offset: 0x00096324
		public CellRect CellRect
		{
			get
			{
				if (this.calculatedRect == default(CellRect))
				{
					this.calculatedRect = new CellRect(this.botLeft.x, this.botLeft.z, 17, 17);
					this.calculatedRect.ClipInsideMap(this.map);
				}
				return this.calculatedRect;
			}
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x00098184 File Offset: 0x00096384
		public Section(IntVec3 sectCoords, Map map)
		{
			this.botLeft = sectCoords * 17;
			this.map = map;
			bool flag = map.info.disableSunShadows || map.Biome.disableShadows;
			bool disableShadows = map.Biome.disableShadows;
			foreach (Type type in typeof(SectionLayer).AllSubclassesNonAbstract())
			{
				if ((!flag || !typeof(SectionLayer_SunShadows).IsAssignableFrom(type)) && (!disableShadows || !typeof(SectionLayer_EdgeShadows).IsAssignableFrom(type)) && (!(type == typeof(SectionLayer_PollutionCloud)) || ModsConfig.BiotechActive))
				{
					SectionLayer sectionLayer = (SectionLayer)Activator.CreateInstance(type, new object[]
					{
						this
					});
					this.layers.Add(sectionLayer);
					SectionLayer_Dynamic sectionLayer_Dynamic = sectionLayer as SectionLayer_Dynamic;
					if (sectionLayer_Dynamic != null)
					{
						this.dynamic.Add(sectionLayer_Dynamic);
					}
				}
			}
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x000982BC File Offset: 0x000964BC
		public void DrawSection()
		{
			if (this.anyLayerDirty)
			{
				this.RegenerateDirtyLayers();
			}
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].DrawLayer();
			}
			if (DebugViewSettings.drawSectionEdges)
			{
				Vector3 a = this.botLeft.ToVector3();
				GenDraw.DrawLineBetween(a, a + new Vector3(0f, 0f, 17f));
				GenDraw.DrawLineBetween(a, a + new Vector3(17f, 0f, 0f));
				if (this.CellRect.Contains(UI.MouseCell()))
				{
					Vector3 a2 = this.bounds.Min.ToVector3();
					Vector3 a3 = this.bounds.Max.ToVector3() + new Vector3(1f, 0f, 1f);
					GenDraw.DrawLineBetween(a2, a2 + new Vector3((float)this.bounds.Width, 0f, 0f), SimpleColor.Magenta, 0.2f);
					GenDraw.DrawLineBetween(a2, a2 + new Vector3(0f, 0f, (float)this.bounds.Height), SimpleColor.Magenta, 0.2f);
					GenDraw.DrawLineBetween(a3, a3 - new Vector3((float)this.bounds.Width, 0f, 0f), SimpleColor.Magenta, 0.2f);
					GenDraw.DrawLineBetween(a3, a3 - new Vector3(0f, 0f, (float)this.bounds.Height), SimpleColor.Magenta, 0.2f);
				}
			}
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x00098460 File Offset: 0x00096660
		public void DrawDynamicSections(CellRect view)
		{
			for (int i = 0; i < this.dynamic.Count; i++)
			{
				if (this.dynamic[i].ShouldDrawDynamic(view))
				{
					this.dynamic[i].DrawLayer();
				}
			}
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000984A8 File Offset: 0x000966A8
		public void RegenerateSingleLayer(SectionLayer layer)
		{
			if (!layer.Visible)
			{
				return;
			}
			try
			{
				layer.Regenerate();
				if (layer.Isnt<SectionLayer_Dynamic>())
				{
					this.bounds = this.bounds.Encapsulate(layer.GetBoundaryRect());
				}
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Could not regenerate layer {0}: {1}", layer.ToStringSafe<SectionLayer>(), arg));
			}
			layer.RefreshSubMeshBounds();
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x00098518 File Offset: 0x00096718
		public void RegenerateAllLayers()
		{
			this.bounds = this.CellRect;
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i].Visible)
				{
					try
					{
						this.layers[i].Regenerate();
						if (this.layers[i].Isnt<SectionLayer_Dynamic>())
						{
							this.bounds = this.bounds.Encapsulate(this.layers[i].GetBoundaryRect());
						}
					}
					catch (Exception arg)
					{
						Log.Error(string.Format("Could not regenerate layer {0}: {1}", this.layers[i].ToStringSafe<SectionLayer>(), arg));
					}
				}
			}
			for (int j = 0; j < this.layers.Count; j++)
			{
				this.layers[j].RefreshSubMeshBounds();
			}
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x00098604 File Offset: 0x00096804
		private void RegenerateDirtyLayers()
		{
			this.bounds = this.CellRect;
			this.anyLayerDirty = false;
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i].Visible)
				{
					if (this.layers[i].Dirty)
					{
						try
						{
							this.layers[i].Regenerate();
							if (this.layers[i].Isnt<SectionLayer_Dynamic>())
							{
								this.bounds = this.bounds.Encapsulate(this.layers[i].GetBoundaryRect());
							}
							goto IL_E4;
						}
						catch (Exception arg)
						{
							Log.Error(string.Format("Could not regenerate layer {0}: {1}", this.layers[i].ToStringSafe<SectionLayer>(), arg));
							goto IL_E4;
						}
					}
					if (this.layers[i].Isnt<SectionLayer_Dynamic>())
					{
						this.bounds = this.bounds.Encapsulate(this.layers[i].GetBoundaryRect());
					}
				}
				IL_E4:;
			}
			for (int j = 0; j < this.layers.Count; j++)
			{
				this.layers[j].RefreshSubMeshBounds();
			}
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x00098744 File Offset: 0x00096944
		public bool TryUpdate(CellRect view)
		{
			if (this.dirtyFlags == 0UL)
			{
				return false;
			}
			this.bounds = this.CellRect;
			bool flag = false;
			bool flag2 = this.bounds.Overlaps(view);
			bool result = false;
			for (int i = 0; i < this.layers.Count; i++)
			{
				SectionLayer sectionLayer = this.layers[i];
				sectionLayer.Dirty = (sectionLayer.Dirty || (this.dirtyFlags & sectionLayer.relevantChangeTypes) > 0UL);
				if (sectionLayer.Dirty)
				{
					if (!flag2)
					{
						flag = (flag || sectionLayer.Dirty);
					}
					else
					{
						try
						{
							sectionLayer.Regenerate();
						}
						catch (Exception arg)
						{
							Log.Error(string.Format("Could not regenerate layer {0}: {1}", sectionLayer.ToStringSafe<SectionLayer>(), arg));
						}
						finally
						{
						}
						result = true;
						sectionLayer.Dirty = false;
					}
				}
			}
			for (int j = 0; j < this.layers.Count; j++)
			{
				if (this.layers[j].Isnt<SectionLayer_Dynamic>())
				{
					this.bounds = this.bounds.Encapsulate(this.layers[j].GetBoundaryRect());
				}
			}
			for (int k = 0; k < this.layers.Count; k++)
			{
				this.layers[k].RefreshSubMeshBounds();
			}
			this.anyLayerDirty = flag;
			this.dirtyFlags = 0UL;
			return result;
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x000988C0 File Offset: 0x00096AC0
		public SectionLayer GetLayer(Type type)
		{
			return this.layers.FirstOrDefault((SectionLayer sect) => sect.GetType() == type);
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x000988F4 File Offset: 0x00096AF4
		public void Dispose()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].Dispose();
			}
			for (int j = 0; j < this.dynamic.Count; j++)
			{
				this.dynamic[j].Dispose();
			}
			this.layers.Clear();
			this.dynamic.Clear();
		}

		// Token: 0x040019DF RID: 6623
		public IntVec3 botLeft;

		// Token: 0x040019E0 RID: 6624
		public readonly Map map;

		// Token: 0x040019E1 RID: 6625
		private CellRect calculatedRect;

		// Token: 0x040019E2 RID: 6626
		private CellRect bounds;

		// Token: 0x040019E3 RID: 6627
		public ulong dirtyFlags;

		// Token: 0x040019E4 RID: 6628
		private bool anyLayerDirty;

		// Token: 0x040019E5 RID: 6629
		private readonly List<SectionLayer> layers = new List<SectionLayer>();

		// Token: 0x040019E6 RID: 6630
		private readonly List<SectionLayer_Dynamic> dynamic = new List<SectionLayer_Dynamic>();

		// Token: 0x040019E7 RID: 6631
		public const int Size = 17;
	}
}
