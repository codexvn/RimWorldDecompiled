using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C13 RID: 15379
	public class GameSetupStep_TileElementsOdyssey : GameSetupStep
	{
		// Token: 0x17003396 RID: 13206
		// (get) Token: 0x06014DB5 RID: 85429 RVA: 0x0063EE54 File Offset: 0x0063D054
		public override int SeedPart
		{
			get
			{
				return 689393;
			}
		}

		// Token: 0x06014DB6 RID: 85430 RVA: 0x0063EE5B File Offset: 0x0063D05B
		public override void GenerateFresh()
		{
			Find.World.landmarks = new WorldLandmarks();
		}
	}
}
