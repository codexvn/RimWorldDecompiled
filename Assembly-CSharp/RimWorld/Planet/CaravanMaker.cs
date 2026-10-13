using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D5F RID: 15711
	public static class CaravanMaker
	{
		// Token: 0x06015670 RID: 87664 RVA: 0x006671DC File Offset: 0x006653DC
		public static Caravan MakeCaravan(IEnumerable<Pawn> pawns, Faction faction, PlanetTile startingTile, bool addToWorldPawnsIfNotAlready)
		{
			if (!startingTile.Valid && addToWorldPawnsIfNotAlready)
			{
				Log.Warning("Tried to create a caravan but chose not to spawn a caravan but pass pawns to world. This can cause bugs because pawns can be discarded.");
			}
			if (!startingTile.LayerDef.canFormCaravans)
			{
				Log.Warning("Tried to create a caravan on a tile which belongs to a layer which cannot form caravans.");
			}
			CaravanMaker.tmpPawns.Clear();
			CaravanMaker.tmpPawns.AddRange(pawns);
			Caravan caravan = (Caravan)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Caravan);
			if (startingTile.Valid)
			{
				caravan.Tile = startingTile;
			}
			caravan.SetFaction(faction);
			if (startingTile.Valid)
			{
				Find.WorldObjects.Add(caravan);
			}
			for (int i = 0; i < CaravanMaker.tmpPawns.Count; i++)
			{
				Pawn pawn = CaravanMaker.tmpPawns[i];
				if (pawn.Dead)
				{
					string str = "Tried to form a caravan with a dead pawn ";
					Pawn pawn2 = pawn;
					Log.Warning(str + ((pawn2 != null) ? pawn2.ToString() : null));
				}
				else
				{
					if (!caravan.ContainsPawn(pawn))
					{
						caravan.AddPawn(pawn, addToWorldPawnsIfNotAlready);
					}
					if (addToWorldPawnsIfNotAlready && !pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			caravan.Name = CaravanNameGenerator.GenerateCaravanName(caravan);
			CaravanMaker.tmpPawns.Clear();
			caravan.SetUniqueId(Find.UniqueIDsManager.GetNextCaravanID());
			return caravan;
		}

		// Token: 0x0400EE49 RID: 61001
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();
	}
}
