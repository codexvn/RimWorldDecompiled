using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200337F RID: 13183
	[StaticConstructorOnStartup]
	public class InspectPaneFiller
	{
		// Token: 0x06012896 RID: 75926 RVA: 0x00577DBC File Offset: 0x00575FBC
		public static void DoPaneContentsFor(ISelectable sel, Rect rect)
		{
			try
			{
				Widgets.BeginGroup(rect);
				float num = 0f;
				Pawn pawn = sel as Pawn;
				Thing thing = sel as Thing;
				if (thing != null && !thing.def.onlyShowInspectString)
				{
					num += 3f;
					WidgetRow row = new WidgetRow(0f, num, UIDirection.RightThenUp, 99999f, 4f);
					InspectPaneFiller.DrawHealth(row, thing);
					if (pawn != null)
					{
						if (pawn.IsGhoul && pawn.needs.food != null)
						{
							InspectPaneFiller.DrawHunger(row, pawn);
						}
						else
						{
							InspectPaneFiller.DrawMood(row, pawn);
						}
						if (pawn.timetable != null && !pawn.IsPrisonerOfColony)
						{
							InspectPaneFiller.DrawTimetableSetting(row, pawn);
						}
						InspectPaneFiller.DrawAreaAllowed(row, pawn);
						Pawn_NeedsTracker needs = pawn.needs;
						if (((needs != null) ? needs.energy : null) != null)
						{
							InspectPaneFiller.DrawMechEnergy(row, pawn);
						}
					}
					num += 18f;
				}
				Rect rect2 = rect.AtZero();
				rect2.yMin = num;
				InspectPaneFiller.DrawInspectStringFor(sel, rect2);
			}
			catch (Exception arg)
			{
				Log.ErrorOnce(string.Format("Error in DoPaneContentsFor {0}: {1}", Find.Selector.FirstSelectedObject, arg), 754672);
			}
			finally
			{
				Widgets.EndGroup();
			}
		}

		// Token: 0x06012897 RID: 75927 RVA: 0x00577F08 File Offset: 0x00576108
		public static void DoPaneContentsForStorageGroup(StorageGroup group, Rect rect)
		{
			Widgets.BeginGroup(rect);
			string text = string.Format("\n{0}: {1} ", "StorageGroupLabel".Translate(), group.RenamableLabel.CapitalizeFirst());
			if (group.MemberCount > 1)
			{
				text += string.Format("({0})", "NumBuildings".Translate(group.MemberCount));
			}
			else
			{
				text += string.Format("({0})", "OneBuilding".Translate());
			}
			InspectPaneFiller.DrawInspectString(text, rect.AtZero());
			Widgets.EndGroup();
		}

		// Token: 0x06012898 RID: 75928 RVA: 0x00577FA8 File Offset: 0x005761A8
		public static void DrawHealth(WidgetRow row, Thing t)
		{
			Pawn pawn = t as Pawn;
			float fillPct;
			string label;
			if (pawn == null)
			{
				if (!t.def.useHitPoints)
				{
					return;
				}
				if (t.HitPoints >= t.MaxHitPoints)
				{
					GUI.color = Color.white;
				}
				else if ((float)t.HitPoints > (float)t.MaxHitPoints * 0.5f)
				{
					GUI.color = Color.yellow;
				}
				else if (t.HitPoints > 0)
				{
					GUI.color = Color.red;
				}
				else
				{
					GUI.color = Color.grey;
				}
				fillPct = (float)t.HitPoints / (float)t.MaxHitPoints;
				label = t.HitPoints.ToStringCached() + " / " + t.MaxHitPoints.ToStringCached();
			}
			else
			{
				GUI.color = Color.white;
				fillPct = pawn.health.summaryHealth.SummaryHealthPercent;
				label = HealthUtility.GetGeneralConditionLabel(pawn, true);
			}
			row.FillableBar(93f, 16f, fillPct, label, InspectPaneFiller.HealthTex, InspectPaneFiller.BarBGTex);
			GUI.color = Color.white;
		}

		// Token: 0x06012899 RID: 75929 RVA: 0x005780A8 File Offset: 0x005762A8
		private static void DrawMood(WidgetRow row, Pawn pawn)
		{
			if (pawn.needs == null || pawn.needs.mood == null)
			{
				return;
			}
			row.Gap(6f);
			row.FillableBar(93f, 16f, pawn.needs.mood.CurLevelPercentage, pawn.needs.mood.MoodString.CapitalizeFirst(), InspectPaneFiller.MoodTex, InspectPaneFiller.BarBGTex);
		}

		// Token: 0x0601289A RID: 75930 RVA: 0x00578118 File Offset: 0x00576318
		private static void DrawTimetableSetting(WidgetRow row, Pawn pawn)
		{
			row.Gap(6f);
			row.FillableBar(93f, 16f, 1f, pawn.timetable.CurrentAssignment.LabelCap, pawn.timetable.CurrentAssignment.ColorTexture, null);
		}

		// Token: 0x0601289B RID: 75931 RVA: 0x0057816C File Offset: 0x0057636C
		private static void DrawAreaAllowed(WidgetRow row, Pawn pawn)
		{
			if (pawn.playerSettings == null || !pawn.playerSettings.SupportsAllowedAreas)
			{
				return;
			}
			if (pawn.Faction != Faction.OfPlayer || pawn.HostFaction != null)
			{
				return;
			}
			if (pawn.IsMutant && !pawn.mutant.Def.respectsAllowedArea)
			{
				return;
			}
			row.Gap(6f);
			Pawn_PlayerSettings playerSettings = pawn.playerSettings;
			bool flag = ((playerSettings != null) ? playerSettings.AreaRestrictionInPawnCurrentMap : null) != null;
			Texture2D fillTex;
			if (flag)
			{
				fillTex = pawn.playerSettings.AreaRestrictionInPawnCurrentMap.ColorTexture;
			}
			else
			{
				fillTex = BaseContent.GreyTex;
			}
			Rect rect = row.FillableBar(93f, 16f, 1f, AreaUtility.AreaAllowedLabel(pawn), fillTex, null);
			if (Mouse.IsOver(rect))
			{
				if (flag)
				{
					pawn.playerSettings.AreaRestrictionInPawnCurrentMap.MarkForDraw();
				}
				Widgets.DrawBox(rect.ContractedBy(-1f), 1, null);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
				{
					pawn.playerSettings.AreaRestrictionInPawnCurrentMap = a;
				}, true, true, pawn.MapHeld);
			}
		}

		// Token: 0x0601289C RID: 75932 RVA: 0x005782B0 File Offset: 0x005764B0
		private static void DrawMechEnergy(WidgetRow row, Pawn pawn)
		{
			row.Gap(6f);
			row.FillableBar(93f, 16f, pawn.needs.energy.CurLevelPercentage, "Energy".Translate(), InspectPaneFiller.EnergyTex, InspectPaneFiller.BarBGTex);
		}

		// Token: 0x0601289D RID: 75933 RVA: 0x00578304 File Offset: 0x00576504
		private static void DrawHunger(WidgetRow row, Pawn pawn)
		{
			row.Gap(6f);
			row.FillableBar(93f, 16f, pawn.needs.food.CurLevelPercentage, pawn.needs.food.CurCategory.GetLabel(), InspectPaneFiller.HungerTex, InspectPaneFiller.BarBGTex);
		}

		// Token: 0x0601289E RID: 75934 RVA: 0x0057835C File Offset: 0x0057655C
		public static void DrawInspectStringFor(ISelectable sel, Rect rect)
		{
			string text;
			try
			{
				text = sel.GetInspectString();
				Thing thing = sel as Thing;
				if (thing != null)
				{
					string inspectStringLowPriority = thing.GetInspectStringLowPriority();
					if (!inspectStringLowPriority.NullOrEmpty())
					{
						if (!text.NullOrEmpty())
						{
							text = text.TrimEndNewlines() + "\n";
						}
						text += inspectStringLowPriority;
					}
				}
			}
			catch (Exception arg)
			{
				text = string.Format("GetInspectString exception on {0}:\n{1}", sel, arg);
				if (!InspectPaneFiller.debug_inspectStringExceptionErrored)
				{
					Log.Error(text);
					InspectPaneFiller.debug_inspectStringExceptionErrored = true;
				}
			}
			if (!text.NullOrEmpty() && GenText.ContainsEmptyLines(text))
			{
				Log.ErrorOnce(string.Format("Inspect string for {0} contains empty lines.\n\nSTART\n{1}\nEND", sel, text), 837163521);
			}
			InspectPaneFiller.DrawInspectString(text, rect);
		}

		// Token: 0x0601289F RID: 75935 RVA: 0x0057840C File Offset: 0x0057660C
		public static void DrawInspectString(string str, Rect rect)
		{
			Text.Font = GameFont.Small;
			Widgets.LabelScrollable(rect, str, ref InspectPaneFiller.inspectStringScrollPos, true, true, false);
		}

		// Token: 0x0400C086 RID: 49286
		private const float BarHeight = 16f;

		// Token: 0x0400C087 RID: 49287
		private static readonly Texture2D MoodTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(26, 52, 52).ToColor);

		// Token: 0x0400C088 RID: 49288
		private static readonly Texture2D BarBGTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(10, 10, 10).ToColor);

		// Token: 0x0400C089 RID: 49289
		private static readonly Texture2D HealthTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(35, 35, 35).ToColor);

		// Token: 0x0400C08A RID: 49290
		private static readonly Texture2D EnergyTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(0, 52, 75).ToColor);

		// Token: 0x0400C08B RID: 49291
		private static readonly Texture2D HungerTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(184, 156, 90).ToColor);

		// Token: 0x0400C08C RID: 49292
		private const float BarWidth = 93f;

		// Token: 0x0400C08D RID: 49293
		private const float BarSpacing = 6f;

		// Token: 0x0400C08E RID: 49294
		private static bool debug_inspectStringExceptionErrored;

		// Token: 0x0400C08F RID: 49295
		private static Vector2 inspectStringScrollPos;
	}
}
