namespace Waylith.Graphics;

public readonly record struct GpuPreference(string? Name, string? Uuid)
{
	public bool IsSet
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				return !string.IsNullOrWhiteSpace(Uuid);
			}
			return true;
		}
	}
}
