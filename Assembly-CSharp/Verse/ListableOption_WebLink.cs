using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFD RID: 3069
	public class ListableOption_WebLink : ListableOption
	{
		// Token: 0x06004E70 RID: 20080 RVA: 0x0018F096 File Offset: 0x0018D296
		public ListableOption_WebLink(string label, Texture2D image) : base(label, null, null)
		{
			this.minHeight = 24f;
			this.image = image;
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x0018F0B3 File Offset: 0x0018D2B3
		public ListableOption_WebLink(string label, string url, Texture2D image) : this(label, image)
		{
			this.url = url;
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x0018F0C4 File Offset: 0x0018D2C4
		public ListableOption_WebLink(string label, Action action, Texture2D image) : this(label, image)
		{
			this.action = action;
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x0018F0D8 File Offset: 0x0018D2D8
		public override float DrawOption(Vector2 pos, float width)
		{
			float num = width - ListableOption_WebLink.Imagesize.x - 3f;
			float num2 = Text.CalcHeight(this.label, num);
			float num3 = Mathf.Max(this.minHeight, num2);
			Rect rect = new Rect(pos.x, pos.y, width, num3);
			GUI.color = Color.white;
			if (this.image != null)
			{
				Rect position = new Rect(pos.x, pos.y + num3 / 2f - ListableOption_WebLink.Imagesize.y / 2f, ListableOption_WebLink.Imagesize.x, ListableOption_WebLink.Imagesize.y);
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
				GUI.DrawTexture(position, this.image);
			}
			Widgets.Label(new Rect(rect.xMax - num, pos.y, num, num2), this.label);
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.action != null)
				{
					this.action();
				}
				else
				{
					Application.OpenURL(this.url);
				}
			}
			return num3;
		}

		// Token: 0x040037BB RID: 14267
		public Texture2D image;

		// Token: 0x040037BC RID: 14268
		public string url;

		// Token: 0x040037BD RID: 14269
		private static readonly Vector2 Imagesize = new Vector2(24f, 18f);
	}
}
