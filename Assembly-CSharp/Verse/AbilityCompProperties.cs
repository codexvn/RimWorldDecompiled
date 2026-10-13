using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200018B RID: 395
	public class AbilityCompProperties
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool OverridesPsyfocusCost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00035AD7 File Offset: 0x00033CD7
		public virtual FloatRange PsyfocusCostRange
		{
			get
			{
				return FloatRange.ZeroToOne;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00035ADE File Offset: 0x00033CDE
		public virtual string PsyfocusCostExplanation
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> ExtraStatSummary()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00035AEC File Offset: 0x00033CEC
		public virtual IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "compClass is null";
			}
			int num;
			for (int i = 0; i < parentDef.comps.Count; i = num + 1)
			{
				if (parentDef.comps[i] != this && parentDef.comps[i].compClass == this.compClass)
				{
					string str = "two comps with same compClass: ";
					Type type = this.compClass;
					yield return str + ((type != null) ? type.ToString() : null);
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04000672 RID: 1650
		[TranslationHandle]
		public Type compClass;
	}
}
