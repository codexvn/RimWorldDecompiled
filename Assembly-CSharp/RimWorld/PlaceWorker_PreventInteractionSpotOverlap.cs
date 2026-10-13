using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200380A RID: 14346
	public class PlaceWorker_PreventInteractionSpotOverlap : PlaceWorker
	{
		// Token: 0x06013DAD RID: 81325 RVA: 0x005E4E80 File Offset: 0x005E3080
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thingToPlace = null)
		{
			ThingDef thingDef = checkingDef as ThingDef;
			if (thingDef == null || !thingDef.HasSingleOrMultipleInteractionCells)
			{
				return true;
			}
			List<IntVec3> list = new List<IntVec3>();
			List<IntVec3> list2 = new List<IntVec3>();
			ThingUtility.InteractionCellsWhenAt(list, thingDef, loc, rot, map, false);
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					foreach (IntVec3 intVec in list)
					{
						IntVec3 c = intVec;
						c.x += i;
						c.z += j;
						if (c.InBounds(map))
						{
							foreach (Thing thing in map.thingGrid.ThingsListAtFast(c))
							{
								if (thing != thingToIgnore)
								{
									ThingDef thingDef2 = thing.def;
									if (thing.def.entityDefToBuild != null)
									{
										thingDef2 = (thing.def.entityDefToBuild as ThingDef);
									}
									if (thingDef2 != null && thingDef2.HasSingleOrMultipleInteractionCells)
									{
										ThingUtility.InteractionCellsWhenAt(list2, thingDef2, thing.Position, thing.Rotation, thing.Map, false);
										using (List<IntVec3>.Enumerator enumerator3 = list2.GetEnumerator())
										{
											while (enumerator3.MoveNext())
											{
												if (enumerator3.Current == intVec)
												{
													return new AcceptanceReport(((thing.def.entityDefToBuild == null) ? "InteractionSpotOverlaps" : "InteractionSpotWillOverlap").Translate(thing.LabelNoCount, thing));
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return true;
		}
	}
}
