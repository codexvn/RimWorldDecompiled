using System;

namespace Verse
{
	// Token: 0x0200041A RID: 1050
	public static class PsychGlowUtility
	{
		// Token: 0x06001A8A RID: 6794 RVA: 0x000865E4 File Offset: 0x000847E4
		public static string GetLabel(this PsychGlow gl)
		{
			switch (gl)
			{
			case PsychGlow.Dark:
				return "Dark".Translate();
			case PsychGlow.Lit:
				return "Lit".Translate();
			case PsychGlow.Overlit:
				return "LitBrightly".Translate();
			default:
				throw new ArgumentException();
			}
		}
	}
}
