using System;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020002E5 RID: 741
	public static class SoundDefHelper
	{
		// Token: 0x0600123D RID: 4669 RVA: 0x0005513B File Offset: 0x0005333B
		public static bool NullOrUndefined(this SoundDef def)
		{
			return def == null || def.isUndefined;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00055148 File Offset: 0x00053348
		public static bool CorrectContextNow(SoundDef def, Map sourceMap)
		{
			if (sourceMap != null && (Find.CurrentMap != sourceMap || WorldRendererUtility.WorldSelected))
			{
				return false;
			}
			switch (def.context)
			{
			case SoundContext.Any:
				return true;
			case SoundContext.MapOnly:
				return Current.ProgramState == ProgramState.Playing && WorldRendererUtility.DrawingMap;
			case SoundContext.WorldOnly:
				return WorldRendererUtility.WorldSelected;
			default:
				throw new NotImplementedException();
			}
		}
	}
}
