using System;
using LudeonTK;
using UnityEngine;
using Verse;

// Token: 0x02000007 RID: 7
public class DevWindowDrawing : IWindowDrawing
{
	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000016 RID: 22 RVA: 0x00002416 File Offset: 0x00000616
	public GUIStyle EmptyStyle
	{
		get
		{
			return DevGUI.EmptyStyle;
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002420 File Offset: 0x00000620
	public bool DoCloseButton(Rect rect, string text)
	{
		return DevGUI.ButtonText(rect, text, null);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002440 File Offset: 0x00000640
	public bool DoClostButtonSmall(Rect rect)
	{
		return DevGUI.ButtonImage(new Rect(rect.x + rect.width - 22f - 4f, rect.y + 4f, 22f, 22f), DevGUI.Close);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000248E File Offset: 0x0000068E
	public void DoGrayOut(Rect rect)
	{
		DevGUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.5f));
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000024AF File Offset: 0x000006AF
	public void DoWindowBackground(Rect rect)
	{
		GUI.color = DevGUI.WindowBGFillColor;
		GUI.DrawTexture(rect, BaseContent.WhiteTex);
		GUI.color = Color.white;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000024D0 File Offset: 0x000006D0
	public void BeginGroup(Rect rect)
	{
		DevGUI.BeginGroup(rect);
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000024D8 File Offset: 0x000006D8
	public void EndGroup()
	{
		DevGUI.EndGroup();
	}
}
