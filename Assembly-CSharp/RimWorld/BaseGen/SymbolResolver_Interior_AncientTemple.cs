using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003991 RID: 14737
	public class SymbolResolver_Interior_AncientTemple : SymbolResolver
	{
		// Token: 0x060143CA RID: 82890 RVA: 0x0060A610 File Offset: 0x00608810
		public override void Resolve(ResolveParams rp)
		{
			List<Thing> list = ThingSetMakerDefOf.MapGen_AncientTempleContents.root.Generate();
			list.SortByDescending((Thing t) => t.MarketValue * (float)t.stackCount);
			for (int i = 0; i < list.Count; i++)
			{
				ResolveParams resolveParams = rp;
				if (ModsConfig.IdeologyActive && i == 0)
				{
					resolveParams.singleThingDef = ThingDefOf.AncientHermeticCrate;
					resolveParams.singleThingInnerThings = new List<Thing>
					{
						list[0]
					};
				}
				else
				{
					resolveParams.singleThingToSpawn = list[i];
				}
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
			if (!Find.Storyteller.difficulty.peacefulTemples)
			{
				if (ModsConfig.AnomalyActive && Rand.Chance(0.5f) && Faction.OfEntities != null)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.fleshbeastsCount = new int?(rp.fleshbeastsCount ?? SymbolResolver_Interior_AncientTemple.FleshbeastsCountRange.RandomInRange);
					BaseGen.symbolStack.Push("randomFleshbeastGroup", resolveParams2, null);
				}
				else if (Rand.Chance(0.65f) && Faction.OfMechanoids != null)
				{
					ResolveParams resolveParams3 = rp;
					resolveParams3.mechanoidsCount = new int?(rp.mechanoidsCount ?? SymbolResolver_Interior_AncientTemple.MechanoidCountRange.RandomInRange);
					BaseGen.symbolStack.Push("randomMechanoidGroup", resolveParams3, null);
				}
				else if (Faction.OfInsects != null)
				{
					ResolveParams resolveParams4 = rp;
					resolveParams4.hivesCount = new int?(rp.hivesCount ?? SymbolResolver_Interior_AncientTemple.HivesCountRange.RandomInRange);
					BaseGen.symbolStack.Push("hives", resolveParams4, null);
				}
			}
			if (rp.rect.Width >= SymbolResolver_Interior_AncientTemple.MinSizeForShrines.x && rp.rect.Height >= SymbolResolver_Interior_AncientTemple.MinSizeForShrines.z)
			{
				BaseGen.symbolStack.Push("ancientShrinesGroup", rp, null);
			}
			if (ModsConfig.IdeologyActive)
			{
				ResolveParams resolveParams5 = rp;
				resolveParams5.singleThingDef = ThingDefOf.AncientBarrel;
				BaseGen.symbolStack.Push("edgeThing", resolveParams5, null);
			}
		}

		// Token: 0x0400DFA4 RID: 57252
		private const float FleshbeastsChance = 0.5f;

		// Token: 0x0400DFA5 RID: 57253
		private const float MechanoidsChance = 0.65f;

		// Token: 0x0400DFA6 RID: 57254
		private static readonly IntRange MechanoidCountRange = new IntRange(1, 5);

		// Token: 0x0400DFA7 RID: 57255
		private static readonly IntRange HivesCountRange = new IntRange(1, 2);

		// Token: 0x0400DFA8 RID: 57256
		private static readonly IntRange FleshbeastsCountRange = new IntRange(2, 5);

		// Token: 0x0400DFA9 RID: 57257
		private static readonly IntVec2 MinSizeForShrines = new IntVec2(4, 3);
	}
}
