using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003922 RID: 14626
	public class SymbolResolver_Filth : SymbolResolver
	{
		// Token: 0x0601427A RID: 82554 RVA: 0x00601831 File Offset: 0x005FFA31
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.filthDef != null && rp.filthDensity != null;
		}

		// Token: 0x0601427B RID: 82555 RVA: 0x00601854 File Offset: 0x005FFA54
		public override void Resolve(ResolveParams rp)
		{
			foreach (IntVec3 intVec in rp.rect)
			{
				if (this.CanPlaceFilth(intVec, rp))
				{
					float num;
					for (num = rp.filthDensity.Value.RandomInRange; num > 1f; num -= 1f)
					{
						FilthMaker.TryMakeFilth(intVec, BaseGen.globalSettings.map, rp.filthDef, 1, FilthSourceFlags.None, true);
					}
					if (Rand.Chance(num))
					{
						FilthMaker.TryMakeFilth(intVec, BaseGen.globalSettings.map, rp.filthDef, 1, FilthSourceFlags.None, true);
					}
				}
			}
		}

		// Token: 0x0601427C RID: 82556 RVA: 0x00601910 File Offset: 0x005FFB10
		private bool CanPlaceFilth(IntVec3 cell, ResolveParams rp)
		{
			return rp.ignoreDoorways == null || !rp.ignoreDoorways.Value || cell.GetDoor(BaseGen.globalSettings.map) == null;
		}
	}
}
