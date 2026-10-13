using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B4C RID: 6988
	public sealed class EntityCodex : IExposable
	{
		// Token: 0x06009C0B RID: 39947 RVA: 0x002D4578 File Offset: 0x002D2778
		public EntityCodex()
		{
			this.hiddenCategories = new Dictionary<EntityCategoryDef, bool>();
			this.hiddenEntries = new Dictionary<EntityCodexEntryDef, bool>();
			this.discoveredEntities = new HashSet<ThingDef>();
			foreach (EntityCategoryDef key in DefDatabase<EntityCategoryDef>.AllDefs)
			{
				this.hiddenCategories.Add(key, true);
			}
		}

		// Token: 0x06009C0C RID: 39948 RVA: 0x002D45F4 File Offset: 0x002D27F4
		public static int EntryCountInCategory(EntityCategoryDef def)
		{
			int num = 0;
			using (IEnumerator<EntityCodexEntryDef> enumerator = DefDatabase<EntityCodexEntryDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.category == def)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06009C0D RID: 39949 RVA: 0x002D4648 File Offset: 0x002D2848
		public int DiscoveredCount(EntityCategoryDef def)
		{
			int num = 0;
			foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefs)
			{
				if (entityCodexEntryDef.category == def && Find.EntityCodex.Discovered(entityCodexEntryDef))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06009C0E RID: 39950 RVA: 0x002D46AC File Offset: 0x002D28AC
		public bool Hidden(ResearchProjectDef def)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (this.debug_UnhideAllResearch)
			{
				return false;
			}
			if (def.knowledgeCategory != null && Find.Anomaly.HighestLevelReached < 1 && Find.Anomaly.GenerateMonolith)
			{
				return true;
			}
			foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefs)
			{
				if (entityCodexEntryDef.discoveredResearchProjects.NotNullAndContains(def))
				{
					return !this.Discovered(entityCodexEntryDef);
				}
			}
			return false;
		}

		// Token: 0x06009C0F RID: 39951 RVA: 0x002D4748 File Offset: 0x002D2948
		public bool Discovered(EntityCategoryDef def)
		{
			bool flag;
			return ModLister.CheckAnomaly("Entity codex") && this.hiddenCategories.TryGetValue(def, out flag) && !flag;
		}

		// Token: 0x06009C10 RID: 39952 RVA: 0x002D477C File Offset: 0x002D297C
		public bool Discovered(EntityCodexEntryDef entry)
		{
			bool flag;
			return ModLister.CheckAnomaly("Entity codex") && this.Discovered(entry.category) && this.hiddenEntries.TryGetValue(entry, out flag) && !flag;
		}

		// Token: 0x06009C11 RID: 39953 RVA: 0x002D47C0 File Offset: 0x002D29C0
		public void SetDiscovered(List<EntityCodexEntryDef> entries, ThingDef discoveredDef = null, Thing discoveredThing = null)
		{
			foreach (EntityCodexEntryDef entry in entries)
			{
				this.SetDiscovered(entry, discoveredDef, discoveredThing);
			}
			Find.HiddenItemsManager.SetDiscovered(discoveredDef);
		}

		// Token: 0x06009C12 RID: 39954 RVA: 0x002D481C File Offset: 0x002D2A1C
		public void SetDiscovered(EntityCodexEntryDef entry, ThingDef discoveredDef = null, Thing discoveredThing = null)
		{
			if (!ModLister.CheckAnomaly("Entity codex"))
			{
				return;
			}
			if (discoveredDef != null)
			{
				this.discoveredEntities.Add(discoveredDef);
			}
			if (this.Discovered(entry))
			{
				return;
			}
			this.hiddenEntries[entry] = false;
			this.hiddenCategories[entry.category] = false;
			Find.SignalManager.SendSignal(new Signal("EntityDiscovered", true));
			if (Current.ProgramState == ProgramState.Playing && Find.Anomaly.AnomalyStudyEnabled)
			{
				if (discoveredThing != null)
				{
					Messages.Message(string.Format("{0}: {1}", "MessageEntityDiscovered".Translate(), entry.LabelCap), discoveredThing, MessageTypeDefOf.PositiveEvent, true);
				}
				if (!entry.discoveredResearchProjects.NullOrEmpty<ResearchProjectDef>())
				{
					string text = string.Empty;
					if (!entry.discoveredResearchProjects.NullOrEmpty<ResearchProjectDef>())
					{
						string text2;
						if ((text2 = ((discoveredThing != null) ? discoveredThing.LabelShort : null)) == null)
						{
							text2 = (((discoveredDef != null) ? discoveredDef.label : null) ?? entry.label);
						}
						string arg = text2;
						text += "LetterTextEntityDiscoveryResearch".Translate(arg.Named("ENTITY")) + ":\n" + (from r in entry.discoveredResearchProjects
						select r.LabelCap).ToLineList("  - ", false);
					}
					if (!text.NullOrEmpty())
					{
						ChoiceLetter_EntityDiscovered choiceLetter_EntityDiscovered = (ChoiceLetter_EntityDiscovered)LetterMaker.MakeLetter("LetterLabelEntityDiscovery".Translate(), text, LetterDefOf.EntityDiscovered, null, null);
						choiceLetter_EntityDiscovered.codexEntry = entry;
						if (discoveredThing != null)
						{
							choiceLetter_EntityDiscovered.lookTargets = discoveredThing;
						}
						Find.LetterStack.ReceiveLetter(choiceLetter_EntityDiscovered, null, 600, true);
					}
				}
				if (Find.Anomaly.GenerateMonolith && Find.Anomaly.monolith != null)
				{
					MonolithLevelDef levelDef = Find.Anomaly.LevelDef;
					string text3;
					string text4;
					if (levelDef.level > Find.Anomaly.lastLevelActivationLetterSent && Find.Anomaly.monolith.CanActivate(out text3, out text4) && !levelDef.activatableLetterLabel.NullOrEmpty() && !levelDef.activatableLetterText.NullOrEmpty())
					{
						Find.Anomaly.lastLevelActivationLetterSent = levelDef.level;
						Find.LetterStack.ReceiveLetter(levelDef.activatableLetterLabel, levelDef.activatableLetterText, LetterDefOf.PositiveEvent, Find.Anomaly.monolith, null, null, null, null, 900, true);
					}
				}
			}
		}

		// Token: 0x06009C13 RID: 39955 RVA: 0x002D4A92 File Offset: 0x002D2C92
		public bool Discovered(ThingDef def)
		{
			return ModLister.CheckAnomaly("Entity codex") && this.discoveredEntities.Contains(def);
		}

		// Token: 0x06009C14 RID: 39956 RVA: 0x002D4AB0 File Offset: 0x002D2CB0
		public void Debug_DiscoverAll()
		{
			foreach (EntityCodexEntryDef entry in DefDatabase<EntityCodexEntryDef>.AllDefs)
			{
				this.SetDiscovered(entry, null, null);
			}
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.entityCodexEntry != null)
				{
					this.discoveredEntities.Add(thingDef);
				}
			}
			foreach (EntityCategoryDef key in DefDatabase<EntityCategoryDef>.AllDefs)
			{
				this.hiddenCategories[key] = false;
			}
		}

		// Token: 0x06009C15 RID: 39957 RVA: 0x002D4B90 File Offset: 0x002D2D90
		public void Reset()
		{
			this.hiddenCategories.Clear();
			this.hiddenEntries.Clear();
			this.discoveredEntities.Clear();
			foreach (EntityCategoryDef key in DefDatabase<EntityCategoryDef>.AllDefs)
			{
				this.hiddenCategories.Add(key, true);
			}
			foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefs)
			{
				this.hiddenEntries.Add(entityCodexEntryDef, !entityCodexEntryDef.startDiscovered);
			}
		}

		// Token: 0x06009C16 RID: 39958 RVA: 0x002D4C4C File Offset: 0x002D2E4C
		public void ExposeData()
		{
			Scribe_Collections.Look<EntityCategoryDef, bool>(ref this.hiddenCategories, "hiddenCategories", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<EntityCodexEntryDef, bool>(ref this.hiddenEntries, "hiddenEntries", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<ThingDef>(ref this.discoveredEntities, "discoveredEntities", LookMode.Def);
			Scribe_Values.Look<bool>(ref this.debug_UnhideAllResearch, "debug_UnhideAllResearch", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.discoveredEntities == null)
				{
					this.discoveredEntities = new HashSet<ThingDef>();
				}
				Dictionary<EntityCategoryDef, bool> dictionary = new Dictionary<EntityCategoryDef, bool>();
				foreach (EntityCategoryDef key in DefDatabase<EntityCategoryDef>.AllDefs)
				{
					dictionary.Add(key, true);
				}
				foreach (KeyValuePair<EntityCategoryDef, bool> keyValuePair in this.hiddenCategories)
				{
					if (dictionary.ContainsKey(keyValuePair.Key))
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				this.hiddenCategories = dictionary;
				Dictionary<EntityCodexEntryDef, bool> dictionary2 = new Dictionary<EntityCodexEntryDef, bool>();
				foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefs)
				{
					dictionary2.Add(entityCodexEntryDef, !entityCodexEntryDef.startDiscovered);
				}
				foreach (KeyValuePair<EntityCodexEntryDef, bool> keyValuePair2 in this.hiddenEntries)
				{
					if (dictionary2.ContainsKey(keyValuePair2.Key))
					{
						dictionary2[keyValuePair2.Key] = keyValuePair2.Value;
					}
				}
				this.hiddenEntries = dictionary2;
				foreach (ThingDef discovered in this.discoveredEntities)
				{
					Find.HiddenItemsManager.SetDiscovered(discovered);
				}
			}
		}

		// Token: 0x04006C40 RID: 27712
		private Dictionary<EntityCategoryDef, bool> hiddenCategories;

		// Token: 0x04006C41 RID: 27713
		private Dictionary<EntityCodexEntryDef, bool> hiddenEntries;

		// Token: 0x04006C42 RID: 27714
		private HashSet<ThingDef> discoveredEntities;

		// Token: 0x04006C43 RID: 27715
		public bool debug_UnhideAllResearch;

		// Token: 0x04006C44 RID: 27716
		public const string EntityDiscoveredSignal = "EntityDiscovered";

		// Token: 0x04006C45 RID: 27717
		private const int EntityDiscoveredLetterDelayTicks = 600;

		// Token: 0x04006C46 RID: 27718
		private const int MonolithInteractableLetterDelayTicks = 900;
	}
}
