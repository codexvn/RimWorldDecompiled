using System.CodeDom.Compiler;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VaryingRegex_9 : Regex
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
				if (num <= inputSpan.Length - 15)
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
				int num12 = 0;
				int num13 = 0;
				int num14 = 0;
				int num15 = 0;
				int num16 = 0;
				int pos = 0;
				int num17 = 0;
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
				int num18 = span.IndexOf(')');
				if (num18 < 0)
				{
					num18 = span.Length;
				}
				span = span.Slice(num18);
				num += num18;
				Capture(1, num2, num);
				if (span.IsEmpty || span[0] != ')')
				{
					UncaptureUntil(0);
					return false;
				}
				num++;
				span = inputSpan.Slice(num);
				num9 = num;
				int j;
				for (j = 0; (uint)j < (uint)span.Length && char.IsWhiteSpace(span[j]); j++)
				{
				}
				span = span.Slice(j);
				num += j;
				num10 = num;
				while (true)
				{
					num6 = Crawlpos();
					num15 = 0;
					while (true)
					{
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
						num15++;
						int arg = num;
						int arg2 = Crawlpos();
						if (!span.StartsWith("flat".AsSpan()))
						{
							goto IL_0202;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 0, arg, arg2);
						num += 4;
						span = inputSpan.Slice(num);
						goto IL_04c5;
						IL_04c5:
						int k;
						for (k = 0; (uint)k < (uint)span.Length && char.IsWhiteSpace(span[k]); k++)
						{
						}
						if (k != 0)
						{
							span = span.Slice(k);
							num += k;
							continue;
						}
						goto IL_046c;
						IL_0202:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (!span.StartsWith("smooth".AsSpan()))
						{
							goto IL_024f;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 1, arg, arg2);
						num += 6;
						span = inputSpan.Slice(num);
						goto IL_04c5;
						IL_03d2:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (!span.StartsWith("mediump".AsSpan()))
						{
							goto IL_041f;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 7, arg, arg2);
						num += 7;
						span = inputSpan.Slice(num);
						goto IL_04c5;
						IL_024f:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (!span.StartsWith("noperspective".AsSpan()))
						{
							goto IL_029d;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 2, arg, arg2);
						num += 13;
						span = inputSpan.Slice(num);
						goto IL_04c5;
						IL_046c:
						if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
						{
							CheckTimeout();
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPop(runstack, ref pos, out arg2, out arg);
						switch (runstack[--pos])
						{
						case 0:
							break;
						case 1:
							goto IL_024f;
						case 2:
							goto IL_029d;
						case 3:
							goto IL_02ea;
						case 4:
							goto IL_0337;
						case 5:
							goto IL_0385;
						case 6:
							goto IL_03d2;
						case 7:
							goto IL_041f;
						default:
							goto IL_04c5;
						case 8:
							goto IL_0508;
						}
						goto IL_0202;
						IL_029d:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (!span.StartsWith("centroid".AsSpan()))
						{
							goto IL_02ea;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 3, arg, arg2);
						num += 8;
						span = inputSpan.Slice(num);
						goto IL_04c5;
						IL_041f:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (span.StartsWith("lowp".AsSpan()))
						{
							_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 8, arg, arg2);
							num += 4;
							span = inputSpan.Slice(num);
							goto IL_04c5;
						}
						goto IL_0508;
						IL_02ea:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (!span.StartsWith("sample".AsSpan()))
						{
							goto IL_0337;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 4, arg, arg2);
						num += 6;
						span = inputSpan.Slice(num);
						goto IL_04c5;
						IL_0544:
						if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
						{
							CheckTimeout();
						}
						if (num15 == 0)
						{
							break;
						}
						goto IL_046c;
						IL_0337:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (!span.StartsWith("invariant".AsSpan()))
						{
							goto IL_0385;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 5, arg, arg2);
						num += 9;
						span = inputSpan.Slice(num);
						goto IL_04c5;
						IL_0508:
						if (--num15 < 0)
						{
							break;
						}
						num = runstack[--pos];
						UncaptureUntil(runstack[--pos]);
						span = inputSpan.Slice(num);
						num3 = num;
						if (!span.IsEmpty)
						{
							char c = span[0];
							if (c != 'i')
							{
								if (c != 'o' || !span.Slice(1).StartsWith("ut".AsSpan()))
								{
									goto IL_0544;
								}
								num += 3;
								span = inputSpan.Slice(num);
							}
							else
							{
								if ((uint)span.Length < 2u || span[1] != 'n')
								{
									goto IL_0544;
								}
								num += 2;
								span = inputSpan.Slice(num);
							}
							Capture(2, num3, num);
							int l;
							for (l = 0; (uint)l < (uint)span.Length && char.IsWhiteSpace(span[l]); l++)
							{
							}
							if (l != 0)
							{
								span = span.Slice(l);
								num += l;
								num4 = num;
								ulong num19;
								if (!span.IsEmpty && (long)((ulong)(-261993005120L << (int)(num19 = (uint)(span[0] - 65))) & (num19 - 64)) < 0L)
								{
									int num20 = span.Slice(1).IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
									if (num20 < 0)
									{
										num20 = span.Length - 1;
									}
									span = span.Slice(num20);
									num += num20;
									num++;
									span = inputSpan.Slice(num);
									Capture(3, num4, num);
									int m;
									for (m = 0; (uint)m < (uint)span.Length && char.IsWhiteSpace(span[m]); m++)
									{
									}
									if (m != 0)
									{
										span = span.Slice(m);
										num += m;
										num5 = num;
										if (!span.IsEmpty && (long)((ulong)(-261993005120L << (int)(num19 = (uint)(span[0] - 65))) & (num19 - 64)) < 0L)
										{
											num++;
											span = inputSpan.Slice(num);
											num11 = num;
											int num21 = span.IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
											if (num21 < 0)
											{
												num21 = span.Length;
											}
											span = span.Slice(num21);
											num += num21;
											num12 = num;
											while (true)
											{
												num7 = Crawlpos();
												Capture(4, num5, num);
												num13 = num;
												int n;
												for (n = 0; (uint)n < (uint)span.Length && char.IsWhiteSpace(span[n]); n++)
												{
												}
												span = span.Slice(n);
												num += n;
												num14 = num;
												while (true)
												{
													num8 = Crawlpos();
													num17 = pos;
													num16 = 0;
													while (true)
													{
														_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
														num16++;
														int start2 = num;
														if (!span.IsEmpty && span[0] == '[')
														{
															int num22 = span.Slice(1).IndexOf(']');
															if (num22 < 0)
															{
																num22 = span.Length - 1;
															}
															span = span.Slice(num22);
															num += num22;
															if ((uint)span.Length >= 2u && span[1] == ']')
															{
																num += 2;
																span = inputSpan.Slice(num);
																Capture(5, start2, num);
																if (num16 == 0)
																{
																	continue;
																}
																goto IL_08ee;
															}
														}
														if (--num16 < 0)
														{
															break;
														}
														num = runstack[--pos];
														UncaptureUntil(runstack[--pos]);
														span = inputSpan.Slice(num);
														goto IL_08ee;
														IL_08ee:
														pos = num17;
														int num23;
														for (num23 = 0; (uint)num23 < (uint)span.Length && char.IsWhiteSpace(span[num23]); num23++)
														{
														}
														span = span.Slice(num23);
														num += num23;
														if (span.IsEmpty || span[0] != ';')
														{
															break;
														}
														Capture(0, start, runtextpos = num + 1);
														return true;
													}
													UncaptureUntil(num8);
													if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
													{
														CheckTimeout();
													}
													if (num13 >= num14)
													{
														break;
													}
													num = --num14;
													span = inputSpan.Slice(num);
												}
												UncaptureUntil(num7);
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
										}
									}
								}
							}
						}
						goto IL_0544;
						IL_0385:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (!span.StartsWith("highp".AsSpan()))
						{
							goto IL_03d2;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 6, arg, arg2);
						num += 5;
						span = inputSpan.Slice(num);
						goto IL_04c5;
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

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VaryingRegex_9 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VaryingRegex_9();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VaryingRegex_9()
	{
		pattern = "(?:layout\\s*\\((?<layout>[^)]*)\\)\\s*)(?:(?:flat|smooth|noperspective|centroid|sample|invariant|highp|mediump|lowp)\\s+)*(?<dir>in|out)\\s+(?<type>[A-Za-z_][A-Za-z0-9_]*)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*(?<array>\\[[^\\]]*\\])?\\s*;";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		base.CapNames = new Hashtable
		{
			{ "0", 0 },
			{ "array", 5 },
			{ "dir", 2 },
			{ "layout", 1 },
			{ "name", 4 },
			{ "type", 3 }
		};
		capslist = new string[6] { "0", "layout", "dir", "type", "name", "array" };
		capsize = 6;
	}
}
