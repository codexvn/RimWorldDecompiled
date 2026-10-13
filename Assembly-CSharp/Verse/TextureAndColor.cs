using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000804 RID: 2052
	public struct TextureAndColor
	{
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06003451 RID: 13393 RVA: 0x0011338A File Offset: 0x0011158A
		public bool HasValue
		{
			get
			{
				return this.texture != null;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06003452 RID: 13394 RVA: 0x00113398 File Offset: 0x00111598
		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x001133A0 File Offset: 0x001115A0
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06003454 RID: 13396 RVA: 0x001133A8 File Offset: 0x001115A8
		public static TextureAndColor None
		{
			get
			{
				return new TextureAndColor(null, Color.white);
			}
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x001133B5 File Offset: 0x001115B5
		public TextureAndColor(Texture2D texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		// Token: 0x06003456 RID: 13398 RVA: 0x001133C5 File Offset: 0x001115C5
		public static implicit operator TextureAndColor(Texture2D texture)
		{
			return new TextureAndColor(texture, Color.white);
		}

		// Token: 0x0400277D RID: 10109
		private Texture2D texture;

		// Token: 0x0400277E RID: 10110
		private Color color;
	}
}
