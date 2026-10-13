using System;

namespace RimWorld
{
	// Token: 0x020036E3 RID: 14051
	public class StatPart_Biocoded : StatPart
	{
		// Token: 0x060137ED RID: 79853 RVA: 0x005C995D File Offset: 0x005C7B5D
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && CompBiocodable.IsBiocoded(req.Thing))
			{
				val *= 0f;
			}
		}

		// Token: 0x060137EE RID: 79854 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string ExplanationPart(StatRequest req)
		{
			return null;
		}
	}
}
