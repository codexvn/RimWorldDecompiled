using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02003671 RID: 13937
	public static class NameGenerator
	{
		// Token: 0x06013587 RID: 79239 RVA: 0x005C16EC File Offset: 0x005BF8EC
		public static string GenerateName(RulePackDef rootPack, IEnumerable<string> extantNames, bool appendNumberIfNameUsed = false, string rootKeyword = null)
		{
			return NameGenerator.GenerateName(rootPack, (string x) => !extantNames.Contains(x), appendNumberIfNameUsed, rootKeyword, null, null);
		}

		// Token: 0x06013588 RID: 79240 RVA: 0x005C171C File Offset: 0x005BF91C
		public static string GenerateName(RulePackDef rootPack, Predicate<string> validator = null, bool appendNumberIfNameUsed = false, string rootKeyword = null, string testPawnNameSymbol = null, List<Rule> extraRules = null)
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(rootPack);
			if (testPawnNameSymbol != null)
			{
				request.Rules.Add(new Rule_String("ANYPAWN_nameDef", testPawnNameSymbol));
				request.Rules.Add(new Rule_String("ANYPAWN_nameIndef", testPawnNameSymbol));
			}
			if (!extraRules.NullOrEmpty<Rule>())
			{
				request.Rules.AddRange(extraRules);
			}
			string rootKeyword2 = rootKeyword ?? rootPack.FirstRuleKeyword;
			string untranslatedRootKeyword = rootKeyword ?? rootPack.FirstUntranslatedRuleKeyword;
			return NameGenerator.GenerateName(request, validator, appendNumberIfNameUsed, rootKeyword2, untranslatedRootKeyword);
		}

		// Token: 0x06013589 RID: 79241 RVA: 0x005C17AC File Offset: 0x005BF9AC
		public static string GenerateName(GrammarRequest request, Predicate<string> validator = null, bool appendNumberIfNameUsed = false, string rootKeyword = null, string untranslatedRootKeyword = null)
		{
			if (untranslatedRootKeyword == null)
			{
				untranslatedRootKeyword = rootKeyword;
			}
			string text = "ErrorName";
			if (appendNumberIfNameUsed)
			{
				for (int i = 0; i < 100; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						text = GenText.CapitalizeAsTitle(GrammarResolver.Resolve(rootKeyword, request, null, false, untranslatedRootKeyword, null, null, true));
						if (i != 0)
						{
							text = text + " " + (i + 1).ToString();
						}
						if (validator == null || validator(text))
						{
							return text;
						}
					}
				}
				return GenText.CapitalizeAsTitle(GrammarResolver.Resolve(rootKeyword, request, null, false, untranslatedRootKeyword, null, null, true));
			}
			for (int k = 0; k < 150; k++)
			{
				text = GenText.CapitalizeAsTitle(GrammarResolver.Resolve(rootKeyword, request, null, false, untranslatedRootKeyword, null, null, true));
				if (validator == null || validator(text))
				{
					return text;
				}
			}
			Log.Error("Could not get new name (first rule pack: " + request.Includes.FirstOrDefault<RulePackDef>().ToStringSafe<RulePackDef>() + ")");
			return text;
		}
	}
}
