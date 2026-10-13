using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A24 RID: 6692
	public class PawnGenOption
	{
		// Token: 0x1700170B RID: 5899
		// (get) Token: 0x060096DC RID: 38620 RVA: 0x002C1AD3 File Offset: 0x002BFCD3
		public float Cost
		{
			get
			{
				return this.kind.combatPower;
			}
		}

		// Token: 0x060096DD RID: 38621 RVA: 0x002C1AE0 File Offset: 0x002BFCE0
		public override string ToString()
		{
			return string.Format("({0} w={1:F2} c={2})", (this.kind != null) ? this.kind.ToString() : "null", this.selectionWeight, (this.kind != null) ? this.Cost.ToString("F2") : "null");
		}

		// Token: 0x060096DE RID: 38622 RVA: 0x002C1B3E File Offset: 0x002BFD3E
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name, null, null, null);
			this.selectionWeight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04006364 RID: 25444
		public PawnKindDef kind;

		// Token: 0x04006365 RID: 25445
		public float selectionWeight;
	}
}
