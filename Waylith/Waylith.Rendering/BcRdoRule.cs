using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Waylith.Rendering;

public readonly record struct BcRdoRule(BcEncodeTarget Target, string PathContains, float Lambda, int Window)
{
	public override string ToString()
	{
		IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
		DefaultInterpolatedStringHandler handler = new DefaultInterpolatedStringHandler(3, 4, invariantCulture);
		handler.AppendFormatted(Target);
		handler.AppendLiteral(":");
		handler.AppendFormatted(PathContains);
		handler.AppendLiteral(":");
		handler.AppendFormatted(Lambda);
		handler.AppendLiteral(":");
		handler.AppendFormatted(Window);
		return string.Create(invariantCulture, ref handler);
	}
}
