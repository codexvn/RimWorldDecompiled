using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C4 RID: 708
	public class AlternateGraphic
	{
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x00051086 File Offset: 0x0004F286
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00051090 File Offset: 0x0004F290
		public Graphic GetGraphic(Graphic other)
		{
			if (this.graphicData == null)
			{
				this.graphicData = new GraphicData();
			}
			this.graphicData.CopyFrom(other.data);
			if (!this.texPath.NullOrEmpty())
			{
				this.graphicData.texPath = this.texPath;
			}
			this.graphicData.color = (this.color ?? other.color);
			this.graphicData.colorTwo = (this.colorTwo ?? other.colorTwo);
			return this.graphicData.Graphic;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00051140 File Offset: 0x0004F340
		public Graphic GetSwimmingGraphic(Graphic other)
		{
			if (this.swimmingGraphicData == null)
			{
				this.swimmingGraphicData = new GraphicData();
			}
			this.swimmingGraphicData.CopyFrom(other.data);
			if (!this.swimmingTexPath.NullOrEmpty())
			{
				this.swimmingGraphicData.texPath = this.swimmingTexPath;
			}
			return this.swimmingGraphicData.Graphic;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0005119C File Offset: 0x0004F39C
		public Graphic GetDessicatedGraphic(Graphic other)
		{
			if (this.dessicatedGraphicData == null)
			{
				this.dessicatedGraphicData = new GraphicData();
			}
			this.dessicatedGraphicData.CopyFrom(other.data);
			if (!this.dessicatedTexPath.NullOrEmpty())
			{
				this.dessicatedGraphicData.texPath = this.dessicatedTexPath;
			}
			return this.dessicatedGraphicData.Graphic;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x000511F8 File Offset: 0x0004F3F8
		public Graphic GetRottingGraphic(Graphic other)
		{
			if (this.rottingGraphicData == null)
			{
				this.rottingGraphicData = new GraphicData();
			}
			this.rottingGraphicData.CopyFrom(other.data);
			if (!this.rottingTexPath.NullOrEmpty())
			{
				this.rottingGraphicData.texPath = this.rottingTexPath;
			}
			return this.rottingGraphicData.Graphic;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00051254 File Offset: 0x0004F454
		public Graphic GetStationaryGraphic(Graphic other)
		{
			if (this.stationaryGraphicData == null)
			{
				this.stationaryGraphicData = new GraphicData();
			}
			this.stationaryGraphicData.CopyFrom(other.data);
			if (!this.stationaryTexPath.NullOrEmpty())
			{
				this.stationaryGraphicData.texPath = this.stationaryTexPath;
			}
			return this.stationaryGraphicData.Graphic;
		}

		// Token: 0x04000F67 RID: 3943
		private float weight = 0.5f;

		// Token: 0x04000F68 RID: 3944
		private string texPath;

		// Token: 0x04000F69 RID: 3945
		private string rottingTexPath;

		// Token: 0x04000F6A RID: 3946
		private string dessicatedTexPath;

		// Token: 0x04000F6B RID: 3947
		private string swimmingTexPath;

		// Token: 0x04000F6C RID: 3948
		private string stationaryTexPath;

		// Token: 0x04000F6D RID: 3949
		private Color? color;

		// Token: 0x04000F6E RID: 3950
		private Color? colorTwo;

		// Token: 0x04000F6F RID: 3951
		public GraphicData graphicData;

		// Token: 0x04000F70 RID: 3952
		public GraphicData dessicatedGraphicData;

		// Token: 0x04000F71 RID: 3953
		public GraphicData swimmingGraphicData;

		// Token: 0x04000F72 RID: 3954
		public GraphicData rottingGraphicData;

		// Token: 0x04000F73 RID: 3955
		public GraphicData stationaryGraphicData;

		// Token: 0x04000F74 RID: 3956
		public List<AttachPoint> attachPoints;
	}
}
