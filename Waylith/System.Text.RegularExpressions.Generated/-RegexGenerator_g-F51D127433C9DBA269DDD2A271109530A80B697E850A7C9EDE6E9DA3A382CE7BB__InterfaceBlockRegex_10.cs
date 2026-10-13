using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__InterfaceBlockRegex_10 : Regex
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
				if (num <= inputSpan.Length - 5)
				{
					ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
					int num2;
					for (num2 = 0; num2 < readOnlySpan.Length - 4; num2++)
					{
						int num3 = readOnlySpan.Slice(num2 + 1).IndexOfAny('n', 'u');
						if (num3 < 0)
						{
							break;
						}
						num2 += num3;
						char c;
						if ((c = readOnlySpan[num2]) == 'i' || c == 'o')
						{
							runtextpos = num + num2;
							return true;
						}
					}
				}
				runtextpos = inputSpan.Length;
				return false;
			}

			private bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				int start = num;
				ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
				if (!_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.IsPreWordCharBoundary(inputSpan, num))
				{
					return false;
				}
				if (readOnlySpan.IsEmpty)
				{
					return false;
				}
				switch (readOnlySpan[0])
				{
				case 'i':
					if ((uint)readOnlySpan.Length < 2u || readOnlySpan[1] != 'n')
					{
						return false;
					}
					num += 2;
					readOnlySpan = inputSpan.Slice(num);
					break;
				case 'o':
					if (!readOnlySpan.Slice(1).StartsWith("ut".AsSpan()))
					{
						return false;
					}
					num += 3;
					readOnlySpan = inputSpan.Slice(num);
					break;
				default:
					return false;
				}
				int i;
				for (i = 0; (uint)i < (uint)readOnlySpan.Length && char.IsWhiteSpace(readOnlySpan[i]); i++)
				{
				}
				if (i == 0)
				{
					return false;
				}
				readOnlySpan = readOnlySpan.Slice(i);
				num += i;
				ulong num2;
				if (readOnlySpan.IsEmpty || (long)((ulong)(-261993005120L << (int)(num2 = (uint)(readOnlySpan[0] - 65))) & (num2 - 64)) >= 0L)
				{
					return false;
				}
				int num3 = readOnlySpan.Slice(1).IndexOfAnyExcept(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_asciiWordChars);
				if (num3 < 0)
				{
					num3 = readOnlySpan.Length - 1;
				}
				readOnlySpan = readOnlySpan.Slice(num3);
				num += num3;
				int j;
				for (j = 1; (uint)j < (uint)readOnlySpan.Length && char.IsWhiteSpace(readOnlySpan[j]); j++)
				{
				}
				readOnlySpan = readOnlySpan.Slice(j);
				num += j;
				if (readOnlySpan.IsEmpty || readOnlySpan[0] != '{')
				{
					return false;
				}
				Capture(0, start, runtextpos = num + 1);
				return true;
			}
		}

		protected override RegexRunner CreateInstance()
		{
			return new Runner();
		}
	}

	internal static readonly _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__InterfaceBlockRegex_10 Instance = new _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__InterfaceBlockRegex_10();

	private _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__InterfaceBlockRegex_10()
	{
		pattern = "\\b(?:in|out)\\s+[A-Za-z_][A-Za-z0-9_]*\\s*\\{";
		roptions = RegexOptions.CultureInvariant;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		capsize = 1;
	}
}
