using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000498 RID: 1176
	public class GenStep_RandomSelector : GenStep
	{
		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001EEA RID: 7914 RVA: 0x000A1977 File Offset: 0x0009FB77
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000A1980 File Offset: 0x0009FB80
		public override void Generate(Map map, GenStepParams parms)
		{
			RandomGenStepSelectorOption randomGenStepSelectorOption = this.options.RandomElementByWeight((RandomGenStepSelectorOption opt) => opt.weight);
			if (randomGenStepSelectorOption.genStep != null)
			{
				randomGenStepSelectorOption.genStep.Generate(map, parms);
			}
			if (randomGenStepSelectorOption.def != null)
			{
				randomGenStepSelectorOption.def.genStep.Generate(map, parms);
			}
		}

		// Token: 0x04001A77 RID: 6775
		public List<RandomGenStepSelectorOption> options;
	}
}
