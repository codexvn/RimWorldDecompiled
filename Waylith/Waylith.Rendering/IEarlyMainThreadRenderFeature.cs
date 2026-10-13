namespace Waylith.Rendering;

public interface IEarlyMainThreadRenderFeature
{
	void BeginMainThreadFrame(in FrameTiming timing);
}
