using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030CD RID: 12493
	public class Designator_Unforbid : Designator
	{
		// Token: 0x17002D71 RID: 11633
		// (get) Token: 0x060118A3 RID: 71843 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x060118A4 RID: 71844 RVA: 0x00509464 File Offset: 0x00507664
		public Designator_Unforbid()
		{
			this.defaultLabel = "DesignatorUnforbid".Translate();
			this.defaultDesc = "DesignatorUnforbidDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Checkbox_TurnedOn;
			this.hotKey = KeyBindingDefOf.Misc6;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "UnforbidAllItems".Translate();
			this.tutorTag = "Unforbid";
		}

		// Token: 0x060118A5 RID: 71845 RVA: 0x0050950C File Offset: 0x0050770C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return "MessageMustDesignateUnforbiddable".Translate();
			}
			return true;
		}

		// Token: 0x060118A6 RID: 71846 RVA: 0x00509574 File Offset: 0x00507774
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

		// Token: 0x060118A7 RID: 71847 RVA: 0x005095C4 File Offset: 0x005077C4
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.category != ThingCategory.Item)
			{
				return false;
			}
			CompForbiddable compForbiddable = t.TryGetComp<CompForbiddable>();
			return compForbiddable != null && compForbiddable.Forbidden;
		}

		// Token: 0x060118A8 RID: 71848 RVA: 0x005095FE File Offset: 0x005077FE
		public override void DesignateThing(Thing t)
		{
			t.SetForbidden(false, false);
		}
	}
}
