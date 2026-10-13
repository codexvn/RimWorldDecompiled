using System;

namespace Verse.Sound
{
	// Token: 0x02000E02 RID: 3586
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		// Token: 0x06005B18 RID: 23320 RVA: 0x001D3348 File Offset: 0x001D1548
		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			this.duration = sourceGrain.durationRange.RandomInRange;
		}

		// Token: 0x06005B19 RID: 23321 RVA: 0x001D3368 File Offset: 0x001D1568
		public override string ToString()
		{
			return "Silence";
		}

		// Token: 0x06005B1A RID: 23322 RVA: 0x001D3370 File Offset: 0x001D1570
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ResolvedGrain_Silence resolvedGrain_Silence = obj as ResolvedGrain_Silence;
			return resolvedGrain_Silence != null && resolvedGrain_Silence.sourceGrain == this.sourceGrain;
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x001D339C File Offset: 0x001D159C
		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}

		// Token: 0x040041F6 RID: 16886
		public AudioGrain_Silence sourceGrain;
	}
}
