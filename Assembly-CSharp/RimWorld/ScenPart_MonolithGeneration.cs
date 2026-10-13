using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020028C0 RID: 10432
	public class ScenPart_MonolithGeneration : ScenPart_DisableMapGen
	{
		// Token: 0x0600E431 RID: 58417 RVA: 0x0042A8A3 File Offset: 0x00428AA3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<MonolithGenerationMethod>(ref this.method, "method", MonolithGenerationMethod.Disabled, false);
		}

		// Token: 0x0600E432 RID: 58418 RVA: 0x0042A8C0 File Offset: 0x00428AC0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.method.ToStringHuman(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(MonolithGenerationMethod)))
				{
					MonolithGenerationMethod localM2 = (MonolithGenerationMethod)obj;
					MonolithGenerationMethod localM = localM2;
					list.Add(new FloatMenuOption(localM.ToStringHuman(), delegate()
					{
						this.method = localM;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600E433 RID: 58419 RVA: 0x0042A9A8 File Offset: 0x00428BA8
		public override void PostMapGenerate(Map map)
		{
			if (this.method == MonolithGenerationMethod.Disabled)
			{
				return;
			}
			if (!Find.Anomaly.GenerateMonolith)
			{
				return;
			}
			if (Find.GameInfo.startingTile != map.Tile)
			{
				return;
			}
			IntVec3 loc;
			if (!LargeBuildingCellFinder.TryFindCell(out loc, map, ScenPart_MonolithGeneration.StructureSpawnParms.ForThing(ThingDefOf.VoidMonolith), null, (IntVec3 c) => ScattererValidator_AvoidSpecialThings.IsValid(c, map), true) && !LargeBuildingCellFinder.TryFindCell(out loc, map, ScenPart_MonolithGeneration.StructureSpawnParmsLoose.ForThing(ThingDefOf.VoidMonolith), null, (IntVec3 c) => ScattererValidator_AvoidSpecialThings.IsValid(c, map), true))
			{
				Log.Error("Failed to generate monolith.");
				return;
			}
			GenStep_Monolith.GenerateMonolith(loc, map);
		}

		// Token: 0x0600E434 RID: 58420 RVA: 0x0042AA6A File Offset: 0x00428C6A
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.method.GetHashCode();
		}

		// Token: 0x040098BE RID: 39102
		private MonolithGenerationMethod method = MonolithGenerationMethod.NearColonists;

		// Token: 0x040098BF RID: 39103
		private static readonly LargeBuildingSpawnParms StructureSpawnParms = new LargeBuildingSpawnParms
		{
			minDistToEdge = 10,
			maxDistanceFromPlayerStartPosition = 20f,
			attemptSpawnLocationType = SpawnLocationType.Outdoors,
			attemptNotUnderBuildings = true,
			canSpawnOnImpassable = false
		};

		// Token: 0x040098C0 RID: 39104
		private static readonly LargeBuildingSpawnParms StructureSpawnParmsLoose = new LargeBuildingSpawnParms
		{
			minDistToEdge = 10,
			attemptSpawnLocationType = SpawnLocationType.Outdoors,
			attemptNotUnderBuildings = true,
			canSpawnOnImpassable = false
		};
	}
}
