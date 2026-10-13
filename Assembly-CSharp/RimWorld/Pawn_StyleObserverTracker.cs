using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020024E1 RID: 9441
	public class Pawn_StyleObserverTracker : IExposable
	{
		// Token: 0x17001F97 RID: 8087
		// (get) Token: 0x0600CE1F RID: 52767 RVA: 0x003C8607 File Offset: 0x003C6807
		public int StyleDominanceThoughtIndex
		{
			get
			{
				return this.styleDominanceThoughtIndex;
			}
		}

		// Token: 0x0600CE20 RID: 52768 RVA: 0x003C860F File Offset: 0x003C680F
		public Pawn_StyleObserverTracker()
		{
		}

		// Token: 0x0600CE21 RID: 52769 RVA: 0x003C861E File Offset: 0x003C681E
		public Pawn_StyleObserverTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CE22 RID: 52770 RVA: 0x003C8634 File Offset: 0x003C6834
		private bool CellValid(IntVec3 cell)
		{
			return cell.InBounds(this.pawn.Map) && !cell.Fogged(this.pawn.Map) && GenSight.LineOfSight(this.pawn.Position, cell, this.pawn.Map);
		}

		// Token: 0x0600CE23 RID: 52771 RVA: 0x003C8688 File Offset: 0x003C6888
		public void StyleObserverTickInterval(int delta)
		{
			if (this.pawn.IsHashIntervalTick(900, delta) && ModsConfig.IdeologyActive && this.pawn.Ideo != null && this.pawn.Spawned)
			{
				int lastIndex = this.styleDominanceThoughtIndex;
				this.styleDominanceThoughtIndex = -1;
				if (this.pawn.Awake() && !PawnUtility.IsBiologicallyOrArtificiallyBlind(this.pawn))
				{
					Pawn_NeedsTracker needs = this.pawn.needs;
					if (((needs != null) ? needs.mood : null) != null)
					{
						Room room = this.pawn.GetRoom(RegionType.Set_All);
						IntVec3 center;
						if (room != null && CellFinder.TryFindRandomCellNear(this.pawn.Position, this.pawn.Map, 5, new Predicate<IntVec3>(this.CellValid), out center, -1))
						{
							float styleDominanceFromCellsCenteredOn = IdeoUtility.GetStyleDominanceFromCellsCenteredOn(center, this.pawn.Position, this.pawn.Map, this.pawn.Ideo);
							float pointsThreshold = (!room.IsDoorway && (float)room.CellCount < 10f) ? ((float)room.CellCount) : 10f;
							this.UpdateStyleDominanceThoughtIndex(styleDominanceFromCellsCenteredOn, pointsThreshold, lastIndex);
						}
					}
				}
			}
		}

		// Token: 0x0600CE24 RID: 52772 RVA: 0x003C87B8 File Offset: 0x003C69B8
		private void UpdateStyleDominanceThoughtIndex(float styleDominance, float pointsThreshold, int lastIndex)
		{
			if (styleDominance >= pointsThreshold)
			{
				this.styleDominanceThoughtIndex = 0;
			}
			else if (styleDominance <= -pointsThreshold)
			{
				this.styleDominanceThoughtIndex = 1;
			}
			else
			{
				this.styleDominanceThoughtIndex = -1;
			}
			if (lastIndex != this.styleDominanceThoughtIndex)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}

		// Token: 0x0600CE25 RID: 52773 RVA: 0x003C880F File Offset: 0x003C6A0F
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.styleDominanceThoughtIndex, "styleDominanceThoughtIndex", -1, false);
		}

		// Token: 0x04008C48 RID: 35912
		public Pawn pawn;

		// Token: 0x04008C49 RID: 35913
		private int styleDominanceThoughtIndex = -1;

		// Token: 0x04008C4A RID: 35914
		private const int StyleObservationCenterCellRadius = 5;

		// Token: 0x04008C4B RID: 35915
		private const int StyleObservationInterval = 900;

		// Token: 0x04008C4C RID: 35916
		private const float BaseDominancePointsThreshold = 10f;
	}
}
