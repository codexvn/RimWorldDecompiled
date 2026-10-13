using System;

namespace Verse
{
	// Token: 0x0200037E RID: 894
	public class LanguageWorker_Catalan : LanguageWorker
	{
		// Token: 0x060016F6 RID: 5878 RVA: 0x00072C1B File Offset: 0x00070E1B
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return this.WithElLaArticle(str, gender, true);
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "unes " : "uns ") + str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00072C5B File Offset: 0x00070E5B
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return this.WithElLaArticle(str, gender, true);
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "les " : "els ") + str;
			}
			return this.WithElLaArticle(str, gender, false);
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00072C90 File Offset: 0x00070E90
		private string WithElLaArticle(string str, Gender gender, bool name)
		{
			if (str.Length == 0 || (!this.IsVowel(str[0]) && str[0] != 'h' && str[0] != 'H'))
			{
				return ((gender == Gender.Female) ? "la " : "el ") + str;
			}
			if (name)
			{
				return ((gender == Gender.Female) ? "l'" : "n'") + str;
			}
			return "l'" + str;
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00072D08 File Offset: 0x00070F08
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			if (gender == Gender.Female)
			{
				return number.ToString() + "a";
			}
			if (number == 1 || number == 3)
			{
				return number.ToString() + "r";
			}
			if (number == 2)
			{
				return number.ToString() + "n";
			}
			if (number == 4)
			{
				return number.ToString() + "t";
			}
			return number.ToString() + "è";
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00072D82 File Offset: 0x00070F82
		public bool IsVowel(char ch)
		{
			return "ieɛaoɔuəuàêèéòóüúIEƐAOƆUƏUÀÊÈÉÒÓÜÚ".IndexOf(ch) >= 0;
		}
	}
}
