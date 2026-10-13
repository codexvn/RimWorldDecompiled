using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000935 RID: 2357
	public static class DebugActionsIncidents
	{
		// Token: 0x06003CCC RID: 15564 RVA: 0x0013EB70 File Offset: 0x0013CD70
		[DebugAction("Incidents", "Do trade caravan arrival...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DoTradeCaravanSpecific()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IncidentDef incidentDef = IncidentDefOf.TraderCaravanArrival;
			Map target = Find.CurrentMap;
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction faction = enumerator.Current;
					if (faction.def.caravanTraderKinds != null && faction.def.caravanTraderKinds.Any<TraderKindDef>())
					{
						list.Add(new DebugMenuOption(faction.Name, DebugMenuOptionMode.Action, delegate()
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							using (List<TraderKindDef>.Enumerator enumerator2 = faction.def.caravanTraderKinds.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									TraderKindDef traderKind = enumerator2.Current;
									string text = traderKind.label;
									IncidentParms parms = StorytellerUtility.DefaultParmsNow(incidentDef.category, target);
									parms.faction = faction;
									parms.traderKind = traderKind;
									if (!incidentDef.Worker.CanFireNow(parms))
									{
										text += " [NO]";
									}
									list2.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
									{
										IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incidentDef.category, target);
										incidentParms.forced = true;
										if (incidentDef.pointsScaleable)
										{
											incidentParms = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain).GenerateParms(incidentDef.category, parms.target);
										}
										incidentParms.faction = faction;
										incidentParms.traderKind = traderKind;
										incidentDef.Worker.TryExecute(incidentParms);
									}));
								}
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
						}));
					}
				}
			}
			if (list.Count == 0)
			{
				Messages.Message("No valid factions found for trade caravans", MessageTypeDefOf.RejectInput, false);
				return;
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x0013EC64 File Offset: 0x0013CE64
		[DebugAction("Incidents", "Execute raid with points...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> ExecuteRaidWithPoints()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (float localP2 in DebugActionsUtility.PointsOptions(true))
			{
				float localP = localP2;
				list.Add(new DebugActionNode(localP.ToString() + " points", DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.points = localP;
						incidentParms.forced = true;
						IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
					}
				});
			}
			return list;
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x0013ECF8 File Offset: 0x0013CEF8
		[DebugAction("Incidents", "Execute raid with faction...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteRaidWithFaction()
		{
			StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
			IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			parms.forced = true;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Func<RaidStrategyDef, bool> <>9__3;
			Func<PawnsArrivalModeDef, bool> <>9__4;
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate()
				{
					parms.faction = localFac;
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = localPoints2;
						list2.Add(new DebugMenuOption(localPoints2.ToString() + " points", DebugMenuOptionMode.Action, delegate()
						{
							parms.points = localPoints;
							IEnumerable<RaidStrategyDef> allDefs = DefDatabase<RaidStrategyDef>.AllDefs;
							Func<RaidStrategyDef, bool> predicate;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((RaidStrategyDef s) => s.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat)));
							}
							List<RaidStrategyDef> source = allDefs.Where(predicate).ToList<RaidStrategyDef>();
							parms.raidStrategy = source.RandomElement<RaidStrategyDef>();
							if (parms.raidStrategy != null)
							{
								IEnumerable<PawnsArrivalModeDef> allDefs2 = DefDatabase<PawnsArrivalModeDef>.AllDefs;
								Func<PawnsArrivalModeDef, bool> predicate2;
								if ((predicate2 = <>9__4) == null)
								{
									predicate2 = (<>9__4 = ((PawnsArrivalModeDef a) => a.Worker.CanUseWith(parms) && parms.raidStrategy.arriveModes.Contains(a)));
								}
								List<PawnsArrivalModeDef> source2 = allDefs2.Where(predicate2).ToList<PawnsArrivalModeDef>();
								parms.raidArrivalMode = source2.RandomElement<PawnsArrivalModeDef>();
							}
							DebugActionsIncidents.DoRaid(parms);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x0013EE10 File Offset: 0x0013D010
		[DebugAction("Incidents", "Psychic ritual siege...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static List<DebugActionNode> RitualSiegeWithSpecifics()
		{
			StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
			IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			parms.forced = true;
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<PsychicRitualDef> enumerator = (from x in DefDatabase<PsychicRitualDef>.AllDefs
			where x.aiCastable
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PsychicRitualDef def = enumerator.Current;
					DebugActionNode item = new DebugActionNode(def.label, DebugActionType.Action, delegate()
					{
						parms.psychicRitualDef = def;
						IncidentDefOf.PsychicRitualSiege.Worker.TryExecute(parms);
					}, null);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x0013EF18 File Offset: 0x0013D118
		[DebugAction("Incidents", "Execute raid with specifics...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteRaidWithSpecifics()
		{
			StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
			IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			parms.forced = true;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Action <>9__4;
			Action <>9__6;
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				Faction localFac = faction;
				string text = localFac.Name + " (" + localFac.def.defName + ")";
				if (!((IncidentWorker_PawnsArrive)(faction.HostileTo(Faction.OfPlayer) ? IncidentDefOf.RaidEnemy : IncidentDefOf.RaidFriendly).Worker).FactionCanBeGroupSource(faction, parms, false))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
				{
					parms.faction = localFac;
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = localPoints2;
						list2.Add(new DebugMenuOption(localPoints2.ToString() + " points", DebugMenuOptionMode.Action, delegate()
						{
							parms.points = localPoints;
							List<DebugMenuOption> list3 = new List<DebugMenuOption>();
							foreach (RaidStrategyDef localStrat2 in DefDatabase<RaidStrategyDef>.AllDefs)
							{
								RaidStrategyDef localStrat = localStrat2;
								string text2 = localStrat.defName;
								if (!localStrat.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat))
								{
									text2 += " [NO]";
								}
								list3.Add(new DebugMenuOption(text2, DebugMenuOptionMode.Action, delegate()
								{
									parms.raidStrategy = localStrat;
									List<DebugMenuOption> list4 = new List<DebugMenuOption>();
									List<DebugMenuOption> list5 = list4;
									string label = "-Random-";
									DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
									Action method;
									if ((method = <>9__4) == null)
									{
										method = (<>9__4 = delegate()
										{
											DebugActionsIncidents.DoRaid(parms);
										});
									}
									list5.Add(new DebugMenuOption(label, mode, method));
									foreach (PawnsArrivalModeDef localArrival2 in DefDatabase<PawnsArrivalModeDef>.AllDefs)
									{
										PawnsArrivalModeDef localArrival = localArrival2;
										string text3 = localArrival.defName;
										if (!localArrival.Worker.CanUseWith(parms) || !localStrat.arriveModes.Contains(localArrival))
										{
											text3 += " [NO]";
										}
										list4.Add(new DebugMenuOption(text3, DebugMenuOptionMode.Action, delegate()
										{
											parms.raidArrivalMode = localArrival;
											if (ModsConfig.BiotechActive)
											{
												List<DebugMenuOption> list6 = new List<DebugMenuOption>();
												List<DebugMenuOption> list7 = list6;
												string label2 = "-Random-";
												DebugMenuOptionMode mode2 = DebugMenuOptionMode.Action;
												Action method2;
												if ((method2 = <>9__6) == null)
												{
													method2 = (<>9__6 = delegate()
													{
														DebugActionsIncidents.DoRaid(parms);
													});
												}
												list7.Add(new DebugMenuOption(label2, mode2, method2));
												foreach (RaidAgeRestrictionDef localAgeRestriction2 in DefDatabase<RaidAgeRestrictionDef>.AllDefs)
												{
													RaidAgeRestrictionDef localAgeRestriction = localAgeRestriction2;
													string text4 = localAgeRestriction.defName;
													if (!localAgeRestriction.Worker.CanUseWith(parms))
													{
														text4 += " [NO]";
													}
													list6.Add(new DebugMenuOption(text4, DebugMenuOptionMode.Action, delegate()
													{
														parms.raidAgeRestriction = localAgeRestriction;
														DebugActionsIncidents.DoRaid(parms);
													}));
												}
												Find.WindowStack.Add(new Dialog_DebugOptionListLister(list6, null));
												return;
											}
											DebugActionsIncidents.DoRaid(parms);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4, null));
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x0013F080 File Offset: 0x0013D280
		[DebugAction("Incidents", "Drop pod raid at location...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteDropPodRaidAtLocation()
		{
			StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
			IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			parms.forced = true;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				Action<LocalTargetInfo> <>9__4;
				while (enumerator.MoveNext())
				{
					Faction fac = enumerator.Current;
					Faction localFac = fac;
					string text = localFac.Name + " (" + localFac.def.defName + ")";
					if (!((IncidentWorker_PawnsArrive)(fac.HostileTo(Faction.OfPlayer) ? IncidentDefOf.RaidEnemy : IncidentDefOf.RaidFriendly).Worker).FactionCanBeGroupSource(fac, parms, false))
					{
						text += " [NO]";
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
					{
						parms.faction = localFac;
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
						{
							float localPoints = localPoints2;
							list2.Add(new DebugMenuOption(localPoints2.ToString() + " points", DebugMenuOptionMode.Action, delegate()
							{
								parms.points = localPoints;
								List<DebugMenuOption> list3 = new List<DebugMenuOption>();
								foreach (int localRadius2 in DebugActionsUtility.RadiusOptions())
								{
									int localRadius = localRadius2;
									list3.Add(new DebugMenuOption(localRadius2.ToString() + " tile radius", DebugMenuOptionMode.Action, delegate()
									{
										parms.raidArrivalMode = PawnsArrivalModeDefOf.SpecificDropDebug;
										parms.raidStrategy = (fac.HostileTo(Faction.OfPlayer) ? RaidStrategyDefOf.ImmediateAttack : RaidStrategyDefOf.ImmediateAttackFriendly);
										parms.target = Find.CurrentMap;
										parms.dropInRadius = localRadius;
										Targeter targeter = Find.Targeter;
										TargetingParameters targetParams = TargetingParameters.ForDropPodsDestination();
										Action<LocalTargetInfo> action;
										if ((action = <>9__4) == null)
										{
											action = (<>9__4 = delegate(LocalTargetInfo target)
											{
												parms.spawnCenter = target.Cell;
												DebugActionsIncidents.DoRaid(parms);
											});
										}
										targeter.BeginTargeting(targetParams, action, null, null, null, true);
									}));
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x0013F1FC File Offset: 0x0013D3FC
		[DebugActionYielder]
		private static IEnumerable<DebugActionNode> IncidentsYielder()
		{
			yield return DebugActionsIncidents.GetIncidentDebugAction("Do incident", 1);
			yield return DebugActionsIncidents.GetIncidentDebugAction("Do incident x10", 10);
			yield return DebugActionsIncidents.GetIncidentWithPointsDebugAction();
			yield break;
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x0013F208 File Offset: 0x0013D408
		private static DebugActionNode GetIncidentDebugAction(string name, int iterations)
		{
			return new DebugActionNode(name, DebugActionType.Action, null, null)
			{
				category = "Incidents",
				labelGetter = (() => name + " (" + DebugActionsIncidents.GetIncidentTargetLabel() + ")..."),
				visibilityGetter = new Func<bool>(DebugActionsIncidents.YielderOptionVisible),
				childGetter = delegate()
				{
					List<DebugActionNode> list = new List<DebugActionNode>();
					foreach (IncidentDef localDef2 in from x in DefDatabase<IncidentDef>.AllDefs
					orderby x.defName
					select x)
					{
						IncidentDef localDef = localDef2;
						DebugActionNode child = new DebugActionNode(localDef.defName, DebugActionType.Action, delegate()
						{
							IIncidentTarget target = DebugActionsIncidents.GetTarget();
							if (target == null || !localDef.TargetAllowed(target))
							{
								Log.Warning("Incident target is null or not allowed.");
								return;
							}
							IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(localDef.category, target);
							incidentParms.forced = true;
							if (localDef.pointsScaleable)
							{
								incidentParms = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain).GenerateParms(localDef.category, incidentParms.target);
							}
							for (int i = 0; i < iterations; i++)
							{
								localDef.Worker.TryExecute(incidentParms);
							}
						}, null);
						child.labelGetter = delegate()
						{
							string text = child.label;
							IIncidentTarget target = DebugActionsIncidents.GetTarget();
							if (target != null)
							{
								IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target);
								if (!localDef.TargetAllowed(target) || !localDef.Worker.CanFireNow(parms))
								{
									text += " [NO]";
								}
							}
							return text;
						};
						list.Add(child);
					}
					return list;
				}
			};
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x0013F278 File Offset: 0x0013D478
		private static DebugActionNode GetIncidentWithPointsDebugAction()
		{
			DebugActionNode debugActionNode = new DebugActionNode("Do incident w/ points", DebugActionType.Action, null, null);
			debugActionNode.category = "Incidents";
			debugActionNode.labelGetter = (() => "Do incident w/ points (" + DebugActionsIncidents.GetIncidentTargetLabel() + ")...");
			debugActionNode.visibilityGetter = new Func<bool>(DebugActionsIncidents.YielderOptionVisible);
			debugActionNode.childGetter = delegate()
			{
				List<DebugActionNode> list = new List<DebugActionNode>();
				foreach (IncidentDef localDef2 in from x in DefDatabase<IncidentDef>.AllDefs
				where x.pointsScaleable
				select x into y
				orderby y.defName
				select y)
				{
					IncidentDef localDef = localDef2;
					IIncidentTarget target = DebugActionsIncidents.GetTarget();
					if (target == null || localDef.TargetAllowed(target))
					{
						DebugActionNode child = new DebugActionNode(localDef.defName, DebugActionType.Action, null, null);
						foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
						{
							float localPoints = localPoints2;
							DebugActionNode grandchild = new DebugActionNode(localPoints.ToString() + " points", DebugActionType.Action, delegate()
							{
								IIncidentTarget target2 = DebugActionsIncidents.GetTarget();
								if (target2 == null || !localDef.TargetAllowed(target2))
								{
									return;
								}
								IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(localDef.category, target2);
								incidentParms.forced = true;
								incidentParms.points = localPoints;
								localDef.Worker.TryExecute(incidentParms);
							}, null);
							grandchild.labelGetter = delegate()
							{
								string text = grandchild.label;
								IIncidentTarget target2 = DebugActionsIncidents.GetTarget();
								if (target2 != null)
								{
									if (!localDef.TargetAllowed(target2))
									{
										text += "[NO]";
									}
									else
									{
										IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(localDef.category, target2);
										incidentParms.points = localPoints;
										if (!localDef.Worker.CanFireNow(incidentParms))
										{
											text += " [NO]";
										}
									}
								}
								return text;
							};
							child.AddChild(grandchild);
						}
						child.labelGetter = delegate()
						{
							string text = child.label;
							IIncidentTarget target2 = DebugActionsIncidents.GetTarget();
							if (target2 != null)
							{
								if (!localDef.TargetAllowed(target2))
								{
									text += " [NO]";
								}
								else
								{
									IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target2);
									if (!localDef.Worker.CanFireNow(parms))
									{
										text += " [NO]";
									}
								}
							}
							return text;
						};
						list.Add(child);
					}
				}
				return list;
			};
			return debugActionNode;
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x0013F2FC File Offset: 0x0013D4FC
		private static void DoRaid(IncidentParms parms)
		{
			IncidentDef incidentDef;
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				incidentDef = IncidentDefOf.RaidEnemy;
			}
			else
			{
				incidentDef = IncidentDefOf.RaidFriendly;
			}
			incidentDef.Worker.TryExecute(parms);
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x0013F338 File Offset: 0x0013D538
		private static bool YielderOptionVisible()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			IIncidentTarget target = DebugActionsIncidents.GetTarget();
			if (target == null)
			{
				return false;
			}
			bool flag = target is World;
			bool flag2 = target is WorldObject;
			if (WorldRendererUtility.WorldSelected)
			{
				return flag || flag2;
			}
			return !flag && !flag2;
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x0013F384 File Offset: 0x0013D584
		private static IIncidentTarget GetTarget()
		{
			IIncidentTarget incidentTarget = WorldRendererUtility.WorldSelected ? (Find.WorldSelector.SingleSelectedObject as IIncidentTarget) : null;
			if (incidentTarget == null && WorldRendererUtility.WorldSelected)
			{
				incidentTarget = Find.World;
			}
			if (incidentTarget == null)
			{
				incidentTarget = Find.CurrentMap;
			}
			return incidentTarget;
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x0013F3C8 File Offset: 0x0013D5C8
		private static string GetIncidentTargetLabel()
		{
			IIncidentTarget target = DebugActionsIncidents.GetTarget();
			if (target == null)
			{
				return "null target";
			}
			if (target is Map)
			{
				return "Map";
			}
			if (target is World)
			{
				return "World";
			}
			if (target is Caravan)
			{
				return ((Caravan)target).LabelCap;
			}
			return target.ToString();
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x0013F41A File Offset: 0x0013D61A
		[DebugAction("Incidents", "Recalculate threat points", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RecalculateThreatPoints()
		{
			Find.CurrentMap.wealthWatcher.ForceRecount(false);
		}
	}
}
