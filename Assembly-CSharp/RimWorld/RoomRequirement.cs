using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020037A3 RID: 14243
	public abstract class RoomRequirement : IExposable
	{
		// Token: 0x06013BAF RID: 80815 RVA: 0x005DC606 File Offset: 0x005DA806
		public bool MetOrDisabled(Room room, Pawn p = null)
		{
			return this.Disabled(room, p) || this.Met(room, p);
		}

		// Token: 0x06013BB0 RID: 80816 RVA: 0x005DC61C File Offset: 0x005DA81C
		public virtual bool Disabled(Room room, Pawn p = null)
		{
			if (this.disablingPrecepts != null && p != null && p.Ideo != null)
			{
				foreach (Precept precept in p.Ideo.PreceptsListForReading)
				{
					if (this.disablingPrecepts.Contains(precept.def))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06013BB1 RID: 80817
		public abstract bool Met(Room r, Pawn p = null);

		// Token: 0x06013BB2 RID: 80818 RVA: 0x005DC69C File Offset: 0x005DA89C
		public virtual string Label(Room r = null)
		{
			return this.labelKey.Translate();
		}

		// Token: 0x06013BB3 RID: 80819 RVA: 0x005DC6AE File Offset: 0x005DA8AE
		public string LabelCap(Room r = null)
		{
			return this.Label(r).CapitalizeFirst();
		}

		// Token: 0x06013BB4 RID: 80820 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x06013BB5 RID: 80821 RVA: 0x005DC6BC File Offset: 0x005DA8BC
		public virtual bool SameOrSubsetOf(RoomRequirement other)
		{
			return base.GetType() == other.GetType();
		}

		// Token: 0x06013BB6 RID: 80822 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool PlayerCanBuildNow()
		{
			return true;
		}

		// Token: 0x06013BB7 RID: 80823 RVA: 0x005DC6CF File Offset: 0x005DA8CF
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.labelKey, "labelKey", null, false);
		}

		// Token: 0x0400DAA5 RID: 55973
		public List<PreceptDef> disablingPrecepts;

		// Token: 0x0400DAA6 RID: 55974
		[NoTranslate]
		public string labelKey;
	}
}
