using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D21 RID: 3361
	[StaticConstructorOnStartup]
	public static class GenderUtility
	{
		// Token: 0x060055C5 RID: 21957 RVA: 0x001BB2D5 File Offset: 0x001B94D5
		public static string GetGenderLabel(this Pawn pawn)
		{
			return pawn.gender.GetLabel(pawn.RaceProps.Animal);
		}

		// Token: 0x060055C6 RID: 21958 RVA: 0x001BB2F0 File Offset: 0x001B94F0
		public static string GetLabel(this Gender gender, bool animal = false)
		{
			switch (gender)
			{
			case Gender.None:
				return "NoneLower".Translate();
			case Gender.Male:
				return animal ? "MaleAnimal".Translate() : "Male".Translate();
			case Gender.Female:
				return animal ? "FemaleAnimal".Translate() : "Female".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x060055C7 RID: 21959 RVA: 0x001BB364 File Offset: 0x001B9564
		public static string GetPronoun(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "Proit".Translate();
			case Gender.Male:
				return "Prohe".Translate();
			case Gender.Female:
				return "Proshe".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x001BB3BC File Offset: 0x001B95BC
		public static string GetPossessive(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "Proits".Translate();
			case Gender.Male:
				return "Prohis".Translate();
			case Gender.Female:
				return "Proher".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x060055C9 RID: 21961 RVA: 0x001BB414 File Offset: 0x001B9614
		public static string GetObjective(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "ProitObj".Translate();
			case Gender.Male:
				return "ProhimObj".Translate();
			case Gender.Female:
				return "ProherObj".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x060055CA RID: 21962 RVA: 0x001BB46C File Offset: 0x001B966C
		public static string GetGenderNoun(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "ProItNoun".Translate();
			case Gender.Male:
				return "ProHeNoun".Translate();
			case Gender.Female:
				return "ProSheNoun".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x060055CB RID: 21963 RVA: 0x001BB4C2 File Offset: 0x001B96C2
		public static Texture2D GetIcon(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return GenderUtility.GenderlessIcon;
			case Gender.Male:
				return GenderUtility.MaleIcon;
			case Gender.Female:
				return GenderUtility.FemaleIcon;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x060055CC RID: 21964 RVA: 0x001BB4EF File Offset: 0x001B96EF
		public static Gender Opposite(this Gender gender)
		{
			if (gender == Gender.Male)
			{
				return Gender.Female;
			}
			if (gender == Gender.Female)
			{
				return Gender.Male;
			}
			return Gender.None;
		}

		// Token: 0x04003E09 RID: 15881
		private static readonly Texture2D GenderlessIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Genderless", true);

		// Token: 0x04003E0A RID: 15882
		private static readonly Texture2D MaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Male", true);

		// Token: 0x04003E0B RID: 15883
		private static readonly Texture2D FemaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Female", true);
	}
}
