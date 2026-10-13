using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BD8 RID: 3032
	public readonly struct TextBlock : IDisposable
	{
		// Token: 0x06004DA0 RID: 19872 RVA: 0x0018AB84 File Offset: 0x00188D84
		public TextBlock(GameFont newFont)
		{
			this = new TextBlock(new GameFont?(newFont), null, null, null);
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x0018ABB8 File Offset: 0x00188DB8
		public TextBlock(TextAnchor newAnchor)
		{
			this = new TextBlock(null, new TextAnchor?(newAnchor), null, null);
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x0018ABEC File Offset: 0x00188DEC
		public TextBlock(bool newWordWrap)
		{
			this = new TextBlock(null, null, new bool?(newWordWrap), null);
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x0018AC20 File Offset: 0x00188E20
		public TextBlock(Color newColor, TextAnchor newAnchor, bool newWordWrap)
		{
			this = new TextBlock(null, new TextAnchor?(newAnchor), new bool?(newWordWrap), new Color?(newColor));
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x0018AC50 File Offset: 0x00188E50
		public TextBlock(Color newColor)
		{
			this = new TextBlock(null, null, null, new Color?(newColor));
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x0018AC84 File Offset: 0x00188E84
		public TextBlock(TextAnchor newAnchor, Color newColor)
		{
			this = new TextBlock(null, new TextAnchor?(newAnchor), null, new Color?(newColor));
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x0018ACB8 File Offset: 0x00188EB8
		public TextBlock(GameFont newFont, TextAnchor newAnchor)
		{
			this = new TextBlock(new GameFont?(newFont), new TextAnchor?(newAnchor), null, null);
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x0018ACEC File Offset: 0x00188EEC
		public TextBlock(GameFont newFont, Color newColor)
		{
			this = new TextBlock(new GameFont?(newFont), null, null, new Color?(newColor));
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x0018AD20 File Offset: 0x00188F20
		public TextBlock(GameFont? newFont, TextAnchor? newAnchor, Color? newColor)
		{
			this = new TextBlock(newFont, newAnchor, null, newColor);
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x0018AD40 File Offset: 0x00188F40
		public TextBlock(GameFont? newFont, TextAnchor? newAnchor, bool? newWordWrap)
		{
			this = new TextBlock(newFont, newAnchor, newWordWrap, null);
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x0018AD60 File Offset: 0x00188F60
		public TextBlock(GameFont? newFont, TextAnchor? newAnchor, bool? newWordWrap, Color? newColor)
		{
			this.oldFont = Text.Font;
			this.oldAnchor = Text.Anchor;
			this.oldWordWrap = Text.WordWrap;
			this.oldColor = GUI.color;
			if (newFont != null)
			{
				Text.Font = newFont.Value;
			}
			if (newAnchor != null)
			{
				Text.Anchor = newAnchor.Value;
			}
			if (newWordWrap != null)
			{
				Text.WordWrap = newWordWrap.Value;
			}
			if (newColor != null)
			{
				GUI.color = newColor.Value;
			}
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x0018ADED File Offset: 0x00188FED
		public static TextBlock Default()
		{
			return new TextBlock(new GameFont?(GameFont.Small), new TextAnchor?(TextAnchor.UpperLeft), new bool?(true), new Color?(Color.white));
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x0018AE10 File Offset: 0x00189010
		public void Dispose()
		{
			Text.Font = this.oldFont;
			Text.Anchor = this.oldAnchor;
			Text.WordWrap = this.oldWordWrap;
			GUI.color = this.oldColor;
		}

		// Token: 0x04003721 RID: 14113
		private readonly GameFont oldFont;

		// Token: 0x04003722 RID: 14114
		private readonly TextAnchor oldAnchor;

		// Token: 0x04003723 RID: 14115
		private readonly bool oldWordWrap;

		// Token: 0x04003724 RID: 14116
		private readonly Color oldColor;
	}
}
