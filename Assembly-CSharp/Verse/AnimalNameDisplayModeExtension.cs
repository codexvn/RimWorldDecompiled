using System;

namespace Verse
{
	// Token: 0x02000D4F RID: 3407
	public static class AnimalNameDisplayModeExtension
	{
		// Token: 0x060056DF RID: 22239 RVA: 0x001BEE1C File Offset: 0x001BD01C
		public static string ToStringHuman(this AnimalNameDisplayMode mode)
		{
			switch (mode)
			{
			case AnimalNameDisplayMode.None:
				return "None".Translate();
			case AnimalNameDisplayMode.TameNamed:
				return "AnimalNameDisplayMode_TameNamed".Translate();
			case AnimalNameDisplayMode.TameAll:
				return "AnimalNameDisplayMode_TameAll".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060056E0 RID: 22240 RVA: 0x001BEE74 File Offset: 0x001BD074
		public static bool ShouldDisplayAnimalName(this AnimalNameDisplayMode mode, Pawn animal)
		{
			switch (mode)
			{
			case AnimalNameDisplayMode.None:
				return false;
			case AnimalNameDisplayMode.TameNamed:
				return animal.Name != null && !animal.Name.Numerical;
			case AnimalNameDisplayMode.TameAll:
				return animal.Name != null;
			default:
				throw new NotImplementedException(Prefs.AnimalNameMode.ToStringSafe<AnimalNameDisplayMode>());
			}
		}
	}
}
