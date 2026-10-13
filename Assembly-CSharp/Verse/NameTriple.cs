using System;

namespace Verse
{
	// Token: 0x020007A0 RID: 1952
	public class NameTriple : Name
	{
		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x000F5A2F File Offset: 0x000F3C2F
		public string First
		{
			get
			{
				return this.firstInt;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002FE2 RID: 12258 RVA: 0x000F5A37 File Offset: 0x000F3C37
		public string Last
		{
			get
			{
				return this.lastInt;
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002FE3 RID: 12259 RVA: 0x000F5A40 File Offset: 0x000F3C40
		public string Nick
		{
			get
			{
				if (!this.nickInt.NullOrEmpty())
				{
					return this.nickInt;
				}
				if (this.Last == "")
				{
					return this.First;
				}
				if (this.First == "")
				{
					return this.Last;
				}
				if ((Gen.HashCombine<int>(this.First.GetHashCode(), this.Last.GetHashCode()) & 1) == 1)
				{
					return this.First;
				}
				return this.Last;
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x000F5AC0 File Offset: 0x000F3CC0
		public override string ToStringFull
		{
			get
			{
				if (this.First == this.Nick || this.Last == this.Nick)
				{
					return this.First + " " + this.Last;
				}
				return string.Concat(new string[]
				{
					this.First,
					" '",
					this.Nick,
					"' ",
					this.Last
				});
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002FE5 RID: 12261 RVA: 0x000F5B40 File Offset: 0x000F3D40
		public override string ToStringShort
		{
			get
			{
				return this.Nick;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x000F5B48 File Offset: 0x000F3D48
		public override bool IsValid
		{
			get
			{
				return !this.First.NullOrEmpty() && !this.Last.NullOrEmpty();
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002FE7 RID: 12263 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Numerical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x000F5B67 File Offset: 0x000F3D67
		public bool NickSet
		{
			get
			{
				return !this.nickInt.NullOrEmpty();
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x000F5B77 File Offset: 0x000F3D77
		public static NameTriple Invalid
		{
			get
			{
				return NameTriple.invalidInt;
			}
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x000F5951 File Offset: 0x000F3B51
		public NameTriple()
		{
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x000F5B7E File Offset: 0x000F3D7E
		public NameTriple(string first, string nick, string last)
		{
			this.firstInt = first.Trim();
			this.nickInt = ((nick != null) ? nick.Trim() : null);
			this.lastInt = last.Trim();
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x000F5BB0 File Offset: 0x000F3DB0
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.firstInt, "first", null, false);
			Scribe_Values.Look<string>(ref this.nickInt, "nick", null, false);
			Scribe_Values.Look<string>(ref this.lastInt, "last", null, false);
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x000F5BE8 File Offset: 0x000F3DE8
		public void PostLoad()
		{
			string text = this.firstInt;
			this.firstInt = ((text != null) ? text.Trim() : null);
			string text2 = this.nickInt;
			this.nickInt = ((text2 != null) ? text2.Trim() : null);
			string text3 = this.lastInt;
			this.lastInt = ((text3 != null) ? text3.Trim() : null);
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x000F5C40 File Offset: 0x000F3E40
		public void ResolveMissingPieces(string overrideLastName = null)
		{
			if (this.First.NullOrEmpty() && this.Nick.NullOrEmpty() && this.Last.NullOrEmpty())
			{
				Log.Error("Cannot resolve missing pieces in PawnName: No name data.");
				this.firstInt = (this.nickInt = (this.lastInt = "Empty"));
				return;
			}
			if (this.First == null)
			{
				this.firstInt = "";
			}
			if (this.Last == null)
			{
				this.lastInt = "";
			}
			if (overrideLastName != null)
			{
				this.lastInt = overrideLastName;
			}
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x000F5CCC File Offset: 0x000F3ECC
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameTriple nameTriple = other as NameTriple;
			if (nameTriple != null)
			{
				if (this.Nick != null && this.Nick == nameTriple.Nick)
				{
					return true;
				}
				if (this.First == nameTriple.First && this.Last == nameTriple.Last)
				{
					return true;
				}
			}
			NameSingle nameSingle = other as NameSingle;
			return nameSingle != null && nameSingle.Name == this.Nick;
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x000F5D48 File Offset: 0x000F3F48
		public static NameTriple FromString(string rawName, bool forceNoNick = false)
		{
			if (rawName.Trim().Length == 0)
			{
				Log.Error("Tried to parse PawnName from empty or whitespace string.");
				return NameTriple.Invalid;
			}
			NameTriple nameTriple = new NameTriple();
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < rawName.Length - 1; i++)
			{
				if (rawName[i] == ' ' && rawName[i + 1] == '\'' && num == -1)
				{
					num = i;
				}
				if (rawName[i] == '\'' && rawName[i + 1] == ' ')
				{
					num2 = i;
				}
			}
			if (num == -1 || num2 == -1)
			{
				if (!rawName.Contains(' '))
				{
					nameTriple.nickInt = rawName.Trim();
				}
				else
				{
					string[] array = rawName.Split(' ', StringSplitOptions.None);
					if (array.Length == 1)
					{
						nameTriple.nickInt = array[0].Trim();
					}
					else if (array.Length == 2)
					{
						nameTriple.firstInt = array[0].Trim();
						nameTriple.lastInt = array[1].Trim();
					}
					else
					{
						nameTriple.firstInt = array[0].Trim();
						nameTriple.lastInt = "";
						for (int j = 1; j < array.Length; j++)
						{
							NameTriple nameTriple2 = nameTriple;
							nameTriple2.lastInt += array[j];
							if (j < array.Length - 1)
							{
								NameTriple nameTriple3 = nameTriple;
								nameTriple3.lastInt += " ";
							}
						}
					}
				}
			}
			else
			{
				nameTriple.firstInt = rawName.Substring(0, num).Trim();
				if (!forceNoNick)
				{
					nameTriple.nickInt = rawName.Substring(num + 2, num2 - num - 2).Trim();
				}
				nameTriple.lastInt = ((num2 < rawName.Length - 2) ? rawName.Substring(num2 + 2).Trim() : "");
			}
			nameTriple.ResolveMissingPieces(null);
			return nameTriple;
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000F5EFF File Offset: 0x000F40FF
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.First,
				" '",
				this.Nick,
				"' ",
				this.Last
			});
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000F5F38 File Offset: 0x000F4138
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is NameTriple))
			{
				return false;
			}
			NameTriple nameTriple = (NameTriple)obj;
			return this.First == nameTriple.First && this.Last == nameTriple.Last && this.Nick == nameTriple.Nick;
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000F5F94 File Offset: 0x000F4194
		public NameTriple WithoutNick()
		{
			return new NameTriple(this.firstInt, null, this.lastInt);
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000F5FA8 File Offset: 0x000F41A8
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.First), this.Last), this.Nick);
		}

		// Token: 0x170008B7 RID: 2231
		public string this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.First;
				case 1:
					return this.Nick;
				case 2:
					return this.Last;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x04002521 RID: 9505
		[LoadAlias("first")]
		private string firstInt;

		// Token: 0x04002522 RID: 9506
		[LoadAlias("nick")]
		private string nickInt;

		// Token: 0x04002523 RID: 9507
		[LoadAlias("last")]
		private string lastInt;

		// Token: 0x04002524 RID: 9508
		private static NameTriple invalidInt = new NameTriple("Invalid", "Invalid", "Invalid");
	}
}
