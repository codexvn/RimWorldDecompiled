using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B20 RID: 11040
	public class DeadPlant : Plant
	{
		// Token: 0x170026A2 RID: 9890
		// (get) Token: 0x0600F6B8 RID: 63160 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026A3 RID: 9891
		// (get) Token: 0x0600F6B9 RID: 63161 RVA: 0x00006AAC File Offset: 0x00004CAC
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170026A4 RID: 9892
		// (get) Token: 0x0600F6BA RID: 63162 RVA: 0x00006AAC File Offset: 0x00004CAC
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170026A5 RID: 9893
		// (get) Token: 0x0600F6BB RID: 63163 RVA: 0x00066AFC File Offset: 0x00064CFC
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170026A6 RID: 9894
		// (get) Token: 0x0600F6BC RID: 63164 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		// Token: 0x0600F6BD RID: 63165 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		// Token: 0x0600F6BE RID: 63166 RVA: 0x0047A800 File Offset: 0x00478A00
		public override string GetInspectString()
		{
			float statValue = this.GetStatValue(StatDefOf.DeteriorationRate, true, -1);
			if (statValue > 0f)
			{
				int numTicks = Mathf.RoundToInt((float)this.HitPoints / statValue * 60000f);
				return "DeteriorateInDuration".Translate(numTicks.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
			}
			return "";
		}

		// Token: 0x0600F6BF RID: 63167 RVA: 0x000026EA File Offset: 0x000008EA
		public override void CropBlighted()
		{
		}
	}
}
