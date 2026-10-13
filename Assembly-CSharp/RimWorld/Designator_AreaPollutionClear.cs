using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003087 RID: 12423
	public abstract class Designator_AreaPollutionClear : Designator_Cells
	{
		// Token: 0x17002D22 RID: 11554
		// (get) Token: 0x060116FB RID: 71419 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D23 RID: 11555
		// (get) Token: 0x060116FC RID: 71420 RVA: 0x00501D53 File Offset: 0x004FFF53
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x060116FD RID: 71421 RVA: 0x0050262F File Offset: 0x0050082F
		public Designator_AreaPollutionClear(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		// Token: 0x060116FE RID: 71422 RVA: 0x0050265C File Offset: 0x0050085C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			bool flag = base.Map.areaManager.PollutionClear[c];
			if (this.mode == DesignateMode.Add)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x060116FF RID: 71423 RVA: 0x005026AD File Offset: 0x005008AD
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.PollutionClear[c] = true;
				return;
			}
			base.Map.areaManager.PollutionClear[c] = false;
		}

		// Token: 0x06011700 RID: 71424 RVA: 0x005026E6 File Offset: 0x005008E6
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.PollutionClear.MarkForDraw();
		}

		// Token: 0x0400B48F RID: 46223
		private DesignateMode mode;
	}
}
