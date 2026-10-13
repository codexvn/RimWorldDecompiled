using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003214 RID: 12820
	[StaticConstructorOnStartup]
	public static class HealthCardUtility
	{
		// Token: 0x06012057 RID: 73815 RVA: 0x005429D0 File Offset: 0x00540BD0
		public static void DrawPawnHealthCard(Rect outRect, Pawn pawn, bool allowOperations, bool showBloodLoss, Thing thingForMedBills)
		{
			if (pawn.Dead && allowOperations)
			{
				Log.Error("Called DrawPawnHealthCard with a dead pawn and allowOperations=true. Operations are disallowed on corpses.");
				allowOperations = false;
			}
			outRect.y += 20f;
			outRect.height -= 20f;
			outRect = outRect.Rounded();
			Rect rect = new Rect(outRect.x, outRect.y, outRect.width * 0.375f, outRect.height).Rounded();
			Rect rect2 = new Rect(rect.xMax, outRect.y, outRect.width - rect.width, outRect.height);
			rect.yMin += 11f;
			HealthCardUtility.DrawHealthSummary(rect, pawn, allowOperations, thingForMedBills);
			HealthCardUtility.DrawHediffListing(rect2.ContractedBy(10f), pawn, showBloodLoss, 0f, Prefs.DevMode && Current.ProgramState == ProgramState.Playing);
		}

		// Token: 0x06012058 RID: 73816 RVA: 0x00542AC0 File Offset: 0x00540CC0
		public static void DrawHealthSummary(Rect rect, Pawn pawn, bool allowOperations, Thing thingForMedBills)
		{
			GUI.color = Color.white;
			if (!allowOperations)
			{
				HealthCardUtility.onOperationTab = false;
			}
			Widgets.DrawMenuSection(rect);
			List<TabRecord> list = new List<TabRecord>();
			list.Add(new TabRecord("HealthOverview".Translate(), delegate()
			{
				HealthCardUtility.onOperationTab = false;
			}, !HealthCardUtility.onOperationTab));
			List<TabRecord> list2 = list;
			if (allowOperations)
			{
				string text = pawn.RaceProps.IsMechanoid ? "MedicalOperationsMechanoidsShort".Translate() : "MedicalOperationsShort".Translate();
				if (pawn.BillStack.Count > 0)
				{
					text = text + " (" + pawn.BillStack.Count.ToString() + ")";
				}
				list2.Add(new TabRecord(text, delegate()
				{
					HealthCardUtility.onOperationTab = true;
				}, HealthCardUtility.onOperationTab));
			}
			TabDrawer.DrawTabs<TabRecord>(rect, list2, 200f);
			rect = rect.ContractedBy(9f);
			Widgets.BeginGroup(rect);
			float curY = 0f;
			Text.Font = GameFont.Medium;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperCenter;
			if (HealthCardUtility.onOperationTab)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.MedicalOperations, KnowledgeAmount.FrameDisplayed);
				curY = HealthCardUtility.DrawMedOperationsTab(rect, pawn, thingForMedBills, curY);
			}
			else
			{
				curY = HealthCardUtility.DrawOverviewTab(rect, pawn, curY);
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.EndGroup();
		}

		// Token: 0x06012059 RID: 73817 RVA: 0x00542C3C File Offset: 0x00540E3C
		public static bool AnyHediffsDisplayed(Pawn pawn, bool showBloodLoss)
		{
			return HealthCardUtility.VisibleHediffGroupsInOrder(pawn, showBloodLoss).Any<IGrouping<BodyPartRecord, Hediff>>();
		}

		// Token: 0x0601205A RID: 73818 RVA: 0x00542C4C File Offset: 0x00540E4C
		public static void DrawHediffListing(Rect rect, Pawn pawn, bool showBloodLoss, float rowLeftPad = 0f, bool showDebugOptions = false)
		{
			GUI.color = Color.white;
			if (showDebugOptions)
			{
				HealthCardUtility.DoDebugOptions(rect, pawn);
			}
			Widgets.BeginGroup(rect);
			float lineHeight = Text.LineHeight;
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height - lineHeight);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, HealthCardUtility.scrollViewHeight);
			Rect rect2 = rect;
			if (viewRect.height > outRect.height)
			{
				rect2.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref HealthCardUtility.scrollPosition, viewRect, true);
			GUI.color = Color.white;
			float b = 0f;
			HealthCardUtility.highlight = true;
			bool flag = false;
			HealthCardUtility.lastMaxIconsTotalWidth = 0f;
			foreach (IGrouping<BodyPartRecord, Hediff> diffs in HealthCardUtility.VisibleHediffGroupsInOrder(pawn, showBloodLoss))
			{
				flag = true;
				HealthCardUtility.DrawHediffRow(rect2, pawn, diffs, ref b, rowLeftPad);
			}
			if (!flag)
			{
				Widgets.NoneLabelCenteredVertically(new Rect(0f, 0f, viewRect.width, outRect.height), "(" + "NoHealthConditions".Translate() + ")");
				b = outRect.height - 1f;
			}
			if (Event.current.type == EventType.Repaint)
			{
				HealthCardUtility.scrollViewHeight = b;
			}
			else if (Event.current.type == EventType.Layout)
			{
				HealthCardUtility.scrollViewHeight = Mathf.Max(HealthCardUtility.scrollViewHeight, b);
			}
			Widgets.EndScrollView();
			float bleedRateTotal = pawn.health.hediffSet.BleedRateTotal;
			if (bleedRateTotal > 0.01f)
			{
				Rect rect3 = new Rect(0f, rect.height - lineHeight, rect.width, lineHeight);
				string text = "BleedingRate".Translate() + ": " + bleedRateTotal.ToStringPercent() + "/" + "LetterDay".Translate();
				int num = HealthUtility.TicksUntilDeathDueToBloodLoss(pawn);
				if (ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Deathless))
				{
					text += " (" + "Deathless".Translate() + ")";
				}
				else if (num < 60000)
				{
					text += " (" + "TimeToDeath".Translate(num.ToStringTicksToPeriod(true, false, true, true, false)) + ")";
				}
				else
				{
					text += " (" + "WontBleedOutSoon".Translate() + ")";
				}
				Widgets.Label(rect3, text);
			}
			Widgets.EndGroup();
			GUI.color = Color.white;
		}

		// Token: 0x0601205B RID: 73819 RVA: 0x00542F50 File Offset: 0x00541150
		private static IEnumerable<IGrouping<BodyPartRecord, Hediff>> VisibleHediffGroupsInOrder(Pawn pawn, bool showBloodLoss)
		{
			foreach (IGrouping<BodyPartRecord, Hediff> grouping in from x in HealthCardUtility.VisibleHediffs(pawn, showBloodLoss)
			group x by x.Part into x
			orderby HealthCardUtility.GetListPriority(x.First<Hediff>().Part) descending
			select x)
			{
				yield return grouping;
			}
			IEnumerator<IGrouping<BodyPartRecord, Hediff>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0601205C RID: 73820 RVA: 0x00542F67 File Offset: 0x00541167
		private static float GetListPriority(BodyPartRecord rec)
		{
			if (rec == null)
			{
				return 9999999f;
			}
			return (float)((int)rec.height * 10000) + rec.coverageAbsWithChildren;
		}

		// Token: 0x0601205D RID: 73821 RVA: 0x00542F86 File Offset: 0x00541186
		private static IEnumerable<Hediff> VisibleHediffs(Pawn pawn, bool showBloodLoss)
		{
			if (!HealthCardUtility.showAllHediffs)
			{
				List<Hediff_MissingPart> mpca = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				int num;
				for (int i = 0; i < mpca.Count; i = num + 1)
				{
					yield return mpca[i];
					num = i;
				}
				IEnumerable<Hediff> enumerable = from d in pawn.health.hediffSet.hediffs
				where !(d is Hediff_MissingPart) && d.Visible && (showBloodLoss || d.def != HediffDefOf.BloodLoss)
				select d;
				foreach (Hediff hediff in enumerable)
				{
					yield return hediff;
				}
				IEnumerator<Hediff> enumerator = null;
				mpca = null;
			}
			else
			{
				foreach (Hediff hediff2 in pawn.health.hediffSet.hediffs)
				{
					yield return hediff2;
				}
				List<Hediff>.Enumerator enumerator2 = default(List<Hediff>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0601205E RID: 73822 RVA: 0x00542FA0 File Offset: 0x005411A0
		private static float DrawMedOperationsTab(Rect leftRect, Pawn pawn, Thing thingForMedBills, float curY)
		{
			curY += 2f;
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				int num = 0;
				foreach (RecipeDef recipeDef in thingForMedBills.def.AllRecipes)
				{
					if (recipeDef.AvailableNow)
					{
						AcceptanceReport report = recipeDef.Worker.AvailableReport(pawn, null);
						if (report.Accepted || !report.Reason.NullOrEmpty())
						{
							IEnumerable<ThingDef> enumerable = recipeDef.PotentiallyMissingIngredients(null, thingForMedBills.MapHeld);
							if (!enumerable.Any((ThingDef x) => x.isTechHediff))
							{
								if (!enumerable.Any((ThingDef x) => x.IsDrug) && (!enumerable.Any<ThingDef>() || !recipeDef.dontShowIfAnyIngredientMissing))
								{
									if (recipeDef.targetsBodyPart)
									{
										using (IEnumerator<BodyPartRecord> enumerator2 = recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).GetEnumerator())
										{
											while (enumerator2.MoveNext())
											{
												BodyPartRecord part = enumerator2.Current;
												if (recipeDef.AvailableOnNow(pawn, part))
												{
													list.Add(HealthCardUtility.GenerateSurgeryOption(pawn, thingForMedBills, recipeDef, enumerable, report, num, part));
													num++;
												}
											}
											continue;
										}
									}
									if (!pawn.health.hediffSet.HasHediff(recipeDef.addsHediff, false))
									{
										list.Add(HealthCardUtility.GenerateSurgeryOption(pawn, thingForMedBills, recipeDef, enumerable, report, num, null));
										num++;
									}
								}
							}
						}
					}
				}
				return list;
			};
			Rect rect = new Rect(leftRect.x - 9f, curY, leftRect.width, leftRect.height - curY - 20f);
			((IBillGiver)thingForMedBills).BillStack.DoListing(rect, recipeOptionsMaker, ref HealthCardUtility.billsScrollPosition, ref HealthCardUtility.billsScrollHeight);
			return curY;
		}

		// Token: 0x0601205F RID: 73823 RVA: 0x00543024 File Offset: 0x00541224
		private static FloatMenuOption GenerateSurgeryOption(Pawn pawn, Thing thingForMedBills, RecipeDef recipe, IEnumerable<ThingDef> missingIngredients, AcceptanceReport report, int index, BodyPartRecord part = null)
		{
			string label = recipe.Worker.GetLabelWhenUsedOn(pawn, part).CapitalizeFirst();
			if (part != null && !recipe.hideBodyPartNames)
			{
				label = label + " (" + part.Label + ")";
			}
			FloatMenuOption floatMenuOption;
			if (!report.Reason.NullOrEmpty())
			{
				label = label + " (" + report.Reason + ")";
				floatMenuOption = new FloatMenuOption(label, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (missingIngredients.Any<ThingDef>())
			{
				label += " (";
				bool flag = true;
				foreach (ThingDef thingDef in missingIngredients)
				{
					if (!flag)
					{
						label += ", ";
					}
					flag = false;
					label += "MissingMedicalBillIngredient".Translate(thingDef.label);
				}
				label += ")";
				floatMenuOption = new FloatMenuOption(label, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				Action<Xenogerm> <>9__3;
				Action action = delegate()
				{
					Pawn medPawn = thingForMedBills as Pawn;
					if (medPawn == null)
					{
						return;
					}
					HediffDef hediffDef = recipe.addsHediff ?? recipe.changesHediffLevel;
					if (hediffDef != null)
					{
						TaggedString text = CompRoyalImplant.CheckForViolations(medPawn, hediffDef, recipe.hediffLevelOffset);
						if (!text.NullOrEmpty())
						{
							Find.WindowStack.Add(new Dialog_MessageBox(text, "Yes".Translate(), delegate()
							{
								HealthCardUtility.CreateSurgeryBill(medPawn, recipe, part, null, true);
							}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
							return;
						}
						TaggedString confirmation = recipe.Worker.GetConfirmation(medPawn);
						if (!confirmation.NullOrEmpty())
						{
							Find.WindowStack.Add(new Dialog_MessageBox(confirmation, "Yes".Translate(), delegate()
							{
								HealthCardUtility.CreateSurgeryBill(medPawn, recipe, part, null, true);
							}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
							return;
						}
						HealthCardUtility.CreateSurgeryBill(medPawn, recipe, part, null, true);
						return;
					}
					else
					{
						if (recipe.Worker is Recipe_ImplantXenogerm)
						{
							WindowStack windowStack = Find.WindowStack;
							Pawn pawn2 = pawn;
							Map map = pawn.Map;
							Xenogerm initialSelected = null;
							Action<Xenogerm> onSelect;
							if ((onSelect = <>9__3) == null)
							{
								onSelect = (<>9__3 = delegate(Xenogerm x)
								{
									x.SetTargetPawn(pawn);
								});
							}
							windowStack.Add(new Dialog_SelectXenogerm(pawn2, map, initialSelected, onSelect));
							return;
						}
						TaggedString confirmation2 = recipe.Worker.GetConfirmation(medPawn);
						if (!confirmation2.NullOrEmpty())
						{
							Find.WindowStack.Add(new Dialog_MessageBox(confirmation2, "Yes".Translate(), delegate()
							{
								HealthCardUtility.CreateSurgeryBill(medPawn, recipe, part, null, true);
							}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
							return;
						}
						HealthCardUtility.CreateSurgeryBill(medPawn, recipe, part, null, true);
						return;
					}
				};
				if (recipe.Worker is Recipe_AdministerIngestible)
				{
					string label2 = label;
					Action action2 = action;
					IngredientCount ingredientCount = recipe.ingredients.FirstOrDefault<IngredientCount>();
					floatMenuOption = new FloatMenuOption(label2, action2, (ingredientCount != null) ? ingredientCount.FixedIngredient : null, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null);
				}
				else if (recipe.Worker is Recipe_RemoveBodyPart)
				{
					floatMenuOption = new FloatMenuOption(label, action, part.def.spawnThingOnRemoved, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null);
				}
				else
				{
					floatMenuOption = new FloatMenuOption(label, action, recipe.UIIconThing, recipe.UIIcon, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null);
				}
			}
			floatMenuOption.extraPartWidth = 29f;
			floatMenuOption.extraPartOnGUI = ((Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, recipe));
			floatMenuOption.mouseoverGuiAction = delegate(Rect rect)
			{
				BillUtility.DoBillInfoWindow(index, label, rect, recipe, part, pawn);
			};
			return floatMenuOption;
		}

		// Token: 0x06012060 RID: 73824 RVA: 0x005432FC File Offset: 0x005414FC
		public static Bill_Medical CreateSurgeryBill(Pawn medPawn, RecipeDef recipe, BodyPartRecord part, List<Thing> uniqueIngredients = null, bool sendMessages = true)
		{
			Bill_Medical bill_Medical = new Bill_Medical(recipe, uniqueIngredients);
			medPawn.BillStack.AddBill(bill_Medical);
			bill_Medical.Part = part;
			if (recipe.conceptLearned != null)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
			}
			if (sendMessages)
			{
				Map mapHeld = medPawn.MapHeld;
				if (!(from p in mapHeld.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.IsFreeColonist || p.IsColonyMechPlayerControlled
				select p).Any((Pawn col) => recipe.PawnSatisfiesSkillRequirements(col)))
				{
					Bill.CreateNoPawnsWithSkillDialog(recipe);
				}
				if (!medPawn.InBed() && medPawn.RaceProps.IsFlesh)
				{
					if (medPawn.RaceProps.Humanlike)
					{
						if (!mapHeld.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(medPawn, x.def) && ((Building_Bed)x).Medical))
						{
							Messages.Message("MessageNoMedicalBeds".Translate(), medPawn, MessageTypeDefOf.CautionInput, false);
						}
					}
					else if (!mapHeld.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(medPawn, x.def)))
					{
						Messages.Message("MessageNoAnimalBeds".Translate(), medPawn, MessageTypeDefOf.CautionInput, false);
					}
				}
				if (medPawn.Faction != null && !medPawn.Faction.Hidden && !medPawn.Faction.HostileTo(Faction.OfPlayer) && recipe.Worker.IsViolationOnPawn(medPawn, part, Faction.OfPlayer))
				{
					Messages.Message("MessageMedicalOperationWillAngerFaction".Translate(medPawn.HomeFaction), medPawn, MessageTypeDefOf.CautionInput, false);
				}
				if (!HealthCardUtility.CanDoRecipeWithMedicineRestriction(medPawn, recipe))
				{
					Messages.Message("MessageWarningNoMedicineForRestriction".Translate(medPawn.Named("PAWN"), medPawn.playerSettings.medCare.GetLabel().Named("RESTRICTIONLABEL")), medPawn, MessageTypeDefOf.CautionInput, false);
				}
				recipe.Worker.CheckForWarnings(medPawn);
			}
			return bill_Medical;
		}

		// Token: 0x06012061 RID: 73825 RVA: 0x0054358C File Offset: 0x0054178C
		private static float DrawOverviewTab(Rect rect, Pawn pawn, float curY)
		{
			HealthCardUtility.<>c__DisplayClass30_0 CS$<>8__locals1 = new HealthCardUtility.<>c__DisplayClass30_0();
			CS$<>8__locals1.pawn = pawn;
			curY += 4f;
			bool flag = false;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(0.9f, 0.9f, 0.9f);
			if (CS$<>8__locals1.pawn.foodRestriction != null && CS$<>8__locals1.pawn.foodRestriction.Configurable && !CS$<>8__locals1.pawn.DevelopmentalStage.Baby())
			{
				Pawn_NeedsTracker needs = CS$<>8__locals1.pawn.needs;
				if (((needs != null) ? needs.food : null) != null && (!CS$<>8__locals1.pawn.IsMutant || !CS$<>8__locals1.pawn.mutant.Def.disablePolicies))
				{
					Rect rect2 = new Rect(0f, curY, rect.width, 23f);
					flag = true;
					TooltipHandler.TipRegionByKey(rect2, "FoodRestrictionDescription");
					Widgets.DrawHighlightIfMouseover(rect2);
					Rect rect3 = rect2;
					rect3.xMax = rect2.center.x - 4f;
					Rect rect4 = rect2;
					rect4.xMin = rect2.center.x + 4f;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect3, string.Format("{0}:", "AllowFood".Translate()));
					Text.Anchor = TextAnchor.UpperLeft;
					if (Widgets.ButtonText(rect4, CS$<>8__locals1.pawn.foodRestriction.CurrentFoodPolicy.label, true, true, true, null))
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						using (List<FoodPolicy>.Enumerator enumerator = Current.Game.foodRestrictionDatabase.AllFoodRestrictions.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								FoodPolicy restriction = enumerator.Current;
								list.Add(new FloatMenuOption(restriction.label, delegate()
								{
									CS$<>8__locals1.pawn.foodRestriction.CurrentFoodPolicy = restriction;
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
						}
						list.Add(new FloatMenuOption("ManageFoodPolicies".Translate() + "...", delegate()
						{
							Find.WindowStack.Add(new Dialog_ManageFoodPolicies(CS$<>8__locals1.pawn.foodRestriction.CurrentFoodPolicy));
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						Find.WindowStack.Add(new FloatMenu(list));
					}
					curY += rect2.height + 4f;
				}
			}
			bool flag2 = CS$<>8__locals1.pawn.Faction == Faction.OfPlayer || CS$<>8__locals1.pawn.HostFaction == Faction.OfPlayer;
			bool flag3 = CS$<>8__locals1.pawn.NonHumanlikeOrWildMan() && CS$<>8__locals1.pawn.InBed() && CS$<>8__locals1.pawn.CurrentBed().Faction == Faction.OfPlayer;
			if (CS$<>8__locals1.pawn.RaceProps.IsFlesh && (flag2 || flag3) && (!CS$<>8__locals1.pawn.IsMutant || CS$<>8__locals1.pawn.mutant.Def.entitledToMedicalCare) && CS$<>8__locals1.pawn.playerSettings != null && !CS$<>8__locals1.pawn.Dead && Current.ProgramState == ProgramState.Playing)
			{
				Rect rect5 = new Rect(0f, curY, rect.width, 23f);
				flag = true;
				TooltipHandler.TipRegionByKey(rect5, "MedicineQualityDescription");
				Widgets.DrawHighlightIfMouseover(rect5);
				Rect rect6 = rect5;
				rect6.xMax = rect5.center.x - 4f;
				Rect rect7 = rect5;
				rect7.xMin = rect5.center.x + 4f;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect6, string.Format("{0}:", "AllowMedicine".Translate()));
				Text.Anchor = TextAnchor.UpperLeft;
				Widgets.DrawButtonGraphic(rect7);
				MedicalCareUtility.MedicalCareSelectButton(rect7, CS$<>8__locals1.pawn);
				curY += rect5.height + 4f;
			}
			if (Current.ProgramState == ProgramState.Playing && CS$<>8__locals1.pawn.IsColonist && !CS$<>8__locals1.pawn.Dead && !CS$<>8__locals1.pawn.DevelopmentalStage.Baby() && CS$<>8__locals1.pawn.playerSettings != null)
			{
				Rect rect8 = new Rect(0f, curY, rect.width, 23f);
				flag = true;
				TooltipHandler.TipRegion(rect8, "AllowSelfTendTip".Translate(Faction.OfPlayer.def.pawnsPlural, 0.7f.ToStringPercent()).CapitalizeFirst());
				Widgets.DrawHighlightIfMouseover(rect8);
				Rect rect9 = rect8;
				rect9.xMax = rect8.center.x - 4f;
				Rect rect10 = rect8;
				rect10.xMin = rect8.center.x + 4f;
				rect10.width = rect10.height;
				rect10.ContractedBy(4f);
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect9, string.Format("{0}:", "AllowSelfTend".Translate()));
				Text.Anchor = TextAnchor.UpperLeft;
				bool selfTend = CS$<>8__locals1.pawn.playerSettings.selfTend;
				Widgets.Checkbox(rect10.x, rect10.y, ref CS$<>8__locals1.pawn.playerSettings.selfTend, rect10.height, false, false, null, null);
				if (CS$<>8__locals1.pawn.playerSettings.selfTend && !selfTend)
				{
					if (CS$<>8__locals1.pawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
					{
						CS$<>8__locals1.pawn.playerSettings.selfTend = false;
						Messages.Message("MessageCannotSelfTendEver".Translate(CS$<>8__locals1.pawn.LabelShort, CS$<>8__locals1.pawn), MessageTypeDefOf.RejectInput, false);
					}
					else if (CS$<>8__locals1.pawn.workSettings.GetPriority(WorkTypeDefOf.Doctor) == 0)
					{
						Messages.Message("MessageSelfTendUnsatisfied".Translate(CS$<>8__locals1.pawn.LabelShort, CS$<>8__locals1.pawn), MessageTypeDefOf.CautionInput, false);
					}
				}
				curY += rect8.height + 10f;
			}
			if (flag)
			{
				Widgets.DrawLineHorizontal(rect.x - 8f, curY, rect.width, Color.gray);
			}
			curY += 10f;
			if (CS$<>8__locals1.pawn.def.race.IsFlesh)
			{
				Pair<string, Color> painLabel = HealthCardUtility.GetPainLabel(CS$<>8__locals1.pawn);
				string painTip = HealthCardUtility.GetPainTip(CS$<>8__locals1.pawn);
				HealthCardUtility.DrawLeftRow(rect, ref curY, "PainLevel".Translate(), painLabel.First, painLabel.Second, painTip);
			}
			curY += 6f;
			if (!CS$<>8__locals1.pawn.Dead)
			{
				IEnumerable<PawnCapacityDef> source;
				if (CS$<>8__locals1.pawn.def.race.Humanlike)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnHumanlikes
					select x;
				}
				else if (CS$<>8__locals1.pawn.def.race.Animal)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnAnimals
					select x;
				}
				else if (CS$<>8__locals1.pawn.def.race.IsAnomalyEntity)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnAnomalyEntities
					select x;
				}
				else if (CS$<>8__locals1.pawn.def.race.IsDrone)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnDrones
					select x;
				}
				else
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnMechanoids
					select x;
				}
				foreach (PawnCapacityDef pawnCapacityDef in from act in source
				orderby act.listOrder
				select act)
				{
					HealthCardUtility.<>c__DisplayClass30_2 CS$<>8__locals3 = new HealthCardUtility.<>c__DisplayClass30_2();
					CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
					if (PawnCapacityUtility.BodyCanEverDoCapacity(CS$<>8__locals3.CS$<>8__locals2.pawn.RaceProps.body, pawnCapacityDef))
					{
						CS$<>8__locals3.activityLocal = pawnCapacityDef;
						Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel(CS$<>8__locals3.CS$<>8__locals2.pawn, pawnCapacityDef);
						HealthCardUtility.DrawLeftRow(rect, ref curY, pawnCapacityDef.GetLabelFor(CS$<>8__locals3.CS$<>8__locals2.pawn).CapitalizeFirst(), efficiencyLabel.First, efficiencyLabel.Second, new TipSignal(new Func<string>(CS$<>8__locals3.<DrawOverviewTab>g__TipGetter|8), CS$<>8__locals3.CS$<>8__locals2.pawn.thingIDNumber ^ (int)pawnCapacityDef.index));
					}
				}
			}
			return curY;
		}

		// Token: 0x06012062 RID: 73826 RVA: 0x00543EC0 File Offset: 0x005420C0
		private static void DrawLeftRow(Rect rect, ref float curY, string leftLabel, string rightLabel, Color rightLabelColor, TipSignal tipSignal)
		{
			Rect rect2 = new Rect(17f, curY, rect.width - 34f - 10f, 22f);
			if (Mouse.IsOver(rect2))
			{
				using (new TextBlock(HealthCardUtility.HighlightColor))
				{
					GUI.DrawTexture(rect2, TexUI.HighlightTex);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect2, leftLabel);
			GUI.color = rightLabelColor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, rightLabel);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = new Rect(0f, curY, rect.width, 20f);
			if (Mouse.IsOver(rect3))
			{
				TooltipHandler.TipRegion(rect3, tipSignal);
			}
			curY += rect2.height;
		}

		// Token: 0x06012063 RID: 73827 RVA: 0x00543F9C File Offset: 0x0054219C
		private static void DrawHediffRow(Rect rect, Pawn pawn, IEnumerable<Hediff> diffs, ref float curY, float rowLeftPad = 0f)
		{
			HealthCardUtility.<>c__DisplayClass32_0 CS$<>8__locals1 = new HealthCardUtility.<>c__DisplayClass32_0();
			CS$<>8__locals1.pawn = pawn;
			float num = rect.width * 0.375f;
			float width = rect.width - num - HealthCardUtility.lastMaxIconsTotalWidth;
			CS$<>8__locals1.part = diffs.First<Hediff>().Part;
			float a;
			if (CS$<>8__locals1.part == null)
			{
				a = Text.CalcHeight("WholeBody".Translate(), num);
			}
			else
			{
				a = Text.CalcHeight(CS$<>8__locals1.part.LabelCap, num);
			}
			float b = 0f;
			float num2 = curY;
			float num3 = 0f;
			foreach (IGrouping<int, Hediff> source in from x in diffs
			group x by x.UIGroupKey)
			{
				int num4 = source.Count<Hediff>();
				string text = source.First<Hediff>().LabelCap;
				if (num4 != 1)
				{
					text = text + " x" + num4.ToString();
				}
				num3 += Text.CalcHeight(text, width);
			}
			b = num3;
			HealthCardUtility.DoRightRowHighlight(new Rect(0f, curY, rect.width, Mathf.Max(a, b)));
			if (CS$<>8__locals1.part != null)
			{
				GUI.color = HealthUtility.GetPartConditionLabel(CS$<>8__locals1.pawn, CS$<>8__locals1.part).Second;
				Widgets.Label(new Rect(rowLeftPad, curY, num, 100f), CS$<>8__locals1.part.LabelCap);
			}
			else
			{
				GUI.color = HealthUtility.RedColor;
				Widgets.Label(new Rect(rowLeftPad, curY, num, 100f), "WholeBody".Translate());
			}
			GUI.color = Color.white;
			foreach (IGrouping<int, Hediff> grouping in from x in diffs
			group x by x.UIGroupKey)
			{
				HealthCardUtility.<>c__DisplayClass32_1 CS$<>8__locals2 = new HealthCardUtility.<>c__DisplayClass32_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				int num5 = 0;
				Hediff hediff3 = null;
				CS$<>8__locals2.bleedingIcon = null;
				CS$<>8__locals2.stateIcon = null;
				CS$<>8__locals2.totalBleedRate = 0f;
				foreach (Hediff hediff2 in grouping)
				{
					if (num5 == 0)
					{
						hediff3 = hediff2;
					}
					CS$<>8__locals2.stateIcon = hediff2.StateIcon;
					if (hediff2.Bleeding)
					{
						CS$<>8__locals2.bleedingIcon = HealthCardUtility.BleedingIcon;
					}
					CS$<>8__locals2.totalBleedRate += hediff2.BleedRate;
					num5++;
				}
				string text2 = hediff3.LabelCap;
				if (num5 != 1)
				{
					text2 = text2 + " x" + num5.ToStringCached();
				}
				float num6 = Text.CalcHeight(text2, width);
				Rect rect2 = new Rect(num, curY, width, num6);
				Widgets.DrawHighlightIfMouseover(rect2);
				GUI.color = hediff3.LabelColor;
				Widgets.Label(rect2, text2);
				GUI.color = Color.white;
				CS$<>8__locals2.iconsRect = new Rect(rect2.x + 10f, rect2.y, rect.width - num - 10f, rect2.height);
				List<GenUI.AnonymousStackElement> list = new List<GenUI.AnonymousStackElement>();
				Hediff localHediff = hediff3;
				if (DebugSettings.ShowDevGizmos && Current.ProgramState == ProgramState.Playing)
				{
					list.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							float num8 = CS$<>8__locals2.iconsRect.width - (r.x - CS$<>8__locals2.iconsRect.x) - 20f;
							r = new Rect(CS$<>8__locals2.iconsRect.x + num8, r.y, 20f, 20f);
							GUI.color = Color.red;
							TooltipHandler.TipRegion(r, () => "DEV: Remove hediff", 1071045645);
							if (GUI.Button(r, TexButton.Delete))
							{
								CS$<>8__locals2.CS$<>8__locals1.pawn.health.RemoveHediff(localHediff);
							}
							GUI.color = Color.white;
						},
						width = 20f
					});
					if (localHediff.def.maxSeverity < 3.4028235E+38f || localHediff.def.lethalSeverity > 0f)
					{
						list.Add(new GenUI.AnonymousStackElement
						{
							drawer = delegate(Rect r)
							{
								float num8 = CS$<>8__locals2.iconsRect.width - (r.x - CS$<>8__locals2.iconsRect.x) - 20f;
								r = new Rect(CS$<>8__locals2.iconsRect.x + num8, r.y, 20f, 20f);
								GUI.color = Color.cyan;
								TooltipHandler.TipRegion(r, () => "DEV: Set severity", 2131648723);
								if (GUI.Button(r, TexButton.Save))
								{
									Find.WindowStack.Add(new Dialog_DebugSetSeverity(localHediff));
								}
								GUI.color = Color.white;
							},
							width = 20f
						});
					}
					if (localHediff.TryGetComp<HediffComp_Disappears>() != null)
					{
						list.Add(new GenUI.AnonymousStackElement
						{
							drawer = delegate(Rect r)
							{
								float num8 = CS$<>8__locals2.iconsRect.width - (r.x - CS$<>8__locals2.iconsRect.x) - 20f;
								r = new Rect(CS$<>8__locals2.iconsRect.x + num8, r.y, 20f, 20f);
								GUI.color = Color.yellow;
								TooltipHandler.TipRegion(r, () => "DEV: Set remaining time", 6234623);
								if (GUI.Button(r, TexButton.Save))
								{
									Find.WindowStack.Add(new Dialog_DebugSetHediffRemaining(localHediff, null));
								}
								GUI.color = Color.white;
							},
							width = 20f
						});
					}
				}
				list.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						float num8 = CS$<>8__locals2.iconsRect.width - (r.x - CS$<>8__locals2.iconsRect.x) - 20f;
						r = new Rect(CS$<>8__locals2.iconsRect.x + num8, r.y, 20f, 20f);
						Widgets.InfoCardButton(r, localHediff);
					},
					width = 20f
				});
				if (CS$<>8__locals2.bleedingIcon)
				{
					list.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							float num8 = CS$<>8__locals2.iconsRect.width - (r.x - CS$<>8__locals2.iconsRect.x) - 20f;
							r = new Rect(CS$<>8__locals2.iconsRect.x + num8, r.y, 20f, 20f);
							GUI.DrawTexture(r.ContractedBy(GenMath.LerpDouble(0f, 0.6f, 5f, 0f, Mathf.Min(CS$<>8__locals2.totalBleedRate, 1f))), CS$<>8__locals2.bleedingIcon);
						},
						width = 20f
					});
				}
				if (CS$<>8__locals2.stateIcon.HasValue)
				{
					list.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							float num8 = CS$<>8__locals2.iconsRect.width - (r.x - CS$<>8__locals2.iconsRect.x) - 20f;
							r = new Rect(CS$<>8__locals2.iconsRect.x + num8, r.y, 20f, 20f);
							GUI.color = CS$<>8__locals2.stateIcon.Color;
							GUI.DrawTexture(r, CS$<>8__locals2.stateIcon.Texture);
							GUI.color = Color.white;
						},
						width = 20f
					});
				}
				GenUI.DrawElementStack<GenUI.AnonymousStackElement>(CS$<>8__locals2.iconsRect, num6, list, delegate(Rect r, GenUI.AnonymousStackElement obj)
				{
					obj.drawer(r);
				}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, true);
				HealthCardUtility.lastMaxIconsTotalWidth = Mathf.Max(HealthCardUtility.lastMaxIconsTotalWidth, list.Sum((GenUI.AnonymousStackElement x) => x.width + 5f) - 5f);
				if (Mouse.IsOver(rect2))
				{
					int num7 = 0;
					using (IEnumerator<Hediff> enumerator2 = grouping.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Hediff hediff = enumerator2.Current;
							TooltipHandler.TipRegion(rect2, new TipSignal(() => hediff.GetTooltip(CS$<>8__locals2.CS$<>8__locals1.pawn, HealthCardUtility.showHediffsDebugInfo), (int)curY + 7857 + num7++, TooltipPriority.Default));
						}
					}
					if (CS$<>8__locals2.CS$<>8__locals1.part != null)
					{
						Rect rect3 = rect2;
						Func<string> textGetter;
						if ((textGetter = CS$<>8__locals2.CS$<>8__locals1.<>9__14) == null)
						{
							textGetter = (CS$<>8__locals2.CS$<>8__locals1.<>9__14 = (() => HealthCardUtility.GetTooltip(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.CS$<>8__locals1.part)));
						}
						TooltipHandler.TipRegion(rect3, new TipSignal(textGetter, (int)curY + 7857 + ++num7, TooltipPriority.Pawn));
						if (DebugViewSettings.drawWoundAnchorsOnHover && CS$<>8__locals2.CS$<>8__locals1.pawn.Drawer.renderer.WoundOverlays.debugDrawPart != CS$<>8__locals2.CS$<>8__locals1.part)
						{
							CS$<>8__locals2.CS$<>8__locals1.pawn.Drawer.renderer.WoundOverlays.debugDrawPart = CS$<>8__locals2.CS$<>8__locals1.part;
							CS$<>8__locals2.CS$<>8__locals1.pawn.Drawer.renderer.WoundOverlays.ClearCache();
							PortraitsCache.SetDirty(CS$<>8__locals2.CS$<>8__locals1.pawn);
							GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(CS$<>8__locals2.CS$<>8__locals1.pawn);
						}
					}
				}
				if (Widgets.ButtonInvisible(rect2, HealthCardUtility.CanEntryBeClicked(grouping, CS$<>8__locals2.CS$<>8__locals1.pawn)))
				{
					HealthCardUtility.EntryClicked(grouping, CS$<>8__locals2.CS$<>8__locals1.pawn);
				}
				curY += num6;
			}
			GUI.color = Color.white;
			curY = num2 + Mathf.Max(a, b);
		}

		// Token: 0x06012064 RID: 73828 RVA: 0x00544748 File Offset: 0x00542948
		private static bool CanDoRecipeWithMedicineRestriction(IBillGiver giver, RecipeDef recipe)
		{
			Pawn pawn = giver as Pawn;
			if (pawn == null || pawn.playerSettings == null)
			{
				return true;
			}
			if (!recipe.ingredients.Any((IngredientCount x) => x.filter.AnyAllowedDef.IsMedicine))
			{
				return true;
			}
			MedicalCareCategory medicalCareCategory = WorkGiver_DoBill.GetMedicalCareCategory(pawn);
			foreach (Thing thing in pawn.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Medicine))
			{
				using (List<IngredientCount>.Enumerator enumerator2 = recipe.ingredients.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.filter.Allows(thing) && medicalCareCategory.AllowsMedicine(thing.def))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06012065 RID: 73829 RVA: 0x00544848 File Offset: 0x00542A48
		public static string GetPainTip(Pawn pawn)
		{
			return "PainLevel".Translate() + ": " + (pawn.health.hediffSet.PainTotal * 100f).ToString("F0") + "%";
		}

		// Token: 0x06012066 RID: 73830 RVA: 0x005448A0 File Offset: 0x00542AA0
		private static string GetTooltip(Pawn pawn, BodyPartRecord part)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(part.LabelCap + ": ");
			stringBuilder.AppendLine(" " + pawn.health.hediffSet.GetPartHealth(part).ToString() + " / " + part.def.GetMaxHealth(pawn).ToString());
			float num = PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, part, false, null);
			if (num != 1f)
			{
				stringBuilder.AppendLine("Efficiency".Translate() + ": " + num.ToStringPercent());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06012067 RID: 73831 RVA: 0x0054495C File Offset: 0x00542B5C
		public static string GetPawnCapacityTip(Pawn pawn, PawnCapacityDef capacity)
		{
			List<PawnCapacityUtility.CapacityImpactor> list = new List<PawnCapacityUtility.CapacityImpactor>();
			float eff = PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, capacity, list, false);
			list.RemoveAll(delegate(PawnCapacityUtility.CapacityImpactor x)
			{
				PawnCapacityUtility.CapacityImpactorCapacity capacityImpactorCapacity = x as PawnCapacityUtility.CapacityImpactorCapacity;
				return capacityImpactorCapacity != null && !capacityImpactorCapacity.capacity.CanShowOnPawn(pawn);
			});
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(capacity.GetLabelFor(pawn).CapitalizeFirst() + ": " + HealthCardUtility.GetEfficiencyEstimateLabel(eff));
			if (list.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("AffectedBy".Translate());
				for (int i = 0; i < list.Count; i++)
				{
					PawnCapacityUtility.CapacityImpactorHediff capacityImpactorHediff = list[i] as PawnCapacityUtility.CapacityImpactorHediff;
					if (capacityImpactorHediff != null && !HealthCardUtility.tmpHediffImpactors.Contains(capacityImpactorHediff.hediff))
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[i].Readable(pawn)));
						HealthCardUtility.tmpHediffImpactors.Add(capacityImpactorHediff.hediff);
					}
				}
				HealthCardUtility.tmpHediffImpactors.Clear();
				for (int j = 0; j < list.Count; j++)
				{
					PawnCapacityUtility.CapacityImpactorBodyPartHealth capacityImpactorBodyPartHealth = list[j] as PawnCapacityUtility.CapacityImpactorBodyPartHealth;
					if (capacityImpactorBodyPartHealth != null && !HealthCardUtility.tmpBodyPartImpactors.Contains(capacityImpactorBodyPartHealth.bodyPart))
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[j].Readable(pawn)));
						HealthCardUtility.tmpBodyPartImpactors.Add(capacityImpactorBodyPartHealth.bodyPart);
					}
				}
				HealthCardUtility.tmpBodyPartImpactors.Clear();
				for (int k = 0; k < list.Count; k++)
				{
					PawnCapacityUtility.CapacityImpactorGene capacityImpactorGene = list[k] as PawnCapacityUtility.CapacityImpactorGene;
					if (capacityImpactorGene != null && !HealthCardUtility.tmpGeneImpactors.Contains(capacityImpactorGene.gene))
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[k].Readable(pawn)));
						HealthCardUtility.tmpGeneImpactors.Add(capacityImpactorGene.gene);
					}
				}
				HealthCardUtility.tmpGeneImpactors.Clear();
				for (int l = 0; l < list.Count; l++)
				{
					if (list[l] is PawnCapacityUtility.CapacityImpactorCapacity)
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[l].Readable(pawn)));
					}
				}
				for (int m = 0; m < list.Count; m++)
				{
					if (list[m] is PawnCapacityUtility.CapacityImpactorPain)
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[m].Readable(pawn)));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06012068 RID: 73832 RVA: 0x00544C04 File Offset: 0x00542E04
		private static bool CanEntryBeClicked(IEnumerable<Hediff> diffs, Pawn pawn)
		{
			LogEntry combatLogEntry;
			Predicate<LogEntry> <>9__1;
			TaggedString taggedString;
			return HealthCardUtility.GetCombatLogInfo(diffs, out taggedString, out combatLogEntry) && combatLogEntry != null && Find.BattleLog.Battles.Any(delegate(Battle b)
			{
				if (b.Concerns(pawn))
				{
					List<LogEntry> entries = b.Entries;
					Predicate<LogEntry> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((LogEntry e) => e == combatLogEntry));
					}
					return entries.Any(predicate);
				}
				return false;
			});
		}

		// Token: 0x06012069 RID: 73833 RVA: 0x00544C58 File Offset: 0x00542E58
		private static void EntryClicked(IEnumerable<Hediff> diffs, Pawn pawn)
		{
			LogEntry combatLogEntry;
			TaggedString taggedString;
			if (!HealthCardUtility.GetCombatLogInfo(diffs, out taggedString, out combatLogEntry) || combatLogEntry == null)
			{
				return;
			}
			Predicate<LogEntry> <>9__1;
			if (!Find.BattleLog.Battles.Any(delegate(Battle b)
			{
				if (b.Concerns(pawn))
				{
					List<LogEntry> entries = b.Entries;
					Predicate<LogEntry> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((LogEntry e) => e == combatLogEntry));
					}
					return entries.Any(predicate);
				}
				return false;
			}))
			{
				return;
			}
			ITab_Pawn_Log tab_Pawn_Log = InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Log)) as ITab_Pawn_Log;
			if (tab_Pawn_Log == null)
			{
				return;
			}
			tab_Pawn_Log.SeekTo(combatLogEntry);
			tab_Pawn_Log.Highlight(combatLogEntry);
		}

		// Token: 0x0601206A RID: 73834 RVA: 0x00544CDC File Offset: 0x00542EDC
		public static bool GetCombatLogInfo(IEnumerable<Hediff> diffs, out TaggedString combatLogText, out LogEntry combatLogEntry)
		{
			combatLogText = null;
			combatLogEntry = null;
			foreach (Hediff hediff in diffs)
			{
				if ((hediff.combatLogEntry != null && hediff.combatLogEntry.Target != null) || (combatLogText.NullOrEmpty() && !hediff.combatLogText.NullOrEmpty()))
				{
					combatLogEntry = ((hediff.combatLogEntry != null) ? hediff.combatLogEntry.Target : null);
					combatLogText = hediff.combatLogText;
				}
				if (combatLogEntry != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0601206B RID: 73835 RVA: 0x00544D8C File Offset: 0x00542F8C
		private static void DoRightRowHighlight(Rect rowRect)
		{
			if (HealthCardUtility.highlight)
			{
				GUI.color = HealthCardUtility.StaticHighlightColor;
				GUI.DrawTexture(rowRect, TexUI.HighlightTex);
			}
			HealthCardUtility.highlight = !HealthCardUtility.highlight;
		}

		// Token: 0x0601206C RID: 73836 RVA: 0x00544DB8 File Offset: 0x00542FB8
		private static void DoDebugOptions(Rect rightRect, Pawn pawn)
		{
			if (Widgets.ButtonText(new Rect(rightRect.x + 240f, rightRect.y - 27f, 115f, 25f), "Dev tool...", true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>(6);
				list.Add(new FloatMenuOption("Add hediff", delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_AddHediff(pawn), null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				FloatMenuOption floatMenuOption = FloatMenuOption.CheckboxLabeled("Hediff debug tooltips", delegate
				{
					HealthCardUtility.showHediffsDebugInfo = !HealthCardUtility.showHediffsDebugInfo;
				}, HealthCardUtility.showHediffsDebugInfo);
				floatMenuOption.tooltip = new TipSignal?("Hover over hediffs in the health window to get extra debug information about them.");
				list.Add(floatMenuOption);
				list.Add(FloatMenuOption.CheckboxLabeled("Show hidden Hediffs", delegate
				{
					HealthCardUtility.showAllHediffs = !HealthCardUtility.showAllHediffs;
				}, HealthCardUtility.showAllHediffs));
				Func<BodyPartRecord, string> <>9__8;
				Func<BodyPartRecord, string> <>9__11;
				Func<BodyPartRecord, string> <>9__13;
				Func<BodyPartRecord, string> <>9__14;
				Func<BodyPartRecord, string> <>9__15;
				Func<BodyPartRecord, string> <>9__16;
				list.Add(new FloatMenuOption("Table: BodyPartRecord info", delegate()
				{
					float totalCorpseNutrition = 0f;
					if (pawn.RaceProps.hasCorpse)
					{
						totalCorpseNutrition = StatDefOf.Nutrition.Worker.GetValueAbstract(pawn.RaceProps.corpseDef, null);
					}
					IEnumerable<BodyPartRecord> allParts = pawn.RaceProps.body.AllParts;
					TableDataGetter<BodyPartRecord>[] array = new TableDataGetter<BodyPartRecord>[10];
					array[0] = new TableDataGetter<BodyPartRecord>("defName", (BodyPartRecord b) => b.def.defName);
					int num = 1;
					string label = "Coverage";
					Func<BodyPartRecord, string> getter;
					if ((getter = <>9__8) == null)
					{
						getter = (<>9__8 = ((BodyPartRecord b) => pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(b).ToStringPercent()));
					}
					array[num] = new TableDataGetter<BodyPartRecord>(label, getter);
					array[2] = new TableDataGetter<BodyPartRecord>("Hit chance\n(this or any child)", (BodyPartRecord b) => b.coverageAbsWithChildren.ToStringPercent());
					array[3] = new TableDataGetter<BodyPartRecord>("Hit chance\n(this part)", (BodyPartRecord b) => b.coverageAbs.ToStringPercent());
					int num2 = 4;
					string label2 = "Efficiency";
					Func<BodyPartRecord, string> getter2;
					if ((getter2 = <>9__11) == null)
					{
						getter2 = (<>9__11 = ((BodyPartRecord b) => PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, b, false, null).ToStringPercent()));
					}
					array[num2] = new TableDataGetter<BodyPartRecord>(label2, getter2);
					array[5] = new TableDataGetter<BodyPartRecord>("Nutrition", delegate(BodyPartRecord b)
					{
						if (totalCorpseNutrition <= 0f)
						{
							return 0f;
						}
						return FoodUtility.GetBodyPartNutrition(totalCorpseNutrition, pawn, b);
					});
					int num3 = 6;
					string label3 = "Solid";
					Func<BodyPartRecord, string> getter3;
					if ((getter3 = <>9__13) == null)
					{
						getter3 = (<>9__13 = delegate(BodyPartRecord b)
						{
							if (!pawn.health.hediffSet.PartIsMissing(b))
							{
								return b.def.IsSolid(b, pawn.health.hediffSet.hediffs).ToStringCheckBlank();
							}
							return "";
						});
					}
					array[num3] = new TableDataGetter<BodyPartRecord>(label3, getter3);
					int num4 = 7;
					string label4 = "Skin covered";
					Func<BodyPartRecord, string> getter4;
					if ((getter4 = <>9__14) == null)
					{
						getter4 = (<>9__14 = delegate(BodyPartRecord b)
						{
							if (!pawn.health.hediffSet.PartIsMissing(b))
							{
								return b.def.IsSkinCovered(b, pawn.health.hediffSet).ToStringCheckBlank();
							}
							return "";
						});
					}
					array[num4] = new TableDataGetter<BodyPartRecord>(label4, getter4);
					int num5 = 8;
					string label5 = "Is missing";
					Func<BodyPartRecord, string> getter5;
					if ((getter5 = <>9__15) == null)
					{
						getter5 = (<>9__15 = ((BodyPartRecord b) => pawn.health.hediffSet.PartIsMissing(b).ToStringCheckBlank()));
					}
					array[num5] = new TableDataGetter<BodyPartRecord>(label5, getter5);
					int num6 = 9;
					string label6 = "Is missing parts";
					Func<BodyPartRecord, string> getter6;
					if ((getter6 = <>9__16) == null)
					{
						getter6 = (<>9__16 = ((BodyPartRecord b) => pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(b).ToStringCheckBlank()));
					}
					array[num6] = new TableDataGetter<BodyPartRecord>(label6, getter6);
					DebugTables.MakeTablesDialog<BodyPartRecord>(allParts, array);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				Func<BodyPartGroupDef, bool> <>9__17;
				Func<BodyPartGroupDef, string> <>9__19;
				list.Add(new FloatMenuOption("Table: BodyPartGroupDef info", delegate()
				{
					IEnumerable<BodyPartGroupDef> allDefs = DefDatabase<BodyPartGroupDef>.AllDefs;
					Func<BodyPartGroupDef, bool> predicate;
					if ((predicate = <>9__17) == null)
					{
						predicate = (<>9__17 = ((BodyPartGroupDef x) => pawn.RaceProps.body.AllParts.Any((BodyPartRecord y) => y.groups.Contains(x))));
					}
					IEnumerable<BodyPartGroupDef> dataSources = allDefs.Where(predicate);
					TableDataGetter<BodyPartGroupDef>[] array = new TableDataGetter<BodyPartGroupDef>[2];
					array[0] = new TableDataGetter<BodyPartGroupDef>("defName", (BodyPartGroupDef b) => b.defName);
					int num = 1;
					string label = "Efficiency";
					Func<BodyPartGroupDef, string> getter;
					if ((getter = <>9__19) == null)
					{
						getter = (<>9__19 = ((BodyPartGroupDef b) => PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(pawn.health.hediffSet, b).ToStringPercent()));
					}
					array[num] = new TableDataGetter<BodyPartGroupDef>(label, getter);
					DebugTables.MakeTablesDialog<BodyPartGroupDef>(dataSources, array);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list.Add(new FloatMenuOption("Table: HediffGiver_Birthday", delegate()
				{
					List<TableDataGetter<HediffGiver_Birthday>> list2 = new List<TableDataGetter<HediffGiver_Birthday>>();
					list2.Add(new TableDataGetter<HediffGiver_Birthday>("label", (HediffGiver_Birthday b) => b.hediff.LabelCap));
					int num = 1;
					while ((float)num < pawn.RaceProps.lifeExpectancy + 20f)
					{
						int age = num;
						list2.Add(new TableDataGetter<HediffGiver_Birthday>("Chance at\n" + num.ToString(), (HediffGiver_Birthday h) => h.DebugChanceToHaveAtAge(pawn, age).ToStringPercent()));
						num++;
					}
					list2.Add(new TableDataGetter<HediffGiver_Birthday>("Spacing", (HediffGiver_Birthday h) => ""));
					int num2 = 1;
					while ((float)num2 < pawn.RaceProps.lifeExpectancy + 20f)
					{
						int age = num2;
						list2.Add(new TableDataGetter<HediffGiver_Birthday>("Count at\n" + num2.ToString(), delegate(HediffGiver_Birthday h)
						{
							float num3 = 0f;
							foreach (HediffGiverSetDef hediffGiverSetDef in pawn.RaceProps.hediffGiverSets)
							{
								foreach (HediffGiver_Birthday hediffGiver_Birthday in hediffGiverSetDef.hediffGivers.OfType<HediffGiver_Birthday>())
								{
									num3 += hediffGiver_Birthday.DebugChanceToHaveAtAge(pawn, age);
								}
							}
							return num3.ToStringDecimalIfSmall();
						}));
						num2++;
					}
					DebugTables.MakeTablesDialog<HediffGiver_Birthday>(pawn.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef x) => x.hediffGivers.OfType<HediffGiver_Birthday>()), list2.ToArray());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				if (pawn.health.immunity.ImmunityListForReading.Any<ImmunityRecord>())
				{
					list.Add(new FloatMenuOption("Table: Immunities", delegate()
					{
						List<TableDataGetter<ImmunityRecord>> list2 = new List<TableDataGetter<ImmunityRecord>>();
						list2.Add(new TableDataGetter<ImmunityRecord>("hediff", (ImmunityRecord i) => i.hediffDef.LabelCap));
						list2.Add(new TableDataGetter<ImmunityRecord>("source", (ImmunityRecord i) => i.source.LabelCap));
						list2.Add(new TableDataGetter<ImmunityRecord>("immunity", (ImmunityRecord i) => i.immunity.ToStringPercent()));
						DebugTables.MakeTablesDialog<ImmunityRecord>(pawn.health.immunity.ImmunityListForReading, list2.ToArray());
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0601206D RID: 73837 RVA: 0x00544F90 File Offset: 0x00543190
		public static Pair<string, Color> GetEfficiencyLabel(Pawn pawn, PawnCapacityDef activity)
		{
			float level = pawn.health.capacities.GetLevel(activity);
			return new Pair<string, Color>(PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, activity, null, false).ToStringPercent(), HealthCardUtility.efficiencyToColor[HealthCardUtility.EfficiencyValueToEstimate(level)]);
		}

		// Token: 0x0601206E RID: 73838 RVA: 0x00544FDC File Offset: 0x005431DC
		public static string GetEfficiencyEstimateLabel(float eff)
		{
			return HealthCardUtility.EfficiencyValueToEstimate(eff).ToString().Translate();
		}

		// Token: 0x0601206F RID: 73839 RVA: 0x00545008 File Offset: 0x00543208
		public static EfficiencyEstimate EfficiencyValueToEstimate(float eff)
		{
			if (eff <= 0f)
			{
				return EfficiencyEstimate.None;
			}
			if (eff < 0.4f)
			{
				return EfficiencyEstimate.VeryPoor;
			}
			if (eff < 0.7f)
			{
				return EfficiencyEstimate.Poor;
			}
			if (eff < 1f && !Mathf.Approximately(eff, 1f))
			{
				return EfficiencyEstimate.Weakened;
			}
			if (Mathf.Approximately(eff, 1f))
			{
				return EfficiencyEstimate.GoodCondition;
			}
			return EfficiencyEstimate.Enhanced;
		}

		// Token: 0x06012070 RID: 73840 RVA: 0x0054505C File Offset: 0x0054325C
		public static Pair<string, Color> GetPainLabel(Pawn pawn)
		{
			float painTotal = pawn.health.hediffSet.PainTotal;
			Color second = Color.white;
			string first;
			if (Mathf.Approximately(painTotal, 0f))
			{
				first = "NoPain".Translate();
				second = HealthUtility.GoodConditionColor;
			}
			else if (painTotal < 0.15f)
			{
				first = "LittlePain".Translate();
				second = Color.gray;
			}
			else if (painTotal < 0.4f)
			{
				first = "MediumPain".Translate();
				second = HealthCardUtility.MediumPainColor;
			}
			else if (painTotal < 0.8f)
			{
				first = "SeverePain".Translate();
				second = HealthCardUtility.SeverePainColor;
			}
			else
			{
				first = "ExtremePain".Translate();
				second = HealthUtility.RedColor;
			}
			return new Pair<string, Color>(first, second);
		}

		// Token: 0x06012071 RID: 73841 RVA: 0x00545128 File Offset: 0x00543328
		public static bool ShowBloodLoss(Thing thing)
		{
			Corpse corpse = thing as Corpse;
			return corpse == null || corpse.Age < 60000;
		}

		// Token: 0x0400BAE2 RID: 47842
		private static Vector2 scrollPosition = Vector2.zero;

		// Token: 0x0400BAE3 RID: 47843
		private static float scrollViewHeight = 0f;

		// Token: 0x0400BAE4 RID: 47844
		private static bool highlight = true;

		// Token: 0x0400BAE5 RID: 47845
		private static bool onOperationTab = false;

		// Token: 0x0400BAE6 RID: 47846
		private static Vector2 billsScrollPosition = Vector2.zero;

		// Token: 0x0400BAE7 RID: 47847
		private static float billsScrollHeight = 1000f;

		// Token: 0x0400BAE8 RID: 47848
		private static bool showAllHediffs = false;

		// Token: 0x0400BAE9 RID: 47849
		private static bool showHediffsDebugInfo = false;

		// Token: 0x0400BAEA RID: 47850
		private static float lastMaxIconsTotalWidth;

		// Token: 0x0400BAEB RID: 47851
		private const float TopPadding = 20f;

		// Token: 0x0400BAEC RID: 47852
		private const float ThoughtLevelHeight = 25f;

		// Token: 0x0400BAED RID: 47853
		private const float ThoughtLevelSpacing = 4f;

		// Token: 0x0400BAEE RID: 47854
		private const float IconSize = 20f;

		// Token: 0x0400BAEF RID: 47855
		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x0400BAF0 RID: 47856
		private static readonly Color StaticHighlightColor = new Color(0.75f, 0.75f, 0.85f, 1f);

		// Token: 0x0400BAF1 RID: 47857
		private static readonly Color MediumPainColor = new Color(0.9f, 0.9f, 0f);

		// Token: 0x0400BAF2 RID: 47858
		private static readonly Color SeverePainColor = new Color(0.9f, 0.5f, 0f);

		// Token: 0x0400BAF3 RID: 47859
		private static readonly Texture2D BleedingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/Bleeding", true);

		// Token: 0x0400BAF4 RID: 47860
		private const int HideBloodLossTicksThreshold = 60000;

		// Token: 0x0400BAF5 RID: 47861
		private static readonly Dictionary<EfficiencyEstimate, Color> efficiencyToColor = new Dictionary<EfficiencyEstimate, Color>
		{
			{
				EfficiencyEstimate.None,
				ColorLibrary.RedReadable
			},
			{
				EfficiencyEstimate.VeryPoor,
				new Color(0.75f, 0.45f, 0.45f)
			},
			{
				EfficiencyEstimate.Poor,
				new Color(0.55f, 0.55f, 0.55f)
			},
			{
				EfficiencyEstimate.Weakened,
				new Color(0.7f, 0.7f, 0.7f)
			},
			{
				EfficiencyEstimate.GoodCondition,
				HealthUtility.GoodConditionColor
			},
			{
				EfficiencyEstimate.Enhanced,
				new Color(0.5f, 0.5f, 0.9f)
			}
		};

		// Token: 0x0400BAF6 RID: 47862
		private static List<Hediff> tmpHediffImpactors = new List<Hediff>();

		// Token: 0x0400BAF7 RID: 47863
		private static List<BodyPartRecord> tmpBodyPartImpactors = new List<BodyPartRecord>();

		// Token: 0x0400BAF8 RID: 47864
		private static List<Gene> tmpGeneImpactors = new List<Gene>();
	}
}
