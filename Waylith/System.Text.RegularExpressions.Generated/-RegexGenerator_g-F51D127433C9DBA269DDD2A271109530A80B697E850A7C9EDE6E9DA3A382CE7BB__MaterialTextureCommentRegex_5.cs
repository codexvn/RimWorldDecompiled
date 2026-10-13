using System.CodeDom.Compiler;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialTextureCommentRegex_5 : Regex
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
				if (num <= inputSpan.Length - 28)
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
				int num9 = 0;
				int num10 = 0;
				int num11 = 0;
				int pos = 0;
				int num12 = 0;
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
				int num13 = span.IndexOf(')');
				if (num13 < 0)
				{
					num13 = span.Length;
				}
				span = span.Slice(num13);
				num += num13;
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
				if (span.IsEmpty)
				{
					UncaptureUntil(0);
					return false;
				}
				switch (span[0])
				{
				case 's':
				{
					if (!span.Slice(1).StartsWith("ampler".AsSpan()))
					{
						UncaptureUntil(0);
						return false;
					}
					int num15 = span.Slice(7).IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
					if (num15 < 0)
					{
						num15 = span.Length - 7;
					}
					span = span.Slice(num15);
					num += num15;
					num += 7;
					span = inputSpan.Slice(num);
					break;
				}
				case 't':
				{
					if (!span.Slice(1).StartsWith("exture".AsSpan()))
					{
						UncaptureUntil(0);
						return false;
					}
					int num14 = span.Slice(7).IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
					if (num14 < 0)
					{
						num14 = span.Length - 7;
					}
					if (num14 == 0)
					{
						UncaptureUntil(0);
						return false;
					}
					span = span.Slice(num14);
					num += num14;
					num += 7;
					span = inputSpan.Slice(num);
					break;
				}
				default:
					UncaptureUntil(0);
					return false;
				}
				int l;
				for (l = 0; (uint)l < (uint)span.Length && char.IsWhiteSpace(span[l]); l++)
				{
				}
				if (l == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				span = span.Slice(l);
				num += l;
				num3 = num;
				ulong num16;
				if (span.IsEmpty || (long)((ulong)(-261993005120L << (int)(num16 = (uint)(span[0] - 65))) & (num16 - 64)) >= 0L)
				{
					UncaptureUntil(0);
					return false;
				}
				num++;
				span = inputSpan.Slice(num);
				num7 = num;
				int num17 = span.IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
				if (num17 < 0)
				{
					num17 = span.Length;
				}
				span = span.Slice(num17);
				num += num17;
				num8 = num;
				while (true)
				{
					num5 = Crawlpos();
					Capture(2, num3, num);
					num9 = num;
					int m;
					for (m = 0; (uint)m < (uint)span.Length && char.IsWhiteSpace(span[m]); m++)
					{
					}
					span = span.Slice(m);
					num += m;
					num10 = num;
					while (true)
					{
						num6 = Crawlpos();
						num12 = pos;
						num11 = 0;
						while (true)
						{
							_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
							num11++;
							if (!span.IsEmpty && span[0] == '[')
							{
								int num18 = span.Slice(1).IndexOf(']');
								if (num18 < 0)
								{
									num18 = span.Length - 1;
								}
								if (num18 != 0)
								{
									span = span.Slice(num18);
									num += num18;
									if ((uint)span.Length >= 2u && span[1] == ']')
									{
										num += 2;
										span = inputSpan.Slice(num);
										if (num11 == 0)
										{
											continue;
										}
										goto IL_0515;
									}
								}
							}
							if (--num11 < 0)
							{
								break;
							}
							num = runstack[--pos];
							UncaptureUntil(runstack[--pos]);
							span = inputSpan.Slice(num);
							goto IL_0515;
							IL_0515:
							pos = num12;
							int n;
							for (n = 0; (uint)n < (uint)span.Length && char.IsWhiteSpace(span[n]); n++)
							{
							}
							span = span.Slice(n);
							num += n;
							if (span.IsEmpty || span[0] != ';')
							{
								break;
							}
							int num19;
							for (num19 = 1; (uint)num19 < (uint)span.Length; num19++)
							{
								char c;
								if ((((c = span[num19]) < '\u0080') ? ("ᨀ\0\u0001\0\0\0\0\0"[(int)c >> 4] & (1 << (c & 0xF))) : (RegexRunner.CharInClass(c, "\u0001\u0004\u0001\n\v\r\u000eﾜ") ? 1 : 0)) == 0)
								{
									break;
								}
							}
							span = span.Slice(num19);
							num += num19;
							if (!span.StartsWith("//".AsSpan()))
							{
								break;
							}
							num += 2;
							span = inputSpan.Slice(num);
							num4 = num;
							int num20 = span.IndexOfAny('\n', '\r');
							if (num20 < 0)
							{
								num20 = span.Length;
							}
							span = span.Slice(num20);
							num += num20;
							Capture(3, num4, num);
							runtextpos = num;
							Capture(0, start, num);
							return true;
						}
						UncaptureUntil(num6);
						if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
						{
							CheckTimeout();
						}
						if (num9 >= num10)
						{
							break;
						}
						num = --num10;
						span = inputSpan.Slice(num);
					}
					UncaptureUntil(num5);
					if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
					if (num7 >= num8)
					{
						break;
					}
					num = --num8;
					span = inputSpan.Slice(num);
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

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialTextureCommentRegex_5 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialTextureCommentRegex_5();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialTextureCommentRegex_5()
	{
		pattern = "layout\\s*\\((?<layout>[^)]*)\\)\\s*uniform\\s+(?:sampler[A-Za-z0-9_]*|texture[A-Za-z0-9_]+)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*(?:\\[[^\\]]+\\])?\\s*;[^\\S\\r\\n]*//(?<comment>[^\\r\\n]*)";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		base.CapNames = new Hashtable
		{
			{ "0", 0 },
			{ "comment", 3 },
			{ "layout", 1 },
			{ "name", 2 }
		};
		capslist = new string[4] { "0", "layout", "name", "comment" };
		capsize = 4;
	}
}
