using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F8 RID: 6648
	public class WeatherCommonalityRecord
	{
		// Token: 0x06009612 RID: 38418 RVA: 0x002BEF38 File Offset: 0x002BD138
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot, null, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400621C RID: 25116
		public WeatherDef weather;

		// Token: 0x0400621D RID: 25117
		public float commonality;
	}
}
