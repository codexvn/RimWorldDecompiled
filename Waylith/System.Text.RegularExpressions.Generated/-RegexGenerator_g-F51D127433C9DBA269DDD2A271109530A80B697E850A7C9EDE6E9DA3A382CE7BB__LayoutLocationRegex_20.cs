using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LayoutLocationRegex_20 : Regex
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
				if (num <= inputSpan.Length - 10)
				{
					int num2 = inputSpan.Slice(num).IndexOfNonAsciiOrAny_1077D49D11363D98138BE1324B9FA34304AD049D25FD415C498913B425487F68();
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
				ReadOnlySpan<char> span = inputSpan.Slice(num);
				num4 = num;
				num3 = Crawlpos();
				if (num != 0)
				{
					goto IL_002e;
				}
				num2 = 0;
				goto IL_0090;
				IL_002e:
				num = num4;
				span = inputSpan.Slice(num);
				UncaptureUntil(num3);
				if (span.IsEmpty || span[0] != ',')
				{
					UncaptureUntil(0);
					return false;
				}
				num2 = 1;
				num++;
				span = inputSpan.Slice(num);
				goto IL_0090;
				IL_0090:
				while (true)
				{
					int i;
					for (i = 0; (uint)i < (uint)span.Length && char.IsWhiteSpace(span[i]); i++)
					{
					}
					span = span.Slice(i);
					num += i;
					if (span.StartsWith("location".AsSpan()))
					{
						int j;
						for (j = 8; (uint)j < (uint)span.Length && char.IsWhiteSpace(span[j]); j++)
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
							num5 = num;
							int l;
							for (l = 0; (uint)l < (uint)span.Length && char.IsDigit(span[l]); l++)
							{
							}
							if (l != 0)
							{
								span = span.Slice(l);
								num += l;
								Capture(1, num5, num);
								runtextpos = num;
								Capture(0, start, num);
								return true;
							}
						}
					}
					if (_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
					if (num2 == 0)
					{
						break;
					}
					if (num2 == 1)
					{
						UncaptureUntil(0);
						return false;
					}
				}
				goto IL_002e;
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

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LayoutLocationRegex_20 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LayoutLocationRegex_20();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LayoutLocationRegex_20()
	{
		pattern = "(?:^|,)\\s*location\\s*=\\s*(\\d+)";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		capsize = 2;
	}
}
