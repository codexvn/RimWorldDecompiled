using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C19 RID: 3097
	public class Dialog_InfoCard : Window
	{
		// Token: 0x06004F86 RID: 20358 RVA: 0x00198939 File Offset: 0x00196B39
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<ThingDef> defs)
		{
			if (defs == null)
			{
				return null;
			}
			return from def in defs
			select new Dialog_InfoCard.Hyperlink(def, -1);
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x00198965 File Offset: 0x00196B65
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink(link.def, -1);
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x00198991 File Offset: 0x00196B91
		public static IEnumerable<Dialog_InfoCard.Hyperlink> TitleDefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink((RoyalTitleDef)link.def, link.faction, -1);
		}

		// Token: 0x06004F89 RID: 20361 RVA: 0x001989C0 File Offset: 0x00196BC0
		public static void PushCurrentToHistoryAndClose()
		{
			Dialog_InfoCard dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
			if (dialog_InfoCard == null)
			{
				return;
			}
			Dialog_InfoCard.history.Add(new Dialog_InfoCard.Hyperlink(dialog_InfoCard, StatsReportUtility.SelectedStatIndex));
			Find.WindowStack.TryRemove(dialog_InfoCard, false);
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06004F8A RID: 20362 RVA: 0x001989FE File Offset: 0x00196BFE
		private Def Def
		{
			get
			{
				if (this.thing != null)
				{
					return this.thing.def;
				}
				if (this.worldObject != null)
				{
					return this.worldObject.def;
				}
				return this.def;
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06004F8B RID: 20363 RVA: 0x00198A2E File Offset: 0x00196C2E
		private Pawn ThingPawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06004F8C RID: 20364 RVA: 0x00198A3B File Offset: 0x00196C3B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 760f);
			}
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06004F8D RID: 20365 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06004F8E RID: 20366 RVA: 0x00198A4C File Offset: 0x00196C4C
		public override QuickSearchWidget CommonSearchWidget
		{
			get
			{
				if (this.tab != Dialog_InfoCard.InfoCardTab.Stats)
				{
					return null;
				}
				return StatsReportUtility.QuickSearchWidget;
			}
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x00198A5D File Offset: 0x00196C5D
		public Dialog_InfoCard(Thing thing, Precept_ThingStyle precept = null) : base(null)
		{
			this.thing = thing;
			this.precept = precept;
			this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			this.Setup();
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x00198A81 File Offset: 0x00196C81
		public Dialog_InfoCard(Def onlyDef, Precept_ThingStyle precept = null) : base(null)
		{
			this.def = onlyDef;
			this.precept = precept;
			this.Setup();
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x00198A9E File Offset: 0x00196C9E
		public Dialog_InfoCard(ThingDef thingDef, ThingDef stuff, Precept_ThingStyle precept = null) : base(null)
		{
			this.def = thingDef;
			this.stuff = stuff;
			this.precept = precept;
			this.Setup();
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x00198AC2 File Offset: 0x00196CC2
		public Dialog_InfoCard(RoyalTitleDef titleDef, Faction faction, Pawn pawn = null) : base(null)
		{
			this.titleDef = titleDef;
			this.faction = faction;
			this.pawn = pawn;
			this.Setup();
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x00198AE6 File Offset: 0x00196CE6
		public Dialog_InfoCard(Hediff hediff) : base(null)
		{
			this.hediff = hediff;
			this.Setup();
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x00198AFC File Offset: 0x00196CFC
		public Dialog_InfoCard(Faction faction) : base(null)
		{
			this.faction = faction;
			this.Setup();
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x00198B12 File Offset: 0x00196D12
		public Dialog_InfoCard(WorldObject worldObject) : base(null)
		{
			this.worldObject = worldObject;
			this.Setup();
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x00198B28 File Offset: 0x00196D28
		public override void Notify_CommonSearchChanged()
		{
			StatsReportUtility.Notify_QuickSearchChanged();
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x00198B2F File Offset: 0x00196D2F
		public override void Close(bool doCloseSound = true)
		{
			base.Close(doCloseSound);
			Dialog_InfoCard.history.Clear();
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x00198B44 File Offset: 0x00196D44
		private void Setup()
		{
			this.forcePause = true;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this.soundAppear = SoundDefOf.InfoCard_Open;
			this.soundClose = SoundDefOf.InfoCard_Close;
			StatsReportUtility.Reset();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InfoCard, KnowledgeAmount.Total);
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x00198B9A File Offset: 0x00196D9A
		public void SetTab(Dialog_InfoCard.InfoCardTab infoCardTab)
		{
			this.tab = infoCardTab;
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x00198BA4 File Offset: 0x00196DA4
		private static bool ShowMaterialsButton(Rect containerRect, bool withBackButtonOffset = false)
		{
			float num = containerRect.x + containerRect.width - 14f - 200f - 16f;
			if (withBackButtonOffset)
			{
				num -= 136f;
			}
			return Widgets.ButtonText(new Rect(num, containerRect.y + 18f, 200f, 40f), "ShowMaterials".Translate(), true, true, true, null);
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x00198C1C File Offset: 0x00196E1C
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect);
			rect = rect.ContractedBy(18f);
			rect.height = 34f;
			rect.x += 34f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.GetTitle());
			Rect rect2 = new Rect(inRect.x + 9f, rect.y, 34f, 34f);
			if (this.thing != null)
			{
				Widgets.ThingIcon(rect2, this.thing, 1f, null, false, 1f, false);
			}
			else
			{
				Widgets.DefIcon(rect2, this.def, this.stuff, 1f, null, true, null, null, null, 1f);
			}
			Rect rect3 = new Rect(inRect);
			rect3.yMin = rect.yMax;
			rect3.yMax -= 38f;
			Rect rect4 = rect3;
			List<TabRecord> list = new List<TabRecord>();
			TabRecord item = new TabRecord("TabStats".Translate(), delegate()
			{
				this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			}, this.tab == Dialog_InfoCard.InfoCardTab.Stats);
			list.Add(item);
			if (this.ThingPawn != null)
			{
				if (this.ThingPawn.RaceProps.Humanlike)
				{
					TabRecord item2 = new TabRecord("TabCharacter".Translate(), delegate()
					{
						this.tab = Dialog_InfoCard.InfoCardTab.Character;
					}, this.tab == Dialog_InfoCard.InfoCardTab.Character);
					list.Add(item2);
				}
				TabRecord item3 = new TabRecord("TabHealth".Translate(), delegate()
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Health;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Health);
				list.Add(item3);
				if (ModsConfig.RoyaltyActive && this.ThingPawn.RaceProps.Humanlike && this.ThingPawn.Faction == Faction.OfPlayer && !this.ThingPawn.IsQuestLodger() && this.ThingPawn.royalty != null && PermitsCardUtility.selectedFaction != null)
				{
					TabRecord item4 = new TabRecord("TabPermits".Translate(), delegate()
					{
						this.tab = Dialog_InfoCard.InfoCardTab.Permits;
					}, this.tab == Dialog_InfoCard.InfoCardTab.Permits);
					list.Add(item4);
				}
				TabRecord item5 = new TabRecord("TabRecords".Translate(), delegate()
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Records;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Records);
				list.Add(item5);
			}
			if (list.Count > 1)
			{
				rect4.yMin += 45f;
				TabDrawer.DrawTabs<TabRecord>(rect4, list, 200f);
			}
			this.FillCard(rect4.ContractedBy(18f));
			if (this.def != null && this.def is BuildableDef)
			{
				IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor((BuildableDef)this.def, TechLevel.Undefined, false);
				if (enumerable.Count<ThingDef>() > 0 && Dialog_InfoCard.ShowMaterialsButton(inRect, Dialog_InfoCard.history.Count > 0))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (ThingDef thingDef in enumerable)
					{
						ThingDef localStuff = thingDef;
						list2.Add(new FloatMenuOption(thingDef.LabelCap, delegate()
						{
							this.stuff = localStuff;
							this.Setup();
						}, thingDef, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			if (Dialog_InfoCard.history.Count > 0 && Widgets.BackButtonFor(inRect))
			{
				Dialog_InfoCard.Hyperlink hyperlink = Dialog_InfoCard.history[Dialog_InfoCard.history.Count - 1];
				Dialog_InfoCard.history.RemoveAt(Dialog_InfoCard.history.Count - 1);
				Find.WindowStack.TryRemove(typeof(Dialog_InfoCard), false);
				hyperlink.ActivateHyperlink();
			}
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x0019902C File Offset: 0x0019722C
		protected void FillCard(Rect cardRect)
		{
			if (this.tab == Dialog_InfoCard.InfoCardTab.Stats)
			{
				if (this.thing != null)
				{
					Thing innerThing = this.thing;
					MinifiedThing minifiedThing = this.thing as MinifiedThing;
					if (minifiedThing != null)
					{
						innerThing = minifiedThing.InnerThing;
					}
					StatsReportUtility.DrawStatsReport(cardRect, innerThing);
				}
				else if (this.titleDef != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.titleDef, this.faction, this.pawn);
				}
				else if (this.hediff != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.hediff);
				}
				else if (this.faction != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.faction);
				}
				else if (this.worldObject != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.worldObject);
				}
				else if (this.def is AbilityDef)
				{
					StatsReportUtility.DrawStatsReport(cardRect, (AbilityDef)this.def);
				}
				else
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.def, this.stuff);
				}
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Character)
			{
				CharacterCardUtility.DrawCharacterCard(cardRect, (Pawn)this.thing, null, default(Rect), true);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Health)
			{
				HealthCardUtility.DrawPawnHealthCard(cardRect, (Pawn)this.thing, false, HealthCardUtility.ShowBloodLoss(this.thing), null);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Records)
			{
				RecordsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Permits)
			{
				PermitsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
			if (this.executeAfterFillCardOnce != null)
			{
				this.executeAfterFillCardOnce();
				this.executeAfterFillCardOnce = null;
			}
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x001991C0 File Offset: 0x001973C0
		private string GetTitle()
		{
			if (this.thing != null)
			{
				if (this.precept == null)
				{
					return this.thing.LabelCapNoCount;
				}
				return this.precept.LabelCap;
			}
			else
			{
				if (this.worldObject != null)
				{
					return this.worldObject.LabelCap;
				}
				if (this.hediff != null)
				{
					return this.hediff.def.LabelCap;
				}
				ThingDef thingDef = this.Def as ThingDef;
				if (thingDef != null)
				{
					if (this.precept == null)
					{
						return GenLabel.ThingLabel(thingDef, this.stuff, 1).CapitalizeFirst();
					}
					return this.precept.LabelCap;
				}
				else
				{
					AbilityDef abilityDef = this.Def as AbilityDef;
					if (abilityDef != null)
					{
						return abilityDef.LabelCap;
					}
					if (this.titleDef != null)
					{
						return this.titleDef.GetLabelCapForBothGenders();
					}
					if (this.faction != null)
					{
						return this.faction.Name;
					}
					if (this.precept == null)
					{
						return this.Def.LabelCap;
					}
					return this.precept.LabelCap;
				}
			}
		}

		// Token: 0x0400390C RID: 14604
		private const float ShowMaterialsButtonWidth = 200f;

		// Token: 0x0400390D RID: 14605
		private const float ShowMaterialsButtonHeight = 40f;

		// Token: 0x0400390E RID: 14606
		private const float ShowMaterialsMargin = 16f;

		// Token: 0x0400390F RID: 14607
		private Action executeAfterFillCardOnce;

		// Token: 0x04003910 RID: 14608
		private static List<Dialog_InfoCard.Hyperlink> history = new List<Dialog_InfoCard.Hyperlink>();

		// Token: 0x04003911 RID: 14609
		private Thing thing;

		// Token: 0x04003912 RID: 14610
		private ThingDef stuff;

		// Token: 0x04003913 RID: 14611
		private Precept_ThingStyle precept;

		// Token: 0x04003914 RID: 14612
		private Def def;

		// Token: 0x04003915 RID: 14613
		private WorldObject worldObject;

		// Token: 0x04003916 RID: 14614
		private RoyalTitleDef titleDef;

		// Token: 0x04003917 RID: 14615
		private Faction faction;

		// Token: 0x04003918 RID: 14616
		private Pawn pawn;

		// Token: 0x04003919 RID: 14617
		private Hediff hediff;

		// Token: 0x0400391A RID: 14618
		private Dialog_InfoCard.InfoCardTab tab;

		// Token: 0x02000C1A RID: 3098
		public enum InfoCardTab : byte
		{
			// Token: 0x0400391C RID: 14620
			Stats,
			// Token: 0x0400391D RID: 14621
			Character,
			// Token: 0x0400391E RID: 14622
			Health,
			// Token: 0x0400391F RID: 14623
			Records,
			// Token: 0x04003920 RID: 14624
			Permits
		}

		// Token: 0x02000C1B RID: 3099
		public struct Hyperlink
		{
			// Token: 0x17000C1C RID: 3100
			// (get) Token: 0x06004FA4 RID: 20388 RVA: 0x001992FD File Offset: 0x001974FD
			public bool HasGeneOwnerThing
			{
				get
				{
					return this.thingIsGeneOwner && this.thing != null;
				}
			}

			// Token: 0x17000C1D RID: 3101
			// (get) Token: 0x06004FA5 RID: 20389 RVA: 0x00199314 File Offset: 0x00197514
			public bool IsHidden
			{
				get
				{
					if (this.thing != null && Find.HiddenItemsManager.Hidden(this.thing.def))
					{
						return true;
					}
					ThingDef thingDef = this.def as ThingDef;
					return thingDef != null && Find.HiddenItemsManager.Hidden(thingDef);
				}
			}

			// Token: 0x17000C1E RID: 3102
			// (get) Token: 0x06004FA6 RID: 20390 RVA: 0x00199364 File Offset: 0x00197564
			public string Label
			{
				get
				{
					string result = null;
					if (this.IsHidden)
					{
						result = string.Format("({0})", "NotYetDiscovered".Translate());
					}
					else if (this.worldObject != null)
					{
						result = this.worldObject.Label;
					}
					else
					{
						if (this.def != null)
						{
							ThingDef thingDef = this.def as ThingDef;
							if (thingDef != null && this.stuff != null)
							{
								return thingDef.label;
							}
						}
						if (this.def != null)
						{
							result = this.def.label;
						}
						else if (this.thing != null && !this.thingIsGeneOwner)
						{
							result = this.thing.Label;
						}
						else if (this.titleDef != null)
						{
							result = this.titleDef.GetLabelCapForBothGenders();
						}
						else if (this.quest != null)
						{
							result = this.quest.name;
						}
						else if (this.ideo != null)
						{
							result = this.ideo.name;
						}
						else if (this.researchProject != null)
						{
							result = this.researchProject.label;
						}
						else if (this.faction != null)
						{
							result = this.faction.Name;
						}
						else if (this.HasGeneOwnerThing)
						{
							result = "InspectGenes".Translate();
						}
					}
					return result;
				}
			}

			// Token: 0x06004FA7 RID: 20391 RVA: 0x001994A0 File Offset: 0x001976A0
			public Hyperlink(Dialog_InfoCard infoCard, int statIndex = -1)
			{
				this.def = infoCard.def;
				this.thing = infoCard.thing;
				this.stuff = infoCard.stuff;
				this.worldObject = infoCard.worldObject;
				this.titleDef = infoCard.titleDef;
				this.faction = infoCard.faction;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06004FA8 RID: 20392 RVA: 0x00199518 File Offset: 0x00197718
			public Hyperlink(Def def, int statIndex = -1)
			{
				this.def = def;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06004FA9 RID: 20393 RVA: 0x00199574 File Offset: 0x00197774
			public Hyperlink(RoyalTitleDef titleDef, Faction faction, int statIndex = -1)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = titleDef;
				this.faction = faction;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06004FAA RID: 20394 RVA: 0x001995D0 File Offset: 0x001977D0
			public Hyperlink(Thing thing, int statIndex = -1, bool thingIsGeneOwner = false)
			{
				this.thing = thing;
				this.stuff = null;
				this.def = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = thingIsGeneOwner;
			}

			// Token: 0x06004FAB RID: 20395 RVA: 0x0019962C File Offset: 0x0019782C
			public Hyperlink(Faction faction, int statIndex = -1, bool thingIsGeneOwner = false)
			{
				this.thing = null;
				this.stuff = null;
				this.def = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = faction;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = thingIsGeneOwner;
			}

			// Token: 0x06004FAC RID: 20396 RVA: 0x00199688 File Offset: 0x00197888
			public Hyperlink(Quest quest, int statIndex = -1)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = quest;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06004FAD RID: 20397 RVA: 0x001996E4 File Offset: 0x001978E4
			public Hyperlink(Ideo ideo)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = 0;
				this.quest = null;
				this.ideo = ideo;
				this.researchProject = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06004FAE RID: 20398 RVA: 0x00199740 File Offset: 0x00197940
			public Hyperlink(ResearchProjectDef researchProject)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = 0;
				this.quest = null;
				this.ideo = null;
				this.researchProject = researchProject;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06004FAF RID: 20399 RVA: 0x0019979C File Offset: 0x0019799C
			public void ActivateHyperlink()
			{
				if (this.IsHidden)
				{
					return;
				}
				if (this.ideo != null)
				{
					Dialog_InfoCard dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
					if (dialog_InfoCard != null)
					{
						dialog_InfoCard.Close(true);
					}
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos, true);
					IdeoUIUtility.SetSelected(this.ideo);
					return;
				}
				if (this.quest != null)
				{
					Dialog_InfoCard dialog_InfoCard2 = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
					if (dialog_InfoCard2 != null)
					{
						dialog_InfoCard2.Close(true);
					}
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					return;
				}
				if (this.researchProject != null)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
					((MainTabWindow_Research)MainButtonDefOf.Research.TabWindow).Select(this.researchProject);
				}
				if (this.HasGeneOwnerThing)
				{
					if (ThingSelectionUtility.SelectableByMapClick(this.thing))
					{
						Find.Selector.Select(this.thing, true, true);
						InspectPaneUtility.OpenTab(typeof(ITab_Genes));
					}
					return;
				}
				Dialog_InfoCard dialog_InfoCard3 = null;
				if (this.def == null && this.thing == null && this.worldObject == null && this.titleDef == null && this.faction == null)
				{
					dialog_InfoCard3 = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
				}
				else
				{
					Dialog_InfoCard.PushCurrentToHistoryAndClose();
					if (this.worldObject != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.worldObject);
					}
					else if (this.def != null && this.def is ThingDef && (this.stuff != null || GenStuff.DefaultStuffFor((ThingDef)this.def) != null))
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.def as ThingDef, this.stuff ?? GenStuff.DefaultStuffFor((ThingDef)this.def), null);
					}
					else if (this.def != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.def, null);
					}
					else if (this.thing != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.thing, null);
					}
					else if (this.titleDef != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.titleDef, this.faction, null);
					}
					else if (this.faction != null)
					{
						Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
						((MainTabWindow_Factions)Find.MainTabsRoot.OpenTab.TabWindow).ScrollToFaction(this.faction);
						dialog_InfoCard3 = new Dialog_InfoCard(this.faction);
					}
				}
				if (dialog_InfoCard3 == null)
				{
					return;
				}
				int localSelectedStatIndex = this.selectedStatIndex;
				if (this.selectedStatIndex >= 0)
				{
					dialog_InfoCard3.executeAfterFillCardOnce = delegate()
					{
						StatsReportUtility.SelectEntry(localSelectedStatIndex);
					};
				}
				Find.WindowStack.Add(dialog_InfoCard3);
			}

			// Token: 0x04003921 RID: 14625
			public Thing thing;

			// Token: 0x04003922 RID: 14626
			public ThingDef stuff;

			// Token: 0x04003923 RID: 14627
			public Def def;

			// Token: 0x04003924 RID: 14628
			public WorldObject worldObject;

			// Token: 0x04003925 RID: 14629
			public RoyalTitleDef titleDef;

			// Token: 0x04003926 RID: 14630
			public Faction faction;

			// Token: 0x04003927 RID: 14631
			public Quest quest;

			// Token: 0x04003928 RID: 14632
			public Ideo ideo;

			// Token: 0x04003929 RID: 14633
			public ResearchProjectDef researchProject;

			// Token: 0x0400392A RID: 14634
			public int selectedStatIndex;

			// Token: 0x0400392B RID: 14635
			public bool thingIsGeneOwner;
		}
	}
}
