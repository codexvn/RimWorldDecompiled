using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200307F RID: 12415
	public class Designator_AreaAllowedClear : Designator_AreaAllowed
	{
		// Token: 0x060116DA RID: 71386 RVA: 0x00501EFC File Offset: 0x005000FC
		public Designator_AreaAllowedClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorClearAreaAllowed".Translate();
			this.defaultDesc = "DesignatorClearAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedClear", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.hotKey = KeyBindingDefOf.Misc10;
			this.tutorTag = "AreaAllowedClear";
		}

		// Token: 0x060116DB RID: 71387 RVA: 0x00501F7E File Offset: 0x0050017E
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x060116DC RID: 71388 RVA: 0x00501FA8 File Offset: 0x005001A8
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = false;
		}
	}
}
