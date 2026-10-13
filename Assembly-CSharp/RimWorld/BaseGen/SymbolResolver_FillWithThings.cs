using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200395C RID: 14684
	public class SymbolResolver_FillWithThings : SymbolResolver
	{
		// Token: 0x06014327 RID: 82727 RVA: 0x006065B0 File Offset: 0x006047B0
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.singleThingToSpawn != null)
			{
				return false;
			}
			if (rp.singleThingDef != null)
			{
				Rot4 rot = rp.thingRot ?? Rot4.North;
				IntVec3 zero = IntVec3.Zero;
				IntVec2 size = rp.singleThingDef.size;
				GenAdj.AdjustForRotation(ref zero, ref size, rot);
				if (rp.rect.Width < size.x || rp.rect.Height < size.z)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06014328 RID: 82728 RVA: 0x00606640 File Offset: 0x00604840
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef;
			if ((thingDef = rp.singleThingDef) == null)
			{
				thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
				where x.IsWeapon || x.IsMedicine || x.IsDrug
				select x).RandomElement<ThingDef>();
			}
			ThingDef thingDef2 = thingDef;
			Rot4 rot = rp.thingRot ?? Rot4.North;
			IntVec3 zero = IntVec3.Zero;
			IntVec2 size = thingDef2.size;
			int num = rp.fillWithThingsPadding.GetValueOrDefault();
			if (num < 0)
			{
				num = 0;
			}
			GenAdj.AdjustForRotation(ref zero, ref size, rot);
			if (size.x <= 0 || size.z <= 0)
			{
				Log.Error("Thing has 0 size.");
				return;
			}
			for (int i = rp.rect.minX; i <= rp.rect.maxX - size.x + 1; i += size.x + num)
			{
				for (int j = rp.rect.minZ; j <= rp.rect.maxZ - size.z + 1; j += size.z + num)
				{
					ResolveParams resolveParams = rp;
					resolveParams.rect = new CellRect(i, j, size.x, size.z);
					resolveParams.singleThingDef = thingDef2;
					resolveParams.thingRot = new Rot4?(rot);
					BaseGen.symbolStack.Push("thing", resolveParams, null);
				}
			}
			BaseGen.symbolStack.Push("clear", rp, null);
		}
	}
}
