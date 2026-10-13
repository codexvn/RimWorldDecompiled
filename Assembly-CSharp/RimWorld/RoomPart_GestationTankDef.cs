using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A76 RID: 6774
	public class RoomPart_GestationTankDef : RoomPartDef
	{
		// Token: 0x060097FE RID: 38910 RVA: 0x002C5C23 File Offset: 0x002C3E23
		public RoomPart_GestationTankDef()
		{
			this.workerClass = typeof(RoomPart_GestationTanks);
		}

		// Token: 0x040065B1 RID: 26033
		public List<RoomPart_GestationTankDef.TankOption> options = new List<RoomPart_GestationTankDef.TankOption>();

		// Token: 0x02001A77 RID: 6775
		public enum State
		{
			// Token: 0x040065B3 RID: 26035
			Dormant,
			// Token: 0x040065B4 RID: 26036
			Proximity,
			// Token: 0x040065B5 RID: 26037
			Empty
		}

		// Token: 0x02001A78 RID: 6776
		public class TankOption
		{
			// Token: 0x060097FF RID: 38911 RVA: 0x002C5C46 File Offset: 0x002C3E46
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "state", "weight");
			}

			// Token: 0x040065B6 RID: 26038
			public RoomPart_GestationTankDef.State state;

			// Token: 0x040065B7 RID: 26039
			public float weight = 1f;
		}
	}
}
