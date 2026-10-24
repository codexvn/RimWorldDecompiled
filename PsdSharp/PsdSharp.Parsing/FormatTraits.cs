using System;

namespace PsdSharp.Parsing;

internal readonly record struct FormatTraits
{
	public PsdFileType PsdFileType { get; }

	public int RleCountSizeBytes { get; }

	public Func<ulong> ReadLenN { get; }

	public static FormatTraits FromPsdFileType(PsdFileType psdFileType, ParseContext ctx)
	{
		return new FormatTraits(psdFileType, ctx);
	}

	private FormatTraits(PsdFileType psdFileType, ParseContext ctx)
	{
		PsdFileType = psdFileType;
		RleCountSizeBytes = ((PsdFileType == PsdFileType.Psb) ? 4 : 2);
		ReadLenN = ((PsdFileType == PsdFileType.Psb) ? ((Func<ulong>)(() => ctx.Reader.ReadUInt64())) : ((Func<ulong>)(() => ctx.Reader.ReadUInt32())));
	}
}
