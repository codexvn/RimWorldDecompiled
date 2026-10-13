using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C80 RID: 3200
	public struct CurveMark
	{
		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06005203 RID: 20995 RVA: 0x001A8A68 File Offset: 0x001A6C68
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06005204 RID: 20996 RVA: 0x001A8A70 File Offset: 0x001A6C70
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06005205 RID: 20997 RVA: 0x001A8A78 File Offset: 0x001A6C78
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x001A8A80 File Offset: 0x001A6C80
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x04003AD3 RID: 15059
		private float x;

		// Token: 0x04003AD4 RID: 15060
		private string message;

		// Token: 0x04003AD5 RID: 15061
		private Color color;
	}
}
