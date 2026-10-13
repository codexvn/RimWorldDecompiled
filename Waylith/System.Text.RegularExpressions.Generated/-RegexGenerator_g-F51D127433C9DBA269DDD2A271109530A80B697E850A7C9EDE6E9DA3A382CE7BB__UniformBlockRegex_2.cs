using System.CodeDom.Compiler;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__UniformBlockRegex_2 : Regex
{
	private sealed class RunnerFactory : RegexRunnerFactory
	{
		private sealed class Runner : RegexRunner
		{
			protected override void Scan(ReadOnlySpan<char> inputSpan)
			{
				while (TryFindNextPossibleStartingPosition(inputSpan) && !TryMatchAtCurrentPosition(inputSpan) && runtextpos != inputSpan.Length)
				{
					runtextpos++;
					if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
				}
			}

			private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				if (num <= inputSpan.Length - 20)
				{
					int num2 = inputSpan.Slice(num).IndexOfAny(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_indexOfString_layout_Ordinal);
					if (num2 >= 0)
					{
						runtextpos = num + num2;
						return true;
					}
				}
				runtextpos = inputSpan.Length;
				return false;
			}

			private bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				int start = num;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				ReadOnlySpan<char> span = inputSpan.Slice(num);
				if (!span.StartsWith("layout".AsSpan()))
				{
					UncaptureUntil(0);
					return false;
				}
				int i;
				for (i = 6; (uint)i < (uint)span.Length && char.IsWhiteSpace(span[i]); i++)
				{
				}
				span = span.Slice(i);
				num += i;
				if (span.IsEmpty || span[0] != '(')
				{
					UncaptureUntil(0);
					return false;
				}
				num++;
				span = inputSpan.Slice(num);
				num2 = num;
				int num9 = span.IndexOf(')');
				if (num9 < 0)
				{
					num9 = span.Length;
				}
				span = span.Slice(num9);
				num += num9;
				Capture(1, num2, num);
				if (span.IsEmpty || span[0] != ')')
				{
					UncaptureUntil(0);
					return false;
				}
				int j;
				for (j = 1; (uint)j < (uint)span.Length && char.IsWhiteSpace(span[j]); j++)
				{
				}
				span = span.Slice(j);
				num += j;
				if (!span.StartsWith("uniform".AsSpan()))
				{
					UncaptureUntil(0);
					return false;
				}
				num += 7;
				span = inputSpan.Slice(num);
				int k;
				for (k = 0; (uint)k < (uint)span.Length && char.IsWhiteSpace(span[k]); k++)
				{
				}
				if (k == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				span = span.Slice(k);
				num += k;
				int l;
				for (l = 0; (uint)l < (uint)span.Length && _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.IsWordChar(span[l]); l++)
				{
				}
				if (l == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				span = span.Slice(l);
				num += l;
				int m;
				for (m = 0; (uint)m < (uint)span.Length && char.IsWhiteSpace(span[m]); m++)
				{
				}
				span = span.Slice(m);
				num += m;
				if (span.IsEmpty || span[0] != '{')
				{
					UncaptureUntil(0);
					return false;
				}
				num++;
				span = inputSpan.Slice(num);
				num3 = num;
				num8 = num;
				while (true)
				{
					num7 = Crawlpos();
					Capture(2, num3, num);
					if (!span.IsEmpty && span[0] == '}')
					{
						num++;
						span = inputSpan.Slice(num);
						num5 = num;
						int n;
						for (n = 0; (uint)n < (uint)span.Length && char.IsWhiteSpace(span[n]); n++)
						{
						}
						span = span.Slice(n);
						num += n;
						num6 = num;
						while (true)
						{
							num4 = Crawlpos();
							int num10;
							for (num10 = 0; (uint)num10 < (uint)span.Length && _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.IsWordChar(span[num10]); num10++)
							{
							}
							span = span.Slice(num10);
							num += num10;
							int num11;
							for (num11 = 0; (uint)num11 < (uint)span.Length && char.IsWhiteSpace(span[num11]); num11++)
							{
							}
							span = span.Slice(num11);
							num += num11;
							if (span.IsEmpty || span[0] != ';')
							{
								UncaptureUntil(num4);
								if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
								{
									CheckTimeout();
								}
								if (num5 >= num6)
								{
									break;
								}
								num = --num6;
								span = inputSpan.Slice(num);
								continue;
							}
							Capture(0, start, runtextpos = num + 1);
							return true;
						}
					}
					UncaptureUntil(num7);
					if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
					num = num8;
					span = inputSpan.Slice(num);
					if (span.IsEmpty)
					{
						UncaptureUntil(0);
						return false;
					}
					num++;
					span = inputSpan.Slice(num);
					num8 = span.IndexOf('}');
					if (num8 < 0)
					{
						break;
					}
					num += num8;
					span = inputSpan.Slice(num);
					num8 = num;
				}
				UncaptureUntil(0);
				return false;
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				void UncaptureUntil(int capturePosition)
				{
					while (Crawlpos() > capturePosition)
					{
						Uncapture();
					}
				}
			}
		}

		protected override RegexRunner CreateInstance()
		{
			return new Runner();
		}
	}

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__UniformBlockRegex_2 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__UniformBlockRegex_2();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__UniformBlockRegex_2()
	{
		pattern = "layout\\s*\\((?<layout>[^)]*)\\)\\s*uniform\\s+\\w+\\s*\\{(?<body>.*?)\\}\\s*\\w*\\s*;";
		roptions = RegexOptions.Singleline | RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		base.CapNames = new Hashtable
		{
			{ "0", 0 },
			{ "body", 2 },
			{ "layout", 1 }
		};
		capslist = new string[3] { "0", "layout", "body" };
		capsize = 3;
	}
}
