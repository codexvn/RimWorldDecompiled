using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003089 RID: 12425
	public class Designator_AreaPollutionClearClear : Designator_AreaPollutionClear
	{
		// Token: 0x06011702 RID: 71426 RVA: 0x00502780 File Offset: 0x00500980
		public Designator_AreaPollutionClearClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorAreaPollutionClearClear".Translate();
			this.defaultDesc = "DesignatorAreaPollutionClearClearDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Icons/PollutionClearAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
		}
	}
}
