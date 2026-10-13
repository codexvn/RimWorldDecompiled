using System;

namespace Verse
{
	// Token: 0x020002C5 RID: 709
	public class MissingPart
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x000512C1 File Offset: 0x0004F4C1
		public BodyPartDef BodyPart
		{
			get
			{
				return this.bodyPart;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x000512C9 File Offset: 0x0004F4C9
		public HediffDef Injury
		{
			get
			{
				return this.injury;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x000512D1 File Offset: 0x0004F4D1
		public float? Chance
		{
			get
			{
				return this.chance;
			}
		}

		// Token: 0x04000F75 RID: 3957
		private BodyPartDef bodyPart;

		// Token: 0x04000F76 RID: 3958
		private HediffDef injury;

		// Token: 0x04000F77 RID: 3959
		private float? chance;
	}
}
