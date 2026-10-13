using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000C93 RID: 3219
	public sealed class ThingOption
	{
		// Token: 0x060052CF RID: 21199 RVA: 0x001AB306 File Offset: 0x001A9506
		public ThingOption()
		{
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x001AB319 File Offset: 0x001A9519
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		// Token: 0x060052D1 RID: 21201 RVA: 0x001AB33C File Offset: 0x001A953C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingOption: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x001AB398 File Offset: 0x001A9598
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				", weight=",
				this.weight.ToString("0.##"),
				")"
			});
		}

		// Token: 0x04003B1A RID: 15130
		public ThingDef thingDef;

		// Token: 0x04003B1B RID: 15131
		public float weight = 1f;
	}
}
