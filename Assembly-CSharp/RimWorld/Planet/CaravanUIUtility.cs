using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D74 RID: 15732
	public static class CaravanUIUtility
	{
		// Token: 0x060156F0 RID: 87792 RVA: 0x0066A474 File Offset: 0x00668674
		public static void CreateCaravanTransferableWidgets(List<TransferableOneWay> transferables, out TransferableOneWayWidget pawnsTransfer, out TransferableOneWayWidget itemsTransfer, out TransferableOneWayWidget travelSuppliesTransfer, string thingCountTip, IgnorePawnsInventoryMode ignorePawnInventoryMass, Func<float> availableMassGetter, bool ignoreSpawnedCorpsesGearAndInventoryMass, PlanetTile tile, bool playerPawnsReadOnly = false)
		{
			pawnsTransfer = new TransferableOneWayWidget(null, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, new PlanetTile?(tile), true, true, true, true, false, true, false, playerPawnsReadOnly, false, false);
			CaravanUIUtility.AddPawnsSections(pawnsTransfer, transferables);
			itemsTransfer = new TransferableOneWayWidget(from x in transferables
			where CaravanUIUtility.GetTransferableCategory(x) == CaravanUIUtility.TransferableCategory.Item
			select x, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, new PlanetTile?(tile), true, false, false, false, true, false, true, false, false, false);
			travelSuppliesTransfer = new TransferableOneWayWidget(from x in transferables
			where CaravanUIUtility.GetTransferableCategory(x) == CaravanUIUtility.TransferableCategory.TravelSupplies
			select x, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, new PlanetTile?(tile), true, false, false, false, true, false, true, false, false, false);
		}

		// Token: 0x060156F1 RID: 87793 RVA: 0x0066A550 File Offset: 0x00668750
		private static CaravanUIUtility.TransferableCategory GetTransferableCategory(Transferable t)
		{
			if (t.ThingDef.category == ThingCategory.Pawn)
			{
				return CaravanUIUtility.TransferableCategory.Pawn;
			}
			if ((!t.ThingDef.thingCategories.NullOrEmpty<ThingCategoryDef>() && t.ThingDef.thingCategories.Contains(ThingCategoryDefOf.Medicine)) || (t.ThingDef.IsIngestible && !t.ThingDef.IsDrug && !t.ThingDef.IsCorpse && (t.ThingDef.plant == null || !t.ThingDef.plant.IsTree)) || (t.AnyThing.GetInnerIfMinified().def.IsBed && t.AnyThing.GetInnerIfMinified().def.building.bed_caravansCanUse))
			{
				return CaravanUIUtility.TransferableCategory.TravelSupplies;
			}
			return CaravanUIUtility.TransferableCategory.Item;
		}

		// Token: 0x060156F2 RID: 87794 RVA: 0x0066A614 File Offset: 0x00668814
		public static void AddPawnsSections(TransferableOneWayWidget widget, List<TransferableOneWay> transferables)
		{
			IEnumerable<TransferableOneWay> source = from x in transferables
			where x.ThingDef.category == ThingCategory.Pawn
			select x;
			widget.AddSection("ColonistsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsFreeNonSlaveColonist
			select x);
			if (ModsConfig.IdeologyActive)
			{
				widget.AddSection("SlavesSection".Translate(), from x in source
				where ((Pawn)x.AnyThing).IsSlave
				select x);
			}
			widget.AddSection("PrisonersSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsPrisoner
			select x);
			widget.AddSection("CaptureSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).Downed && CaravanUtility.ShouldAutoCapture((Pawn)x.AnyThing, Faction.OfPlayer)
			select x);
			widget.AddSection("AnimalsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsAnimal
			select x);
			if (ModsConfig.BiotechActive)
			{
				widget.AddSection("MechsSection".Translate(), from x in source
				where ((Pawn)x.AnyThing).IsColonyMech && ((Pawn)x.AnyThing).OverseerSubject != null && ((Pawn)x.AnyThing).OverseerSubject.State == OverseerSubjectState.Overseen
				select x);
			}
			if (ModsConfig.AnomalyActive)
			{
				widget.AddSection("EntitiesSection".Translate(), from x in source
				where ((Pawn)x.AnyThing).IsColonySubhuman && ((Pawn)x.AnyThing).mutant.Def.canTravelInCaravan
				select x);
			}
		}

		// Token: 0x060156F3 RID: 87795 RVA: 0x0066A7F4 File Offset: 0x006689F4
		private static string GetDaysWorthOfFoodLabel([TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})] ValueTuple<float, float> daysWorthOfFood, bool multiline)
		{
			if (daysWorthOfFood.Item1 >= 600f)
			{
				return "InfiniteDaysWorthOfFoodInfo".Translate();
			}
			string text = daysWorthOfFood.Item1.ToString("0.#");
			string str = multiline ? "\n" : " ";
			if (daysWorthOfFood.Item2 < 600f && daysWorthOfFood.Item2 < daysWorthOfFood.Item1)
			{
				text += str + "(" + "DaysWorthOfFoodInfoRot".Translate(string.Format("{0:0.#})", daysWorthOfFood.Item2));
			}
			return text;
		}

		// Token: 0x060156F4 RID: 87796 RVA: 0x0066A89C File Offset: 0x00668A9C
		private static Color GetDaysWorthOfFoodColor([TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})] ValueTuple<float, float> daysWorthOfFood, int? ticksToArrive)
		{
			if (daysWorthOfFood.Item1 >= 600f)
			{
				return Color.white;
			}
			float num = Mathf.Min(daysWorthOfFood.Item1, daysWorthOfFood.Item2);
			if (ticksToArrive != null)
			{
				return GenUI.LerpColor(CaravanUIUtility.DaysWorthOfFoodKnownRouteColor, num / ((float)ticksToArrive.Value / 60000f));
			}
			return GenUI.LerpColor(CaravanUIUtility.DaysWorthOfFoodColor, num);
		}

		// Token: 0x060156F5 RID: 87797 RVA: 0x0066A900 File Offset: 0x00668B00
		private static string GetMovementSpeedLabel(float tilesPerDay, bool immobile, bool shortVersion)
		{
			if (immobile)
			{
				return shortVersion ? "TilesPerDayImmobileShort".Translate() : "TilesPerDayImmobile".Translate();
			}
			return string.Format("{0:0.#} ", tilesPerDay) + "TilesPerDay".Translate();
		}

		// Token: 0x060156F6 RID: 87798 RVA: 0x0066A953 File Offset: 0x00668B53
		private static Color GetMovementSpeedColor(float tilesPerDay, bool immobile)
		{
			if (!immobile)
			{
				return GenUI.LerpColor(CaravanUIUtility.TilesPerDayColor, tilesPerDay);
			}
			return Color.red;
		}

		// Token: 0x060156F7 RID: 87799 RVA: 0x0066A96C File Offset: 0x00668B6C
		public static void DrawCaravanInfo(CaravanUIUtility.CaravanInfo info, CaravanUIUtility.CaravanInfo? info2, PlanetTile currentTile, int? ticksToArrive, float lastMassFlashTime, Rect rect, bool lerpMassColor = true, string extraDaysWorthOfFoodTipInfo = null, bool multiline = false, bool isCaravan = true)
		{
			CaravanUIUtility.tmpInfo.Clear();
			isCaravan = (isCaravan && currentTile.Valid && currentTile.LayerDef.SurfaceTiles);
			TaggedString taggedString = string.Format("{0} / {1:F0} ", info.massUsage.ToStringEnsureThreshold(info.massCapacity, 0), info.massCapacity) + "kg".Translate();
			TaggedString taggedString2 = (info2 != null) ? (string.Format("{0} / {1:F0} ", info2.Value.massUsage.ToStringEnsureThreshold(info2.Value.massCapacity, 0), info2.Value.massCapacity) + "kg".Translate()) : null;
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("Mass".Translate(), taggedString, CaravanUIUtility.GetMassColor(info.massUsage, info.massCapacity, lerpMassColor), CaravanUIUtility.GetMassTip(info.massUsage, info.massCapacity, info.massCapacityExplanation, (info2 != null) ? new float?(info2.GetValueOrDefault().massUsage) : null, (info2 != null) ? new float?(info2.GetValueOrDefault().massCapacity) : null, (info2 != null) ? info2.GetValueOrDefault().massCapacityExplanation : null, isCaravan), taggedString2, (info2 != null) ? CaravanUIUtility.GetMassColor(info2.Value.massUsage, info2.Value.massCapacity, lerpMassColor) : Color.white, lastMassFlashTime));
			if (!Mathf.Approximately(info.extraMassUsage, -1f) && isCaravan)
			{
				TaggedString taggedString3 = string.Format("{0} / {1:F0} ", info.extraMassUsage.ToStringEnsureThreshold(info.extraMassCapacity, 0), info.extraMassCapacity) + "kg".Translate();
				TaggedString taggedString4 = (info2 != null) ? (string.Format("{0} / {1:F0} ", info2.Value.extraMassUsage.ToStringEnsureThreshold(info2.Value.extraMassCapacity, 0), info2.Value.extraMassCapacity) + "kg".Translate()) : null;
				CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("CaravanMass".Translate(), taggedString3, CaravanUIUtility.GetMassColor(info.extraMassUsage, info.extraMassCapacity, true), CaravanUIUtility.GetMassTip(info.extraMassUsage, info.extraMassCapacity, info.extraMassCapacityExplanation, (info2 != null) ? new float?(info2.GetValueOrDefault().extraMassUsage) : null, (info2 != null) ? new float?(info2.GetValueOrDefault().extraMassCapacity) : null, (info2 != null) ? info2.GetValueOrDefault().extraMassCapacityExplanation : null, isCaravan), taggedString4, (info2 != null) ? CaravanUIUtility.GetMassColor(info2.Value.extraMassUsage, info2.Value.extraMassCapacity, true) : Color.white, -9999f));
			}
			if (isCaravan)
			{
				string text = "CaravanMovementSpeedTip".Translate();
				if (!info.tilesPerDayExplanation.NullOrEmpty())
				{
					text = text + "\n\n" + info.tilesPerDayExplanation;
				}
				if (info2 != null && !info2.Value.tilesPerDayExplanation.NullOrEmpty())
				{
					text = text + "\n\n-----\n\n" + info2.Value.tilesPerDayExplanation;
				}
				CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("CaravanMovementSpeed".Translate(), CaravanUIUtility.GetMovementSpeedLabel(info.tilesPerDay, info.massUsage > info.massCapacity, info2 != null), CaravanUIUtility.GetMovementSpeedColor(info.tilesPerDay, info.massUsage > info.massCapacity), text, (info2 != null) ? CaravanUIUtility.GetMovementSpeedLabel(info2.Value.tilesPerDay, info2.Value.massUsage > info2.Value.massCapacity, true) : null, (info2 != null) ? CaravanUIUtility.GetMovementSpeedColor(info2.Value.tilesPerDay, info2.Value.massUsage > info2.Value.massCapacity) : Color.white, -9999f));
			}
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("DaysWorthOfFood".Translate(), CaravanUIUtility.GetDaysWorthOfFoodLabel(info.daysWorthOfFood, multiline), CaravanUIUtility.GetDaysWorthOfFoodColor(info.daysWorthOfFood, ticksToArrive), "DaysWorthOfFoodTooltip".Translate() + extraDaysWorthOfFoodTipInfo + "\n\n" + VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(currentTile, Find.TickManager.TicksAbs), (info2 != null) ? CaravanUIUtility.GetDaysWorthOfFoodLabel(info2.Value.daysWorthOfFood, multiline) : null, (info2 != null) ? CaravanUIUtility.GetDaysWorthOfFoodColor(info2.Value.daysWorthOfFood, ticksToArrive) : Color.white, -9999f));
			string text2 = info.foragedFoodPerDay.Item2.ToString("0.#");
			string text3 = (info2 != null) ? info2.GetValueOrDefault().foragedFoodPerDay.Item2.ToString("0.#") : null;
			TaggedString taggedString5 = "ForagedFoodPerDayTip".Translate();
			taggedString5 += "\n\n" + info.foragedFoodPerDayExplanation;
			if (info2 != null)
			{
				taggedString5 += "\n\n-----\n\n" + info2.Value.foragedFoodPerDayExplanation;
			}
			if (info.foragedFoodPerDay.Item2 > 0f || (info2 != null && info2.Value.foragedFoodPerDay.Item2 > 0f))
			{
				string text4 = multiline ? "\n" : " ";
				if (info2 == null)
				{
					text2 = string.Concat(new string[]
					{
						text2,
						text4,
						"(",
						info.foragedFoodPerDay.Item1.label,
						")"
					});
				}
				else
				{
					text3 = string.Concat(new string[]
					{
						text3,
						text4,
						"(",
						info2.Value.foragedFoodPerDay.Item1.label.Truncate(50f, null),
						")"
					});
				}
			}
			if (isCaravan)
			{
				CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("ForagedFoodPerDay".Translate(), text2, Color.white, taggedString5, text3, Color.white, -9999f));
				string text5 = "CaravanVisibilityTip".Translate();
				if (!info.visibilityExplanation.NullOrEmpty())
				{
					text5 = text5 + "\n\n" + info.visibilityExplanation;
				}
				if (info2 != null && !info2.Value.visibilityExplanation.NullOrEmpty())
				{
					text5 = text5 + "\n\n-----\n\n" + info2.Value.visibilityExplanation;
				}
				CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("Visibility".Translate(), info.visibility.ToStringPercent(), GenUI.LerpColor(CaravanUIUtility.VisibilityColor, info.visibility), text5, (info2 != null) ? info2.GetValueOrDefault().visibility.ToStringPercent() : null, (info2 != null) ? GenUI.LerpColor(CaravanUIUtility.VisibilityColor, info2.Value.visibility) : Color.white, -9999f));
			}
			TransferableUIUtility.DrawExtraInfo(CaravanUIUtility.tmpInfo, rect);
		}

		// Token: 0x060156F8 RID: 87800 RVA: 0x0066B146 File Offset: 0x00669346
		private static Color GetMassColor(float massUsage, float massCapacity, bool lerpMassColor)
		{
			if (massCapacity == 0f)
			{
				return Color.white;
			}
			if (massUsage > massCapacity)
			{
				return Color.red;
			}
			if (lerpMassColor)
			{
				return GenUI.LerpColor(CaravanUIUtility.MassColor, massUsage / massCapacity);
			}
			return Color.white;
		}

		// Token: 0x060156F9 RID: 87801 RVA: 0x0066B178 File Offset: 0x00669378
		private static string GetMassTip(float massUsage, float massCapacity, string massCapacityExplanation, float? massUsage2, float? massCapacity2, string massCapacity2Explanation, bool isCaravan)
		{
			TaggedString taggedString = "MassCarriedSimple".Translate() + ": " + massUsage.ToStringEnsureThreshold(massCapacity, 2) + " " + "kg".Translate() + "\n" + "MassCapacity".Translate() + ": " + massCapacity.ToString("F2") + " " + "kg".Translate();
			if (massUsage2 != null)
			{
				taggedString += "\n <-> \n" + "MassCarriedSimple".Translate() + ": " + massUsage2.Value.ToStringEnsureThreshold(massCapacity2.Value, 2) + " " + "kg".Translate() + "\n" + "MassCapacity".Translate() + ": " + massCapacity2.Value.ToString("F2") + " " + "kg".Translate();
			}
			if (isCaravan)
			{
				taggedString += "\n\n" + "CaravanMassUsageTooltip".Translate();
			}
			if (!massCapacityExplanation.NullOrEmpty())
			{
				taggedString += "\n\n" + "MassCapacity".Translate() + ":" + "\n" + massCapacityExplanation;
			}
			if (!massCapacity2Explanation.NullOrEmpty())
			{
				taggedString += "\n\n-----\n\n" + "MassCapacity".Translate() + ":" + "\n" + massCapacity2Explanation;
			}
			return taggedString;
		}

		// Token: 0x0400EE8B RID: 61067
		private static readonly List<Pair<float, Color>> MassColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0.37f, Color.green),
			new Pair<float, Color>(0.82f, Color.yellow),
			new Pair<float, Color>(1f, new Color(1f, 0.6f, 0f))
		};

		// Token: 0x0400EE8C RID: 61068
		private static readonly List<Pair<float, Color>> TilesPerDayColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0f, Color.white),
			new Pair<float, Color>(0.001f, ColorLibrary.RedReadable),
			new Pair<float, Color>(1f, Color.yellow),
			new Pair<float, Color>(2f, Color.white)
		};

		// Token: 0x0400EE8D RID: 61069
		private static readonly List<Pair<float, Color>> DaysWorthOfFoodColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(1f, Color.red),
			new Pair<float, Color>(2f, Color.white)
		};

		// Token: 0x0400EE8E RID: 61070
		private static readonly List<Pair<float, Color>> DaysWorthOfFoodKnownRouteColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0.3f, ColorLibrary.RedReadable),
			new Pair<float, Color>(0.9f, Color.yellow),
			new Pair<float, Color>(1.02f, Color.green)
		};

		// Token: 0x0400EE8F RID: 61071
		private static readonly List<Pair<float, Color>> VisibilityColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0f, Color.white),
			new Pair<float, Color>(0.01f, Color.green),
			new Pair<float, Color>(0.2f, Color.green),
			new Pair<float, Color>(1f, Color.white),
			new Pair<float, Color>(1.2f, ColorLibrary.RedReadable)
		};

		// Token: 0x0400EE90 RID: 61072
		private static readonly List<TransferableUIUtility.ExtraInfo> tmpInfo = new List<TransferableUIUtility.ExtraInfo>();

		// Token: 0x02003D75 RID: 15733
		private enum TransferableCategory
		{
			// Token: 0x0400EE92 RID: 61074
			Pawn,
			// Token: 0x0400EE93 RID: 61075
			Item,
			// Token: 0x0400EE94 RID: 61076
			TravelSupplies
		}

		// Token: 0x02003D76 RID: 15734
		public struct CaravanInfo
		{
			// Token: 0x060156FB RID: 87803 RVA: 0x0066B528 File Offset: 0x00669728
			public CaravanInfo(float massUsage, float massCapacity, string massCapacityExplanation, float tilesPerDay, string tilesPerDayExplanation, [TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})] ValueTuple<float, float> daysWorthOfFood, [TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})] ValueTuple<ThingDef, float> foragedFoodPerDay, string foragedFoodPerDayExplanation, float visibility, string visibilityExplanation, float extraMassUsage = -1f, float extraMassCapacity = -1f, string extraMassCapacityExplanation = null)
			{
				this.massUsage = massUsage;
				this.massCapacity = massCapacity;
				this.massCapacityExplanation = massCapacityExplanation;
				this.tilesPerDay = tilesPerDay;
				this.tilesPerDayExplanation = tilesPerDayExplanation;
				this.daysWorthOfFood = daysWorthOfFood;
				this.foragedFoodPerDay = foragedFoodPerDay;
				this.foragedFoodPerDayExplanation = foragedFoodPerDayExplanation;
				this.visibility = visibility;
				this.visibilityExplanation = visibilityExplanation;
				this.extraMassUsage = extraMassUsage;
				this.extraMassCapacity = extraMassCapacity;
				this.extraMassCapacityExplanation = extraMassCapacityExplanation;
			}

			// Token: 0x0400EE95 RID: 61077
			public float massUsage;

			// Token: 0x0400EE96 RID: 61078
			public float massCapacity;

			// Token: 0x0400EE97 RID: 61079
			public string massCapacityExplanation;

			// Token: 0x0400EE98 RID: 61080
			public float tilesPerDay;

			// Token: 0x0400EE99 RID: 61081
			public string tilesPerDayExplanation;

			// Token: 0x0400EE9A RID: 61082
			[TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			public ValueTuple<float, float> daysWorthOfFood;

			// Token: 0x0400EE9B RID: 61083
			[TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			public ValueTuple<ThingDef, float> foragedFoodPerDay;

			// Token: 0x0400EE9C RID: 61084
			public string foragedFoodPerDayExplanation;

			// Token: 0x0400EE9D RID: 61085
			public float visibility;

			// Token: 0x0400EE9E RID: 61086
			public string visibilityExplanation;

			// Token: 0x0400EE9F RID: 61087
			public float extraMassUsage;

			// Token: 0x0400EEA0 RID: 61088
			public float extraMassCapacity;

			// Token: 0x0400EEA1 RID: 61089
			public string extraMassCapacityExplanation;
		}
	}
}
