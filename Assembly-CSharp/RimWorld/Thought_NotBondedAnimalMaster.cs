using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001857 RID: 6231
	public class Thought_NotBondedAnimalMaster : Thought_Situational
	{
		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x06008F94 RID: 36756 RVA: 0x002A6510 File Offset: 0x002A4710
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_NotBondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		// Token: 0x04005C32 RID: 23602
		private const int MaxAnimals = 3;
	}
}
