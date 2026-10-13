using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CD RID: 205
	public struct CurvePoint
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0001B936 File Offset: 0x00019B36
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0001B93E File Offset: 0x00019B3E
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001B94B File Offset: 0x00019B4B
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001B958 File Offset: 0x00019B58
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001B967 File Offset: 0x00019B67
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001B970 File Offset: 0x00019B70
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint(ParseHelper.FromString<Vector2>(str));
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001B97D File Offset: 0x00019B7D
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001B936 File Offset: 0x00019B36
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}

		// Token: 0x040002E4 RID: 740
		private Vector2 loc;
	}
}
