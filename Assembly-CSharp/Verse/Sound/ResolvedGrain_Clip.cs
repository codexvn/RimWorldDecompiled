using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E01 RID: 3585
	public class ResolvedGrain_Clip : ResolvedGrain
	{
		// Token: 0x06005B14 RID: 23316 RVA: 0x001D32C7 File Offset: 0x001D14C7
		public ResolvedGrain_Clip(AudioClip clip)
		{
			this.clip = clip;
			this.duration = clip.length;
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x001D32E2 File Offset: 0x001D14E2
		public override string ToString()
		{
			return "Clip:" + this.clip.name;
		}

		// Token: 0x06005B16 RID: 23318 RVA: 0x001D32FC File Offset: 0x001D14FC
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ResolvedGrain_Clip resolvedGrain_Clip = obj as ResolvedGrain_Clip;
			return resolvedGrain_Clip != null && resolvedGrain_Clip.clip == this.clip;
		}

		// Token: 0x06005B17 RID: 23319 RVA: 0x001D332B File Offset: 0x001D152B
		public override int GetHashCode()
		{
			if (this.clip == null)
			{
				return 0;
			}
			return this.clip.GetHashCode();
		}

		// Token: 0x040041F5 RID: 16885
		public AudioClip clip;
	}
}
