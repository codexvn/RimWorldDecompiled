using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001854 RID: 6228
	public class Thought_BondedAnimalMaster : Thought_Situational
	{
		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x06008F8E RID: 36750 RVA: 0x002A64A1 File Offset: 0x002A46A1
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_BondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		// Token: 0x04005C31 RID: 23601
		private const int MaxAnimals = 3;
	}
}
