using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200079D RID: 1949
	public class SummaryHealthHandler
	{
		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002FC7 RID: 12231 RVA: 0x000F56A4 File Offset: 0x000F38A4
		public float SummaryHealthPercent
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.dirty)
				{
					List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
					float num = 1f;
					for (int i = 0; i < hediffs.Count; i++)
					{
						if (!(hediffs[i] is Hediff_MissingPart))
						{
							float num2 = Mathf.Min(hediffs[i].SummaryHealthPercentImpact, 0.95f);
							num *= 1f - num2;
						}
					}
					List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
					for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
					{
						float num3 = Mathf.Min(missingPartsCommonAncestors[j].SummaryHealthPercentImpact, 0.95f);
						num *= 1f - num3;
					}
					this.cachedSummaryHealthPercent = Mathf.Clamp(num, 0.05f, 1f);
					this.dirty = false;
				}
				return this.cachedSummaryHealthPercent;
			}
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000F579F File Offset: 0x000F399F
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000F57C0 File Offset: 0x000F39C0
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}

		// Token: 0x0400251C RID: 9500
		private Pawn pawn;

		// Token: 0x0400251D RID: 9501
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x0400251E RID: 9502
		private bool dirty = true;
	}
}
