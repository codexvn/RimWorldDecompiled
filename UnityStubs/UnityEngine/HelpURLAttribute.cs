using System;

namespace UnityEngine;

public class HelpURLAttribute : Attribute
{
	private readonly string m_Url;

	public string URL => m_Url;

	public HelpURLAttribute(string url)
	{
		m_Url = url;
	}
}
