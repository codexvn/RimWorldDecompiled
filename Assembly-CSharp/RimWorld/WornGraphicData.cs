using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E0 RID: 6624
	public class WornGraphicData
	{
		// Token: 0x06009593 RID: 38291 RVA: 0x002BCA84 File Offset: 0x002BAC84
		public WornGraphicDirectionData GetDirectionalData(Rot4 facing)
		{
			switch (facing.AsInt)
			{
			case 0:
				return this.north;
			case 1:
				return this.east;
			case 2:
				return this.south;
			case 3:
				return this.west;
			default:
				throw new ArgumentException(string.Format("Unhandled Rot4: {0}", facing));
			}
		}

		// Token: 0x06009594 RID: 38292 RVA: 0x002BCAE4 File Offset: 0x002BACE4
		public Vector2 BeltOffsetAt(Rot4 facing, BodyTypeDef bodyType)
		{
			WornGraphicDirectionData directionalData = this.GetDirectionalData(facing);
			Vector2 vector = directionalData.offset;
			if (bodyType == BodyTypeDefOf.Male)
			{
				vector += directionalData.male.offset;
			}
			else if (bodyType == BodyTypeDefOf.Female)
			{
				vector += directionalData.female.offset;
			}
			else if (bodyType == BodyTypeDefOf.Thin)
			{
				vector += directionalData.thin.offset;
			}
			else if (bodyType == BodyTypeDefOf.Hulk)
			{
				vector += directionalData.hulk.offset;
			}
			else if (bodyType == BodyTypeDefOf.Fat)
			{
				vector += directionalData.fat.offset;
			}
			return vector;
		}

		// Token: 0x06009595 RID: 38293 RVA: 0x002BCB8C File Offset: 0x002BAD8C
		public Vector2 BeltScaleAt(Rot4 facing, BodyTypeDef bodyType)
		{
			Vector2 vector = this.GetDirectionalData(facing).Scale;
			if (bodyType == BodyTypeDefOf.Male)
			{
				vector *= this.male.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Female)
			{
				vector *= this.female.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Thin)
			{
				vector *= this.thin.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Hulk)
			{
				vector *= this.hulk.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Fat)
			{
				vector *= this.fat.Scale;
			}
			return vector;
		}

		// Token: 0x0400616B RID: 24939
		public bool renderUtilityAsPack;

		// Token: 0x0400616C RID: 24940
		public WornGraphicDirectionData north;

		// Token: 0x0400616D RID: 24941
		public WornGraphicDirectionData south;

		// Token: 0x0400616E RID: 24942
		public WornGraphicDirectionData east;

		// Token: 0x0400616F RID: 24943
		public WornGraphicDirectionData west;

		// Token: 0x04006170 RID: 24944
		public WornGraphicBodyTypeData male;

		// Token: 0x04006171 RID: 24945
		public WornGraphicBodyTypeData female;

		// Token: 0x04006172 RID: 24946
		public WornGraphicBodyTypeData thin;

		// Token: 0x04006173 RID: 24947
		public WornGraphicBodyTypeData hulk;

		// Token: 0x04006174 RID: 24948
		public WornGraphicBodyTypeData fat;
	}
}
