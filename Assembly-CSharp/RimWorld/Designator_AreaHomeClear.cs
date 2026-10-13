using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003084 RID: 12420
	public class Designator_AreaHomeClear : Designator_AreaHome
	{
		// Token: 0x060116EC RID: 71404 RVA: 0x00502310 File Offset: 0x00500510
		public Designator_AreaHomeClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorAreaHomeClear".Translate();
			this.defaultDesc = "DesignatorAreaHomeClearDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.hotKey = KeyBindingDefOf.Misc7;
		}
	}
}
