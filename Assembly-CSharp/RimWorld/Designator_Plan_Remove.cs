using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030F8 RID: 12536
	public class Designator_Plan_Remove : Designator_Plan
	{
		// Token: 0x17002DC2 RID: 11714
		// (get) Token: 0x060119DC RID: 72156 RVA: 0x0050E5F0 File Offset: 0x0050C7F0
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.RemovePlans;
			}
		}

		// Token: 0x060119DD RID: 72157 RVA: 0x0050E5F8 File Offset: 0x0050C7F8
		public Designator_Plan_Remove()
		{
			this.defaultLabel = "DesignatorPlanRemove".Translate();
			this.defaultDesc = "DesignatorPlanRemoveDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_PlanRemove;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc8;
		}

		// Token: 0x060119DE RID: 72158 RVA: 0x0050E680 File Offset: 0x0050C880
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (base.Map.planManager.PlanAt(c) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060119DF RID: 72159 RVA: 0x0050E6B8 File Offset: 0x0050C8B8
		public override void DesignateSingleCell(IntVec3 c)
		{
			Plan plan = base.Map.planManager.PlanAt(c);
			plan.RemoveCell(c);
			this.justDesignated.Add(plan);
		}

		// Token: 0x060119E0 RID: 72160 RVA: 0x0050E6EC File Offset: 0x0050C8EC
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			foreach (Plan plan in this.justDesignated)
			{
				plan.CheckContiguous();
			}
			this.justDesignated.Clear();
		}

		// Token: 0x0400B58E RID: 46478
		private readonly HashSet<Plan> justDesignated = new HashSet<Plan>();
	}
}
