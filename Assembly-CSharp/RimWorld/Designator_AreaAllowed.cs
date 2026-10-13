using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200307C RID: 12412
	public abstract class Designator_AreaAllowed : Designator_Cells
	{
		// Token: 0x17002D17 RID: 11543
		// (get) Token: 0x060116CC RID: 71372 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D18 RID: 11544
		// (get) Token: 0x060116CD RID: 71373 RVA: 0x00501D4C File Offset: 0x004FFF4C
		public static Area SelectedArea
		{
			get
			{
				return Designator_AreaAllowed.selectedArea;
			}
		}

		// Token: 0x17002D19 RID: 11545
		// (get) Token: 0x060116CE RID: 71374 RVA: 0x00501D53 File Offset: 0x004FFF53
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x060116CF RID: 71375 RVA: 0x00501D5A File Offset: 0x004FFF5A
		public Designator_AreaAllowed(DesignateMode mode)
		{
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		// Token: 0x060116D0 RID: 71376 RVA: 0x00501D7F File Offset: 0x004FFF7F
		public static void ClearSelectedArea()
		{
			Designator_AreaAllowed.selectedArea = null;
		}

		// Token: 0x060116D1 RID: 71377 RVA: 0x00501D87 File Offset: 0x004FFF87
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			if (Designator_AreaAllowed.selectedArea != null && Find.WindowStack.FloatMenu == null)
			{
				Designator_AreaAllowed.selectedArea.MarkForDraw();
			}
		}

		// Token: 0x060116D2 RID: 71378 RVA: 0x00501DAC File Offset: 0x004FFFAC
		public override void ProcessInput(Event ev)
		{
			if (!base.CheckCanInteract())
			{
				return;
			}
			if (Designator_AreaAllowed.selectedArea != null)
			{
				base.ProcessInput(ev);
			}
			AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
			{
				Designator_AreaAllowed.selectedArea = a;
				this.<>n__0(ev);
			}, false, true, base.Map);
		}

		// Token: 0x060116D3 RID: 71379 RVA: 0x00501E02 File Offset: 0x00500002
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}

		// Token: 0x0400B487 RID: 46215
		public static Area selectedArea;
	}
}
