using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D3E RID: 7486
	public class GameCondition_GillRot : GameCondition
	{
		// Token: 0x0600A7BD RID: 42941 RVA: 0x00300E73 File Offset: 0x002FF073
		public override void Init()
		{
			base.Init();
			this.fishPopulationOffsetFactorPerDay = this.def.fishPopulationOffsetPerDay;
		}

		// Token: 0x0600A7BE RID: 42942 RVA: 0x00300E8C File Offset: 0x002FF08C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.fishPopulationOffsetFactorPerDay, "fishPopulationOffsetFactorPerDay", 0f, false);
		}

		// Token: 0x04007423 RID: 29731
		public float fishPopulationOffsetFactorPerDay;
	}
}
