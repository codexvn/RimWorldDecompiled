using System;

namespace RimWorld
{
	// Token: 0x020020F6 RID: 8438
	public class TileMutatorWorker_Pond : TileMutatorWorker_Lake
	{
		// Token: 0x17001BFC RID: 7164
		// (get) Token: 0x0600B843 RID: 47171 RVA: 0x0034E5D8 File Offset: 0x0034C7D8
		protected override float LakeRadius
		{
			get
			{
				return 0.3f;
			}
		}

		// Token: 0x17001BFD RID: 7165
		// (get) Token: 0x0600B844 RID: 47172 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool GenerateDeepWater
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B845 RID: 47173 RVA: 0x0034E5E7 File Offset: 0x0034C7E7
		public TileMutatorWorker_Pond(TileMutatorDef def) : base(def)
		{
		}
	}
}
