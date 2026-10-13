using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A82 RID: 6786
	public class LearningDesireDef : Def
	{
		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x0600982E RID: 38958 RVA: 0x002C66F3 File Offset: 0x002C48F3
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x0600982F RID: 38959 RVA: 0x002C671B File Offset: 0x002C491B
		public LearningGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (LearningGiver)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06009830 RID: 38960 RVA: 0x002C674D File Offset: 0x002C494D
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.jobDef == null)
			{
				yield return "Learning desires require a job to be configured.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04006607 RID: 26119
		public float selectionWeight = 1f;

		// Token: 0x04006608 RID: 26120
		public float xpPerTick;

		// Token: 0x04006609 RID: 26121
		[NoTranslate]
		public string iconPath;

		// Token: 0x0400660A RID: 26122
		public Type workerClass = typeof(LearningGiver);

		// Token: 0x0400660B RID: 26123
		public JobDef jobDef;

		// Token: 0x0400660C RID: 26124
		private Texture2D icon;

		// Token: 0x0400660D RID: 26125
		private LearningGiver workerInt;
	}
}
