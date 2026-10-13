namespace UnityEngine.TextCore;

public struct FaceInfo
{
	internal int faceIndex { get; set; }

	public string familyName { get; set; }

	public string styleName { get; set; }

	public int pointSize { get; set; }

	public float scale { get; set; }

	internal int unitsPerEM { get; set; }

	public float lineHeight { get; set; }

	public float ascentLine { get; set; }

	public float capLine { get; set; }

	public float meanLine { get; set; }

	public float baseline { get; set; }

	public float descentLine { get; set; }

	public float superscriptOffset { get; set; }

	public float superscriptSize { get; set; }

	public float subscriptOffset { get; set; }

	public float subscriptSize { get; set; }

	public float underlineOffset { get; set; }

	public float underlineThickness { get; set; }

	public float strikethroughOffset { get; set; }

	public float strikethroughThickness { get; set; }

	public float tabWidth { get; set; }

	public bool Compare(FaceInfo other)
	{
		if (familyName == other.familyName && styleName == other.styleName && pointSize == other.pointSize && scale == other.scale && unitsPerEM == other.unitsPerEM && lineHeight == other.lineHeight && ascentLine == other.ascentLine && capLine == other.capLine && meanLine == other.meanLine && baseline == other.baseline && descentLine == other.descentLine && superscriptOffset == other.superscriptOffset && superscriptSize == other.superscriptSize && subscriptOffset == other.subscriptOffset && subscriptSize == other.subscriptSize && underlineOffset == other.underlineOffset && underlineThickness == other.underlineThickness && strikethroughOffset == other.strikethroughOffset && strikethroughThickness == other.strikethroughThickness)
		{
			return tabWidth == other.tabWidth;
		}
		return false;
	}
}
