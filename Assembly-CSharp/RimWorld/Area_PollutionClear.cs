using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E93 RID: 7827
	public class Area_PollutionClear : Area
	{
		// Token: 0x17001A9B RID: 6811
		// (get) Token: 0x0600ADCA RID: 44490 RVA: 0x003192A2 File Offset: 0x003174A2
		public override string Label
		{
			get
			{
				return "PollutionClear".Translate();
			}
		}

		// Token: 0x17001A9C RID: 6812
		// (get) Token: 0x0600ADCB RID: 44491 RVA: 0x003192B3 File Offset: 0x003174B3
		public override Color Color
		{
			get
			{
				return new Color(0.1f, 0.8f, 0.1f);
			}
		}

		// Token: 0x17001A9D RID: 6813
		// (get) Token: 0x0600ADCC RID: 44492 RVA: 0x00301765 File Offset: 0x002FF965
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x0600ADCD RID: 44493 RVA: 0x0031916A File Offset: 0x0031736A
		public Area_PollutionClear()
		{
		}

		// Token: 0x0600ADCE RID: 44494 RVA: 0x00319172 File Offset: 0x00317372
		public Area_PollutionClear(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x0600ADCF RID: 44495 RVA: 0x003192C9 File Offset: 0x003174C9
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_PollutionClear";
		}
	}
}
