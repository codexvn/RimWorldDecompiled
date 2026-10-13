using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E91 RID: 7825
	public class Area_NoRoof : Area
	{
		// Token: 0x17001A95 RID: 6805
		// (get) Token: 0x0600ADBE RID: 44478 RVA: 0x00319207 File Offset: 0x00317407
		public override string Label
		{
			get
			{
				return "NoRoof".Translate();
			}
		}

		// Token: 0x17001A96 RID: 6806
		// (get) Token: 0x0600ADBF RID: 44479 RVA: 0x00319218 File Offset: 0x00317418
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.5f, 0.1f);
			}
		}

		// Token: 0x17001A97 RID: 6807
		// (get) Token: 0x0600ADC0 RID: 44480 RVA: 0x0031922E File Offset: 0x0031742E
		public override int ListPriority
		{
			get
			{
				return 8000;
			}
		}

		// Token: 0x0600ADC1 RID: 44481 RVA: 0x0031916A File Offset: 0x0031736A
		public Area_NoRoof()
		{
		}

		// Token: 0x0600ADC2 RID: 44482 RVA: 0x00319172 File Offset: 0x00317372
		public Area_NoRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x0600ADC3 RID: 44483 RVA: 0x00319235 File Offset: 0x00317435
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_NoRoof";
		}
	}
}
