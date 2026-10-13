using System;

namespace Verse
{
	// Token: 0x02000D48 RID: 3400
	public static class TemperatureDisplayModeExtension
	{
		// Token: 0x060056DB RID: 22235 RVA: 0x001BECD0 File Offset: 0x001BCED0
		public static string ToStringHuman(this TemperatureDisplayMode mode)
		{
			switch (mode)
			{
			case TemperatureDisplayMode.Celsius:
				return "Celsius".Translate();
			case TemperatureDisplayMode.Fahrenheit:
				return "Fahrenheit".Translate();
			case TemperatureDisplayMode.Kelvin:
				return "Kelvin".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
