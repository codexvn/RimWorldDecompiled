using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA0 RID: 6816
	public class MusicTransitionDef : Def
	{
		// Token: 0x0600989D RID: 39069 RVA: 0x002C830F File Offset: 0x002C650F
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.workerType != null && !typeof(MusicTransition).IsAssignableFrom(this.workerType))
			{
				yield return "[def: " + this.defName + "] Music condition type is not a subclass of MusicTransition, type was: " + this.workerType.FullName;
			}
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04006700 RID: 26368
		public Type workerType = typeof(MusicTransition);

		// Token: 0x04006701 RID: 26369
		public int priority = 100;

		// Token: 0x04006702 RID: 26370
		public MusicSequenceDef sequence;

		// Token: 0x04006703 RID: 26371
		public MusicDangerRequirement dangerRequirement;

		// Token: 0x04006704 RID: 26372
		public bool overridesInterruptions;
	}
}
