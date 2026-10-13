using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D58 RID: 11608
	public class MeditationFocusOffsetPerBuilding
	{
		// Token: 0x060104F3 RID: 66803 RVA: 0x000024DF File Offset: 0x000006DF
		public MeditationFocusOffsetPerBuilding()
		{
		}

		// Token: 0x060104F4 RID: 66804 RVA: 0x004B5937 File Offset: 0x004B3B37
		public MeditationFocusOffsetPerBuilding(ThingDef building, float offset)
		{
			this.building = building;
			this.offset = offset;
		}

		// Token: 0x060104F5 RID: 66805 RVA: 0x004B5950 File Offset: 0x004B3B50
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.Name != "li")
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "building", xmlRoot, null, null, null);
				this.offset = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "building", xmlRoot.InnerText, null, null, null);
			this.offset = float.MinValue;
		}

		// Token: 0x0400AA31 RID: 43569
		public ThingDef building;

		// Token: 0x0400AA32 RID: 43570
		public float offset;
	}
}
