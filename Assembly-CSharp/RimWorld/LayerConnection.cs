using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020028C5 RID: 10437
	public class LayerConnection : IEquatable<LayerConnection>, IExposable
	{
		// Token: 0x0600E448 RID: 58440 RVA: 0x0042AE46 File Offset: 0x00429046
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "tag", "fuelCost");
		}

		// Token: 0x0600E449 RID: 58441 RVA: 0x0042AE5C File Offset: 0x0042905C
		public bool Equals(LayerConnection other)
		{
			return other != null && (this == other || (this.tag == other.tag && this.fuelCost.Equals(other.fuelCost) && this.zoomMode == other.zoomMode));
		}

		// Token: 0x0600E44A RID: 58442 RVA: 0x0042AEAA File Offset: 0x004290AA
		public override bool Equals(object obj)
		{
			return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((LayerConnection)obj)));
		}

		// Token: 0x0600E44B RID: 58443 RVA: 0x0042AED8 File Offset: 0x004290D8
		public override int GetHashCode()
		{
			return HashCode.Combine<string, float, LayerConnection.ZoomMode>(this.tag, this.fuelCost, this.zoomMode);
		}

		// Token: 0x0600E44C RID: 58444 RVA: 0x0042AEF1 File Offset: 0x004290F1
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Values.Look<float>(ref this.fuelCost, "fuelCost", 0f, false);
			Scribe_Values.Look<LayerConnection.ZoomMode>(ref this.zoomMode, "zoomMode", LayerConnection.ZoomMode.None, false);
		}

		// Token: 0x040098CB RID: 39115
		public string tag;

		// Token: 0x040098CC RID: 39116
		public float fuelCost;

		// Token: 0x040098CD RID: 39117
		public LayerConnection.ZoomMode zoomMode;

		// Token: 0x020028C6 RID: 10438
		public enum ZoomMode : byte
		{
			// Token: 0x040098CF RID: 39119
			None,
			// Token: 0x040098D0 RID: 39120
			ZoomIn,
			// Token: 0x040098D1 RID: 39121
			ZoomOut
		}
	}
}
