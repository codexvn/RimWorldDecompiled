using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002679 RID: 9849
	public class Precept_Xenotype : Precept
	{
		// Token: 0x17002129 RID: 8489
		// (get) Token: 0x0600D83E RID: 55358 RVA: 0x00400FB7 File Offset: 0x003FF1B7
		private string XenotypeName
		{
			get
			{
				if (this.xenotype != null)
				{
					return this.xenotype.label;
				}
				CustomXenotype customXenotype = this.customXenotype;
				if (customXenotype == null)
				{
					return null;
				}
				return customXenotype.name;
			}
		}

		// Token: 0x1700212A RID: 8490
		// (get) Token: 0x0600D83F RID: 55359 RVA: 0x00400FDE File Offset: 0x003FF1DE
		public override string UIInfoFirstLine
		{
			get
			{
				return this.XenotypeName.CapitalizeFirst();
			}
		}

		// Token: 0x1700212B RID: 8491
		// (get) Token: 0x0600D840 RID: 55360 RVA: 0x00400FEB File Offset: 0x003FF1EB
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + this.XenotypeName.CapitalizeFirst();
			}
		}

		// Token: 0x0600D841 RID: 55361 RVA: 0x003FC6DE File Offset: 0x003FA8DE
		public override string GenerateNameRaw()
		{
			return this.name;
		}

		// Token: 0x0600D842 RID: 55362 RVA: 0x0040101C File Offset: 0x003FF21C
		public override void DrawIcon(Rect rect)
		{
			if (this.xenotype != null)
			{
				GUI.DrawTexture(rect, this.xenotype.Icon);
				return;
			}
			if (this.customXenotype != null)
			{
				GUI.DrawTexture(rect, this.customXenotype.IconDef.Icon);
			}
		}

		// Token: 0x0600D843 RID: 55363 RVA: 0x00401056 File Offset: 0x003FF256
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			Precept_Xenotype precept_Xenotype = (Precept_Xenotype)other;
			precept_Xenotype.xenotype = this.xenotype;
			precept_Xenotype.customXenotype = this.customXenotype;
		}

		// Token: 0x0600D844 RID: 55364 RVA: 0x0040107C File Offset: 0x003FF27C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<XenotypeDef>(ref this.xenotype, "xenotype");
			Scribe_Deep.Look<CustomXenotype>(ref this.customXenotype, "customXenotype", Array.Empty<object>());
		}

		// Token: 0x0400928E RID: 37518
		public XenotypeDef xenotype;

		// Token: 0x0400928F RID: 37519
		public CustomXenotype customXenotype;
	}
}
