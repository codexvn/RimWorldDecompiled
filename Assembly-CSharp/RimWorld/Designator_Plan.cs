using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020030EC RID: 12524
	public abstract class Designator_Plan : Designator_Cells
	{
		// Token: 0x17002DA2 RID: 11682
		// (get) Token: 0x0601197A RID: 72058 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002DA3 RID: 11683
		// (get) Token: 0x0601197B RID: 72059 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002DA4 RID: 11684
		// (get) Token: 0x0601197C RID: 72060 RVA: 0x0050CFD7 File Offset: 0x0050B1D7
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Plans;
			}
		}

		// Token: 0x0601197D RID: 72061 RVA: 0x0050CFDE File Offset: 0x0050B1DE
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoZoneEdgeLines();
		}
	}
}
