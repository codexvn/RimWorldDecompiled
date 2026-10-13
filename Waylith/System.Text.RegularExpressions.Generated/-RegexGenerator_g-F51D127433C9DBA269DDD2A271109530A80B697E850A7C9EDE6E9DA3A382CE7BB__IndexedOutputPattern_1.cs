using System.CodeDom.Compiler;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__IndexedOutputPattern_1 : Regex
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
				if (num <= inputSpan.Length - 23)
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
				int pos = 0;
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
				num7 = num;
				int num14 = span.IndexOf(')');
				if (num14 < 0)
				{
					num14 = span.Length;
				}
				span = span.Slice(num14);
				num += num14;
				num8 = num;
				while (true)
				{
					num4 = Crawlpos();
					if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.IsPreWordCharBoundary(inputSpan, num) && span.StartsWith("index".AsSpan()))
					{
						int j;
						for (j = 5; (uint)j < (uint)span.Length && char.IsWhiteSpace(span[j]); j++)
						{
						}
						span = span.Slice(j);
						num += j;
						if (!span.IsEmpty && span[0] == '=')
						{
							int k;
							for (k = 1; (uint)k < (uint)span.Length && char.IsWhiteSpace(span[k]); k++)
							{
							}
							span = span.Slice(k);
							num += k;
							num2 = num;
							num9 = num;
							int l;
							for (l = 0; (uint)l < (uint)span.Length && char.IsDigit(span[l]); l++)
							{
							}
							if (l != 0)
							{
								span = span.Slice(l);
								num += l;
								num10 = num;
								num9++;
								while (true)
								{
									num5 = Crawlpos();
									Capture(1, num2, num);
									int num15 = span.IndexOf(')');
									if (num15 < 0)
									{
										num15 = span.Length;
									}
									span = span.Slice(num15);
									num += num15;
									if (!span.IsEmpty && span[0] == ')')
									{
										num++;
										span = inputSpan.Slice(num);
										num11 = num;
										int m;
										for (m = 0; (uint)m < (uint)span.Length && char.IsWhiteSpace(span[m]); m++)
										{
										}
										span = span.Slice(m);
										num += m;
										num12 = num;
										while (true)
										{
											num6 = Crawlpos();
											num13 = 0;
											while (true)
											{
												_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
												num13++;
												int n;
												for (n = 0; (uint)n < (uint)span.Length && _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.IsWordChar(span[n]); n++)
												{
												}
												if (n == 0)
												{
													break;
												}
												span = span.Slice(n);
												num += n;
												int num16;
												for (num16 = 0; (uint)num16 < (uint)span.Length && char.IsWhiteSpace(span[num16]); num16++)
												{
												}
												if (num16 == 0)
												{
													break;
												}
												span = span.Slice(num16);
												num += num16;
											}
											while (--num13 >= 0)
											{
												num = runstack[--pos];
												UncaptureUntil(runstack[--pos]);
												span = inputSpan.Slice(num);
												if (!span.StartsWith("out".AsSpan()))
												{
													continue;
												}
												num += 3;
												span = inputSpan.Slice(num);
												int num17;
												for (num17 = 0; (uint)num17 < (uint)span.Length && char.IsWhiteSpace(span[num17]); num17++)
												{
												}
												if (num17 == 0)
												{
													continue;
												}
												span = span.Slice(num17);
												num += num17;
												int num18;
												for (num18 = 0; (uint)num18 < (uint)span.Length && _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.IsWordChar(span[num18]); num18++)
												{
												}
												if (num18 == 0)
												{
													continue;
												}
												span = span.Slice(num18);
												num += num18;
												int num19;
												for (num19 = 0; (uint)num19 < (uint)span.Length && char.IsWhiteSpace(span[num19]); num19++)
												{
												}
												if (num19 == 0)
												{
													continue;
												}
												span = span.Slice(num19);
												num += num19;
												num3 = num;
												int num20;
												for (num20 = 0; (uint)num20 < (uint)span.Length && _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.IsWordChar(span[num20]); num20++)
												{
												}
												if (num20 != 0)
												{
													span = span.Slice(num20);
													num += num20;
													Capture(2, num3, num);
													int num21;
													for (num21 = 0; (uint)num21 < (uint)span.Length && char.IsWhiteSpace(span[num21]); num21++)
													{
													}
													span = span.Slice(num21);
													num += num21;
													if (!span.IsEmpty && span[0] == ';')
													{
														Capture(0, start, runtextpos = num + 1);
														return true;
													}
												}
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
							}
						}
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

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__IndexedOutputPattern_1 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__IndexedOutputPattern_1();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__IndexedOutputPattern_1()
	{
		pattern = "layout\\s*\\([^)]*\\bindex\\s*=\\s*(?<index>\\d+)[^)]*\\)\\s*(?:\\w+\\s+)*out\\s+\\w+\\s+(?<name>\\w+)\\s*;";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		base.CapNames = new Hashtable
		{
			{ "0", 0 },
			{ "index", 1 },
			{ "name", 2 }
		};
		capslist = new string[3] { "0", "index", "name" };
		capsize = 3;
	}
}
