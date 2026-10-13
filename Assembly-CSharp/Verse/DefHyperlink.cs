using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020001F6 RID: 502
	public class DefHyperlink
	{
		// Token: 0x06000D7A RID: 3450 RVA: 0x000024DF File Offset: 0x000006DF
		public DefHyperlink()
		{
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00040878 File Offset: 0x0003EA78
		public DefHyperlink(Def def)
		{
			this.def = def;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00040887 File Offset: 0x0003EA87
		public DefHyperlink(RoyalTitleDef def, Faction faction)
		{
			this.def = def;
			this.faction = faction;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x000408A0 File Offset: 0x0003EAA0
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured DefHyperlink: " + xmlRoot.OuterXml);
				return;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlRoot.Name, null);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Misconfigured DefHyperlink. Could not find def of type " + xmlRoot.Name);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.FirstChild.Value, null, null, typeInAnyAssembly);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00040916 File Offset: 0x0003EB16
		public static implicit operator DefHyperlink(Def def)
		{
			return new DefHyperlink
			{
				def = def
			};
		}

		// Token: 0x040007CB RID: 1995
		public Def def;

		// Token: 0x040007CC RID: 1996
		public Faction faction;
	}
}
