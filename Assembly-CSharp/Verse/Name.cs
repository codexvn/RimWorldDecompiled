using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200079E RID: 1950
	public abstract class Name : IExposable
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002FCA RID: 12234
		public abstract string ToStringFull { get; }

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002FCB RID: 12235
		public abstract string ToStringShort { get; }

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002FCC RID: 12236
		public abstract bool IsValid { get; }

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002FCD RID: 12237 RVA: 0x000F57CC File Offset: 0x000F39CC
		public bool UsedThisGame
		{
			get
			{
				using (IEnumerator<Name> enumerator = NameUseChecker.AllPawnsNamesEverUsed.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ConfusinglySimilarTo(this))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06002FCE RID: 12238
		public abstract bool ConfusinglySimilarTo(Name other);

		// Token: 0x06002FCF RID: 12239
		public abstract void ExposeData();

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002FD0 RID: 12240
		public abstract bool Numerical { get; }
	}
}
