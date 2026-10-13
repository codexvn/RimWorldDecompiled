using System;
using RimWorld;
using Verse;

// Token: 0x02000016 RID: 22
public class LayoutWorkerComplex_Ancient : LayoutWorkerComplex
{
	// Token: 0x06000056 RID: 86 RVA: 0x000036F0 File Offset: 0x000018F0
	public LayoutWorkerComplex_Ancient(LayoutDef def) : base(def)
	{
	}

	// Token: 0x06000057 RID: 87 RVA: 0x000036FC File Offset: 0x000018FC
	public override Faction GetFixedHostileFactionForThreats()
	{
		if (!Rand.Chance(base.Def.fixedHostileFactionChance))
		{
			return null;
		}
		if (Faction.OfInsects != null && Faction.OfMechanoids != null)
		{
			if (!Rand.Bool)
			{
				return Faction.OfMechanoids;
			}
			return Faction.OfInsects;
		}
		else
		{
			if (Faction.OfInsects != null)
			{
				return Faction.OfInsects;
			}
			return Faction.OfMechanoids;
		}
	}
}
