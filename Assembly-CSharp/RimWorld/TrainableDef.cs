using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B16 RID: 6934
	public class TrainableDef : Def
	{
		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x06009AE9 RID: 39657 RVA: 0x002CFCCB File Offset: 0x002CDECB
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null)
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x06009AEA RID: 39658 RVA: 0x002CFCF4 File Offset: 0x002CDEF4
		public bool MatchesTag(string tag)
		{
			if (tag == this.defName)
			{
				return true;
			}
			using (List<string>.Enumerator enumerator = this.tags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == tag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06009AEB RID: 39659 RVA: 0x002CFD60 File Offset: 0x002CDF60
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.difficulty < 0f)
			{
				yield return "difficulty not set";
			}
			yield break;
			yield break;
		}

		// Token: 0x04006B04 RID: 27396
		public float difficulty = -1f;

		// Token: 0x04006B05 RID: 27397
		public float minBodySize;

		// Token: 0x04006B06 RID: 27398
		public List<TrainableDef> prerequisites;

		// Token: 0x04006B07 RID: 27399
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04006B08 RID: 27400
		public bool defaultTrainable;

		// Token: 0x04006B09 RID: 27401
		public TrainabilityDef requiredTrainability;

		// Token: 0x04006B0A RID: 27402
		public int steps = 1;

		// Token: 0x04006B0B RID: 27403
		public float listPriority;

		// Token: 0x04006B0C RID: 27404
		[NoTranslate]
		public string icon;

		// Token: 0x04006B0D RID: 27405
		public bool specialTrainable;

		// Token: 0x04006B0E RID: 27406
		public AbilityDef enablesAbility;

		// Token: 0x04006B0F RID: 27407
		[Unsaved(false)]
		public int indent;

		// Token: 0x04006B10 RID: 27408
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
