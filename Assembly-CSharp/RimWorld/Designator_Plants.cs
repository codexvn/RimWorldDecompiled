using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003109 RID: 12553
	public abstract class Designator_Plants : Designator
	{
		// Token: 0x17002DDB RID: 11739
		// (get) Token: 0x06011A32 RID: 72242 RVA: 0x0050FAE3 File Offset: 0x0050DCE3
		protected override DesignationDef Designation
		{
			get
			{
				return this.designationDef;
			}
		}

		// Token: 0x17002DDC RID: 11740
		// (get) Token: 0x06011A33 RID: 72243 RVA: 0x0050FAEB File Offset: 0x0050DCEB
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Plants;
			}
		}

		// Token: 0x06011A34 RID: 72244 RVA: 0x0050FAF2 File Offset: 0x0050DCF2
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.plant == null)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, this.designationDef) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06011A35 RID: 72245 RVA: 0x0050FB30 File Offset: 0x0050DD30
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			Plant plant = c.GetPlant(base.Map);
			if (plant == null)
			{
				return "MessageMustDesignatePlants".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(plant);
			if (!result.Accepted)
			{
				return result;
			}
			return true;
		}

		// Token: 0x06011A36 RID: 72246 RVA: 0x0050FB99 File Offset: 0x0050DD99
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetPlant(base.Map));
		}

		// Token: 0x06011A37 RID: 72247 RVA: 0x0050FBAD File Offset: 0x0050DDAD
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.designationDef, null));
		}

		// Token: 0x06011A38 RID: 72248 RVA: 0x00502E05 File Offset: 0x00501005
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x0400B5A5 RID: 46501
		protected DesignationDef designationDef;
	}
}
