using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002007 RID: 8199
	public class GenStep_ScenParts : GenStep
	{
		// Token: 0x17001B83 RID: 7043
		// (get) Token: 0x0600B49E RID: 46238 RVA: 0x0033D1C0 File Offset: 0x0033B3C0
		public override int SeedPart
		{
			get
			{
				return 1561683158;
			}
		}

		// Token: 0x0600B49F RID: 46239 RVA: 0x0033D1C7 File Offset: 0x0033B3C7
		public override void Generate(Map map, GenStepParams parms)
		{
			Find.Scenario.GenerateIntoMap(map);
		}
	}
}
