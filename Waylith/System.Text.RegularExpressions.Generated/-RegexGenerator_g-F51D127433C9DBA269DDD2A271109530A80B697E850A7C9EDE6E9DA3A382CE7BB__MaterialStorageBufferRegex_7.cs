using System.CodeDom.Compiler;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialStorageBufferRegex_7 : Regex
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
				if (num <= inputSpan.Length - 17)
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
				num2 = num;
				int num8 = span.IndexOf(')');
				if (num8 < 0)
				{
					num8 = span.Length;
				}
				span = span.Slice(num8);
				num += num8;
				Capture(1, num2, num);
				if (span.IsEmpty || span[0] != ')')
				{
					UncaptureUntil(0);
					return false;
				}
				num++;
				span = inputSpan.Slice(num);
				num5 = num;
				int j;
				for (j = 0; (uint)j < (uint)span.Length && char.IsWhiteSpace(span[j]); j++)
				{
				}
				span = span.Slice(j);
				num += j;
				num6 = num;
				while (true)
				{
					num4 = Crawlpos();
					num7 = 0;
					while (true)
					{
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
						num7++;
						int arg = num;
						int arg2 = Crawlpos();
						if (!span.StartsWith("readonly".AsSpan()))
						{
							goto IL_01e4;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 0, arg, arg2);
						num += 8;
						span = inputSpan.Slice(num);
						goto IL_0362;
						IL_02cc:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (span.StartsWith("volatile".AsSpan()))
						{
							_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 4, arg, arg2);
							num += 8;
							span = inputSpan.Slice(num);
							goto IL_0362;
						}
						goto IL_03a2;
						IL_01e4:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (!span.StartsWith("writeonly".AsSpan()))
						{
							goto IL_0232;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 1, arg, arg2);
						num += 9;
						span = inputSpan.Slice(num);
						goto IL_0362;
						IL_0362:
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
						goto IL_0319;
						IL_0232:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (!span.StartsWith("restrict".AsSpan()))
						{
							goto IL_027f;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 2, arg, arg2);
						num += 8;
						span = inputSpan.Slice(num);
						goto IL_0362;
						IL_03a2:
						if (--num7 < 0)
						{
							break;
						}
						num = runstack[--pos];
						UncaptureUntil(runstack[--pos]);
						span = inputSpan.Slice(num);
						if (span.StartsWith("buffer".AsSpan()))
						{
							num += 6;
							span = inputSpan.Slice(num);
							int l;
							for (l = 0; (uint)l < (uint)span.Length && char.IsWhiteSpace(span[l]); l++)
							{
							}
							if (l != 0)
							{
								span = span.Slice(l);
								num += l;
								num3 = num;
								ulong num9;
								if (!span.IsEmpty && (long)((ulong)(-261993005120L << (int)(num9 = (uint)(span[0] - 65))) & (num9 - 64)) < 0L)
								{
									int num10 = span.Slice(1).IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
									if (num10 < 0)
									{
										num10 = span.Length - 1;
									}
									span = span.Slice(num10);
									num += num10;
									num++;
									span = inputSpan.Slice(num);
									Capture(2, num3, num);
									int m;
									for (m = 0; (uint)m < (uint)span.Length && char.IsWhiteSpace(span[m]); m++)
									{
									}
									span = span.Slice(m);
									num += m;
									if (!span.IsEmpty && span[0] == '{')
									{
										Capture(0, start, runtextpos = num + 1);
										return true;
									}
								}
							}
						}
						if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
						{
							CheckTimeout();
						}
						if (num7 == 0)
						{
							break;
						}
						goto IL_0319;
						IL_027f:
						num = arg;
						span = inputSpan.Slice(num);
						UncaptureUntil(arg2);
						if (!span.StartsWith("coherent".AsSpan()))
						{
							goto IL_02cc;
						}
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, 3, arg, arg2);
						num += 8;
						span = inputSpan.Slice(num);
						goto IL_0362;
						IL_0319:
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
							goto IL_0232;
						case 2:
							goto IL_027f;
						case 3:
							goto IL_02cc;
						default:
							goto IL_0362;
						case 4:
							goto IL_03a2;
						}
						goto IL_01e4;
					}
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

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialStorageBufferRegex_7 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialStorageBufferRegex_7();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialStorageBufferRegex_7()
	{
		pattern = "layout\\s*\\((?<layout>[^)]*)\\)\\s*(?:(?:readonly|writeonly|restrict|coherent|volatile)\\s+)*buffer\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*\\{";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		base.CapNames = new Hashtable
		{
			{ "0", 0 },
			{ "layout", 1 },
			{ "name", 2 }
		};
		capslist = new string[3] { "0", "layout", "name" };
		capsize = 3;
	}
}
