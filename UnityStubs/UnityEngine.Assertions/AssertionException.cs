using System;

namespace UnityEngine.Assertions;

public class AssertionException : Exception
{
	private string m_UserMessage;

	public override string Message
	{
		get
		{
			if (m_UserMessage == null)
			{
				return base.Message;
			}
			return m_UserMessage + "\n" + base.Message;
		}
	}

	public AssertionException(string message, string userMessage)
		: base(message)
	{
		m_UserMessage = userMessage;
	}
}
