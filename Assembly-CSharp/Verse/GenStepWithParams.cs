using System;

namespace Verse
{
	// Token: 0x02000282 RID: 642
	public struct GenStepWithParams : IEquatable<GenStepWithParams>
	{
		// Token: 0x06001028 RID: 4136 RVA: 0x0004BF37 File Offset: 0x0004A137
		public GenStepWithParams(GenStepDef def, GenStepParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0004BF47 File Offset: 0x0004A147
		public bool Equals(GenStepWithParams other)
		{
			return object.Equals(this.def, other.def);
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0004BF5C File Offset: 0x0004A15C
		public override bool Equals(object obj)
		{
			if (obj is GenStepWithParams)
			{
				GenStepWithParams other = (GenStepWithParams)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0004BF81 File Offset: 0x0004A181
		public override int GetHashCode()
		{
			return HashCode.Combine<GenStepDef, GenStepParams>(this.def, this.parms);
		}

		// Token: 0x04000CBB RID: 3259
		public GenStepDef def;

		// Token: 0x04000CBC RID: 3260
		public GenStepParams parms;
	}
}
