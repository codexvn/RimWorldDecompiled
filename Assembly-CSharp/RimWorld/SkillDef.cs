using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001AE8 RID: 6888
	public class SkillDef : Def
	{
		// Token: 0x060099F0 RID: 39408 RVA: 0x002CC93B File Offset: 0x002CAB3B
		public override void PostLoad()
		{
			if (this.label == null)
			{
				this.label = this.skillLabel;
			}
		}

		// Token: 0x060099F1 RID: 39409 RVA: 0x002CC951 File Offset: 0x002CAB51
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.lessonInteraction == null)
			{
				this.lessonInteraction = InteractionDefOf.LessonGeneric;
			}
		}

		// Token: 0x060099F2 RID: 39410 RVA: 0x002CC96C File Offset: 0x002CAB6C
		public bool IsDisabled(WorkTags combinedDisabledWorkTags, IEnumerable<WorkTypeDef> disabledWorkTypes)
		{
			if ((combinedDisabledWorkTags & this.disablingWorkTags) != WorkTags.None)
			{
				return true;
			}
			if (this.neverDisabledBasedOnWorkTypes)
			{
				return false;
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			bool result = false;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				for (int j = 0; j < workTypeDef.relevantSkills.Count; j++)
				{
					if (workTypeDef.relevantSkills[j] == this)
					{
						if (!disabledWorkTypes.Contains(workTypeDef))
						{
							return false;
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x040069A8 RID: 27048
		[MustTranslate]
		public string skillLabel;

		// Token: 0x040069A9 RID: 27049
		public RulePack generalRules;

		// Token: 0x040069AA RID: 27050
		public bool usuallyDefinedInBackstories = true;

		// Token: 0x040069AB RID: 27051
		public bool pawnCreatorSummaryVisible;

		// Token: 0x040069AC RID: 27052
		public WorkTags disablingWorkTags;

		// Token: 0x040069AD RID: 27053
		public float listOrder;

		// Token: 0x040069AE RID: 27054
		public bool neverDisabledBasedOnWorkTypes;

		// Token: 0x040069AF RID: 27055
		public InteractionDef lessonInteraction;
	}
}
