using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200308E RID: 12430
	public class Designator_Claim : Designator
	{
		// Token: 0x17002D27 RID: 11559
		// (get) Token: 0x06011715 RID: 71445 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x06011716 RID: 71446 RVA: 0x00502F64 File Offset: 0x00501164
		public Designator_Claim()
		{
			this.defaultLabel = "DesignatorClaim".Translate();
			this.defaultDesc = "DesignatorClaimDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Claim", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc4;
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x06011717 RID: 71447 RVA: 0x00502FEC File Offset: 0x005011EC
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
				return "MessageMustDesignateClaimable".Translate();
			}
			return true;
		}

		// Token: 0x06011718 RID: 71448 RVA: 0x00503058 File Offset: 0x00501258
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

		// Token: 0x06011719 RID: 71449 RVA: 0x005030A7 File Offset: 0x005012A7
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.Faction != Faction.OfPlayer)
			{
				return t.ClaimableBy(Faction.OfPlayer);
			}
			return false;
		}

		// Token: 0x0601171A RID: 71450 RVA: 0x005030C8 File Offset: 0x005012C8
		public override void DesignateThing(Thing t)
		{
			t.SetFaction(Faction.OfPlayer, null);
			foreach (IntVec3 cell in t.OccupiedRect())
			{
				FleckMaker.ThrowMetaPuffs(new TargetInfo(cell, base.Map, false));
			}
		}
	}
}
