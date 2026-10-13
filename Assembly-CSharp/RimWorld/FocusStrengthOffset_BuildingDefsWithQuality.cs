using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D5B RID: 11611
	public class FocusStrengthOffset_BuildingDefsWithQuality : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x06010505 RID: 66821 RVA: 0x004B5E5C File Offset: 0x004B405C
		protected override float OffsetForBuilding(Thing b)
		{
			QualityCategory qualityCategory;
			if (b.TryGetQuality(out qualityCategory))
			{
				return this.focusPerQuality.Evaluate((float)qualityCategory);
			}
			return 0f;
		}

		// Token: 0x06010506 RID: 66822 RVA: 0x004B5E88 File Offset: 0x004B4088
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			string value2 = this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y.ToString("0%");
			return this.explanationKey.Translate(value, this.maxBuildings, value2) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x06010507 RID: 66823 RVA: 0x004B5F34 File Offset: 0x004B4134
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			string value = this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y.ToString("0%");
			return this.explanationKeyAbstract.Translate(this.maxBuildings, value) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x06010508 RID: 66824 RVA: 0x004B5FC0 File Offset: 0x004B41C0
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y;
		}

		// Token: 0x0400AA3D RID: 43581
		public SimpleCurve focusPerQuality;
	}
}
