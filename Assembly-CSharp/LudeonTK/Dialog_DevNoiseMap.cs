using System;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000091 RID: 145
	public class Dialog_DevNoiseMap : Dialog_DevNoiseBase
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000315 RID: 789 RVA: 0x000110CA File Offset: 0x0000F2CA
		protected override string Title
		{
			get
			{
				return "Map Noise Visualizer";
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000110D1 File Offset: 0x0000F2D1
		protected override void OnNoiseChanged()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return;
			}
			currentMap.mapDrawer.RegenerateEverythingNow();
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000110E7 File Offset: 0x0000F2E7
		protected override int GetSeed()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return 0;
			}
			return currentMap.ConstantRandSeed;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000110FC File Offset: 0x0000F2FC
		protected override void DoWindowListing()
		{
			if (Find.CurrentMap == null)
			{
				base.PrintLabel("Not viewing map", GameFont.Tiny);
				return;
			}
			base.DoWindowListing();
			IntVec3 intVec = UI.MouseCell();
			if (!intVec.InBounds(Find.CurrentMap))
			{
				base.PrintLabel("Out of bounds", GameFont.Tiny);
				return;
			}
			float num = (float)this.noise.GetValue((double)intVec.x, (double)intVec.y, (double)intVec.z);
			base.PrintLabel(string.Format("Selected cell: {0}", intVec), GameFont.Tiny);
			base.PrintLabel(string.Format("Noise value: {0:0.###}", num), GameFont.Tiny);
		}
	}
}
