using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B02 RID: 6914
	public class BodyTypeGraphicData
	{
		// Token: 0x06009A67 RID: 39527 RVA: 0x002CE4B5 File Offset: 0x002CC6B5
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "bodyType", xmlRoot.Name, null, null, null);
			this.texturePath = (xmlRoot.HasChildNodes ? ParseHelper.FromString<string>(xmlRoot.FirstChild.Value) : null);
		}

		// Token: 0x04006A62 RID: 27234
		public BodyTypeDef bodyType;

		// Token: 0x04006A63 RID: 27235
		public string texturePath;
	}
}
