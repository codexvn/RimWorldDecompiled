using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200313A RID: 12602
	public class Dialog_ChooseThingsForNewColony : Window
	{
		// Token: 0x17002E13 RID: 11795
		// (get) Token: 0x06011B87 RID: 72583 RVA: 0x0051B6BA File Offset: 0x005198BA
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(540f, 800f);
			}
		}

		// Token: 0x17002E14 RID: 11796
		// (get) Token: 0x06011B88 RID: 72584 RVA: 0x0051B6CC File Offset: 0x005198CC
		public int ColonistCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.colonists.Count; i++)
				{
					if (this.selected.Contains(this.colonists[i]))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17002E15 RID: 11797
		// (get) Token: 0x06011B89 RID: 72585 RVA: 0x0051B710 File Offset: 0x00519910
		public int SlaveCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.colonists.Count; i++)
				{
					if (this.selected.Contains(this.colonists[i]))
					{
						Pawn pawn = this.colonists[i] as Pawn;
						if (pawn != null && pawn.IsSlaveOfColony)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17002E16 RID: 11798
		// (get) Token: 0x06011B8A RID: 72586 RVA: 0x0051B770 File Offset: 0x00519970
		public int AnimalCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.animals.Count; i++)
				{
					if (this.selected.Contains(this.animals[i]))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17002E17 RID: 11799
		// (get) Token: 0x06011B8B RID: 72587 RVA: 0x0051B7B4 File Offset: 0x005199B4
		public int RelicCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.relics.Count; i++)
				{
					if (this.selected.Contains(this.relics[i]))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17002E18 RID: 11800
		// (get) Token: 0x06011B8C RID: 72588 RVA: 0x0051B7F8 File Offset: 0x005199F8
		private AcceptanceReport AcceptanceReport
		{
			get
			{
				int colonistCount = this.ColonistCount;
				if (colonistCount > this.maxColonists)
				{
					return "MessageNewColonyMax".Translate(this.maxColonists, "People".Translate());
				}
				if (colonistCount == 0)
				{
					return "MessageNewColoyRequiresOneColonist".Translate();
				}
				if (this.AnimalCount > this.maxAnimals)
				{
					return "MessageNewColonyMax".Translate(this.maxAnimals, "AnimalsLower".Translate());
				}
				if (this.RelicCount > this.maxRelics)
				{
					return "MessageNewColonyMax".Translate(this.maxRelics, (this.maxRelics == 1) ? "RelicLower".Translate() : "RelicsLower".Translate());
				}
				if (this.selectedItemCount > this.maxItems)
				{
					return "MessageNewColonyMax".Translate(this.maxItems, "ItemsLower".Translate());
				}
				return AcceptanceReport.WasAccepted;
			}
		}

		// Token: 0x06011B8D RID: 72589 RVA: 0x0051B914 File Offset: 0x00519B14
		public Dialog_ChooseThingsForNewColony(Action<List<Thing>> postAccepted, int maxColonists = 5, int maxAnimals = 5, int maxRelics = 1, int maxItems = 7, Action cancel = null) : base(null)
		{
			if (!ModLister.CheckIdeology("Choose new colony"))
			{
				return;
			}
			this.postAccepted = postAccepted;
			this.maxAnimals = maxAnimals;
			this.maxColonists = maxColonists;
			this.maxRelics = maxRelics;
			this.maxItems = maxItems;
			this.forcePause = true;
			this.closeOnCancel = true;
			this.absorbInputAroundWindow = true;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
			this.preventSave = true;
			this.cancel = cancel;
		}

		// Token: 0x06011B8E RID: 72590 RVA: 0x0051BA14 File Offset: 0x00519C14
		public override void PostOpen()
		{
			Dialog_ChooseThingsForNewColony.<>c__DisplayClass42_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			this.colonists.Clear();
			this.animals.Clear();
			this.relics.Clear();
			this.items.Clear();
			this.selected.Clear();
			this.selectedItemCount = 0;
			this.itemArchonexusAllowedStackCount.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
			{
				if (!pawn.IsQuestLodger())
				{
					if (pawn.IsColonist)
					{
						this.colonists.Add(pawn);
					}
					else if (pawn.IsAnimal)
					{
						this.animals.Add(pawn);
					}
				}
			}
			foreach (Precept precept in Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading)
			{
				Precept_Relic precept_Relic = precept as Precept_Relic;
				if (precept_Relic != null && precept_Relic.RelicInPlayerPossession)
				{
					this.relics.Add(precept_Relic.GeneratedRelic);
				}
			}
			CS$<>8__locals1.itemDefMap = new Dictionary<ThingDef, List<Thing>>();
			CS$<>8__locals1.distinctItem = new List<Thing>();
			List<Thing> list = new List<Thing>();
			foreach (TravellingTransporters travellingTransporters in Find.WorldObjects.TravellingTransporters)
			{
				if (travellingTransporters.IsPlayerControlled)
				{
					ThingOwnerUtility.GetAllThingsRecursively(travellingTransporters, list, false, new Predicate<IThingHolder>(WealthWatcher.WealthItemsFilter));
					foreach (Thing t4 in list)
					{
						this.<PostOpen>g__CountThing|42_0(t4, ref CS$<>8__locals1);
					}
				}
			}
			List<Thing> list2 = new List<Thing>();
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				if (caravan.IsPlayerControlled)
				{
					ThingOwnerUtility.GetAllThingsRecursively(caravan, list2, false, new Predicate<IThingHolder>(WealthWatcher.WealthItemsFilter));
					foreach (Thing t2 in list2)
					{
						this.<PostOpen>g__CountThing|42_0(t2, ref CS$<>8__locals1);
					}
				}
			}
			List<Thing> list3 = new List<Thing>();
			foreach (Map holder in Find.Maps)
			{
				ThingOwnerUtility.GetAllThingsRecursively(holder, list3, false, new Predicate<IThingHolder>(WealthWatcher.WealthItemsFilter));
				foreach (Thing t3 in list3)
				{
					this.<PostOpen>g__CountThing|42_0(t3, ref CS$<>8__locals1);
				}
			}
			foreach (List<Thing> list4 in CS$<>8__locals1.itemDefMap.Values)
			{
				foreach (Thing item in list4)
				{
					this.items.Add(item);
				}
			}
			foreach (Thing item2 in CS$<>8__locals1.distinctItem)
			{
				this.items.Add(item2);
			}
			this.colonists.SortBy((Thing t) => t.Label);
			this.animals.SortBy((Thing t) => t.Label);
			this.relics.SortBy((Thing t) => t.Label);
			this.items = this.items.OrderByDescending((Thing i) => i.def, this.itemComparator).ThenBy((Thing i) => i.def.label).ThenByDescending(delegate(Thing i)
			{
				QualityCategory result;
				i.TryGetQuality(out result);
				return result;
			}).ToList<Thing>();
		}

		// Token: 0x06011B8F RID: 72591 RVA: 0x0051BF24 File Offset: 0x0051A124
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect.width, 40f), "ChooseThingsForNewColonyTitle".Translate());
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(0f, 40f, rect.width, 25f), "ChooseThingsForNewColonyDesc".Translate());
			float num = rect.width;
			Rect outRect = new Rect(rect.x, rect.y + 40f + 25f + 10f, rect.width, rect.height - 40f - 25f - 70f);
			float num2 = Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.colonists.Count) + Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.animals.Count) + Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.relics.Count) + Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.items.Count);
			if (num2 > outRect.height)
			{
				num -= 16f;
			}
			Rect rect2 = new Rect(0f, 0f, num, num2);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			if (this.colonists.Count > 0)
			{
				this.DrawThingList(this.colonists, rect2, "ChoosePeopleDesc".Translate(this.maxColonists), false);
				rect2.yMin += Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.colonists.Count);
			}
			if (this.animals.Count > 0)
			{
				this.DrawThingList(this.animals, rect2, "ChooseThingsDesc".Translate(this.maxAnimals, "AnimalsLower".Translate()), false);
				rect2.yMin += Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.animals.Count);
			}
			if (this.relics.Count > 0)
			{
				TaggedString value = (this.maxRelics == 1) ? "RelicLower".Translate() : "RelicsLower".Translate();
				this.DrawThingList(this.relics, rect2, "ChooseThingsDesc".Translate(this.maxRelics, value), false);
				rect2.yMin += Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.relics.Count);
			}
			if (this.items.Count > 0)
			{
				this.DrawThingList(this.items, rect2, "ChooseThingsDesc".Translate(this.maxItems, "ItemsLower".Translate()), true);
				rect2.yMin += Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.items.Count);
			}
			Widgets.EndScrollView();
			Rect rect3 = new Rect(0f, rect.yMax - 70f, rect.width, 70f);
			Rect rect4 = new Rect(rect3.xMax - this.BottomButtonSize.x, rect3.yMax - this.BottomButtonSize.y, this.BottomButtonSize.x, this.BottomButtonSize.y);
			AcceptanceReport acceptanceReport = this.AcceptanceReport;
			if (!acceptanceReport.Accepted)
			{
				Rect rect5 = rect4;
				rect5.x -= 162f;
				rect5.width = 160f;
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleRight;
				GUI.color = Color.red;
				Widgets.Label(rect5, acceptanceReport.Reason);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			int slaveCount = this.SlaveCount;
			bool flag = slaveCount > 0 && slaveCount == this.ColonistCount;
			if (acceptanceReport.Accepted && flag)
			{
				Rect rect6 = rect4;
				rect6.x -= 162f;
				rect6.width = 160f;
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleLeft;
				GUI.color = Color.yellow;
				Widgets.Label(rect6, "ChooseOnlySlavesInfo".Translate());
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			if (Widgets.ButtonText(rect4, "AcceptButton".Translate(), true, true, true, null))
			{
				if (acceptanceReport.Accepted)
				{
					this.ConfirmArchonexusSettlementConsequences(this.selected.ToList<Thing>(), flag);
				}
				else
				{
					Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
				}
			}
			Rect rect7 = rect4;
			rect7.x = rect3.x;
			if (Widgets.ButtonText(rect7, "Cancel".Translate(), true, true, true, null))
			{
				if (this.cancel != null)
				{
					this.cancel();
				}
				this.Close(true);
			}
		}

		// Token: 0x06011B90 RID: 72592 RVA: 0x0051C3F6 File Offset: 0x0051A5F6
		public override void OnCancelKeyPressed()
		{
			base.OnCancelKeyPressed();
			if (this.cancel != null)
			{
				this.cancel();
			}
		}

		// Token: 0x06011B91 RID: 72593 RVA: 0x0051C414 File Offset: 0x0051A614
		private void ConfirmArchonexusSettlementConsequences(List<Thing> selectedList, bool onlySlavesSelected)
		{
			Dialog_ChooseThingsForNewColony.<>c__DisplayClass45_0 CS$<>8__locals1 = new Dialog_ChooseThingsForNewColony.<>c__DisplayClass45_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.onlySlavesSelected = onlySlavesSelected;
			CS$<>8__locals1.selectedList = selectedList;
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			if (this.items.Count > this.selectedItemCount && this.selectedItemCount < this.maxItems)
			{
				if (flag)
				{
					stringBuilder.Append("\n\n");
				}
				flag = true;
				stringBuilder.Append("ArchonexusItemsNotChosen".Translate(this.selectedItemCount, Mathf.Min(this.items.Count, this.maxItems)).Colorize(ColoredText.WarningColor));
			}
			List<Thing> abandonedRelicsCarriedByPawns = MoveColonyUtility.GetAbandonedRelicsCarriedByPawns(CS$<>8__locals1.selectedList);
			if (this.relics.Count > this.RelicCount && this.RelicCount <= 0)
			{
				if (flag)
				{
					stringBuilder.Append("\n\n");
				}
				flag = true;
				stringBuilder.Append("ArchonexusRelicNotChosen".Translate().Colorize(ColoredText.WarningColor));
			}
			else if (abandonedRelicsCarriedByPawns.Count > 0)
			{
				if (flag)
				{
					stringBuilder.Append("\n\n");
				}
				flag = true;
				stringBuilder.Append("RemoveEquippedRelicsDescription".Translate().Colorize(ColoredText.WarningColor) + "\n\n" + (from r in abandonedRelicsCarriedByPawns
				select r.Label.Colorize(ColoredText.WarningColor)).ToLineList(" - ".Colorize(ColoredText.WarningColor), false));
			}
			CS$<>8__locals1.escapeShip = Find.WorldObjects.AllWorldObjects.FirstOrDefault((WorldObject wo) => wo.def == WorldObjectDefOf.EscapeShip);
			if (CS$<>8__locals1.escapeShip != null)
			{
				if (flag)
				{
					stringBuilder.Append("\n\n");
				}
				flag = true;
				stringBuilder.Append("ShipEscapeWillEndDescription".Translate());
			}
			if (flag)
			{
				stringBuilder.Append("\n\n");
			}
			stringBuilder.Append("WantToProceed".Translate());
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), new Action(CS$<>8__locals1.<ConfirmArchonexusSettlementConsequences>g__CloseAction|2), false, "ConfirmDecisionsTitle".Translate(), WindowLayer.Dialog));
		}

		// Token: 0x06011B92 RID: 72594 RVA: 0x0051C648 File Offset: 0x0051A848
		private void DrawThingList(List<Thing> things, Rect rect, string title, bool isItem)
		{
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 30f), title);
			Widgets.DrawLineHorizontal(rect.x, rect.y + 21f, Text.CalcSize(title).x);
			Text.Anchor = TextAnchor.UpperLeft;
			int count = things.Count;
			float num = rect.y + 30f;
			for (int i = 0; i < count; i++)
			{
				Rect rect2 = new Rect(rect.x, num + (float)i * 30f, rect.width, 30f);
				this.DoRow(rect2, things[i], i, isItem);
			}
		}

		// Token: 0x06011B93 RID: 72595 RVA: 0x0051C700 File Offset: 0x0051A900
		private void DoRow(Rect rect, Thing t, int index, bool isItem)
		{
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Widgets.BeginGroup(rect);
			float num = 4f;
			Pawn p = t as Pawn;
			string label = t.LabelCap;
			Pawn p3 = p;
			bool flag;
			if (p3 == null)
			{
				flag = false;
			}
			else
			{
				RaceProperties raceProps = p3.RaceProps;
				bool? flag2 = (raceProps != null) ? new bool?(raceProps.Animal) : null;
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				label = p.LabelCap + string.Format(" ({0}, {1})", p.GetGenderLabel(), Mathf.FloorToInt(p.ageTracker.AgeBiologicalYearsFloat));
			}
			else if (isItem)
			{
				label = GenLabel.ThingLabel(t, 1, false, true).CapitalizeFirst();
			}
			float num2 = isItem ? 250f : 196f;
			Rect rect2 = new Rect(0f, 0f, 80f + num2, rect.height);
			Color color = (p != null && p.IsSlave) ? PawnNameColorUtility.PawnNameColorOf(p) : Color.white;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegion(rect2, new TipSignal(delegate()
				{
					string text = label;
					string text2 = (p != null) ? (p.MainDesc(true, true) + "\n\n" + p.def.description) : t.DescriptionDetailed;
					if (!text2.NullOrEmpty())
					{
						text = text + ": " + text2 + TransferableUIUtility.ContentSourceDescription(t);
					}
					return text;
				}, t.GetHashCode()));
			}
			Widgets.InfoCardButton(0f, 0f, t);
			Widgets.ThingIcon(new Rect(36f, 0f, 27f, 27f), t, 1f, null, false, 1f, false);
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect3 = new Rect(80f, 0f, num2, rect2.height);
			Text.WordWrap = false;
			GUI.color = color;
			Widgets.Label(rect3, label);
			GUI.color = Color.white;
			Text.WordWrap = true;
			num += 320f;
			if (p != null && p.Ideo != null && !Find.IdeoManager.classicMode)
			{
				Rect rect4 = new Rect(num, 0f, 27f, rect.height);
				Widgets.DrawHighlightIfMouseover(rect4);
				p.Ideo.DrawIcon(rect4);
				TooltipHandler.TipRegion(rect4, p.Ideo.name);
			}
			else
			{
				Pawn p2 = p;
				bool flag4;
				if (p2 == null)
				{
					flag4 = false;
				}
				else
				{
					RaceProperties raceProps2 = p2.RaceProps;
					bool? flag2 = (raceProps2 != null) ? new bool?(raceProps2.Animal) : null;
					bool flag3 = true;
					flag4 = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag4)
				{
					if (p.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true))
					{
						TransferableUIUtility.DrawPregnancyIcon(p, new Rect(num, 2f, 23f, rect.height - 4f));
						num += 27f;
					}
					if (p.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null) != null)
					{
						TransferableUIUtility.DrawBondedIcon(p, new Rect(num, 2f, 23f, rect.height - 4f));
						num += 27f;
					}
				}
			}
			num += 27f;
			if (p != null && p.IsSlave)
			{
				Rect rect5 = new Rect(num, 0f, 27f, rect.height);
				Widgets.DrawHighlightIfMouseover(rect5);
				GUI.DrawTexture(rect5, p.guest.GetIcon());
				TooltipHandler.TipRegion(rect5, p.guest.GetLabel());
				num += 27f;
			}
			if (isItem)
			{
				Rect rect6 = new Rect(rect.width - 27f - 4f - 60f - 10f, 1f, 60f, rect.height);
				if (Mouse.IsOver(rect6))
				{
					Widgets.DrawHighlight(rect6);
					TooltipHandler.TipRegion(rect6, new TipSignal(() => "ItemQuantityTooltip".Translate(label, t.def.ArchonexusMaxAllowedCount), t.GetHashCode()));
				}
				GUI.color = Color.white;
				Text.WordWrap = false;
				TextAnchor anchor = Text.Anchor;
				Text.Anchor = TextAnchor.MiddleRight;
				int num3 = MoveColonyUtility.IsDistinctArchonexusItem(t.def) ? t.stackCount : this.itemArchonexusAllowedStackCount[t];
				Widgets.Label(rect6.LeftPartPixels(rect6.width - 10f), num3.ToString());
				Text.Anchor = anchor;
				Text.WordWrap = true;
			}
			Rect rect7 = new Rect(rect.width - 27f - 4f, 1f, 27f, 27f);
			Rect butRect = new Rect(rect7);
			butRect.xMin -= 6f;
			butRect.width += 12f;
			butRect.yMin -= 1.5f;
			butRect.height += 3f;
			if (Widgets.ButtonInvisible(butRect, true))
			{
				if (this.selected.Contains(t))
				{
					this.selected.Remove(t);
					if (isItem)
					{
						this.selectedItemCount--;
					}
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				else
				{
					this.selected.Add(t);
					if (isItem)
					{
						this.selectedItemCount++;
					}
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
			}
			GUI.DrawTexture(rect7.ContractedBy(1f), this.selected.Contains(t) ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
			GenUI.ResetLabelAlign();
			Widgets.EndGroup();
		}

		// Token: 0x06011B94 RID: 72596 RVA: 0x0051CD08 File Offset: 0x0051AF08
		[CompilerGenerated]
		private void <PostOpen>g__CountThing|42_0(Thing t, ref Dialog_ChooseThingsForNewColony.<>c__DisplayClass42_0 A_2)
		{
			if (t.def.ArchonexusMaxAllowedCount == 0)
			{
				return;
			}
			if (!MoveColonyUtility.IsBringableItem(t))
			{
				return;
			}
			if (!A_2.itemDefMap.ContainsKey(t.def))
			{
				A_2.itemDefMap[t.def] = new List<Thing>();
			}
			if (MoveColonyUtility.IsDistinctArchonexusItem(t.def))
			{
				A_2.distinctItem.Add(t);
				return;
			}
			foreach (Thing thing in A_2.itemDefMap[t.def])
			{
				if (t.CanStackWith(thing))
				{
					this.itemArchonexusAllowedStackCount[thing] = Mathf.Min(t.def.ArchonexusMaxAllowedCount, t.stackCount + this.itemArchonexusAllowedStackCount[thing]);
					return;
				}
			}
			this.itemArchonexusAllowedStackCount[t] = Mathf.Min(t.def.ArchonexusMaxAllowedCount, t.stackCount);
			A_2.itemDefMap[t.def].Add(t);
		}

		// Token: 0x06011B95 RID: 72597 RVA: 0x0051CE2C File Offset: 0x0051B02C
		[CompilerGenerated]
		internal static float <DoWindowContents>g__CalculateSectionHeight|43_0(int numberOfThings)
		{
			if (numberOfThings == 0)
			{
				return 0f;
			}
			return 40f + (float)numberOfThings * 30f;
		}

		// Token: 0x0400B6AC RID: 46764
		private int maxColonists;

		// Token: 0x0400B6AD RID: 46765
		private int maxAnimals;

		// Token: 0x0400B6AE RID: 46766
		private int maxRelics;

		// Token: 0x0400B6AF RID: 46767
		private int maxItems;

		// Token: 0x0400B6B0 RID: 46768
		private Action<List<Thing>> postAccepted;

		// Token: 0x0400B6B1 RID: 46769
		private List<Thing> colonists = new List<Thing>();

		// Token: 0x0400B6B2 RID: 46770
		private List<Thing> animals = new List<Thing>();

		// Token: 0x0400B6B3 RID: 46771
		private List<Thing> relics = new List<Thing>();

		// Token: 0x0400B6B4 RID: 46772
		private List<Thing> items = new List<Thing>();

		// Token: 0x0400B6B5 RID: 46773
		private Dictionary<Thing, int> itemArchonexusAllowedStackCount = new Dictionary<Thing, int>();

		// Token: 0x0400B6B6 RID: 46774
		private HashSet<Thing> selected = new HashSet<Thing>();

		// Token: 0x0400B6B7 RID: 46775
		private int selectedItemCount;

		// Token: 0x0400B6B8 RID: 46776
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x0400B6B9 RID: 46777
		private Action cancel;

		// Token: 0x0400B6BA RID: 46778
		private const float TitleHeight = 40f;

		// Token: 0x0400B6BB RID: 46779
		private const float DescriptionHeight = 25f;

		// Token: 0x0400B6BC RID: 46780
		private const float BottomAreaHeight = 70f;

		// Token: 0x0400B6BD RID: 46781
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400B6BE RID: 46782
		private const float RowHeight = 30f;

		// Token: 0x0400B6BF RID: 46783
		private const float CategoryHeaderHeight = 30f;

		// Token: 0x0400B6C0 RID: 46784
		private const float MiscIconSize = 27f;

		// Token: 0x0400B6C1 RID: 46785
		private const float CheckboxSize = 27f;

		// Token: 0x0400B6C2 RID: 46786
		private const float ItemCountSize = 60f;

		// Token: 0x0400B6C3 RID: 46787
		private const float ErrorAndWarningTextWidth = 160f;

		// Token: 0x0400B6C4 RID: 46788
		private const float MiscIconOffset = 320f;

		// Token: 0x0400B6C5 RID: 46789
		private const float NameOffset = 80f;

		// Token: 0x0400B6C6 RID: 46790
		private const float ItemNameWidth = 250f;

		// Token: 0x0400B6C7 RID: 46791
		private const float ThingNameWidth = 196f;

		// Token: 0x0400B6C8 RID: 46792
		private readonly Comparer<ThingDef> itemComparator = Comparer<ThingDef>.Create((ThingDef td1, ThingDef td2) => TransferableComparer_Archonexus.Compare(td1, td2));
	}
}
