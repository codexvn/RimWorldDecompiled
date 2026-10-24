using System;

namespace UnityEngine;

public struct Resolution
{
	public int height { get; set; }

	public RefreshRate refreshRateRatio { get; set; }

	[Obsolete("Resolution.refreshRate is obsolete. Use refreshRateRatio instead.", false)]
	public int refreshRate
	{
		get
		{
			return (int)Math.Round(refreshRateRatio.value);
		}
		set
		{
			refreshRateRatio = new RefreshRate
			{
				numerator = (uint)Math.Max(value, 0),
				denominator = 1u
			};
		}
	}

	public int width { get; set; }
}
