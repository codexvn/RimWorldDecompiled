using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200207B RID: 8315
	public class GenStep_PrisonerWillingToJoin : GenStep_Scatterer
	{
		// Token: 0x17001BD8 RID: 7128
		// (get) Token: 0x0600B64E RID: 46670 RVA: 0x0034611B File Offset: 0x0034431B
		public override int SeedPart
		{
			get
			{
				return 69356099;
			}
		}

		// Token: 0x0600B64F RID: 46671 RVA: 0x00346124 File Offset: 0x00344324
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			if (c.Fogged(map))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(c, 8, 8);
			if (MapGenerator.UsedRects.Any((CellRect ur) => ur.Overlaps(rect)))
			{
				return false;
			}
			foreach (IntVec3 c2 in rect)
			{
				if (!c2.InBounds(map) || c2.GetEdifice(map) != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B650 RID: 46672 RVA: 0x003461E0 File Offset: 0x003443E0
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			CellRect cellRect = CellRect.CenteredOn(loc, 8, 8).ClipInsideMap(map);
			Pawn singlePawnToSpawn;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				singlePawnToSpawn = (Pawn)parms.sitePart.things.Take(parms.sitePart.things[0]);
			}
			else
			{
				PrisonerWillingToJoinComp component = map.Parent.GetComponent<PrisonerWillingToJoinComp>();
				if (component != null && component.pawn.Any)
				{
					singlePawnToSpawn = component.pawn.Take(component.pawn[0]);
				}
				else
				{
					singlePawnToSpawn = PrisonerWillingToJoinQuestUtility.GeneratePrisoner(map.Tile, faction);
				}
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("prisonCell", resolveParams, null);
			BaseGen.Generate();
			ResolveParams resolveParams2 = default(ResolveParams);
			resolveParams2.rect = cellRect;
			resolveParams2.faction = faction;
			resolveParams2.singlePawnToSpawn = singlePawnToSpawn;
			resolveParams2.singlePawnSpawnCellExtraPredicate = ((IntVec3 x) => x.GetDoor(map) == null);
			resolveParams2.postThingSpawn = delegate(Thing x)
			{
				MapGenerator.rootsToUnfog.Add(x.Position);
				((Pawn)x).mindState.WillJoinColonyIfRescued = true;
			};
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("pawn", resolveParams2, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
			map.fogGrid.Refog(cellRect.ContractedBy(1));
		}

		// Token: 0x04007D16 RID: 32022
		private const int Size = 8;
	}
}
