using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A81 RID: 14977
	public class QuestNode_GetSitePartDefsByTagsAndFaction : QuestNode
	{
		// Token: 0x06014761 RID: 83809 RVA: 0x0061C594 File Offset: 0x0061A794
		protected override bool TestRunInt(Slate slate)
		{
			return this.TrySetVars(slate);
		}

		// Token: 0x06014762 RID: 83810 RVA: 0x0061C59D File Offset: 0x0061A79D
		protected override void RunInt()
		{
			if (!this.TrySetVars(QuestGen.slate))
			{
				Log.Error("Could not resolve site parts.");
			}
		}

		// Token: 0x06014763 RID: 83811 RVA: 0x0061C5B8 File Offset: 0x0061A7B8
		private bool TrySetVars(Slate slate)
		{
			float points = slate.Get<float>("points", 0f, false);
			Faction faction = slate.Get<Faction>("enemyFaction", null, false);
			Pawn asker = slate.Get<Pawn>("asker", null, false);
			Thing mustBeHostileToFactionOfResolved = this.mustBeHostileToFactionOf.GetValue(slate);
			Func<SitePartDef, bool> <>9__3;
			Func<string, IEnumerable<SitePartDef>> <>9__1;
			Predicate<Faction> <>9__2;
			for (int i = 0; i < 2; i++)
			{
				QuestNode_GetSitePartDefsByTagsAndFaction.tmpTags.Clear();
				foreach (QuestNode_GetSitePartDefsByTagsAndFaction.SitePartOption sitePartOption in this.sitePartsTags.GetValue(slate))
				{
					if (Rand.Chance(sitePartOption.chance) && (i != 1 || sitePartOption.chance >= 1f))
					{
						QuestNode_GetSitePartDefsByTagsAndFaction.tmpTags.Add(sitePartOption.tag);
					}
				}
				IEnumerable<string> source = from x in QuestNode_GetSitePartDefsByTagsAndFaction.tmpTags
				where x != null
				select x;
				Func<string, IEnumerable<SitePartDef>> selector;
				if ((selector = <>9__1) == null)
				{
					selector = (<>9__1 = delegate(string x)
					{
						IEnumerable<SitePartDef> enumerable = SiteMakerHelper.SitePartDefsWithTag(x);
						IEnumerable<SitePartDef> source2 = enumerable;
						Func<SitePartDef, bool> predicate;
						if ((predicate = <>9__3) == null)
						{
							predicate = (<>9__3 = ((SitePartDef y) => points >= y.minThreatPoints));
						}
						IEnumerable<SitePartDef> enumerable2 = source2.Where(predicate);
						if (!enumerable2.Any<SitePartDef>())
						{
							return enumerable;
						}
						return enumerable2;
					});
				}
				IEnumerable<IEnumerable<SitePartDef>> sitePartsCandidates = source.Select(selector);
				Faction factionToUse = faction;
				bool disallowNonHostileFactions = true;
				Predicate<Faction> extraFactionValidator;
				if ((extraFactionValidator = <>9__2) == null)
				{
					extraFactionValidator = (<>9__2 = ((Faction x) => (asker == null || asker.Faction == null || asker.Faction != x) && (mustBeHostileToFactionOfResolved == null || mustBeHostileToFactionOfResolved.Faction == null || (x != mustBeHostileToFactionOfResolved.Faction && x.HostileTo(mustBeHostileToFactionOfResolved.Faction)))));
				}
				List<SitePartDef> list;
				Faction var;
				if (SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsCandidates, out list, out var, factionToUse, disallowNonHostileFactions, extraFactionValidator))
				{
					slate.Set<List<SitePartDef>>(this.storeAs.GetValue(slate), list, false);
					slate.Set<int>("sitePartCount", list.Count, false);
					if (QuestGen.Working)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						for (int j = 0; j < list.Count; j++)
						{
							dictionary[list[j].defName + "_exists"] = "True";
						}
						QuestGen.AddQuestDescriptionConstants(dictionary);
					}
					if (!this.storeFactionAs.GetValue(slate).NullOrEmpty())
					{
						slate.Set<Faction>(this.storeFactionAs.GetValue(slate), var, false);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400E270 RID: 57968
		public SlateRef<IEnumerable<QuestNode_GetSitePartDefsByTagsAndFaction.SitePartOption>> sitePartsTags;

		// Token: 0x0400E271 RID: 57969
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E272 RID: 57970
		[NoTranslate]
		public SlateRef<string> storeFactionAs;

		// Token: 0x0400E273 RID: 57971
		public SlateRef<Thing> mustBeHostileToFactionOf;

		// Token: 0x0400E274 RID: 57972
		private static List<string> tmpTags = new List<string>();

		// Token: 0x02003A82 RID: 14978
		public class SitePartOption
		{
			// Token: 0x0400E275 RID: 57973
			[NoTranslate]
			public string tag;

			// Token: 0x0400E276 RID: 57974
			public float chance = 1f;
		}
	}
}
