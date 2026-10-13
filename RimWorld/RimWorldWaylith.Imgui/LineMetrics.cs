namespace RimWorldWaylith.Imgui;

internal readonly struct LineMetrics
{
	public readonly float Ascent;

	public readonly float FirstLine;

	public readonly float Pitch;

	public readonly float Spacing;

	public LineMetrics(float ascent, float firstLine, float pitch, float spacing)
	{
		Ascent = ascent;
		FirstLine = firstLine;
		Pitch = pitch;
		Spacing = spacing;
	}
}
