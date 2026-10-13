using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FBA RID: 8122
	public class GenStep_Glaciers : GenStep_ScatterLumpsMineable
	{
		// Token: 0x0600B322 RID: 45858 RVA: 0x003353DC File Offset: 0x003335DC
		public override void Generate(Map map, GenStepParams parms)
		{
			this.forcedDefToScatter = ThingDefOf.SolidIce;
			base.Generate(map, parms);
		}
	}
}
