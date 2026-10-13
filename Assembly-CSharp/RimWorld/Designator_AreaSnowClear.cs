using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200308A RID: 12426
	public abstract class Designator_AreaSnowClear : Designator_Cells
	{
		// Token: 0x17002D24 RID: 11556
		// (get) Token: 0x06011703 RID: 71427 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D25 RID: 11557
		// (get) Token: 0x06011704 RID: 71428 RVA: 0x00501D53 File Offset: 0x004FFF53
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x06011705 RID: 71429 RVA: 0x005027EC File Offset: 0x005009EC
		public Designator_AreaSnowClear(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "AreaSnowClear";
		}

		// Token: 0x06011706 RID: 71430 RVA: 0x0050283C File Offset: 0x00500A3C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			bool flag = base.Map.areaManager.SnowOrSandClear[c];
			if (this.mode == DesignateMode.Add)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x06011707 RID: 71431 RVA: 0x0050288D File Offset: 0x00500A8D
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.SnowOrSandClear[c] = true;
				return;
			}
			base.Map.areaManager.SnowOrSandClear[c] = false;
		}

		// Token: 0x06011708 RID: 71432 RVA: 0x005028C6 File Offset: 0x00500AC6
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.SnowOrSandClear.MarkForDraw();
		}

		// Token: 0x0400B490 RID: 46224
		private DesignateMode mode;
	}
}
