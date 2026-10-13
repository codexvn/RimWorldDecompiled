using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000251 RID: 593
	public class LifeStageWorkSettings
	{
		// Token: 0x06000F10 RID: 3856 RVA: 0x00046787 File Offset: 0x00044987
		public bool IsDisabled(Pawn pawn)
		{
			return pawn.ageTracker.AgeBiologicalYears < this.minAge;
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0004679C File Offset: 0x0004499C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "workType", xmlRoot.Name, null, null, null);
			this.minAge = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04000A85 RID: 2693
		public WorkTypeDef workType;

		// Token: 0x04000A86 RID: 2694
		public int minAge;
	}
}
