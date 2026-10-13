using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F57 RID: 12119
	public class CompProperties_Useable_CallBossgroup : CompProperties_UseEffect
	{
		// Token: 0x06011156 RID: 69974 RVA: 0x004E7E1D File Offset: 0x004E601D
		public CompProperties_Useable_CallBossgroup()
		{
			this.compClass = typeof(CompUseEffect_CallBossgroup);
		}

		// Token: 0x06011157 RID: 69975 RVA: 0x004E7E47 File Offset: 0x004E6047
		public override void Notify_PostUnlockedByResearch(ThingDef parent)
		{
			if (Find.TickManager.TicksGame > 0 && !this.unlockedLetterLabelKey.NullOrEmpty() && !this.unlockedLetterTextKey.NullOrEmpty())
			{
				this.SendBossgroupDetailsLetter(this.unlockedLetterLabelKey, this.unlockedLetterTextKey, parent);
			}
		}

		// Token: 0x06011158 RID: 69976 RVA: 0x004E7E84 File Offset: 0x004E6084
		public void SendBossgroupDetailsLetter(string labelKey, string textKey, ThingDef parent)
		{
			List<ThingDef> list = new List<ThingDef>
			{
				parent
			};
			list.AddRange(from t in this.bossgroupDef.boss.kindDef.race.killedLeavingsPlayerHostile
			select t.thingDef);
			Find.LetterStack.ReceiveLetter(this.FormatLetterLabel(labelKey), this.FormatLetterText(textKey, parent), LetterDefOf.NeutralEvent, null, null, null, list, null, 0, true);
		}

		// Token: 0x06011159 RID: 69977 RVA: 0x004E7F11 File Offset: 0x004E6111
		public string FormatLetterLabel(string label)
		{
			return label.Translate(this.bossgroupDef.boss.kindDef.Named("LEADER"));
		}

		// Token: 0x0601115A RID: 69978 RVA: 0x004E7F38 File Offset: 0x004E6138
		public string FormatLetterText(string text, ThingDef parent)
		{
			string arg = (from r in this.bossgroupDef.boss.kindDef.race.killedLeavingsPlayerHostile
			select r.Label + " x" + r.count.ToString()).ToLineList("- ", false);
			return text.Translate(parent.Named("PARENT"), this.bossgroupDef.boss.kindDef.Named("LEADER"), arg.Named("REWARDSLIST"));
		}

		// Token: 0x0400B15C RID: 45404
		public BossgroupDef bossgroupDef;

		// Token: 0x0400B15D RID: 45405
		public EffecterDef effecterDef;

		// Token: 0x0400B15E RID: 45406
		public EffecterDef prepareEffecterDef;

		// Token: 0x0400B15F RID: 45407
		[NoTranslate]
		public string spawnLetterTextKey;

		// Token: 0x0400B160 RID: 45408
		[NoTranslate]
		public string spawnLetterLabelKey;

		// Token: 0x0400B161 RID: 45409
		[NoTranslate]
		public string unlockedLetterTextKey;

		// Token: 0x0400B162 RID: 45410
		[NoTranslate]
		public string unlockedLetterLabelKey;

		// Token: 0x0400B163 RID: 45411
		public int delayTicks = -1;

		// Token: 0x0400B164 RID: 45412
		private List<ThingDef> tmpMechsUsingRewards = new List<ThingDef>();
	}
}
