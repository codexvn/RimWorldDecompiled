using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003083 RID: 12419
	public class Designator_AreaHomeExpand : Designator_AreaHome
	{
		// Token: 0x060116EB RID: 71403 RVA: 0x00502288 File Offset: 0x00500488
		public Designator_AreaHomeExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorAreaHomeExpand".Translate();
			this.defaultDesc = "DesignatorAreaHomeExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.tutorTag = "AreaHomeExpand";
			this.hotKey = KeyBindingDefOf.Misc4;
		}
	}
}
