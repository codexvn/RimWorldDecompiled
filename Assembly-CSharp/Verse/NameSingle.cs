using System;

namespace Verse
{
	// Token: 0x0200079F RID: 1951
	public class NameSingle : Name
	{
		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002FD2 RID: 12242 RVA: 0x000F5820 File Offset: 0x000F3A20
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002FD3 RID: 12243 RVA: 0x000F5820 File Offset: 0x000F3A20
		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002FD4 RID: 12244 RVA: 0x000F5820 File Offset: 0x000F3A20
		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002FD5 RID: 12245 RVA: 0x000F5828 File Offset: 0x000F3A28
		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002FD6 RID: 12246 RVA: 0x000F5838 File Offset: 0x000F3A38
		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x000F5840 File Offset: 0x000F3A40
		private int FirstDigitPosition
		{
			get
			{
				if (!this.numerical)
				{
					return -1;
				}
				if (this.nameInt.NullOrEmpty() || !char.IsDigit(this.nameInt[this.nameInt.Length - 1]))
				{
					return -1;
				}
				for (int i = this.nameInt.Length - 2; i >= 0; i--)
				{
					if (!char.IsDigit(this.nameInt[i]))
					{
						return i + 1;
					}
				}
				return 0;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002FD8 RID: 12248 RVA: 0x000F58B8 File Offset: 0x000F3AB8
		public string NameWithoutNumber
		{
			get
			{
				if (!this.numerical)
				{
					return this.nameInt;
				}
				int firstDigitPosition = this.FirstDigitPosition;
				if (firstDigitPosition < 0)
				{
					return this.nameInt;
				}
				int num = firstDigitPosition;
				if (num - 1 >= 0 && this.nameInt[num - 1] == ' ')
				{
					num--;
				}
				if (num <= 0)
				{
					return "";
				}
				return this.nameInt.Substring(0, num);
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002FD9 RID: 12249 RVA: 0x000F591C File Offset: 0x000F3B1C
		public int Number
		{
			get
			{
				if (!this.numerical)
				{
					return 0;
				}
				int firstDigitPosition = this.FirstDigitPosition;
				if (firstDigitPosition < 0)
				{
					return 0;
				}
				return int.Parse(this.nameInt.Substring(firstDigitPosition));
			}
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000F5951 File Offset: 0x000F3B51
		public NameSingle()
		{
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x000F5959 File Offset: 0x000F3B59
		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x000F596F File Offset: 0x000F3B6F
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x000F5998 File Offset: 0x000F3B98
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameSingle nameSingle = other as NameSingle;
			if (nameSingle != null && nameSingle.nameInt == this.nameInt)
			{
				return true;
			}
			NameTriple nameTriple = other as NameTriple;
			return nameTriple != null && nameTriple.Nick == this.nameInt;
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x000F5820 File Offset: 0x000F3A20
		public override string ToString()
		{
			return this.nameInt;
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x000F59E4 File Offset: 0x000F3BE4
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is NameSingle))
			{
				return false;
			}
			NameSingle nameSingle = (NameSingle)obj;
			return this.nameInt == nameSingle.nameInt;
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x000F5A18 File Offset: 0x000F3C18
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}

		// Token: 0x0400251F RID: 9503
		private string nameInt;

		// Token: 0x04002520 RID: 9504
		private bool numerical;
	}
}
