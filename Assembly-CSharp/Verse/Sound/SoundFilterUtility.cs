using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E36 RID: 3638
	public static class SoundFilterUtility
	{
		// Token: 0x06005BCE RID: 23502 RVA: 0x001D5CA2 File Offset: 0x001D3EA2
		public static void DisableAllFiltersOn(AudioSource source)
		{
			SoundFilterUtility.DisableFilterOn<AudioLowPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioHighPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioEchoFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioReverbFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioDistortionFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioChorusFilter>(source);
		}

		// Token: 0x06005BCF RID: 23503 RVA: 0x001D5CC8 File Offset: 0x001D3EC8
		private static void DisableFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T component = source.GetComponent<T>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}
}
