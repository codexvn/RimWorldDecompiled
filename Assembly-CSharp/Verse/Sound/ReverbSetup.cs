using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E04 RID: 3588
	public class ReverbSetup
	{
		// Token: 0x06005B1D RID: 23325 RVA: 0x001D3488 File Offset: 0x001D1688
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (widgetRow.ButtonText("Setup from preset...", "Set up the reverb filter from a preset.", true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(AudioReverbPreset)))
				{
					AudioReverbPreset audioReverbPreset = (AudioReverbPreset)obj;
					if (audioReverbPreset != AudioReverbPreset.User)
					{
						AudioReverbPreset localPreset = audioReverbPreset;
						list.Add(new FloatMenuOption(audioReverbPreset.ToString(), delegate()
						{
							this.SetupAs(localPreset);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06005B1E RID: 23326 RVA: 0x001D356C File Offset: 0x001D176C
		public void ApplyTo(AudioReverbFilter filter)
		{
			filter.dryLevel = this.dryLevel;
			filter.room = this.room;
			filter.roomHF = this.roomHF;
			filter.roomLF = this.roomLF;
			filter.decayTime = this.decayTime;
			filter.decayHFRatio = this.decayHFRatio;
			filter.reflectionsLevel = this.reflectionsLevel;
			filter.reflectionsDelay = this.reflectionsDelay;
			filter.reverbLevel = this.reverbLevel;
			filter.reverbDelay = this.reverbDelay;
			filter.hfReference = this.hfReference;
			filter.lfReference = this.lfReference;
			filter.diffusion = this.diffusion;
			filter.density = this.density;
		}

		// Token: 0x06005B1F RID: 23327 RVA: 0x001D3624 File Offset: 0x001D1824
		public static ReverbSetup Lerp(ReverbSetup A, ReverbSetup B, float t)
		{
			return new ReverbSetup
			{
				dryLevel = Mathf.Lerp(A.dryLevel, B.dryLevel, t),
				room = Mathf.Lerp(A.room, B.room, t),
				roomHF = Mathf.Lerp(A.roomHF, B.roomHF, t),
				roomLF = Mathf.Lerp(A.roomLF, B.roomLF, t),
				decayTime = Mathf.Lerp(A.decayTime, B.decayTime, t),
				decayHFRatio = Mathf.Lerp(A.decayHFRatio, B.decayHFRatio, t),
				reflectionsLevel = Mathf.Lerp(A.reflectionsLevel, B.reflectionsLevel, t),
				reflectionsDelay = Mathf.Lerp(A.reflectionsDelay, B.reflectionsDelay, t),
				reverbLevel = Mathf.Lerp(A.reverbLevel, B.reverbLevel, t),
				reverbDelay = Mathf.Lerp(A.reverbDelay, B.reverbDelay, t),
				hfReference = Mathf.Lerp(A.hfReference, B.hfReference, t),
				lfReference = Mathf.Lerp(A.lfReference, B.lfReference, t),
				diffusion = Mathf.Lerp(A.diffusion, B.diffusion, t),
				density = Mathf.Lerp(A.density, B.density, t)
			};
		}

		// Token: 0x040041F7 RID: 16887
		public float dryLevel;

		// Token: 0x040041F8 RID: 16888
		public float room;

		// Token: 0x040041F9 RID: 16889
		public float roomHF;

		// Token: 0x040041FA RID: 16890
		public float roomLF;

		// Token: 0x040041FB RID: 16891
		public float decayTime = 1f;

		// Token: 0x040041FC RID: 16892
		public float decayHFRatio = 0.5f;

		// Token: 0x040041FD RID: 16893
		public float reflectionsLevel = -10000f;

		// Token: 0x040041FE RID: 16894
		public float reflectionsDelay;

		// Token: 0x040041FF RID: 16895
		public float reverbLevel;

		// Token: 0x04004200 RID: 16896
		public float reverbDelay = 0.04f;

		// Token: 0x04004201 RID: 16897
		public float hfReference = 5000f;

		// Token: 0x04004202 RID: 16898
		public float lfReference = 250f;

		// Token: 0x04004203 RID: 16899
		public float diffusion = 100f;

		// Token: 0x04004204 RID: 16900
		public float density = 100f;
	}
}
