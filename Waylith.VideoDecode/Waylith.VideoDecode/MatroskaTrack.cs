using System.Collections.Generic;

namespace Waylith.VideoDecode;

internal sealed class MatroskaTrack
{
	public int Number;

	public bool IsVideo;

	public string Codec;

	public int Width;

	public int Height;

	public double FrameDuration;

	public readonly List<MatroskaFrame> Frames = new List<MatroskaFrame>();
}
