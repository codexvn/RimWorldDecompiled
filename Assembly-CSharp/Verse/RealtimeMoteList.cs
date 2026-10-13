using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020008BA RID: 2234
	public class RealtimeMoteList
	{
		// Token: 0x06003876 RID: 14454 RVA: 0x0012CE00 File Offset: 0x0012B000
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x0012CE0D File Offset: 0x0012B00D
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x0012CE1B File Offset: 0x0012B01B
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x0012CE2C File Offset: 0x0012B02C
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}

		// Token: 0x040029CE RID: 10702
		public List<Mote> allMotes = new List<Mote>();
	}
}
