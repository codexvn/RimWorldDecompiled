using System;

namespace UnityEngine;

public class MissingReferenceException : SystemException
{
	public MissingReferenceException()
		: base("A Unity Runtime error occurred!")
	{
		base.HResult = -2147467261;
	}

	public MissingReferenceException(string message)
		: base(message)
	{
		base.HResult = -2147467261;
	}

	public MissingReferenceException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2147467261;
	}
}
