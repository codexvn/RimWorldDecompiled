using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002658 RID: 9816
	public class Precept : IExposable, ILoadReferenceable
	{
		// Token: 0x170020E9 RID: 8425
		// (get) Token: 0x0600D71B RID: 55067 RVA: 0x003FC65F File Offset: 0x003FA85F
		public virtual string TipLabel
		{
			get
			{
				return this.def.tipLabelOverride ?? (this.def.issue.LabelCap + ": " + this.def.LabelCap);
			}
		}

		// Token: 0x170020EA RID: 8426
		// (get) Token: 0x0600D71C RID: 55068 RVA: 0x003FC69F File Offset: 0x003FA89F
		public virtual string UIInfoFirstLine
		{
			get
			{
				return this.def.issue.LabelCap;
			}
		}

		// Token: 0x170020EB RID: 8427
		// (get) Token: 0x0600D71D RID: 55069 RVA: 0x003FC6B6 File Offset: 0x003FA8B6
		public virtual string UIInfoSecondLine
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x170020EC RID: 8428
		// (get) Token: 0x0600D71E RID: 55070 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CanRegenerate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170020ED RID: 8429
		// (get) Token: 0x0600D71F RID: 55071 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool SortByImpact
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170020EE RID: 8430
		// (get) Token: 0x0600D720 RID: 55072 RVA: 0x003FC6C8 File Offset: 0x003FA8C8
		public virtual Color LabelColor
		{
			get
			{
				return new Color(0.8f, 0.8f, 0.8f);
			}
		}

		// Token: 0x170020EF RID: 8431
		// (get) Token: 0x0600D721 RID: 55073 RVA: 0x003FC6DE File Offset: 0x003FA8DE
		public virtual string Label
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170020F0 RID: 8432
		// (get) Token: 0x0600D722 RID: 55074 RVA: 0x003FC6E8 File Offset: 0x003FA8E8
		public string LabelCap
		{
			get
			{
				string result;
				if ((result = this.labelCapCache) == null)
				{
					result = (this.labelCapCache = this.name.CapitalizeFirst());
				}
				return result;
			}
		}

		// Token: 0x170020F1 RID: 8433
		// (get) Token: 0x0600D723 RID: 55075 RVA: 0x003FC713 File Offset: 0x003FA913
		public string Description
		{
			get
			{
				if (!this.descOverride.NullOrEmpty())
				{
					return this.descOverride;
				}
				return this.def.description;
			}
		}

		// Token: 0x170020F2 RID: 8434
		// (get) Token: 0x0600D724 RID: 55076 RVA: 0x003FC734 File Offset: 0x003FA934
		public virtual string DescriptionForTip
		{
			get
			{
				return this.Description;
			}
		}

		// Token: 0x170020F3 RID: 8435
		// (get) Token: 0x0600D725 RID: 55077 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool UsesGeneratedName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170020F4 RID: 8436
		// (get) Token: 0x0600D726 RID: 55078 RVA: 0x003FC73C File Offset: 0x003FA93C
		public virtual Texture2D Icon
		{
			get
			{
				return this.def.Icon ?? this.ideo.Icon;
			}
		}

		// Token: 0x170020F5 RID: 8437
		// (get) Token: 0x0600D727 RID: 55079 RVA: 0x00002C42 File Offset: 0x00000E42
		// (set) Token: 0x0600D728 RID: 55080 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual List<PreceptApparelRequirement> ApparelRequirements
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170020F6 RID: 8438
		// (get) Token: 0x0600D729 RID: 55081 RVA: 0x003FC758 File Offset: 0x003FA958
		public int Id
		{
			get
			{
				return this.ID;
			}
		}

		// Token: 0x0600D72A RID: 55082 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostMake()
		{
		}

		// Token: 0x0600D72B RID: 55083 RVA: 0x003FC760 File Offset: 0x003FA960
		public virtual void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			this.ideo = ideo;
			this.ID = Find.UniqueIDsManager.GetNextPreceptID();
			this.randomSeed = Rand.Int;
			this.name = this.def.issue.label;
		}

		// Token: 0x0600D72C RID: 55084 RVA: 0x003FC79A File Offset: 0x003FA99A
		public virtual void Regenerate(Ideo ideo, FactionDef generatingFor = null)
		{
			this.Init(ideo, generatingFor);
			this.ClearTipCache();
			ideo.anyPreceptEdited = true;
		}

		// Token: 0x0600D72D RID: 55085 RVA: 0x003FC7B1 File Offset: 0x003FA9B1
		public virtual void ClearTipCache()
		{
			this.tipCached = null;
		}

		// Token: 0x0600D72E RID: 55086 RVA: 0x003FC7BA File Offset: 0x003FA9BA
		protected string ColorizeDescTitle(TaggedString title)
		{
			return title.Resolve().Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x0600D72F RID: 55087 RVA: 0x003FC7CD File Offset: 0x003FA9CD
		protected string ColorizeWarning(TaggedString title)
		{
			return title.Resolve().Colorize(ColoredText.ThreatColor);
		}

		// Token: 0x0600D730 RID: 55088 RVA: 0x003FC7E0 File Offset: 0x003FA9E0
		public virtual string GetTip()
		{
			Precept.<>c__DisplayClass52_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			Precept.tmpCompsDesc.Clear();
			if (!this.DescriptionForTip.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Append(this.DescriptionForTip);
			}
			bool flag = true;
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				PreceptComp_UnwillingToDo preceptComp_UnwillingToDo = this.def.comps[i] as PreceptComp_UnwillingToDo;
				if (preceptComp_UnwillingToDo != null)
				{
					string prohibitionText = preceptComp_UnwillingToDo.GetProhibitionText();
					if (!prohibitionText.NullOrEmpty())
					{
						if (flag)
						{
							flag = false;
							Precept.tmpCompsDesc.AppendLine();
							Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("Prohibitions".Translate() + ":"));
						}
						Precept.tmpCompsDesc.AppendInNewLine("  - " + prohibitionText);
					}
				}
			}
			Precept.tmpDescs.Clear();
			bool flag2 = true;
			int j = 0;
			while (j < this.def.comps.Count)
			{
				PreceptComp_SelfTookMemoryThought preceptComp_SelfTookMemoryThought = this.def.comps[j] as PreceptComp_SelfTookMemoryThought;
				PreceptComp preceptComp;
				if (preceptComp_SelfTookMemoryThought != null)
				{
					preceptComp = preceptComp_SelfTookMemoryThought;
					goto IL_195;
				}
				PreceptComp_SituationalThought preceptComp_SituationalThought = this.def.comps[j] as PreceptComp_SituationalThought;
				if (preceptComp_SituationalThought != null && preceptComp_SituationalThought.AffectsMood)
				{
					preceptComp = preceptComp_SituationalThought;
					goto IL_195;
				}
				PreceptComp_KnowsMemoryThought preceptComp_KnowsMemoryThought = this.def.comps[j] as PreceptComp_KnowsMemoryThought;
				if (preceptComp_KnowsMemoryThought != null && preceptComp_KnowsMemoryThought.AffectsMood)
				{
					preceptComp = preceptComp_KnowsMemoryThought;
					goto IL_195;
				}
				PreceptComp_BedThought preceptComp_BedThought = this.def.comps[j] as PreceptComp_BedThought;
				if (preceptComp_BedThought != null && preceptComp_BedThought.AffectsMood)
				{
					preceptComp = preceptComp_BedThought;
					goto IL_195;
				}
				IL_21F:
				j++;
				continue;
				IL_195:
				if (flag2)
				{
					flag2 = false;
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("Mood".Translate() + ":"));
				}
				foreach (string text in preceptComp.GetDescriptions())
				{
					if (Precept.tmpDescs.Add(text))
					{
						Precept.tmpCompsDesc.AppendInNewLine("  - " + text);
					}
				}
				goto IL_21F;
			}
			Precept.tmpDescs.Clear();
			bool flag3 = true;
			int k = 0;
			while (k < this.def.comps.Count)
			{
				PreceptComp_SituationalThought preceptComp_SituationalThought2 = this.def.comps[k] as PreceptComp_SituationalThought;
				PreceptComp preceptComp2;
				if (preceptComp_SituationalThought2 != null && !preceptComp_SituationalThought2.AffectsMood)
				{
					preceptComp2 = preceptComp_SituationalThought2;
					goto IL_2AC;
				}
				PreceptComp_KnowsMemoryThought preceptComp_KnowsMemoryThought2 = this.def.comps[k] as PreceptComp_KnowsMemoryThought;
				if (preceptComp_KnowsMemoryThought2 != null && !preceptComp_KnowsMemoryThought2.AffectsMood)
				{
					preceptComp2 = preceptComp_KnowsMemoryThought2;
					goto IL_2AC;
				}
				IL_336:
				k++;
				continue;
				IL_2AC:
				if (flag3)
				{
					flag3 = false;
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("Opinions".Translate() + ":"));
				}
				foreach (string text2 in preceptComp2.GetDescriptions())
				{
					if (Precept.tmpDescs.Add(text2))
					{
						Precept.tmpCompsDesc.AppendInNewLine("  - " + text2);
					}
				}
				goto IL_336;
			}
			Precept.tmpDescs.Clear();
			bool flag4 = true;
			for (int l = 0; l < this.def.comps.Count; l++)
			{
				PreceptComp_MentalBreak preceptComp_MentalBreak = this.def.comps[l] as PreceptComp_MentalBreak;
				if (preceptComp_MentalBreak != null)
				{
					if (flag4)
					{
						flag4 = false;
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("MentalBreaks".Translate() + ":"));
					}
					foreach (string text3 in preceptComp_MentalBreak.GetDescriptions())
					{
						if (Precept.tmpDescs.Add(text3))
						{
							Precept.tmpCompsDesc.AppendInNewLine("  - " + text3);
						}
					}
				}
			}
			Precept.tmpDescs.Clear();
			bool flag5 = true;
			if (!this.def.enablesNeeds.NullOrEmpty<NeedDef>())
			{
				for (int m = 0; m < this.def.enablesNeeds.Count; m++)
				{
					if (flag5)
					{
						flag5 = false;
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("AddsNeeds".Translate() + ":"));
					}
					for (int n = 0; n < this.def.enablesNeeds.Count; n++)
					{
						NeedDef needDef = this.def.enablesNeeds[n];
						if (Precept.tmpDescs.Add(needDef.label))
						{
							Precept.tmpCompsDesc.AppendInNewLine("  - " + needDef.LabelCap);
						}
					}
				}
			}
			Precept.tmpDescs.Clear();
			flag5 = true;
			if (!this.def.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				for (int num = 0; num < this.def.disablesNeeds.Count; num++)
				{
					if (flag5)
					{
						flag5 = false;
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("DisablesNeeds".Translate() + ":"));
					}
					for (int num2 = 0; num2 < this.def.disablesNeeds.Count; num2++)
					{
						NeedDef needDef2 = this.def.disablesNeeds[num2];
						if (Precept.tmpDescs.Add(needDef2.label))
						{
							Precept.tmpCompsDesc.AppendInNewLine("  - " + needDef2.LabelCap);
						}
					}
				}
			}
			CS$<>8__locals1.firstStat = true;
			if (this.def.statOffsets != null)
			{
				for (int num3 = 0; num3 < this.def.statOffsets.Count; num3++)
				{
					this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
					Precept.tmpCompsDesc.AppendInNewLine("  - " + this.def.statOffsets[num3].stat.LabelCap + ": " + this.def.statOffsets[num3].ValueToStringAsOffset);
				}
			}
			if (this.def.conditionalStatAffecters != null)
			{
				for (int num4 = 0; num4 < this.def.conditionalStatAffecters.Count; num4++)
				{
					ConditionalStatAffecter conditionalStatAffecter = this.def.conditionalStatAffecters[num4];
					if (conditionalStatAffecter.statOffsets != null)
					{
						for (int num5 = 0; num5 < conditionalStatAffecter.statOffsets.Count; num5++)
						{
							this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
							Precept.tmpCompsDesc.AppendInNewLine("  - " + conditionalStatAffecter.statOffsets[num5].stat.LabelCap + ": " + conditionalStatAffecter.statOffsets[num5].ValueToStringAsOffset + " (" + conditionalStatAffecter.Label + ")");
						}
					}
				}
			}
			if (this.def.statFactors != null)
			{
				for (int num6 = 0; num6 < this.def.statFactors.Count; num6++)
				{
					this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
					Precept.tmpCompsDesc.AppendInNewLine("  - " + this.def.statFactors[num6].stat.LabelCap + ": " + this.def.statFactors[num6].ToStringAsFactor);
				}
			}
			if (!Mathf.Approximately(this.def.growthVatSpeedFactor, 1f))
			{
				this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
				Precept.tmpCompsDesc.AppendInNewLine("  - " + StatDefOf.GrowthVatOccupantSpeed.LabelCap + ": x" + this.def.growthVatSpeedFactor.ToStringPercent());
			}
			if (!Mathf.Approximately(this.def.biosculpterPodCycleSpeedFactor, 1f))
			{
				this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
				Precept.tmpCompsDesc.AppendInNewLine("  - " + StatDefOf.BiosculpterPodSpeedFactor.LabelCap + ": x" + this.def.biosculpterPodCycleSpeedFactor.ToStringPercent());
			}
			if (this.def.conditionalStatAffecters != null)
			{
				for (int num7 = 0; num7 < this.def.conditionalStatAffecters.Count; num7++)
				{
					ConditionalStatAffecter conditionalStatAffecter2 = this.def.conditionalStatAffecters[num7];
					if (conditionalStatAffecter2.statFactors != null)
					{
						for (int num8 = 0; num8 < conditionalStatAffecter2.statFactors.Count; num8++)
						{
							this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
							Precept.tmpCompsDesc.AppendInNewLine("  - " + conditionalStatAffecter2.statFactors[num8].stat.LabelCap + ": " + conditionalStatAffecter2.statFactors[num8].ToStringAsFactor + " (" + conditionalStatAffecter2.Label + ")");
						}
					}
				}
			}
			if (this.def.abilityStatFactors != null)
			{
				for (int num9 = 0; num9 < this.def.abilityStatFactors.Count; num9++)
				{
					foreach (StatModifier statModifier in this.def.abilityStatFactors[num9].modifiers)
					{
						this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
						Precept.tmpCompsDesc.AppendInNewLine("  - " + this.def.abilityStatFactors[num9].ability.LabelCap + ": " + statModifier.stat.LabelCap + ": " + statModifier.ToStringAsFactor);
					}
				}
			}
			if (this.def.TraitsAffecting.Count != 0)
			{
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("AffectedByTraits".Translate() + ":"));
				foreach (TraitDegreeData traitDegreeData in from x in this.def.TraitsAffecting
				select x.def.DataAtDegree(x.degree.GetValueOrDefault()))
				{
					Precept.tmpCompsDesc.AppendInNewLine("  - " + traitDegreeData.GetLabelFor(Gender.Male).CapitalizeFirst());
				}
			}
			bool flag6 = true;
			if (this.ideo.Fluid)
			{
				for (int num10 = 0; num10 < this.def.comps.Count; num10++)
				{
					if (this.def.comps[num10] is PreceptComp_DevelopmentPoints)
					{
						if (flag6)
						{
							flag6 = false;
							Precept.tmpCompsDesc.AppendLine();
							Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("DevelopmentPoints".Translate() + ":"));
						}
						foreach (string str in this.def.comps[num10].GetDescriptions())
						{
							Precept.tmpCompsDesc.AppendInNewLine("  - " + str);
						}
					}
				}
			}
			return Precept.tmpCompsDesc.ToString();
		}

		// Token: 0x0600D731 RID: 55089 RVA: 0x003FD4A0 File Offset: 0x003FB6A0
		public virtual IEnumerable<Alert> GetAlerts()
		{
			return Enumerable.Empty<Alert>();
		}

		// Token: 0x0600D732 RID: 55090 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Tick()
		{
		}

		// Token: 0x0600D733 RID: 55091 RVA: 0x00066B11 File Offset: 0x00064D11
		public virtual string GenerateNameRaw()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D734 RID: 55092 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual List<PreceptApparelRequirement> GenerateNewApparelRequirements(FactionDef generatingFor = null)
		{
			return null;
		}

		// Token: 0x0600D735 RID: 55093 RVA: 0x003FD4A8 File Offset: 0x003FB6A8
		public string GenerateNewName()
		{
			Precept.tmpUsedWords.Clear();
			foreach (Precept precept in this.ideo.PreceptsListForReading)
			{
				if (precept != this && precept.GetType() == base.GetType() && precept.name != null)
				{
					Precept.tmpUsedWords.Add(precept.name);
					foreach (string item in precept.name.Split(' ', StringSplitOptions.None))
					{
						Precept.tmpUsedWords.Add(item);
					}
				}
			}
			float num = 0f;
			if (this.def.takeNameFrom != null)
			{
				return this.ideo.PreceptsListForReading.First((Precept p) => p.def == this.def.takeNameFrom).name;
			}
			this.labelCapCache = null;
			string result;
			if (this.def.ignoreNameUniqueness || this.ideo.classicMode)
			{
				result = this.GenerateNameRaw();
			}
			else
			{
				int num2 = 0;
				string text;
				for (;;)
				{
					text = this.GenerateNameRaw();
					float num3 = Precept.<GenerateNewName>g__NameUniqueness|58_0(text);
					if (num3 >= 1f)
					{
						break;
					}
					if (num3 > num)
					{
						num = num3;
					}
					if (num2++ > 50)
					{
						goto Block_6;
					}
				}
				return text;
				Block_6:
				result = text;
			}
			return result;
		}

		// Token: 0x0600D736 RID: 55094 RVA: 0x003FD60C File Offset: 0x003FB80C
		public void RegenerateName()
		{
			this.SetName(this.GenerateNewName());
		}

		// Token: 0x0600D737 RID: 55095 RVA: 0x003FD61C File Offset: 0x003FB81C
		public void SetName(string newName)
		{
			if (newName == this.name)
			{
				return;
			}
			this.labelCapCache = null;
			this.name = newName;
			if (this.ideo != null)
			{
				foreach (Precept precept in this.ideo.PreceptsListForReading)
				{
					if (precept.def.takeNameFrom == this.def)
					{
						precept.RegenerateName();
					}
				}
			}
		}

		// Token: 0x0600D738 RID: 55096 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			return null;
		}

		// Token: 0x0600D739 RID: 55097 RVA: 0x003FD6AC File Offset: 0x003FB8AC
		public virtual List<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
		{
			this.tmpSituationalThoughts.Clear();
			return this.tmpSituationalThoughts;
		}

		// Token: 0x0600D73A RID: 55098 RVA: 0x003FD6C0 File Offset: 0x003FB8C0
		protected void AddIdeoRulesTo(ref GrammarRequest request)
		{
			string ideoName = null;
			IdeoSymbolPack ideoSymbolPack;
			if (this.TryGuessChosenSymbolPack(out ideoSymbolPack))
			{
				if (!ideoSymbolPack.ideoName.NullOrEmpty())
				{
					request.Rules.Add(new Rule_String("chosenIdeoName", ideoSymbolPack.ideoName));
					ideoName = ideoSymbolPack.ideoName;
				}
				if (!ideoSymbolPack.theme.NullOrEmpty())
				{
					request.Rules.Add(new Rule_String("chosenTheme", this.MatchIdeoNameCapitalization(ideoSymbolPack.theme, ideoName)));
				}
			}
			foreach (MemeDef memeDef in this.ideo.memes)
			{
				if (memeDef.generalRules != null)
				{
					request.IncludesBare.Add(memeDef.generalRules);
				}
				if (memeDef.symbolPacks != null)
				{
					foreach (IdeoSymbolPack ideoSymbolPack2 in memeDef.symbolPacks)
					{
						if (!ideoSymbolPack2.theme.NullOrEmpty())
						{
							request.Rules.Add(new Rule_String("memePackTheme", this.MatchIdeoNameCapitalization(ideoSymbolPack2.theme, ideoName)));
						}
						if (!ideoSymbolPack2.adjective.NullOrEmpty())
						{
							request.Rules.Add(new Rule_String("memePackAdjective", this.MatchIdeoNameCapitalization(ideoSymbolPack2.adjective, ideoName)));
						}
					}
				}
			}
			if (!this.ideo.adjective.NullOrEmpty())
			{
				request.Rules.Add(new Rule_String("chosenAdjective", this.ideo.adjective));
			}
			if (this.ideo.KeyDeityName != null)
			{
				request.Rules.Add(new Rule_String("keyDeity", this.ideo.KeyDeityName));
			}
			if (!this.ideo.leaderTitleMale.NullOrEmpty())
			{
				request.Rules.Add(new Rule_String("leaderTitle", this.ideo.leaderTitleMale));
			}
		}

		// Token: 0x0600D73B RID: 55099 RVA: 0x003FD8D8 File Offset: 0x003FBAD8
		private bool TryGuessChosenSymbolPack(out IdeoSymbolPack result)
		{
			foreach (MemeDef memeDef in this.ideo.memes)
			{
				if (memeDef.symbolPacks != null)
				{
					foreach (IdeoSymbolPack ideoSymbolPack in memeDef.symbolPacks)
					{
						if (ideoSymbolPack.adjective == this.ideo.adjective || ideoSymbolPack.member == this.ideo.memberName || (ideoSymbolPack.ideoName != null && this.ideo.name.Contains(ideoSymbolPack.ideoName)))
						{
							result = ideoSymbolPack;
							return true;
						}
					}
				}
			}
			result = new IdeoSymbolPack();
			return false;
		}

		// Token: 0x0600D73C RID: 55100 RVA: 0x003FD9D8 File Offset: 0x003FBBD8
		private string MatchIdeoNameCapitalization(string value, string ideoName)
		{
			if (value.Equals(ideoName, StringComparison.InvariantCultureIgnoreCase))
			{
				return ideoName;
			}
			return value;
		}

		// Token: 0x0600D73D RID: 55101 RVA: 0x003FD9E8 File Offset: 0x003FBBE8
		public virtual void DrawPreceptBox(Rect preceptBox, IdeoEditMode editMode, bool forceHighlight = false)
		{
			GUI.color = Color.white;
			Color backgroundColor = IdeoUIUtility.GetBackgroundColor(this.def.impact);
			Widgets.DrawRectFast(preceptBox, backgroundColor, null);
			GUI.color = new Color(backgroundColor.r + 0.05f, backgroundColor.g + 0.05f, backgroundColor.b + 0.05f);
			Widgets.DrawBox(preceptBox, 1, null);
			GUI.color = Color.white;
			if (Mouse.IsOver(preceptBox) || forceHighlight)
			{
				Widgets.DrawHighlight(preceptBox);
			}
			if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(preceptBox, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				MemeDef memeThatRequiresPrecept = this.ideo.GetMemeThatRequiresPrecept(this.def);
				bool flag = false;
				IEnumerable<FloatMenuOption> enumerable = this.EditFloatMenuOptions();
				if (enumerable != null)
				{
					foreach (FloatMenuOption floatMenuOption in enumerable)
					{
						flag = true;
						floatMenuOption.orderInPriority = 2000;
						list.Add(floatMenuOption);
					}
				}
				FloatMenuOption floatMenuOption2 = null;
				if (this.def.canRemoveInUI && !this.def.issue.HasDefaultPrecept)
				{
					floatMenuOption2 = new FloatMenuOption("Remove".Translate().CapitalizeFirst(), delegate()
					{
						this.ideo.RemovePrecept(this, false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 1900);
					MemeDef memeThatRequiresPrecept2 = this.ideo.GetMemeThatRequiresPrecept(this.def);
					if (editMode != IdeoEditMode.Dev && memeThatRequiresPrecept2 != null)
					{
						floatMenuOption2.action = null;
						floatMenuOption2.Label = "CannotRemove".Translate() + ": " + "RequiredByMeme".Translate(memeThatRequiresPrecept2.label);
					}
					else
					{
						flag = true;
					}
				}
				if (this.CanRegenerate)
				{
					FactionDef faction = (Find.World == null) ? Find.Scenario.playerFaction.factionDef : null;
					list.Add(new FloatMenuOption("Regenerate".Translate().CapitalizeFirst(), delegate()
					{
						this.Regenerate(this.ideo, faction);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 1800));
				}
				else if (base.GetType() == typeof(Precept))
				{
					using (IEnumerator<PreceptDef> enumerator2 = (from x in DefDatabase<PreceptDef>.AllDefs
					where x != this.def && x.issue == this.def.issue
					select x).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							PreceptDef p = enumerator2.Current;
							AcceptanceReport report = IdeoUIUtility.CanListPrecept(this.ideo, p, editMode);
							if (report || !string.IsNullOrWhiteSpace(report.Reason))
							{
								TaggedString taggedString = p.LabelCap;
								Action action = delegate()
								{
									this.ideo.AddPrecept(PreceptMaker.MakePrecept(p), true, null, null);
									this.ideo.RemovePrecept(this, true);
								};
								if (!report)
								{
									action = null;
									taggedString += " (" + report.Reason + ")";
								}
								FloatMenuOption floatMenuOption3 = new FloatMenuOption(taggedString, action, p.issue.Icon, IdeoUIUtility.GetIconAndLabelColor(p.impact), MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
								floatMenuOption3.orderInPriority = p.displayOrderInIssue;
								flag = true;
								list.Add(floatMenuOption3);
							}
						}
					}
				}
				if (flag)
				{
					if (floatMenuOption2 != null)
					{
						list.Add(floatMenuOption2);
					}
				}
				else if (memeThatRequiresPrecept == null)
				{
					list.Add(new FloatMenuOption("CannotEdit".Translate() + ": " + "Required".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else
				{
					list.Add(new FloatMenuOption("CannotEdit".Translate() + ": " + "RequiredByMeme".Translate(memeThatRequiresPrecept.LabelCap), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.EditingPrecepts, KnowledgeAmount.Total);
				Find.WindowStack.Add(new FloatMenu(list));
			}
			GUI.color = IdeoUIUtility.GetIconAndLabelColor(this.def.impact);
			Rect rect = new Rect(preceptBox.x + (preceptBox.height - 50f) / 2f, preceptBox.y + (preceptBox.height - 50f) / 2f, 50f, 50f);
			this.DrawIcon(rect);
			Rect rect2 = new Rect(rect.xMax + 10f, preceptBox.y + 3f, preceptBox.xMax - rect.xMax - 10f, preceptBox.height / 2f);
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = this.LabelColor;
			Widgets.Label(rect2, this.UIInfoFirstLine);
			GUI.color = Color.white;
			GenUI.ResetLabelAlign();
			GUI.color = IdeoUIUtility.GetIconAndLabelColor(this.def.impact);
			Rect rect3 = new Rect(rect.xMax + 10f, preceptBox.y + preceptBox.height / 2f - 3f, preceptBox.xMax - rect.xMax - 10f, preceptBox.height / 2f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.LabelFit(rect3, this.UIInfoSecondLine);
			GenUI.ResetLabelAlign();
			bool flag2;
			this.PostDrawBox(preceptBox, out flag2);
			if (!flag2 && Mouse.IsOver(preceptBox) && Find.WindowStack.WindowOfType<FloatMenu>() == null)
			{
				TooltipHandler.TipRegion(preceptBox, this.TipLabel.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + this.GetTip() + ((editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty));
			}
		}

		// Token: 0x0600D73E RID: 55102 RVA: 0x003FE020 File Offset: 0x003FC220
		public virtual void DrawIcon(Rect rect)
		{
			GUI.DrawTexture(rect, this.def.issue.Icon);
		}

		// Token: 0x0600D73F RID: 55103 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CompatibleWith(Precept other)
		{
			return true;
		}

		// Token: 0x0600D740 RID: 55104 RVA: 0x003FE038 File Offset: 0x003FC238
		public virtual bool GetPlayerWarning(out string shortText, out string description)
		{
			string text;
			description = (text = null);
			shortText = text;
			return false;
		}

		// Token: 0x0600D741 RID: 55105 RVA: 0x00131DB4 File Offset: 0x0012FFB4
		protected virtual void PostDrawBox(Rect rect, out bool anyIconTooltipActive)
		{
			anyIconTooltipActive = false;
		}

		// Token: 0x0600D742 RID: 55106 RVA: 0x003FE050 File Offset: 0x003FC250
		public FloatMenuOption EditFloatMenuOption()
		{
			return new FloatMenuOption("Edit".Translate() + "...", delegate()
			{
				Find.WindowStack.Add(new Dialog_EditPrecept(this));
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
		}

		// Token: 0x0600D743 RID: 55107 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
		}

		// Token: 0x0600D744 RID: 55108 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_IdeoNotPrimaryAnymore(Ideo newIdeo)
		{
		}

		// Token: 0x0600D745 RID: 55109 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberLost(Pawn pawn)
		{
		}

		// Token: 0x0600D746 RID: 55110 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberGained(Pawn pawn)
		{
		}

		// Token: 0x0600D747 RID: 55111 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberGuestStatusChanged(Pawn pawn)
		{
		}

		// Token: 0x0600D748 RID: 55112 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_GameStarted()
		{
		}

		// Token: 0x0600D749 RID: 55113 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_IdeoReformed()
		{
		}

		// Token: 0x0600D74A RID: 55114 RVA: 0x003FE094 File Offset: 0x003FC294
		public virtual void Notify_MemberGenerated(Pawn pawn, bool newborn, bool ignoreApparel = false)
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				this.def.comps[i].Notify_MemberGenerated(pawn, this, newborn, ignoreApparel);
			}
		}

		// Token: 0x0600D74B RID: 55115 RVA: 0x003FE0D8 File Offset: 0x003FC2D8
		public virtual void Notify_HistoryEvent(HistoryEvent ev)
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				this.def.comps[i].Notify_HistoryEvent(ev, this);
			}
		}

		// Token: 0x0600D74C RID: 55116 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_RecachedPrecepts()
		{
		}

		// Token: 0x0600D74D RID: 55117 RVA: 0x00132D70 File Offset: 0x00130F70
		public virtual bool TryGetLostByReformingWarning(out string warning)
		{
			warning = null;
			return false;
		}

		// Token: 0x0600D74E RID: 55118 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_RemovedByReforming()
		{
		}

		// Token: 0x0600D74F RID: 55119 RVA: 0x003FE118 File Offset: 0x003FC318
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Defs.Look<PreceptDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<int>(ref this.randomSeed, "randomSeed", 0, false);
			Scribe_Values.Look<bool>(ref this.usesDefiniteArticle, "usesDefiniteArticle", false, false);
			Scribe_Values.Look<string>(ref this.descOverride, "descOverride", null, false);
			Scribe_Values.Look<bool>(ref this.nameLocked, "nameLocked", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && (this.ID == -1 || GameDataSaveLoader.IsSavingOrLoadingExternalIdeo))
			{
				this.ID = Find.UniqueIDsManager.GetNextPreceptID();
			}
		}

		// Token: 0x0600D750 RID: 55120 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberDied(Pawn p)
		{
		}

		// Token: 0x0600D751 RID: 55121 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberCorpseDestroyed(Pawn p)
		{
		}

		// Token: 0x0600D752 RID: 55122 RVA: 0x003FE1C9 File Offset: 0x003FC3C9
		public string GetUniqueLoadID()
		{
			return "Precept_" + this.ID.ToString();
		}

		// Token: 0x0600D753 RID: 55123 RVA: 0x003FE1E0 File Offset: 0x003FC3E0
		public virtual void CopyTo(Precept other)
		{
			other.ideo = this.ideo;
			other.def = this.def;
			other.name = this.name;
			other.ID = this.ID;
			other.usesDefiniteArticle = this.usesDefiniteArticle;
			other.descOverride = this.descOverride;
			other.randomSeed = this.randomSeed;
			other.nameLocked = this.nameLocked;
		}

		// Token: 0x0600D756 RID: 55126 RVA: 0x003FE290 File Offset: 0x003FC490
		[CompilerGenerated]
		private void <GetTip>g__TryDrawStatLine|52_0(ref Precept.<>c__DisplayClass52_0 A_1)
		{
			if (A_1.firstStat)
			{
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("PreceptStats".Translate() + ":"));
				A_1.firstStat = false;
			}
		}

		// Token: 0x0600D757 RID: 55127 RVA: 0x003FE2DC File Offset: 0x003FC4DC
		[CompilerGenerated]
		internal static float <GenerateNewName>g__NameUniqueness|58_0(string newName)
		{
			string[] array = newName.Split(' ', StringSplitOptions.None);
			int num = 0;
			int num2 = 0;
			foreach (string text in array)
			{
				if (text.Length >= 4)
				{
					num2++;
					if (Precept.tmpUsedWords.Contains(text))
					{
						num++;
					}
				}
			}
			if (num2 != 0)
			{
				return 1f - (float)num / (float)num2;
			}
			return 1f;
		}

		// Token: 0x0400921B RID: 37403
		public Ideo ideo;

		// Token: 0x0400921C RID: 37404
		public PreceptDef def;

		// Token: 0x0400921D RID: 37405
		protected string name;

		// Token: 0x0400921E RID: 37406
		private int ID = -1;

		// Token: 0x0400921F RID: 37407
		public bool usesDefiniteArticle = true;

		// Token: 0x04009220 RID: 37408
		public string descOverride;

		// Token: 0x04009221 RID: 37409
		public int randomSeed;

		// Token: 0x04009222 RID: 37410
		public bool nameLocked;

		// Token: 0x04009223 RID: 37411
		protected readonly List<Thought_Situational> tmpSituationalThoughts = new List<Thought_Situational>();

		// Token: 0x04009224 RID: 37412
		protected string labelCapCache;

		// Token: 0x04009225 RID: 37413
		protected string tipCached;

		// Token: 0x04009226 RID: 37414
		public const int FloatMenuEditOrder = 2000;

		// Token: 0x04009227 RID: 37415
		public const int FloatMenuRemoveOrder = 1900;

		// Token: 0x04009228 RID: 37416
		public const int FloatMenuRegenerateOrder = 1800;

		// Token: 0x04009229 RID: 37417
		public const float OutlineBrightnessOffset = 0.05f;

		// Token: 0x0400922A RID: 37418
		private static readonly HashSet<string> tmpDescs = new HashSet<string>();

		// Token: 0x0400922B RID: 37419
		protected static readonly StringBuilder tmpCompsDesc = new StringBuilder();

		// Token: 0x0400922C RID: 37420
		private static readonly HashSet<string> tmpUsedWords = new HashSet<string>();
	}
}
