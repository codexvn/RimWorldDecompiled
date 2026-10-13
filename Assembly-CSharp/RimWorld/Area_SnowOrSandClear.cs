using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E92 RID: 7826
	public class Area_SnowOrSandClear : Area
	{
		// Token: 0x17001A98 RID: 6808
		// (get) Token: 0x0600ADC4 RID: 44484 RVA: 0x00319251 File Offset: 0x00317451
		public override string Label
		{
			get
			{
				return (ModsConfig.OdysseyActive ? "SnowAndSandClear" : "SnowClear").Translate();
			}
		}

		// Token: 0x17001A99 RID: 6809
		// (get) Token: 0x0600ADC5 RID: 44485 RVA: 0x00319270 File Offset: 0x00317470
		public override Color Color
		{
			get
			{
				return new Color(0.8f, 0.1f, 0.1f);
			}
		}

		// Token: 0x17001A9A RID: 6810
		// (get) Token: 0x0600ADC6 RID: 44486 RVA: 0x00301765 File Offset: 0x002FF965
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x0600ADC7 RID: 44487 RVA: 0x0031916A File Offset: 0x0031736A
		public Area_SnowOrSandClear()
		{
		}

		// Token: 0x0600ADC8 RID: 44488 RVA: 0x00319172 File Offset: 0x00317372
		public Area_SnowOrSandClear(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x0600ADC9 RID: 44489 RVA: 0x00319286 File Offset: 0x00317486
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_SnowClear";
		}
	}
}
