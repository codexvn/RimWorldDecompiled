using System.CodeDom.Compiler;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__ShaderVertexInputRegex_8 : Regex
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
				if (num <= inputSpan.Length - 25)
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
				int arg = 0;
				int arg2 = 0;
				int num8 = 0;
				int num9 = 0;
				int num10 = 0;
				int pos = 0;
				int num11 = 0;
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
				int j;
				for (j = 1; (uint)j < (uint)span.Length && char.IsWhiteSpace(span[j]); j++)
				{
				}
				span = span.Slice(j);
				num += j;
				if (!span.StartsWith("location".AsSpan()))
				{
					UncaptureUntil(0);
					return false;
				}
				int k;
				for (k = 8; (uint)k < (uint)span.Length && char.IsWhiteSpace(span[k]); k++)
				{
				}
				span = span.Slice(k);
				num += k;
				if (span.IsEmpty || span[0] != '=')
				{
					UncaptureUntil(0);
					return false;
				}
				int l;
				for (l = 1; (uint)l < (uint)span.Length && char.IsWhiteSpace(span[l]); l++)
				{
				}
				span = span.Slice(l);
				num += l;
				num2 = num;
				int m;
				for (m = 0; (uint)m < (uint)span.Length && char.IsDigit(span[m]); m++)
				{
				}
				if (m == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				span = span.Slice(m);
				num += m;
				Capture(1, num2, num);
				int n;
				for (n = 0; (uint)n < (uint)span.Length && char.IsWhiteSpace(span[n]); n++)
				{
				}
				span = span.Slice(n);
				num += n;
				if (span.IsEmpty || span[0] != ')')
				{
					UncaptureUntil(0);
					return false;
				}
				int num12;
				for (num12 = 1; (uint)num12 < (uint)span.Length && char.IsWhiteSpace(span[num12]); num12++)
				{
				}
				span = span.Slice(num12);
				num += num12;
				if (!span.StartsWith("in".AsSpan()))
				{
					UncaptureUntil(0);
					return false;
				}
				num += 2;
				span = inputSpan.Slice(num);
				int num13;
				for (num13 = 0; (uint)num13 < (uint)span.Length && char.IsWhiteSpace(span[num13]); num13++)
				{
				}
				if (num13 == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				span = span.Slice(num13);
				num += num13;
				num3 = num;
				int num14 = span.IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
				if (num14 < 0)
				{
					num14 = span.Length;
				}
				if (num14 == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				span = span.Slice(num14);
				num += num14;
				Capture(2, num3, num);
				int num15;
				for (num15 = 0; (uint)num15 < (uint)span.Length && char.IsWhiteSpace(span[num15]); num15++)
				{
				}
				if (num15 == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				span = span.Slice(num15);
				num += num15;
				num4 = num;
				ulong num16;
				if (span.IsEmpty || (long)((ulong)(-261993005120L << (int)(num16 = (uint)(span[0] - 65))) & (num16 - 64)) >= 0L)
				{
					UncaptureUntil(0);
					return false;
				}
				int num17 = span.Slice(1).IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
				if (num17 < 0)
				{
					num17 = span.Length - 1;
				}
				span = span.Slice(num17);
				num += num17;
				num++;
				span = inputSpan.Slice(num);
				Capture(3, num4, num);
				int num18;
				for (num18 = 0; (uint)num18 < (uint)span.Length && char.IsWhiteSpace(span[num18]); num18++)
				{
				}
				span = span.Slice(num18);
				num += num18;
				if (span.IsEmpty || span[0] != ';')
				{
					UncaptureUntil(0);
					return false;
				}
				num++;
				span = inputSpan.Slice(num);
				num6 = num;
				int num19;
				for (num19 = 0; (uint)num19 < (uint)span.Length && char.IsWhiteSpace(span[num19]); num19++)
				{
				}
				span = span.Slice(num19);
				num += num19;
				num7 = num;
				while (true)
				{
					num5 = Crawlpos();
					int num20 = pos;
					num11 = pos;
					num8 = 0;
					while (true)
					{
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
						num8++;
						int start2;
						if (span.StartsWith("//".AsSpan()))
						{
							int num21;
							for (num21 = 2; (uint)num21 < (uint)span.Length && char.IsWhiteSpace(span[num21]); num21++)
							{
							}
							span = span.Slice(num21);
							num += num21;
							if (!span.IsEmpty && span[0] == '@')
							{
								num++;
								span = inputSpan.Slice(num);
								start2 = num;
								if (!span.IsEmpty)
								{
									char c = span[0];
									if (c != 'i')
									{
										if (c == 'v' && span.Slice(1).StartsWith("ertex".AsSpan()))
										{
											num += 6;
											span = inputSpan.Slice(num);
											goto IL_05e2;
										}
									}
									else if (span.Slice(1).StartsWith("nstance".AsSpan()))
									{
										num += 8;
										span = inputSpan.Slice(num);
										goto IL_05e2;
									}
								}
							}
						}
						goto IL_094b;
						IL_0928:
						int num22;
						span = span.Slice(num22);
						num += num22;
						int start3;
						Capture(7, start3, num);
						if (num8 == 0)
						{
							continue;
						}
						goto IL_0985;
						IL_05e2:
						Capture(4, start2, num);
						num9 = 0;
						while (true)
						{
							_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
							num9++;
							int num23;
							for (num23 = 0; (uint)num23 < (uint)span.Length && char.IsWhiteSpace(span[num23]); num23++)
							{
							}
							int num24;
							if (num23 != 0)
							{
								span = span.Slice(num23);
								num += num23;
								num24 = num;
								if (!span.IsEmpty && (long)((ulong)(-261993005120L << (int)(num16 = (uint)(span[0] - 65))) & (num16 - 64)) < 0L)
								{
									num++;
									span = inputSpan.Slice(num);
									arg = num;
									int num25 = span.IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
									if (num25 < 0)
									{
										num25 = span.Length;
									}
									span = span.Slice(num25);
									num += num25;
									arg2 = num;
									goto IL_070e;
								}
							}
							goto IL_075e;
							IL_08fb:
							_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, num10);
							start3 = num;
							num22 = span.IndexOfAny('\n', '\r');
							if (num22 < 0)
							{
								num22 = span.Length;
							}
							goto IL_0928;
							IL_070e:
							_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, arg, arg2, Crawlpos());
							Capture(5, num24, num);
							_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, num24);
							if (num9 == 0)
							{
								continue;
							}
							goto IL_07b0;
							IL_07b0:
							_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, num9);
							num10 = 0;
							while (true)
							{
								_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
								num10++;
								int num26;
								for (num26 = 0; (uint)num26 < (uint)span.Length && char.IsWhiteSpace(span[num26]); num26++)
								{
								}
								if (num26 == 0)
								{
									break;
								}
								span = span.Slice(num26);
								num += num26;
								int start4 = num;
								if (span.IsEmpty)
								{
									break;
								}
								switch (span[0])
								{
								case 'r':
									if (!span.Slice(1).StartsWith("equired".AsSpan()))
									{
										break;
									}
									num += 8;
									span = inputSpan.Slice(num);
									goto IL_08ae;
								case 'o':
									if (!span.Slice(1).StartsWith("ptional".AsSpan()))
									{
										break;
									}
									num += 8;
									span = inputSpan.Slice(num);
									goto IL_08ae;
								}
								break;
								IL_08ae:
								Capture(6, start4, num);
								if (num10 == 0)
								{
									continue;
								}
								goto IL_08fb;
							}
							if (--num10 < 0)
							{
								num9 = runstack[--pos];
								if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
								{
									CheckTimeout();
								}
								if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
								{
									CheckTimeout();
								}
								if (num9 != 0)
								{
									num24 = runstack[--pos];
									UncaptureUntil(runstack[--pos]);
									_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPop(runstack, ref pos, out arg2, out arg);
									if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
									{
										CheckTimeout();
									}
									if (arg < arg2)
									{
										num = --arg2;
										span = inputSpan.Slice(num);
										goto IL_070e;
									}
									goto IL_075e;
								}
								break;
							}
							num = runstack[--pos];
							UncaptureUntil(runstack[--pos]);
							span = inputSpan.Slice(num);
							goto IL_08fb;
							IL_075e:
							if (--num9 < 0)
							{
								break;
							}
							num = runstack[--pos];
							UncaptureUntil(runstack[--pos]);
							span = inputSpan.Slice(num);
							goto IL_07b0;
						}
						goto IL_094b;
						IL_094b:
						if (--num8 < 0)
						{
							break;
						}
						num = runstack[--pos];
						UncaptureUntil(runstack[--pos]);
						span = inputSpan.Slice(num);
						goto IL_0985;
						IL_0985:
						pos = num11;
						pos = num20;
						runtextpos = num;
						Capture(0, start, num);
						return true;
					}
					UncaptureUntil(num5);
					if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
					if (num6 >= num7)
					{
						break;
					}
					num = --num7;
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

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__ShaderVertexInputRegex_8 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__ShaderVertexInputRegex_8();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__ShaderVertexInputRegex_8()
	{
		pattern = "layout\\s*\\(\\s*location\\s*=\\s*(?<location>\\d+)\\s*\\)\\s*in\\s+(?<type>[A-Za-z0-9_]+)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*;\\s*(?://\\s*@(?<kind>vertex|instance)(?:\\s+(?<semantic>[A-Za-z_][A-Za-z0-9_]*))?(?:\\s+(?<presence>required|optional))?(?<tail>[^\\r\\n]*))?";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		base.CapNames = new Hashtable
		{
			{ "0", 0 },
			{ "kind", 4 },
			{ "location", 1 },
			{ "name", 3 },
			{ "presence", 6 },
			{ "semantic", 5 },
			{ "tail", 7 },
			{ "type", 2 }
		};
		capslist = new string[8] { "0", "location", "type", "name", "kind", "semantic", "presence", "tail" };
		capsize = 8;
	}
}
