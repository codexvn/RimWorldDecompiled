using System;
using System.Collections.Generic;

namespace PsdSharp.ImageResources;

internal static class ImageResourceRegistry
{
	public static Dictionary<ImageResourceId, Type> ResourceTypes { get; } = new Dictionary<ImageResourceId, Type>
	{
		{
			ImageResourceId.ResolutionInfo,
			typeof(ResolutionInfo)
		},
		{
			ImageResourceId.ThumbnailResource,
			typeof(ThumbnailResource)
		},
		{
			ImageResourceId.VersionInfo,
			typeof(VersionInfo)
		}
	};
}
