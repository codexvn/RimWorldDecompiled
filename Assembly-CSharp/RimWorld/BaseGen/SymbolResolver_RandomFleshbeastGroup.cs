using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x0200396E RID: 14702
	public class SymbolResolver_RandomFleshbeastGroup : SymbolResolver
	{
		// Token: 0x06014365 RID: 82789 RVA: 0x006079B0 File Offset: 0x00605BB0
		public override void Resolve(ResolveParams rp)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			int num = rp.fleshbeastsCount ?? SymbolResolver_RandomFleshbeastGroup.DefaultFleshbeastsCountRange.RandomInRange;
			Lord lord = rp.singlePawnLord;
			if (lord == null && num > 0)
			{
				lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_FleshbeastAssault(), BaseGen.globalSettings.map, null);
			}
			for (int i = 0; i < num; i++)
			{
				PawnKindDef singlePawnKindDef = rp.singlePawnKindDef ?? FleshbeastUtility.AllFleshbeasts.RandomElement<PawnKindDef>();
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnKindDef = singlePawnKindDef;
				resolveParams.singlePawnLord = lord;
				resolveParams.faction = Faction.OfEntities;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
		}

		// Token: 0x0400DF5E RID: 57182
		private static readonly IntRange DefaultFleshbeastsCountRange = new IntRange(2, 5);
	}
}
