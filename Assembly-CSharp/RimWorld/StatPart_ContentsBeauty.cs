using System;

namespace RimWorld
{
	// Token: 0x020036EE RID: 14062
	public class StatPart_ContentsBeauty : StatPart
	{
		// Token: 0x06013812 RID: 79890 RVA: 0x005C9FA4 File Offset: 0x005C81A4
		public override void TransformValue(StatRequest req, ref float val)
		{
			IBeautyContainer beautyContainer = req.Thing as IBeautyContainer;
			if (beautyContainer != null)
			{
				val += beautyContainer.BeautyOffset;
			}
		}

		// Token: 0x06013813 RID: 79891 RVA: 0x005C9FCC File Offset: 0x005C81CC
		public override string ExplanationPart(StatRequest req)
		{
			IBeautyContainer beautyContainer = req.Thing as IBeautyContainer;
			if (beautyContainer != null)
			{
				return beautyContainer.BeautyOffsetExplanation;
			}
			return null;
		}
	}
}
