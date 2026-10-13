using System;

namespace Verse
{
	// Token: 0x02000647 RID: 1607
	public class PawnRenderNodeWorker_AnimalPack : PawnRenderNodeWorker
	{
		// Token: 0x06002884 RID: 10372 RVA: 0x000D6220 File Offset: 0x000D4420
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && !parms.Portrait && parms.pawn.RaceProps.Animal && parms.pawn.inventory != null && parms.pawn.inventory.innerContainer.Count > 0;
		}
	}
}
