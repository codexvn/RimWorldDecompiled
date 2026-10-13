using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200188A RID: 6282
	public class Thought_SharedBed : Thought_Situational
	{
		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x06009003 RID: 36867 RVA: 0x002A7DC8 File Offset: 0x002A5FC8
		protected override float BaseMoodOffset
		{
			get
			{
				Pawn mostDislikedNonPartnerBedOwner = LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(this.pawn);
				if (mostDislikedNonPartnerBedOwner == null)
				{
					return 0f;
				}
				return Mathf.Min(0.05f * (float)this.pawn.relations.OpinionOf(mostDislikedNonPartnerBedOwner) - 5f, 0f);
			}
		}
	}
}
