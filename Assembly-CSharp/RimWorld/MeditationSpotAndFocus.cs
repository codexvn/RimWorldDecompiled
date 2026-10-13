using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001505 RID: 5381
	public struct MeditationSpotAndFocus
	{
		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x0600802E RID: 32814 RVA: 0x00269EE3 File Offset: 0x002680E3
		public bool IsValid
		{
			get
			{
				return this.spot.IsValid;
			}
		}

		// Token: 0x0600802F RID: 32815 RVA: 0x00269EF0 File Offset: 0x002680F0
		public MeditationSpotAndFocus(LocalTargetInfo spot)
		{
			this.spot = spot;
			this.focus = LocalTargetInfo.Invalid;
		}

		// Token: 0x06008030 RID: 32816 RVA: 0x00269F04 File Offset: 0x00268104
		public MeditationSpotAndFocus(LocalTargetInfo spot, LocalTargetInfo focus)
		{
			this.spot = spot;
			this.focus = focus;
		}

		// Token: 0x040055AC RID: 21932
		public LocalTargetInfo spot;

		// Token: 0x040055AD RID: 21933
		public LocalTargetInfo focus;
	}
}
