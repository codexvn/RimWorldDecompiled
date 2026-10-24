using System.Collections.Generic;
using System.Text;

namespace PsdSharp.Parsing;

internal class ParseContext
{
	public BigEndianReader Reader { get; }

	public FormatTraits Traits { get; private set; }

	public PsdLoadOptions PsdLoadOptions { get; }

	public Encoding StringEncoding => PsdLoadOptions.StringEncoding;

	public ColorMode ColorMode { get; private set; } = ColorMode.Rgb;

	public List<string> Warnings { get; } = new List<string>();

	public ParseContext(BigEndianReader reader, PsdLoadOptions psdLoadOptions)
	{
		Reader = reader;
		PsdLoadOptions = psdLoadOptions;
		Traits = default(FormatTraits);
	}

	public void InitPsdFileType(PsdFileType psdFileType)
	{
		if (Traits.Equals(default(FormatTraits)))
		{
			Traits = FormatTraits.FromPsdFileType(psdFileType, this);
		}
	}

	public void InitColorMode(ColorMode colorMode)
	{
		if (ColorMode.Equals(null))
		{
			ColorMode = colorMode;
		}
	}
}
