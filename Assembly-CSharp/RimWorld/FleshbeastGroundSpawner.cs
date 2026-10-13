using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A95 RID: 10901
	public class FleshbeastGroundSpawner : PawnGroundSpawner
	{
		// Token: 0x170025EB RID: 9707
		// (get) Token: 0x0600F2B4 RID: 62132 RVA: 0x004679B8 File Offset: 0x00465BB8
		protected override SoundDef SustainerSound
		{
			get
			{
				return SoundDefOf.FleshbeastDigging;
			}
		}
	}
}
