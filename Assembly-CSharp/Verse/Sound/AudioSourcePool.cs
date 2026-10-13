using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E3F RID: 3647
	public class AudioSourcePool
	{
		// Token: 0x06005BF4 RID: 23540 RVA: 0x001D657A File Offset: 0x001D477A
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		// Token: 0x06005BF5 RID: 23541 RVA: 0x001D6598 File Offset: 0x001D4798
		public AudioSource GetSource(bool onCamera)
		{
			if (onCamera)
			{
				return this.sourcePoolCamera.GetSourceCamera();
			}
			return this.sourcePoolWorld.GetSourceWorld();
		}

		// Token: 0x0400429D RID: 17053
		public AudioSourcePoolCamera sourcePoolCamera;

		// Token: 0x0400429E RID: 17054
		public AudioSourcePoolWorld sourcePoolWorld;
	}
}
