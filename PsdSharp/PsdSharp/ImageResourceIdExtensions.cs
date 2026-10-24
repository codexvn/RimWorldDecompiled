using System;

namespace PsdSharp;

public static class ImageResourceIdExtensions
{
	public static bool IsPathRecord(this ImageResourceId imageResourceId)
	{
		if ((int)imageResourceId >= 2000)
		{
			return (int)imageResourceId <= 2997;
		}
		return false;
	}

	public static bool IsPluginResource(this ImageResourceId imageResourceId)
	{
		if ((int)imageResourceId >= 4000)
		{
			return (int)imageResourceId <= 4999;
		}
		return false;
	}

	public static bool IsValidResourceId(this ImageResourceId imageResourceId)
	{
		if (!Enum.IsDefined(typeof(ImageResourceId), imageResourceId) && !imageResourceId.IsPathRecord())
		{
			return imageResourceId.IsPluginResource();
		}
		return true;
	}
}
