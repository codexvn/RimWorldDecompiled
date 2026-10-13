using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200247E RID: 9342
	public class Need_Indoors : Need
	{
		// Token: 0x17001EC8 RID: 7880
		// (get) Token: 0x0600CAB0 RID: 51888 RVA: 0x003B5DBE File Offset: 0x003B3FBE
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17001EC9 RID: 7881
		// (get) Token: 0x0600CAB1 RID: 51889 RVA: 0x003B5DC9 File Offset: 0x003B3FC9
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				return Math.Sign(this.lastEffectiveDelta);
			}
		}

		// Token: 0x17001ECA RID: 7882
		// (get) Token: 0x0600CAB2 RID: 51890 RVA: 0x003B5DE0 File Offset: 0x003B3FE0
		public IndoorsCategory CurCategory
		{
			get
			{
				if (this.CurLevel > Need_Indoors.Thresholds[0])
				{
					return IndoorsCategory.ComfortablyIndoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[1])
				{
					return IndoorsCategory.JustOutdoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[2])
				{
					return IndoorsCategory.Outdoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[3])
				{
					return IndoorsCategory.LongOutdoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[4])
				{
					return IndoorsCategory.VeryLongOutdoors;
				}
				return IndoorsCategory.BrutalOutdoors;
			}
		}

		// Token: 0x17001ECB RID: 7883
		// (get) Token: 0x0600CAB3 RID: 51891 RVA: 0x003B5E43 File Offset: 0x003B4043
		private bool Disabled
		{
			get
			{
				return this.pawn.Dead || this.pawn.needs.PrefersOutdoors;
			}
		}

		// Token: 0x0600CAB4 RID: 51892 RVA: 0x003B5E64 File Offset: 0x003B4064
		public Need_Indoors(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>(Need_Indoors.Thresholds);
		}

		// Token: 0x0600CAB5 RID: 51893 RVA: 0x003B5485 File Offset: 0x003B3685
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x0600CAB6 RID: 51894 RVA: 0x003B5E80 File Offset: 0x003B4080
		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.CurLevel = 1f;
				return;
			}
			if (this.IsFrozen)
			{
				return;
			}
			bool flag = !this.pawn.Spawned || this.pawn.Position.UsesOutdoorTemperature(this.pawn.Map);
			RoofDef roofDef = this.pawn.Spawned ? this.pawn.Position.GetRoof(this.pawn.Map) : null;
			float curLevel = this.CurLevel;
			float num;
			if (!flag)
			{
				if (roofDef == null)
				{
					num = 0f;
				}
				else if (!roofDef.isThickRoof)
				{
					num = 1f;
				}
				else
				{
					num = 2f;
				}
			}
			else if (roofDef == null)
			{
				num = -0.25f;
			}
			else if (roofDef.isThickRoof)
			{
				num = 0f;
			}
			else
			{
				num = -0.25f;
			}
			num *= 0.0025f;
			this.CurLevel = ((num < 0f) ? Mathf.Min(this.CurLevel, this.CurLevel + num) : Mathf.Min(this.CurLevel + num, 1f));
			this.lastEffectiveDelta = this.CurLevel - curLevel;
		}

		// Token: 0x04008A0D RID: 35341
		private static readonly float[] Thresholds = new float[]
		{
			0.8f,
			0.6f,
			0.4f,
			0.2f,
			0.05f
		};

		// Token: 0x04008A0E RID: 35342
		private const float Delta_Indoors_ThickRoof = 2f;

		// Token: 0x04008A0F RID: 35343
		private const float Delta_Indoors_ThinRoof = 1f;

		// Token: 0x04008A10 RID: 35344
		private const float Delta_Indoors_NoRoof = 0f;

		// Token: 0x04008A11 RID: 35345
		private const float Delta_Outdoors_ThickRoof = 0f;

		// Token: 0x04008A12 RID: 35346
		private const float Delta_Outdoors_ThinRoof = -0.25f;

		// Token: 0x04008A13 RID: 35347
		private const float Delta_Outdoors_NoRoof = -0.25f;

		// Token: 0x04008A14 RID: 35348
		private float lastEffectiveDelta;
	}
}
