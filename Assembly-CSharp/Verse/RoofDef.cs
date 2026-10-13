using System;

namespace Verse
{
	// Token: 0x020002D7 RID: 727
	public class RoofDef : Def
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x000540B8 File Offset: 0x000522B8
		public bool VanishOnCollapse
		{
			get
			{
				return !this.isThickRoof;
			}
		}

		// Token: 0x04001031 RID: 4145
		public bool isNatural;

		// Token: 0x04001032 RID: 4146
		public bool isThickRoof;

		// Token: 0x04001033 RID: 4147
		public ThingDef collapseLeavingThingDef;

		// Token: 0x04001034 RID: 4148
		public ThingDef filthLeaving;

		// Token: 0x04001035 RID: 4149
		public SoundDef soundPunchThrough;

		// Token: 0x04001036 RID: 4150
		public bool canCollapse = true;
	}
}
