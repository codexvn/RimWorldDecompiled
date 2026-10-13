using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000DD6 RID: 3542
	public struct GrammarRequest
	{
		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06005A2F RID: 23087 RVA: 0x001CE0C5 File Offset: 0x001CC2C5
		public List<Rule> RulesAllowNull
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06005A30 RID: 23088 RVA: 0x001CE0CD File Offset: 0x001CC2CD
		public List<Rule> Rules
		{
			get
			{
				if (this.rules == null)
				{
					this.rules = new List<Rule>();
				}
				return this.rules;
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06005A31 RID: 23089 RVA: 0x001CE0E8 File Offset: 0x001CC2E8
		public List<RulePack> IncludesBareAllowNull
		{
			get
			{
				return this.includesBare;
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06005A32 RID: 23090 RVA: 0x001CE0F0 File Offset: 0x001CC2F0
		public List<RulePack> IncludesBare
		{
			get
			{
				if (this.includesBare == null)
				{
					this.includesBare = new List<RulePack>();
				}
				return this.includesBare;
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06005A33 RID: 23091 RVA: 0x001CE10B File Offset: 0x001CC30B
		public List<RulePackDef> IncludesAllowNull
		{
			get
			{
				return this.includes;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06005A34 RID: 23092 RVA: 0x001CE113 File Offset: 0x001CC313
		public List<RulePackDef> Includes
		{
			get
			{
				if (this.includes == null)
				{
					this.includes = new List<RulePackDef>();
				}
				return this.includes;
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06005A35 RID: 23093 RVA: 0x001CE12E File Offset: 0x001CC32E
		public Dictionary<string, string> ConstantsAllowNull
		{
			get
			{
				return this.constants;
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06005A36 RID: 23094 RVA: 0x001CE136 File Offset: 0x001CC336
		public Dictionary<string, string> Constants
		{
			get
			{
				if (this.constants == null)
				{
					this.constants = new Dictionary<string, string>();
				}
				return this.constants;
			}
		}

		// Token: 0x06005A37 RID: 23095 RVA: 0x001CE154 File Offset: 0x001CC354
		public bool HasRule(string keyword)
		{
			GrammarRequest.<>c__DisplayClass22_0 CS$<>8__locals1 = new GrammarRequest.<>c__DisplayClass22_0();
			CS$<>8__locals1.keyword = keyword;
			return (this.rules != null && this.rules.Any(new Predicate<Rule>(CS$<>8__locals1.<HasRule>g__HasTargetRule|0))) || (this.includes != null && this.includes.Any((RulePackDef i) => i.RulesPlusIncludes.Any(new Predicate<Rule>(base.<HasRule>g__HasTargetRule|0)))) || (this.includesBare != null && this.includesBare.Any((RulePack rp) => rp.Rules.Any(new Predicate<Rule>(base.<HasRule>g__HasTargetRule|0))));
		}

		// Token: 0x06005A38 RID: 23096 RVA: 0x001CE1D8 File Offset: 0x001CC3D8
		public void Clear()
		{
			if (this.rules != null)
			{
				this.rules.Clear();
			}
			if (this.includesBare != null)
			{
				this.includesBare.Clear();
			}
			if (this.includes != null)
			{
				this.includes.Clear();
			}
			if (this.constants != null)
			{
				this.constants.Clear();
			}
		}

		// Token: 0x04004105 RID: 16645
		private List<Rule> rules;

		// Token: 0x04004106 RID: 16646
		private List<RulePack> includesBare;

		// Token: 0x04004107 RID: 16647
		private List<RulePackDef> includes;

		// Token: 0x04004108 RID: 16648
		private Dictionary<string, string> constants;

		// Token: 0x04004109 RID: 16649
		public GrammarRequest.ICustomizer customizer;

		// Token: 0x02000DD7 RID: 3543
		public interface ICustomizer
		{
			// Token: 0x06005A39 RID: 23097
			int PriorityBucket(Rule rule);

			// Token: 0x06005A3A RID: 23098
			void Notify_RuleUsed(Rule rule);

			// Token: 0x06005A3B RID: 23099
			bool ValidateRule(Rule rule);
		}
	}
}
