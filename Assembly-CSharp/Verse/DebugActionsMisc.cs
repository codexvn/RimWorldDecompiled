using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Profile;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200096D RID: 2413
	public static class DebugActionsMisc
	{
		// Token: 0x06003D9A RID: 15770 RVA: 0x001427CC File Offset: 0x001409CC
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllPlants()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				if (thing is Plant)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x00142838 File Offset: 0x00140A38
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllThings()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x00142898 File Offset: 0x00140A98
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyClutter()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Chunk).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
			foreach (Thing thing2 in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Filth).ToList<Thing>())
			{
				thing2.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x0014294C File Offset: 0x00140B4C
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllHats()
		{
			foreach (Pawn pawn in PawnsFinder.AllMaps)
			{
				if (pawn.RaceProps.Humanlike)
				{
					for (int i = pawn.apparel.WornApparel.Count - 1; i >= 0; i--)
					{
						Apparel apparel = pawn.apparel.WornApparel[i];
						if (apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))
						{
							apparel.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x00142A18 File Offset: 0x00140C18
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllCorpses()
		{
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x00142A56 File Offset: 0x00140C56
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FinishAllResearch()
		{
			Find.ResearchManager.DebugSetAllProjectsFinished();
			Find.EntityCodex.debug_UnhideAllResearch = true;
			Messages.Message("All research finished.", MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00142A80 File Offset: 0x00140C80
		[DebugAction("General", "Add techprint to project", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddTechprintsForProject()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ResearchProjectDef localProject2 in from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.TechprintRequirementMet
			select p)
			{
				ResearchProjectDef localProject = localProject2;
				list.Add(new DebugMenuOption(localProject.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					Find.ResearchManager.AddTechprints(localProject, localProject.TechprintCount - Find.ResearchManager.GetTechprints(localProject));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x00142B38 File Offset: 0x00140D38
		[DebugAction("General", "Apply techprint on project", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ApplyTechprintsForProject()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ResearchProjectDef localProject2 in from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.TechprintRequirementMet
			select p)
			{
				ResearchProjectDef localProject = localProject2;
				Action <>9__2;
				list.Add(new DebugMenuOption(localProject.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<DebugMenuOption> list3 = list2;
					string label = "None";
					DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
					Action method;
					if ((method = <>9__2) == null)
					{
						method = (<>9__2 = delegate()
						{
							Find.ResearchManager.ApplyTechprint(localProject, null);
						});
					}
					list3.Add(new DebugMenuOption(label, mode, method));
					foreach (Pawn localColonist2 in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
					{
						Pawn localColonist = localColonist2;
						list2.Add(new DebugMenuOption(localColonist.LabelCap, DebugMenuOptionMode.Action, delegate()
						{
							Find.ResearchManager.ApplyTechprint(localProject, localColonist);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x00142BF0 File Offset: 0x00140DF0
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> AddTradeShipOfKind()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<TraderKindDef> enumerator = (from t in DefDatabase<TraderKindDef>.AllDefs
			where t.orbital
			select t).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TraderKindDef traderKind = enumerator.Current;
					list.Add(new DebugActionNode(traderKind.label, DebugActionType.Action, delegate()
					{
						Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.traderKind = traderKind;
						IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x00142C90 File Offset: 0x00140E90
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ReplaceAllTradeShips()
		{
			Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
			for (int i = 0; i < 5; i++)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = Find.CurrentMap;
				IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
			}
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x00142CDC File Offset: 0x00140EDC
		[DebugAction("General", "Change weather...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> ChangeWeather()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (WeatherDef localWeather2 in DefDatabase<WeatherDef>.AllDefs)
			{
				WeatherDef localWeather = localWeather2;
				list.Add(new DebugActionNode(localWeather.LabelCap, DebugActionType.Action, delegate()
				{
					Find.CurrentMap.weatherManager.TransitionTo(localWeather);
				}, null));
			}
			return list;
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x00142D60 File Offset: 0x00140F60
		[DebugAction("General", "Celestial debugger", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OpenCelestialDebugger()
		{
			Dialog_DevCelestial dialog_DevCelestial;
			if (Find.WindowStack.TryGetWindow<Dialog_DevCelestial>(out dialog_DevCelestial))
			{
				dialog_DevCelestial.Close(true);
			}
			Find.WindowStack.Add(new Dialog_DevCelestial());
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x00142D94 File Offset: 0x00140F94
		[DebugAction("Sound", "Music debugger", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OpenMusicDebugger()
		{
			Dialog_DevMusic dialog_DevMusic;
			if (Find.WindowStack.TryGetWindow<Dialog_DevMusic>(out dialog_DevMusic))
			{
				dialog_DevMusic.Close(true);
			}
			Find.WindowStack.Add(new Dialog_DevMusic());
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00142DC8 File Offset: 0x00140FC8
		[DebugAction("General", "World noise visualizer", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.WorldRenderedNow)]
		private static void OpenWorldNoiseDebugger()
		{
			Dialog_DevNoiseWorld dialog_DevNoiseWorld;
			if (Find.WindowStack.TryGetWindow<Dialog_DevNoiseWorld>(out dialog_DevNoiseWorld))
			{
				dialog_DevNoiseWorld.Close(true);
			}
			Find.WindowStack.Add(new Dialog_DevNoiseWorld());
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x00142DFC File Offset: 0x00140FFC
		[DebugAction("General", "Map noise visualizer", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		private static void OpenMapNoiseDebugger()
		{
			Dialog_DevNoiseMap dialog_DevNoiseMap;
			if (Find.WindowStack.TryGetWindow<Dialog_DevNoiseMap>(out dialog_DevNoiseMap))
			{
				dialog_DevNoiseMap.Close(true);
			}
			Find.WindowStack.Add(new Dialog_DevNoiseMap());
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x00142E2D File Offset: 0x0014102D
		[DebugAction("Sound", "Test music fadeout and silence", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestFadeoutAndSilence()
		{
			Find.MusicManagerPlay.ForceFadeoutAndSilenceFor(120f, 5f, true);
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x00142E44 File Offset: 0x00141044
		[DebugAction("Sound", "Play song...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> PlaySong()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SongDef localSong2 in DefDatabase<SongDef>.AllDefs)
			{
				SongDef localSong = localSong2;
				list.Add(new DebugActionNode(localSong.defName, DebugActionType.Action, delegate()
				{
					Find.MusicManagerPlay.ForcePlaySong(localSong, false);
				}, null));
			}
			return list;
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x00142EC4 File Offset: 0x001410C4
		[DebugAction("Sound", "Trigger transition...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TriggerTransition()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (MusicTransitionDef local2 in DefDatabase<MusicTransitionDef>.AllDefs)
			{
				MusicTransitionDef local = local2;
				list.Add(new DebugActionNode(local.defName, DebugActionType.Action, delegate()
				{
					Find.MusicManagerPlay.ForceTriggerTransition(local);
				}, null));
			}
			return list;
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x00142F44 File Offset: 0x00141144
		[DebugAction("Sound", "Play sound...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> PlaySound()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SoundDef localSd2 in from s in DefDatabase<SoundDef>.AllDefs
			where !s.sustain
			select s)
			{
				SoundDef localSd = localSd2;
				list.Add(new DebugActionNode(localSd.defName, DebugActionType.Action, delegate()
				{
					if (localSd.subSounds.Any((SubSoundDef sub) => sub.onCamera))
					{
						localSd.PlayOneShotOnCamera(null);
						return;
					}
					localSd.PlayOneShot(SoundInfo.InMap(new TargetInfo(Find.CameraDriver.MapPosition, Find.CurrentMap, false), MaintenanceType.None));
				}, null));
			}
			return list;
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x00142FE8 File Offset: 0x001411E8
		[DebugAction("General", "End game condition...", false, false, false, false, false, 0, false, allowedGameStates = (AllowedGameStates.Playing | AllowedGameStates.IsCurrentlyOnMap | AllowedGameStates.HasGameCondition))]
		private static void EndGameCondition()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GameCondition localMc2 in Find.CurrentMap.gameConditionManager.ActiveConditions)
			{
				GameCondition localMc = localMc2;
				list.Add(new DebugMenuOption(localMc.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					localMc.End();
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x00143084 File Offset: 0x00141284
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static List<DebugActionNode> SimulateSanguophageMeeting()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 1; i <= 8; i++)
			{
				int num = i;
				list.Add(new DebugActionNode(num.ToString() + " sanguophages", DebugActionType.ToolMap, delegate()
				{
					List<FactionRelation> list2 = new List<FactionRelation>();
					foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
					{
						if (!faction.def.PermanentlyHostileTo(FactionDefOf.Sanguophages))
						{
							list2.Add(new FactionRelation(faction, FactionRelationKind.Neutral));
						}
					}
					Faction faction2 = FactionGenerator.NewGeneratedFactionWithRelations(FactionDefOf.Sanguophages, list2, true);
					faction2.temporary = true;
					Find.FactionManager.Add(faction2);
					List<Pawn> list3 = new List<Pawn>();
					PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Sanguophage, faction2, PawnGenerationContext.NonPlayer, null, true, false, false, true, true, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
					for (int j = 0; j < num; j++)
					{
						list3.Add(PawnGenerator.GeneratePawn(request));
					}
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					PawnsArrivalModeDef edgeWalkIn = PawnsArrivalModeDefOf.EdgeWalkIn;
					edgeWalkIn.Worker.TryResolveRaidSpawnCenter(incidentParms);
					edgeWalkIn.Worker.Arrive(list3, incidentParms);
					LordMaker.MakeNewLord(faction2, new LordJob_SanguophageMeeting(UI.MouseCell(), new List<Thing>(), 60000, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty), Find.CurrentMap, list3);
				}, null));
			}
			return list;
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x001430E0 File Offset: 0x001412E0
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceEnemyAssault()
		{
			foreach (Lord lord in Find.CurrentMap.lordManager.lords)
			{
				LordToil_Stage lordToil_Stage = lord.CurLordToil as LordToil_Stage;
				if (lordToil_Stage != null)
				{
					foreach (Transition transition in lord.Graph.transitions)
					{
						if (transition.sources.Contains(lordToil_Stage) && (transition.target is LordToil_AssaultColony || transition.target is LordToil_AssaultColonyBreaching || transition.target is LordToil_AssaultColonyPrisoners || transition.target is LordToil_AssaultColonySappers || transition.target is LordToil_AssaultColonyBossgroup || transition.target is LordToil_MoveInBossgroup))
						{
							string str = "Debug forcing to assault toil: ";
							Faction faction = lord.faction;
							Messages.Message(str + ((faction != null) ? faction.ToString() : null), MessageTypeDefOf.TaskCompletion, false);
							lord.GotoToil(transition.target);
							return;
						}
					}
				}
			}
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (quest.State == QuestState.Ongoing)
				{
					foreach (QuestPart questPart in quest.PartsListForReading)
					{
						QuestPart_BossgroupArrives questPart_BossgroupArrives = questPart as QuestPart_BossgroupArrives;
						if (questPart_BossgroupArrives != null && questPart_BossgroupArrives.State == QuestPartState.Enabled)
						{
							questPart_BossgroupArrives.DebugForceComplete();
							Messages.Message("Debug forcing bossgroup assault.", MessageTypeDefOf.TaskCompletion, false);
						}
					}
				}
			}
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00143318 File Offset: 0x00141518
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceEnemyFlee()
		{
			foreach (Lord lord in Find.CurrentMap.lordManager.lords)
			{
				if (lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer) && lord.faction.def.autoFlee)
				{
					LordToil lordToil = lord.Graph.lordToils.FirstOrDefault((LordToil st) => st is LordToil_PanicFlee);
					if (lordToil != null)
					{
						lord.GotoToil(lordToil);
					}
				}
			}
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x001433D4 File Offset: 0x001415D4
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AdaptionProgress10Days()
		{
			Find.StoryWatcher.watcherAdaptation.Debug_OffsetAdaptDays(10f);
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x001433EA File Offset: 0x001415EA
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void UnloadUnusedAssets()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x001433F4 File Offset: 0x001415F4
		[DebugAction("General", "Name settlement...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void NameSettlement()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Faction", DebugMenuOptionMode.Action, delegate()
			{
				Find.WindowStack.Add(new Dialog_NamePlayerFaction());
			}));
			if (Find.CurrentMap != null && Find.CurrentMap.IsPlayerHome && Find.CurrentMap.Parent is Settlement)
			{
				Settlement factionBase = (Settlement)Find.CurrentMap.Parent;
				list.Add(new DebugMenuOption("Faction base", DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_NamePlayerSettlement(factionBase));
				}));
				list.Add(new DebugMenuOption("Faction and faction base", DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(factionBase));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x001434C1 File Offset: 0x001416C1
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void NextLesson()
		{
			LessonAutoActivator.DebugForceInitiateBestLessonNow();
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x001434C8 File Offset: 0x001416C8
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 900)]
		private static List<DebugActionNode> ChangeCameraConfig()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			list.Add(new DebugActionNode("Open editor", DebugActionType.Action, delegate()
			{
				Find.WindowStack.Add(new Dialog_CameraConfig());
			}, null));
			List<DebugActionNode> list2 = list;
			foreach (Type localType2 in typeof(CameraMapConfig).AllSubclasses())
			{
				Type localType = localType2;
				string text = localType.Name;
				if (text.StartsWith("CameraMapConfig_"))
				{
					text = text.Substring("CameraMapConfig_".Length);
				}
				list2.Add(new DebugActionNode(text, DebugActionType.Action, delegate()
				{
					Find.CameraDriver.config = (CameraMapConfig)Activator.CreateInstance(localType);
				}, null));
			}
			return list2;
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x001435AC File Offset: 0x001417AC
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ForceShipCountdown()
		{
			ShipCountdown.InitiateCountdown(null);
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x001435B4 File Offset: 0x001417B4
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ForceStartShip()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return;
			}
			Building_ShipComputerCore building_ShipComputerCore = (Building_ShipComputerCore)currentMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Ship_ComputerCore).FirstOrDefault<Building>();
			if (building_ShipComputerCore == null)
			{
				Messages.Message("Could not find any compute core on current map!", MessageTypeDefOf.NeutralEvent, true);
			}
			building_ShipComputerCore.ForceLaunch();
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x00143600 File Offset: 0x00141800
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashTradeDropSpot()
		{
			IntVec3 intVec = DropCellFinder.TradeDropSpot(Find.CurrentMap);
			Find.CurrentMap.debugDrawer.FlashCell(intVec, 0f, null, 50);
			string str = "trade drop spot: ";
			IntVec3 intVec2 = intVec;
			Log.Message(str + intVec2.ToString());
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x00143650 File Offset: 0x00141850
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeFactionLeader(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactionsVisible.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction faction = enumerator.Current;
					list.Add(new DebugMenuOption(faction.Name, DebugMenuOptionMode.Action, delegate()
					{
						if (faction.leader != p)
						{
							faction.leader = p;
							if (ModsConfig.IdeologyActive)
							{
								foreach (Precept precept in faction.ideos.PrimaryIdeo.PreceptsListForReading)
								{
									Precept_Role precept_Role = precept as Precept_Role;
									if (precept_Role != null && precept_Role.def.leaderRole)
									{
										precept_Role.Assign(p, false);
										break;
									}
								}
							}
							DebugActionsUtility.DustPuffFrom(p);
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x001436F4 File Offset: 0x001418F4
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillFactionLeader()
		{
			Pawn leader = (from x in Find.FactionManager.AllFactions
			where x.leader != null
			select x).RandomElement<Faction>().leader;
			int num = 0;
			while (!leader.Dead)
			{
				if (++num > 1000)
				{
					Log.Warning("Could not kill faction leader.");
					return;
				}
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Bullet, 30f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetIgnoreInstantKillProtection(true);
				leader.TakeDamage(dinfo);
			}
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x00143794 File Offset: 0x00141994
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillKidnappedPawn()
		{
			IEnumerable<Pawn> pawnsBySituation = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.Kidnapped);
			if (pawnsBySituation.Any<Pawn>())
			{
				Pawn pawn = pawnsBySituation.RandomElement<Pawn>();
				pawn.Kill(null, null);
				Messages.Message("Killed " + pawn.LabelCap, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x001437E8 File Offset: 0x001419E8
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
			{
				Pawn pLocal = pawn;
				list.Add(new DebugMenuOption(pawn.LabelShort + "(" + pawn.kindDef.label + ")", DebugMenuOptionMode.Action, delegate()
				{
					pLocal.Kill(null, null);
					Messages.Message("Killed " + pLocal.LabelCap, MessageTypeDefOf.NeutralEvent, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x00143894 File Offset: 0x00141A94
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetFactionRelations()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac3 in Find.FactionManager.AllFactionsVisibleInViewOrder)
			{
				Faction localFac = localFac3;
				foreach (object obj in Enum.GetValues(typeof(FactionRelationKind)))
				{
					FactionRelationKind localRk2 = (FactionRelationKind)obj;
					FactionRelationKind localRk = localRk2;
					Faction localFac2 = localFac;
					FloatMenuOption item = new FloatMenuOption(((localFac2 != null) ? localFac2.ToString() : null) + " - " + localRk.ToString(), delegate()
					{
						if (localRk == FactionRelationKind.Hostile)
						{
							Faction.OfPlayer.TryAffectGoodwillWith(localFac, -100, true, true, HistoryEventDefOf.DebugGoodwill, null);
							return;
						}
						if (localRk == FactionRelationKind.Ally)
						{
							Faction.OfPlayer.TryAffectGoodwillWith(localFac, 100, true, true, HistoryEventDefOf.DebugGoodwill, null);
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x001439D4 File Offset: 0x00141BD4
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void VisitorGift()
		{
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.AllPawnsSpawned)
			{
				if (pawn.Faction != null && !pawn.Faction.IsPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					list.Add(pawn);
					break;
				}
			}
			VisitorGiftForPlayerUtility.GiveRandomGift(list, list[0].Faction);
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x00143A6C File Offset: 0x00141C6C
		[DebugAction("General", "Increment time", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 500)]
		private static List<DebugActionNode> IncrementTime()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < DebugActionsMisc.TimeIncreases.Length; i++)
			{
				int durationLocal = DebugActionsMisc.TimeIncreases[i];
				list.Add(new DebugActionNode(durationLocal.ToStringTicksToPeriod(true, false, true, true, false), DebugActionType.Action, delegate()
				{
					Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + durationLocal);
				}, null));
			}
			return list;
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x00143AD0 File Offset: 0x00141CD0
		[DebugAction("General", "Storywatcher tick 1 day", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void StorywatcherTick1Day()
		{
			for (int i = 0; i < 60000; i++)
			{
				Find.StoryWatcher.StoryWatcherTick();
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1);
			}
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x00143B10 File Offset: 0x00141D10
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void KillRandomLentColonist()
		{
			if (QuestUtility.TotalBorrowedColonistCount() > 0)
			{
				DebugActionsMisc.tmpLentColonists.Clear();
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (questsListForReading[i].State == QuestState.Ongoing)
					{
						List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
						for (int j = 0; j < partsListForReading.Count; j++)
						{
							QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = partsListForReading[j] as QuestPart_LendColonistsToFaction;
							if (questPart_LendColonistsToFaction != null)
							{
								List<Thing> lentColonistsListForReading = questPart_LendColonistsToFaction.LentColonistsListForReading;
								for (int k = 0; k < lentColonistsListForReading.Count; k++)
								{
									Pawn pawn = lentColonistsListForReading[k] as Pawn;
									if (pawn != null && !pawn.Dead)
									{
										DebugActionsMisc.tmpLentColonists.Add(pawn);
									}
								}
							}
						}
					}
				}
				Pawn pawn2 = DebugActionsMisc.tmpLentColonists.RandomElement<Pawn>();
				bool flag = pawn2.health.hediffSet.hediffs.Any((Hediff x) => x.def.isBad);
				pawn2.Kill(null, flag ? pawn2.health.hediffSet.hediffs.RandomElement<Hediff>() : null);
			}
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x00143C51 File Offset: 0x00141E51
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ClearPrisonerInteractionSchedule(Pawn p)
		{
			if (p.IsPrisonerOfColony)
			{
				p.mindState.lastAssignedInteractTime = -1;
				p.mindState.interactionsToday = 0;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x00143C7C File Offset: 0x00141E7C
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -100)]
		private static void GlowAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (c.InBounds(currentMap))
				{
					float num = Find.CurrentMap.glowGrid.GroundGlowAt(c, false, false);
					currentMap.debugDrawer.FlashCell(c, 0f, num.ToString("F1"), 100);
				}
			}
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x00143D10 File Offset: 0x00141F10
		[DebugAction("General", "HSV At Position", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -100)]
		private static void HSVAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (c.InBounds(currentMap))
				{
					float num;
					float num2;
					float num3;
					Color.RGBToHSV(Find.CurrentMap.glowGrid.VisualGlowAt(c), out num, out num2, out num3);
					currentMap.debugDrawer.FlashCell(c, 0.5f, string.Format("HSV({0:.0#},{1:.0#},{2:.0#})", num, num2, num3), 100);
				}
			}
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x00143DC0 File Offset: 0x00141FC0
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashBlockedLandingCells()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in currentMap.AllCells)
			{
				if (!c.Fogged(currentMap) && !DropCellFinder.IsGoodDropSpot(c, currentMap, false, false, false))
				{
					currentMap.debugDrawer.FlashCell(c, 0f, "bl", 50);
				}
			}
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x00143E3C File Offset: 0x0014203C
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> PawnKindApparelCheck()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef3 in from kd in DefDatabase<PawnKindDef>.AllDefs
			where kd.race == ThingDefOf.Human
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef3;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.Action, delegate()
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef);
					bool flag = false;
					for (int i = 0; i < 100; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, faction, null);
						if (pawn.royalty != null)
						{
							RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
							if (mostSeniorTitle != null && !mostSeniorTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
							{
								for (int j = 0; j < mostSeniorTitle.def.requiredApparel.Count; j++)
								{
									ApparelRequirement apparelRequirement = mostSeniorTitle.def.requiredApparel[j];
									if (apparelRequirement.IsActive(pawn) && !apparelRequirement.IsMet(pawn))
									{
										string[] array = new string[6];
										int num = 0;
										PawnKindDef localKindDef2 = localKindDef;
										array[num] = ((localKindDef2 != null) ? localKindDef2.ToString() : null);
										array[1] = " (";
										array[2] = mostSeniorTitle.def.label;
										array[3] = ")  does not have its title requirements met. index=";
										array[4] = j.ToString();
										array[5] = DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|49_0(pawn);
										Log.Error(string.Concat(array));
										flag = true;
									}
								}
							}
						}
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int k = 0; k < wornApparel.Count; k++)
						{
							string text = DebugActionsMisc.<PawnKindApparelCheck>g__apparelOkayToWear|49_1(pawn, wornApparel[k]);
							if (text != "OK")
							{
								Log.Error(text + " - " + wornApparel[k].Label + DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|49_0(pawn));
								flag = true;
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (!flag)
					{
						Log.Message("No errors for " + localKindDef.defName);
					}
				}, null));
			}
			return list;
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x00143F04 File Offset: 0x00142104
		[DebugAction("General", "Edit effecter...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> EditEffecter()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (EffecterDef localDef2 in DefDatabase<EffecterDef>.AllDefs)
			{
				EffecterDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, delegate()
				{
					if (!Find.WindowStack.TryRemove(typeof(EditWindow_DefEditor), true))
					{
						Find.WindowStack.Add(new EditWindow_DefEditor(localDef));
					}
				}, null));
			}
			return list;
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x00143F84 File Offset: 0x00142184
		[DebugAction("General", "Edit Animation...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> EditAnimation()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (AnimationDef localDef2 in DefDatabase<AnimationDef>.AllDefs)
			{
				AnimationDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, delegate()
				{
					if (!Find.WindowStack.TryRemove(typeof(EditWindow_DefEditor), true))
					{
						Find.WindowStack.Add(new EditWindow_DefEditor(localDef));
					}
				}, null));
			}
			return list;
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x00144004 File Offset: 0x00142204
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> PawnKindAbilityCheck()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef3 in from kd in DefDatabase<PawnKindDef>.AllDefs
			where kd.titleRequired != null || !kd.titleSelectOne.NullOrEmpty<RoyalTitleDef>()
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef3;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.Action, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef);
					for (int i = 0; i < 100; i++)
					{
						RoyalTitleDef royalTitleDef = null;
						if (localKindDef.titleRequired != null)
						{
							royalTitleDef = localKindDef.titleRequired;
						}
						else if (!localKindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() && Rand.Chance(localKindDef.royalTitleChance))
						{
							royalTitleDef = localKindDef.titleSelectOne.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
						}
						PawnKindDef localKindDef2 = localKindDef;
						Faction faction2 = faction;
						PawnGenerationContext context = PawnGenerationContext.NonPlayer;
						RoyalTitleDef fixedTitle = royalTitleDef;
						Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(localKindDef2, faction2, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, fixedTitle, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
						RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
						if (mostSeniorTitle != null)
						{
							Hediff_Psylink mainPsylinkSource = pawn.GetMainPsylinkSource();
							if (mainPsylinkSource == null)
							{
								if (mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def) > 0)
								{
									string text = mostSeniorTitle.def.LabelCap + " - No psylink.";
									if (pawn.abilities.abilities.Any((Ability x) => x.def.IsPsycast && x.def.level > 0))
									{
										text += " Has psycasts without psylink.";
									}
									stringBuilder.AppendLine(text);
								}
							}
							else if (mainPsylinkSource.level < mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def))
							{
								stringBuilder.AppendLine("Psylink at level " + mainPsylinkSource.level.ToString() + ", but requires " + mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def).ToString());
							}
							else if (mainPsylinkSource.level > mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def))
							{
								stringBuilder.AppendLine("Psylink at level " + mainPsylinkSource.level.ToString() + ". Max is " + mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def).ToString());
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (stringBuilder.Length == 0)
					{
						Log.Message("No errors for " + localKindDef.defName);
						return;
					}
					string str = "Errors:\n";
					StringBuilder stringBuilder2 = stringBuilder;
					Log.Error(str + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null));
				}, null));
			}
			return list;
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x001440CC File Offset: 0x001422CC
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		public static void AtlasRebuild()
		{
			GlobalTextureAtlasManager.rebakeAtlas = true;
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x001440D4 File Offset: 0x001422D4
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DumpPawnAtlases()
		{
			string text = Application.dataPath + "\\atlasDump_Pawn";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			GlobalTextureAtlasManager.DumpPawnAtlases(text);
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x00144108 File Offset: 0x00142308
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DumpStaticAtlases()
		{
			string text = Application.dataPath + "\\atlasDump_Static";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			GlobalTextureAtlasManager.DumpStaticAtlases(text);
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x0014413C File Offset: 0x0014233C
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, name = "Set Anomaly level...", allowedGameStates = AllowedGameStates.Playing, requiresAnomaly = true)]
		private static List<DebugActionNode> SetMonolithLevel()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<MonolithLevelDef> enumerator = DefDatabase<MonolithLevelDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MonolithLevelDef def = enumerator.Current;
					list.Add(new DebugActionNode(def.defName, DebugActionType.Action, delegate()
					{
						Find.Anomaly.SetLevel(def, false);
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x001441B8 File Offset: 0x001423B8
		[DebugAction("General", null, false, false, false, false, false, 0, false, name = "Hot reload Defs", allowedGameStates = AllowedGameStates.Playing, displayPriority = 9999)]
		private static void HotReloadDefs()
		{
			PlayDataLoader.HotReloadDefs();
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x001441BF File Offset: 0x001423BF
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void SetGraphicsDirty(Pawn p)
		{
			p.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x001441D4 File Offset: 0x001423D4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ToggleMovement(Pawn p)
		{
			p.pather.debugDisabled = !p.pather.debugDisabled;
			MoteMaker.ThrowText(p.Position.ToVector3(), Find.CurrentMap, "Movement " + (p.pather.debugDisabled ? "Disabled" : "Enabled"), -1f);
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x0014423C File Offset: 0x0014243C
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ToggleMaxMoveSpeed(Pawn p)
		{
			p.debugMaxMoveSpeed = !p.debugMaxMoveSpeed;
			MoteMaker.ThrowText(p.Position.ToVector3(), Find.CurrentMap, "Max MoveSpeed " + (p.debugMaxMoveSpeed ? "Enabled" : "Disabled"), -1f);
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x00144294 File Offset: 0x00142494
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static List<DebugActionNode> LockRotation()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Rot4 lRot2 in Rot4.AllRotations)
			{
				Rot4 lRot = lRot2;
				list.Add(new DebugActionNode(lRot2.ToStringHuman() ?? "", DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn pawn)
					{
						if (pawn.debugRotLocked)
						{
							pawn.debugRotLocked = false;
						}
						pawn.Rotation = lRot;
						pawn.debugRotLocked = true;
						MoteMaker.ThrowText(pawn.Position.ToVector3(), Find.CurrentMap, "Rot Locked", -1f);
					}
				});
			}
			return list;
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x00144320 File Offset: 0x00142520
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void UnlockRotation(Pawn p)
		{
			if (!p.debugRotLocked)
			{
				return;
			}
			p.debugRotLocked = false;
			MoteMaker.ThrowText(p.Position.ToVector3(), Find.CurrentMap, "Rot Unlocked", -1f);
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x0014435F File Offset: 0x0014255F
		[DebugAction("General", "Reveal Hidden Defs", false, false, false, false, false, 0, false)]
		private static void RevealHiddenDefs()
		{
			Find.HiddenItemsManager.ClearHiddenDefs();
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x0014436C File Offset: 0x0014256C
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, name = "End revenant hypnosis", requiresAnomaly = true)]
		private static void EndRevenantHypnosis(Pawn p)
		{
			Hediff hediff = p.health.hediffSet.hediffs.Find((Hediff hd) => hd.def == HediffDefOf.RevenantHypnosis);
			if (hediff != null)
			{
				p.health.RemoveHediff(hediff);
			}
			p.stances.stunner.StopStun();
			Find.Anomaly.EndHypnotize(p);
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x001443D8 File Offset: 0x001425D8
		[DebugAction("General", "Layer pathfinder...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void LayerPathfinder()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer origin2 = planetLayer;
				PlanetLayer origin = origin2;
				list.Add(new DebugMenuOption(origin.Def.label, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (KeyValuePair<int, PlanetLayer> keyValuePair2 in Find.WorldGrid.PlanetLayers)
					{
						int num2;
						PlanetLayer planetLayer2;
						keyValuePair2.Deconstruct(out num2, out planetLayer2);
						PlanetLayer dest2 = planetLayer2;
						PlanetLayer dest = dest2;
						if (dest != origin)
						{
							list2.Add(new DebugMenuOption(dest.Def.label, DebugMenuOptionMode.Action, delegate()
							{
								List<PlanetLayerConnection> list3 = new List<PlanetLayerConnection>();
								float num3;
								string text;
								if (origin.TryGetPath(dest, list3, out num3, 2147483647))
								{
									text = string.Format("Found path between {0} and {1}, cost: {2}", origin.Def.label, dest.Def.label, num3);
									using (List<PlanetLayerConnection>.Enumerator enumerator3 = list3.GetEnumerator())
									{
										while (enumerator3.MoveNext())
										{
											PlanetLayerConnection planetLayerConnection = enumerator3.Current;
											text += string.Format("\n {0} ({1}) -> {2}", planetLayerConnection.origin.Def.label, planetLayerConnection.fuelCost, planetLayerConnection.target.Def.label);
										}
										goto IL_FE;
									}
								}
								text = "No path between " + origin.Def.label + " and " + dest.Def.label;
								IL_FE:
								Log.Message(text);
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, "Select destination"));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, "Select origin"));
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x00144484 File Offset: 0x00142684
		[DebugAction("Other", "Clear cached materials", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Invalid, hideInSubMenu = true)]
		private static void ClearCachedMaterials()
		{
			MatLoader.ClearCache();
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x0014448B File Offset: 0x0014268B
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void LogCellMapData()
		{
			Find.CurrentMap.pathFinder.MapData.LogCell(UI.MouseCell());
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x001444A6 File Offset: 0x001426A6
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		private static void LogPathfinderState()
		{
			Find.CurrentMap.pathFinder.LogPathfinderState();
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x001444B7 File Offset: 0x001426B7
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void LogCellGridResult()
		{
			Find.CurrentMap.pathFinder.LogGridCellResult(UI.MouseCell());
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x001444D0 File Offset: 0x001426D0
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Breach()
		{
			IntVec3 start;
			Action <>9__1;
			DebugTools.curTool = new DebugTool("start...", delegate()
			{
				start = UI.MouseCell();
				string label = "end...";
				Action clickAction;
				if ((clickAction = <>9__1) == null)
				{
					clickAction = (<>9__1 = delegate()
					{
						IntVec3 end = UI.MouseCell();
						new BreachingGrid(Find.CurrentMap, null).CreateBreachPath(start, end, 1, 3, true);
						DebugTools.curTool = null;
					});
				}
				DebugTools.curTool = new DebugTool(label, clickAction, null);
			}, null);
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x00144500 File Offset: 0x00142700
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Goto(Pawn pawn)
		{
			DebugTools.curTool = new DebugTool("Select destination...", delegate()
			{
				IntVec3 c = UI.MouseCell();
				if (!pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					MoteMaker.ThrowText(c.ToVector3(), Find.CurrentMap, "Cannot reach cell", -1f);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Goto, c);
				job.locomotionUrgency = LocomotionUrgency.Jog;
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				DebugTools.curTool = null;
			}, null);
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x00144538 File Offset: 0x00142738
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.Playing, requiresOdyssey = true)]
		private static void RetroactivelyAddLandmarksToWorld()
		{
			new WorldGenStep_Landmarks().GenerateFresh(Find.World.info.seedString, Find.WorldGrid.Surface);
			Find.World.renderer.GetLayer<WorldDrawLayer_Terrain>(Find.WorldGrid.Surface).RegenerateNow();
			Find.World.renderer.GetLayer<WorldDrawLayer_Landmarks>(Find.WorldGrid.Surface).RegenerateNow();
			Find.World.renderer.GetLayer<WorldDrawLayer_Hills>(Find.WorldGrid.Surface).RegenerateNow();
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x001445C4 File Offset: 0x001427C4
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.Playing, requiresOdyssey = true)]
		private static void RegenerateMapFeatures()
		{
			foreach (Tile tile in Find.WorldGrid.Surface.Tiles)
			{
				List<TileMutatorDef> mutatorsNullable = tile.mutatorsNullable;
				if (mutatorsNullable != null)
				{
					mutatorsNullable.Clear();
				}
			}
			WorldGenStep_Mutators.AddMutatorsFromTile(Find.WorldGrid.Surface);
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x0014465C File Offset: 0x0014285C
		[CompilerGenerated]
		internal static string <PawnKindApparelCheck>g__logApparel|49_0(Pawn p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Apparel of " + p.LabelShort + ":");
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				stringBuilder.AppendLine("  - " + wornApparel[i].Label);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x001446D4 File Offset: 0x001428D4
		[CompilerGenerated]
		internal static string <PawnKindApparelCheck>g__apparelOkayToWear|49_1(Pawn pawn, Apparel apparel)
		{
			ApparelProperties app = apparel.def.apparel;
			if (!pawn.kindDef.apparelRequired.NullOrEmpty<ThingDef>() && pawn.kindDef.apparelRequired.Contains(apparel.def))
			{
				return "OK";
			}
			if (!app.PawnCanWear(pawn, false))
			{
				return "Pawn cannot wear.";
			}
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (PawnApparelGenerator.ApparelRequirementHandlesThing(specificApparelRequirements[i], apparel.def) && PawnApparelGenerator.ApparelRequirementTagsMatch(specificApparelRequirements[i], apparel.def))
					{
						return "OK";
					}
				}
			}
			if (!pawn.kindDef.apparelTags.NullOrEmpty<string>())
			{
				if (!app.tags.Any((string tag) => pawn.kindDef.apparelTags.Contains(tag)))
				{
					return "Required tag missing.";
				}
				if ((pawn.royalty == null || pawn.royalty.MostSeniorTitle == null) && app.tags.Contains("Royal") && !pawn.kindDef.apparelTags.Any((string tag) => app.tags.Contains(tag)))
				{
					return "Royal apparel on non-royal pawn.";
				}
			}
			if (!pawn.kindDef.apparelDisallowTags.NullOrEmpty<string>() && pawn.kindDef.apparelDisallowTags.Any((string t) => app.tags.Contains(t)))
			{
				return "Has a disallowed tag.";
			}
			if (pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Any<RoyalTitle>())
			{
				RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
				QualityCategory qualityCategory;
				if (apparel.TryGetQuality(out qualityCategory) && qualityCategory < mostSeniorTitle.def.requiredMinimumApparelQuality)
				{
					return "Quality too low.";
				}
			}
			return "OK";
		}

		// Token: 0x04002C45 RID: 11333
		private static readonly int[] TimeIncreases = new int[]
		{
			2500,
			15000,
			30000,
			60000,
			900000
		};

		// Token: 0x04002C46 RID: 11334
		private static List<Pawn> tmpLentColonists = new List<Pawn>();

		// Token: 0x04002C47 RID: 11335
		private const string NoErrorString = "OK";

		// Token: 0x04002C48 RID: 11336
		private const string RoyalApparelTag = "Royal";

		// Token: 0x04002C49 RID: 11337
		private const int PawnsToGenerate = 100;
	}
}
