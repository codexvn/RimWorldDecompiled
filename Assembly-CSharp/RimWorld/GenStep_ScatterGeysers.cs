using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FFB RID: 8187
	public class GenStep_ScatterGeysers : GenStep_ScatterThings
	{
		// Token: 0x0600B462 RID: 46178 RVA: 0x0033C05C File Offset: 0x0033A25C
		protected override int CalculateFinalCount(Map map)
		{
			float num = map.Biome.geyserCountFactor;
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				num *= tileMutatorDef.geyserCountFactor;
			}
			return Mathf.RoundToInt((float)base.CalculateFinalCount(map) * num);
		}
	}
}
