using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000425 RID: 1061
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001AEB RID: 6891 RVA: 0x000440FD File Offset: 0x000422FD
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00087F68 File Offset: 0x00086168
		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			this.drawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 3640, 1f);
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x00087FDC File Offset: 0x000861DC
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.defGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.defGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "defGrid");
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.countGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.countGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "countGrid");
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x00088039 File Offset: 0x00086239
		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x00088058 File Offset: 0x00086258
		public int CountAt(IntVec3 c)
		{
			return (int)this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x00088074 File Offset: 0x00086274
		public void SetAt(IntVec3 c, ThingDef def, int count)
		{
			if (count == 0)
			{
				def = null;
			}
			ushort num;
			if (def == null)
			{
				num = 0;
			}
			else
			{
				num = def.shortHash;
			}
			ushort num2 = (ushort)count;
			if (count > 65535)
			{
				Log.Error("Cannot store count " + count.ToString() + " in DeepResourceGrid: out of ushort range.");
				num2 = ushort.MaxValue;
			}
			if (count < 0)
			{
				Log.Error("Cannot store count " + count.ToString() + " in DeepResourceGrid: out of ushort range.");
				num2 = 0;
			}
			int num3 = this.map.cellIndices.CellToIndex(c);
			if (this.defGrid[num3] == num && this.countGrid[num3] == num2)
			{
				return;
			}
			this.defGrid[num3] = num;
			this.countGrid[num3] = num2;
			this.drawer.SetDirty();
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x00088128 File Offset: 0x00086328
		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x00088142 File Offset: 0x00086342
		public void MarkForDraw()
		{
			if (this.map == Find.CurrentMap && !Find.ScreenshotModeHandler.Active)
			{
				this.drawer.MarkForDraw();
			}
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x00088168 File Offset: 0x00086368
		public void DrawPlacingMouseAttachments(BuildableDef placingDef)
		{
			ThingDef thingDef = placingDef as ThingDef;
			if (thingDef != null && thingDef.CompDefFor<CompDeepDrill>() != null && this.AnyActiveDeepScannersOnMap())
			{
				this.RenderMouseAttachments();
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x00088198 File Offset: 0x00086398
		public void DeepResourcesOnGUI()
		{
			Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
			if (singleSelectedThing == null)
			{
				return;
			}
			CompDeepScanner compDeepScanner = singleSelectedThing.TryGetComp<CompDeepScanner>();
			CompDeepDrill compDeepDrill = singleSelectedThing.TryGetComp<CompDeepDrill>();
			if (compDeepScanner == null && compDeepDrill == null)
			{
				return;
			}
			if (!this.AnyActiveDeepScannersOnMap())
			{
				return;
			}
			this.RenderMouseAttachments();
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x000881D8 File Offset: 0x000863D8
		public bool AnyActiveDeepScannersOnMap()
		{
			foreach (Building thing in this.map.listerBuildings.allBuildingsColonist)
			{
				CompDeepScanner compDeepScanner = thing.TryGetComp<CompDeepScanner>();
				if (compDeepScanner != null && compDeepScanner.ShouldShowDeepResourceOverlay())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x00088248 File Offset: 0x00086448
		private void RenderMouseAttachments()
		{
			IntVec3 c = UI.MouseCell();
			if (!c.InBounds(this.map))
			{
				return;
			}
			ThingDef thingDef = this.map.deepResourceGrid.ThingDefAt(c);
			if (thingDef == null)
			{
				return;
			}
			int num = this.map.deepResourceGrid.CountAt(c);
			if (num <= 0)
			{
				return;
			}
			Vector2 vector = c.ToVector3().MapToUIPosition();
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			float num2 = (UI.CurUICellSize() - 27f) / 2f;
			Rect rect = new Rect(vector.x + num2, vector.y - UI.CurUICellSize() + num2, 27f, 27f);
			Widgets.ThingIcon(rect, thingDef, null, null, 1f, null, null, 1f);
			Widgets.Label(new Rect(rect.xMax + 4f, rect.y, 999f, 29f), "DeepResourceRemaining".Translate(thingDef.Named("RESOURCE"), num.Named("COUNT")));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x00088371 File Offset: 0x00086571
		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x00088390 File Offset: 0x00086590
		public Color GetCellExtraColor(int index)
		{
			IntVec3 c = this.map.cellIndices.IndexToCell(index);
			float num = (float)this.CountAt(c);
			ThingDef thingDef = this.ThingDefAt(c);
			return DebugMatsSpectrum.Mat(Mathf.RoundToInt(num / (float)thingDef.deepCountPerCell / 2f * 100f) % 100, true).color;
		}

		// Token: 0x040017A2 RID: 6050
		private const float LineSpacing = 29f;

		// Token: 0x040017A3 RID: 6051
		private const float IconPaddingRight = 4f;

		// Token: 0x040017A4 RID: 6052
		private const float IconSize = 27f;

		// Token: 0x040017A5 RID: 6053
		private Map map;

		// Token: 0x040017A6 RID: 6054
		private CellBoolDrawer drawer;

		// Token: 0x040017A7 RID: 6055
		private ushort[] defGrid;

		// Token: 0x040017A8 RID: 6056
		private ushort[] countGrid;
	}
}
