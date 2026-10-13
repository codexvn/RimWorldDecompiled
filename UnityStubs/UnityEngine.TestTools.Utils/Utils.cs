using System;

namespace UnityEngine.TestTools.Utils;

public static class Utils
{
	public static bool AreFloatsEqual(float expected, float actual, float allowedError)
	{
		return Math.Abs(expected - actual) <= allowedError;
	}
}
