using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003744 RID: 14148
	public class StatWorker_MeleeArmorPenetration : StatWorker
	{
		// Token: 0x060139D6 RID: 80342 RVA: 0x005D396D File Offset: 0x005D1B6D
		public override bool IsDisabledFor(Thing thing)
		{
			return base.IsDisabledFor(thing) || StatDefOf.MeleeHitChance.Worker.IsDisabledFor(thing);
		}

		// Token: 0x060139D7 RID: 80343 RVA: 0x005D398A File Offset: 0x005D1B8A
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.Thing == null)
			{
				string str = "Getting MeleeArmorPenetration stat for ";
				Def def = req.Def;
				Log.Error(str + ((def != null) ? def.ToString() : null) + " without concrete pawn. This always returns 0.");
			}
			return this.GetArmorPenetration(req, applyPostProcess);
		}

		// Token: 0x060139D8 RID: 80344 RVA: 0x005D39C4 File Offset: 0x005D1BC4
		public override bool ShouldShowFor(StatRequest req)
		{
			return base.ShouldShowFor(req) && req.Thing is Pawn;
		}

		// Token: 0x060139D9 RID: 80345 RVA: 0x005D39E0 File Offset: 0x005D1BE0
		private float GetArmorPenetration(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false);
			if (updatedAvailableVerbsList.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
			{
				if (updatedAvailableVerbsList[i].IsMeleeAttack)
				{
					num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
				}
			}
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = 0f;
			for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
			{
				if (updatedAvailableVerbsList[j].IsMeleeAttack)
				{
					num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * updatedAvailableVerbsList[j].verb.verbProps.AdjustedArmorPenetration(updatedAvailableVerbsList[j].verb, pawn);
				}
			}
			return num2;
		}
	}
}
