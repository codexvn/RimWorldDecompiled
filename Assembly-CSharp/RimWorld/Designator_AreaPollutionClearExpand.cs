using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003088 RID: 12424
	public class Designator_AreaPollutionClearExpand : Designator_AreaPollutionClear
	{
		// Token: 0x06011701 RID: 71425 RVA: 0x00502704 File Offset: 0x00500904
		public Designator_AreaPollutionClearExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorAreaPollutionClearExpand".Translate();
			this.defaultDesc = "DesignatorAreaPollutionClearExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Icons/PollutionClearArea", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_RemovePollution;
			this.tutorTag = "AreaPollutionClearExpand";
		}
	}
}
