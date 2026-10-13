using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000762 RID: 1890
	public class Hediff_PsychicLove : HediffWithTarget
	{
		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002E62 RID: 11874 RVA: 0x000EEB9C File Offset: 0x000ECD9C
		public override string LabelBase
		{
			get
			{
				string[] array = new string[5];
				array[0] = base.LabelBase;
				array[1] = " ";
				array[2] = this.def.targetPrefix;
				array[3] = " ";
				int num = 4;
				Thing target = this.target;
				array[num] = ((target != null) ? target.LabelShortCap : null);
				return string.Concat(array);
			}
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x000EEBF0 File Offset: 0x000ECDF0
		public override void Notify_RelationAdded(Pawn otherPawn, PawnRelationDef relationDef)
		{
			if (otherPawn == this.target && (relationDef == PawnRelationDefOf.Lover || relationDef == PawnRelationDefOf.Fiance || relationDef == PawnRelationDefOf.Spouse))
			{
				this.pawn.health.RemoveHediff(this);
			}
		}
	}
}
