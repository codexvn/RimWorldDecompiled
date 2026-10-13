using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002478 RID: 9336
	public class Need_Comfort : Need_Seeker
	{
		// Token: 0x17001EB4 RID: 7860
		// (get) Token: 0x0600CA88 RID: 51848 RVA: 0x003B52E7 File Offset: 0x003B34E7
		public override float CurInstantLevel
		{
			get
			{
				if (this.lastComfortUseTick >= Find.TickManager.TicksGame - 15)
				{
					return Mathf.Clamp01(this.lastComfortUsed);
				}
				return 0f;
			}
		}

		// Token: 0x17001EB5 RID: 7861
		// (get) Token: 0x0600CA89 RID: 51849 RVA: 0x003B5310 File Offset: 0x003B3510
		public ComfortCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.1f)
				{
					return ComfortCategory.Uncomfortable;
				}
				if (this.CurLevel < 0.6f)
				{
					return ComfortCategory.Normal;
				}
				if (this.CurLevel < 0.7f)
				{
					return ComfortCategory.Comfortable;
				}
				if (this.CurLevel < 0.8f)
				{
					return ComfortCategory.VeryComfortable;
				}
				if (this.CurLevel < 0.9f)
				{
					return ComfortCategory.ExtremelyComfortable;
				}
				return ComfortCategory.LuxuriantlyComfortable;
			}
		}

		// Token: 0x0600CA8A RID: 51850 RVA: 0x003B536C File Offset: 0x003B356C
		public Need_Comfort(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.9f);
		}

		// Token: 0x0600CA8B RID: 51851 RVA: 0x003B53DB File Offset: 0x003B35DB
		public void ComfortUsed(float comfort)
		{
			this.lastComfortUsed = comfort;
			this.lastComfortUseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x040089EB RID: 35307
		public float lastComfortUsed;

		// Token: 0x040089EC RID: 35308
		public int lastComfortUseTick;

		// Token: 0x040089ED RID: 35309
		private const float MinNormal = 0.1f;

		// Token: 0x040089EE RID: 35310
		private const float MinComfortable = 0.6f;

		// Token: 0x040089EF RID: 35311
		private const float MinVeryComfortable = 0.7f;

		// Token: 0x040089F0 RID: 35312
		private const float MinExtremelyComfortablee = 0.8f;

		// Token: 0x040089F1 RID: 35313
		private const float MinLuxuriantlyComfortable = 0.9f;

		// Token: 0x040089F2 RID: 35314
		public const int ComfortUseInterval = 15;
	}
}
