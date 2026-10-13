using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DFA RID: 3578
	public class AudioGrain_Clip : AudioGrain
	{
		// Token: 0x06005AF3 RID: 23283 RVA: 0x001D2F30 File Offset: 0x001D1130
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioClip audioClip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			if (audioClip != null)
			{
				yield return new ResolvedGrain_Clip(audioClip);
			}
			else
			{
				Log.Error("Grain couldn't resolve: Clip not found at " + this.clipPath);
			}
			yield break;
		}

		// Token: 0x040041E4 RID: 16868
		[NoTranslate]
		public string clipPath = "";
	}
}
