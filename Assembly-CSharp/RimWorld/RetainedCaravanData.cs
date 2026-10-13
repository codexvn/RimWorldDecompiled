using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200215A RID: 8538
	public class RetainedCaravanData : IExposable
	{
		// Token: 0x17001C66 RID: 7270
		// (get) Token: 0x0600BA97 RID: 47767 RVA: 0x0035DD0F File Offset: 0x0035BF0F
		public bool HasDestinationTile
		{
			get
			{
				return this.destinationTile.Valid;
			}
		}

		// Token: 0x0600BA98 RID: 47768 RVA: 0x0035DD1C File Offset: 0x0035BF1C
		public RetainedCaravanData(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600BA99 RID: 47769 RVA: 0x0035DD44 File Offset: 0x0035BF44
		public void ExposeData()
		{
			Scribe_Values.Look<PlanetTile>(ref this.nextTile, "nextTile", PlanetTile.Invalid, false);
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Values.Look<bool>(ref this.shouldPassStoryState, "shouldPassStoryState", false, false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeftPct, "nextTileCostLeftPct", -1f, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
		}

		// Token: 0x0600BA9A RID: 47770 RVA: 0x0035DDD0 File Offset: 0x0035BFD0
		public void Notify_GeneratedTempIncidentMapFor(Caravan caravan)
		{
			if (!this.map.Parent.def.isTempIncidentMapOwner)
			{
				return;
			}
			this.Set(caravan);
		}

		// Token: 0x0600BA9B RID: 47771 RVA: 0x0035DDF4 File Offset: 0x0035BFF4
		public void Notify_CaravanFormed(Caravan caravan)
		{
			if (this.shouldPassStoryState)
			{
				this.shouldPassStoryState = false;
				this.map.StoryState.CopyTo(caravan.StoryState);
			}
			if (this.nextTile.Valid && this.nextTile != caravan.Tile && caravan.CanReach(this.nextTile))
			{
				caravan.pather.StartPath(this.nextTile, null, true, true);
				caravan.pather.nextTileCostLeft = caravan.pather.nextTileCostTotal * this.nextTileCostLeftPct;
				caravan.pather.Paused = this.paused;
				caravan.tweener.ResetTweenedPosToRoot();
			}
			if (this.HasDestinationTile && this.destinationTile != caravan.Tile)
			{
				caravan.pather.StartPath(this.destinationTile, this.arrivalAction, true, true);
				this.destinationTile = PlanetTile.Invalid;
				this.arrivalAction = null;
			}
		}

		// Token: 0x0600BA9C RID: 47772 RVA: 0x0035DEEC File Offset: 0x0035C0EC
		private void Set(Caravan caravan)
		{
			caravan.StoryState.CopyTo(this.map.StoryState);
			this.shouldPassStoryState = true;
			if (caravan.pather.Moving)
			{
				this.nextTile = caravan.pather.nextTile;
				this.nextTileCostLeftPct = caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal;
				this.paused = caravan.pather.Paused;
				this.destinationTile = caravan.pather.Destination;
				this.arrivalAction = caravan.pather.ArrivalAction;
				return;
			}
			this.nextTile = PlanetTile.Invalid;
			this.nextTileCostLeftPct = 0f;
			this.paused = false;
			this.destinationTile = PlanetTile.Invalid;
			this.arrivalAction = null;
		}

		// Token: 0x040080A2 RID: 32930
		private Map map;

		// Token: 0x040080A3 RID: 32931
		public PlanetTile nextTile = PlanetTile.Invalid;

		// Token: 0x040080A4 RID: 32932
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x040080A5 RID: 32933
		private bool shouldPassStoryState;

		// Token: 0x040080A6 RID: 32934
		private float nextTileCostLeftPct;

		// Token: 0x040080A7 RID: 32935
		private bool paused;

		// Token: 0x040080A8 RID: 32936
		private CaravanArrivalAction arrivalAction;

		// Token: 0x040080A9 RID: 32937
		private int backCompatTile;
	}
}
