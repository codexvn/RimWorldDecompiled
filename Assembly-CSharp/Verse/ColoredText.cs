using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200090C RID: 2316
	public static class ColoredText
	{
		// Token: 0x06003C07 RID: 15367 RVA: 0x0013AB90 File Offset: 0x00138D90
		public static void ResetStaticData()
		{
			ColoredText.DateTimeRegexes = new List<Regex>();
			ColoredText.AddRegexPatternsForDateString("PeriodYears".Translate(), "Period1Year".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodQuadrums".Translate(), "Period1Quadrum".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodDays".Translate(), "Period1Day".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodHours".Translate(), "Period1Hour".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodSeconds".Translate(), "Period1Second".Translate());
			string str = string.Concat(new string[]
			{
				"(",
				FactionDefOf.PlayerColony.pawnsPlural,
				"|",
				FactionDefOf.PlayerColony.pawnSingular,
				")"
			});
			ColoredText.ColonistCountRegex = new Regex("\\d+\\.?\\d* " + str);
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x0013ACAC File Offset: 0x00138EAC
		private static void AddRegexPatternsForDateString(string dateMany, string dateOne)
		{
			if (dateMany.Contains("{0}"))
			{
				ColoredText.DateTimeRegexes.Add(new Regex(string.Concat(new string[]
				{
					"(",
					string.Format(dateMany, "\\d+\\.?\\d*"),
					"|",
					dateOne,
					")"
				})));
				return;
			}
			List<string> list = GrammarResolverSimple.TryParseNumCase(dateMany);
			if (!list.NullOrEmpty<string>())
			{
				foreach (string text in list)
				{
					ColoredText.DateTimeRegexes.Add(new Regex(string.Concat(new string[]
					{
						"(\\d+\\.?\\d* ",
						text,
						"|",
						dateOne,
						")"
					})));
				}
			}
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x0013AD90 File Offset: 0x00138F90
		public static void ClearCache()
		{
			ColoredText.cache.Clear();
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x0013AD9C File Offset: 0x00138F9C
		public static TaggedString ApplyTag(this string s, TagType tagType, string arg = null)
		{
			if (arg == null)
			{
				return string.Format("(*{0}){1}(/{0})", tagType.ToString(), s);
			}
			return string.Format("(*{0}={1}){2}(/{0})", tagType.ToString(), arg, s);
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x0013ADE8 File Offset: 0x00138FE8
		public static TaggedString ApplyTag(this string s, Faction faction)
		{
			if (faction == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Faction, faction.GetUniqueLoadID());
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x0013AE01 File Offset: 0x00139001
		public static TaggedString ApplyTag(this string s, Ideo ideo)
		{
			if (ideo == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Ideo, ideo.GetUniqueLoadID());
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x0013AE1C File Offset: 0x0013901C
		public static string StripTags(this string s)
		{
			if (s.NullOrEmpty() || (s.IndexOf("(*") < 0 && s.IndexOf('<') < 0))
			{
				return s;
			}
			s = ColoredText.XMLRegex.Replace(s, string.Empty);
			return ColoredText.TagRegex.Replace(s, string.Empty);
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x0013AE6E File Offset: 0x0013906E
		public static string ResolveTags(this string str)
		{
			return ColoredText.Resolve(str);
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x0013AE7C File Offset: 0x0013907C
		public static string Resolve(TaggedString taggedStr)
		{
			if (taggedStr == null)
			{
				return null;
			}
			string rawText = taggedStr.RawText;
			if (rawText.NullOrEmpty())
			{
				return rawText;
			}
			string result;
			if (ColoredText.cache.TryGetValue(rawText, out result))
			{
				return result;
			}
			ColoredText.resultBuffer.Length = 0;
			if (rawText.IndexOf("(*") < 0)
			{
				ColoredText.resultBuffer.Append(rawText);
			}
			else
			{
				for (int i = 0; i < rawText.Length; i++)
				{
					char c = rawText[i];
					if (c == '(' && i < rawText.Length - 1 && rawText[i + 1] == '*' && rawText.IndexOf(')', i) > i + 1)
					{
						bool flag = false;
						int num = i;
						ColoredText.tagBuffer.Length = 0;
						ColoredText.argBuffer.Length = 0;
						ColoredText.capStage = ColoredText.CaptureStage.Tag;
						for (i += 2; i < rawText.Length; i++)
						{
							char c2 = rawText[i];
							if (c2 == ')')
							{
								ColoredText.capStage = ColoredText.CaptureStage.Result;
								if (flag)
								{
									string value = rawText.Substring(num, i - num + 1).SwapTagWithColor(ColoredText.tagBuffer.ToString(), ColoredText.argBuffer.ToString());
									ColoredText.resultBuffer.Append(value);
									break;
								}
							}
							else if (c2 == '/')
							{
								flag = true;
							}
							if (ColoredText.capStage == ColoredText.CaptureStage.Arg)
							{
								ColoredText.argBuffer.Append(c2);
							}
							if (!flag && c2 == '=')
							{
								ColoredText.capStage = ColoredText.CaptureStage.Arg;
							}
							if (ColoredText.capStage == ColoredText.CaptureStage.Tag)
							{
								ColoredText.tagBuffer.Append(c2);
							}
						}
						if (!flag)
						{
							ColoredText.resultBuffer.Append(c);
							i = num + 1;
						}
					}
					else
					{
						ColoredText.resultBuffer.Append(c);
					}
				}
			}
			string text = ColoredText.resultBuffer.ToString();
			for (int j = 0; j < ColoredText.DateTimeRegexes.Count; j++)
			{
				text = ColoredText.DateTimeRegexes[j].Replace(text, "$&".Colorize(ColoredText.DateTimeColor));
			}
			text = ColoredText.CurrencyRegex.Replace(text, "$&".Colorize(ColoredText.CurrencyColor));
			text = ColoredText.ColonistCountRegex.Replace(text, "$&".Colorize(ColoredText.ColonistCountColor));
			ColoredText.cache.Add(rawText, text);
			return text;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x0013B0B4 File Offset: 0x001392B4
		public static string Colorize(this TaggedString ts, Color color)
		{
			return ts.Resolve().Colorize(color);
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x0013B0C3 File Offset: 0x001392C3
		public static string Colorize(this string s, Color color)
		{
			return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), s);
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x0013B0D8 File Offset: 0x001392D8
		private static string SwapTagWithColor(this string str, string tag, string arg)
		{
			TagType tagType = ColoredText.ParseEnum<TagType>(tag.CapitalizeFirst(), true);
			string text = str.StripTags();
			switch (tagType)
			{
			case TagType.Undefined:
				return str;
			case TagType.Name:
				return text.Colorize(ColoredText.NameColor);
			case TagType.Faction:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Faction faction = Find.FactionManager.AllFactions.ToList<Faction>().Find((Faction x) => x.GetUniqueLoadID() == arg);
				if (faction == null)
				{
					return text.Colorize(ColoredText.SubtleGrayColor);
				}
				return text.Colorize(ColoredText.GetFactionRelationColor(faction));
			}
			case TagType.Settlement:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Faction faction2 = Find.FactionManager.AllFactionsVisible.ToList<Faction>().Find((Faction x) => x.GetUniqueLoadID() == arg);
				if (faction2 == null)
				{
					return text.Colorize(ColoredText.SubtleGrayColor);
				}
				if (faction2 == null)
				{
					return text;
				}
				return text.Colorize(faction2.Color);
			}
			case TagType.DateTime:
				return text.Colorize(ColoredText.DateTimeColor);
			case TagType.ColonistCount:
				return text.Colorize(ColoredText.ColonistCountColor);
			case TagType.Threat:
				return text.Colorize(ColoredText.ThreatColor);
			case TagType.Ideo:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Ideo ideo = Find.IdeoManager.IdeosListForReading.Find((Ideo x) => x.GetUniqueLoadID() == arg);
				if (ideo == null)
				{
					return text;
				}
				return text.Colorize(ideo.TextColor);
			}
			case TagType.SectionTitle:
				return text.Colorize(ColoredText.TipSectionTitleColor);
			case TagType.Red:
				return text.Colorize(ColorLibrary.RedReadable);
			case TagType.Reward:
				return text.Colorize(ColoredText.CurrencyColor);
			case TagType.Gray:
				return text.Colorize(ColoredText.SubtleGrayColor);
			default:
				Log.ErrorOnce("Invalid tag '" + tag + "'", tag.GetHashCode());
				return text;
			}
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x0013B2A0 File Offset: 0x001394A0
		private static Color GetFactionRelationColor(Faction faction)
		{
			if (faction == null)
			{
				return Color.white;
			}
			if (faction.IsPlayer)
			{
				return faction.Color;
			}
			switch (faction.RelationKindWith(Faction.OfPlayer))
			{
			case FactionRelationKind.Hostile:
				return ColoredText.FactionColor_Hostile;
			case FactionRelationKind.Neutral:
				return ColoredText.FactionColor_Neutral;
			case FactionRelationKind.Ally:
				return ColoredText.FactionColor_Ally;
			default:
				return faction.Color;
			}
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x0013B300 File Offset: 0x00139500
		private static T ParseEnum<T>(string value, bool ignoreCase = true)
		{
			if (Enum.IsDefined(typeof(T), value))
			{
				return (T)((object)Enum.Parse(typeof(T), value, ignoreCase));
			}
			return default(T);
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x0013B33F File Offset: 0x0013953F
		public static StringBuilder AppendTagged(this StringBuilder sb, TaggedString taggedString)
		{
			return sb.Append(taggedString.Resolve());
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x0013B34E File Offset: 0x0013954E
		public static StringBuilder AppendLineTagged(this StringBuilder sb, TaggedString taggedString)
		{
			return sb.AppendLine(taggedString.Resolve());
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x0013B35D File Offset: 0x0013955D
		public static TaggedString ToTaggedString(this StringBuilder sb)
		{
			return new TaggedString(sb.ToString());
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x0013B36A File Offset: 0x0013956A
		public static string AsTipTitle(this TaggedString ts)
		{
			return ts.Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x0013B377 File Offset: 0x00139577
		public static string AsTipTitle(this string s)
		{
			return s.Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x04002B22 RID: 11042
		private static StringBuilder resultBuffer = new StringBuilder();

		// Token: 0x04002B23 RID: 11043
		private static StringBuilder tagBuffer = new StringBuilder();

		// Token: 0x04002B24 RID: 11044
		private static StringBuilder argBuffer = new StringBuilder();

		// Token: 0x04002B25 RID: 11045
		private static Dictionary<string, string> cache = new Dictionary<string, string>();

		// Token: 0x04002B26 RID: 11046
		private static ColoredText.CaptureStage capStage = ColoredText.CaptureStage.Result;

		// Token: 0x04002B27 RID: 11047
		private static Regex ColonistCountRegex;

		// Token: 0x04002B28 RID: 11048
		private static List<Regex> DateTimeRegexes;

		// Token: 0x04002B29 RID: 11049
		public static readonly Color NameColor = GenColor.FromHex("d09b61");

		// Token: 0x04002B2A RID: 11050
		public static readonly Color CurrencyColor = GenColor.FromHex("dbb40c");

		// Token: 0x04002B2B RID: 11051
		public static readonly Color TipSectionTitleColor = new Color(0.9f, 0.9f, 0.3f);

		// Token: 0x04002B2C RID: 11052
		public static readonly Color DateTimeColor = GenColor.FromHex("87f6f6");

		// Token: 0x04002B2D RID: 11053
		public static readonly Color FactionColor_Ally = GenColor.FromHex("00ff00");

		// Token: 0x04002B2E RID: 11054
		public static readonly Color FactionColor_Hostile = ColorLibrary.RedReadable;

		// Token: 0x04002B2F RID: 11055
		public static readonly Color ThreatColor = GenColor.FromHex("d46f68");

		// Token: 0x04002B30 RID: 11056
		public static readonly Color FactionColor_Neutral = GenColor.FromHex("00bfff");

		// Token: 0x04002B31 RID: 11057
		public static readonly Color WarningColor = GenColor.FromHex("ff0000");

		// Token: 0x04002B32 RID: 11058
		public static readonly Color ColonistCountColor = GenColor.FromHex("dcffaf");

		// Token: 0x04002B33 RID: 11059
		public static readonly Color SubtleGrayColor = GenColor.FromHex("999999");

		// Token: 0x04002B34 RID: 11060
		public static readonly Color ExpectationsColor = new Color(0.57f, 0.9f, 0.69f);

		// Token: 0x04002B35 RID: 11061
		public static readonly Color ImpactColor = GenColor.FromHex("c79fef");

		// Token: 0x04002B36 RID: 11062
		public static readonly Color GeneColor = ColorLibrary.LightBlue;

		// Token: 0x04002B37 RID: 11063
		private static readonly Regex CurrencyRegex = new Regex("\\$\\d+\\.?\\d*");

		// Token: 0x04002B38 RID: 11064
		private static readonly Regex TagRegex = new Regex("\\([\\*\\/][^\\)]*\\)");

		// Token: 0x04002B39 RID: 11065
		private static readonly Regex XMLRegex = new Regex("<[^>]*>");

		// Token: 0x04002B3A RID: 11066
		private const string Digits = "\\d+\\.?\\d*";

		// Token: 0x04002B3B RID: 11067
		private const string Replacement = "$&";

		// Token: 0x04002B3C RID: 11068
		private const string TagStartString = "(*";

		// Token: 0x04002B3D RID: 11069
		private const char TagStartChar = '(';

		// Token: 0x04002B3E RID: 11070
		private const char TagEndChar = ')';

		// Token: 0x0200090D RID: 2317
		private enum CaptureStage
		{
			// Token: 0x04002B40 RID: 11072
			Tag,
			// Token: 0x04002B41 RID: 11073
			Arg,
			// Token: 0x04002B42 RID: 11074
			Result
		}
	}
}
