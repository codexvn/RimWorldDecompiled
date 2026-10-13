using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x0200037F RID: 895
	public class LanguageWorker_Czech : LanguageWorker
	{
		// Token: 0x060016FC RID: 5884 RVA: 0x00072DA0 File Offset: 0x00070FA0
		public override string ResolveFunction(string functionName, List<string> args, string fullStringForReference)
		{
			try
			{
				if (functionName == "lookup" && (args.Count == 3 || args.Count == 2))
				{
					if (DebugSettings.logTranslationLookupErrors)
					{
						LanguageWorker_Czech._log.Clear();
						LanguageWorker_Czech._log.AppendLine("ResolveLookup - (" + string.Join(", ", args) + ")");
					}
					return this.DoLookup(args, fullStringForReference);
				}
			}
			finally
			{
				if (DebugSettings.logTranslationLookupErrors && LanguageWorker_Czech._emitLog)
				{
					Log.Message(LanguageWorker_Czech._log.ToString());
					LanguageWorker_Czech._emitLog = false;
					LanguageWorker_Czech._log.Clear();
				}
			}
			if (functionName == "replace" && args.Count > 1)
			{
				return this.DoReplace(args);
			}
			return base.ResolveFunction(functionName, args, fullStringForReference);
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x00072E78 File Offset: 0x00071078
		private string DoLookup(List<string> args, string fullStringForReference)
		{
			if (DebugSettings.logTranslationLookupErrors)
			{
				LanguageWorker_Czech._emitLog = true;
			}
			string text = args[0];
			string text2 = args[1];
			if (Path.DirectorySeparatorChar != '\\')
			{
				text2 = text2.Replace('\\', Path.DirectorySeparatorChar);
				args[1] = text2;
			}
			int num;
			int index = (args.Count == 3) ? (int.TryParse(args[2], out num) ? num : -1) : 1;
			LanguageWorker_Czech.LookupCacheKey lookupCacheKey = new LanguageWorker_Czech.LookupCacheKey(text, text2, index);
			string text3;
			if (this.TryLookup(args, lookupCacheKey, out text3))
			{
				return text3;
			}
			int num2;
			if (LanguageWorker_Czech.TryFindIndexOfAny(text, LanguageWorker_Czech._fastLookupChars, 0, out num2, true))
			{
				if (this.TryLookup(args, text, 0, num2, out text3))
				{
					LanguageWorker_Czech.SaveToLookupCache(lookupCacheKey, text3);
					return text3;
				}
			}
			else
			{
				num2 = 0;
			}
			if (this.TryLookupRecursive(args, text, 0, (num2 == 0) ? text.Length : num2, out text3))
			{
				LanguageWorker_Czech.SaveToLookupCache(lookupCacheKey, text3);
				return text3;
			}
			args[0] = text;
			text3 = base.ResolveLookup(args, fullStringForReference);
			LanguageWorker_Czech.SaveToLookupCache(lookupCacheKey, text3);
			return text3;
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00072F70 File Offset: 0x00071170
		private bool TryLookup(List<string> args, LanguageWorker_Czech.LookupCacheKey lookupCacheKey, out string lookupResult)
		{
			if (LanguageWorker_Czech._lookupCache.TryGetValue(lookupCacheKey, out lookupResult))
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					LanguageWorker_Czech._log.AppendLine(string.Format(" - Cache hit for {0} at index {1}: {2}", lookupCacheKey.Path, lookupCacheKey.Index, lookupResult));
				}
				LanguageWorker_Czech._emitLog = false;
				return true;
			}
			if (this.TryLookup(args, lookupCacheKey.OriginalInput, 0, lookupCacheKey.OriginalInput.Length, out lookupResult))
			{
				LanguageWorker_Czech.SaveToLookupCache(lookupCacheKey, lookupResult);
				return true;
			}
			return false;
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00072FF0 File Offset: 0x000711F0
		private bool TryLookupRecursive(List<string> args, string originalInput, int fromIndex, int count, out string output)
		{
			if (fromIndex + count >= originalInput.Length)
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					LanguageWorker_Czech._emitLog = true;
					LanguageWorker_Czech._log.AppendLine(string.Format("TryLookupRecursive - Invalid range {0} >= {1} (length of '{2}'), returning false.", fromIndex + count, originalInput.Length, originalInput));
				}
				output = null;
				return false;
			}
			int num = 0;
			while (num++ <= 50)
			{
				int num2 = fromIndex + count - 1;
				if (!LanguageWorker_Czech.TryFindWhiteSpaceBackward(originalInput, ref num2, fromIndex, true))
				{
					output = null;
					return false;
				}
				LanguageWorker_Czech.TryFindWhiteSpaceBackward(originalInput, ref num2, fromIndex, false);
				count = num2 - fromIndex;
				if (this.TryLookup(args, originalInput, fromIndex, count, out output))
				{
					return true;
				}
			}
			if (DebugSettings.logTranslationLookupErrors)
			{
				LanguageWorker_Czech._emitLog = true;
				LanguageWorker_Czech._log.AppendLine(string.Format("TryLookupRecursive - Too many iterations ({0}), returning false.", num));
			}
			output = null;
			return false;
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x000730B8 File Offset: 0x000712B8
		private bool TryLookup(List<string> args, string originalInput, int fromIndex, int count, out string output)
		{
			if (DebugSettings.logTranslationLookupErrors)
			{
				LanguageWorker_Czech._log.Append(string.Format("TryLookup: from:{0}, count:{1}", fromIndex, count));
			}
			output = "";
			if (count <= 0)
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					LanguageWorker_Czech._log.AppendLine(" - Invalid count, returning false.");
				}
				return false;
			}
			if (fromIndex != 0 || count != originalInput.Length)
			{
				args[0] = originalInput.Substring(fromIndex, count);
			}
			string text = base.ResolveLookup(args, originalInput);
			if (text == "" || text == args[0])
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					LanguageWorker_Czech._log.AppendLine(" - FAILED - args[0]: '" + args[0] + "'");
				}
				return false;
			}
			bool flag = fromIndex > 0;
			int num = fromIndex + count;
			bool flag2 = num < originalInput.Length;
			output = (flag ? (flag2 ? (originalInput.Substring(0, fromIndex) + text + originalInput.Substring(num)) : (originalInput.Substring(0, fromIndex) + text)) : (flag2 ? (text + originalInput.Substring(num)) : text));
			if (DebugSettings.logTranslationLookupErrors)
			{
				LanguageWorker_Czech._log.AppendLine(string.Format(" - args[0]: '{0}' - return: {1} (lookupResult: {2}, hasPrefix: {3}, hasSuffix: {4}, endLookupIndex: {5})", new object[]
				{
					args[0],
					output,
					text,
					flag,
					flag2,
					num
				}));
			}
			return output != "";
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00073235 File Offset: 0x00071435
		private static void SaveToLookupCache(LanguageWorker_Czech.LookupCacheKey lookupCacheKey, string lookupResult)
		{
			LanguageWorker_Czech.SaveToCache<LanguageWorker_Czech.LookupCacheKey, string>(lookupCacheKey, lookupResult, LanguageWorker_Czech._lookupCache, LanguageWorker_Czech._lookupKeys, ref LanguageWorker_Czech._lookupKeyDeleteIndex, 107);
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00073250 File Offset: 0x00071450
		private string DoReplace(List<string> args)
		{
			if (args.Count == 0)
			{
				return null;
			}
			string text = args[0];
			if (args.Count == 1)
			{
				return text;
			}
			for (int i = 1; i < args.Count; i++)
			{
				string input = args[i];
				Match match = LanguageWorker_Czech._replacePatternArgRegex.Match(input);
				if (!match.Success)
				{
					return null;
				}
				string value = match.Groups["old"].Value;
				string value2 = match.Groups["new"].Value;
				if (text.Contains(value))
				{
					return text.Replace(value, value2);
				}
				Regex replaceRegex = this.GetReplaceRegex(value);
				if (replaceRegex != null)
				{
					string text2 = replaceRegex.Replace(text, value2);
					if (!(text2 == text))
					{
						return text2;
					}
				}
			}
			return text;
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x0007331C File Offset: 0x0007151C
		private Regex GetReplaceRegex(string regexPattern)
		{
			Regex regex;
			if (LanguageWorker_Czech._replaceRegexCache.TryGetValue(regexPattern, out regex))
			{
				return regex;
			}
			int num;
			if (!LanguageWorker_Czech.TryFindIndexOfAny(regexPattern, LanguageWorker_Czech._firstPossibleRegexChars, 0, out num, false))
			{
				LanguageWorker_Czech.SaveToReplaceCache(regexPattern, null);
				return null;
			}
			Regex result;
			try
			{
				regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
				LanguageWorker_Czech.SaveToReplaceCache(regexPattern, regex);
				result = regex;
			}
			catch
			{
				LanguageWorker_Czech.SaveToReplaceCache(regexPattern, null);
				result = null;
			}
			return result;
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00073388 File Offset: 0x00071588
		private static void SaveToReplaceCache(string replacePattern, Regex regex)
		{
			LanguageWorker_Czech.SaveToCache<string, Regex>(replacePattern, regex, LanguageWorker_Czech._replaceRegexCache, LanguageWorker_Czech._replaceRegexKeys, ref LanguageWorker_Czech._replaceRegexKeyDeleteIndex, 107);
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x000733A2 File Offset: 0x000715A2
		private static bool TryFindIndexOfAny(string input, char[] chars, int fromIndex, out int index, bool trim = false)
		{
			index = input.IndexOfAny(chars, fromIndex);
			if (index == -1)
			{
				return false;
			}
			if (trim)
			{
				LanguageWorker_Czech.TryFindWhiteSpaceBackward(input, ref index, fromIndex, false);
			}
			return true;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x000733C4 File Offset: 0x000715C4
		private static bool TryFindWhiteSpaceBackward(string input, ref int index, int stopIndex, bool lookForWhitespace)
		{
			while (index > 0)
			{
				if (index <= stopIndex)
				{
					return false;
				}
				if (char.IsWhiteSpace(input[index - 1]) == lookForWhitespace)
				{
					return true;
				}
				index--;
			}
			return false;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x000733F0 File Offset: 0x000715F0
		private static void SaveToCache<TKey, TValue>(TKey key, TValue value, Dictionary<TKey, TValue> cache, TKey[] keys, ref int deleteIndex, int cacheSize)
		{
			if (cache.Count == cacheSize)
			{
				TKey key2 = keys[deleteIndex];
				cache.Remove(key2);
				keys[deleteIndex] = key;
				deleteIndex = (deleteIndex + 1) % cacheSize;
			}
			else
			{
				keys[cache.Count] = key;
			}
			cache[key] = value;
		}

		// Token: 0x040014D1 RID: 5329
		private const int ReplaceRegexCacheSize = 107;

		// Token: 0x040014D2 RID: 5330
		private const int LookupCacheSize = 107;

		// Token: 0x040014D3 RID: 5331
		private static readonly StringBuilder _log = new StringBuilder();

		// Token: 0x040014D4 RID: 5332
		private static bool _emitLog;

		// Token: 0x040014D5 RID: 5333
		private static readonly Dictionary<LanguageWorker_Czech.LookupCacheKey, string> _lookupCache = new Dictionary<LanguageWorker_Czech.LookupCacheKey, string>(107);

		// Token: 0x040014D6 RID: 5334
		private static readonly LanguageWorker_Czech.LookupCacheKey[] _lookupKeys = new LanguageWorker_Czech.LookupCacheKey[107];

		// Token: 0x040014D7 RID: 5335
		private static int _lookupKeyDeleteIndex;

		// Token: 0x040014D8 RID: 5336
		private static readonly char[] _fastLookupChars = new char[]
		{
			'(',
			'['
		};

		// Token: 0x040014D9 RID: 5337
		private static readonly Dictionary<string, Regex> _replaceRegexCache = new Dictionary<string, Regex>(107);

		// Token: 0x040014DA RID: 5338
		private static readonly string[] _replaceRegexKeys = new string[107];

		// Token: 0x040014DB RID: 5339
		private static int _replaceRegexKeyDeleteIndex;

		// Token: 0x040014DC RID: 5340
		private static readonly char[] _firstPossibleRegexChars = new char[]
		{
			'(',
			'[',
			'{',
			'*',
			'+',
			'?',
			'.',
			'^',
			'$',
			'|'
		};

		// Token: 0x040014DD RID: 5341
		private static readonly Regex _replacePatternArgRegex = new Regex("(?<old>[^\"]*?)\"-\"(?<new>[^\"]*?)\"", RegexOptions.Compiled);

		// Token: 0x02000380 RID: 896
		private readonly struct LookupCacheKey : IEquatable<LanguageWorker_Czech.LookupCacheKey>
		{
			// Token: 0x17000450 RID: 1104
			// (get) Token: 0x0600170A RID: 5898 RVA: 0x000734CB File Offset: 0x000716CB
			public string OriginalInput { get; }

			// Token: 0x17000451 RID: 1105
			// (get) Token: 0x0600170B RID: 5899 RVA: 0x000734D3 File Offset: 0x000716D3
			public string Path { get; }

			// Token: 0x17000452 RID: 1106
			// (get) Token: 0x0600170C RID: 5900 RVA: 0x000734DB File Offset: 0x000716DB
			public int Index { get; }

			// Token: 0x0600170D RID: 5901 RVA: 0x000734E3 File Offset: 0x000716E3
			public LookupCacheKey(string originalInput, string path, int index)
			{
				this.OriginalInput = originalInput;
				this.Path = path;
				this.Index = index;
			}

			// Token: 0x0600170E RID: 5902 RVA: 0x000734FA File Offset: 0x000716FA
			private bool Equals(LanguageWorker_Czech.LookupCacheKey other)
			{
				return this.Index == other.Index && string.Equals(this.Path, other.Path, StringComparison.Ordinal) && string.Equals(this.OriginalInput, other.OriginalInput, StringComparison.Ordinal);
			}

			// Token: 0x0600170F RID: 5903 RVA: 0x00073538 File Offset: 0x00071738
			public override bool Equals(object obj)
			{
				if (obj is LanguageWorker_Czech.LookupCacheKey)
				{
					LanguageWorker_Czech.LookupCacheKey other = (LanguageWorker_Czech.LookupCacheKey)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x06001710 RID: 5904 RVA: 0x0007355D File Offset: 0x0007175D
			public override int GetHashCode()
			{
				int num = this.Index * 397;
				string path = this.Path;
				int num2 = (num ^ ((path != null) ? path.GetHashCode() : 0)) * 397;
				string originalInput = this.OriginalInput;
				return num2 ^ ((originalInput != null) ? originalInput.GetHashCode() : 0);
			}

			// Token: 0x06001711 RID: 5905 RVA: 0x00073597 File Offset: 0x00071797
			bool IEquatable<LanguageWorker_Czech.LookupCacheKey>.Equals(LanguageWorker_Czech.LookupCacheKey other)
			{
				return this.Equals(other);
			}
		}
	}
}
