using System;

namespace Verse.Sound
{
	// Token: 0x02000E10 RID: 3600
	public class SoundParamSource_Underground : SoundParamSource
	{
		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06005B39 RID: 23353 RVA: 0x001D3A9C File Offset: 0x001D1C9C
		public override string Label
		{
			get
			{
				return "Underground";
			}
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x001D3AA4 File Offset: 0x001D1CA4
		public override float ValueFor(Sample samp)
		{
			Map currentMap = Find.CurrentMap;
			bool flag;
			if (currentMap == null)
			{
				flag = false;
			}
			else
			{
				MapGeneratorDef generatorDef = currentMap.generatorDef;
				bool? flag2 = (generatorDef != null) ? new bool?(generatorDef.isUnderground) : null;
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (!flag)
			{
				return 0f;
			}
			return 1f;
		}
	}
}
