using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020037DB RID: 14299
	public class DuplicateSet : IExposable
	{
		// Token: 0x1700329D RID: 12957
		// (get) Token: 0x06013D00 RID: 81152 RVA: 0x005E1603 File Offset: 0x005DF803
		public int Count
		{
			get
			{
				return this.pawns.Count;
			}
		}

		// Token: 0x06013D01 RID: 81153 RVA: 0x005E1610 File Offset: 0x005DF810
		public void Add(Pawn pawn)
		{
			this.pawns.Add(pawn);
		}

		// Token: 0x06013D02 RID: 81154 RVA: 0x005E161F File Offset: 0x005DF81F
		public void Remove(Pawn pawn)
		{
			this.pawns.Remove(pawn);
		}

		// Token: 0x06013D03 RID: 81155 RVA: 0x005E162E File Offset: 0x005DF82E
		public bool Contains(Pawn pawn)
		{
			return this.pawns.Contains(pawn);
		}

		// Token: 0x06013D04 RID: 81156 RVA: 0x005E163C File Offset: 0x005DF83C
		public void Clear()
		{
			this.pawns.Clear();
		}

		// Token: 0x06013D05 RID: 81157 RVA: 0x005E1649 File Offset: 0x005DF849
		public Pawn First()
		{
			return this.pawns.First<Pawn>();
		}

		// Token: 0x06013D06 RID: 81158 RVA: 0x005E1656 File Offset: 0x005DF856
		public HashSet<Pawn>.Enumerator GetEnumerator()
		{
			return this.pawns.GetEnumerator();
		}

		// Token: 0x06013D07 RID: 81159 RVA: 0x005E1663 File Offset: 0x005DF863
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.pawns.Contains(null))
			{
				this.pawns.Remove(null);
			}
		}

		// Token: 0x0400DB74 RID: 56180
		private HashSet<Pawn> pawns = new HashSet<Pawn>();
	}
}
