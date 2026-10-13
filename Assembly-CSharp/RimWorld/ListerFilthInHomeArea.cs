using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F72 RID: 8050
	public class ListerFilthInHomeArea
	{
		// Token: 0x17001B13 RID: 6931
		// (get) Token: 0x0600B1CD RID: 45517 RVA: 0x0032CFF6 File Offset: 0x0032B1F6
		public List<Thing> FilthInHomeArea
		{
			get
			{
				return this.filthInHomeArea;
			}
		}

		// Token: 0x0600B1CE RID: 45518 RVA: 0x0032CFFE File Offset: 0x0032B1FE
		public ListerFilthInHomeArea(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600B1CF RID: 45519 RVA: 0x0032D018 File Offset: 0x0032B218
		public void RebuildAll()
		{
			this.filthInHomeArea.Clear();
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.Notify_HomeAreaChanged(c);
			}
		}

		// Token: 0x0600B1D0 RID: 45520 RVA: 0x0032D078 File Offset: 0x0032B278
		public void Notify_FilthSpawned(Filth f)
		{
			if (this.map.areaManager.Home[f.Position])
			{
				this.filthInHomeArea.Add(f);
			}
		}

		// Token: 0x0600B1D1 RID: 45521 RVA: 0x0032D0A4 File Offset: 0x0032B2A4
		public void Notify_FilthDespawned(Filth f)
		{
			for (int i = 0; i < this.filthInHomeArea.Count; i++)
			{
				if (this.filthInHomeArea[i] == f)
				{
					this.filthInHomeArea.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600B1D2 RID: 45522 RVA: 0x0032D0E4 File Offset: 0x0032B2E4
		public void Notify_HomeAreaChanged(IntVec3 c)
		{
			if (this.map.areaManager.Home[c])
			{
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Filth filth = thingList[i] as Filth;
					if (filth != null)
					{
						this.filthInHomeArea.Add(filth);
					}
				}
				return;
			}
			for (int j = this.filthInHomeArea.Count - 1; j >= 0; j--)
			{
				if (this.filthInHomeArea[j].Position == c)
				{
					this.filthInHomeArea.RemoveAt(j);
				}
			}
		}

		// Token: 0x0600B1D3 RID: 45523 RVA: 0x0032D184 File Offset: 0x0032B384
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Filth in home area");
			foreach (Thing thing in this.filthInHomeArea)
			{
				stringBuilder.AppendLine(thing.ThingID + " " + thing.Position.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400799E RID: 31134
		private Map map;

		// Token: 0x0400799F RID: 31135
		private List<Thing> filthInHomeArea = new List<Thing>();
	}
}
