namespace Waylith.Rendering;

public static class LayerPassRouter
{
	private static readonly (int mask, byte passId)[] registrations = new(int, byte)[8];

	private static int registrationCount;

	private static byte defaultPassId = 0;

	public static void RegisterLayerMask(int layerMask, byte passId)
	{
		registrations[registrationCount++] = (mask: layerMask, passId: passId);
	}

	public static void SetDefault(byte passId)
	{
		defaultPassId = passId;
	}

	public static byte Resolve(int layer)
	{
		if ((uint)layer < 32u)
		{
			int num = 1 << layer;
			for (int i = 0; i < registrationCount; i++)
			{
				if ((registrations[i].mask & num) != 0)
				{
					return registrations[i].passId;
				}
			}
		}
		return defaultPassId;
	}
}
