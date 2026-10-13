using System;

namespace Verse.Sound
{
	// Token: 0x02000E1A RID: 3610
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06005B52 RID: 23378
		public abstract string Label { get; }

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06005B53 RID: 23379 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06005B54 RID: 23380
		public abstract void SetOn(Sample sample, float value);
	}
}
