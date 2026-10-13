using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld.QuestGen;

namespace Verse.Grammar
{
	// Token: 0x02000DED RID: 3565
	public class Rule_String : Rule
	{
		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06005ABD RID: 23229 RVA: 0x001D1D30 File Offset: 0x001CFF30
		public override float BaseSelectionWeight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06005ABE RID: 23230 RVA: 0x001D1D38 File Offset: 0x001CFF38
		public override float Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06005ABF RID: 23231 RVA: 0x001D1D40 File Offset: 0x001CFF40
		public bool OutputNull
		{
			get
			{
				return this.output == null;
			}
		}

		// Token: 0x06005AC0 RID: 23232 RVA: 0x001D1D4B File Offset: 0x001CFF4B
		public override Rule DeepCopy()
		{
			Rule_String rule_String = (Rule_String)base.DeepCopy();
			rule_String.output = this.output;
			rule_String.weight = this.weight;
			rule_String.priority = this.priority;
			return rule_String;
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x001D1D7C File Offset: 0x001CFF7C
		public Rule_String()
		{
		}

		// Token: 0x06005AC2 RID: 23234 RVA: 0x001D1D8F File Offset: 0x001CFF8F
		public Rule_String(string keyword, string output)
		{
			this.keyword = keyword;
			this.output = output;
		}

		// Token: 0x06005AC3 RID: 23235 RVA: 0x001D1DB0 File Offset: 0x001CFFB0
		public Rule_String(string rawString)
		{
			Match match = Rule_String.Pattern.Match(rawString);
			if (!match.Success)
			{
				Log.Error("Bad string pass when reading rule " + rawString);
				return;
			}
			this.keyword = match.Groups["keyword"].Value;
			this.output = match.Groups["output"].Value;
			for (int i = 0; i < match.Groups["paramname"].Captures.Count; i++)
			{
				string value = match.Groups["paramname"].Captures[i].Value;
				string value2 = match.Groups["paramoperator"].Captures[i].Value;
				string value3 = match.Groups["paramvalue"].Captures[i].Value;
				if (value == "p")
				{
					if (value2 != "=")
					{
						Log.Error("Attempt to compare p instead of assigning in rule " + rawString);
					}
					this.weight = float.Parse(value3);
				}
				else if (value == "priority")
				{
					if (value2 != "=")
					{
						Log.Error("Attempt to compare priority instead of assigning in rule " + rawString);
					}
					this.priority = float.Parse(value3);
				}
				else if (value == "tag")
				{
					if (value2 != "=")
					{
						Log.Error("Attempt to compare tag instead of assigning in rule " + rawString);
					}
					this.tag = value3;
				}
				else if (value == "requiredTag")
				{
					if (value2 != "=")
					{
						Log.Error("Attempt to compare requiredTag instead of assigning in rule " + rawString);
					}
					this.requiredTag = value3;
				}
				else if (value == "uses")
				{
					if (value2 != "=")
					{
						Log.Error("Attempt to compare uses instead of assigning in rule " + rawString);
					}
					this.usesLimit = new int?(int.Parse(value3));
				}
				else if (value == "debug")
				{
					Log.Error("Rule " + rawString + " contains debug flag; fix before commit");
				}
				else if (Rule_String.ValidOperators.Contains(value2))
				{
					base.AddConstantConstraint(value, value3, value2);
				}
				else
				{
					Log.Error("Unknown parameter " + value + " in rule " + rawString);
				}
			}
		}

		// Token: 0x06005AC4 RID: 23236 RVA: 0x001D202F File Offset: 0x001D022F
		public override string Generate()
		{
			return this.output;
		}

		// Token: 0x06005AC5 RID: 23237 RVA: 0x001D2037 File Offset: 0x001D0237
		public override string ToString()
		{
			return (this.keyword ?? "null_keyword") + " → " + ((this.output != null) ? this.output.Replace("\n", "\\n") : "null_output");
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x001D2078 File Offset: 0x001D0278
		public void AppendPrefixToAllKeywords(string prefix)
		{
			Rule_String.tmpPrefix = prefix;
			if (this.output == null)
			{
				Log.Error("Rule_String output was null.");
				this.output = "";
			}
			this.output = Regex.Replace(this.output, "\\[(.*?)\\]", new MatchEvaluator(Rule_String.RegexMatchEvaluatorAppendPrefix));
			if (this.constantConstraints != null)
			{
				for (int i = 0; i < this.constantConstraints.Count; i++)
				{
					Rule.ConstantConstraint constantConstraint = default(Rule.ConstantConstraint);
					constantConstraint.key = this.constantConstraints[i].key;
					if (!prefix.NullOrEmpty())
					{
						constantConstraint.key = prefix + "/" + constantConstraint.key;
					}
					constantConstraint.key = QuestGenUtility.NormalizeVarPath(constantConstraint.key);
					constantConstraint.value = this.constantConstraints[i].value;
					constantConstraint.type = this.constantConstraints[i].type;
					this.constantConstraints[i] = constantConstraint;
				}
			}
		}

		// Token: 0x06005AC7 RID: 23239 RVA: 0x001D2180 File Offset: 0x001D0380
		private static string RegexMatchEvaluatorAppendPrefix(Match match)
		{
			string text = match.Groups[1].Value;
			if (!Rule_String.tmpPrefix.NullOrEmpty())
			{
				text = Rule_String.tmpPrefix + "/" + text;
			}
			text = QuestGenUtility.NormalizeVarPath(text);
			return "[" + text + "]";
		}

		// Token: 0x040041B3 RID: 16819
		[MustTranslate]
		private string output;

		// Token: 0x040041B4 RID: 16820
		private float weight = 1f;

		// Token: 0x040041B5 RID: 16821
		private float priority;

		// Token: 0x040041B6 RID: 16822
		private static readonly List<string> ValidOperators = new List<string>
		{
			"==",
			"!=",
			">",
			"<",
			">=",
			"<=",
			"[less_than]",
			"[greater_than]"
		};

		// Token: 0x040041B7 RID: 16823
		private static readonly Regex Pattern = new Regex("\r\n        # hold on to your butts, this is gonna get weird\r\n\r\n        ^\r\n        (?<keyword>[a-zA-Z0-9_/]+)\t\t\t\t\t# keyword; roughly limited to standard C# identifier rules\r\n        (\t\t\t\t\t\t\t\t\t\t\t# parameter list is optional, open the capture group so we can keep it or ignore it\r\n            \\(\t\t\t\t\t\t\t\t\t\t# this is the actual parameter list opening\r\n                (\t\t\t\t\t\t\t\t\t# unlimited number of parameter groups\r\n                    [ ]*                            # leading whitespace for readability\r\n                    (?<paramname>[a-zA-Z0-9_/]+)\t# parameter name is similar\r\n                    (?<paramoperator>==|=|!=|>=|<=|>|<|\\[less_than\\]|\\[greater_than\\]) # operators; empty operator is allowed\r\n                    (?<paramvalue>[^\\,\\)]*)\t\t\t# parameter value, however, allows everything except comma and closeparen!\r\n                    ,?\t\t\t\t\t\t\t\t# comma can be used to separate blocks; it is also silently ignored if it's a trailing comma\r\n                )*\r\n            \\)\r\n        )?\r\n        [ ]*                                        # leading whitespace before -> for readability\r\n        ->(?<output>.*)\t\t\t\t\t\t\t\t# output is anything-goes\r\n        $\r\n\r\n        ", RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

		// Token: 0x040041B8 RID: 16824
		private static string tmpPrefix;
	}
}
