using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200266A RID: 9834
	public class Precept_Relic : Precept_ThingStyle
	{
		// Token: 0x1700210A RID: 8458
		// (get) Token: 0x0600D7BA RID: 55226 RVA: 0x003FF7F7 File Offset: 0x003FD9F7
		public override string TipLabel
		{
			get
			{
				if (this.tipLabelCached == null)
				{
					this.tipLabelCached = base.LabelCap + "\n" + base.ThingDef.LabelCap;
				}
				return this.tipLabelCached;
			}
		}

		// Token: 0x1700210B RID: 8459
		// (get) Token: 0x0600D7BB RID: 55227 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanRegenerate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700210C RID: 8460
		// (get) Token: 0x0600D7BC RID: 55228 RVA: 0x003FF832 File Offset: 0x003FDA32
		public bool CanGenerateRelic
		{
			get
			{
				return !this.relicGenerated;
			}
		}

		// Token: 0x1700210D RID: 8461
		// (get) Token: 0x0600D7BD RID: 55229 RVA: 0x003FF83D File Offset: 0x003FDA3D
		public Thing GeneratedRelic
		{
			get
			{
				return this.generatedRelic;
			}
		}

		// Token: 0x1700210E RID: 8462
		// (get) Token: 0x0600D7BE RID: 55230 RVA: 0x003FF845 File Offset: 0x003FDA45
		protected override string ThingLabelCap
		{
			get
			{
				return Find.ActiveLanguageWorker.PostProcessThingLabelForRelic(base.ThingLabelCap);
			}
		}

		// Token: 0x1700210F RID: 8463
		// (get) Token: 0x0600D7BF RID: 55231 RVA: 0x003FF857 File Offset: 0x003FDA57
		public override string DescriptionForTip
		{
			get
			{
				ThingDef thingDef = base.ThingDef;
				return ((thingDef != null) ? thingDef.description : null) ?? base.Description;
			}
		}

		// Token: 0x17002110 RID: 8464
		// (get) Token: 0x0600D7C0 RID: 55232 RVA: 0x003FF878 File Offset: 0x003FDA78
		public bool RelicInPlayerPossession
		{
			get
			{
				Thing thing = this.GeneratedRelic;
				return thing != null && !thing.Destroyed && thing.EverSeenByPlayer && (thing.MapHeld != null || ThingOwnerUtility.AnyParentIs<Caravan>(thing) || ThingOwnerUtility.AnyParentIs<TravellingTransporters>(thing));
			}
		}

		// Token: 0x17002111 RID: 8465
		// (get) Token: 0x0600D7C1 RID: 55233 RVA: 0x003FF8BA File Offset: 0x003FDABA
		protected override string NameRootSymbol
		{
			get
			{
				return "r_ideoRelicName";
			}
		}

		// Token: 0x0600D7C2 RID: 55234 RVA: 0x003FF8C4 File Offset: 0x003FDAC4
		public static ThingDef GenerateStuffFor(ThingDef thing, Ideo ideo = null)
		{
			IEnumerable<ThingDef> source = GenStuff.AllowedStuffsFor(thing, TechLevel.Undefined, false);
			if (ideo != null)
			{
				IEnumerable<ThingDef> alreadyUsedStuffs = (from p in ideo.PreceptsListForReading.Where(delegate(Precept p)
				{
					Precept_Relic precept_Relic = p as Precept_Relic;
					return precept_Relic != null && precept_Relic.ThingDef == thing;
				})
				select ((Precept_Relic)p).stuff into p
				where p != null
				select p).Distinct<ThingDef>();
				source = from stuff in source
				where !alreadyUsedStuffs.Contains(stuff)
				select stuff;
			}
			return source.RandomElementByWeight((ThingDef stuff) => stuff.BaseMarketValue);
		}

		// Token: 0x0600D7C3 RID: 55235 RVA: 0x003FF99A File Offset: 0x003FDB9A
		public void SetRandomStuff()
		{
			if (base.ThingDef.MadeFromStuff)
			{
				this.stuff = Precept_Relic.GenerateStuffFor(base.ThingDef, this.ideo);
				return;
			}
			this.stuff = null;
		}

		// Token: 0x0600D7C4 RID: 55236 RVA: 0x003FF9C8 File Offset: 0x003FDBC8
		protected override void Notify_ThingDefSet()
		{
			base.Notify_ThingDefSet();
			this.SetRandomStuff();
		}

		// Token: 0x0600D7C5 RID: 55237 RVA: 0x003FF9D8 File Offset: 0x003FDBD8
		public override string GenerateNameRaw()
		{
			CompProperties_GeneratedName compProperties = base.ThingDef.GetCompProperties<CompProperties_GeneratedName>();
			if (compProperties != null)
			{
				return CompGeneratedNames.GenerateName(compProperties);
			}
			return base.GenerateNameRaw();
		}

		// Token: 0x0600D7C6 RID: 55238 RVA: 0x003FFA04 File Offset: 0x003FDC04
		public Thing GenerateRelic()
		{
			if (this.stuff == null && base.ThingDef.MadeFromStuff)
			{
				Log.Warning("Tried generating relic with stuff, but precept had no stuff set? If this is an old savegame from testing this warning is to be expected.");
				this.stuff = Precept_Relic.GenerateStuffFor(base.ThingDef, this.ideo);
			}
			Thing thing;
			if (base.ThingDef.CompDefFor<CompQuality>() != null)
			{
				ThingStuffPairWithQuality thingStuffPairWithQuality = new ThingStuffPairWithQuality(base.ThingDef, this.stuff, QualityCategory.Legendary);
				thing = thingStuffPairWithQuality.MakeThing(true);
			}
			else
			{
				thing = ThingMaker.MakeThing(base.ThingDef, this.stuff);
			}
			thing.StyleSourcePrecept = this;
			this.relicGenerated = true;
			this.generatedRelic = thing;
			return thing;
		}

		// Token: 0x0600D7C7 RID: 55239 RVA: 0x003FFA9B File Offset: 0x003FDC9B
		public override void ClearTipCache()
		{
			base.ClearTipCache();
			this.tipLabelCached = null;
		}

		// Token: 0x0600D7C8 RID: 55240 RVA: 0x003FFAAA File Offset: 0x003FDCAA
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			yield return base.EditFloatMenuOption();
			yield break;
		}

		// Token: 0x0600D7C9 RID: 55241 RVA: 0x003FFABA File Offset: 0x003FDCBA
		public override string GetTip()
		{
			return base.GetTip() + "\n\n" + (this.ideo.classicMode ? "RelicTipClassic" : "RelicTip").Translate().Colorize(Color.grey);
		}

		// Token: 0x0600D7CA RID: 55242 RVA: 0x003FFAF4 File Offset: 0x003FDCF4
		public override void DrawIcon(Rect rect)
		{
			Widgets.DefIcon(rect, base.ThingDef, this.stuff, 1f, this.ideo.GetStyleFor(base.ThingDef), false, null, null, null, 1f);
		}

		// Token: 0x0600D7CB RID: 55243 RVA: 0x003FFB44 File Offset: 0x003FDD44
		public override void Notify_ThingLost(Thing thing, bool destroyed = false)
		{
			if (thing.def == base.ThingDef && !QuestPart_NewColony.IsGeneratingNewColony)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(destroyed ? HistoryEventDefOf.RelicDestroyed : HistoryEventDefOf.RelicLost, thing.Named("SUBJECT")), true);
				List<Faction> list = new List<Faction>();
				HashSet<Pawn> hashSet = new HashSet<Pawn>();
				if (thing.EverSeenByPlayer)
				{
					list.Add(Faction.OfPlayer);
				}
				if (thing.MapHeld != null)
				{
					foreach (Pawn pawn in thing.MapHeld.mapPawns.AllPawnsSpawned)
					{
						if (!pawn.Dead && pawn.Faction != null && !list.Contains(pawn.Faction))
						{
							list.Add(pawn.Faction);
						}
					}
				}
				foreach (Pawn pawn2 in PawnsFinder.AllMapsAndWorld_Alive)
				{
					if (pawn2.needs != null && pawn2.needs.mood != null && pawn2.needs.mood.thoughts != null && pawn2.needs.mood.thoughts.memories != null && !hashSet.Contains(pawn2) && list.Contains(pawn2.Faction))
					{
						pawn2.needs.mood.thoughts.memories.TryGainMemory(destroyed ? ThoughtDefOf.RelicDestroyed : ThoughtDefOf.RelicLost, null, null);
						if (pawn2.Faction == Faction.OfPlayer)
						{
							hashSet.Add(pawn2);
						}
					}
				}
				if (list.Contains(Faction.OfPlayer) && (destroyed || thing.MapHeld == null || !thing.MapHeld.IsPlayerHome))
				{
					TaggedString label = (destroyed ? "LetterLabelRelicDestroyed" : "LetterLabelRelicLost").Translate() + ": " + base.LabelCap;
					TaggedString text = (destroyed ? "LetterTextRelicDestroyed" : "LetterTextRelicLost").Translate(base.LabelCap, (from p in hashSet
					select p.LabelNoCountColored.Resolve()).ToList<string>().ToLineList("- "));
					Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, new LookTargets(from p in hashSet
					where p.Faction == Faction.OfPlayer
					select p), null, null, null, null, 0, true);
				}
			}
		}

		// Token: 0x0600D7CC RID: 55244 RVA: 0x003FFE04 File Offset: 0x003FE004
		public void Notify_NewColonyStarted()
		{
			if (this.relicGenerated && !this.RelicInPlayerPossession)
			{
				this.relicGenerated = false;
			}
		}

		// Token: 0x0600D7CD RID: 55245 RVA: 0x003FFE1D File Offset: 0x003FE01D
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
		{
			if (this.ideo.classicMode)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelicStatus".Translate(), "Relic".Translate().CapitalizeFirst(), "Stat_Thing_RelicStatus_Desc".Translate(), 1109, null, null, false, false);
			}
			else
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelicOf_Name".Translate(), this.ideo.name, "Stat_Thing_RelicOf_Desc".Translate(), 1109, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(this.ideo)
				}, false, false);
			}
			yield break;
		}

		// Token: 0x0600D7CE RID: 55246 RVA: 0x003FFE2D File Offset: 0x003FE02D
		public override string TransformThingLabel(string label)
		{
			return this.name + ", " + "Relic".Translate();
		}

		// Token: 0x0600D7CF RID: 55247 RVA: 0x003FFE54 File Offset: 0x003FE054
		public override string InspectStringExtra(Thing thing)
		{
			if (this.ideo.classicMode)
			{
				return "Relic".Translate().CapitalizeFirst();
			}
			return "RelicOf".Translate(this.ideo.Named("IDEO")).Resolve();
		}

		// Token: 0x0600D7D0 RID: 55248 RVA: 0x003FFEA8 File Offset: 0x003FE0A8
		private IEnumerable<Quest> GetQuestsToDeactivateIfLost()
		{
			List<Quest> allQuests = Find.QuestManager.QuestsListForReading;
			int num;
			for (int i = 0; i < allQuests.Count; i = num + 1)
			{
				if (!allQuests[i].Historical)
				{
					List<QuestPart> partsListForReading = allQuests[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_SubquestGenerator_RelicHunt questPart_SubquestGenerator_RelicHunt = partsListForReading[j] as QuestPart_SubquestGenerator_RelicHunt;
						if (questPart_SubquestGenerator_RelicHunt != null && questPart_SubquestGenerator_RelicHunt.relic == this)
						{
							yield return allQuests[i];
							break;
						}
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600D7D1 RID: 55249 RVA: 0x003FFEB8 File Offset: 0x003FE0B8
		public override bool TryGetLostByReformingWarning(out string warning)
		{
			warning = null;
			IEnumerable<Quest> questsToDeactivateIfLost = this.GetQuestsToDeactivateIfLost();
			if (questsToDeactivateIfLost.Any<Quest>())
			{
				foreach (Quest quest in questsToDeactivateIfLost)
				{
					if (!warning.NullOrEmpty())
					{
						warning += "\n\n";
					}
					warning += "ReformIdeoRelicHuntQuestEnd".Translate(base.LabelCap, quest.name);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600D7D2 RID: 55250 RVA: 0x003FFF54 File Offset: 0x003FE154
		public override void Notify_RemovedByReforming()
		{
			foreach (Quest quest in this.GetQuestsToDeactivateIfLost())
			{
				quest.hidden = true;
				quest.End(QuestEndOutcome.Unknown, false, true);
			}
		}

		// Token: 0x0600D7D3 RID: 55251 RVA: 0x003FFFA8 File Offset: 0x003FE1A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			if (!GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
			{
				if (Scribe.mode == LoadSaveMode.Saving && this.relicGenerated && this.generatedRelic != null && (this.generatedRelic.Destroyed || this.generatedRelic.Discarded))
				{
					this.relicGenerated = false;
					this.generatedRelic = null;
				}
				Scribe_Values.Look<bool>(ref this.relicGenerated, "everGenerated", false, false);
				Scribe_References.Look<Thing>(ref this.generatedRelic, "generatedRelic", false);
			}
		}

		// Token: 0x0600D7D4 RID: 55252 RVA: 0x00400035 File Offset: 0x003FE235
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			Precept_Relic precept_Relic = (Precept_Relic)other;
			precept_Relic.stuff = this.stuff;
			precept_Relic.relicGenerated = this.relicGenerated;
			precept_Relic.generatedRelic = this.generatedRelic;
		}

		// Token: 0x0600D7D5 RID: 55253 RVA: 0x00400067 File Offset: 0x003FE267
		public override string ToString()
		{
			return "Relic - " + base.LabelCap;
		}

		// Token: 0x04009257 RID: 37463
		private static List<ThingDef> usedThingsTmp = new List<ThingDef>();

		// Token: 0x04009258 RID: 37464
		public ThingDef stuff;

		// Token: 0x04009259 RID: 37465
		private bool relicGenerated;

		// Token: 0x0400925A RID: 37466
		private Thing generatedRelic;

		// Token: 0x0400925B RID: 37467
		private string tipLabelCached;
	}
}
