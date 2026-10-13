using System;
using System.Collections.Generic;
using UnityEngine;
using Waylith.Rendering;

namespace UnityStubs.Gui;

public interface IGuiBackend
{
	bool MouseOutsideHitClip { get; }

	int GroupCount { get; }

	bool InlineControlsObscured { get; }

	Vector2 MeasureText(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth);

	float LineSpacing(int fontIndex, float fontSize);

	void FontMetrics(int fontIndex, out float importSize, out float ascent, out float lineSpacing);

	int RegisterFont(string name, byte[] fontData, float importSize, int renderingMode);

	int TextIndexAt(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth, Vector2 local);

	Vector2 TextCursorPos(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth, int index, out float lineHeight);

	void TextSelectionRects(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth, int from, int to, List<Rect> rects);

	void DrawText(Rect rect, string text, int fontIndex, float fontSize, FontStyle fontStyle, TextAnchor anchor, bool wordWrap, Color color, Rect clipRect = default(Rect));

	void DrawTexture(Rect rect, UnityEngine.Texture image, ScaleMode scaleMode, Color color);

	void DrawTextureWithTexCoords(Rect rect, UnityEngine.Texture image, Rect texCoords, Color color);

	void DrawTextureWithMaterial(Rect rect, UnityEngine.Texture image, Rect texCoords, Color color, UnityEngine.Material material, Waylith.Rendering.Material passMaterial);

	void FillRect(Rect rect, Color color);

	void FillRectRounded(Rect rect, Color color, float rounding);

	void DrawTextureQuad(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, UnityEngine.Texture image, Color color);

	void DrawPrimitives(int mode, ReadOnlySpan<GuiVertex> vertices, UnityEngine.Texture texture, bool screenSpace);

	void SetMatrix(Matrix4x4 matrix);

	void PushGroup(Rect rect);

	void PushScrollView(Rect clipRect, Vector2 contentOffset);

	void PopGroup();

	void GetGroup(int index, out Rect rect, out Vector2 contentOrigin);

	void BeginWindowContent(Rect windowRect);

	void EndWindowContent();

	void PushClipRect(Rect rect);

	void PopClipRect();

	void ActiveRenderTextureChanged(UnityEngine.RenderTexture active);

	void SetTextLanguage(string languageFolderName);

	Vector2 ScreenToGuiPoint(Vector2 screenPoint);

	Vector2 GuiToScreenPoint(Vector2 guiPoint);
}
