using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x02000DE9 RID: 3561
	public class RulePack
	{
		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06005AAF RID: 23215 RVA: 0x001D1694 File Offset: 0x001CF894
		public List<Rule> Rules
		{
			get
			{
				if (this.rulesResolved == null)
				{
					this.rulesResolved = RulePack.GetRulesResolved(this.rulesRaw, this.rulesStrings, this.rulesFiles);
					if (this.include != null)
					{
						foreach (RulePackDef rulePackDef in this.include)
						{
							this.rulesResolved.AddRange(rulePackDef.RulesPlusIncludes);
						}
					}
				}
				return this.rulesResolved;
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06005AB0 RID: 23216 RVA: 0x001D1724 File Offset: 0x001CF924
		public List<Rule> UntranslatedRules
		{
			get
			{
				if (this.untranslatedRulesResolved == null)
				{
					this.untranslatedRulesResolved = RulePack.GetRulesResolved(this.untranslatedRulesRaw, this.untranslatedRulesStrings, this.untranslatedRulesFiles);
					if (this.include != null)
					{
						foreach (RulePackDef rulePackDef in this.include)
						{
							this.untranslatedRulesResolved.AddRange(rulePackDef.UntranslatedRulesPlusIncludes);
						}
					}
				}
				return this.untranslatedRulesResolved;
			}
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x001D17B4 File Offset: 0x001CF9B4
		public void PostLoad()
		{
			this.untranslatedRulesStrings = this.rulesStrings.ToList<string>();
			this.untranslatedRulesFiles = this.rulesFiles.ToList<string>();
			if (this.rulesRaw != null)
			{
				this.untranslatedRulesRaw = new List<Rule>();
				for (int i = 0; i < this.rulesRaw.Count; i++)
				{
					this.untranslatedRulesRaw.Add(this.rulesRaw[i].DeepCopy());
				}
			}
		}

		// Token: 0x06005AB2 RID: 23218 RVA: 0x001D1828 File Offset: 0x001CFA28
		private static List<Rule> GetRulesResolved(List<Rule> rulesRaw, List<string> rulesStrings, List<string> rulesFiles)
		{
			List<Rule> list = new List<Rule>();
			for (int i = 0; i < rulesStrings.Count; i++)
			{
				try
				{
					Rule_String rule_String = new Rule_String(rulesStrings[i]);
					rule_String.Init();
					list.Add(rule_String);
				}
				catch (Exception ex)
				{
					string str = "Exception parsing grammar rule from ";
					string str2 = rulesStrings[i];
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			for (int j = 0; j < rulesFiles.Count; j++)
			{
				try
				{
					string[] array = rulesFiles[j].Split(new string[]
					{
						"->"
					}, StringSplitOptions.None);
					Rule_File rule_File = new Rule_File();
					rule_File.keyword = array[0].Trim();
					rule_File.path = array[1].Trim();
					rule_File.Init();
					list.Add(rule_File);
				}
				catch (Exception ex3)
				{
					string str4 = "Error initializing Rule_File ";
					string str5 = rulesFiles[j];
					string str6 = ": ";
					Exception ex4 = ex3;
					Log.Error(str4 + str5 + str6 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
			if (rulesRaw != null)
			{
				for (int k = 0; k < rulesRaw.Count; k++)
				{
					try
					{
						rulesRaw[k].Init();
						list.Add(rulesRaw[k]);
					}
					catch (Exception ex5)
					{
						string str7 = "Error initializing rule ";
						string str8 = rulesRaw[k].ToStringSafe<Rule>();
						string str9 = ": ";
						Exception ex6 = ex5;
						Log.Error(str7 + str8 + str9 + ((ex6 != null) ? ex6.ToString() : null));
					}
				}
			}
			return list;
		}

		// Token: 0x0400419B RID: 16795
		[MustTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesStrings = new List<string>();

		// Token: 0x0400419C RID: 16796
		[MayTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesFiles = new List<string>();

		// Token: 0x0400419D RID: 16797
		private List<Rule> rulesRaw;

		// Token: 0x0400419E RID: 16798
		public List<RulePackDef> include;

		// Token: 0x0400419F RID: 16799
		[Unsaved(false)]
		private List<Rule> rulesResolved;

		// Token: 0x040041A0 RID: 16800
		[Unsaved(false)]
		private List<Rule> untranslatedRulesResolved;

		// Token: 0x040041A1 RID: 16801
		[Unsaved(false)]
		private List<string> untranslatedRulesStrings;

		// Token: 0x040041A2 RID: 16802
		[Unsaved(false)]
		private List<string> untranslatedRulesFiles;

		// Token: 0x040041A3 RID: 16803
		[Unsaved(false)]
		private List<Rule> untranslatedRulesRaw;
	}
}
