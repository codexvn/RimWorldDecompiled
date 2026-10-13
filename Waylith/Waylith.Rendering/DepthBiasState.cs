namespace Waylith.Rendering;

public readonly record struct DepthBiasState(float ConstantFactor, float SlopeFactor)
{
	public bool Enabled
	{
		get
		{
			if (ConstantFactor == 0f)
			{
				return SlopeFactor != 0f;
			}
			return true;
		}
	}
}
