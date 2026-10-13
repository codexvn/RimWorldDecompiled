using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E06 RID: 3590
	public abstract class SoundFilter
	{
		// Token: 0x06005B23 RID: 23331
		public abstract void SetupOn(AudioSource source);

		// Token: 0x06005B24 RID: 23332 RVA: 0x001D3808 File Offset: 0x001D1A08
		protected static T GetOrMakeFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T t = source.gameObject.GetComponent<T>();
			if (t != null)
			{
				t.enabled = true;
			}
			else
			{
				t = source.gameObject.AddComponent<T>();
			}
			return t;
		}
	}
}
