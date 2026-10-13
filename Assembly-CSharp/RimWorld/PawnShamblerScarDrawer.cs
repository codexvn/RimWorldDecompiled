using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022E1 RID: 8929
	public class PawnShamblerScarDrawer : PawnScarDrawer
	{
		// Token: 0x17001DB5 RID: 7605
		// (get) Token: 0x0600C21E RID: 49694 RVA: 0x00383E1C File Offset: 0x0038201C
		protected override string ScarTexturePath
		{
			get
			{
				return "Things/Pawn/Overlays/ShamblerScars/ShamblerScarOverlay";
			}
		}

		// Token: 0x0600C21F RID: 49695 RVA: 0x00383E23 File Offset: 0x00382023
		public PawnShamblerScarDrawer(Pawn pawn) : base(pawn)
		{
		}
	}
}
