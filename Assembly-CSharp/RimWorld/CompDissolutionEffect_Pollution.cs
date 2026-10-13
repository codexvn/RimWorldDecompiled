using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002EE1 RID: 12001
	public class CompDissolutionEffect_Pollution : CompDissolutionEffect
	{
		// Token: 0x17002BB8 RID: 11192
		// (get) Token: 0x06010F1B RID: 69403 RVA: 0x004E05B8 File Offset: 0x004DE7B8
		public CompProperties_DissolutionEffectPollution Props
		{
			get
			{
				return (CompProperties_DissolutionEffectPollution)this.props;
			}
		}

		// Token: 0x06010F1C RID: 69404 RVA: 0x004E05C8 File Offset: 0x004DE7C8
		public override void DoDissolutionEffectMap(int amount)
		{
			PollutionUtility.GrowPollutionAt(this.parent.PositionHeld, this.parent.MapHeld, amount * this.Props.cellsToPollutePerDissolution, null, false, null);
			FleckMaker.Static(this.parent.PositionHeld, this.parent.MapHeld, FleckDefOf.Fleck_WastePackDissolutionSource, 1f);
			SoundDefOf.WastepackDissolution.PlayOneShot(this.parent);
		}

		// Token: 0x06010F1D RID: 69405 RVA: 0x004E063C File Offset: 0x004DE83C
		public override void DoDissolutionEffectWorld(int amount, PlanetTile tileId)
		{
			if (!tileId.Valid)
			{
				return;
			}
			Tile tile = Find.World.grid[tileId];
			if (tile == null)
			{
				return;
			}
			float num = this.Props.tilePollutionPerDissolution * (float)amount;
			if (tile.WaterCovered)
			{
				num *= this.Props.waterTilePollutionFactor;
			}
			CompDissolutionEffect_Pollution.WorldPollutionEvent item = new CompDissolutionEffect_Pollution.WorldPollutionEvent
			{
				tile = tileId,
				amount = num
			};
			CompDissolutionEffect_Pollution.pendingWorldEvents.Add(item);
		}

		// Token: 0x06010F1E RID: 69406 RVA: 0x004E06B4 File Offset: 0x004DE8B4
		public static void WorldUpdate()
		{
			if (CompDissolutionEffect_Pollution.pendingWorldEvents.Count <= 0)
			{
				return;
			}
			foreach (IGrouping<PlanetTile, CompDissolutionEffect_Pollution.WorldPollutionEvent> grouping in from e in CompDissolutionEffect_Pollution.pendingWorldEvents
			group e by e.tile)
			{
				WorldPollutionUtility.PolluteWorldAtTile(grouping.Key, grouping.Sum((CompDissolutionEffect_Pollution.WorldPollutionEvent e) => e.amount));
			}
			CompDissolutionEffect_Pollution.pendingWorldEvents.Clear();
		}

		// Token: 0x0400B035 RID: 45109
		private static List<CompDissolutionEffect_Pollution.WorldPollutionEvent> pendingWorldEvents = new List<CompDissolutionEffect_Pollution.WorldPollutionEvent>();

		// Token: 0x02002EE2 RID: 12002
		private struct WorldPollutionEvent
		{
			// Token: 0x0400B036 RID: 45110
			public PlanetTile tile;

			// Token: 0x0400B037 RID: 45111
			public float amount;
		}
	}
}
