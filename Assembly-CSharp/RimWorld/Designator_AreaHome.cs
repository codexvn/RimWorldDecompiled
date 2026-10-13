using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003082 RID: 12418
	public abstract class Designator_AreaHome : Designator_Cells
	{
		// Token: 0x17002D1C RID: 11548
		// (get) Token: 0x060116E4 RID: 71396 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D1D RID: 11549
		// (get) Token: 0x060116E5 RID: 71397 RVA: 0x00501D53 File Offset: 0x004FFF53
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x060116E6 RID: 71398 RVA: 0x00502195 File Offset: 0x00500395
		public Designator_AreaHome(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x060116E7 RID: 71399 RVA: 0x005021CC File Offset: 0x005003CC
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			bool flag = base.Map.areaManager.Home[c];
			if (this.mode == DesignateMode.Add)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x060116E8 RID: 71400 RVA: 0x0050221D File Offset: 0x0050041D
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.Home[c] = true;
				return;
			}
			base.Map.areaManager.Home[c] = false;
		}

		// Token: 0x060116E9 RID: 71401 RVA: 0x00502256 File Offset: 0x00500456
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HomeArea, KnowledgeAmount.Total);
		}

		// Token: 0x060116EA RID: 71402 RVA: 0x00502269 File Offset: 0x00500469
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.Home.MarkForDraw();
		}

		// Token: 0x0400B48D RID: 46221
		private DesignateMode mode;
	}
}
