using System;

namespace RimWorld.SketchGen
{
	// Token: 0x020038D0 RID: 14544
	public class SketchResolver_MechCluster : SketchResolver
	{
		// Token: 0x06014130 RID: 82224 RVA: 0x005F815A File Offset: 0x005F635A
		protected override void ResolveInt(SketchResolveParams parms)
		{
			MechClusterGenerator.ResolveSketch(parms);
		}

		// Token: 0x06014131 RID: 82225 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}
	}
}
