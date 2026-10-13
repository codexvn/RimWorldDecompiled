using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DFC RID: 3580
	public class AudioGrain_Folder : AudioGrain
	{
		// Token: 0x06005AFD RID: 23293 RVA: 0x001D303B File Offset: 0x001D123B
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			foreach (AudioClip clip in ContentFinder<AudioClip>.GetAllInFolder(this.clipFolderPath))
			{
				yield return new ResolvedGrain_Clip(clip);
			}
			IEnumerator<AudioClip> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040041E9 RID: 16873
		[LoadAlias("clipPath")]
		[NoTranslate]
		public string clipFolderPath = "";
	}
}
