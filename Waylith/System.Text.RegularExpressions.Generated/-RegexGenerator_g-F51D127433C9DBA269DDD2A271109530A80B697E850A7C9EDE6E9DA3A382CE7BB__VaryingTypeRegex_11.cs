using System.CodeDom.Compiler;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VaryingTypeRegex_11 : Regex
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
				if (num <= inputSpan.Length - 3 && num == 0)
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
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				int num9 = 0;
				int num10 = 0;
				int pos = 0;
				int num11 = 0;
				ReadOnlySpan<char> span = inputSpan.Slice(num);
				if (num != 0)
				{
					UncaptureUntil(0);
					return false;
				}
				num4 = num;
				num3 = Crawlpos();
				num5 = num;
				if (!span.IsEmpty)
				{
					switch (span[0])
					{
					case 'f':
						if (!span.Slice(1).StartsWith("loat".AsSpan()))
						{
							break;
						}
						num += 5;
						span = inputSpan.Slice(num);
						goto IL_0165;
					case 'i':
						if (!span.Slice(1).StartsWith("nt".AsSpan()))
						{
							break;
						}
						num += 3;
						span = inputSpan.Slice(num);
						goto IL_0165;
					case 'u':
						if (!span.Slice(1).StartsWith("int".AsSpan()))
						{
							break;
						}
						num += 4;
						span = inputSpan.Slice(num);
						goto IL_0165;
					case 'b':
						if (!span.Slice(1).StartsWith("ool".AsSpan()))
						{
							break;
						}
						num += 4;
						span = inputSpan.Slice(num);
						goto IL_0165;
					case 'd':
						{
							if (!span.Slice(1).StartsWith("ouble".AsSpan()))
							{
								break;
							}
							num += 6;
							span = inputSpan.Slice(num);
							goto IL_0165;
						}
						IL_0165:
						Capture(1, num5, num);
						num2 = 0;
						goto IL_03d6;
					}
				}
				goto IL_0176;
				IL_0176:
				num = num4;
				span = inputSpan.Slice(num);
				UncaptureUntil(num3);
				num6 = num;
				uint num12;
				if (!span.IsEmpty && (int)((uint)(-1593831424 << (int)(short)(num12 = (ushort)(span[0] - 98))) & (num12 - 32)) < 0)
				{
					span = span.Slice(1);
					num++;
				}
				Capture(2, num6, num);
				if (span.StartsWith("vec".AsSpan()))
				{
					num += 3;
					span = inputSpan.Slice(num);
					num7 = num;
					if (!span.IsEmpty && char.IsBetween(span[0], '2', '4'))
					{
						num++;
						span = inputSpan.Slice(num);
						Capture(3, num7, num);
						num2 = 1;
						goto IL_03d6;
					}
				}
				goto IL_0231;
				IL_03d6:
				while (true)
				{
					if (num < inputSpan.Length - 1 || ((uint)num < (uint)inputSpan.Length && inputSpan[num] != '\n'))
					{
						if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
						{
							CheckTimeout();
						}
						switch (num2)
						{
						case 0:
							break;
						case 1:
							goto IL_0231;
						case 2:
							UncaptureUntil(0);
							return false;
						default:
							continue;
						}
						break;
					}
					runtextpos = num;
					Capture(0, start, num);
					return true;
				}
				goto IL_0176;
				IL_0231:
				num = num4;
				span = inputSpan.Slice(num);
				UncaptureUntil(num3);
				num8 = num;
				if (!span.IsEmpty && span[0] == 'd')
				{
					span = span.Slice(1);
					num++;
				}
				Capture(2, num8, num);
				if (!span.StartsWith("mat".AsSpan()))
				{
					UncaptureUntil(0);
					return false;
				}
				num += 3;
				span = inputSpan.Slice(num);
				num9 = num;
				if (span.IsEmpty || !char.IsBetween(span[0], '2', '4'))
				{
					UncaptureUntil(0);
					return false;
				}
				num++;
				span = inputSpan.Slice(num);
				Capture(4, num9, num);
				num11 = pos;
				num10 = 0;
				while (true)
				{
					_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.StackPush(ref runstack, ref pos, Crawlpos(), num);
					num10++;
					if (!span.IsEmpty && span[0] == 'x')
					{
						num++;
						span = inputSpan.Slice(num);
						int start2 = num;
						if (!span.IsEmpty && char.IsBetween(span[0], '2', '4'))
						{
							num++;
							span = inputSpan.Slice(num);
							Capture(5, start2, num);
							if (num10 != 0)
							{
								break;
							}
							continue;
						}
					}
					if (--num10 < 0)
					{
						UncaptureUntil(0);
						return false;
					}
					num = runstack[--pos];
					UncaptureUntil(runstack[--pos]);
					span = inputSpan.Slice(num);
					break;
				}
				pos = num11;
				num2 = 2;
				goto IL_03d6;
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

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VaryingTypeRegex_11 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VaryingTypeRegex_11();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VaryingTypeRegex_11()
	{
		pattern = "^(?:(?<scalar>float|int|uint|bool|double)|(?<prefix>[iubd]?)vec(?<size>[234])|(?<prefix>d?)mat(?<columns>[234])(?:x(?<rows>[234]))?)$";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		base.CapNames = new Hashtable
		{
			{ "0", 0 },
			{ "columns", 4 },
			{ "prefix", 2 },
			{ "rows", 5 },
			{ "scalar", 1 },
			{ "size", 3 }
		};
		capslist = new string[6] { "0", "scalar", "prefix", "size", "columns", "rows" };
		capsize = 6;
	}
}
