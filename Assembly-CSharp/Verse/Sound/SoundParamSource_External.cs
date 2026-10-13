using System;

namespace Verse.Sound
{
	// Token: 0x02000E0E RID: 3598
	public class SoundParamSource_External : SoundParamSource
	{
		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06005B33 RID: 23347 RVA: 0x001D39FD File Offset: 0x001D1BFD
		public override string Label
		{
			get
			{
				if (this.inParamName == "")
				{
					return "Undefined external";
				}
				return this.inParamName;
			}
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x001D3A20 File Offset: 0x001D1C20
		public override float ValueFor(Sample samp)
		{
			float result;
			if (samp.ExternalParams.TryGetValue(this.inParamName, out result))
			{
				return result;
			}
			return this.defaultValue;
		}

		// Token: 0x04004217 RID: 16919
		[Description("The name of the independent parameter that the game will change to drive this relationship.\n\nThis must exactly match a string that the code will use to modify this sound. If the code doesn't reference this, it will have no effect.\n\nOn the graph, this is the X axis.")]
		public string inParamName = "";

		// Token: 0x04004218 RID: 16920
		[Description("If the code has never set this parameter on a sustainer, it will use this value.")]
		private float defaultValue = 1f;
	}
}
