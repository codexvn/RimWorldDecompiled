using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200309E RID: 12446
	public class Designator_FillIn : Designator
	{
		// Token: 0x17002D37 RID: 11575
		// (get) Token: 0x06011779 RID: 71545 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x17002D38 RID: 11576
		// (get) Token: 0x0601177A RID: 71546 RVA: 0x00223CAF File Offset: 0x00221EAF
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.FillIn;
			}
		}

		// Token: 0x0601177B RID: 71547 RVA: 0x0050471C File Offset: 0x0050291C
		public Designator_FillIn()
		{
			this.soundSucceeded = SoundDefOf.Tick_Low;
			this.defaultLabel = "DesignatorFillIn".Translate();
			this.defaultDesc = "DesignatorFillInDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/FillCrater", true);
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x0601177C RID: 71548 RVA: 0x0050477C File Offset: 0x0050297C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0601177D RID: 71549 RVA: 0x005047E0 File Offset: 0x005029E0
		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x0601177E RID: 71550 RVA: 0x0050482F File Offset: 0x00502A2F
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			return t is Crater && base.Map.designationManager.DesignationOn(t, this.Designation) == null;
		}

		// Token: 0x0601177F RID: 71551 RVA: 0x00503E3F File Offset: 0x0050203F
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
		}
	}
}
