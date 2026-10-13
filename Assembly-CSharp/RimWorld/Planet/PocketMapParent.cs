using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DB8 RID: 15800
	public class PocketMapParent : MapParent
	{
		// Token: 0x17003592 RID: 13714
		// (get) Token: 0x0601589A RID: 88218 RVA: 0x006737FB File Offset: 0x006719FB
		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return this.mapGenerator;
			}
		}

		// Token: 0x0601589C RID: 88220 RVA: 0x00673803 File Offset: 0x00671A03
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.sourceMap, "sourceMap", false);
			Scribe_Defs.Look<MapGeneratorDef>(ref this.mapGenerator, "mapGenerator");
		}

		// Token: 0x0400EF88 RID: 61320
		public Map sourceMap;

		// Token: 0x0400EF89 RID: 61321
		public MapGeneratorDef mapGenerator;
	}
}
