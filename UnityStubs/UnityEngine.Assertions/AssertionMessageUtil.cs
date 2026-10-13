using System;

namespace UnityEngine.Assertions;

internal class AssertionMessageUtil
{
	private const string k_Expected = "Expected:";

	private const string k_AssertionFailed = "Assertion failure.";

	public static string GetMessage(string failureMessage)
	{
		return "Assertion failure. " + failureMessage;
	}

	public static string GetMessage(string failureMessage, string expected)
	{
		return GetMessage($"{failureMessage}{Environment.NewLine}{"Expected:"} {expected}");
	}

	public static string GetEqualityMessage(object actual, object expected, bool expectEqual)
	{
		return GetMessage("Values are " + (expectEqual ? "not " : "") + "equal.", $"{actual} {(expectEqual ? "==" : "!=")} {expected}");
	}

	public static string NullFailureMessage(object value, bool expectNull)
	{
		return GetMessage("Value was " + (expectNull ? "not " : "") + "Null", "Value was " + (expectNull ? "" : "not ") + "Null");
	}

	public static string BooleanFailureMessage(bool expected)
	{
		return GetMessage("Value was " + !expected, expected.ToString());
	}
}
