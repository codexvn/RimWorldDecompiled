using System;

namespace UnityEngine;

public class CreateAssetMenuAttribute : Attribute
{
	public string menuName { get; set; }

	public string fileName { get; set; }

	public int order { get; set; }
}
