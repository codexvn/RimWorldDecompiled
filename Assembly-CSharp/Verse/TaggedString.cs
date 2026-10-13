using System;

namespace Verse
{
	// Token: 0x02000BD5 RID: 3029
	public struct TaggedString
	{
		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06004D76 RID: 19830 RVA: 0x0018A26D File Offset: 0x0018846D
		public string RawText
		{
			get
			{
				return this.rawText;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		public char this[int i]
		{
			get
			{
				return this.RawText[i];
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06004D78 RID: 19832 RVA: 0x0018A283 File Offset: 0x00188483
		public int Length
		{
			get
			{
				return this.RawText.Length;
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06004D79 RID: 19833 RVA: 0x0018A290 File Offset: 0x00188490
		public int StrippedLength
		{
			get
			{
				return this.RawText.StripTags().Length;
			}
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x0018A2A2 File Offset: 0x001884A2
		public TaggedString(string dat)
		{
			this.rawText = dat;
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x0018A2AB File Offset: 0x001884AB
		public string Resolve()
		{
			return ColoredText.Resolve(this);
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x0018A2B8 File Offset: 0x001884B8
		public TaggedString CapitalizeFirst()
		{
			if (this.rawText.NullOrEmpty())
			{
				return this;
			}
			int num = this.FirstLetterBetweenTags();
			if (char.ToUpper(this.rawText[num]) == this.rawText[num])
			{
				return this;
			}
			if (this.rawText.Length == 1)
			{
				return new TaggedString(this.rawText.ToUpper());
			}
			if (num == 0)
			{
				return new TaggedString(char.ToUpper(this.rawText[num]).ToString() + this.rawText.Substring(num + 1));
			}
			return new TaggedString(this.rawText.Substring(0, num) + char.ToUpper(this.rawText[num]).ToString() + this.rawText.Substring(num + 1));
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x0018A398 File Offset: 0x00188598
		public TaggedString EndWithPeriod()
		{
			if (this.rawText.NullOrEmpty())
			{
				return this;
			}
			if (this.rawText[this.rawText.Length - 1] == '.')
			{
				return this;
			}
			return this.rawText + ".";
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x0018A3F4 File Offset: 0x001885F4
		private int FirstLetterBetweenTags()
		{
			bool flag = false;
			for (int i = 0; i < this.rawText.Length - 1; i++)
			{
				if (this.rawText[i] == '(' && this.rawText[i + 1] == '*')
				{
					flag = true;
				}
				else
				{
					if (flag && this.rawText[i] == ')' && this.rawText[i + 1] != '(')
					{
						return i + 1;
					}
					if (!flag)
					{
						return i;
					}
				}
			}
			return 0;
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x0018A470 File Offset: 0x00188670
		public bool NullOrEmpty()
		{
			return this.RawText.NullOrEmpty();
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x0018A47D File Offset: 0x0018867D
		public TaggedString AdjustedFor(Pawn p, string pawnSymbol = "PAWN", bool addRelationInfoSymbol = true)
		{
			return this.RawText.AdjustedFor(p, pawnSymbol, addRelationInfoSymbol);
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x0018A492 File Offset: 0x00188692
		public float GetWidthCached()
		{
			return this.RawText.StripTags().GetWidthCached();
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x0018A4A4 File Offset: 0x001886A4
		public TaggedString Trim()
		{
			return new TaggedString(this.RawText.Trim());
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x0018A4B6 File Offset: 0x001886B6
		public TaggedString Shorten()
		{
			this.rawText = this.rawText.Shorten();
			return this;
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x0018A4CF File Offset: 0x001886CF
		public TaggedString ToLower()
		{
			return new TaggedString(this.RawText.ToLower());
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x0018A4E1 File Offset: 0x001886E1
		public TaggedString Replace(string oldValue, string newValue)
		{
			return new TaggedString(this.RawText.Replace(oldValue, newValue));
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x0018A4F5 File Offset: 0x001886F5
		public static implicit operator string(TaggedString taggedString)
		{
			return taggedString.RawText.StripTags();
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x0018A503 File Offset: 0x00188703
		public static implicit operator TaggedString(string str)
		{
			return new TaggedString(str);
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x0018A50B File Offset: 0x0018870B
		public static TaggedString operator +(TaggedString t1, TaggedString t2)
		{
			return new TaggedString(t1.RawText + t2.RawText);
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0018A525 File Offset: 0x00188725
		public static TaggedString operator +(string t1, TaggedString t2)
		{
			return new TaggedString(t1 + t2.RawText);
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x0018A539 File Offset: 0x00188739
		public static TaggedString operator +(TaggedString t1, string t2)
		{
			return new TaggedString(t1.RawText + t2);
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x0018A54D File Offset: 0x0018874D
		public override string ToString()
		{
			return this.RawText;
		}

		// Token: 0x0400370E RID: 14094
		private string rawText;

		// Token: 0x0400370F RID: 14095
		public static readonly TaggedString Empty = new TaggedString("");
	}
}
