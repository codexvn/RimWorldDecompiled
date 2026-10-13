using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D43 RID: 7491
	public class GameCondition_HeatVentHeat : GameCondition
	{
		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x0600A7D1 RID: 42961 RVA: 0x001304E5 File Offset: 0x0012E6E5
		private int TargetTempOffset
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x0600A7D2 RID: 42962 RVA: 0x003010BF File Offset: 0x002FF2BF
		public override int TransitionTicks
		{
			get
			{
				return 2500;
			}
		}

		// Token: 0x0600A7D3 RID: 42963 RVA: 0x003010C6 File Offset: 0x002FF2C6
		public override void PostMake()
		{
			base.PostMake();
			this.tempOffset = 0f;
		}

		// Token: 0x0600A7D4 RID: 42964 RVA: 0x003010D9 File Offset: 0x002FF2D9
		public override float TemperatureOffset()
		{
			return this.tempOffset;
		}

		// Token: 0x0600A7D5 RID: 42965 RVA: 0x003010E1 File Offset: 0x002FF2E1
		public override void GameConditionTick()
		{
			this.tempOffset += Mathf.Sign((float)this.TargetTempOffset - this.tempOffset) * 0.001f;
		}

		// Token: 0x0600A7D6 RID: 42966 RVA: 0x00301109 File Offset: 0x002FF309
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.tempOffset, "tempOffset", 0f, false);
		}

		// Token: 0x0400742D RID: 29741
		private const float TempChangePerTick = 0.001f;

		// Token: 0x0400742E RID: 29742
		private const float DefaultTargetTemp = 20f;

		// Token: 0x0400742F RID: 29743
		private float tempOffset;
	}
}
