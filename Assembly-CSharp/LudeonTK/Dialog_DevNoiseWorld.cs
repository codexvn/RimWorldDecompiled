using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000092 RID: 146
	public class Dialog_DevNoiseWorld : Dialog_DevNoiseBase
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0001119B File Offset: 0x0000F39B
		protected override string Title
		{
			get
			{
				return "World Noise Visualizer";
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000111A2 File Offset: 0x0000F3A2
		public override void PreOpen()
		{
			base.PreOpen();
			this.OnNoiseChanged();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000111B0 File Offset: 0x0000F3B0
		public override void PostClose()
		{
			base.PostClose();
			this.OnNoiseChanged();
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000111BE File Offset: 0x0000F3BE
		protected override void OnNoiseChanged()
		{
			Find.World.renderer.SetDirty<WorldDrawLayer_DebugNoise>(PlanetLayer.Selected);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x000111D4 File Offset: 0x0000F3D4
		protected override int GetSeed()
		{
			return Find.World.info.Seed;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000111E8 File Offset: 0x0000F3E8
		protected override void DoWindowListing()
		{
			WorldDrawLayer_DebugNoise worldDrawLayer_DebugNoise;
			if (!Find.World.renderer.TryGetLayer<WorldDrawLayer_DebugNoise>(PlanetLayer.Selected, out worldDrawLayer_DebugNoise))
			{
				return;
			}
			base.DoWindowListing();
			if (Find.WorldSelector.SelectedTile == PlanetTile.Invalid)
			{
				base.PrintLabel("No tile selected", GameFont.Tiny);
				return;
			}
			PlanetTile selectedTile = Find.WorldSelector.SelectedTile;
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(selectedTile);
			float num = (float)this.noise.GetValue((double)tileCenter.x, (double)tileCenter.y, (double)tileCenter.z);
			base.PrintLabel(string.Format("Selected tile ({0}):", selectedTile), GameFont.Tiny);
			base.PrintLabel("Coordinates: " + tileCenter.ToString("0.#"), GameFont.Tiny);
			base.PrintLabel(string.Format("Noise value: {0:0.###}", num), GameFont.Tiny);
		}
	}
}
