using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D5C RID: 11612
	public class FocusStrengthOffset_NearbyGraves : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x0601050A RID: 66826 RVA: 0x004B6008 File Offset: 0x004B4208
		protected override float OffsetForBuilding(Thing b)
		{
			float num = base.OffsetFor(b.def);
			Building_Grave building_Grave = b as Building_Grave;
			if (building_Grave != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike)
			{
				num += this.focusPerFullGrave;
			}
			return num;
		}

		// Token: 0x0601050B RID: 66827 RVA: 0x004B6055 File Offset: 0x004B4255
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * (this.focusPerFullGrave + base.MaxOffsetPerBuilding);
		}

		// Token: 0x0601050C RID: 66828 RVA: 0x004B606C File Offset: 0x004B426C
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			return this.explanationKey.Translate(value, this.maxBuildings, base.MaxOffsetPerBuilding.ToString("0%"), (base.MaxOffsetPerBuilding + this.focusPerFullGrave).ToString("0%")) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x0601050D RID: 66829 RVA: 0x004B6110 File Offset: 0x004B4310
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.explanationKeyAbstract.Translate(this.maxBuildings, base.MaxOffsetPerBuilding.ToString("0%"), (base.MaxOffsetPerBuilding + this.focusPerFullGrave).ToString("0%")) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x0400AA3E RID: 43582
		public float focusPerFullGrave;
	}
}
