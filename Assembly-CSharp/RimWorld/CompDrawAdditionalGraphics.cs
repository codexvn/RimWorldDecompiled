using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C66 RID: 11366
	public class CompDrawAdditionalGraphics : ThingComp
	{
		// Token: 0x17002829 RID: 10281
		// (get) Token: 0x0600FED7 RID: 65239 RVA: 0x0049DB0C File Offset: 0x0049BD0C
		private CompProperties_DrawAdditionalGraphics Props
		{
			get
			{
				return (CompProperties_DrawAdditionalGraphics)this.props;
			}
		}

		// Token: 0x0600FED8 RID: 65240 RVA: 0x0049DB1C File Offset: 0x0049BD1C
		public override void PostDraw()
		{
			foreach (GraphicData graphicData in this.Props.graphics)
			{
				graphicData.Graphic.Draw(this.parent.DrawPos, this.parent.Rotation, this.parent, 0f);
			}
		}
	}
}
