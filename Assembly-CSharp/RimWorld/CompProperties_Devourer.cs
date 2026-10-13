using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C59 RID: 11353
	public class CompProperties_Devourer : CompProperties
	{
		// Token: 0x0600FE92 RID: 65170 RVA: 0x0049CA54 File Offset: 0x0049AC54
		public CompProperties_Devourer()
		{
			this.compClass = typeof(CompDevourer);
		}

		// Token: 0x0400A6C7 RID: 42695
		[MustTranslate]
		public string messageDigested;

		// Token: 0x0400A6C8 RID: 42696
		[MustTranslate]
		public string messageEmerged;

		// Token: 0x0400A6C9 RID: 42697
		[MustTranslate]
		public string messageEmergedCorpse;

		// Token: 0x0400A6CA RID: 42698
		[MustTranslate]
		public string messageDigestionCompleted;

		// Token: 0x0400A6CB RID: 42699
		[MustTranslate]
		public string digestingInspector;

		// Token: 0x0400A6CC RID: 42700
		public int completeDigestionDamage = 125;

		// Token: 0x0400A6CD RID: 42701
		public SimpleCurve bodySizeDigestTimeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 10f),
				true
			},
			{
				new CurvePoint(1f, 60f),
				true
			},
			{
				new CurvePoint(3.5f, 90f),
				true
			}
		};

		// Token: 0x0400A6CE RID: 42702
		public SimpleCurve timeDamageCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 5f),
				true
			},
			{
				new CurvePoint(60f, 35f),
				true
			}
		};
	}
}
