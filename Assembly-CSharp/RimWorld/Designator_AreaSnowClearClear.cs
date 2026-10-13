using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200308C RID: 12428
	public class Designator_AreaSnowClearClear : Designator_AreaSnowClear
	{
		// Token: 0x0601170A RID: 71434 RVA: 0x0050296C File Offset: 0x00500B6C
		public Designator_AreaSnowClearClear() : base(DesignateMode.Remove)
		{
			string key = "DesignatorAreaSnowClearClear";
			string key2 = "DesignatorAreaSnowClearClearDesc";
			if (ModsConfig.OdysseyActive)
			{
				key = "DesignatorAreaSnowOrSandClearClear";
				key2 = "DesignatorAreaSnowOrSandClearClearDesc";
			}
			this.defaultLabel = key.Translate();
			this.defaultDesc = key2.Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
		}
	}
}
