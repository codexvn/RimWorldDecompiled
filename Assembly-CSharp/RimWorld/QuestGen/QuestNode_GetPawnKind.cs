using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A65 RID: 14949
	public class QuestNode_GetPawnKind : QuestNode
	{
		// Token: 0x060146FD RID: 83709 RVA: 0x0061B2B6 File Offset: 0x006194B6
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return !this.failIfNotFound || this.GetKindDef(slate) != null;
		}

		// Token: 0x060146FE RID: 83710 RVA: 0x0061B2D3 File Offset: 0x006194D3
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060146FF RID: 83711 RVA: 0x0061B2E0 File Offset: 0x006194E0
		private void SetVars(Slate slate)
		{
			slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), this.GetKindDef(slate), false);
		}

		// Token: 0x06014700 RID: 83712 RVA: 0x0061B2FC File Offset: 0x006194FC
		private PawnKindDef GetKindDef(Slate slate)
		{
			QuestNode_GetPawnKind.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetPawnKind.Option x) => x.weight);
			PawnKindDef result;
			if (option.kindDef != null)
			{
				result = option.kindDef;
			}
			else if (option.anyAnimal)
			{
				int highestAnimalSkill = option.mustBeAbleToHandleAnimal ? 0 : int.MaxValue;
				if (option.mustBeAbleToHandleAnimal)
				{
					Map map = slate.Get<Map>("map", null, false);
					if (map != null)
					{
						using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonistsSpawned.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Pawn pawn = enumerator.Current;
								highestAnimalSkill = Mathf.Max(highestAnimalSkill, pawn.skills.GetSkill(SkillDefOf.Animals).Level);
							}
							goto IL_158;
						}
					}
					foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists)
					{
						highestAnimalSkill = Mathf.Max(highestAnimalSkill, pawn2.skills.GetSkill(SkillDefOf.Animals).Level);
					}
				}
				IL_158:
				result = (from x in DefDatabase<PawnKindDef>.AllDefs
				where x.RaceProps.Animal && !x.RaceProps.neverIncludeInQuests && !x.RaceProps.Dryad && (option.onlyAllowedFleshType == null || x.RaceProps.FleshType == option.onlyAllowedFleshType) && base.<GetKindDef>g__CanHandle|1(x)
				select x).RandomElementWithFallback(null);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0400E219 RID: 57881
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E21A RID: 57882
		public SlateRef<List<QuestNode_GetPawnKind.Option>> options;

		// Token: 0x0400E21B RID: 57883
		public bool failIfNotFound;

		// Token: 0x02003A66 RID: 14950
		public class Option
		{
			// Token: 0x0400E21C RID: 57884
			public PawnKindDef kindDef;

			// Token: 0x0400E21D RID: 57885
			public float weight;

			// Token: 0x0400E21E RID: 57886
			public bool anyAnimal;

			// Token: 0x0400E21F RID: 57887
			public bool mustBeAbleToHandleAnimal;

			// Token: 0x0400E220 RID: 57888
			public FleshTypeDef onlyAllowedFleshType;
		}
	}
}
