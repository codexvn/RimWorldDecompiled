using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200309F RID: 12447
	public class Designator_Forbid : Designator
	{
		// Token: 0x17002D39 RID: 11577
		// (get) Token: 0x06011781 RID: 71553 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x06011782 RID: 71554 RVA: 0x00504878 File Offset: 0x00502A78
		public Designator_Forbid()
		{
			this.defaultLabel = "DesignatorForbid".Translate();
			this.defaultDesc = "DesignatorForbidDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Checkbox_TurnedOff;
			this.hotKey = KeyBindingDefOf.Command_ItemForbid;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "ForbidAllItems".Translate();
		}

		// Token: 0x06011783 RID: 71555 RVA: 0x00504918 File Offset: 0x00502B18
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return "MessageMustDesignateForbiddable".Translate();
			}
			return true;
		}

		// Token: 0x06011784 RID: 71556 RVA: 0x00504980 File Offset: 0x00502B80
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

		// Token: 0x06011785 RID: 71557 RVA: 0x005049D0 File Offset: 0x00502BD0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.category != ThingCategory.Item)
			{
				return false;
			}
			CompForbiddable compForbiddable = t.TryGetComp<CompForbiddable>();
			return compForbiddable != null && !compForbiddable.Forbidden;
		}

		// Token: 0x06011786 RID: 71558 RVA: 0x00504A0D File Offset: 0x00502C0D
		public override void DesignateThing(Thing t)
		{
			t.SetForbidden(true, false);
		}
	}
}
