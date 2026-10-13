using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LeadingNumberRegex_14 : Regex
{
	private sealed class RunnerFactory : RegexRunnerFactory
	{
		private sealed class Runner : RegexRunner
		{
			protected override void Scan(ReadOnlySpan<char> inputSpan)
			{
				if (TryFindNextPossibleStartingPosition(inputSpan) && !TryMatchAtCurrentPosition(inputSpan))
				{
					runtextpos = inputSpan.Length;
				}
			}

			private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				if ((uint)num < (uint)inputSpan.Length && num == 0)
				{
					return true;
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
				int arg = 0;
				int arg2 = 0;
				int num6 = 0;
				int num7 = 0;
				int arg3 = 0;
				int arg4 = 0;
				int num8 = 0;
				int num9 = 0;
				int num10 = 0;
				int num11 = 0;
				int pos = 0;
				ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
				if (num != 0)
				{
					return false;
				}
				char c;
				if (!readOnlySpan.IsEmpty && ((c = readOnlySpan[0]) == '+' || c == '-'))
				{
					readOnlySpan = readOnlySpan.Slice(1);
					num++;
				}
				num3 = num;
				num4 = num;
				int i;
				for (i = 0; (uint)i < (uint)readOnlySpan.Length && char.IsDigit(readOnlySpan[i]); i++)
				{
				}
				if (i != 0)
				{
					readOnlySpan = readOnlySpan.Slice(i);
					num += i;
					num5 = num;
					num4++;
					goto IL_00e9;
				}
				goto IL_0201;
				IL_01b9:
				if (--num10 < 0)
				{
					goto IL_00c1;
				}
				num = runstack[--pos];
				readOnlySpan = inputSpan.Slice(num);
				goto IL_01fa;
				IL_02c1:
				while (true)
				{
					num11 = 0;
					while (true)
					{
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, num);
						num11++;
						if (!readOnlySpan.IsEmpty && (readOnlySpan[0] | 0x20) == 101)
						{
							if ((uint)readOnlySpan.Length > 1u && ((c = readOnlySpan[1]) == '+' || c == '-'))
							{
								readOnlySpan = readOnlySpan.Slice(1);
								num++;
							}
							num++;
							readOnlySpan = inputSpan.Slice(num);
							arg3 = num;
							int j;
							for (j = 0; (uint)j < (uint)readOnlySpan.Length && char.IsDigit(readOnlySpan[j]); j++)
							{
							}
							if (j != 0)
							{
								readOnlySpan = readOnlySpan.Slice(j);
								num += j;
								arg4 = num;
								arg3++;
								goto IL_03b3;
							}
						}
						goto IL_03cd;
						IL_040e:
						num8 = num;
						ulong num12;
						if (!readOnlySpan.IsEmpty && (long)((ulong)(-9223090559730515968L << (int)(num12 = (uint)(readOnlySpan[0] - 70))) & (num12 - 64)) < 0L)
						{
							readOnlySpan = readOnlySpan.Slice(1);
							num++;
						}
						num9 = num;
						while (true)
						{
							int num13 = num;
							if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
							{
								CheckTimeout();
							}
							if (!readOnlySpan.IsEmpty && (c = readOnlySpan[0]) < '{' && ("\0\0䄀Ͽ\ufffe蟿\ufffe߿"[(int)c >> 4] & (1 << (c & 0xF))) != 0)
							{
								if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
								{
									CheckTimeout();
								}
								if (num8 >= num9)
								{
									break;
								}
								num = --num9;
								readOnlySpan = inputSpan.Slice(num);
								continue;
							}
							num = num13;
							readOnlySpan = inputSpan.Slice(num);
							runtextpos = num;
							Capture(0, start, num);
							return true;
						}
						if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
						{
							CheckTimeout();
						}
						if (num11 != 0)
						{
							_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPop(runstack, ref pos, out arg4, out arg3);
							if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
							{
								CheckTimeout();
							}
							if (arg3 < arg4)
							{
								num = --arg4;
								readOnlySpan = inputSpan.Slice(num);
								goto IL_03b3;
							}
							goto IL_03cd;
						}
						break;
						IL_03cd:
						if (--num11 < 0)
						{
							break;
						}
						num = runstack[--pos];
						readOnlySpan = inputSpan.Slice(num);
						goto IL_040e;
						IL_03b3:
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, arg3, arg4);
						if (num11 == 0)
						{
							continue;
						}
						goto IL_040e;
					}
					if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
					if (num2 == 0)
					{
						break;
					}
					if (num2 != 1)
					{
						continue;
					}
					goto IL_027f;
				}
				if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
				{
					CheckTimeout();
				}
				if (num10 != 0)
				{
					_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPop(runstack, ref pos, out arg2, out arg);
					if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
					if (arg < arg2)
					{
						num = --arg2;
						readOnlySpan = inputSpan.Slice(num);
						goto IL_019f;
					}
					goto IL_01b9;
				}
				goto IL_00c1;
				IL_00ec:
				_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, num);
				num10++;
				if (!readOnlySpan.IsEmpty && readOnlySpan[0] == '.')
				{
					num++;
					readOnlySpan = inputSpan.Slice(num);
					arg = num;
					int k;
					for (k = 0; (uint)k < (uint)readOnlySpan.Length && char.IsDigit(readOnlySpan[k]); k++)
					{
					}
					readOnlySpan = readOnlySpan.Slice(k);
					num += k;
					arg2 = num;
					goto IL_019f;
				}
				goto IL_01b9;
				IL_00c1:
				if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
				{
					CheckTimeout();
				}
				if (num4 < num5)
				{
					num = --num5;
					readOnlySpan = inputSpan.Slice(num);
					goto IL_00e9;
				}
				goto IL_0201;
				IL_02a6:
				num2 = 1;
				goto IL_02c1;
				IL_01fa:
				num2 = 0;
				goto IL_02c1;
				IL_0201:
				num = num3;
				readOnlySpan = inputSpan.Slice(num);
				if (readOnlySpan.IsEmpty || readOnlySpan[0] != '.')
				{
					return false;
				}
				num++;
				readOnlySpan = inputSpan.Slice(num);
				num6 = num;
				int l;
				for (l = 0; (uint)l < (uint)readOnlySpan.Length && char.IsDigit(readOnlySpan[l]); l++)
				{
				}
				if (l == 0)
				{
					return false;
				}
				readOnlySpan = readOnlySpan.Slice(l);
				num += l;
				num7 = num;
				num6++;
				goto IL_02a6;
				IL_027f:
				if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
				{
					CheckTimeout();
				}
				if (num6 >= num7)
				{
					return false;
				}
				num = --num7;
				readOnlySpan = inputSpan.Slice(num);
				goto IL_02a6;
				IL_019f:
				_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, arg, arg2);
				if (num10 == 0)
				{
					goto IL_00ec;
				}
				goto IL_01fa;
				IL_00e9:
				num10 = 0;
				goto IL_00ec;
			}
		}

		protected override RegexRunner CreateInstance()
		{
			return new Runner();
		}
	}

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LeadingNumberRegex_14 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LeadingNumberRegex_14();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LeadingNumberRegex_14()
	{
		pattern = "^[-+]?(?:\\d+(?:\\.\\d*)?|\\.\\d+)(?:[eE][-+]?\\d+)?[fFuU]?(?![A-Za-z0-9_.(])";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		capsize = 1;
	}
}
