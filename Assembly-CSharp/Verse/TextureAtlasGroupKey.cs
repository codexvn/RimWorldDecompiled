using System;

namespace Verse
{
	// Token: 0x02000800 RID: 2048
	public struct TextureAtlasGroupKey : IEquatable<TextureAtlasGroupKey>
	{
		// Token: 0x06003436 RID: 13366 RVA: 0x001122F7 File Offset: 0x001104F7
		public bool Equals(TextureAtlasGroupKey other)
		{
			return other.group == this.group && other.hasMask == this.hasMask;
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x00112318 File Offset: 0x00110518
		public override bool Equals(object obj)
		{
			if (obj is TextureAtlasGroupKey)
			{
				TextureAtlasGroupKey other = (TextureAtlasGroupKey)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x00112340 File Offset: 0x00110540
		public override int GetHashCode()
		{
			int num = this.group.GetHashCode();
			if (this.hasMask)
			{
				num *= -1;
			}
			return num;
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x0011236C File Offset: 0x0011056C
		public override string ToString()
		{
			return this.group.ToString() + "_" + this.hasMask.ToString();
		}

		// Token: 0x0400276C RID: 10092
		public TextureAtlasGroup group;

		// Token: 0x0400276D RID: 10093
		public bool hasMask;
	}
}
