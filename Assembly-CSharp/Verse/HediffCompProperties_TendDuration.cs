using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200070D RID: 1805
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06002C14 RID: 11284 RVA: 0x000E5173 File Offset: 0x000E3373
		public bool TendIsPermanent
		{
			get
			{
				return this.baseTendDurationHours < 0f;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06002C15 RID: 11285 RVA: 0x000E5182 File Offset: 0x000E3382
		public int TendTicksFull
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksFull on permanent-tend Hediff.", 6163263);
				}
				return Mathf.RoundToInt((this.baseTendDurationHours + this.tendOverlapHours) * 2500f);
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002C16 RID: 11286 RVA: 0x000E51B3 File Offset: 0x000E33B3
		public int TendTicksBase
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksBase on permanent-tend Hediff.", 61621263);
				}
				return Mathf.RoundToInt(this.baseTendDurationHours * 2500f);
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002C17 RID: 11287 RVA: 0x000E51DD File Offset: 0x000E33DD
		public int TendTicksOverlap
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksOverlap on permanent-tend Hediff.", 1963263);
				}
				return Mathf.RoundToInt(this.tendOverlapHours * 2500f);
			}
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x000E5207 File Offset: 0x000E3407
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}

		// Token: 0x04002369 RID: 9065
		private float baseTendDurationHours = -1f;

		// Token: 0x0400236A RID: 9066
		private float tendOverlapHours = 3f;

		// Token: 0x0400236B RID: 9067
		public bool tendAllAtOnce;

		// Token: 0x0400236C RID: 9068
		public int disappearsAtTotalTendQuality = -1;

		// Token: 0x0400236D RID: 9069
		public float severityPerDayTended;

		// Token: 0x0400236E RID: 9070
		public bool showTendQuality = true;

		// Token: 0x0400236F RID: 9071
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell;

		// Token: 0x04002370 RID: 9072
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner;

		// Token: 0x04002371 RID: 9073
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell;
	}
}
