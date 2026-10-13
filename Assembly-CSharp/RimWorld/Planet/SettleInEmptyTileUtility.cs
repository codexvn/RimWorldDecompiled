using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003D83 RID: 15747
	public static class SettleInEmptyTileUtility
	{
		// Token: 0x0601573E RID: 87870 RVA: 0x0066D8F4 File Offset: 0x0066BAF4
		public static void Settle(Caravan caravan)
		{
			Faction faction = caravan.Faction;
			if (faction != Faction.OfPlayer)
			{
				Log.Error("Cannot settle with non-player faction.");
				return;
			}
			if (Find.AnyPlayerHomeMap == null)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					MemoryThoughtHandler memoryThoughtHandler;
					if (needs == null)
					{
						memoryThoughtHandler = null;
					}
					else
					{
						Need_Mood mood = needs.mood;
						if (mood == null)
						{
							memoryThoughtHandler = null;
						}
						else
						{
							ThoughtHandler thoughts = mood.thoughts;
							memoryThoughtHandler = ((thoughts != null) ? thoughts.memories : null);
						}
					}
					MemoryThoughtHandler memoryThoughtHandler2 = memoryThoughtHandler;
					if (memoryThoughtHandler2 != null)
					{
						memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyOptimism);
						memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyHope);
						if (pawn.IsFreeNonSlaveColonist)
						{
							memoryThoughtHandler2.TryGainMemory(ThoughtDefOf.NewColonyOptimism, null, null);
						}
					}
				}
			}
			Settlement newHome = SettleUtility.AddNewHome(caravan.Tile, faction);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GetOrGenerateMapUtility.GetOrGenerateMap(caravan.Tile, Find.World.info.initialMapSize, null, null, false);
			}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
			LongEventHandler.QueueLongEvent(delegate()
			{
				Map map = newHome.Map;
				Thing t = caravan.PawnsListForReading[0];
				CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Center, CaravanDropInventoryMode.DropInstantly, false, (IntVec3 x) => x.GetRoom(map).CellCount >= 600);
				newHome.Notify_MyMapSettled(map);
				CameraJumper.TryJump(t, CameraJumper.MovementMode.Pan);
			}, "SpawningColonists", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
		}

		// Token: 0x0601573F RID: 87871 RVA: 0x0066DA38 File Offset: 0x0066BC38
		public static Command SettleCommand(Caravan caravan)
		{
			Command_Settle command_Settle = new Command_Settle();
			command_Settle.defaultLabel = "CommandSettle".Translate();
			command_Settle.defaultDesc = "CommandSettleDesc".Translate();
			command_Settle.icon = SettleUtility.SettleCommandTex;
			Action <>9__1;
			command_Settle.action = delegate()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				PlanetTile tile = caravan.Tile;
				Action settleAction;
				if ((settleAction = <>9__1) == null)
				{
					settleAction = (<>9__1 = delegate()
					{
						SettleInEmptyTileUtility.Settle(caravan);
					});
				}
				SettlementProximityGoodwillUtility.CheckConfirmSettle(tile, settleAction, null, null);
			};
			SettleInEmptyTileUtility.tmpSettleFailReason.Length = 0;
			if (!TileFinder.IsValidTileForNewSettlement(caravan.Tile, SettleInEmptyTileUtility.tmpSettleFailReason, false))
			{
				command_Settle.Disable(SettleInEmptyTileUtility.tmpSettleFailReason.ToString());
			}
			else if (SettleUtility.PlayerSettlementsCountLimitReached)
			{
				if (Prefs.MaxNumberOfPlayerSettlements > 1)
				{
					command_Settle.Disable("CommandSettleFailReachedMaximumNumberOfBases".Translate());
				}
				else
				{
					command_Settle.Disable("CommandSettleFailAlreadyHaveBase".Translate());
				}
			}
			return command_Settle;
		}

		// Token: 0x06015740 RID: 87872 RVA: 0x0066DB10 File Offset: 0x0066BD10
		public static Command SetupCamp(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandCamp".Translate();
			command_Action.defaultDesc = "CommandCampDesc".Translate();
			command_Action.icon = SettleUtility.CreateCampCommandTex;
			Action <>9__1;
			command_Action.action = delegate()
			{
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate()
					{
						Map map = GetOrGenerateMapUtility.GetOrGenerateMap(caravan.Tile, WorldObjectDefOf.Camp.overrideMapSize ?? Find.World.info.initialMapSize, WorldObjectDefOf.Camp, null, false);
						map.Parent.SetFaction(caravan.Faction);
						Thing t = caravan.PawnsListForReading[0];
						CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Center, CaravanDropInventoryMode.DoNotDrop, false, (IntVec3 x) => x.GetRoom(map).CellCount >= 600 && !x.GetTerrain(map).IsWater);
						TimedDetectionRaids component = map.Parent.GetComponent<TimedDetectionRaids>();
						if (component != null)
						{
							component.StartDetectionCountdown(240000, 60000);
						}
						CameraJumper.TryJump(t, CameraJumper.MovementMode.Pan);
					});
				}
				LongEventHandler.QueueLongEvent(action, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
			};
			if (!SettleInEmptyTileUtility.CanCreateMapAt(caravan.Tile, false) || Find.WorldObjects.AnyMapParentAt(caravan.Tile))
			{
				command_Action.Disable("CommandCampFailExistingWorldObject".Translate());
			}
			return command_Action;
		}

		// Token: 0x06015741 RID: 87873 RVA: 0x0066DBB8 File Offset: 0x0066BDB8
		public static bool CanCreateMapAt(PlanetTile tile, bool forGravship = false)
		{
			using (IEnumerator<WorldObject> enumerator = Find.WorldObjects.ObjectsAt(tile).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.def.canHaveMap)
					{
						return false;
					}
				}
			}
			return !Find.WorldObjects.AnyMapParentAt(tile) && Current.Game.FindMap(tile) == null;
		}

		// Token: 0x0400EED7 RID: 61143
		private const int MinStartingLocCellsCount = 600;

		// Token: 0x0400EED8 RID: 61144
		private static StringBuilder tmpSettleFailReason = new StringBuilder();
	}
}
