using System;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02003C11 RID: 15377
	public class GameSetupStep_Grids : GameSetupStep
	{
		// Token: 0x17003394 RID: 13204
		// (get) Token: 0x06014DAE RID: 85422 RVA: 0x0063EDCE File Offset: 0x0063CFCE
		public override int SeedPart
		{
			get
			{
				return 83469557;
			}
		}

		// Token: 0x06014DAF RID: 85423 RVA: 0x0063EDD5 File Offset: 0x0063CFD5
		public override void GenerateFresh()
		{
			LongEventHandler.ExecuteWhenFinished(new Action(WorldTerrainColliderManager.ClearCache));
			Find.World.grid = new WorldGrid();
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
			Find.WorldGrid.GenerateFreshWorld();
		}

		// Token: 0x06014DB0 RID: 85424 RVA: 0x0063EE15 File Offset: 0x0063D015
		public override void GenerateFromScribe()
		{
			LongEventHandler.ExecuteWhenFinished(new Action(WorldTerrainColliderManager.ClearCache));
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
		}
	}
}
