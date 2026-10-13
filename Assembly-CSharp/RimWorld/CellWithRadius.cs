using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F67 RID: 8039
	public struct CellWithRadius : IEquatable<CellWithRadius>
	{
		// Token: 0x0600B1A2 RID: 45474 RVA: 0x0032C60F File Offset: 0x0032A80F
		public CellWithRadius(IntVec3 c, float r)
		{
			this.cell = c;
			this.radius = r;
		}

		// Token: 0x0600B1A3 RID: 45475 RVA: 0x0032C620 File Offset: 0x0032A820
		public bool Equals(CellWithRadius other)
		{
			return this.cell.Equals(other.cell) && this.radius.Equals(other.radius);
		}

		// Token: 0x0600B1A4 RID: 45476 RVA: 0x0032C65C File Offset: 0x0032A85C
		public override bool Equals(object obj)
		{
			if (obj is CellWithRadius)
			{
				CellWithRadius other = (CellWithRadius)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600B1A5 RID: 45477 RVA: 0x0032C684 File Offset: 0x0032A884
		public override int GetHashCode()
		{
			return this.cell.GetHashCode() * 397 ^ this.radius.GetHashCode();
		}

		// Token: 0x04007986 RID: 31110
		public readonly IntVec3 cell;

		// Token: 0x04007987 RID: 31111
		public readonly float radius;
	}
}
