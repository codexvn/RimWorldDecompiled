using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000E42 RID: 3650
	public class SoundParams
	{
		// Token: 0x17000EC7 RID: 3783
		public float this[string key]
		{
			get
			{
				return this.storedParams[key];
			}
			set
			{
				this.storedParams[key] = value;
			}
		}

		// Token: 0x06005BFC RID: 23548 RVA: 0x001D67CD File Offset: 0x001D49CD
		public bool TryGetValue(string key, out float val)
		{
			return this.storedParams.TryGetValue(key, out val);
		}

		// Token: 0x040042A4 RID: 17060
		private Dictionary<string, float> storedParams = new Dictionary<string, float>();

		// Token: 0x040042A5 RID: 17061
		public SoundSizeAggregator sizeAggregator;
	}
}
