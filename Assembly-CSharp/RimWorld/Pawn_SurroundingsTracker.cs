using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003792 RID: 14226
	public class Pawn_SurroundingsTracker : IExposable
	{
		// Token: 0x06013B37 RID: 80695 RVA: 0x005DA752 File Offset: 0x005D8952
		public Pawn_SurroundingsTracker()
		{
		}

		// Token: 0x06013B38 RID: 80696 RVA: 0x005DA786 File Offset: 0x005D8986
		public Pawn_SurroundingsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06013B39 RID: 80697 RVA: 0x005DA7C4 File Offset: 0x005D89C4
		private int NumSightingsInRange(ref List<TreeSighting> sightings, int ticks)
		{
			int num = 0;
			for (int i = 0; i < sightings.Count; i++)
			{
				if (sightings[i].TicksSinceSighting <= ticks)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06013B3A RID: 80698 RVA: 0x005DA7FC File Offset: 0x005D89FC
		public int NumSightingsInRange(TreeCategory treeCategory, int ticks)
		{
			switch (treeCategory)
			{
			case TreeCategory.Mini:
				return this.NumSightingsInRange(ref this.miniTreeSightings, ticks);
			case TreeCategory.Full:
				return this.NumSightingsInRange(ref this.fullTreeSightings, ticks);
			case TreeCategory.Super:
				return this.NumSightingsInRange(ref this.superTreeSightings, ticks);
			default:
				return 0;
			}
		}

		// Token: 0x06013B3B RID: 80699 RVA: 0x005DA84A File Offset: 0x005D8A4A
		public int NumSkullspikeSightings()
		{
			return this.skullspikeSightings.Count;
		}

		// Token: 0x06013B3C RID: 80700 RVA: 0x005DA858 File Offset: 0x005D8A58
		public void SurroundingsTrackerTickInterval(int delta)
		{
			if (this.pawn.IsHashIntervalTick(2500, delta) && ModsConfig.IdeologyActive && this.pawn.Ideo != null && this.pawn.Ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.TreesDesired) && this.pawn.Awake())
			{
				if (this.pawn.Spawned)
				{
					this.GetSpawnedTreeSightings();
				}
				else
				{
					this.GetCaravanTreeSightings();
				}
			}
			if (this.pawn.IsHashIntervalTick(60, delta) && ModsConfig.IdeologyActive && this.pawn.Ideo != null && this.pawn.Spawned && this.pawn.Awake() && (this.pawn.Ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.Skullspike_Desired) || this.pawn.Ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.Skullspike_Disapproved)) && !PawnUtility.IsBiologicallyOrArtificiallyBlind(this.pawn))
			{
				for (int i = this.skullspikeSightings.Count - 1; i >= 0; i--)
				{
					if (this.skullspikeSightings[i].TicksSinceSighting > 1800)
					{
						this.skullspikeSightings.RemoveAt(i);
					}
				}
				if (this.pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Skullspike).Count != 0)
				{
					IntVec3 positionHeld = this.pawn.PositionHeld;
					int num = GenRadial.NumCellsInRadius(10f);
					for (int j = 0; j < num; j++)
					{
						IntVec3 c = positionHeld + GenRadial.RadialPattern[j];
						if (c.InBounds(this.pawn.Map) && !c.Fogged(this.pawn.Map))
						{
							Thing firstThing = c.GetFirstThing(this.pawn.Map, ThingDefOf.Skullspike);
							if (firstThing != null && GenSight.LineOfSight(this.pawn.PositionHeld, firstThing.PositionHeld, firstThing.Map, true, null, 0, 0))
							{
								bool flag = false;
								for (int k = 0; k < this.skullspikeSightings.Count; k++)
								{
									if (this.skullspikeSightings[k].skullspike == firstThing)
									{
										SkullspikeSighting value = this.skullspikeSightings[k];
										value.tickSighted = Find.TickManager.TicksGame;
										this.skullspikeSightings[k] = value;
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									this.skullspikeSightings.Add(new SkullspikeSighting
									{
										skullspike = firstThing,
										tickSighted = Find.TickManager.TicksGame
									});
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06013B3D RID: 80701 RVA: 0x005DAB1C File Offset: 0x005D8D1C
		private void GetSpawnedTreeSightings()
		{
			foreach (TreeSighting treeSighting in IdeoUtility.TreeSightingsNearPawn(this.pawn.Position, this.pawn.Map, this.pawn.Ideo))
			{
				if (treeSighting.Tree != null)
				{
					TreeCategory treeCategory = treeSighting.Tree.def.plant.treeCategory;
				}
				switch (treeSighting.Tree.def.plant.treeCategory)
				{
				case TreeCategory.Mini:
					this.AddSighting(ref this.miniTreeSightings, treeSighting);
					break;
				case TreeCategory.Full:
					this.AddSighting(ref this.fullTreeSightings, treeSighting);
					break;
				case TreeCategory.Super:
					this.AddSighting(ref this.superTreeSightings, treeSighting);
					break;
				}
			}
		}

		// Token: 0x06013B3E RID: 80702 RVA: 0x005DAC00 File Offset: 0x005D8E00
		private void GetCaravanTreeSightings()
		{
			Caravan caravan = this.pawn.GetCaravan();
			if (caravan == null)
			{
				return;
			}
			int treeSightingsPerHourFromCaravan = caravan.Biome.TreeSightingsPerHourFromCaravan;
			for (int i = 0; i < treeSightingsPerHourFromCaravan; i++)
			{
				this.AddSighting(ref this.fullTreeSightings, new TreeSighting(null, Find.TickManager.TicksGame));
			}
		}

		// Token: 0x06013B3F RID: 80703 RVA: 0x005DAC54 File Offset: 0x005D8E54
		private void AddSighting(ref List<TreeSighting> list, TreeSighting newSighting)
		{
			if (newSighting.tree != null)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (list[i].tree != null && list[i].tree == newSighting.tree)
					{
						list.RemoveAt(i);
					}
				}
			}
			list.Add(newSighting);
			for (int j = 0; j < list.Count - 15; j++)
			{
				list.RemoveAt(0);
			}
		}

		// Token: 0x06013B40 RID: 80704 RVA: 0x005DACCE File Offset: 0x005D8ECE
		public void Clear()
		{
			this.miniTreeSightings.Clear();
			this.fullTreeSightings.Clear();
			this.superTreeSightings.Clear();
		}

		// Token: 0x06013B41 RID: 80705 RVA: 0x005DACF4 File Offset: 0x005D8EF4
		public void ExposeData()
		{
			Scribe_Collections.Look<TreeSighting>(ref this.miniTreeSightings, "miniTreeSightings", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<TreeSighting>(ref this.fullTreeSightings, "fullTreeSightings", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<TreeSighting>(ref this.superTreeSightings, "superTreeSightings", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0400DA38 RID: 55864
		public Pawn pawn;

		// Token: 0x0400DA39 RID: 55865
		public List<TreeSighting> miniTreeSightings = new List<TreeSighting>();

		// Token: 0x0400DA3A RID: 55866
		public List<TreeSighting> fullTreeSightings = new List<TreeSighting>();

		// Token: 0x0400DA3B RID: 55867
		public List<TreeSighting> superTreeSightings = new List<TreeSighting>();

		// Token: 0x0400DA3C RID: 55868
		public List<SkullspikeSighting> skullspikeSightings = new List<SkullspikeSighting>();

		// Token: 0x0400DA3D RID: 55869
		private const int MaxSightingsPerCategory = 15;

		// Token: 0x0400DA3E RID: 55870
		private const int TreeCheckInterval = 2500;
	}
}
