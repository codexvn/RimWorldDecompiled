using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000180 RID: 384
	public class DamageFactor
	{
		// Token: 0x06000AF8 RID: 2808 RVA: 0x00034BE7 File Offset: 0x00032DE7
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "damageDef", xmlRoot.Name, null, null, null);
			this.factor = (xmlRoot.HasChildNodes ? ParseHelper.FromString<float>(xmlRoot.FirstChild.Value) : 1f);
		}

		// Token: 0x04000640 RID: 1600
		public DamageDef damageDef;

		// Token: 0x04000641 RID: 1601
		public float factor = 1f;
	}
}
