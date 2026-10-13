using System;

namespace Verse.Sound
{
	// Token: 0x02000E0C RID: 3596
	public class SoundParameterMapping
	{
		// Token: 0x06005B2E RID: 23342 RVA: 0x001D3964 File Offset: 0x001D1B64
		public SoundParameterMapping()
		{
			this.curve = new SimpleCurve();
			this.curve.Add(new CurvePoint(0f, 0f), true);
			this.curve.Add(new CurvePoint(1f, 1f), true);
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x001D39B8 File Offset: 0x001D1BB8
		public void Apply(Sample samp)
		{
			if (this.inParam == null || this.outParam == null)
			{
				return;
			}
			float x = this.inParam.ValueFor(samp);
			float value = this.curve.Evaluate(x);
			this.outParam.SetOn(samp, value);
		}

		// Token: 0x04004213 RID: 16915
		[Description("The independent parameter that the game will change to drive this relationship.\n\nOn the graph, this is the X axis.")]
		public SoundParamSource inParam;

		// Token: 0x04004214 RID: 16916
		[Description("The dependent parameter that will respond to changes to the in-parameter.\n\nThis must match something the game can change about this sound.\n\nOn the graph, this is the y-axis.")]
		public SoundParamTarget outParam;

		// Token: 0x04004215 RID: 16917
		[Description("Determines when sound parameters should be applies to samples.\n\nConstant means the parameters are updated every frame and can change continuously.\n\nOncePerSample means that the parameters are applied exactly once to each sample that plays.")]
		public SoundParamUpdateMode paramUpdateMode;

		// Token: 0x04004216 RID: 16918
		[EditorHidden]
		public SimpleCurve curve;
	}
}
