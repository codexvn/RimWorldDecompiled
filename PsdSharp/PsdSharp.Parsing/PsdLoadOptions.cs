using System.Text;

namespace PsdSharp.Parsing;

public sealed class PsdLoadOptions
{
	public bool LeaveInputOpen { get; set; } = true;

	public Encoding? StringEncoding { get; set; }

	public ImageDataLoading ImageDataLoading { get; set; }

	public long AutoImageLoadingDiskCacheThresholdBytes { get; set; } = 67108864L;
}
