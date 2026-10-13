using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__FullNumberRegex_15 : Regex
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
				int num8 = 0;
				int num9 = 0;
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
					goto IL_00dd;
				}
				goto IL_01f5;
				IL_00b5:
				if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
				{
					CheckTimeout();
				}
				if (num4 < num5)
				{
					num = --num5;
					readOnlySpan = inputSpan.Slice(num);
					goto IL_00dd;
				}
				goto IL_01f5;
				IL_01ad:
				if (--num8 < 0)
				{
					goto IL_00b5;
				}
				num = runstack[--pos];
				readOnlySpan = inputSpan.Slice(num);
				goto IL_01ee;
				IL_00e0:
				_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, num);
				num8++;
				if (!readOnlySpan.IsEmpty && readOnlySpan[0] == '.')
				{
					num++;
					readOnlySpan = inputSpan.Slice(num);
					arg = num;
					int j;
					for (j = 0; (uint)j < (uint)readOnlySpan.Length && char.IsDigit(readOnlySpan[j]); j++)
					{
					}
					readOnlySpan = readOnlySpan.Slice(j);
					num += j;
					arg2 = num;
					goto IL_0193;
				}
				goto IL_01ad;
				IL_0273:
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
				goto IL_029a;
				IL_01f5:
				num = num3;
				readOnlySpan = inputSpan.Slice(num);
				if (readOnlySpan.IsEmpty || readOnlySpan[0] != '.')
				{
					return false;
				}
				num++;
				readOnlySpan = inputSpan.Slice(num);
				num6 = num;
				int k;
				for (k = 0; (uint)k < (uint)readOnlySpan.Length && char.IsDigit(readOnlySpan[k]); k++)
				{
				}
				if (k == 0)
				{
					return false;
				}
				readOnlySpan = readOnlySpan.Slice(k);
				num += k;
				num7 = num;
				num6++;
				goto IL_029a;
				IL_01ee:
				num2 = 0;
				goto IL_02b5;
				IL_029a:
				num2 = 1;
				goto IL_02b5;
				IL_02b5:
				while (true)
				{
					num9 = 0;
					while (true)
					{
						_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, num);
						num9++;
						if (!readOnlySpan.IsEmpty && (readOnlySpan[0] | 0x20) == 101)
						{
							if ((uint)readOnlySpan.Length > 1u && ((c = readOnlySpan[1]) == '+' || c == '-'))
							{
								readOnlySpan = readOnlySpan.Slice(1);
								num++;
							}
							num++;
							readOnlySpan = inputSpan.Slice(num);
							int l;
							for (l = 0; (uint)l < (uint)readOnlySpan.Length && char.IsDigit(readOnlySpan[l]); l++)
							{
							}
							if (l != 0)
							{
								readOnlySpan = readOnlySpan.Slice(l);
								num += l;
								if (num9 == 0)
								{
									continue;
								}
								goto IL_0392;
							}
						}
						goto IL_036c;
						IL_0392:
						ulong num10;
						if (!readOnlySpan.IsEmpty && (long)((ulong)(-9223090559730515968L << (int)(num10 = (uint)(readOnlySpan[0] - 70))) & (num10 - 64)) < 0L)
						{
							readOnlySpan = readOnlySpan.Slice(1);
							num++;
						}
						if (num >= inputSpan.Length - 1 && ((uint)num >= (uint)inputSpan.Length || inputSpan[num] == '\n'))
						{
							runtextpos = num;
							Capture(0, start, num);
							return true;
						}
						goto IL_036c;
						IL_036c:
						if (--num9 < 0)
						{
							break;
						}
						num = runstack[--pos];
						readOnlySpan = inputSpan.Slice(num);
						goto IL_0392;
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
					goto IL_0273;
				}
				if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
				{
					CheckTimeout();
				}
				if (num8 != 0)
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
						goto IL_0193;
					}
					goto IL_01ad;
				}
				goto IL_00b5;
				IL_0193:
				_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, arg, arg2);
				if (num8 == 0)
				{
					goto IL_00e0;
				}
				goto IL_01ee;
				IL_00dd:
				num8 = 0;
				goto IL_00e0;
			}
		}

		protected override RegexRunner CreateInstance()
		{
			return new Runner();
		}
	}

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__FullNumberRegex_15 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__FullNumberRegex_15();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__FullNumberRegex_15()
	{
		pattern = "^[-+]?(?:\\d+(?:\\.\\d*)?|\\.\\d+)(?:[eE][-+]?\\d+)?[fFuU]?$";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		capsize = 1;
	}
}
