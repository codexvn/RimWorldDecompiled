using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C1 RID: 6593
	public class SkillNeed_BaseBonus : SkillNeed
	{
		// Token: 0x0600950E RID: 38158 RVA: 0x002BB5B8 File Offset: 0x002B97B8
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(this.skill).Level;
			return this.ValueAtLevel(level);
		}

		// Token: 0x0600950F RID: 38159 RVA: 0x002BB5F1 File Offset: 0x002B97F1
		private float ValueAtLevel(int level)
		{
			return this.baseValue + this.bonusPerLevel * (float)level;
		}

		// Token: 0x06009510 RID: 38160 RVA: 0x002BB603 File Offset: 0x002B9803
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 1; i <= 20; i = num + 1)
			{
				if (this.ValueAtLevel(i) <= 0f)
				{
					yield return "SkillNeed yields factor < 0 at skill level " + i.ToString();
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x04006116 RID: 24854
		private float baseValue = 0.5f;

		// Token: 0x04006117 RID: 24855
		private float bonusPerLevel = 0.05f;
	}
}
