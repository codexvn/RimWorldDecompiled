using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020009EC RID: 2540
	public static class DebugToolsPawns
	{
		// Token: 0x06003FDC RID: 16348 RVA: 0x0014E169 File Offset: 0x0014C369
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = 1000)]
		private static void AddSlave()
		{
			DebugToolsPawns.AddGuest(GuestStatus.Slave);
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x0014E171 File Offset: 0x0014C371
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void AddPrisoner()
		{
			DebugToolsPawns.AddGuest(GuestStatus.Prisoner);
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x0014E179 File Offset: 0x0014C379
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void AddGuest()
		{
			DebugToolsPawns.AddGuest(GuestStatus.Guest);
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x0014E184 File Offset: 0x0014C384
		private static void AddGuest(GuestStatus guestStatus)
		{
			foreach (Building_Bed building_Bed in Find.CurrentMap.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>())
			{
				if ((!building_Bed.OwnersForReading.Any<Pawn>() || building_Bed.AnyUnownedSleepingSlot) && (guestStatus != GuestStatus.Prisoner || building_Bed.ForPrisoners) && (guestStatus != GuestStatus.Slave || building_Bed.ForSlaves))
				{
					PawnKindDef pawnKindDef;
					if (guestStatus == GuestStatus.Guest)
					{
						pawnKindDef = PawnKindDefOf.SpaceRefugee;
					}
					else
					{
						pawnKindDef = (from pk in DefDatabase<PawnKindDef>.AllDefs
						where pk.defaultFactionDef != null && !pk.defaultFactionDef.isPlayer && pk.RaceProps.Humanlike && pk.mutant == null
						select pk).RandomElement<PawnKindDef>();
					}
					Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionDef);
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, faction, null);
					GenSpawn.Spawn(pawn, building_Bed.Position, Find.CurrentMap, WipeMode.Vanish);
					foreach (ThingWithComps eq in pawn.equipment.AllEquipmentListForReading.ToList<ThingWithComps>())
					{
						ThingWithComps thingWithComps;
						if (pawn.equipment.TryDropEquipment(eq, out thingWithComps, pawn.Position, true))
						{
							thingWithComps.Destroy(DestroyMode.Vanish);
						}
					}
					pawn.inventory.innerContainer.Clear();
					pawn.ownership.ClaimBedIfNonMedical(building_Bed);
					pawn.guest.SetGuestStatus(Faction.OfPlayer, guestStatus);
					break;
				}
			}
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x0014E334 File Offset: 0x0014C534
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void DamageUntilDown(Pawn p)
		{
			HealthUtility.DamageUntilDowned(p, true, null, null, null);
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x0014E340 File Offset: 0x0014C540
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DamageLegs(Pawn p)
		{
			HealthUtility.DamageLegsUntilIncapableOfMoving(p, true);
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x0014E349 File Offset: 0x0014C549
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DamageUntilIncapableOfManipulation(Pawn p)
		{
			HealthUtility.DamageLimbsUntilIncapableOfManipulation(p, true);
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x0014E352 File Offset: 0x0014C552
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p, null, null, null);
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0014E35D File Offset: 0x0014C55D
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void CarriedDamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p.carryTracker.CarriedThing as Pawn, null, null, null);
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0014E378 File Offset: 0x0014C578
		[DebugAction("Pawns", "10 damage until dead", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void Do10DamageUntilDead()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				for (int i = 0; i < 1000; i++)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
					dinfo.SetIgnoreInstantKillProtection(true);
					thing.TakeDamage(dinfo);
					if (thing.Destroyed)
					{
						string str = "Took " + (i + 1).ToString() + " hits";
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							if (pawn.health.ShouldBeDeadFromLethalDamageThreshold())
							{
								str = str + " (reached lethal damage threshold of " + pawn.health.LethalDamageThreshold.ToString("0.#") + ")";
							}
							else if (PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, pawn.RaceProps.body.corePart, false, null) <= 0.0001f)
							{
								str += " (core part hp reached 0)";
							}
							else
							{
								PawnCapacityDef pawnCapacityDef = pawn.health.ShouldBeDeadFromRequiredCapacity();
								if (pawnCapacityDef != null)
								{
									str = str + " (incapable of " + pawnCapacityDef.defName + ")";
								}
							}
						}
						Log.Message(str + ".");
						break;
					}
				}
			}
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x0014E524 File Offset: 0x0014C724
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DamageHeldPawnToDeath()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.CarriedThing is Pawn)
				{
					HealthUtility.DamageUntilDead((Pawn)pawn.carryTracker.CarriedThing, null, null, null);
				}
			}
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x0014E5C4 File Offset: 0x0014C7C4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RestoreBodyPart(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RestorePart(p), null));
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x0014E5DC File Offset: 0x0014C7DC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetHeadType(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (List<HeadTypeDef>.Enumerator enumerator = DefDatabase<HeadTypeDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HeadTypeDef headTypeDef = enumerator.Current;
					list.Add(new FloatMenuOption(headTypeDef.defName, delegate()
					{
						p.story.headType = headTypeDef;
						p.Drawer.renderer.SetAllGraphicsDirty();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list, p.LabelShort, false));
			}
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x0014E6A0 File Offset: 0x0014C8A0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetBodyType(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (List<BodyTypeDef>.Enumerator enumerator = DefDatabase<BodyTypeDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BodyTypeDef bodyTypeDef = enumerator.Current;
					list.Add(new FloatMenuOption(bodyTypeDef.defName, delegate()
					{
						p.story.bodyType = bodyTypeDef;
						p.Drawer.renderer.SetAllGraphicsDirty();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list, p.LabelShort, false));
			}
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x0014E764 File Offset: 0x0014C964
		[DebugAction("Pawns", "Apply damage", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> ApplyDamage()
		{
			return DebugTools_Health.Options_ApplyDamage();
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x0014E76C File Offset: 0x0014C96C
		[DebugAction("Pawns", "Heal random injury (10)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void HealRandomInjury10(Pawn p)
		{
			List<Hediff_Injury> source = new List<Hediff_Injury>();
			p.health.hediffSet.GetHediffs<Hediff_Injury>(ref source, (Hediff_Injury x) => x.CanHealNaturally() || x.CanHealFromTending());
			Hediff_Injury hediff_Injury;
			if (source.TryRandomElement(out hediff_Injury))
			{
				hediff_Injury.Heal(10f);
			}
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x0014E7C8 File Offset: 0x0014C9C8
		[DebugAction("Pawns", "Make injuries permanent", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeInjuryPermanent(Pawn p)
		{
			foreach (Hediff hd in p.health.hediffSet.hediffs)
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = hd.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null)
				{
					hediffComp_GetsPermanent.IsPermanent = true;
				}
			}
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x0014E830 File Offset: 0x0014CA30
		[DebugAction("Pawns", "Toggle immunity", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleImmunity(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				Hediff hediff2 = hediff;
				ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(hediff2.def);
				if (immunityRecord != null)
				{
					Texture2D iconTex = (immunityRecord.immunity < 1f) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
					list.Add(new FloatMenuOption(hediff2.LabelCap, delegate()
					{
						if (immunityRecord.immunity < 1f)
						{
							immunityRecord.immunity = 1f;
							return;
						}
						immunityRecord.immunity = 0f;
					}, iconTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list, p.LabelShort, false));
			}
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0014E930 File Offset: 0x0014CB30
		[DebugAction("Pawns", "Activate HediffGiver", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ActivateHediffGiver(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (p.RaceProps.hediffGiverSets != null)
			{
				foreach (HediffGiver localHdg2 in p.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef set) => set.hediffGivers))
				{
					HediffGiver localHdg = localHdg2;
					list.Add(new FloatMenuOption(localHdg.hediff.defName, delegate()
					{
						if (localHdg.TryApply(p, null))
						{
							Messages.Message(localHdg.hediff.defName + " applied to " + p.Label, MessageTypeDefOf.NeutralEvent, false);
							return;
						}
						Messages.Message("failed to apply " + localHdg.hediff.defName + " to " + p.Label, MessageTypeDefOf.NegativeEvent, false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list));
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x0014EA3C File Offset: 0x0014CC3C
		[DebugAction("Pawns", "Activate HediffGiver World Pawn", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ActivateHediffGiverWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawnLocal2 in from p in Find.WorldPawns.AllPawnsAlive
			where p.RaceProps.Humanlike
			select p)
			{
				Pawn pawnLocal = pawnLocal2;
				list.Add(new DebugMenuOption(pawnLocal.Label, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (HediffGiver hediffGiverLocal2 in pawnLocal.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef s) => s.hediffGivers))
					{
						HediffGiver hediffGiverLocal = hediffGiverLocal2;
						list2.Add(new DebugMenuOption(hediffGiverLocal.hediff.defName, DebugMenuOptionMode.Action, delegate()
						{
							if (hediffGiverLocal.TryApply(pawnLocal, null))
							{
								Messages.Message(hediffGiverLocal.hediff.defName + " applied to " + pawnLocal.Label, MessageTypeDefOf.NeutralEvent, false);
								return;
							}
							Messages.Message("failed to apply " + hediffGiverLocal.hediff.defName + " to " + pawnLocal.Label, MessageTypeDefOf.NegativeEvent, false);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x0014EAF4 File Offset: 0x0014CCF4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DiscoverHediffs(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				if (!hediff.Visible)
				{
					hediff.Severity = Mathf.Max(hediff.Severity, hediff.def.stages.First((HediffStage s) => s.becomeVisible).minSeverity);
				}
			}
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0014EB98 File Offset: 0x0014CD98
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrantImmunities(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(hediff.def);
				if (immunityRecord != null)
				{
					immunityRecord.immunity = 1f;
				}
			}
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x0014EC14 File Offset: 0x0014CE14
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void GiveBirth(Pawn p)
		{
			Hediff_Pregnant.DoBirthSpawn(p, null);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x0014EC24 File Offset: 0x0014CE24
		[DebugAction("Pawns", "Resistance -1", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ResistanceMinus1(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 1f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0014EC78 File Offset: 0x0014CE78
		[DebugAction("Pawns", "Resistance -10", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ResistanceMinus10(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 10f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x0014ECCC File Offset: 0x0014CECC
		[DebugAction("Pawns", "Add/remove pawn relation", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddRemovePawnRelation(Pawn p)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return;
			}
			Func<Pawn, bool> <>9__5;
			Action<bool> act = delegate(bool add)
			{
				if (add)
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (PawnRelationDef pawnRelationDef in DefDatabase<PawnRelationDef>.AllDefs)
					{
						if (!pawnRelationDef.implied)
						{
							PawnRelationDef defLocal = pawnRelationDef;
							list2.Add(new DebugMenuOption(defLocal.defName, DebugMenuOptionMode.Action, delegate()
							{
								List<DebugMenuOption> list4 = new List<DebugMenuOption>();
								IEnumerable<Pawn> source = from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
								where x.RaceProps.IsFlesh || (x.RaceProps.IsMechanoid && x.Faction == Faction.OfPlayer)
								select x;
								Func<Pawn, bool> keySelector;
								if ((keySelector = <>9__5) == null)
								{
									keySelector = (<>9__5 = ((Pawn x) => x.def == p.def));
								}
								foreach (Pawn pawn in source.OrderByDescending(keySelector).ThenBy((Pawn x) => x.IsWorldPawn()))
								{
									if (p != pawn && (!defLocal.familyByBloodRelation || pawn.def == p.def) && !p.relations.DirectRelationExists(defLocal, pawn))
									{
										Pawn otherLocal = pawn;
										list4.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
										{
											p.relations.AddDirectRelation(defLocal, otherLocal);
											if (defLocal == PawnRelationDefOf.Fiance)
											{
												otherLocal.relations.nextMarriageNameChange = (p.relations.nextMarriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage(p));
											}
										}));
									}
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4, null));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
					return;
				}
				List<DebugMenuOption> list3 = new List<DebugMenuOption>();
				List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					DirectPawnRelation rel = directRelations[i];
					list3.Add(new DebugMenuOption(rel.def.defName + " - " + rel.otherPawn.LabelShort, DebugMenuOptionMode.Action, delegate()
					{
						p.relations.RemoveDirectRelation(rel);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Add", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			if (!p.relations.DirectRelations.NullOrEmpty<DirectPawnRelation>())
			{
				list.Add(new DebugMenuOption("Remove", DebugMenuOptionMode.Action, delegate()
				{
					act(false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x0014ED74 File Offset: 0x0014CF74
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void AddOpinionTalksAbout(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			Action<bool> act = delegate(bool good)
			{
				Func<ThoughtDef, bool> <>9__4;
				foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Humanlike
				select x)
				{
					if (p != pawn)
					{
						IEnumerable<ThoughtDef> allDefs = DefDatabase<ThoughtDef>.AllDefs;
						Func<ThoughtDef, bool> predicate;
						if ((predicate = <>9__4) == null)
						{
							predicate = (<>9__4 = ((ThoughtDef x) => typeof(Thought_MemorySocial).IsAssignableFrom(x.thoughtClass) && ((good && x.stages[0].baseOpinionOffset > 0f) || (!good && x.stages[0].baseOpinionOffset < 0f))));
						}
						IEnumerable<ThoughtDef> source = allDefs.Where(predicate);
						if (source.Any<ThoughtDef>())
						{
							int num = Rand.Range(2, 5);
							for (int i = 0; i < num; i++)
							{
								ThoughtDef def = source.RandomElement<ThoughtDef>();
								pawn.needs.mood.thoughts.memories.TryGainMemory(def, p, null);
							}
						}
					}
				}
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Good", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			list.Add(new DebugMenuOption("Bad", DebugMenuOptionMode.Action, delegate()
			{
				act(false);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x0014EE04 File Offset: 0x0014D004
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SetSkill()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.defName, DebugActionType.Action, null, null);
				for (int i = 0; i <= 20; i++)
				{
					int level = i;
					debugActionNode.AddChild(new DebugActionNode(level.ToString(), DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							if (p.skills != null)
							{
								SkillRecord skill = p.skills.GetSkill(localDef);
								skill.Level = level;
								skill.xpSinceLastLevel = skill.XpRequiredForLevelUp / 2f;
								DebugActionsUtility.DustPuffFrom(p);
							}
						}
					});
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x0014EED8 File Offset: 0x0014D0D8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MaxSkill()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					Pawn_SkillTracker skills = p.skills;
					if (skills == null)
					{
						return;
					}
					skills.Learn(localDef, 100000000f, false, false);
				}));
			}
			return list;
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x0014EF58 File Offset: 0x0014D158
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MaxAllSkills(Pawn p)
		{
			if (p.skills != null)
			{
				foreach (SkillDef sDef in DefDatabase<SkillDef>.AllDefs)
				{
					p.skills.Learn(sDef, 100000000f, false, false);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
			if (p.training != null)
			{
				foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
				{
					Pawn trainer = p.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
					bool flag;
					if (p.training.CanAssignToTrain(td, out flag).Accepted)
					{
						p.training.Train(td, trainer, false);
					}
				}
			}
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x0014F03C File Offset: 0x0014D23C
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SetPassion()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.defName, DebugActionType.Action, null, null);
				using (IEnumerator enumerator2 = Enum.GetValues(typeof(Passion)).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object passion = enumerator2.Current;
						debugActionNode.AddChild(new DebugActionNode(passion.ToString(), DebugActionType.ToolMapForPawns, null, null)
						{
							pawnAction = delegate(Pawn p)
							{
								if (p.skills != null)
								{
									p.skills.GetSkill(localDef).passion = (Passion)passion;
									DebugActionsUtility.DustPuffFrom(p);
								}
							}
						});
					}
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x0014F148 File Offset: 0x0014D348
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MaxPassion()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					if (p.skills == null)
					{
						return;
					}
					p.skills.GetSkill(localDef).passion = Passion.Major;
				}));
			}
			return list;
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x0014F1C8 File Offset: 0x0014D3C8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MaxAllPassions(Pawn p)
		{
			if (p.skills != null)
			{
				foreach (SkillRecord skillRecord in p.skills.skills)
				{
					skillRecord.passion = Passion.Major;
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x0014F22C File Offset: 0x0014D42C
		[DebugAction("Pawns", "Mental break", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MentalBreak()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("(log possibles)", DebugActionType.ToolMapForPawns, null, null);
			debugActionNode.pawnAction = delegate(Pawn p)
			{
				p.mindState.mentalBreaker.LogPossibleMentalBreaks();
				DebugActionsUtility.DustPuffFrom(p);
			};
			list2.Add(debugActionNode);
			List<DebugActionNode> list3 = list;
			DebugActionNode debugActionNode2 = new DebugActionNode("(natural mood break)", DebugActionType.ToolMapForPawns, null, null);
			debugActionNode2.pawnAction = delegate(Pawn p)
			{
				p.mindState.mentalBreaker.TryDoRandomMoodCausedMentalBreak();
				DebugActionsUtility.DustPuffFrom(p);
			};
			list3.Add(debugActionNode2);
			foreach (MentalBreakDef locBrDef2 in from x in DefDatabase<MentalBreakDef>.AllDefs
			orderby x.intensity descending
			select x)
			{
				MentalBreakDef locBrDef = locBrDef2;
				Predicate<Pawn> <>9__5;
				list.Add(new DebugActionNode(locBrDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						locBrDef.Worker.TryStart(p, null, false);
						DebugActionsUtility.DustPuffFrom(p);
					},
					labelGetter = delegate()
					{
						string text = locBrDef.defName;
						if (Find.CurrentMap != null)
						{
							List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
							Predicate<Pawn> predicate;
							if ((predicate = <>9__5) == null)
							{
								predicate = (<>9__5 = ((Pawn x) => locBrDef.Worker.BreakCanOccur(x)));
							}
							if (!freeColonists.Any(predicate))
							{
								text += " [NO]";
							}
						}
						return text;
					}
				});
			}
			return list;
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x0014F358 File Offset: 0x0014D558
		[DebugAction("Pawns", "Mental state...", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MentalState()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (MentalStateDef locBrDef2 in DefDatabase<MentalStateDef>.AllDefs)
			{
				MentalStateDef locBrDef = locBrDef2;
				Predicate<Pawn> <>9__4;
				list.Add(new DebugActionNode(locBrDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						if (locBrDef != MentalStateDefOf.SocialFighting)
						{
							p.mindState.mentalStateHandler.TryStartMentalState(locBrDef, null, false, true, false, null, false, false, false);
							DebugActionsUtility.DustPuffFrom(p);
							return;
						}
						DebugTools.curTool = new DebugTool("...with", delegate()
						{
							Pawn pawn = (Pawn)(from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
							where t is Pawn
							select t).FirstOrDefault<Thing>();
							if (pawn != null)
							{
								p.interactions.StartSocialFight(pawn, "MessageSocialFight");
								DebugTools.curTool = null;
							}
						}, null);
					},
					labelGetter = delegate()
					{
						string text = locBrDef.defName;
						if (Find.CurrentMap == null)
						{
							return text;
						}
						List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
						Predicate<Pawn> predicate;
						if ((predicate = <>9__4) == null)
						{
							predicate = (<>9__4 = ((Pawn x) => locBrDef.Worker.StateCanOccur(x)));
						}
						if (!freeColonists.Any(predicate))
						{
							text += " [NO]";
						}
						return text;
					}
				});
			}
			return list;
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x0014F3F0 File Offset: 0x0014D5F0
		[DebugAction("Pawns", "Stop mental state", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StopMentalState(Pawn p)
		{
			if (p.InMentalState)
			{
				p.MentalState.RecoverFromState();
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x0014F414 File Offset: 0x0014D614
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> Inspiration()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (InspirationDef localDef2 in DefDatabase<InspirationDef>.AllDefs)
			{
				InspirationDef localDef = localDef2;
				Predicate<Pawn> <>9__2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						InspirationHandler inspirationHandler = p.mindState.inspirationHandler;
						if (inspirationHandler != null)
						{
							inspirationHandler.TryStartInspiration(localDef, "Debug gain", true);
						}
						DebugActionsUtility.DustPuffFrom(p);
					},
					labelGetter = delegate()
					{
						string text = localDef.defName;
						List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
						Predicate<Pawn> predicate;
						if ((predicate = <>9__2) == null)
						{
							predicate = (<>9__2 = ((Pawn x) => localDef.Worker.InspirationCanOccur(x)));
						}
						if (!freeColonists.Any(predicate))
						{
							text += " [NO]";
						}
						return text;
					}
				});
			}
			return list;
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x0014F4AC File Offset: 0x0014D6AC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> GiveTrait()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (TraitDef traitDef in DefDatabase<TraitDef>.AllDefs)
			{
				TraitDef trDef = traitDef;
				for (int j = 0; j < traitDef.degreeDatas.Count; j++)
				{
					int i = j;
					list.Add(new DebugActionNode(trDef.degreeDatas[i].label + " (" + trDef.degreeDatas[j].degree.ToString() + ")", DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							if (p.story != null)
							{
								p.story.traits.GainTrait(new Trait(trDef, trDef.degreeDatas[i].degree, false), true);
								DebugActionsUtility.DustPuffFrom(p);
							}
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x0014F5B8 File Offset: 0x0014D7B8
		[DebugAction("Pawns", "Remove all traits", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveAllTraits(Pawn p)
		{
			if (p.story != null)
			{
				for (int i = p.story.traits.allTraits.Count - 1; i >= 0; i--)
				{
					p.story.traits.RemoveTrait(p.story.traits.allTraits[i], false);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x0014F61C File Offset: 0x0014D81C
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static List<DebugActionNode> SetBackstory()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			DebugActionNode debugActionNode = new DebugActionNode("Adulthood", DebugActionType.Action, null, null);
			foreach (DebugActionNode child in DebugToolsPawns.<SetBackstory>g__BackstoryOptionNodes|39_0(BackstorySlot.Adulthood))
			{
				debugActionNode.AddChild(child);
			}
			list.Add(debugActionNode);
			DebugActionNode debugActionNode2 = new DebugActionNode("Childhood", DebugActionType.Action, null, null);
			foreach (DebugActionNode child2 in DebugToolsPawns.<SetBackstory>g__BackstoryOptionNodes|39_0(BackstorySlot.Childhood))
			{
				debugActionNode2.AddChild(child2);
			}
			list.Add(debugActionNode2);
			return list;
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x0014F6E8 File Offset: 0x0014D8E8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> GiveAbility()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*All", DebugActionType.ToolMapForPawns, null, null);
			debugActionNode.pawnAction = delegate(Pawn p)
			{
				if (p.abilities != null)
				{
					foreach (AbilityDef def in DefDatabase<AbilityDef>.AllDefs)
					{
						p.abilities.GainAbility(def);
					}
				}
			};
			list2.Add(debugActionNode);
			foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefs)
			{
				AbilityDef localAb = abilityDef;
				list.Add(new DebugActionNode(abilityDef.label, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					Pawn_AbilityTracker abilities = p.abilities;
					if (abilities == null)
					{
						return;
					}
					abilities.GainAbility(localAb);
				}));
			}
			return list;
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0014F798 File Offset: 0x0014D998
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresRoyalty = true)]
		private static List<DebugActionNode> GivePsylink()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			if (!ModsConfig.RoyaltyActive)
			{
				return list;
			}
			for (int i = 1; i <= (int)HediffDefOf.PsychicAmplifier.maxSeverity; i++)
			{
				int level = i;
				list.Add(new DebugActionNode("Level " + i.ToString(), DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						Hediff_Level hediff_Level = p.GetMainPsylinkSource();
						if (hediff_Level == null)
						{
							hediff_Level = (HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, p, p.health.hediffSet.GetBrain()) as Hediff_Level);
							p.health.AddHediff(hediff_Level, null, null, null);
						}
						hediff_Level.ChangeLevel(level - hediff_Level.level);
					}
				});
			}
			return list;
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0014F80C File Offset: 0x0014DA0C
		[DebugAction("Pawns", "Play Animation...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> PlayAnimation()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (AnimationDef localDef2 in DefDatabase<AnimationDef>.AllDefs)
			{
				AnimationDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						p.Drawer.renderer.SetAnimation(localDef);
					}
				});
			}
			return list;
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x0014F890 File Offset: 0x0014DA90
		[DebugAction("Pawns", "Give good thought", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void GiveGoodThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugGood, null, null);
			}
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x0014F8C0 File Offset: 0x0014DAC0
		[DebugAction("Pawns", "Give bad thought", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void GiveBadThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugBad, null, null);
			}
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x0014F8F0 File Offset: 0x0014DAF0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ClearBoundUnfinishedThings()
		{
			foreach (Building_WorkTable building_WorkTable in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Building_WorkTable
			select t).Cast<Building_WorkTable>())
			{
				foreach (Bill bill in building_WorkTable.BillStack)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = bill as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null)
					{
						bill_ProductionWithUft.ClearBoundUft();
					}
				}
			}
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x0014F9B0 File Offset: 0x0014DBB0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceBirthday(Pawn p)
		{
			p.ageTracker.AgeBiologicalTicks = (long)((p.ageTracker.AgeBiologicalYears + 1) * 3600000 + 1);
			p.ageTracker.DebugForceBirthdayBiological();
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x0014F9E0 File Offset: 0x0014DBE0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void Recruit(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer)
			{
				if (p.RaceProps.Humanlike)
				{
					InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p, true);
					DebugActionsUtility.DustPuffFrom(p);
					return;
				}
				if (p.RaceProps.IsMechanoid)
				{
					p.SetFaction(Faction.OfPlayer, null);
					DebugActionsUtility.DustPuffFrom(p);
				}
			}
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x0014FA49 File Offset: 0x0014DC49
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = 1000)]
		private static void Enslave(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
			{
				GenGuest.EnslavePrisoner(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x0014FA88 File Offset: 0x0014DC88
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleRecruitable(Pawn p)
		{
			if (p.guest != null)
			{
				p.guest.Recruitable = !p.guest.Recruitable;
				DebugActionsUtility.DustPuffFrom(p);
				MoteMaker.ThrowText(p.DrawPos, p.MapHeld, "Recruitable:\n" + p.guest.Recruitable.ToStringYesNo(), -1f);
			}
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x0014FAEC File Offset: 0x0014DCEC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrowPawnToMaturity()
		{
			Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
			if (firstPawn != null)
			{
				firstPawn.ageTracker.AgeBiologicalTicks += (long)Mathf.FloorToInt(firstPawn.ageTracker.AdultMinAge * 3600000f);
			}
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x0014FB38 File Offset: 0x0014DD38
		[DebugAction("Pawns", "Wear apparel (selected)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> WearApparel_ToSelected()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*Remove all apparel", DebugActionType.Action, null, null);
			debugActionNode.action = delegate()
			{
				foreach (object obj in Find.Selector.SelectedObjectsListForReading)
				{
					Pawn pawn = obj as Pawn;
					if (pawn != null)
					{
						Pawn_ApparelTracker apparel = pawn.apparel;
						if (apparel != null)
						{
							apparel.DestroyAll(DestroyMode.Vanish);
						}
					}
				}
			};
			list2.Add(debugActionNode);
			foreach (ThingDef localDef2 in from d in DefDatabase<ThingDef>.AllDefs
			where d.IsApparel
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						foreach (object obj in Find.Selector.SelectedObjectsListForReading)
						{
							Pawn pawn = obj as Pawn;
							if (pawn != null && pawn.apparel != null)
							{
								ThingDef stuff = GenStuff.RandomStuffFor(localDef);
								Apparel newApparel = (Apparel)ThingMaker.MakeThing(localDef, stuff);
								pawn.apparel.Wear(newApparel, false, false);
							}
						}
					}
				});
			}
			return list;
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x0014FC3C File Offset: 0x0014DE3C
		[DebugAction("Pawns", "Equip primary (selected)...", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> EquipPrimary_ToSelected()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*Remove primary", DebugActionType.Action, null, null);
			debugActionNode.action = delegate()
			{
				foreach (object obj in Find.Selector.SelectedObjectsListForReading)
				{
					Pawn pawn = obj as Pawn;
					if (pawn != null)
					{
						Pawn_EquipmentTracker equipment = pawn.equipment;
						if (((equipment != null) ? equipment.Primary : null) != null)
						{
							pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
						}
					}
				}
			};
			list2.Add(debugActionNode);
			using (IEnumerator<ThingDef> enumerator = (from d in DefDatabase<ThingDef>.AllDefs
			where d.equipmentType == EquipmentType.Primary
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					ThingDef def2 = def;
					list.Add(new DebugActionNode(def2.defName, DebugActionType.Action, null, null)
					{
						action = delegate()
						{
							foreach (object obj in Find.Selector.SelectedObjectsListForReading)
							{
								Pawn pawn = obj as Pawn;
								if (pawn != null && pawn.equipment != null)
								{
									if (pawn.equipment.Primary != null)
									{
										pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
									}
									ThingDef stuff = GenStuff.RandomStuffFor(def);
									ThingWithComps newEq = (ThingWithComps)ThingMaker.MakeThing(def, stuff);
									pawn.equipment.AddEquipment(newEq);
								}
							}
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x0014FD40 File Offset: 0x0014DF40
		public static List<FloatMenuOption> PawnGearDevOptions(Pawn pawn)
		{
			return new List<FloatMenuOption>
			{
				new FloatMenuOption("Set primary", delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_SetPrimary(pawn), null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				new FloatMenuOption("Wear", delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_Wear(pawn), null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				new FloatMenuOption("Add to inventory", delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_GiveToInventory(pawn), null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				new FloatMenuOption("Damage random apparel", delegate()
				{
					pawn.apparel.WornApparel.RandomElement<Apparel>().TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 30f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
			};
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x0014FE00 File Offset: 0x0014E000
		private static List<DebugMenuOption> Options_Wear(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Remove all apparel", DebugMenuOptionMode.Action, delegate()
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}));
			IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
			Func<ThingDef, bool> <>9__1;
			Func<ThingDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((ThingDef def) => def.IsApparel && def.apparel.developmentalStageFilter.Has(pawn.DevelopmentalStage)));
			}
			using (IEnumerator<ThingDef> enumerator = (from d in allDefs.Where(predicate)
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						ThingDef stuff = GenStuff.RandomStuffFor(def);
						Apparel newApparel = (Apparel)ThingMaker.MakeThing(def, stuff);
						pawn.apparel.Wear(newApparel, false, false);
					}));
				}
			}
			return list;
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x0014FEF8 File Offset: 0x0014E0F8
		private static List<DebugMenuOption> Options_SetPrimary(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Remove primary", DebugMenuOptionMode.Action, delegate()
			{
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
				}
			}));
			using (IEnumerator<ThingDef> enumerator = (from def in DefDatabase<ThingDef>.AllDefs
			where def.equipmentType == EquipmentType.Primary
			select def into d
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						if (pawn.equipment.Primary != null)
						{
							pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
						}
						ThingDef stuff = GenStuff.RandomStuffFor(def);
						ThingWithComps newEq = (ThingWithComps)ThingMaker.MakeThing(def, stuff);
						pawn.equipment.AddEquipment(newEq);
					}));
				}
			}
			return list;
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x0014FFEC File Offset: 0x0014E1EC
		private static List<DebugMenuOption> Options_GiveToInventory(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Clear all", DebugMenuOptionMode.Action, delegate()
			{
				pawn.inventory.DestroyAll(DestroyMode.Vanish);
			}));
			using (IEnumerator<ThingDef> enumerator = (from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.label, DebugMenuOptionMode.Action, delegate()
					{
						pawn.inventory.TryAddItemNotForSale(ThingMaker.MakeThing(def, null));
					}));
				}
			}
			return list;
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x001500E0 File Offset: 0x0014E2E0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void TameAnimal(Pawn p)
		{
			if (p.AnimalOrWildMan() && p.Faction != Faction.OfPlayer)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.FirstOrDefault<Pawn>(), p, true);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x0015011C File Offset: 0x0014E31C
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TrainAnimal(Pawn p)
		{
			if (p.IsAnimal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				DebugActionsUtility.DustPuffFrom(p);
				bool flag = false;
				foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
				{
					if (p.training.GetWanted(td))
					{
						p.training.Train(td, null, true);
						flag = true;
					}
				}
				if (!flag)
				{
					foreach (TrainableDef td2 in DefDatabase<TrainableDef>.AllDefs)
					{
						if (p.training.CanAssignToTrain(td2).Accepted)
						{
							p.training.Train(td2, null, true);
						}
					}
				}
			}
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x0015020C File Offset: 0x0014E40C
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryDevelopBoundRelation(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			Pawn humanlike;
			if (p.RaceProps.Humanlike)
			{
				Pawn animal;
				if ((from x in p.Map.mapPawns.AllPawnsSpawned
				where x.IsAnimal && x.Faction == p.Faction
				select x).TryRandomElement(out animal))
				{
					RelationsUtility.TryDevelopBondRelation(p, animal, 999999f);
					return;
				}
			}
			else if (p.IsAnimal && (from x in p.Map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike && x.Faction == p.Faction
			select x).TryRandomElement(out humanlike))
			{
				RelationsUtility.TryDevelopBondRelation(humanlike, p, 999999f);
			}
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x001502D6 File Offset: 0x0014E4D6
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void QueueTrainingDecay(Pawn p)
		{
			if (p.IsAnimal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				p.training.Debug_MakeDegradeHappenSoon();
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x00150308 File Offset: 0x0014E508
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DisplayRelationsInfo(Pawn pawn)
		{
			List<TableDataGetter<Pawn>> list = new List<TableDataGetter<Pawn>>();
			list.Add(new TableDataGetter<Pawn>("name", (Pawn p) => p.LabelCap));
			list.Add(new TableDataGetter<Pawn>("kind label", (Pawn p) => p.KindLabel));
			list.Add(new TableDataGetter<Pawn>("gender", (Pawn p) => p.gender.GetLabel(false)));
			list.Add(new TableDataGetter<Pawn>("age", (Pawn p) => p.ageTracker.AgeBiologicalYears));
			list.Add(new TableDataGetter<Pawn>("my compat", (Pawn p) => pawn.relations.CompatibilityWith(p).ToString("F2")));
			list.Add(new TableDataGetter<Pawn>("their compat", (Pawn p) => p.relations.CompatibilityWith(pawn).ToString("F2")));
			list.Add(new TableDataGetter<Pawn>("my 2nd\nrom chance", (Pawn p) => pawn.relations.SecondaryRomanceChanceFactor(p).ToStringPercent("F0")));
			list.Add(new TableDataGetter<Pawn>("their 2nd\nrom chance", (Pawn p) => p.relations.SecondaryRomanceChanceFactor(pawn).ToStringPercent("F0")));
			list.Add(new TableDataGetter<Pawn>("lovin mtb", (Pawn p) => LovePartnerRelationUtility.GetLovinMtbHours(pawn, p).ToString("F1") + " h"));
			List<TableDataGetter<Pawn>> list2 = list;
			DebugTables.MakeTablesDialog<Pawn>(from x in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
			where x != pawn && x.RaceProps.Humanlike
			select x, list2.ToArray());
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x001504AC File Offset: 0x0014E6AC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DisplayInteractionsInfo(Pawn pawn)
		{
			DebugToolsPawns.<>c__DisplayClass62_0 CS$<>8__locals1 = new DebugToolsPawns.<>c__DisplayClass62_0();
			CS$<>8__locals1.pawn = pawn;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<Pawn> source = CS$<>8__locals1.pawn.Map.mapPawns.SpawnedPawnsInFaction(CS$<>8__locals1.pawn.Faction);
			Func<Pawn, bool> predicate;
			if ((predicate = CS$<>8__locals1.<>9__0) == null)
			{
				predicate = (CS$<>8__locals1.<>9__0 = ((Pawn x) => x != CS$<>8__locals1.pawn && x.RaceProps.Humanlike));
			}
			using (IEnumerator<Pawn> enumerator = source.Where(predicate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugToolsPawns.<>c__DisplayClass62_1 CS$<>8__locals2 = new DebugToolsPawns.<>c__DisplayClass62_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.p = enumerator.Current;
					float totalWeight = DefDatabase<InteractionDef>.AllDefs.Sum((InteractionDef x) => x.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p));
					Func<InteractionDef, string> <>9__5;
					list.Add(new DebugMenuOption(CS$<>8__locals2.p.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						List<TableDataGetter<InteractionDef>> list2 = new List<TableDataGetter<InteractionDef>>();
						list2.Add(new TableDataGetter<InteractionDef>("defName", (InteractionDef i) => i.defName));
						string label = "sel weight";
						Func<InteractionDef, float> getter;
						if ((getter = CS$<>8__locals2.<>9__4) == null)
						{
							getter = (CS$<>8__locals2.<>9__4 = ((InteractionDef i) => i.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p)));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label, getter));
						string label2 = "sel chance";
						Func<InteractionDef, string> getter2;
						if ((getter2 = <>9__5) == null)
						{
							getter2 = (<>9__5 = ((InteractionDef i) => (i.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p) / totalWeight).ToStringPercent()));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label2, getter2));
						string label3 = "fight\nchance";
						Func<InteractionDef, string> getter3;
						if ((getter3 = CS$<>8__locals2.<>9__6) == null)
						{
							getter3 = (CS$<>8__locals2.<>9__6 = ((InteractionDef i) => CS$<>8__locals2.p.interactions.SocialFightChance(i, CS$<>8__locals2.CS$<>8__locals1.pawn).ToStringPercent()));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label3, getter3));
						string label4 = "success\nchance";
						Func<InteractionDef, string> getter4;
						if ((getter4 = CS$<>8__locals2.<>9__7) == null)
						{
							getter4 = (CS$<>8__locals2.<>9__7 = delegate(InteractionDef i)
							{
								if (i == InteractionDefOf.RomanceAttempt)
								{
									return InteractionWorker_RomanceAttempt.SuccessChance(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p, 0.6f).ToStringPercent();
								}
								if (i == InteractionDefOf.MarriageProposal)
								{
									return InteractionWorker_MarriageProposal.AcceptanceChance(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p).ToStringPercent();
								}
								return "";
							});
						}
						list2.Add(new TableDataGetter<InteractionDef>(label4, getter4));
						List<TableDataGetter<InteractionDef>> list3 = list2;
						DebugTables.MakeTablesDialog<InteractionDef>(DefDatabase<InteractionDef>.AllDefs, list3.ToArray());
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x001505CC File Offset: 0x0014E7CC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void StartMarriageCeremony(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike
			select x)
			{
				if (p != pawn)
				{
					Pawn otherLocal = pawn;
					list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
					{
						if (!p.relations.DirectRelationExists(PawnRelationDefOf.Fiance, otherLocal))
						{
							p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, otherLocal);
							p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Spouse, otherLocal);
							p.relations.AddDirectRelation(PawnRelationDefOf.Fiance, otherLocal);
							Messages.Message("DEV: Auto added fiance relation.", p, MessageTypeDefOf.TaskCompletion, false);
						}
						if (!p.Map.lordsStarter.TryStartMarriageCeremony(p, otherLocal))
						{
							Messages.Message("Could not find any valid marriage site.", MessageTypeDefOf.RejectInput, false);
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x001506E4 File Offset: 0x0014E8E4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceInteraction(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction))
			{
				if (pawn != p)
				{
					Pawn otherLocal = pawn;
					list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (InteractionDef interactionLocal2 in DefDatabase<InteractionDef>.AllDefsListForReading)
						{
							InteractionDef interactionLocal = interactionLocal2;
							list2.Add(new DebugMenuOption(interactionLocal.label, DebugMenuOptionMode.Action, delegate()
							{
								p.interactions.TryInteractWith(otherLocal, interactionLocal);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x001507E4 File Offset: 0x0014E9E4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> StartGathering()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*Random", DebugActionType.Action, null, null);
			debugActionNode.action = delegate()
			{
				if (!Find.CurrentMap.lordsStarter.TryStartRandomGathering(true))
				{
					Messages.Message("Could not find any valid gathering spot or organizer.", MessageTypeDefOf.RejectInput, false);
				}
			};
			list2.Add(debugActionNode);
			foreach (GatheringDef gatheringDef2 in DefDatabase<GatheringDef>.AllDefsListForReading)
			{
				GatheringDef gatheringDef = gatheringDef2;
				list.Add(new DebugActionNode(gatheringDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						gatheringDef.Worker.TryExecute(Find.CurrentMap, null);
					},
					labelGetter = delegate()
					{
						TaggedString t = gatheringDef.LabelCap + " (";
						World world = Find.World;
						return t + ((((world != null) ? world.factionManager : null) != null && gatheringDef.Worker.CanExecute(Find.CurrentMap, null)) ? "Yes" : "No") + ")";
					}
				});
			}
			return list;
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x001508B8 File Offset: 0x0014EAB8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void StartPrisonBreak(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return;
			}
			PrisonBreakUtility.StartPrisonBreak(p);
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x001508C9 File Offset: 0x0014EAC9
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void PassToWorld(Pawn p)
		{
			p.DeSpawn(DestroyMode.Vanish);
			Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.KeepForever);
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x001508E0 File Offset: 0x0014EAE0
		[DebugAction("Spawning", "Remove world pawn...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void RemoveWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAliveOrDead)
			{
				Pawn pLocal = pawn;
				string text = pawn.LabelShort;
				WorldPawnSituation situation = Find.WorldPawns.GetSituation(pawn);
				if (situation != WorldPawnSituation.Free)
				{
					text = text + " [" + situation.ToString() + "]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
				{
					Find.WorldPawns.RemovePawn(pLocal);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x001509A8 File Offset: 0x0014EBA8
		[DebugAction("Pawns", "Make +1 year older", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Make1YearOlder(Pawn p)
		{
			float num = p.ageTracker.BiologicalTicksPerTick;
			if (num == 0f)
			{
				num = 1f;
			}
			p.ageTracker.AgeTickMothballed(Mathf.RoundToInt(3600000f / num));
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x001509E6 File Offset: 0x0014EBE6
		[DebugAction("Pawns", "Make +1 day older", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Make1DayOlder(Pawn p)
		{
			p.ageTracker.AgeTickMothballed(60000);
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x001509F8 File Offset: 0x0014EBF8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryJobGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localType2 in typeof(ThinkNode_JobGiver).AllSubclasses())
			{
				Type localType = localType2;
				list.Add(new DebugMenuOption(localType.Name, DebugMenuOptionMode.Action, delegate()
				{
					ThinkNode_JobGiver thinkNode_JobGiver = (ThinkNode_JobGiver)Activator.CreateInstance(localType);
					thinkNode_JobGiver.ResolveReferences();
					ThinkResult thinkResult = thinkNode_JobGiver.TryIssueJobPackage(p, default(JobIssueParams));
					if (thinkResult.Job != null)
					{
						p.jobs.StartJob(thinkResult.Job, JobCondition.None, null, false, true, null, null, false, false, null, false, true, false);
						return;
					}
					Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x00150AB0 File Offset: 0x0014ECB0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryJoyGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<JoyGiverDef>.Enumerator enumerator = DefDatabase<JoyGiverDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JoyGiverDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.Worker.CanBeGivenTo(p) ? def.defName : (def.defName + " [NO]"), DebugMenuOptionMode.Action, delegate()
					{
						Job job = def.Worker.TryGiveJob(p);
						if (job != null)
						{
							p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
							return;
						}
						Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x00150B88 File Offset: 0x0014ED88
		[DebugAction("Pawns", "EndCurrentJob(InterruptForced)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void EndCurrentJobInterruptForced(Pawn p)
		{
			p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00150B9F File Offset: 0x0014ED9F
		[DebugAction("Pawns", "CheckForJobOverride", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckForJobOverride(Pawn p)
		{
			p.jobs.CheckForJobOverride(0f, true);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x00150BB8 File Offset: 0x0014EDB8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleJobLogging(Pawn p)
		{
			if (((p != null) ? p.jobs : null) == null)
			{
				return;
			}
			p.jobs.debugLog = !p.jobs.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
			MoteMaker.ThrowText(p.DrawPosHeld.Value, p.MapHeld, p.LabelShort + "\n" + (p.jobs.debugLog ? "ON" : "OFF"), -1f);
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x00150C3C File Offset: 0x0014EE3C
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TogglePathDebugging(Pawn p)
		{
			if (((p != null) ? p.jobs : null) == null)
			{
				return;
			}
			p.pather.debugLog = !p.pather.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
			MoteMaker.ThrowText(p.DrawPosHeld.Value, p.MapHeld, p.LabelShort + "\n" + (p.pather.debugLog ? "ON" : "OFF"), -1f);
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x00150CBE File Offset: 0x0014EEBE
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void LogJobDetails(Pawn p)
		{
			if (p.CurJob != null)
			{
				p.CurJob.LogDetails(p);
			}
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00150CD4 File Offset: 0x0014EED4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ToggleStanceLogging(Pawn p)
		{
			p.stances.debugLog = !p.stances.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x00150CF8 File Offset: 0x0014EEF8
		[DebugAction("Pawns", "Kidnap colonist", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void Kidnap(Pawn p)
		{
			if (p.IsColonist)
			{
				Faction faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
				if (faction != null)
				{
					faction.kidnapped.Kidnap(p, faction.leader);
				}
			}
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x00150D34 File Offset: 0x0014EF34
		[DebugAction("Pawns", "Face cell (selected)...", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Selected_SetFacing()
		{
			foreach (object obj in Find.Selector.SelectedObjectsListForReading)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null)
				{
					pawn.rotationTracker.FaceTarget(UI.MouseCell());
				}
			}
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x00150DA4 File Offset: 0x0014EFA4
		[DebugAction("Pawns", "Set enemy target for (selected)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Selected_SetTarget(Pawn p)
		{
			foreach (object obj in Find.Selector.SelectedObjectsListForReading)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null)
				{
					pawn.mindState.enemyTarget = p;
					pawn.mindState.lastEngageTargetTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x00150E20 File Offset: 0x0014F020
		[DebugAction("Pawns", "Progress life stage", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ProgressLifeStage(Pawn p)
		{
			int curLifeStageIndex = p.ageTracker.CurLifeStageIndex;
			if (curLifeStageIndex < p.ageTracker.MaxRaceLifeStageIndex)
			{
				float minAge = p.RaceProps.lifeStageAges[curLifeStageIndex + 1].minAge;
				float minAge2 = p.RaceProps.lifeStageAges[p.RaceProps.lifeStageAges.Count - 1].minAge;
				if (p.RaceProps.Humanlike)
				{
					p.ageTracker.DebugSetAge((long)minAge * 3600000L);
				}
				else
				{
					p.ageTracker.DebugSetGrowth(minAge / minAge2);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x00150EBF File Offset: 0x0014F0BF
		[DebugAction("Pawns", "Make guilty", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void MakeGuilty(Pawn p)
		{
			Pawn_GuiltTracker guilt = p.guilt;
			if (guilt != null)
			{
				guilt.Notify_Guilty(60000);
			}
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x00150EDD File Offset: 0x0014F0DD
		[DebugAction("Pawns", "Force age reversal demand now", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true, requiresIdeology = true)]
		private static void ForceAgeReversalDemandNow(Pawn p)
		{
			p.ageTracker.DebugForceAgeReversalDemandNow();
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x00150EF0 File Offset: 0x0014F0F0
		[DebugAction("Pawns", "Reset age reversal demand", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true, requiresIdeology = true)]
		private static void ResetAgeReversalDemandNow(Pawn p)
		{
			p.ageTracker.DebugResetAgeReversalDemand();
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x00150F04 File Offset: 0x0014F104
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Resurrect()
		{
			foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap).ToList<Thing>())
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					ResurrectionUtility.TryResurrect(corpse.InnerPawn, null);
				}
			}
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x00150F74 File Offset: 0x0014F174
		public static List<DebugMenuOption> Options_AddGene(Action<GeneDef> callback)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GeneDef localDef2 in from x in DefDatabase<GeneDef>.AllDefs
			orderby x.defName
			select x)
			{
				GeneDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					callback(localDef);
				}));
			}
			return list;
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x00151030 File Offset: 0x0014F230
		public static List<DebugMenuOption> Options_RemoveGene(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			if (pawn.genes != null)
			{
				foreach (Gene localG2 in pawn.genes.GenesListForReading)
				{
					Gene localG = localG2;
					list.Add(new DebugMenuOption(localG.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						pawn.genes.RemoveGene(localG);
					}));
				}
			}
			return list;
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x001510E4 File Offset: 0x0014F2E4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true, displayPriority = 1000)]
		private static List<DebugActionNode> AddGene()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			DebugActionNode debugActionNode = new DebugActionNode("Xenogene", DebugActionType.Action, null, null);
			foreach (DebugActionNode child in DebugToolsPawns.<AddGene>g__GeneOptionNodes|89_0(true, false))
			{
				debugActionNode.AddChild(child);
			}
			list.Add(debugActionNode);
			DebugActionNode debugActionNode2 = new DebugActionNode("Endogene", DebugActionType.Action, null, null);
			foreach (DebugActionNode child2 in DebugToolsPawns.<AddGene>g__GeneOptionNodes|89_0(false, false))
			{
				debugActionNode2.AddChild(child2);
			}
			list.Add(debugActionNode2);
			DebugActionNode debugActionNode3 = new DebugActionNode("Heritable", DebugActionType.Action, null, null);
			foreach (DebugActionNode child3 in DebugToolsPawns.<AddGene>g__GeneOptionNodes|89_0(false, true))
			{
				debugActionNode3.AddChild(child3);
			}
			list.Add(debugActionNode3);
			return list;
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x00151210 File Offset: 0x0014F410
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true, displayPriority = 1000)]
		private static void RemoveGene(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_RemoveGene(p), null));
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x00151228 File Offset: 0x0014F428
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true, displayPriority = 1000)]
		private static List<DebugActionNode> SetXenotype()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (XenotypeDef localDef2 in from x in DefDatabase<XenotypeDef>.AllDefs
			orderby x.defName
			select x)
			{
				XenotypeDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						Pawn_GeneTracker genes = p.genes;
						if (genes != null)
						{
							genes.SetXenotype(localDef);
						}
						DebugActionsUtility.DustPuffFrom(p);
					}
				});
			}
			return list;
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x001512D0 File Offset: 0x0014F4D0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static List<DebugActionNode> AddLearningDesire()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (LearningDesireDef defLocal2 in DefDatabase<LearningDesireDef>.AllDefs)
			{
				LearningDesireDef defLocal = defLocal2;
				list.Add(new DebugActionNode(defLocal.defName, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					Pawn_LearningTracker learning = p.learning;
					if (learning != null)
					{
						learning.Debug_SetLearningDesire(defLocal);
					}
					DebugActionsUtility.DustPuffFrom(p);
				}));
			}
			return list;
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x00151350 File Offset: 0x0014F550
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static void TryLearningGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<LearningDesireDef>.Enumerator enumerator = DefDatabase<LearningDesireDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LearningDesireDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.Worker.CanDo(p) ? def.defName : (def.defName + " [NO]"), DebugMenuOptionMode.Action, delegate()
					{
						Job job = def.Worker.TryGiveJob(p);
						if (job != null)
						{
							p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
							return;
						}
						Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x00151428 File Offset: 0x0014F628
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ListMeleeVerbs(Pawn p)
		{
			List<Verb> allMeleeVerbs = (from x in p.meleeVerbs.GetUpdatedAvailableVerbsList(false)
			select x.verb).ToList<Verb>();
			float highestWeight = 0f;
			foreach (Verb v2 in allMeleeVerbs)
			{
				float num = VerbUtility.InitialVerbWeight(v2, p);
				if (num > highestWeight)
				{
					highestWeight = num;
				}
			}
			float totalSelectionWeight = 0f;
			foreach (Verb verb in allMeleeVerbs)
			{
				totalSelectionWeight += VerbUtility.FinalSelectionWeight(verb, p, allMeleeVerbs, highestWeight);
			}
			allMeleeVerbs.SortBy((Verb x) => -VerbUtility.InitialVerbWeight(x, p));
			List<TableDataGetter<Verb>> list = new List<TableDataGetter<Verb>>();
			list.Add(new TableDataGetter<Verb>("verb", (Verb v) => v.ToString().Split('/', StringSplitOptions.None)[1].TrimEnd(')')));
			list.Add(new TableDataGetter<Verb>("source", delegate(Verb v)
			{
				if (v.HediffSource != null)
				{
					return v.HediffSource.Label;
				}
				if (v.tool != null)
				{
					return v.tool.label;
				}
				return "";
			}));
			list.Add(new TableDataGetter<Verb>("damage", (Verb v) => v.verbProps.AdjustedMeleeDamageAmount(v, p)));
			list.Add(new TableDataGetter<Verb>("cooldown", (Verb v) => v.verbProps.AdjustedCooldown(v, p).ToString() + "s"));
			list.Add(new TableDataGetter<Verb>("dmg/sec", (Verb v) => VerbUtility.DPS(v, p)));
			list.Add(new TableDataGetter<Verb>("armor pen", (Verb v) => v.verbProps.AdjustedArmorPenetration(v, p)));
			list.Add(new TableDataGetter<Verb>("hediff", delegate(Verb v)
			{
				string text = "";
				if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty<DamageDefAdditionalHediff>())
				{
					foreach (DamageDefAdditionalHediff damageDefAdditionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
					{
						text = text + damageDefAdditionalHediff.hediff.label + " ";
					}
				}
				return text;
			}));
			list.Add(new TableDataGetter<Verb>("weight", (Verb v) => VerbUtility.InitialVerbWeight(v, p)));
			list.Add(new TableDataGetter<Verb>("category", delegate(Verb v)
			{
				VerbSelectionCategory selectionCategory = v.GetSelectionCategory(p, highestWeight);
				if (selectionCategory == VerbSelectionCategory.Best)
				{
					return "Best".Colorize(Color.green);
				}
				if (selectionCategory != VerbSelectionCategory.Worst)
				{
					return "Mid";
				}
				return "Worst".Colorize(Color.grey);
			}));
			list.Add(new TableDataGetter<Verb>("sel %", (Verb v) => base.<ListMeleeVerbs>g__GetSelectionPercent|1(v).ToStringPercent("F2")));
			List<TableDataGetter<Verb>> list2 = list;
			DebugTables.MakeTablesDialog<Verb>(allMeleeVerbs, list2.ToArray());
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x001516D4 File Offset: 0x0014F8D4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DoVoiceCall(Pawn p)
		{
			Pawn_CallTracker caller = p.caller;
			if (caller == null)
			{
				return;
			}
			caller.DoCall(false);
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x001516E8 File Offset: 0x0014F8E8
		[DebugAction("Pawns", "Force vomit", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ForceVomit(Pawn p)
		{
			p.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x001441BF File Offset: 0x001423BF
		[DebugAction("Pawns", "Reset pawn render cache", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ResetRenderCache(Pawn p)
		{
			p.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x00151728 File Offset: 0x0014F928
		[DebugAction("Anomaly", "Max ghoul upgrades", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void MaxGhoulUpgrades(Pawn p)
		{
			p.health.AddHediff(HediffDefOf.GhoulBarbs, p.health.hediffSet.GetBodyPartRecord(BodyPartDefOf.Torso), null, null);
			p.health.AddHediff(HediffDefOf.GhoulPlating, p.health.hediffSet.GetBodyPartRecord(BodyPartDefOf.Torso), null, null);
			p.health.AddHediff(HediffDefOf.AdrenalHeart, p.health.hediffSet.GetBodyPartRecord(BodyPartDefOf.Heart), null, null);
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x001517C8 File Offset: 0x0014F9C8
		[DebugAction("Anomaly", "Raise corpse as shambler", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void RaiseAsShambler()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					MutantUtility.ResurrectAsShambler(corpse.InnerPawn, -1, null);
					break;
				}
			}
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x00151834 File Offset: 0x0014FA34
		[DebugAction("Anomaly", "Set mutant...", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SetMutant()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<MutantDef> enumerator = DefDatabase<MutantDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MutantDef mutant = enumerator.Current;
					list.Add(new DebugActionNode(mutant.LabelCap, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
					{
						MutantUtility.SetPawnAsMutantInstantly(p, mutant, RotStage.Fresh);
					}));
				}
			}
			return list;
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x001518B4 File Offset: 0x0014FAB4
		[DebugAction("Anomaly", "Revert mutant", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void RevertMutant(Pawn p)
		{
			if (p.IsMutant)
			{
				p.mutant.Revert(false);
				p.mutant = null;
			}
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x001518D4 File Offset: 0x0014FAD4
		[DebugAction("Pawns", "Create baby from parents", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static void CreateBabyFromParents()
		{
			DebugTool tool = null;
			Pawn parent1 = null;
			Action <>9__2;
			tool = new DebugTool("First parent...", delegate()
			{
				parent1 = DebugToolsPawns.<CreateBabyFromParents>g__PawnAt|102_0(UI.MouseCell());
				if (parent1 != null)
				{
					string label = "Second parent...";
					Action clickAction;
					if ((clickAction = <>9__2) == null)
					{
						clickAction = (<>9__2 = delegate()
						{
							Pawn pawn = DebugToolsPawns.<CreateBabyFromParents>g__PawnAt|102_0(UI.MouseCell());
							if (pawn != null)
							{
								PregnancyUtility.ApplyBirthOutcome(RitualOutcomeEffectDefOf.ChildBirth.BestOutcome, 1f, null, null, parent1, pawn, pawn, null, null, null, false);
							}
							DebugTools.curTool = tool;
						});
					}
					DebugTools.curTool = new DebugTool(label, clickAction, null);
				}
			}, null);
			DebugTools.curTool = tool;
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x00151920 File Offset: 0x0014FB20
		[DebugAction("Pawns", "Infection pathway debugger", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OpenInfectionPathwayDebugger()
		{
			Dialog_DevInfectionPathways dialog_DevInfectionPathways;
			if (Find.WindowStack.TryGetWindow<Dialog_DevInfectionPathways>(out dialog_DevInfectionPathways))
			{
				dialog_DevInfectionPathways.Close(true);
			}
			Find.WindowStack.Add(new Dialog_DevInfectionPathways());
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x00151954 File Offset: 0x0014FB54
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> AddInfectionPathway()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<InfectionPathwayDef> enumerator = DefDatabase<InfectionPathwayDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InfectionPathwayDef def = enumerator.Current;
					string label = (!def.PawnRequired) ? def.defName : (def.defName + " [NO]");
					list.Add(new DebugActionNode(label, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
					{
						if (def.PawnRequired)
						{
							return;
						}
						p.infectionVectors.AddInfectionVector(def, null);
						SoundDefOf.Click.PlayOneShotOnCamera(null);
					}));
				}
			}
			return list;
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x001519F8 File Offset: 0x0014FBF8
		[DebugAction("Pawns", "Destroy factionless animals", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAnimals()
		{
			List<Pawn> list = new List<Pawn>();
			list.AddRange(Find.CurrentMap.mapPawns.AllPawnsSpawned);
			foreach (Pawn pawn in list)
			{
				if (pawn.RaceProps.Animal && pawn.Faction == null)
				{
					pawn.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x00151A74 File Offset: 0x0014FC74
		[DebugAction("Pawns", "Destroy player animals", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyPlayerAnimals()
		{
			List<Pawn> list = new List<Pawn>();
			list.AddRange(Find.CurrentMap.mapPawns.SpawnedColonyAnimals);
			foreach (Pawn pawn in list)
			{
				pawn.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x00151ADC File Offset: 0x0014FCDC
		[DebugAction("Pawns", "Destroy non-colonists", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAllNonColonists()
		{
			IReadOnlyList<Pawn> allPawnsSpawned = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int i = allPawnsSpawned.Count - 1; i >= 0; i--)
			{
				Pawn pawn = allPawnsSpawned[i];
				if (!pawn.IsColonist || !pawn.RaceProps.Humanlike)
				{
					pawn.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x00151B30 File Offset: 0x0014FD30
		[DebugAction("Other", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, hideInSubMenu = true)]
		private static void GarbageCollectWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WorldPawnSituation worldPawnSituation in (WorldPawnSituation[])Enum.GetValues(typeof(WorldPawnSituation)))
			{
				if (worldPawnSituation != WorldPawnSituation.None)
				{
					foreach (Pawn pawn in Find.WorldPawns.GetPawnsBySituation(worldPawnSituation).OrderBy(delegate(Pawn x)
					{
						Faction faction = x.Faction;
						if (faction == null)
						{
							return -1;
						}
						return faction.loadID;
					}))
					{
						if (pawn.relations != null)
						{
							Pawn local = pawn;
							list.Add(new DebugMenuOption(pawn.LabelShort + " [" + worldPawnSituation.ToString() + "]", DebugMenuOptionMode.Action, delegate()
							{
								local.markedForDiscard = true;
								Find.WorldPawns.RemoveAndDiscardPawnViaGC(local);
								int num = Find.RelationshipRecords.CleanupUnusedRecords();
								Log.Message(string.Format("{0} records found and removed.", num));
							}));
						}
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x00151C44 File Offset: 0x0014FE44
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void UnlockAllApparel(Pawn p)
		{
			p.apparel.UnlockAll();
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x00151C54 File Offset: 0x0014FE54
		[DebugAction("Pawns", "Try make animal fish for food", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryFishForFood(Pawn p)
		{
			if (!p.IsAnimal)
			{
				Messages.Message("Must select animal to fish for food.", MessageTypeDefOf.RejectInput, false);
				return;
			}
			Job job = JobGiver_GetFood.TryFindFishJob(p);
			if (job != null)
			{
				p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
				return;
			}
			Messages.Message("Unable to find fish job", MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x00151CC0 File Offset: 0x0014FEC0
		[CompilerGenerated]
		internal static List<DebugActionNode> <SetBackstory>g__BackstoryOptionNodes|39_0(BackstorySlot slot)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			IEnumerable<BackstoryDef> allDefs = DefDatabase<BackstoryDef>.AllDefs;
			Func<BackstoryDef, bool> <>9__1;
			Func<BackstoryDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((BackstoryDef b) => b.slot == slot));
			}
			using (IEnumerator<BackstoryDef> enumerator = allDefs.Where(predicate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BackstoryDef outerBackstory = enumerator.Current;
					list.Add(new DebugActionNode(outerBackstory.defName, DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							if (p.story != null)
							{
								if (slot == BackstorySlot.Adulthood)
								{
									p.story.Adulthood = outerBackstory;
								}
								else
								{
									p.story.Childhood = outerBackstory;
								}
								MeditationFocusTypeAvailabilityCache.ClearFor(p);
								DebugActionsUtility.DustPuffFrom(p);
							}
						}
					});
				}
			}
			return list;
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x00151D80 File Offset: 0x0014FF80
		[CompilerGenerated]
		internal static List<DebugActionNode> <AddGene>g__GeneOptionNodes|89_0(bool xenogene, bool heritableOnly)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (GeneDef localDef2 in from x in DefDatabase<GeneDef>.AllDefs
			orderby x.defName
			select x)
			{
				GeneDef localDef = localDef2;
				if ((xenogene || localDef.biostatArc <= 0) && (!heritableOnly || localDef.endogeneCategory != EndogeneCategory.None || !localDef.forcedTraits.NullOrEmpty<GeneticTraitData>() || DefDatabase<XenotypeDef>.AllDefs.Any((XenotypeDef x) => x.genes.Contains(localDef) && x.inheritable)))
				{
					list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							Pawn_GeneTracker genes = p.genes;
							if (genes != null)
							{
								genes.AddGene(localDef, xenogene);
							}
							DebugActionsUtility.DustPuffFrom(p);
						}
					});
				}
			}
			return list;
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x00151EA4 File Offset: 0x001500A4
		[CompilerGenerated]
		internal static Pawn <CreateBabyFromParents>g__PawnAt|102_0(IntVec3 c)
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(c))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
			}
			return null;
		}
	}
}
