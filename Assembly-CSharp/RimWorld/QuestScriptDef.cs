using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001AC7 RID: 6855
	public class QuestScriptDef : Def
	{
		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x06009973 RID: 39283 RVA: 0x002CB218 File Offset: 0x002C9418
		public bool IsRootRandomSelected
		{
			get
			{
				return this.rootSelectionWeight != 0f && this.randomlySelectable;
			}
		}

		// Token: 0x1700178B RID: 6027
		// (get) Token: 0x06009974 RID: 39284 RVA: 0x002CB22F File Offset: 0x002C942F
		public bool IsRootDecree
		{
			get
			{
				return this.decreeSelectionWeight != 0f;
			}
		}

		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x06009975 RID: 39285 RVA: 0x002CB241 File Offset: 0x002C9441
		public bool IsRootAny
		{
			get
			{
				return this.IsRootRandomSelected || this.IsRootDecree || this.isRootSpecial;
			}
		}

		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x06009976 RID: 39286 RVA: 0x002CB25B File Offset: 0x002C945B
		public bool IsEpic
		{
			get
			{
				return this.epic;
			}
		}

		// Token: 0x06009977 RID: 39287 RVA: 0x002CB263 File Offset: 0x002C9463
		public void Run()
		{
			this.InitializeRules();
			this.root.Run();
		}

		// Token: 0x06009978 RID: 39288 RVA: 0x002CB278 File Offset: 0x002C9478
		public void InitializeRules()
		{
			if (this.questDescriptionRules != null)
			{
				QuestGen.AddQuestDescriptionRules(this.questDescriptionRules);
			}
			if (this.questNameRules != null)
			{
				QuestGen.AddQuestNameRules(this.questNameRules);
			}
			if (this.questDescriptionAndNameRules != null)
			{
				QuestGen.AddQuestDescriptionRules(this.questDescriptionAndNameRules);
				QuestGen.AddQuestNameRules(this.questDescriptionAndNameRules);
			}
			if (this.questContentRules != null)
			{
				QuestGen.AddQuestContentRules(this.questContentRules);
			}
		}

		// Token: 0x06009979 RID: 39289 RVA: 0x002CB2DC File Offset: 0x002C94DC
		public bool CanRun(Slate slate, IIncidentTarget target)
		{
			bool result;
			using (new ProfilerBlock("CanRun()"))
			{
				int num;
				if (!slate.TryGet<int>("points", out num, false))
				{
					num = 0;
				}
				if (this.lastCheckCanRunTick == Find.TickManager.TicksGame && this.lastCheckCanRunPoints == num)
				{
					result = this.lastCanRunResult;
				}
				else
				{
					this.lastCheckCanRunTick = Find.TickManager.TicksGame;
					this.lastCheckCanRunPoints = num;
					try
					{
						this.lastCanRunResult = (target != null && this.CanQuestOccurOnTile(target.Tile) && this.root.TestRun(slate.DeepCopy()));
						Scenario scenario = Find.Scenario;
						int num2 = 0;
						while (num2 < scenario.parts.Count && this.lastCanRunResult)
						{
							ScenPart_DisableQuest scenPart_DisableQuest = scenario.parts[num2] as ScenPart_DisableQuest;
							if (scenPart_DisableQuest != null && scenPart_DisableQuest.questDef == this)
							{
								this.lastCanRunResult = false;
							}
							num2++;
						}
					}
					catch (Exception arg)
					{
						this.lastCanRunResult = false;
						Log.Error(string.Format("Error while checking if can generate quest: {0}", arg));
						throw;
					}
					result = this.lastCanRunResult;
				}
			}
			return result;
		}

		// Token: 0x0600997A RID: 39290 RVA: 0x002CB414 File Offset: 0x002C9614
		private bool CanQuestOccurOnTile(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return true;
			}
			PlanetLayerDef layerDef = tile.LayerDef;
			return (this.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || this.layerWhitelist.Contains(layerDef)) && (this.layerBlacklist.NullOrEmpty<PlanetLayerDef>() || !this.layerBlacklist.Contains(layerDef));
		}

		// Token: 0x0600997B RID: 39291 RVA: 0x002CB470 File Offset: 0x002C9670
		public bool CanRun(float points, IIncidentTarget target)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return this.CanRun(slate, target);
		}

		// Token: 0x0600997C RID: 39292 RVA: 0x002CB498 File Offset: 0x002C9698
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.rootSelectionWeight > 0f && !this.autoAccept && this.expireDaysRange.TrueMax <= 0f)
			{
				yield return "rootSelectionWeight > 0 but expireDaysRange not set";
			}
			if (this.autoAccept && this.expireDaysRange.TrueMax > 0f)
			{
				yield return "autoAccept but there is an expireDaysRange set";
			}
			if (this.defaultChallengeRating > 0 && !this.IsRootAny)
			{
				yield return "non-root quest has defaultChallengeRating";
			}
			yield break;
			yield break;
		}

		// Token: 0x040068D0 RID: 26832
		public QuestNode root;

		// Token: 0x040068D1 RID: 26833
		public float rootSelectionWeight;

		// Token: 0x040068D2 RID: 26834
		public SimpleCurve rootSelectionWeightFactorFromPointsCurve;

		// Token: 0x040068D3 RID: 26835
		public bool randomlySelectable = true;

		// Token: 0x040068D4 RID: 26836
		public float rootMinPoints;

		// Token: 0x040068D5 RID: 26837
		public float rootMinProgressScore;

		// Token: 0x040068D6 RID: 26838
		public int rootEarliestDay;

		// Token: 0x040068D7 RID: 26839
		public bool rootIncreasesPopulation;

		// Token: 0x040068D8 RID: 26840
		public float minRefireDays;

		// Token: 0x040068D9 RID: 26841
		public float decreeSelectionWeight;

		// Token: 0x040068DA RID: 26842
		public List<string> decreeTags;

		// Token: 0x040068DB RID: 26843
		public RulePack questDescriptionRules;

		// Token: 0x040068DC RID: 26844
		public RulePack questNameRules;

		// Token: 0x040068DD RID: 26845
		public RulePack questDescriptionAndNameRules;

		// Token: 0x040068DE RID: 26846
		public RulePack questContentRules;

		// Token: 0x040068DF RID: 26847
		public RulePack questSubjectRules;

		// Token: 0x040068E0 RID: 26848
		public bool autoAccept;

		// Token: 0x040068E1 RID: 26849
		public bool hideOnCleanup;

		// Token: 0x040068E2 RID: 26850
		public FloatRange expireDaysRange = new FloatRange(-1f, -1f);

		// Token: 0x040068E3 RID: 26851
		public bool nameMustBeUnique;

		// Token: 0x040068E4 RID: 26852
		public int defaultChallengeRating = -1;

		// Token: 0x040068E5 RID: 26853
		public bool defaultHidden;

		// Token: 0x040068E6 RID: 26854
		public bool isRootSpecial;

		// Token: 0x040068E7 RID: 26855
		public bool canGiveRoyalFavor;

		// Token: 0x040068E8 RID: 26856
		public string questAvailableLetterLabel;

		// Token: 0x040068E9 RID: 26857
		public LetterDef questAvailableLetterDef;

		// Token: 0x040068EA RID: 26858
		public bool questAvailableLetterTextIsDescription;

		// Token: 0x040068EB RID: 26859
		public bool hideInvolvedFactionsInfo;

		// Token: 0x040068EC RID: 26860
		public bool affectedByPopulation;

		// Token: 0x040068ED RID: 26861
		public bool affectedByPoints = true;

		// Token: 0x040068EE RID: 26862
		public bool defaultCharity;

		// Token: 0x040068EF RID: 26863
		public HistoryEventDef successHistoryEvent;

		// Token: 0x040068F0 RID: 26864
		public HistoryEventDef failedOrExpiredHistoryEvent;

		// Token: 0x040068F1 RID: 26865
		public bool sendAvailableLetter = true;

		// Token: 0x040068F2 RID: 26866
		public bool epic;

		// Token: 0x040068F3 RID: 26867
		public QuestScriptDef epicParent;

		// Token: 0x040068F4 RID: 26868
		public bool endOnColonyMove = true;

		// Token: 0x040068F5 RID: 26869
		public bool everAcceptableInSpace;

		// Token: 0x040068F6 RID: 26870
		public List<QuestGiverTag> givenBy = new List<QuestGiverTag>();

		// Token: 0x040068F7 RID: 26871
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x040068F8 RID: 26872
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x040068F9 RID: 26873
		public bool canOccurOnAllPlanetLayers;

		// Token: 0x040068FA RID: 26874
		[Unsaved(false)]
		private int lastCheckCanRunTick;

		// Token: 0x040068FB RID: 26875
		[Unsaved(false)]
		private int lastCheckCanRunPoints;

		// Token: 0x040068FC RID: 26876
		[Unsaved(false)]
		private bool lastCanRunResult;
	}
}
