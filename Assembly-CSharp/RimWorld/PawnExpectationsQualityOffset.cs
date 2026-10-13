using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02002757 RID: 10071
	public class PawnExpectationsQualityOffset
	{
		// Token: 0x170021CE RID: 8654
		// (get) Token: 0x0600DCEB RID: 56555 RVA: 0x0041020F File Offset: 0x0040E40F
		public string Label
		{
			get
			{
				return this.labelOverride ?? this.expectation.LabelCap;
			}
		}

		// Token: 0x0600DCEC RID: 56556 RVA: 0x0041022B File Offset: 0x0040E42B
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "expectation", xmlRoot.Name, null, null, null);
			this.offset = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040094F8 RID: 38136
		public ExpectationDef expectation;

		// Token: 0x040094F9 RID: 38137
		public float offset;

		// Token: 0x040094FA RID: 38138
		[MustTranslate]
		public string labelOverride;
	}
}
