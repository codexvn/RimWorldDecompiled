using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003931 RID: 14641
	public class SymbolResolver_PrisonerBed : SymbolResolver
	{
		// Token: 0x060142A9 RID: 82601 RVA: 0x006022D4 File Offset: 0x006004D4
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			Action<Thing> prevPostThingSpawn = resolveParams.postThingSpawn;
			resolveParams.postThingSpawn = delegate(Thing x)
			{
				if (prevPostThingSpawn != null)
				{
					prevPostThingSpawn(x);
				}
				Building_Bed building_Bed = x as Building_Bed;
				if (building_Bed != null)
				{
					building_Bed.ForPrisoners = true;
				}
			};
			BaseGen.symbolStack.Push("bed", resolveParams, null);
		}
	}
}
