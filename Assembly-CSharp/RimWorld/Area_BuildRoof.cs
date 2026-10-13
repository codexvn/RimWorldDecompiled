using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E90 RID: 7824
	public class Area_BuildRoof : Area
	{
		// Token: 0x17001A92 RID: 6802
		// (get) Token: 0x0600ADB8 RID: 44472 RVA: 0x003191BD File Offset: 0x003173BD
		public override string Label
		{
			get
			{
				return "BuildRoof".Translate();
			}
		}

		// Token: 0x17001A93 RID: 6803
		// (get) Token: 0x0600ADB9 RID: 44473 RVA: 0x003191CE File Offset: 0x003173CE
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.9f, 0.5f);
			}
		}

		// Token: 0x17001A94 RID: 6804
		// (get) Token: 0x0600ADBA RID: 44474 RVA: 0x003191E4 File Offset: 0x003173E4
		public override int ListPriority
		{
			get
			{
				return 9000;
			}
		}

		// Token: 0x0600ADBB RID: 44475 RVA: 0x0031916A File Offset: 0x0031736A
		public Area_BuildRoof()
		{
		}

		// Token: 0x0600ADBC RID: 44476 RVA: 0x00319172 File Offset: 0x00317372
		public Area_BuildRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x0600ADBD RID: 44477 RVA: 0x003191EB File Offset: 0x003173EB
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_BuildRoof";
		}
	}
}
