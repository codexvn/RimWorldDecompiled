using System;

namespace UnityEngine;

public sealed class ExitGUIException : Exception
{
	public ExitGUIException()
	{
	}

	internal ExitGUIException(string message)
		: base(message)
	{
	}
}
