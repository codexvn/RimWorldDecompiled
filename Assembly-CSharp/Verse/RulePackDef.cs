using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020002DD RID: 733
	public class RulePackDef : Def
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x00054264 File Offset: 0x00052464
		public List<Rule> RulesPlusIncludes
		{
			get
			{
				if (this.cachedRules == null)
				{
					this.cachedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedRules.AddRange(this.rulePack.Rules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedRules.AddRange(this.include[i].RulesPlusIncludes);
						}
					}
				}
				return this.cachedRules;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x000542E4 File Offset: 0x000524E4
		public List<Rule> UntranslatedRulesPlusIncludes
		{
			get
			{
				if (this.cachedUntranslatedRules == null)
				{
					this.cachedUntranslatedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedUntranslatedRules.AddRange(this.rulePack.UntranslatedRules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedUntranslatedRules.AddRange(this.include[i].UntranslatedRulesPlusIncludes);
						}
					}
				}
				return this.cachedUntranslatedRules;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x00054362 File Offset: 0x00052562
		public List<Rule> RulesImmediate
		{
			get
			{
				RulePack rulePack = this.rulePack;
				if (rulePack == null)
				{
					return null;
				}
				return rulePack.Rules;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x00054375 File Offset: 0x00052575
		public List<Rule> UntranslatedRulesImmediate
		{
			get
			{
				RulePack rulePack = this.rulePack;
				if (rulePack == null)
				{
					return null;
				}
				return rulePack.UntranslatedRules;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x00054388 File Offset: 0x00052588
		public string FirstRuleKeyword
		{
			get
			{
				List<Rule> rulesPlusIncludes = this.RulesPlusIncludes;
				if (!rulesPlusIncludes.Any<Rule>())
				{
					return "none";
				}
				return rulesPlusIncludes[0].keyword;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x000543B8 File Offset: 0x000525B8
		public string FirstUntranslatedRuleKeyword
		{
			get
			{
				List<Rule> untranslatedRulesPlusIncludes = this.UntranslatedRulesPlusIncludes;
				if (!untranslatedRulesPlusIncludes.Any<Rule>())
				{
					return "none";
				}
				return untranslatedRulesPlusIncludes[0].keyword;
			}
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x000543E6 File Offset: 0x000525E6
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.include != null)
			{
				int num;
				for (int i = 0; i < this.include.Count; i = num + 1)
				{
					if (this.include[i].include != null && this.include[i].include.Contains(this))
					{
						yield return "includes other RulePackDef which includes it: " + this.include[i].defName;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x000543F6 File Offset: 0x000525F6
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}

		// Token: 0x04001048 RID: 4168
		public List<RulePackDef> include;

		// Token: 0x04001049 RID: 4169
		private RulePack rulePack;

		// Token: 0x0400104A RID: 4170
		public bool directTestable;

		// Token: 0x0400104B RID: 4171
		[Unsaved(false)]
		private List<Rule> cachedRules;

		// Token: 0x0400104C RID: 4172
		[Unsaved(false)]
		private List<Rule> cachedUntranslatedRules;
	}
}
