using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020032B1 RID: 12977
	[StaticConstructorOnStartup]
	public class UI_BackgroundMain : UIMenuBackground
	{
		// Token: 0x17002F18 RID: 12056
		// (get) Token: 0x06012439 RID: 74809 RVA: 0x00561DF7 File Offset: 0x0055FFF7
		private float DeltaAlpha
		{
			get
			{
				return Time.deltaTime * 2f;
			}
		}

		// Token: 0x0601243A RID: 74810 RVA: 0x00561E04 File Offset: 0x00560004
		public void SetupExpansionFadeData()
		{
			this.expansionImageFades = new Dictionary<ExpansionDef, float>();
			foreach (ExpansionDef key in ModLister.AllExpansions)
			{
				this.expansionImageFades.Add(key, 0f);
			}
		}

		// Token: 0x0601243B RID: 74811 RVA: 0x00561E6C File Offset: 0x0056006C
		public override void BackgroundOnGUI()
		{
			Vector2 vector = (this.overrideBGImage != null) ? new Vector2((float)this.overrideBGImage.width, (float)this.overrideBGImage.height) : UI_BackgroundMain.BGPlanetSize;
			bool flag = true;
			if ((float)UI.screenWidth > (float)UI.screenHeight * (vector.x / vector.y))
			{
				flag = false;
			}
			Rect rect;
			if (flag)
			{
				float height = (float)UI.screenHeight;
				float num = (float)UI.screenHeight * (vector.x / vector.y);
				rect = new Rect((float)(UI.screenWidth / 2) - num / 2f, 0f, num, height);
			}
			else
			{
				float width = (float)UI.screenWidth;
				float num2 = (float)UI.screenWidth * (vector.y / vector.x);
				rect = new Rect(0f, (float)(UI.screenHeight / 2) - num2 / 2f, width, num2);
			}
			GUI.DrawTexture(rect, this.overrideBGImage ?? UI_BackgroundMain.BGPlanet, ScaleMode.ScaleToFit);
			if (Event.current.type == EventType.Repaint)
			{
				this.DoOverlay(rect);
			}
		}

		// Token: 0x0601243C RID: 74812 RVA: 0x00561F78 File Offset: 0x00560178
		private void DoOverlay(Rect bgRect)
		{
			if (this.expansionImageFades == null)
			{
				return;
			}
			foreach (ExpansionDef expansionDef in ModLister.AllExpansions)
			{
				if (!expansionDef.isCore && !expansionDef.BackgroundImage.NullOrBad() && this.expansionImageFades[expansionDef] > 0f)
				{
					if (expansionDef.BackgroundImage != this.overrideBGImage)
					{
						GUI.color = new Color(1f, 1f, 1f, this.expansionImageFades[expansionDef]);
						GUI.DrawTexture(bgRect, expansionDef.BackgroundImage, ScaleMode.ScaleAndCrop);
						GUI.color = Color.white;
					}
					this.expansionImageFades[expansionDef] = Mathf.Clamp01(this.expansionImageFades[expansionDef] - this.DeltaAlpha / 2f);
				}
			}
		}

		// Token: 0x0601243D RID: 74813 RVA: 0x0056207C File Offset: 0x0056027C
		public void Notify_Hovered(ExpansionDef expansionDef)
		{
			if (Event.current.type == EventType.Repaint)
			{
				this.expansionImageFades[expansionDef] = Mathf.Clamp01(this.expansionImageFades[expansionDef] + this.DeltaAlpha);
			}
		}

		// Token: 0x0400BE1C RID: 48668
		public Texture2D overrideBGImage;

		// Token: 0x0400BE1D RID: 48669
		private Dictionary<ExpansionDef, float> expansionImageFades;

		// Token: 0x0400BE1E RID: 48670
		private static readonly Vector2 BGPlanetSize = new Vector2(2048f, 1280f);

		// Token: 0x0400BE1F RID: 48671
		private static readonly Texture2D BGPlanet = ContentFinder<Texture2D>.Get("UI/HeroArt/BGPlanet", true);
	}
}
