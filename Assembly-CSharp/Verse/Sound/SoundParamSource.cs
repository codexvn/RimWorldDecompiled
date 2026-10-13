using System;

namespace Verse.Sound
{
	// Token: 0x02000E0D RID: 3597
	[EditorShowClassName]
	[EditorReplaceable]
	public abstract class SoundParamSource
	{
		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06005B30 RID: 23344
		public abstract string Label { get; }

		// Token: 0x06005B31 RID: 23345
		public abstract float ValueFor(Sample samp);
	}
}
