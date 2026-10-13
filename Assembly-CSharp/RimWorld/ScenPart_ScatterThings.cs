using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020028F2 RID: 10482
	public abstract class ScenPart_ScatterThings : ScenPart_ThingCount
	{
		// Token: 0x170022C7 RID: 8903
		// (get) Token: 0x0600E538 RID: 58680
		protected abstract bool NearPlayerStart { get; }

		// Token: 0x0600E539 RID: 58681 RVA: 0x0042E7B8 File Offset: 0x0042C9B8
		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			new GenStep_ScatterThings
			{
				nearPlayerStart = this.NearPlayerStart,
				allowFoggedPositions = !this.NearPlayerStart,
				thingDef = this.thingDef,
				stuff = this.stuff,
				count = this.count,
				spotMustBeStandable = true,
				minSpacing = 5f,
				quality = this.quality,
				clusterSize = ((this.thingDef.category == ThingCategory.Building) ? 1 : 4),
				allowRoofed = this.allowRoofed,
				minify = true
			}.Generate(map, default(GenStepParams));
		}

		// Token: 0x0600E53A RID: 58682 RVA: 0x0042E869 File Offset: 0x0042CA69
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.allowRoofed, "allowRoofed", false, false);
		}

		// Token: 0x0600E53B RID: 58683 RVA: 0x0042E883 File Offset: 0x0042CA83
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ (this.allowRoofed ? 1 : 0);
		}

		// Token: 0x04009960 RID: 39264
		public bool allowRoofed = true;
	}
}
