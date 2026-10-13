using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C12 RID: 15378
	public class GameSetupStep_TileElementsCore : GameSetupStep
	{
		// Token: 0x17003395 RID: 13205
		// (get) Token: 0x06014DB2 RID: 85426 RVA: 0x0063EE3C File Offset: 0x0063D03C
		public override int SeedPart
		{
			get
			{
				return 711240483;
			}
		}

		// Token: 0x06014DB3 RID: 85427 RVA: 0x0063EE43 File Offset: 0x0063D043
		public override void GenerateFresh()
		{
			Find.World.features = new WorldFeatures();
		}
	}
}
