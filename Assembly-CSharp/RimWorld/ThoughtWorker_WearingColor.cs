using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B7 RID: 6327
	public abstract class ThoughtWorker_WearingColor : ThoughtWorker
	{
		// Token: 0x06009084 RID: 36996
		protected abstract Color? Color(Pawn p);

		// Token: 0x06009085 RID: 36997 RVA: 0x002A9778 File Offset: 0x002A7978
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Color? color = this.Color(p);
			if (color == null)
			{
				return false;
			}
			int num = 0;
			foreach (Apparel thing in p.apparel.WornApparel)
			{
				CompColorable compColorable = thing.TryGetComp<CompColorable>();
				if (compColorable != null && compColorable.Active && compColorable.Color.IndistinguishableFrom(color.Value))
				{
					num++;
				}
			}
			return (float)num / (float)p.apparel.WornApparelCount >= 0.6f;
		}

		// Token: 0x04005C47 RID: 23623
		public const float RequiredMinPercentage = 0.6f;
	}
}
