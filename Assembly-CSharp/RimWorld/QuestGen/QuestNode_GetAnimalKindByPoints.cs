using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A34 RID: 14900
	public class QuestNode_GetAnimalKindByPoints : QuestNode
	{
		// Token: 0x06014652 RID: 83538 RVA: 0x0061803D File Offset: 0x0061623D
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		// Token: 0x06014653 RID: 83539 RVA: 0x00618046 File Offset: 0x00616246
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014654 RID: 83540 RVA: 0x00618054 File Offset: 0x00616254
		private bool SetVars(Slate slate)
		{
			float points = slate.Get<float>("points", 0f, false);
			PawnKindDef var;
			if ((from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && !x.RaceProps.Dryad && !x.RaceProps.neverIncludeInQuests && x.combatPower < points
			select x).TryRandomElement(out var))
			{
				slate.Set<PawnKindDef>("animalKindDef", var, false);
				return true;
			}
			return false;
		}
	}
}
