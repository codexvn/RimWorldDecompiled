using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020030F4 RID: 12532
	public class Designator_Plan_CopySelection : Designator_Plan
	{
		// Token: 0x17002DB7 RID: 11703
		// (get) Token: 0x060119BA RID: 72122 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x17002DB8 RID: 11704
		// (get) Token: 0x060119BB RID: 72123 RVA: 0x00002501 File Offset: 0x00000701
		public override bool DrawHighlight
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060119BC RID: 72124 RVA: 0x0050DEFC File Offset: 0x0050C0FC
		public Designator_Plan_CopySelection()
		{
			this.useMouseIcon = true;
			this.hideMouseIcon = true;
			this.mouseText = "CommandCopyPlanSelectionMouse".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanCopySelection", true);
			this.defaultLabel = "CommandCopyPlanSelectionLabel".Translate();
			this.defaultDesc = "CommandCopyPlanSelectionDesc".Translate();
			this.hotKey = KeyBindingDefOf.Designator_Cancel;
		}

		// Token: 0x060119BD RID: 72125 RVA: 0x0050DF78 File Offset: 0x0050C178
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return loc.InBounds(base.Map);
		}

		// Token: 0x060119BE RID: 72126 RVA: 0x0050DF8C File Offset: 0x0050C18C
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			Designator_Plan_CopySelectionPaste designator_Plan_CopySelectionPaste = DesignatorUtility.FindAllowedDesignator<Designator_Plan_CopySelectionPaste>();
			designator_Plan_CopySelectionPaste.Initialize(cells);
			Find.DesignatorManager.Select(designator_Plan_CopySelectionPaste);
			SoundDefOf.Click.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
		}

		// Token: 0x060119BF RID: 72127 RVA: 0x0050DFC1 File Offset: 0x0050C1C1
		public override void Selected()
		{
			base.Selected();
			Find.Selector.SelectedPlan = null;
		}
	}
}
