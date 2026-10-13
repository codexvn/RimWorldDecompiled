using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E7B RID: 15995
	public class WorldInspectPane : Window, IInspectPane
	{
		// Token: 0x170036AC RID: 13996
		// (get) Token: 0x06015E12 RID: 89618 RVA: 0x0068BC1D File Offset: 0x00689E1D
		// (set) Token: 0x06015E13 RID: 89619 RVA: 0x0068BC25 File Offset: 0x00689E25
		public Type OpenTabType
		{
			get
			{
				return this.openTabType;
			}
			set
			{
				this.openTabType = value;
			}
		}

		// Token: 0x170036AD RID: 13997
		// (get) Token: 0x06015E14 RID: 89620 RVA: 0x0068BC2E File Offset: 0x00689E2E
		// (set) Token: 0x06015E15 RID: 89621 RVA: 0x0068BC36 File Offset: 0x00689E36
		public float RecentHeight { get; set; }

		// Token: 0x170036AE RID: 13998
		// (get) Token: 0x06015E16 RID: 89622 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170036AF RID: 13999
		// (get) Token: 0x06015E17 RID: 89623 RVA: 0x0058B0BC File Offset: 0x005892BC
		public override Vector2 InitialSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x170036B0 RID: 14000
		// (get) Token: 0x06015E18 RID: 89624 RVA: 0x0068BC3F File Offset: 0x00689E3F
		private List<WorldObject> Selected
		{
			get
			{
				return Find.WorldSelector.SelectedObjects;
			}
		}

		// Token: 0x170036B1 RID: 14001
		// (get) Token: 0x06015E19 RID: 89625 RVA: 0x0068BC4B File Offset: 0x00689E4B
		private int NumSelectedObjects
		{
			get
			{
				return Find.WorldSelector.NumSelectedObjects;
			}
		}

		// Token: 0x170036B2 RID: 14002
		// (get) Token: 0x06015E1A RID: 89626 RVA: 0x0068BC58 File Offset: 0x00689E58
		public float PaneTopY
		{
			get
			{
				float num = (float)UI.screenHeight - 165f;
				if (Current.ProgramState == ProgramState.Playing)
				{
					num -= 35f;
				}
				return num;
			}
		}

		// Token: 0x170036B3 RID: 14003
		// (get) Token: 0x06015E1B RID: 89627 RVA: 0x0068BC83 File Offset: 0x00689E83
		public bool AnythingSelected
		{
			get
			{
				return Find.WorldSelector.AnyObjectOrTileSelected;
			}
		}

		// Token: 0x170036B4 RID: 14004
		// (get) Token: 0x06015E1C RID: 89628 RVA: 0x0064EDDB File Offset: 0x0064CFDB
		private PlanetTile SelectedTile
		{
			get
			{
				return Find.WorldSelector.SelectedTile;
			}
		}

		// Token: 0x170036B5 RID: 14005
		// (get) Token: 0x06015E1D RID: 89629 RVA: 0x0068BC90 File Offset: 0x00689E90
		private bool SelectedSingleObjectOrTile
		{
			get
			{
				return this.NumSelectedObjects == 1 || (this.NumSelectedObjects == 0 && this.SelectedTile.Valid);
			}
		}

		// Token: 0x170036B6 RID: 14006
		// (get) Token: 0x06015E1E RID: 89630 RVA: 0x0068BCC0 File Offset: 0x00689EC0
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x170036B7 RID: 14007
		// (get) Token: 0x06015E1F RID: 89631 RVA: 0x0068BCC0 File Offset: 0x00689EC0
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x170036B8 RID: 14008
		// (get) Token: 0x06015E20 RID: 89632 RVA: 0x0068BCC8 File Offset: 0x00689EC8
		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				if (this.NumSelectedObjects == 1)
				{
					return Find.WorldSelector.SingleSelectedObject.GetInspectTabs();
				}
				if (this.NumSelectedObjects == 0 && this.SelectedTile.Valid)
				{
					return PlanetLayer.Selected.Def.Tabs;
				}
				return null;
			}
		}

		// Token: 0x170036B9 RID: 14009
		// (get) Token: 0x06015E21 RID: 89633 RVA: 0x0068BD18 File Offset: 0x00689F18
		private string TileInspectString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				Vector2 vector = Find.WorldGrid.LongLatOf(this.SelectedTile);
				stringBuilder.Append(vector.y.ToStringLatitude());
				stringBuilder.Append(" ");
				stringBuilder.Append(vector.x.ToStringLongitude());
				Tile tile = Find.WorldGrid[this.SelectedTile];
				if (!tile.PrimaryBiome.impassable && tile.hilliness != Hilliness.Undefined)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(tile.hilliness.GetLabelCap());
				}
				SurfaceTile surfaceTile = tile as SurfaceTile;
				if (surfaceTile != null)
				{
					if (surfaceTile.Roads != null)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append((from rl in surfaceTile.Roads
						select rl.road).MaxBy((RoadDef road) => road.priority).LabelCap);
					}
					if (!Find.World.Impassable(this.SelectedTile))
					{
						string arg = (WorldPathGrid.CalculatedMovementDifficultyAt(this.SelectedTile, false, null, null) * Find.WorldGrid.GetRoadMovementDifficultyMultiplier(this.SelectedTile, PlanetTile.Invalid, null)).ToString("0.#");
						stringBuilder.AppendLine();
						stringBuilder.Append(string.Format("{0}: {1}", "MovementDifficulty".Translate(), arg));
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Format("{0}: {1}", "AvgTemp".Translate(), GenTemperature.GetAverageTemperatureLabel(this.SelectedTile)));
				if (ModsConfig.BiotechActive && tile.pollution > 0f && tile.OnSurface)
				{
					stringBuilder.AppendLine();
					string text = GenWorld.GetPollutionDescription(tile.pollution);
					text = text + " (" + tile.pollution.ToStringPercent() + ")";
					stringBuilder.Append(string.Format("{0}: {1}", "TilePollution".Translate(), text));
				}
				if (ModsConfig.OdysseyActive && tile.Mutators.Any<TileMutatorDef>())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("TileMutators".Translate() + ": " + (from m in tile.Mutators
					orderby -m.displayPriority
					select m.Label(tile.tile)).ToCommaList(false, false).CapitalizeFirst());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06015E22 RID: 89634 RVA: 0x0068C00E File Offset: 0x0068A20E
		public WorldInspectPane() : base(null)
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x06015E23 RID: 89635 RVA: 0x0068C048 File Offset: 0x0068A248
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			this.windowRect.x = 0f;
			this.windowRect.y = this.PaneTopY;
		}

		// Token: 0x06015E24 RID: 89636 RVA: 0x0068C074 File Offset: 0x0068A274
		public string GetLabel(Rect rect)
		{
			if (this.NumSelectedObjects > 0)
			{
				return WorldInspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
			}
			if (!this.SelectedTile.Valid)
			{
				return "error";
			}
			if (ModsConfig.OdysseyActive && Find.World.landmarks[this.SelectedTile] != null)
			{
				return Find.World.landmarks[this.SelectedTile].name;
			}
			return Find.WorldGrid[this.SelectedTile].PrimaryBiome.LabelCap;
		}

		// Token: 0x06015E25 RID: 89637 RVA: 0x0068C105 File Offset: 0x0068A305
		public void SelectNextInCell()
		{
			if (!this.AnythingSelected)
			{
				return;
			}
			if (this.NumSelectedObjects > 0)
			{
				Find.WorldSelector.SelectFirstOrNextAt(this.Selected[0].Tile);
				return;
			}
			Find.WorldSelector.SelectFirstOrNextAt(this.SelectedTile);
		}

		// Token: 0x06015E26 RID: 89638 RVA: 0x0068C148 File Offset: 0x0068A348
		public void DoPaneContents(Rect rect)
		{
			if (this.NumSelectedObjects > 0)
			{
				InspectPaneFiller.DoPaneContentsFor(Find.WorldSelector.FirstSelectedObject, rect);
				return;
			}
			if (this.SelectedTile.Valid)
			{
				InspectPaneFiller.DrawInspectString(this.TileInspectString, rect);
			}
		}

		// Token: 0x06015E27 RID: 89639 RVA: 0x0068C18C File Offset: 0x0068A38C
		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
			if (singleSelectedObject != null || this.SelectedTile.Valid)
			{
				float x = rect.width - 48f;
				if (singleSelectedObject != null)
				{
					Widgets.InfoCardButton(x, 0f, singleSelectedObject);
				}
				else if (Find.WorldGrid[this.SelectedTile].PrimaryBiome != null)
				{
					Widgets.InfoCardButton(x, 0f, Find.WorldGrid[this.SelectedTile].PrimaryBiome);
				}
				lineEndWidth += 24f;
			}
		}

		// Token: 0x06015E28 RID: 89640 RVA: 0x0068C219 File Offset: 0x0068A419
		public override void DoWindowContents(Rect rect)
		{
			InspectPaneUtility.InspectPaneOnGUI(rect, this);
		}

		// Token: 0x06015E29 RID: 89641 RVA: 0x0058B8F5 File Offset: 0x00589AF5
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
		}

		// Token: 0x06015E2A RID: 89642 RVA: 0x0068C222 File Offset: 0x0068A422
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
		}

		// Token: 0x06015E2B RID: 89643 RVA: 0x0068C230 File Offset: 0x0068A430
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x06015E2C RID: 89644 RVA: 0x0068C230 File Offset: 0x0068A430
		public void Reset()
		{
			this.openTabType = null;
		}

		// Token: 0x0400F2C1 RID: 62145
		private Type openTabType;
	}
}
