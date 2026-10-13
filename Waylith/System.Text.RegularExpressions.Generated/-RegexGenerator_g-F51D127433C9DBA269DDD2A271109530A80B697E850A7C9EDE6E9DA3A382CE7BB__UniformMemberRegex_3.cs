using System.CodeDom.Compiler;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__UniformMemberRegex_3 : Regex
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
				if (num <= inputSpan.Length - 6)
				{
					int num2 = inputSpan.Slice(num).IndexOfAny(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_ascii_40324000);
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
				int num12 = 0;
				int num13 = 0;
				int num14 = 0;
				int num15 = 0;
				int num16 = 0;
				int num17 = 0;
				int pos = 0;
				int num18 = 0;
				int num19 = 0;
				ReadOnlySpan<char> span = inputSpan.Slice(num);
				num15 = 0;
				while (true)
				{
					_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
					num15++;
					if (span.StartsWith("layout".AsSpan()))
					{
						int i;
						for (i = 6; (uint)i < (uint)span.Length && char.IsWhiteSpace(span[i]); i++)
						{
						}
						span = span.Slice(i);
						num += i;
						if (!span.IsEmpty && span[0] == '(')
						{
							int num20 = span.Slice(1).IndexOf(')');
							if (num20 < 0)
							{
								num20 = span.Length - 1;
							}
							span = span.Slice(num20);
							num += num20;
							if ((uint)span.Length >= 2u && span[1] == ')')
							{
								int j;
								for (j = 2; (uint)j < (uint)span.Length && char.IsWhiteSpace(span[j]); j++)
								{
								}
								span = span.Slice(j);
								num += j;
								if (num15 == 0)
								{
									continue;
								}
								goto IL_0198;
							}
						}
					}
					goto IL_0158;
					IL_0158:
					if (--num15 < 0)
					{
						break;
					}
					num = runstack[--pos];
					UncaptureUntil(runstack[--pos]);
					span = inputSpan.Slice(num);
					goto IL_0198;
					IL_0198:
					num2 = num;
					if (!span.IsEmpty)
					{
						char c = span[0];
						if ((uint)c <= 105u)
						{
							if (c != 'f')
							{
								if (c != 'i' || !span.Slice(1).StartsWith("nt".AsSpan()))
								{
									goto IL_0158;
								}
								num += 3;
								span = inputSpan.Slice(num);
							}
							else
							{
								if (!span.Slice(1).StartsWith("loat".AsSpan()))
								{
									goto IL_0158;
								}
								num += 5;
								span = inputSpan.Slice(num);
							}
						}
						else if (c != 'm')
						{
							if (c != 'v' || (uint)span.Length < 4u || !span.Slice(1).StartsWith("ec".AsSpan()) || !char.IsBetween(span[3], '2', '4'))
							{
								goto IL_0158;
							}
							num += 4;
							span = inputSpan.Slice(num);
						}
						else
						{
							if ((uint)span.Length < 4u || !span.Slice(1).StartsWith("at".AsSpan()) || !char.IsBetween(span[3], '3', '4'))
							{
								goto IL_0158;
							}
							num += 4;
							span = inputSpan.Slice(num);
						}
						Capture(1, num2, num);
						int k;
						for (k = 0; (uint)k < (uint)span.Length && char.IsWhiteSpace(span[k]); k++)
						{
						}
						if (k != 0)
						{
							span = span.Slice(k);
							num += k;
							num3 = num;
							ulong num21;
							if (!span.IsEmpty && (long)((ulong)(-261993005120L << (int)(num21 = (uint)(span[0] - 65))) & (num21 - 64)) < 0L)
							{
								num++;
								span = inputSpan.Slice(num);
								num7 = num;
								int num22 = span.IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
								if (num22 < 0)
								{
									num22 = span.Length;
								}
								span = span.Slice(num22);
								num += num22;
								num8 = num;
								while (true)
								{
									num4 = Crawlpos();
									Capture(2, num3, num);
									num9 = num;
									int l;
									for (l = 0; (uint)l < (uint)span.Length && char.IsWhiteSpace(span[l]); l++)
									{
									}
									span = span.Slice(l);
									num += l;
									num10 = num;
									while (true)
									{
										num5 = Crawlpos();
										num18 = pos;
										num16 = 0;
										while (true)
										{
											_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
											num16++;
											int start2 = num;
											if (!span.IsEmpty && span[0] == '[')
											{
												int num23 = span.Slice(1).IndexOf(']');
												if (num23 < 0)
												{
													num23 = span.Length - 1;
												}
												if (num23 != 0)
												{
													span = span.Slice(num23);
													num += num23;
													if ((uint)span.Length >= 2u && span[1] == ']')
													{
														num += 2;
														span = inputSpan.Slice(num);
														Capture(3, start2, num);
														if (num16 == 0)
														{
															continue;
														}
														goto IL_052b;
													}
												}
											}
											if (--num16 < 0)
											{
												break;
											}
											num = runstack[--pos];
											UncaptureUntil(runstack[--pos]);
											span = inputSpan.Slice(num);
											goto IL_052b;
											IL_052b:
											pos = num18;
											int m;
											for (m = 0; (uint)m < (uint)span.Length && char.IsWhiteSpace(span[m]); m++)
											{
											}
											span = span.Slice(m);
											num += m;
											if (span.IsEmpty || span[0] != ';')
											{
												break;
											}
											num++;
											span = inputSpan.Slice(num);
											num11 = num;
											int n;
											for (n = 0; (uint)n < (uint)span.Length && char.IsWhiteSpace(span[n]); n++)
											{
											}
											span = span.Slice(n);
											num += n;
											num12 = num;
											while (true)
											{
												num6 = Crawlpos();
												int num24 = pos;
												num19 = pos;
												num17 = 0;
												while (true)
												{
													_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
													num17++;
													if (!span.StartsWith("//".AsSpan()))
													{
														goto IL_075a;
													}
													num += 2;
													span = inputSpan.Slice(num);
													num13 = num;
													int num25;
													for (num25 = 0; (uint)num25 < (uint)span.Length && char.IsWhiteSpace(span[num25]); num25++)
													{
													}
													span = span.Slice(num25);
													num += num25;
													num14 = num;
													int start3;
													int num26;
													while (true)
													{
														_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, num13, num14, Crawlpos());
														start3 = num;
														num26 = span.IndexOfAny('\n', '\r');
														if (num26 < 0)
														{
															num26 = span.Length;
														}
														if (num26 != 0)
														{
															break;
														}
														UncaptureUntil(runstack[--pos]);
														_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPop(runstack, ref pos, out num14, out num13);
														if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
														{
															CheckTimeout();
														}
														if (num13 < num14 && (num14 = inputSpan.Slice(num13, num14 - num13).LastIndexOfAnyExcept('\n', '\r')) >= 0)
														{
															num14 += num13;
															num = num14;
															span = inputSpan.Slice(num);
															continue;
														}
														goto IL_075a;
													}
													span = span.Slice(num26);
													num += num26;
													Capture(4, start3, num);
													if (num17 == 0)
													{
														continue;
													}
													goto IL_0794;
													IL_0794:
													pos = num19;
													pos = num24;
													runtextpos = num;
													Capture(0, start, num);
													return true;
													IL_075a:
													if (--num17 < 0)
													{
														break;
													}
													num = runstack[--pos];
													UncaptureUntil(runstack[--pos]);
													span = inputSpan.Slice(num);
													goto IL_0794;
												}
												UncaptureUntil(num6);
												if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
												{
													CheckTimeout();
												}
												if (num11 >= num12)
												{
													break;
												}
												num = --num12;
												span = inputSpan.Slice(num);
											}
											break;
										}
										UncaptureUntil(num5);
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
									UncaptureUntil(num4);
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
							}
						}
					}
					goto IL_0158;
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

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__UniformMemberRegex_3 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__UniformMemberRegex_3();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__UniformMemberRegex_3()
	{
		pattern = "(?:layout\\s*\\([^)]*\\)\\s*)?(?<type>mat3|mat4|vec2|vec3|vec4|int|float)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*(?<array>\\[[^\\]]+\\])?\\s*;\\s*(?://\\s*(?<comment>[^\\r\\n]+))?";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		base.CapNames = new Hashtable
		{
			{ "0", 0 },
			{ "array", 3 },
			{ "comment", 4 },
			{ "name", 2 },
			{ "type", 1 }
		};
		capslist = new string[5] { "0", "type", "name", "array", "comment" };
		capsize = 5;
	}
}
