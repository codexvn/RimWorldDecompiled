using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002427 RID: 9255
	public class PawnRelationWorker_Kin : PawnRelationWorker
	{
		// Token: 0x0600C85A RID: 51290 RVA: 0x003A8198 File Offset: 0x003A6398
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other || me.RaceProps.Animal != other.RaceProps.Animal)
			{
				return false;
			}
			IEnumerable<Pawn> familyByBlood = me.relations.FamilyByBlood;
			HashSet<Pawn> hashSet = familyByBlood as HashSet<Pawn>;
			if (hashSet == null)
			{
				return familyByBlood.Contains(other);
			}
			return hashSet.Contains(other);
		}
	}
}
