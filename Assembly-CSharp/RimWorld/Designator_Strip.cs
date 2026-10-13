using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030C5 RID: 12485
	public class Designator_Strip : Designator
	{
		// Token: 0x17002D69 RID: 11625
		// (get) Token: 0x06011875 RID: 71797 RVA: 0x00508A96 File Offset: 0x00506C96
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Strip;
			}
		}

		// Token: 0x17002D6A RID: 11626
		// (get) Token: 0x06011876 RID: 71798 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x06011877 RID: 71799 RVA: 0x00508AA0 File Offset: 0x00506CA0
		public Designator_Strip()
		{
			this.defaultLabel = "DesignatorStrip".Translate();
			this.defaultDesc = "DesignatorStripDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Strip", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		// Token: 0x06011878 RID: 71800 RVA: 0x00508B21 File Offset: 0x00506D21
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.StrippablesInCell(c).Any<Thing>())
			{
				return "MessageMustDesignateStrippable".Translate();
			}
			return true;
		}

		// Token: 0x06011879 RID: 71801 RVA: 0x00508B5C File Offset: 0x00506D5C
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.StrippablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0601187A RID: 71802 RVA: 0x00508BAC File Offset: 0x00506DAC
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			return StrippableUtility.CanBeStrippedByColony(t);
		}

		// Token: 0x0601187B RID: 71803 RVA: 0x00508BD9 File Offset: 0x00506DD9
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(t);
		}

		// Token: 0x0601187C RID: 71804 RVA: 0x00508C03 File Offset: 0x00506E03
		private IEnumerable<Thing> StrippablesInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return thingList[i];
				}
				num = i;
			}
			yield break;
		}
	}
}
