using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022E2 RID: 8930
	public class PawnScariaSoreDrawer : PawnScarDrawer
	{
		// Token: 0x17001DB6 RID: 7606
		// (get) Token: 0x0600C220 RID: 49696 RVA: 0x00383E2C File Offset: 0x0038202C
		protected override string ScarTexturePath
		{
			get
			{
				return "Things/Pawn/Overlays/Scaria/ScariaSoresOverlay";
			}
		}

		// Token: 0x17001DB7 RID: 7607
		// (get) Token: 0x0600C221 RID: 49697 RVA: 0x0031CA56 File Offset: 0x0031AC56
		protected override float ScaleFactor
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x0600C222 RID: 49698 RVA: 0x00383E23 File Offset: 0x00382023
		public PawnScariaSoreDrawer(Pawn pawn) : base(pawn)
		{
		}
	}
}
