using System;
using System.IO.Enumeration;

namespace Waylith.Profiling.Instrumentation;

public readonly record struct ProfilerTargetPattern(string Assembly, string Type, string Method)
{
	public bool HasAssemblyWildcard => Assembly.AsSpan().IndexOfAny('*', '?') >= 0;

	public static ProfilerTargetPattern Parse(string pattern)
	{
		ReadOnlySpan<char> readOnlySpan = pattern.AsSpan();
		string assembly = "*";
		int num = readOnlySpan.IndexOf('!');
		if (num >= 0)
		{
			assembly = Part(readOnlySpan.Slice(0, num));
			readOnlySpan = readOnlySpan.Slice(num + 1);
		}
		string method = "*";
		int num2 = readOnlySpan.IndexOf("::".AsSpan(), StringComparison.Ordinal);
		if (num2 >= 0)
		{
			method = Part(readOnlySpan.Slice(num2 + 2));
			readOnlySpan = readOnlySpan.Slice(0, num2);
		}
		return new ProfilerTargetPattern(assembly, Part(readOnlySpan), method);
		static string Part(ReadOnlySpan<char> value)
		{
			ReadOnlySpan<char> readOnlySpan2 = value.Trim();
			if (!readOnlySpan2.IsEmpty)
			{
				return readOnlySpan2.ToString();
			}
			return "*";
		}
	}

	public bool MatchesAssembly(string assemblyName)
	{
		return Matches(Assembly, assemblyName);
	}

	public bool MatchesType(string typeFullName)
	{
		return Matches(Type, typeFullName);
	}

	public bool MatchesMethod(string methodName)
	{
		return Matches(Method, methodName);
	}

	private static bool Matches(string pattern, string value)
	{
		if (!(pattern == "*"))
		{
			return FileSystemName.MatchesSimpleExpression(pattern.AsSpan(), value.AsSpan());
		}
		return true;
	}
}
