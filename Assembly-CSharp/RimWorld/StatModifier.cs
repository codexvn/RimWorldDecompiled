using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C6 RID: 6598
	public class StatModifier
	{
		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x06009523 RID: 38179 RVA: 0x002BB904 File Offset: 0x002B9B04
		public string ValueToStringAsOffset
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Offset);
			}
		}

		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x06009524 RID: 38180 RVA: 0x002BB91E File Offset: 0x002B9B1E
		public string ToStringAsFactor
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Factor);
			}
		}

		// Token: 0x06009525 RID: 38181 RVA: 0x002BB938 File Offset: 0x002B9B38
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "stat", xmlRoot.Name, null, null, null);
			this.value = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06009526 RID: 38182 RVA: 0x002BB964 File Offset: 0x002B9B64
		public override string ToString()
		{
			if (this.stat == null)
			{
				return "(null stat)";
			}
			return this.stat.defName + "-" + this.value.ToString();
		}

		// Token: 0x04006120 RID: 24864
		public StatDef stat;

		// Token: 0x04006121 RID: 24865
		public float value;
	}
}
