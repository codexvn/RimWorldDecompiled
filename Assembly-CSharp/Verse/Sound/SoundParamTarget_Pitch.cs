using System;

namespace Verse.Sound
{
	// Token: 0x02000E1D RID: 3613
	public class SoundParamTarget_Pitch : SoundParamTarget
	{
		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06005B59 RID: 23385 RVA: 0x001D3D1F File Offset: 0x001D1F1F
		public override string Label
		{
			get
			{
				return "Pitch";
			}
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x001D3D28 File Offset: 0x001D1F28
		public override void SetOn(Sample sample, float value)
		{
			float num;
			if (this.pitchType == PitchParamType.Multiply)
			{
				num = value;
			}
			else
			{
				num = (float)Math.Pow(1.05946, (double)value);
			}
			sample.source.pitch = AudioSourceUtility.GetSanitizedPitch(sample.SanitizedPitch * num, "SoundParamTarget_Pitch");
		}

		// Token: 0x04004227 RID: 16935
		[Description("The scale used for this pitch input.\n\nMultiply means a multiplier for the natural frequency of the sound. 1.0 gives normal sound, 0.5 gives twice as long and one octave down, and 2.0 gives half as long and an octave up.\n\nSemitones sets a number of semitones to offset the sound.")]
		private PitchParamType pitchType;
	}
}
