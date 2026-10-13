using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE0 RID: 6880
	public class ScenPartDef : Def
	{
		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x060099BF RID: 39359 RVA: 0x002CC1E7 File Offset: 0x002CA3E7
		public bool PlayerAddRemovable
		{
			get
			{
				return this.category != ScenPartCategory.Fixed && this.canBePlayerAddedRemoved;
			}
		}

		// Token: 0x060099C0 RID: 39360 RVA: 0x002CC1FA File Offset: 0x002CA3FA
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.scenPartClass == null)
			{
				yield return "scenPartClass is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x04006963 RID: 26979
		public ScenPartCategory category;

		// Token: 0x04006964 RID: 26980
		public Type scenPartClass;

		// Token: 0x04006965 RID: 26981
		public float summaryPriority = -1f;

		// Token: 0x04006966 RID: 26982
		public float selectionWeight = 1f;

		// Token: 0x04006967 RID: 26983
		public int maxUses = 999999;

		// Token: 0x04006968 RID: 26984
		public bool canBeRandomlyAdded = true;

		// Token: 0x04006969 RID: 26985
		public bool canBePlayerAddedRemoved = true;

		// Token: 0x0400696A RID: 26986
		public Type pageClass;

		// Token: 0x0400696B RID: 26987
		public GameConditionDef gameCondition;

		// Token: 0x0400696C RID: 26988
		public bool gameConditionTargetsWorld;

		// Token: 0x0400696D RID: 26989
		public FloatRange durationRandomRange = new FloatRange(30f, 100f);

		// Token: 0x0400696E RID: 26990
		public Type designatorType;

		// Token: 0x0400696F RID: 26991
		public GenStepDef genStep;

		// Token: 0x04006970 RID: 26992
		public FactionDef preventRemovalOfFaction;
	}
}
