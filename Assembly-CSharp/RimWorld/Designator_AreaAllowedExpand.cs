using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200307E RID: 12414
	public class Designator_AreaAllowedExpand : Designator_AreaAllowed
	{
		// Token: 0x060116D7 RID: 71383 RVA: 0x00501E38 File Offset: 0x00500038
		public Designator_AreaAllowedExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorExpandAreaAllowed".Translate();
			this.defaultDesc = "DesignatorExpandAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedExpand", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_AllowedArea;
			this.hotKey = KeyBindingDefOf.Misc8;
			this.tutorTag = "AreaAllowedExpand";
		}

		// Token: 0x060116D8 RID: 71384 RVA: 0x00501EBE File Offset: 0x005000BE
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && !Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x060116D9 RID: 71385 RVA: 0x00501EEB File Offset: 0x005000EB
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = true;
		}
	}
}
