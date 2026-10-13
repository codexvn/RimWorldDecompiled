using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020020EF RID: 8431
	public class TileMutatorWorker_IncreasedPollution : TileMutatorWorker
	{
		// Token: 0x0600B826 RID: 47142 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_IncreasedPollution(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B827 RID: 47143 RVA: 0x00351F6C File Offset: 0x0035016C
		public override void OnAddedToTile(PlanetTile tile)
		{
			Tile tile2 = tile.Tile;
			tile2.pollution = Mathf.Clamp01(tile2.pollution + TileMutatorWorker_IncreasedPollution.PollutionOffsetRange.RandomInRange);
		}

		// Token: 0x04007EEA RID: 32490
		private static readonly FloatRange PollutionOffsetRange = new FloatRange(0.1f, 0.2f);
	}
}
