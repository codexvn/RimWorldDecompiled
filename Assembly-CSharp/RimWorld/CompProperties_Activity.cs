using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BA5 RID: 11173
	public class CompProperties_Activity : CompProperties
	{
		// Token: 0x0600F9AB RID: 63915 RVA: 0x00489E48 File Offset: 0x00488048
		public CompProperties_Activity()
		{
			this.compClass = typeof(CompActivity);
		}

		// Token: 0x17002711 RID: 10001
		// (get) Token: 0x0600F9AC RID: 63916 RVA: 0x00489F20 File Offset: 0x00488120
		public ActivityWorker Worker
		{
			get
			{
				ActivityWorker result;
				if ((result = this.workerInt) == null)
				{
					result = (this.workerInt = (ActivityWorker)Activator.CreateInstance(this.workerClass));
				}
				return result;
			}
		}

		// Token: 0x0400A425 RID: 42021
		public float changePerDayBase = 0.1f;

		// Token: 0x0400A426 RID: 42022
		public float? changePerDayOutside;

		// Token: 0x0400A427 RID: 42023
		public float changePerDayElectroharvester = 0.15f;

		// Token: 0x0400A428 RID: 42024
		public float changePerDamage = 0.005f;

		// Token: 0x0400A429 RID: 42025
		public float warning = 0.7f;

		// Token: 0x0400A42A RID: 42026
		public bool dirtyGraphicsOnActivityChange;

		// Token: 0x0400A42B RID: 42027
		public FloatRange startingRange = FloatRange.ZeroToOne;

		// Token: 0x0400A42C RID: 42028
		public Type workerClass = typeof(PawnActivityWorker);

		// Token: 0x0400A42D RID: 42029
		public SimpleCurve activityResearchFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.5f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(0.99f, 2f),
				true
			}
		};

		// Token: 0x0400A42E RID: 42030
		public SimpleCurve damagedActivityMultiplierCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x0400A42F RID: 42031
		public bool showLetterOnActivated;

		// Token: 0x0400A430 RID: 42032
		public bool showLetterOnManualActivation;

		// Token: 0x0400A431 RID: 42033
		public bool requiresHoldingPlatform;

		// Token: 0x0400A432 RID: 42034
		public LetterDef letterDef;

		// Token: 0x0400A433 RID: 42035
		[MustTranslate]
		public string letterTitle;

		// Token: 0x0400A434 RID: 42036
		[MustTranslate]
		public string letterDesc;

		// Token: 0x0400A435 RID: 42037
		private ActivityWorker workerInt;
	}
}
