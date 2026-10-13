using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003266 RID: 12902
	[StaticConstructorOnStartup]
	public static class TransferableUIUtility
	{
		// Token: 0x06012261 RID: 74337 RVA: 0x00550524 File Offset: 0x0054E724
		public static void DoCountAdjustInterface(Rect rect, Transferable trad, int index, int min, int max, bool flash = false, List<TransferableCountToTransferStoppingPoint> extraStoppingPoints = null, bool readOnly = false)
		{
			TransferableUIUtility.stoppingPoints.Clear();
			if (extraStoppingPoints != null)
			{
				TransferableUIUtility.stoppingPoints.AddRange(extraStoppingPoints);
			}
			for (int i = TransferableUIUtility.stoppingPoints.Count - 1; i >= 0; i--)
			{
				if (TransferableUIUtility.stoppingPoints[i].threshold != 0 && (TransferableUIUtility.stoppingPoints[i].threshold <= min || TransferableUIUtility.stoppingPoints[i].threshold >= max))
				{
					TransferableUIUtility.stoppingPoints.RemoveAt(i);
				}
			}
			bool flag = false;
			for (int j = 0; j < TransferableUIUtility.stoppingPoints.Count; j++)
			{
				if (TransferableUIUtility.stoppingPoints[j].threshold == 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TransferableUIUtility.stoppingPoints.Add(new TransferableCountToTransferStoppingPoint(0, "0", "0"));
			}
			TransferableUIUtility.DoCountAdjustInterfaceInternal(rect, trad, index, min, max, flash, readOnly);
		}

		// Token: 0x06012262 RID: 74338 RVA: 0x00550600 File Offset: 0x0054E800
		private static void DoCountAdjustInterfaceInternal(Rect rect, Transferable trad, int index, int min, int max, bool flash, bool readOnly)
		{
			rect = rect.Rounded();
			Rect rect2 = new Rect(rect.center.x - 45f, rect.center.y - 12.5f, 90f, 25f).Rounded();
			if (flash)
			{
				GUI.DrawTexture(rect2, TransferableUIUtility.FlashTex);
			}
			TransferableOneWay transferableOneWay = trad as TransferableOneWay;
			bool flag = transferableOneWay != null && transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn && transferableOneWay.MaxCount == 1;
			if (!trad.Interactive || readOnly)
			{
				if (flag)
				{
					bool flag2 = trad.CountToTransfer != 0;
					Widgets.Checkbox(rect2.position, ref flag2, 24f, true, false, null, null);
				}
				else
				{
					GUI.color = ((trad.CountToTransfer == 0) ? TransferableUIUtility.ZeroCountColor : Color.white);
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect2, trad.CountToTransfer.ToStringCached());
				}
			}
			else if (flag)
			{
				bool flag3 = trad.CountToTransfer != 0;
				bool flag4 = flag3;
				Widgets.Checkbox(rect2.position, ref flag4, 24f, false, true, null, null);
				if (flag4 != flag3)
				{
					if (flag4)
					{
						trad.AdjustTo(trad.GetMaximumToTransfer());
					}
					else
					{
						trad.AdjustTo(trad.GetMinimumToTransfer());
					}
				}
			}
			else
			{
				Rect rect3 = rect2.ContractedBy(2f);
				rect3.xMax -= 15f;
				rect3.xMin += 16f;
				int countToTransfer = trad.CountToTransfer;
				string editBuffer = trad.EditBuffer;
				Widgets.TextFieldNumeric<int>(rect3, ref countToTransfer, ref editBuffer, (float)min, (float)max);
				trad.AdjustTo(countToTransfer);
				trad.EditBuffer = editBuffer;
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			if (trad.Interactive && !flag && !readOnly)
			{
				TransferablePositiveCountDirection positiveCountDirection = trad.PositiveCountDirection;
				int num = (positiveCountDirection == TransferablePositiveCountDirection.Source) ? 1 : -1;
				int num2 = GenUI.CurrentAdjustmentMultiplier();
				bool flag5 = trad.GetRange() == 1;
				if (trad.CanAdjustBy(num * num2).Accepted)
				{
					Rect rect4 = new Rect(rect2.x - 30f, rect.y, 30f, rect.height);
					if (flag5)
					{
						rect4.x -= rect4.width;
						rect4.width += rect4.width;
					}
					if (Widgets.ButtonText(rect4, "<", true, true, true, null))
					{
						trad.AdjustBy(num * num2);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					if (!flag5)
					{
						string label = "<<";
						int? num3 = null;
						int num4 = 0;
						for (int i = 0; i < TransferableUIUtility.stoppingPoints.Count; i++)
						{
							TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint = TransferableUIUtility.stoppingPoints[i];
							if (positiveCountDirection == TransferablePositiveCountDirection.Source)
							{
								if (trad.CountToTransfer < transferableCountToTransferStoppingPoint.threshold && (transferableCountToTransferStoppingPoint.threshold < num4 || num3 == null))
								{
									label = transferableCountToTransferStoppingPoint.leftLabel;
									num3 = new int?(transferableCountToTransferStoppingPoint.threshold);
								}
							}
							else if (trad.CountToTransfer > transferableCountToTransferStoppingPoint.threshold && (transferableCountToTransferStoppingPoint.threshold > num4 || num3 == null))
							{
								label = transferableCountToTransferStoppingPoint.leftLabel;
								num3 = new int?(transferableCountToTransferStoppingPoint.threshold);
							}
						}
						rect4.x -= rect4.width;
						if (Widgets.ButtonText(rect4, label, true, true, true, null))
						{
							if (num3 != null)
							{
								trad.AdjustTo(num3.Value);
							}
							else if (num == 1)
							{
								trad.AdjustTo(trad.GetMaximumToTransfer());
							}
							else
							{
								trad.AdjustTo(trad.GetMinimumToTransfer());
							}
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						}
					}
				}
				if (trad.CanAdjustBy(-num * num2).Accepted)
				{
					Rect rect5 = new Rect(rect2.xMax, rect.y, 30f, rect.height);
					if (flag5)
					{
						rect5.width += rect5.width;
					}
					if (Widgets.ButtonText(rect5, ">", true, true, true, null))
					{
						trad.AdjustBy(-num * num2);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
					if (!flag5)
					{
						string label2 = ">>";
						int? num5 = null;
						int num6 = 0;
						for (int j = 0; j < TransferableUIUtility.stoppingPoints.Count; j++)
						{
							TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint2 = TransferableUIUtility.stoppingPoints[j];
							if (positiveCountDirection == TransferablePositiveCountDirection.Destination)
							{
								if (trad.CountToTransfer < transferableCountToTransferStoppingPoint2.threshold && (transferableCountToTransferStoppingPoint2.threshold < num6 || num5 == null))
								{
									label2 = transferableCountToTransferStoppingPoint2.rightLabel;
									num5 = new int?(transferableCountToTransferStoppingPoint2.threshold);
								}
							}
							else if (trad.CountToTransfer > transferableCountToTransferStoppingPoint2.threshold && (transferableCountToTransferStoppingPoint2.threshold > num6 || num5 == null))
							{
								label2 = transferableCountToTransferStoppingPoint2.rightLabel;
								num5 = new int?(transferableCountToTransferStoppingPoint2.threshold);
							}
						}
						rect5.x += rect5.width;
						if (Widgets.ButtonText(rect5, label2, true, true, true, null))
						{
							if (num5 != null)
							{
								trad.AdjustTo(num5.Value);
							}
							else if (num == 1)
							{
								trad.AdjustTo(trad.GetMinimumToTransfer());
							}
							else
							{
								trad.AdjustTo(trad.GetMaximumToTransfer());
							}
							SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						}
					}
				}
			}
			if (trad.CountToTransfer != 0)
			{
				Rect position = new Rect(rect2.x + rect2.width / 2f - (float)(TransferableUIUtility.TradeArrow.width / 2), rect2.y + rect2.height / 2f - (float)(TransferableUIUtility.TradeArrow.height / 2), (float)TransferableUIUtility.TradeArrow.width, (float)TransferableUIUtility.TradeArrow.height);
				TransferablePositiveCountDirection positiveCountDirection2 = trad.PositiveCountDirection;
				if ((positiveCountDirection2 == TransferablePositiveCountDirection.Source && trad.CountToTransfer > 0) || (positiveCountDirection2 == TransferablePositiveCountDirection.Destination && trad.CountToTransfer < 0))
				{
					position.x += position.width;
					position.width *= -1f;
				}
				GUI.DrawTexture(position, TransferableUIUtility.TradeArrow);
			}
		}

		// Token: 0x06012263 RID: 74339 RVA: 0x00550C40 File Offset: 0x0054EE40
		public static void DrawTransferableInfo(Transferable trad, Rect idRect, Color labelColor)
		{
			if (!trad.HasAnyThing && trad.IsThing)
			{
				return;
			}
			if (Mouse.IsOver(idRect))
			{
				Widgets.DrawHighlight(idRect);
			}
			Rect rect = new Rect(0f, 0f, 27f, 27f);
			if (trad.IsThing)
			{
				try
				{
					Widgets.ThingIcon(rect, trad.AnyThing, 1f, null, false, 1f, false);
					goto IL_99;
				}
				catch (Exception ex)
				{
					Log.Error("Exception drawing thing icon for " + trad.AnyThing.def.defName + ": " + ex.ToString());
					goto IL_99;
				}
			}
			trad.DrawIcon(rect);
			IL_99:
			if (trad.IsThing)
			{
				Widgets.InfoCardButton(40f, 0f, trad.AnyThing);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = new Rect(80f, 0f, idRect.width - 80f, idRect.height);
			Text.WordWrap = false;
			GUI.color = labelColor;
			Widgets.Label(rect2, trad.LabelCap);
			GUI.color = Color.white;
			Text.WordWrap = true;
			if (Mouse.IsOver(idRect))
			{
				Transferable localTrad = trad;
				TooltipHandler.TipRegion(idRect, new TipSignal(delegate()
				{
					if (!localTrad.HasAnyThing && localTrad.IsThing)
					{
						return "";
					}
					string text = localTrad.LabelCap;
					string tipDescription = localTrad.TipDescription;
					if (localTrad.AnyThing is Book)
					{
						text = tipDescription;
					}
					else if (!tipDescription.NullOrEmpty())
					{
						text = text + ": " + tipDescription + TransferableUIUtility.ContentSourceDescription(localTrad.AnyThing);
					}
					CompIngredients compIngredients = localTrad.AnyThing.TryGetComp<CompIngredients>();
					if (compIngredients != null)
					{
						text = text + "\n\n" + compIngredients.CompInspectStringExtra();
					}
					return text;
				}, localTrad.GetHashCode()));
			}
		}

		// Token: 0x06012264 RID: 74340 RVA: 0x00550D9C File Offset: 0x0054EF9C
		public static float DefaultListOrderPriority(Transferable transferable)
		{
			if (!transferable.HasAnyThing)
			{
				return 0f;
			}
			return TransferableUIUtility.DefaultListOrderPriority(transferable.ThingDef);
		}

		// Token: 0x06012265 RID: 74341 RVA: 0x00550DB8 File Offset: 0x0054EFB8
		public static float DefaultArchonexusItemListOrderPriority(ThingDef def)
		{
			if (def == ThingDefOf.MealSurvivalPack)
			{
				return 100.2f;
			}
			if (def == ThingDefOf.Pemmican)
			{
				return 100.1f;
			}
			if (def == ThingDefOf.Luciferium)
			{
				return 90.1f;
			}
			if (def.IsNonMedicalDrug)
			{
				return 90f;
			}
			if (def.IsMedicine)
			{
				return 80f;
			}
			if (MoveColonyUtility.IsDistinctArchonexusItem(def))
			{
				return 75f;
			}
			if (def == ThingDefOf.Silver)
			{
				return 50.2f;
			}
			if (def == ThingDefOf.Gold)
			{
				return 50.1f;
			}
			if (def.thingCategories.Contains(ThingCategoryDefOf.ResourcesRaw))
			{
				return 70f;
			}
			if (def.thingCategories.Contains(ThingCategoryDefOf.Manufactured) || def.thingCategories.Contains(ThingCategoryDefOf.Drugs))
			{
				return 60f;
			}
			if (def.thingCategories.Contains(ThingCategoryDefOf.PlantMatter))
			{
				return -10f;
			}
			if (def.IsEgg || def.IsAnimalProduct)
			{
				return -20f;
			}
			if (def.thingCategories.Contains(ThingCategoryDefOf.Foods) || def.thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
			{
				return -30f;
			}
			if (def.IsLeather || def.IsWool || def.thingCategories.Contains(ThingCategoryDefOf.Textiles))
			{
				return -40f;
			}
			if (def.thingCategories.Contains(ThingCategoryDefOf.StoneBlocks))
			{
				return -50f;
			}
			return 0f;
		}

		// Token: 0x06012266 RID: 74342 RVA: 0x00550F14 File Offset: 0x0054F114
		public static float DefaultListOrderPriority(ThingDef def)
		{
			if (def == ThingDefOf.Silver)
			{
				return 100f;
			}
			if (def == ThingDefOf.Gold)
			{
				return 99f;
			}
			if (def.Minifiable)
			{
				return 90f;
			}
			if (def.IsApparel)
			{
				return 80f;
			}
			if (def.IsRangedWeapon)
			{
				return 70f;
			}
			if (def.IsMeleeWeapon)
			{
				return 60f;
			}
			if (def.isTechHediff)
			{
				return 50f;
			}
			if (def.CountAsResource)
			{
				return -10f;
			}
			return 20f;
		}

		// Token: 0x06012267 RID: 74343 RVA: 0x00550F98 File Offset: 0x0054F198
		public static void DoTransferableSorters(TransferableSorterDef sorter1, TransferableSorterDef sorter2, Action<TransferableSorterDef> sorter1Setter, Action<TransferableSorterDef> sorter2Setter)
		{
			Widgets.BeginGroup(TransferableUIUtility.SortersRect);
			Text.Font = GameFont.Tiny;
			Rect rect = new Rect(0f, 0f, 60f, 27f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, "SortBy".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(rect.xMax + 10f, 0f, 130f, 27f);
			if (Widgets.ButtonText(rect2, sorter1.LabelCap.Truncate(rect2.width - 2f, null), true, true, true, null))
			{
				TransferableUIUtility.OpenSorterChangeFloatMenu(sorter1Setter);
			}
			Rect rect3 = new Rect(rect2.xMax + 10f, 0f, 130f, 27f);
			if (Widgets.ButtonText(rect3, sorter2.LabelCap.Truncate(rect3.width - 2f, null), true, true, true, null))
			{
				TransferableUIUtility.OpenSorterChangeFloatMenu(sorter2Setter);
			}
			Widgets.EndGroup();
		}

		// Token: 0x06012268 RID: 74344 RVA: 0x005510A8 File Offset: 0x0054F2A8
		public static void DoTransferableSearcher(QuickSearchWidget searchWidget, Action onSearchChange)
		{
			Rect searcherRect = TransferableUIUtility.SearcherRect;
			Widgets.BeginGroup(searcherRect);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, (searcherRect.height - 24f) / 2f, 170f, 24f);
			searchWidget.OnGUI(rect, onSearchChange, null);
			Text.Font = GameFont.Tiny;
			Widgets.EndGroup();
		}

		// Token: 0x06012269 RID: 74345 RVA: 0x00551104 File Offset: 0x0054F304
		public static void DoExtraIcons(Transferable trad, Rect rect, ref float curX)
		{
			Pawn pawn = trad.AnyThing as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.RaceProps.Animal)
			{
				if (pawn.IsCaravanRideable())
				{
					Rect rect2 = new Rect(curX - TransferableUIUtility.RideableIconWidth, (rect.height - TransferableUIUtility.RideableIconWidth) / 2f, TransferableUIUtility.RideableIconWidth, TransferableUIUtility.RideableIconWidth);
					curX -= rect2.width;
					GUI.DrawTexture(rect2, TransferableUIUtility.RideableIcon);
					if (Mouse.IsOver(rect2))
					{
						TooltipHandler.TipRegion(rect2, CaravanRideableUtility.GetIconTooltipText(pawn));
					}
				}
				if (pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null) != null)
				{
					TransferableUIUtility.DrawBondedIcon(pawn, new Rect(curX - TransferableUIUtility.BondIconWidth, (rect.height - TransferableUIUtility.BondIconWidth) / 2f, TransferableUIUtility.BondIconWidth, TransferableUIUtility.BondIconWidth));
					curX -= TransferableUIUtility.BondIconWidth;
				}
				if (pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true))
				{
					TransferableUIUtility.DrawPregnancyIcon(pawn, new Rect(curX - TransferableUIUtility.PregnancyIconWidth, (rect.height - TransferableUIUtility.PregnancyIconWidth) / 2f, TransferableUIUtility.PregnancyIconWidth, TransferableUIUtility.PregnancyIconWidth));
					curX -= TransferableUIUtility.PregnancyIconWidth;
				}
				if (pawn.health.hediffSet.AnyHediffMakesSickThought)
				{
					TransferableUIUtility.DrawSickIcon(pawn, new Rect(curX - TransferableUIUtility.SickIconWidth, (rect.height - TransferableUIUtility.SickIconWidth) / 2f, TransferableUIUtility.SickIconWidth, TransferableUIUtility.SickIconWidth));
					curX -= TransferableUIUtility.SickIconWidth;
					return;
				}
			}
			else if (ModsConfig.BiotechActive && pawn.IsColonyMech)
			{
				Pawn overseer = pawn.GetOverseer();
				if (overseer != null)
				{
					TransferableUIUtility.DrawOverseerIcon(pawn, overseer, new Rect(curX - TransferableUIUtility.OverseerIconWidth, (rect.height - TransferableUIUtility.OverseerIconWidth) / 2f, TransferableUIUtility.OverseerIconWidth, TransferableUIUtility.OverseerIconWidth));
					curX -= TransferableUIUtility.OverseerIconWidth;
					return;
				}
			}
			else if (CaravanBonusUtility.HasCaravanBonus(pawn))
			{
				Rect rect3 = new Rect(curX - TransferableUIUtility.RideableIconWidth, (rect.height - TransferableUIUtility.RideableIconWidth) / 2f, TransferableUIUtility.RideableIconWidth, TransferableUIUtility.RideableIconWidth);
				curX -= rect3.width;
				GUI.DrawTexture(rect3, TransferableUIUtility.BonusIcon);
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, CaravanBonusUtility.GetIconTooltipText(pawn));
				}
			}
		}

		// Token: 0x0601226A RID: 74346 RVA: 0x00551340 File Offset: 0x0054F540
		public static void DrawBondedIcon(Pawn bondedPawn, Rect rect)
		{
			GUI.DrawTexture(rect, TransferableUIUtility.BondIcon);
			if (Mouse.IsOver(rect))
			{
				string iconTooltipText = TrainableUtility.GetIconTooltipText(bondedPawn);
				if (!iconTooltipText.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect, iconTooltipText);
				}
			}
		}

		// Token: 0x0601226B RID: 74347 RVA: 0x0055137B File Offset: 0x0054F57B
		public static void DrawPregnancyIcon(Pawn pregnantPawn, Rect rect)
		{
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, PawnColumnWorker_Pregnant.GetTooltipText(pregnantPawn));
			}
			GUI.DrawTexture(rect, TransferableUIUtility.PregnantIcon);
		}

		// Token: 0x0601226C RID: 74348 RVA: 0x005513A4 File Offset: 0x0054F5A4
		private static void DrawOverseerIcon(Pawn mech, Pawn overseer, Rect rect)
		{
			GUI.DrawTexture(rect, PortraitsCache.Get(overseer, new Vector2(TransferableUIUtility.OverseerIconWidth, TransferableUIUtility.OverseerIconWidth), Rot4.South, default(Vector3), 1f, true, true, true, true, null, null, false, null));
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, "MechOverseer".Translate(overseer));
			}
		}

		// Token: 0x0601226D RID: 74349 RVA: 0x00551420 File Offset: 0x0054F620
		private static void DrawSickIcon(Pawn pawn, Rect rect)
		{
			if (Mouse.IsOver(rect))
			{
				IEnumerable<string> entries = from h in pawn.health.hediffSet.hediffs
				where h.def.makesSickThought
				select h.LabelCap;
				TooltipHandler.TipRegion(rect, "CaravanAnimalSick".Translate() + ":\n\n" + entries.ToLineList(" - ", false));
			}
			GUI.DrawTexture(rect, TransferableUIUtility.SickIcon);
		}

		// Token: 0x0601226E RID: 74350 RVA: 0x005514CC File Offset: 0x0054F6CC
		private static void OpenSorterChangeFloatMenu(Action<TransferableSorterDef> sorterSetter)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<TransferableSorterDef> allDefsListForReading = DefDatabase<TransferableSorterDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				TransferableSorterDef def = allDefsListForReading[i];
				list.Add(new FloatMenuOption(def.LabelCap, delegate()
				{
					sorterSetter(def);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0601226F RID: 74351 RVA: 0x00551568 File Offset: 0x0054F768
		public static void DrawExtraInfo(List<TransferableUIUtility.ExtraInfo> info, Rect rect)
		{
			if (rect.width > (float)info.Count * 230f)
			{
				rect.x += Mathf.Floor((rect.width - (float)info.Count * 230f) / 2f);
				rect.width = (float)info.Count * 230f;
			}
			Widgets.BeginGroup(rect);
			float num = Mathf.Floor(rect.width / (float)info.Count);
			float num2 = 0f;
			for (int i = 0; i < info.Count; i++)
			{
				float num3 = (i == info.Count - 1) ? (rect.width - num2) : num;
				Rect rect2 = new Rect(num2, 0f, num3, rect.height);
				Rect rect3 = new Rect(num2, 0f, num3, rect.height / 2f);
				Rect rect4 = new Rect(num2, rect.height / 2f, num3, rect.height / 2f);
				if (Time.time - info[i].lastFlashTime < 1f)
				{
					GUI.DrawTexture(rect2, TransferableUIUtility.FlashTex);
				}
				Text.Anchor = TextAnchor.LowerCenter;
				Text.Font = GameFont.Tiny;
				GUI.color = Color.gray;
				Widgets.Label(new Rect(rect3.x, rect3.y - 2f, rect3.width, rect3.height - -3f), info[i].key);
				Rect rect5 = new Rect(rect4.x, rect4.y + -3f + 2f, rect4.width, rect4.height - -3f);
				Text.Font = GameFont.Small;
				if (info[i].secondValue.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperCenter;
					GUI.color = info[i].color;
					Widgets.Label(rect5, info[i].value);
				}
				else
				{
					Rect rect6 = rect5;
					rect6.width = Mathf.Floor(rect5.width / 2f - 15f);
					Text.Anchor = TextAnchor.UpperRight;
					GUI.color = info[i].color;
					Widgets.Label(rect6, info[i].value);
					Rect rect7 = rect5;
					rect7.xMin += Mathf.Ceil(rect5.width / 2f + 15f);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = info[i].secondColor;
					Widgets.Label(rect7, info[i].secondValue);
					Rect position = rect5;
					position.x = Mathf.Floor(rect5.x + rect5.width / 2f - 7.5f);
					position.y += 3f;
					position.width = 15f;
					position.height = 15f;
					GUI.color = Color.white;
					GUI.DrawTexture(position, TransferableUIUtility.DividerTex);
				}
				GUI.color = Color.white;
				Widgets.DrawHighlightIfMouseover(rect2);
				TooltipHandler.TipRegion(rect2, info[i].tip);
				num2 += num3;
			}
			Widgets.EndGroup();
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06012270 RID: 74352 RVA: 0x005518B0 File Offset: 0x0054FAB0
		public static void DrawCaptiveTradeInfo(Transferable trad, ITrader trader, Rect rect, ref float curX)
		{
			Pawn pawn = trad.AnyThing as Pawn;
			if (pawn == null || pawn.guest == null || !pawn.RaceProps.Humanlike)
			{
				return;
			}
			if (TransferableUIUtility.TransferableIsCaptive(trad) && (pawn.IsSlaveOfColony || pawn.IsPrisonerOfColony))
			{
				if (pawn.HomeFaction == trader.Faction)
				{
					Rect rect2 = new Rect(curX - TransferableUIUtility.SlaveTradeIconWidth, (rect.height - TransferableUIUtility.SlaveTradeIconWidth) / 2f, TransferableUIUtility.SlaveTradeIconWidth, TransferableUIUtility.SlaveTradeIconWidth);
					curX -= TransferableUIUtility.SlaveTradeIconWidth;
					GUI.DrawTexture(rect2, GuestUtility.RansomIcon);
					if (Mouse.IsOver(rect2))
					{
						TooltipHandler.TipRegion(rect2, "SellingAsRansom".Translate());
						return;
					}
				}
				else
				{
					Rect rect3 = new Rect(curX - TransferableUIUtility.SlaveTradeIconWidth, (rect.height - TransferableUIUtility.SlaveTradeIconWidth) / 2f, TransferableUIUtility.SlaveTradeIconWidth, TransferableUIUtility.SlaveTradeIconWidth);
					curX -= TransferableUIUtility.SlaveTradeIconWidth;
					GUI.DrawTexture(rect3, GuestUtility.SlaveIcon);
					if (Mouse.IsOver(rect3))
					{
						TooltipHandler.TipRegion(rect3, "SellingAsSlave".Translate());
						return;
					}
				}
			}
			else
			{
				float width = 140f;
				string label = ((pawn.guest.joinStatus == JoinStatus.JoinAsColonist) ? "JoinsAsColonist" : "JoinsAsSlave").Translate();
				Rect rect4 = new Rect(curX, 0f, width, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect4, label);
				Text.Anchor = TextAnchor.UpperLeft;
				if (Mouse.IsOver(rect4))
				{
					Widgets.DrawHighlight(rect4);
					string key = (pawn.guest.joinStatus == JoinStatus.JoinAsColonist) ? "JoinsAsColonistDesc" : "JoinsAsSlaveDesc";
					TooltipHandler.TipRegion(rect4, key.Translate());
				}
			}
		}

		// Token: 0x06012271 RID: 74353 RVA: 0x00551A68 File Offset: 0x0054FC68
		public static bool TransferableIsCaptive(Transferable trad)
		{
			Pawn pawn = trad.AnyThing as Pawn;
			return pawn != null && pawn.RaceProps.Humanlike && (pawn.IsSlave || pawn.IsPrisoner);
		}

		// Token: 0x06012272 RID: 74354 RVA: 0x00551AA3 File Offset: 0x0054FCA3
		public static bool TradeIsPlayerSellingToSlavery(Tradeable trad, Faction traderFaction)
		{
			return TransferableUIUtility.TransferableIsCaptive(trad) && trad.CountHeldBy(Transactor.Colony) > 0 && ((Pawn)trad.AnyThing).HomeFaction != traderFaction;
		}

		// Token: 0x06012273 RID: 74355 RVA: 0x00551AD0 File Offset: 0x0054FCD0
		public static string ContentSourceDescription(Thing thing)
		{
			if (((thing != null) ? thing.ContentSource : null) == null || thing.ContentSource.IsCoreMod)
			{
				return "";
			}
			return "\n\n" + ("Stat_Source_Label".Translate() + ": " + thing.ContentSource.Name).Resolve().Colorize(ColoredText.SubtleGrayColor);
		}

		// Token: 0x0400BC92 RID: 48274
		private static List<TransferableCountToTransferStoppingPoint> stoppingPoints = new List<TransferableCountToTransferStoppingPoint>();

		// Token: 0x0400BC93 RID: 48275
		private const float AmountAreaWidth = 90f;

		// Token: 0x0400BC94 RID: 48276
		private const float AmountAreaHeight = 25f;

		// Token: 0x0400BC95 RID: 48277
		private const float AdjustArrowWidth = 30f;

		// Token: 0x0400BC96 RID: 48278
		public const float ResourceIconSize = 27f;

		// Token: 0x0400BC97 RID: 48279
		public const float SortersHeight = 27f;

		// Token: 0x0400BC98 RID: 48280
		public const float ExtraInfoHeight = 40f;

		// Token: 0x0400BC99 RID: 48281
		public const float ExtraInfoMargin = 12f;

		// Token: 0x0400BC9A RID: 48282
		public static readonly Color ZeroCountColor = new Color(0.5f, 0.5f, 0.5f);

		// Token: 0x0400BC9B RID: 48283
		public static readonly Texture2D FlashTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0f, 0f, 0.4f));

		// Token: 0x0400BC9C RID: 48284
		private static readonly Texture2D TradeArrow = ContentFinder<Texture2D>.Get("UI/Widgets/TradeArrow", true);

		// Token: 0x0400BC9D RID: 48285
		private static readonly Texture2D DividerTex = ContentFinder<Texture2D>.Get("UI/Widgets/Divider", true);

		// Token: 0x0400BC9E RID: 48286
		private static readonly Texture2D PregnantIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);

		// Token: 0x0400BC9F RID: 48287
		private static readonly Texture2D BondIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Bond", true);

		// Token: 0x0400BCA0 RID: 48288
		private static readonly Texture2D RideableIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Rideable", true);

		// Token: 0x0400BCA1 RID: 48289
		private static readonly Texture2D BonusIcon = ContentFinder<Texture2D>.Get("UI/Icons/MoveSpeedBonus", true);

		// Token: 0x0400BCA2 RID: 48290
		private static readonly Texture2D SickIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Sick", true);

		// Token: 0x0400BCA3 RID: 48291
		private static readonly Rect SortersRect = new Rect(0f, 0f, 350f, 27f);

		// Token: 0x0400BCA4 RID: 48292
		private static readonly Rect SearcherRect = new Rect(360f, 0f, 170f, 27f);

		// Token: 0x0400BCA5 RID: 48293
		[TweakValue("Interface", 0f, 50f)]
		private static float PregnancyIconWidth = 24f;

		// Token: 0x0400BCA6 RID: 48294
		[TweakValue("Interface", 0f, 50f)]
		private static float BondIconWidth = 24f;

		// Token: 0x0400BCA7 RID: 48295
		[TweakValue("Interface", 0f, 50f)]
		private static float RideableIconWidth = 24f;

		// Token: 0x0400BCA8 RID: 48296
		[TweakValue("Interface", 0f, 50f)]
		private static float SlaveTradeIconWidth = 24f;

		// Token: 0x0400BCA9 RID: 48297
		[TweakValue("Interface", 0f, 50f)]
		private static float OverseerIconWidth = 36f;

		// Token: 0x0400BCAA RID: 48298
		[TweakValue("Interface", 0f, 50f)]
		private static float SickIconWidth = 24f;

		// Token: 0x02003267 RID: 12903
		public struct ExtraInfo
		{
			// Token: 0x06012275 RID: 74357 RVA: 0x00551C7B File Offset: 0x0054FE7B
			public ExtraInfo(string key, string value, Color color, string tip, float lastFlashTime = -9999f)
			{
				this.key = key;
				this.value = value;
				this.color = color;
				this.tip = tip;
				this.lastFlashTime = lastFlashTime;
				this.secondValue = null;
				this.secondColor = default(Color);
			}

			// Token: 0x06012276 RID: 74358 RVA: 0x00551CB5 File Offset: 0x0054FEB5
			public ExtraInfo(string key, string value, Color color, string tip, string secondValue, Color secondColor, float lastFlashTime = -9999f)
			{
				this.key = key;
				this.value = value;
				this.color = color;
				this.tip = tip;
				this.lastFlashTime = lastFlashTime;
				this.secondValue = secondValue;
				this.secondColor = secondColor;
			}

			// Token: 0x0400BCAB RID: 48299
			public string key;

			// Token: 0x0400BCAC RID: 48300
			public string value;

			// Token: 0x0400BCAD RID: 48301
			public string secondValue;

			// Token: 0x0400BCAE RID: 48302
			public string tip;

			// Token: 0x0400BCAF RID: 48303
			public float lastFlashTime;

			// Token: 0x0400BCB0 RID: 48304
			public Color color;

			// Token: 0x0400BCB1 RID: 48305
			public Color secondColor;
		}
	}
}
