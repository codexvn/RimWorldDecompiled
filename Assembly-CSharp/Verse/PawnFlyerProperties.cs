using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000247 RID: 583
	public class PawnFlyerProperties
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x00045F81 File Offset: 0x00044181
		public PawnFlyerWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnFlyerWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					});
				}
				return this.workerInt;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x00045FB1 File Offset: 0x000441B1
		public AnimationCurve ProgressCurve
		{
			get
			{
				if (this.progressCurveInt == null && this.progressCurve != null)
				{
					this.progressCurveInt = this.progressCurve.ToAnimationCurve();
				}
				return this.progressCurveInt;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x00045FDA File Offset: 0x000441DA
		public Material ShadowMaterial
		{
			get
			{
				if (this.cachedShadowMaterial == null && !this.shadowTexPath.NullOrEmpty())
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom(this.shadowTexPath, ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		// Token: 0x04000A25 RID: 2597
		private Type workerClass = typeof(PawnFlyerWorker);

		// Token: 0x04000A26 RID: 2598
		private SimpleCurve progressCurve;

		// Token: 0x04000A27 RID: 2599
		[NoTranslate]
		private string shadowTexPath = "Things/Skyfaller/SkyfallerShadowCircle";

		// Token: 0x04000A28 RID: 2600
		public float flightDurationMin = 0.5f;

		// Token: 0x04000A29 RID: 2601
		public float flightSpeed = 12f;

		// Token: 0x04000A2A RID: 2602
		public float heightFactor = 2f;

		// Token: 0x04000A2B RID: 2603
		public IntRange stunDurationTicksRange = IntRange.Zero;

		// Token: 0x04000A2C RID: 2604
		private PawnFlyerWorker workerInt;

		// Token: 0x04000A2D RID: 2605
		private AnimationCurve progressCurveInt;

		// Token: 0x04000A2E RID: 2606
		private Material cachedShadowMaterial;
	}
}
