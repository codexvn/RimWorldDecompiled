using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200392F RID: 14639
	public class SymbolResolver_PawnGroup : SymbolResolver
	{
		// Token: 0x060142A2 RID: 82594 RVA: 0x00602184 File Offset: 0x00600384
		public static PawnGroupMakerParms GetGroupMakerParms(Map map, ResolveParams rp)
		{
			PawnGroupMakerParms pawnGroupMakerParms;
			if ((pawnGroupMakerParms = rp.pawnGroupMakerParams) == null)
			{
				PawnGroupMakerParms pawnGroupMakerParms2 = new PawnGroupMakerParms();
				pawnGroupMakerParms2.tile = map.Tile;
				pawnGroupMakerParms2.faction = Find.FactionManager.RandomRaidableEnemyFaction(false, false, true, TechLevel.Undefined);
				pawnGroupMakerParms = pawnGroupMakerParms2;
				pawnGroupMakerParms2.points = 250f;
			}
			PawnGroupMakerParms pawnGroupMakerParms3 = pawnGroupMakerParms;
			pawnGroupMakerParms3.groupKind = (rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Combat);
			return pawnGroupMakerParms3;
		}

		// Token: 0x060142A3 RID: 82595 RVA: 0x006021E0 File Offset: 0x006003E0
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			return rp.rect.Cells.Any((IntVec3 x) => x.Standable(BaseGen.globalSettings.map));
		}

		// Token: 0x060142A4 RID: 82596 RVA: 0x00602230 File Offset: 0x00600430
		public override void Resolve(ResolveParams rp)
		{
			PawnGroupMakerParms groupMakerParms = SymbolResolver_PawnGroup.GetGroupMakerParms(BaseGen.globalSettings.map, rp);
			List<PawnKindDef> list = new List<PawnKindDef>();
			foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(groupMakerParms, true))
			{
				list.Add(pawn.kindDef);
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnToSpawn = pawn;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
		}

		// Token: 0x0400DEFD RID: 57085
		private const float DefaultPoints = 250f;
	}
}
