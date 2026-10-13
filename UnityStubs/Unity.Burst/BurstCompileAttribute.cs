using System;

namespace Unity.Burst;

public class BurstCompileAttribute : Attribute
{
	public bool CompileSynchronously { get; set; }
}
