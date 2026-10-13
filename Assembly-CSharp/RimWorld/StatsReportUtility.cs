using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003246 RID: 12870
	public static class StatsReportUtility
	{
		// Token: 0x17002EC3 RID: 11971
		// (get) Token: 0x06012170 RID: 74096 RVA: 0x0054C122 File Offset: 0x0054A322
		public static int SelectedStatIndex
		{
			get
			{
				if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>() || StatsReportUtility.selectedEntry == null)
				{
					return -1;
				}
				return StatsReportUtility.cachedDrawEntries.IndexOf(StatsReportUtility.selectedEntry);
			}
		}

		// Token: 0x17002EC4 RID: 11972
		// (get) Token: 0x06012171 RID: 74097 RVA: 0x0054C148 File Offset: 0x0054A348
		public static QuickSearchWidget QuickSearchWidget
		{
			get
			{
				return StatsReportUtility.quickSearchWidget;
			}
		}

		// Token: 0x06012172 RID: 74098 RVA: 0x0054C150 File Offset: 0x0054A350
		public static void Reset()
		{
			StatsReportUtility.scrollPosition = default(Vector2);
			StatsReportUtility.scrollPositionRightPanel = default(Vector2);
			StatsReportUtility.selectedEntry = null;
			StatsReportUtility.scrollPositioner.Arm(false);
			StatsReportUtility.mousedOverEntry = null;
			StatsReportUtility.cachedDrawEntries.Clear();
			StatsReportUtility.cachedEntryValues.Clear();
			StatsReportUtility.quickSearchWidget.Reset();
			PermitsCardUtility.selectedPermit = null;
			PermitsCardUtility.selectedFaction = ((ModLister.RoyaltyInstalled && Current.ProgramState == ProgramState.Playing) ? Faction.OfEmpire : null);
		}

		// Token: 0x06012173 RID: 74099 RVA: 0x0054C1CC File Offset: 0x0054A3CC
		public static void DrawStatsReport(Rect rect, Def def, ThingDef stuff)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				BuildableDef buildableDef = def as BuildableDef;
				StatRequest req = (buildableDef != null) ? StatRequest.For(buildableDef, stuff, QualityCategory.Normal) : StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def, stuff)
				where r.ShouldDisplay(null)
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x06012174 RID: 74100 RVA: 0x0054C258 File Offset: 0x0054A458
		public static void DrawStatsReport(Rect rect, AbilityDef def)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatRequest req = StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def)
				where r.ShouldDisplay(null)
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x06012175 RID: 74101 RVA: 0x0054C2D0 File Offset: 0x0054A4D0
		public static void DrawStatsReport(Rect rect, Thing thing)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(thing.def.SpecialDisplayStats(StatRequest.For(thing)));
				StatsReportUtility.cachedDrawEntries.AddRange(StatsReportUtility.StatsToDraw(thing));
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => (de.stat != null && !de.stat.showNonAbstract) || !de.ShouldDisplay(thing));
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, thing, null);
		}

		// Token: 0x06012176 RID: 74102 RVA: 0x0054C360 File Offset: 0x0054A560
		public static void DrawStatsReport(Rect rect, Hediff hediff)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(hediff.SpecialDisplayStats(StatRequest.ForEmpty()));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(hediff.def, null)
				where r.ShouldDisplay(null)
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x06012177 RID: 74103 RVA: 0x0054C3DC File Offset: 0x0054A5DC
		public static void DrawStatsReport(Rect rect, WorldObject worldObject)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(worldObject.def.SpecialDisplayStats(StatRequest.ForEmpty()));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(worldObject)
				where r.ShouldDisplay(null)
				select r);
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, worldObject);
		}

		// Token: 0x06012178 RID: 74104 RVA: 0x0054C484 File Offset: 0x0054A684
		public static void DrawStatsReport(Rect rect, RoyalTitleDef title, Faction faction, Pawn pawn = null)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(title.SpecialDisplayStats(StatRequest.For(title, faction, pawn)));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(title, faction)
				where r.ShouldDisplay(pawn)
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x06012179 RID: 74105 RVA: 0x0054C4FC File Offset: 0x0054A6FC
		public static void DrawStatsReport(Rect rect, Faction faction)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatRequest req = StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(faction.def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(faction)
				where r.ShouldDisplay(null)
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x0601217A RID: 74106 RVA: 0x0054C577 File Offset: 0x0054A777
		private static IEnumerable<StatDrawEntry> StatsToDraw(Def def, ThingDef stuff)
		{
			yield return StatsReportUtility.DescriptionEntry(def);
			BuildableDef eDef = def as BuildableDef;
			if (eDef != null)
			{
				StatRequest statRequest = StatRequest.For(eDef, stuff, QualityCategory.Normal);
				IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
				Func<StatDef, bool> predicate;
				Func<StatDef, bool> <>9__0;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((StatDef st) => st.Worker.ShouldShowFor(statRequest)));
				}
				foreach (StatDef statDef in allDefs.Where(predicate))
				{
					yield return new StatDrawEntry(statDef.category, statDef, eDef.GetStatValueAbstract(statDef, stuff), StatRequest.For(eDef, stuff, QualityCategory.Normal), ToStringNumberSense.Undefined, null, false);
				}
				IEnumerator<StatDef> enumerator = null;
			}
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null && thingDef.IsStuff)
			{
				foreach (StatDrawEntry statDrawEntry in StatsReportUtility.StuffStats(thingDef))
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0601217B RID: 74107 RVA: 0x0054C58E File Offset: 0x0054A78E
		private static IEnumerable<StatDrawEntry> StatsToDraw(RoyalTitleDef title, Faction faction)
		{
			yield return StatsReportUtility.DescriptionEntry(title, faction);
			yield break;
		}

		// Token: 0x0601217C RID: 74108 RVA: 0x0054C5A5 File Offset: 0x0054A7A5
		private static IEnumerable<StatDrawEntry> StatsToDraw(Faction faction)
		{
			yield return StatsReportUtility.DescriptionEntry(faction);
			yield break;
		}

		// Token: 0x0601217D RID: 74109 RVA: 0x0054C5B5 File Offset: 0x0054A7B5
		private static IEnumerable<StatDrawEntry> StatsToDraw(AbilityDef def)
		{
			yield return StatsReportUtility.DescriptionEntry(def);
			StatRequest statRequest = StatRequest.For(def, null);
			IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
			Func<StatDef, bool> <>9__0;
			Func<StatDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((StatDef st) => st.Worker.ShouldShowFor(statRequest)));
			}
			foreach (StatDef statDef in allDefs.Where(predicate))
			{
				yield return new StatDrawEntry(statDef.category, statDef, def.GetStatValueAbstract(statDef, null), StatRequest.For(def, null), ToStringNumberSense.Undefined, null, false);
			}
			IEnumerator<StatDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0601217E RID: 74110 RVA: 0x0054C5C5 File Offset: 0x0054A7C5
		private static IEnumerable<StatDrawEntry> StatsToDraw(Thing thing)
		{
			yield return StatsReportUtility.DescriptionEntry(thing);
			StatDrawEntry statDrawEntry = StatsReportUtility.QualityEntry(thing);
			if (statDrawEntry != null)
			{
				yield return statDrawEntry;
			}
			IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
			Func<StatDef, bool> <>9__0;
			Func<StatDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((StatDef st) => st.Worker.ShouldShowFor(StatRequest.For(thing))));
			}
			foreach (StatDef statDef in allDefs.Where(predicate))
			{
				if (!statDef.Worker.IsDisabledFor(thing))
				{
					float statValue = thing.GetStatValue(statDef, true, -1);
					if (statDef.showOnDefaultValue || statValue != statDef.defaultBaseValue)
					{
						yield return new StatDrawEntry(statDef.category, statDef, statValue, StatRequest.For(thing), ToStringNumberSense.Undefined, null, false);
					}
				}
				else
				{
					yield return new StatDrawEntry(statDef.category, statDef);
				}
			}
			IEnumerator<StatDef> enumerator = null;
			if (thing.def.useHitPoints)
			{
				StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "HitPointsBasic".Translate().CapitalizeFirst(), thing.HitPoints.ToString() + " / " + thing.MaxHitPoints.ToString(), "Stat_HitPoints_Desc".Translate(), 99998, null, null, false, false);
				yield return statDrawEntry2;
			}
			foreach (StatDrawEntry statDrawEntry3 in thing.SpecialDisplayStats())
			{
				yield return statDrawEntry3;
			}
			IEnumerator<StatDrawEntry> enumerator2 = null;
			if (thing.def.IsStuff)
			{
				foreach (StatDrawEntry statDrawEntry4 in StatsReportUtility.StuffStats(thing.def))
				{
					yield return statDrawEntry4;
				}
				enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0601217F RID: 74111 RVA: 0x0054C5D5 File Offset: 0x0054A7D5
		private static IEnumerable<StatDrawEntry> StatsToDraw(WorldObject worldObject)
		{
			yield return StatsReportUtility.DescriptionEntry(worldObject);
			foreach (StatDrawEntry statDrawEntry in worldObject.SpecialDisplayStats)
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06012180 RID: 74112 RVA: 0x0054C5E5 File Offset: 0x0054A7E5
		private static IEnumerable<StatDrawEntry> StuffStats(ThingDef stuffDef)
		{
			if (!stuffDef.stuffProps.statFactors.NullOrEmpty<StatModifier>())
			{
				int num;
				for (int i = 0; i < stuffDef.stuffProps.statFactors.Count; i = num + 1)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.StuffStatFactors, stuffDef.stuffProps.statFactors[i].stat, stuffDef.stuffProps.statFactors[i].value, StatRequest.ForEmpty(), ToStringNumberSense.Factor, null, false);
					num = i;
				}
			}
			if (!stuffDef.stuffProps.statFactorsQuality.NullOrEmpty<StatModifierQuality>())
			{
				foreach (StatModifierQuality statModifierQuality in stuffDef.stuffProps.statFactorsQuality)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.StuffStatFactors, statModifierQuality.stat, statModifierQuality.ToStringAsFactorRange);
				}
				List<StatModifierQuality>.Enumerator enumerator = default(List<StatModifierQuality>.Enumerator);
			}
			if (!stuffDef.stuffProps.statOffsets.NullOrEmpty<StatModifier>())
			{
				int num;
				for (int i = 0; i < stuffDef.stuffProps.statOffsets.Count; i = num + 1)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.StuffStatOffsets, stuffDef.stuffProps.statOffsets[i].stat, stuffDef.stuffProps.statOffsets[i].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset, null, false);
					num = i;
				}
			}
			if (!stuffDef.stuffProps.statOffsetsQuality.NullOrEmpty<StatModifierQuality>())
			{
				foreach (StatModifierQuality statModifierQuality2 in stuffDef.stuffProps.statOffsetsQuality)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.StuffStatOffsets, statModifierQuality2.stat, statModifierQuality2.ToStringAsOffsetRange);
				}
				List<StatModifierQuality>.Enumerator enumerator = default(List<StatModifierQuality>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06012181 RID: 74113 RVA: 0x0054C5F8 File Offset: 0x0054A7F8
		private static void FinalizeCachedDrawEntries(IEnumerable<StatDrawEntry> original)
		{
			StatsReportUtility.cachedDrawEntries = (from sd in original
			orderby sd.category.displayOrder, sd.DisplayPriorityWithinCategory descending, sd.LabelCap
			select sd).ToList<StatDrawEntry>();
			StatsReportUtility.quickSearchWidget.noResultsMatched = !StatsReportUtility.cachedDrawEntries.Any<StatDrawEntry>();
			foreach (StatDrawEntry statDrawEntry in StatsReportUtility.cachedDrawEntries)
			{
				StatsReportUtility.cachedEntryValues.Add(statDrawEntry.ValueString);
			}
			if (StatsReportUtility.selectedEntry != null)
			{
				StatsReportUtility.selectedEntry = StatsReportUtility.cachedDrawEntries.FirstOrDefault((StatDrawEntry e) => e.Same(StatsReportUtility.selectedEntry));
			}
			if (StatsReportUtility.quickSearchWidget.filter.Active)
			{
				foreach (StatDrawEntry sd2 in StatsReportUtility.cachedDrawEntries)
				{
					if (StatsReportUtility.Matches(sd2))
					{
						StatsReportUtility.selectedEntry = sd2;
						StatsReportUtility.scrollPositioner.Arm(true);
						break;
					}
				}
			}
		}

		// Token: 0x06012182 RID: 74114 RVA: 0x0054C77C File Offset: 0x0054A97C
		private static bool Matches(StatDrawEntry sd)
		{
			return StatsReportUtility.quickSearchWidget.filter.Matches(sd.LabelCap);
		}

		// Token: 0x06012183 RID: 74115 RVA: 0x0054C794 File Offset: 0x0054A994
		private static StatDrawEntry DescriptionEntry(Def def)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", def.description, 99999, null, Dialog_InfoCard.DefsToHyperlinks(def.descriptionHyperlinks), false, false);
		}

		// Token: 0x06012184 RID: 74116 RVA: 0x0054C7D8 File Offset: 0x0054A9D8
		private static StatDrawEntry DescriptionEntry(Faction faction)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", faction.GetReportText, 99999, null, Dialog_InfoCard.DefsToHyperlinks(faction.def.descriptionHyperlinks), false, false);
		}

		// Token: 0x06012185 RID: 74117 RVA: 0x0054C824 File Offset: 0x0054AA24
		private static StatDrawEntry DescriptionEntry(RoyalTitleDef title, Faction faction)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", title.GetReportText(faction), 99999, null, Dialog_InfoCard.TitleDefsToHyperlinks(title.GetHyperlinks(faction)), false, false);
		}

		// Token: 0x06012186 RID: 74118 RVA: 0x0054C86C File Offset: 0x0054AA6C
		private static StatDrawEntry DescriptionEntry(Thing thing)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", thing.DescriptionFlavor, 99999, null, Dialog_InfoCard.DefsToHyperlinks(thing.DescriptionHyperlinks), false, true);
		}

		// Token: 0x06012187 RID: 74119 RVA: 0x0054C8B0 File Offset: 0x0054AAB0
		private static StatDrawEntry DescriptionEntry(WorldObject worldObject)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", worldObject.GetDescription(), 99999, null, null, false, false);
		}

		// Token: 0x06012188 RID: 74120 RVA: 0x0054C8EC File Offset: 0x0054AAEC
		private static StatDrawEntry QualityEntry(Thing t)
		{
			QualityCategory cat;
			if (!t.TryGetQuality(out cat))
			{
				return null;
			}
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Quality".Translate(), cat.GetLabel().CapitalizeFirst(), "QualityDescription".Translate(), 99999, null, null, false, false);
		}

		// Token: 0x06012189 RID: 74121 RVA: 0x0054C941 File Offset: 0x0054AB41
		public static void SelectEntry(int index)
		{
			if (index < 0 || index > StatsReportUtility.cachedDrawEntries.Count)
			{
				return;
			}
			StatsReportUtility.SelectEntry(StatsReportUtility.cachedDrawEntries[index], true);
		}

		// Token: 0x0601218A RID: 74122 RVA: 0x0054C968 File Offset: 0x0054AB68
		public static void SelectEntry(StatDef stat, bool playSound = false)
		{
			foreach (StatDrawEntry statDrawEntry in StatsReportUtility.cachedDrawEntries)
			{
				if (statDrawEntry.stat == stat)
				{
					StatsReportUtility.SelectEntry(statDrawEntry, playSound);
					return;
				}
			}
			Messages.Message("MessageCannotSelectInvisibleStat".Translate(stat), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x0601218B RID: 74123 RVA: 0x0054C9E8 File Offset: 0x0054ABE8
		private static void SelectEntry(StatDrawEntry rec, bool playSound = true)
		{
			StatsReportUtility.selectedEntry = rec;
			if (playSound)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0601218C RID: 74124 RVA: 0x0054CA00 File Offset: 0x0054AC00
		private static void DrawStatsWorker(Rect rect, Thing optionalThing, WorldObject optionalWorldObject)
		{
			Rect rect2 = new Rect(rect);
			rect2.width *= 0.5f;
			Rect rect3 = new Rect(rect);
			rect3.x = rect2.xMax;
			rect3.width = rect.xMax - rect3.x;
			StatsReportUtility.scrollPositioner.ClearInterestRects();
			Text.Font = GameFont.Small;
			Rect viewRect = new Rect(0f, 0f, rect2.width - 16f, StatsReportUtility.listHeight);
			Widgets.BeginScrollView(rect2, ref StatsReportUtility.scrollPosition, viewRect, true);
			float num = 0f;
			string b = null;
			StatsReportUtility.mousedOverEntry = null;
			for (int i = 0; i < StatsReportUtility.cachedDrawEntries.Count; i++)
			{
				StatDrawEntry ent = StatsReportUtility.cachedDrawEntries[i];
				if (ent.category.LabelCap != b)
				{
					Widgets.ListSeparator(ref num, viewRect.width, ent.category.LabelCap);
					b = ent.category.LabelCap;
				}
				bool highlightLabel = false;
				bool lowlightLabel = false;
				bool flag = StatsReportUtility.selectedEntry == ent;
				bool flag2 = false;
				GUI.color = Color.white;
				if (StatsReportUtility.quickSearchWidget.filter.Active)
				{
					if (StatsReportUtility.Matches(ent))
					{
						highlightLabel = true;
						flag2 = true;
					}
					else
					{
						lowlightLabel = true;
					}
				}
				Rect rect4 = new Rect(8f, num, viewRect.width - 8f, 30f);
				num += ent.Draw(rect4.x, rect4.y, rect4.width, flag, highlightLabel, lowlightLabel, delegate
				{
					StatsReportUtility.SelectEntry(ent, true);
				}, delegate
				{
					StatsReportUtility.mousedOverEntry = ent;
				}, StatsReportUtility.scrollPosition, rect2, StatsReportUtility.cachedEntryValues[i]);
				rect4.yMax = num;
				if (flag || flag2)
				{
					StatsReportUtility.scrollPositioner.RegisterInterestRect(rect4);
				}
			}
			StatsReportUtility.listHeight = num + 100f;
			Widgets.EndScrollView();
			StatsReportUtility.scrollPositioner.ScrollVertically(ref StatsReportUtility.scrollPosition, rect2.size);
			Rect outRect = rect3.ContractedBy(10f);
			StatDrawEntry statDrawEntry;
			if ((statDrawEntry = StatsReportUtility.selectedEntry) == null)
			{
				statDrawEntry = (StatsReportUtility.mousedOverEntry ?? StatsReportUtility.cachedDrawEntries.FirstOrDefault<StatDrawEntry>());
			}
			StatDrawEntry statDrawEntry2 = statDrawEntry;
			if (statDrawEntry2 != null)
			{
				Rect rect5 = new Rect(0f, 0f, outRect.width - 16f, StatsReportUtility.rightPanelHeight);
				StatRequest statRequest;
				if (statDrawEntry2.hasOptionalReq)
				{
					statRequest = statDrawEntry2.optionalReq;
				}
				else if (optionalThing != null)
				{
					statRequest = StatRequest.For(optionalThing);
				}
				else
				{
					statRequest = StatRequest.ForEmpty();
				}
				string explanationText = statDrawEntry2.GetExplanationText(statRequest);
				float num2 = 0f;
				Widgets.BeginScrollView(outRect, ref StatsReportUtility.scrollPositionRightPanel, rect5, true);
				Rect rect6 = rect5;
				rect6.width -= 4f;
				Widgets.Label(rect6, explanationText);
				float num3 = Text.CalcHeight(explanationText, rect6.width) + 10f;
				num2 += num3;
				IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks = statDrawEntry2.GetHyperlinks(statRequest);
				if (hyperlinks != null)
				{
					Rect rect7 = new Rect(rect6.x, rect6.y + num3, rect6.width, rect6.height - num3);
					Color color = GUI.color;
					GUI.color = Widgets.NormalOptionColor;
					foreach (Dialog_InfoCard.Hyperlink hyperlink in hyperlinks)
					{
						float num4 = Text.CalcHeight(hyperlink.Label, rect7.width);
						Rect rect8 = new Rect(rect7.x, rect7.y, rect7.width, num4);
						string text = hyperlink.HasGeneOwnerThing ? hyperlink.Label : "ViewHyperlink".Translate(hyperlink.Label).ToString();
						Widgets.HyperlinkWithIcon(rect8, hyperlink, text, 2f, 6f, null, false, null);
						rect7.y += num4;
						rect7.height -= num4;
						num2 += num4;
					}
					GUI.color = color;
				}
				StatsReportUtility.rightPanelHeight = num2;
				Widgets.EndScrollView();
			}
		}

		// Token: 0x0601218D RID: 74125 RVA: 0x0054CE6C File Offset: 0x0054B06C
		public static void Notify_QuickSearchChanged()
		{
			StatsReportUtility.cachedDrawEntries.Clear();
			StatsReportUtility.cachedEntryValues.Clear();
		}

		// Token: 0x0400BBE2 RID: 48098
		private static StatDrawEntry selectedEntry;

		// Token: 0x0400BBE3 RID: 48099
		private static StatDrawEntry mousedOverEntry;

		// Token: 0x0400BBE4 RID: 48100
		private static Vector2 scrollPosition;

		// Token: 0x0400BBE5 RID: 48101
		private static ScrollPositioner scrollPositioner = new ScrollPositioner();

		// Token: 0x0400BBE6 RID: 48102
		private static Vector2 scrollPositionRightPanel;

		// Token: 0x0400BBE7 RID: 48103
		private static QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x0400BBE8 RID: 48104
		private static float listHeight;

		// Token: 0x0400BBE9 RID: 48105
		private static float rightPanelHeight;

		// Token: 0x0400BBEA RID: 48106
		private static List<StatDrawEntry> cachedDrawEntries = new List<StatDrawEntry>();

		// Token: 0x0400BBEB RID: 48107
		private static List<string> cachedEntryValues = new List<string>();
	}
}
