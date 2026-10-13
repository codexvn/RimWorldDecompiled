using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000991 RID: 2449
	public static class DebugActionsPrefabs
	{
		// Token: 0x06003E47 RID: 15943 RVA: 0x00145C94 File Offset: 0x00143E94
		[DebugAction("Generation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RotatePrefabSpawn()
		{
			DebugActionsPrefabs.Rotation.Rotate(RotationDirection.Clockwise);
			Messages.Message("Prefab rotation: " + DebugActionsPrefabs.Rotation.ToStringHuman(), MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x00145CC0 File Offset: 0x00143EC0
		[DebugAction("Generation", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CreatePrefab()
		{
			DebugToolsGeneral.GenericRectTool("Create", delegate(CellRect rect)
			{
				PrefabDef prefabDef = PrefabUtility.CreatePrefab(rect, DebugGenerationSettings.prefabCopyAllThings, DebugGenerationSettings.prefabCopyTerrain);
				StringBuilder stringBuilder = new StringBuilder();
				string text = "  ";
				stringBuilder.AppendLine("\n<PrefabDef>");
				stringBuilder.AppendLine(text + "<defName>NewPrefab</defName> <!-- rename -->");
				stringBuilder.AppendLine(string.Format("{0}<size>({1},{2})</size>", text, rect.Size.x, rect.Size.z));
				if (prefabDef.things.CountAllowNull<PrefabThingData>() > 0)
				{
					stringBuilder.AppendLine(text + "<things>");
					for (int i = 0; i < prefabDef.things.Count; i++)
					{
						PrefabThingData prefabThingData = prefabDef.things[i];
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							text,
							text,
							"<",
							prefabThingData.def.defName,
							">"
						}));
						if (prefabThingData.rects != null)
						{
							stringBuilder.AppendLine(text + text + text + "<rects>");
							foreach (CellRect cellRect in prefabThingData.rects)
							{
								stringBuilder.AppendLine(string.Format("{0}{1}{2}{3}<li>{4}</li>", new object[]
								{
									text,
									text,
									text,
									text,
									cellRect
								}));
							}
							stringBuilder.AppendLine(text + text + text + "</rects>");
						}
						else if (prefabThingData.positions != null)
						{
							stringBuilder.AppendLine(text + text + text + "<positions>");
							foreach (IntVec3 intVec in prefabThingData.positions)
							{
								stringBuilder.AppendLine(string.Format("{0}{1}{2}{3}<li>{4}</li>", new object[]
								{
									text,
									text,
									text,
									text,
									intVec
								}));
							}
							stringBuilder.AppendLine(text + text + text + "</positions>");
						}
						else
						{
							stringBuilder.AppendLine(string.Format("{0}{1}{2}<position>{3}</position>", new object[]
							{
								text,
								text,
								text,
								prefabThingData.position
							}));
						}
						if (prefabThingData.relativeRotation != RotationDirection.None)
						{
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								text,
								text,
								text,
								"<relativeRotation>",
								Enum.GetName(typeof(RotationDirection), prefabThingData.relativeRotation),
								"</relativeRotation>"
							}));
						}
						if (prefabThingData.stuff != null)
						{
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								text,
								text,
								text,
								"<stuff>",
								prefabThingData.stuff.defName,
								"</stuff>"
							}));
						}
						if (prefabThingData.quality != null)
						{
							stringBuilder.AppendLine(string.Format("{0}{1}{2}<quality>{3}</quality>", new object[]
							{
								text,
								text,
								text,
								prefabThingData.quality
							}));
						}
						if (prefabThingData.hp != 0)
						{
							stringBuilder.AppendLine(string.Format("{0}{1}{2}<hp>{3}</hp>", new object[]
							{
								text,
								text,
								text,
								prefabThingData.hp
							}));
						}
						if (prefabThingData.stackCountRange != IntRange.One)
						{
							stringBuilder.AppendLine(string.Format("{0}{1}{2}<stackCountRange>{3}~{4}</stackCountRange>", new object[]
							{
								text,
								text,
								text,
								prefabThingData.stackCountRange.min,
								prefabThingData.stackCountRange.max
							}));
						}
						if (prefabThingData.colorDef != null)
						{
							stringBuilder.AppendLine(string.Format("{0}{1}{2}<colorDef>{3}</colorDef>", new object[]
							{
								text,
								text,
								text,
								prefabThingData.colorDef
							}));
						}
						if (prefabThingData.color != default(Color))
						{
							stringBuilder.AppendLine(string.Format("{0}{1}{2}<color>{3}</color>", new object[]
							{
								text,
								text,
								text,
								prefabThingData.color
							}));
						}
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							text,
							text,
							"</",
							prefabThingData.def.defName,
							">"
						}));
					}
					stringBuilder.AppendLine(text + "</things>");
				}
				if (prefabDef.terrain.CountAllowNull<PrefabTerrainData>() > 0)
				{
					stringBuilder.AppendLine(text + "<terrain>");
					foreach (PrefabTerrainData prefabTerrainData in prefabDef.terrain)
					{
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							text,
							text,
							"<",
							prefabTerrainData.def.defName,
							">"
						}));
						if (prefabTerrainData.color != null)
						{
							stringBuilder.AppendLine(string.Format("{0}{1}{2}<color>{3}</color>", new object[]
							{
								text,
								text,
								text,
								prefabTerrainData.color
							}));
						}
						stringBuilder.AppendLine(text + text + text + "<rects>");
						foreach (CellRect cellRect2 in prefabTerrainData.rects)
						{
							stringBuilder.AppendLine(string.Format("{0}{1}{2}{3}<li>{4}</li>", new object[]
							{
								text,
								text,
								text,
								text,
								cellRect2
							}));
						}
						stringBuilder.AppendLine(text + text + text + "</rects>");
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							text,
							text,
							"</",
							prefabTerrainData.def.defName,
							">"
						}));
					}
					stringBuilder.AppendLine(text + "</terrain>");
				}
				stringBuilder.AppendLine("</PrefabDef>");
				DebugActionsPrefabs.buffer = prefabDef;
				GUIUtility.systemCopyBuffer = stringBuilder.ToString();
				Messages.Message("Copied to clipboard", MessageTypeDefOf.NeutralEvent, false);
			}, true);
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x00145CEC File Offset: 0x00143EEC
		[DebugAction("Generation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnPlayerPrefab()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("Buffer", DebugActionType.ToolMap, null, null);
			debugActionNode.action = delegate()
			{
				if (DebugActionsPrefabs.buffer != null)
				{
					DebugActionsPrefabs.SpawnAtMouseCell(DebugActionsPrefabs.buffer, false, Faction.OfPlayer);
				}
			};
			list2.Add(debugActionNode);
			using (List<PrefabDef>.Enumerator enumerator = DefDatabase<PrefabDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PrefabDef def = enumerator.Current;
					list.Add(new DebugActionNode(def.defName ?? "", DebugActionType.ToolMap, null, null)
					{
						action = delegate()
						{
							DebugActionsPrefabs.SpawnAtMouseCell(def, false, Faction.OfPlayer);
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00145DB8 File Offset: 0x00143FB8
		[DebugAction("Generation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnPrefab()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("Buffer", DebugActionType.ToolMap, null, null);
			debugActionNode.action = delegate()
			{
				if (DebugActionsPrefabs.buffer != null)
				{
					DebugActionsPrefabs.SpawnAtMouseCell(DebugActionsPrefabs.buffer, false, null);
				}
			};
			list2.Add(debugActionNode);
			using (List<PrefabDef>.Enumerator enumerator = DefDatabase<PrefabDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PrefabDef def = enumerator.Current;
					list.Add(new DebugActionNode(def.defName ?? "", DebugActionType.ToolMap, null, null)
					{
						action = delegate()
						{
							DebugActionsPrefabs.SpawnAtMouseCell(def, false, null);
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x00145E84 File Offset: 0x00144084
		[DebugAction("Generation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnPrefabBlueprint()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("Buffer", DebugActionType.ToolMap, null, null);
			debugActionNode.action = delegate()
			{
				if (DebugActionsPrefabs.buffer != null)
				{
					DebugActionsPrefabs.SpawnAtMouseCell(DebugActionsPrefabs.buffer, true, null);
				}
			};
			list2.Add(debugActionNode);
			using (List<PrefabDef>.Enumerator enumerator = DefDatabase<PrefabDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PrefabDef def = enumerator.Current;
					list.Add(new DebugActionNode(def.defName ?? "", DebugActionType.ToolMap, null, null)
					{
						action = delegate()
						{
							DebugActionsPrefabs.SpawnAtMouseCell(def, true, null);
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x00145F50 File Offset: 0x00144150
		private static void SpawnAtMouseCell(PrefabDef def, bool blueprint = false, Faction faction = null)
		{
			IntVec3 intVec = UI.MouseCell();
			CellRect cellRect = GenAdj.OccupiedRect(intVec, DebugActionsPrefabs.Rotation, def.size);
			if (!intVec.InBounds(Find.CurrentMap))
			{
				return;
			}
			foreach (IntVec3 c in cellRect.Cells)
			{
				if (c.InBounds(Find.CurrentMap))
				{
					Find.CurrentMap.terrainGrid.SetTerrain(c, TerrainDefOf.MetalTile);
				}
			}
			PrefabUtility.SpawnPrefab(def, Find.CurrentMap, intVec, DebugActionsPrefabs.Rotation, faction, null, null, null, blueprint);
		}

		// Token: 0x04002C90 RID: 11408
		private static Rot4 Rotation = Rot4.North;

		// Token: 0x04002C91 RID: 11409
		private static PrefabDef buffer;
	}
}
