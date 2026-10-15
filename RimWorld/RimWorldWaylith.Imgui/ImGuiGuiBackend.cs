using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Brutal.ImGuiApi;
using Brutal.Numerics;
using Brutal.VulkanApi;
using RimWorldWaylith.Rendering;
using UnityEngine;
using UnityStubs.Gui;
using Waylith.Logging;
using Waylith.Rendering;

namespace RimWorldWaylith.Imgui;

internal sealed class ImGuiGuiBackend : IGuiBackend
{
	private readonly ImGuiHost host;

	private readonly List<ImGuiMaterialDraw> materialDraws = new List<ImGuiMaterialDraw>();

	private ImDrawListPtr drawList;

	private bool frameActive;

	private bool drawingEnabled;

	private float m00 = 1f;

	private float m01;

	private float m10;

	private float m11 = 1f;

	private bool rotated;

	private float scale = 1f;

	private float2 translate = new float2(0f, 0f);

	private float2 offset = new float2(0f, 0f);

	private readonly Stack<float2> offsetStack = new Stack<float2>();

	private int clipDepth;

	private readonly Stack<float4> hitClipStack = new Stack<float4>();

	private readonly List<float4> windowRectsAccum = new List<float4>();

	private float4[] obscuringWindowRects = Array.Empty<float4>();

	private int windowContentDepth;

	private bool physMouseOverWindow;

	private float2 physMouse;

	private readonly CameraTextureBaker? textureBaker;

	private readonly ImGuiDrawSnapshot drawTargetSnapshot = new ImGuiDrawSnapshot();

	private readonly Action<CommandBuffer, VkRenderPass> recordDrawTarget;

	private UnityEngine.RenderTexture? drawTarget;

	private ImDrawListPtr screenDrawList;

	private int screenClipDepth;

	private int drawTargetBakedIndices;

	private readonly List<(Rect rect, Vector2 origin)> openGroups = new List<(Rect, Vector2)>();

	private const float MaxGuiCoordinate = 10000000f;

	private readonly TextLayout textLayout;

	private const uint GlyphColoredBit = 1u;

	private const uint GlyphVisibleBit = 2u;

	private static readonly ConditionalWeakTable<UnityEngine.Texture, object> reportedUnresolved = new ConditionalWeakTable<UnityEngine.Texture, object>();

	internal IReadOnlyList<ImGuiMaterialDraw> MaterialDraws => materialDraws;

	public int GroupCount => openGroups.Count;

	public bool InlineControlsObscured
	{
		get
		{
			if (windowContentDepth == 0)
			{
				return physMouseOverWindow;
			}
			return false;
		}
	}

	public bool MouseOutsideHitClip { get; private set; }

	public ImGuiGuiBackend(ImGuiHost host, CameraTextureBaker? textureBaker)
	{
		ImGuiGuiBackend imGuiGuiBackend = this;
		this.host = host;
		this.textureBaker = textureBaker;
		textLayout = new TextLayout(host);
		recordDrawTarget = delegate(CommandBuffer commandBuffer, VkRenderPass renderPass)
		{
			host.Renderer.RecordOffscreen(imGuiGuiBackend.drawTargetSnapshot, commandBuffer, renderPass);
		};
		if (textureBaker != null)
		{
			textureBaker.BeforeReadPixels += BakeDrawTarget;
		}
		host.SetGuiBackend(this);
	}

	public void BeginFrame()
	{
		drawList = ImGui.GetBackgroundDrawList();
		materialDraws.Clear();
		frameActive = true;
		drawingEnabled = false;
		ResetTransform();
	}

	public void BeginInputPhase()
	{
		frameActive = true;
		drawingEnabled = false;
		ResetTransform();
	}

	public void EndInputPhase()
	{
		frameActive = false;
	}

	public void BeginPass(bool drawing, Vector2 physicalMouseTopDown)
	{
		drawingEnabled = drawing;
		physMouse = new float2(physicalMouseTopDown.x, physicalMouseTopDown.y);
		obscuringWindowRects = ((windowRectsAccum.Count > 0) ? windowRectsAccum.ToArray() : Array.Empty<float4>());
		windowRectsAccum.Clear();
		windowContentDepth = 0;
		physMouseOverWindow = ComputePhysMouseOverWindow();
		ResetTransform();
		UpdateEventMouse();
	}

	public void EndPass()
	{
		if (frameActive)
		{
			EndDrawTarget();
			BalanceGroups();
		}
	}

	public void BalanceGroups()
	{
		if (frameActive)
		{
			while (clipDepth > 0)
			{
				drawList.PopClipRect();
				clipDepth--;
			}
			offsetStack.Clear();
			offset = new float2(0f, 0f);
			hitClipStack.Clear();
			openGroups.Clear();
		}
	}

	public void EndFrame()
	{
		EndDrawTarget();
		while (clipDepth > 0)
		{
			drawList.PopClipRect();
			clipDepth--;
		}
		frameActive = false;
		drawingEnabled = false;
	}

	private void ResetTransform()
	{
		m00 = 1f;
		m01 = 0f;
		m10 = 0f;
		m11 = 1f;
		rotated = false;
		scale = 1f;
		translate = new float2(0f, 0f);
		offset = new float2(0f, 0f);
		offsetStack.Clear();
		hitClipStack.Clear();
		openGroups.Clear();
		clipDepth = 0;
	}

	public void SetTextLanguage(string languageFolderName)
	{
		host.SetTextLanguage(languageFolderName);
	}

	public void ActiveRenderTextureChanged(UnityEngine.RenderTexture active)
	{
		EndDrawTarget();
		if (active != null && frameActive && drawingEnabled && textureBaker != null)
		{
			BeginDrawTarget(active);
		}
	}

	private void BeginDrawTarget(UnityEngine.RenderTexture target)
	{
		screenDrawList = drawList;
		screenClipDepth = clipDepth;
		drawList = host.RenderTextureDrawList.Reset(drawList.GetClipRectMin(), drawList.GetClipRectMax());
		clipDepth = 0;
		drawTarget = target;
		drawTargetBakedIndices = 0;
	}

	private void EndDrawTarget()
	{
		if (!(drawTarget == null))
		{
			BakeDrawTarget(drawTarget);
			drawList = screenDrawList;
			clipDepth = screenClipDepth;
			drawTarget = null;
		}
	}

	private void BakeDrawTarget(UnityEngine.RenderTexture texture)
	{
		if ((object)texture == drawTarget && drawList.IdxBuffer.Count > drawTargetBakedIndices)
		{
			float2 displaySize = ImGui.GetIO().DisplaySize;
			if (displaySize.X > 0f && displaySize.Y > 0f)
			{
				host.Renderer.ProcessPendingTextureUpdates();
				float2 framebufferScale = new float2((float)texture.width / displaySize.X, (float)texture.height / displaySize.Y);
				drawTargetSnapshot.CopyFrom(drawList, drawTargetBakedIndices, displaySize, framebufferScale, materialDraws);
				textureBaker.RecordIntoTarget(texture, recordDrawTarget);
			}
			drawTargetBakedIndices = drawList.IdxBuffer.Count;
		}
	}

	public void SetMatrix(Matrix4x4 matrix)
	{
		m00 = matrix.m00;
		m01 = matrix.m01;
		m10 = matrix.m10;
		m11 = matrix.m11;
		rotated = m01 != 0f || m10 != 0f || m00 < 0f || m11 < 0f;
		scale = MathF.Sqrt(m00 * m00 + m10 * m10);
		if (scale == 0f)
		{
			m00 = 1f;
			m01 = 0f;
			m10 = 0f;
			m11 = 1f;
			rotated = false;
			scale = 1f;
		}
		translate = new float2(matrix.m03, matrix.m13);
		UpdateEventMouse();
	}

	public void PushGroup(Rect rect)
	{
		PushClip(rect, new float2(rect.x, rect.y), tightenHitClip: false);
	}

	public void PushScrollView(Rect clipRect, Vector2 contentOffset)
	{
		PushClip(clipRect, new float2(contentOffset.x, contentOffset.y), tightenHitClip: true);
	}

	private void PushClip(Rect clipRect, float2 contentOffsetDelta, bool tightenHitClip)
	{
		if (frameActive)
		{
			ScreenBounds(clipRect, out var min, out var max);
			if (!rotated)
			{
				min = new float2(MathF.Floor(min.X + 0.51f), MathF.Floor(min.Y + 0.51f));
				max = new float2(MathF.Floor(max.X + 0.51f), MathF.Floor(max.Y + 0.51f));
			}
			if (drawingEnabled)
			{
				drawList.PushClipRect(in min, in max, intersectWithCurrentClipRect: true);
				clipDepth++;
			}
			float4 @float = ((hitClipStack.Count > 0) ? hitClipStack.Peek() : new float4(-1E+09f, -1E+09f, 1E+09f, 1E+09f));
			float4 item = @float;
			if (tightenHitClip)
			{
				item = new float4(Math.Max(min.X, @float.X), Math.Max(min.Y, @float.Y), Math.Min(max.X, @float.Z), Math.Min(max.Y, @float.W));
			}
			hitClipStack.Push(item);
			offsetStack.Push(offset);
			offset = new float2(offset.X + contentOffsetDelta.X, offset.Y + contentOffsetDelta.Y);
			if (!rotated)
			{
				float2 float2 = ToScreen(0f, 0f);
				float2 float3 = ToGui(MathF.Floor(float2.X + 0.5f), MathF.Floor(float2.Y + 0.5f));
				offset = new float2(offset.X + float3.X, offset.Y + float3.Y);
			}
			openGroups.Add((clipRect, new Vector2(offset.X, offset.Y)));
			UpdateEventMouse();
		}
	}

	public void GetGroup(int index, out Rect rect, out Vector2 contentOrigin)
	{
		(rect, contentOrigin) = openGroups[index];
	}

	public void PopGroup()
	{
		if (frameActive)
		{
			if (offsetStack.Count > 0)
			{
				offset = offsetStack.Pop();
			}
			if (hitClipStack.Count > 0)
			{
				hitClipStack.Pop();
			}
			if (openGroups.Count > 0)
			{
				openGroups.RemoveAt(openGroups.Count - 1);
			}
			UpdateEventMouse();
			if (drawingEnabled && clipDepth > 0)
			{
				drawList.PopClipRect();
				clipDepth--;
			}
		}
	}

	public void BeginWindowContent(Rect windowRect)
	{
		if (frameActive)
		{
			ScreenBounds(windowRect, out var min, out var max);
			windowRectsAccum.Add(new float4(min.X, min.Y, max.X, max.Y));
			windowContentDepth++;
		}
	}

	public void EndWindowContent()
	{
		if (frameActive && windowContentDepth > 0)
		{
			windowContentDepth--;
		}
	}

	private bool ComputePhysMouseOverWindow()
	{
		float4[] array = obscuringWindowRects;
		for (int i = 0; i < array.Length; i++)
		{
			float4 @float = array[i];
			if (physMouse.X >= @float.X && physMouse.Y >= @float.Y && physMouse.X <= @float.Z && physMouse.Y <= @float.W)
			{
				return true;
			}
		}
		return false;
	}

	public void PushClipRect(Rect rect)
	{
		if (frameActive && drawingEnabled)
		{
			ScreenBounds(rect, out var min, out var max);
			drawList.PushClipRect(in min, in max, intersectWithCurrentClipRect: true);
		}
	}

	public void PopClipRect()
	{
		if (frameActive && drawingEnabled)
		{
			drawList.PopClipRect();
		}
	}

	private float2 ToScreen(float x, float y)
	{
		float num = offset.X + x;
		float num2 = offset.Y + y;
		return new float2(m00 * num + m01 * num2 + translate.X, m10 * num + m11 * num2 + translate.Y);
	}

	private float2 SnapLineOrigin(float x, float y)
	{
		if (rotated)
		{
			float2 @float = ToScreen(x, y);
			return new float2(MathF.Floor(@float.X + 0.5f), MathF.Floor(@float.Y + 0.5f));
		}
		float2 float2 = ToScreen(0f, 0f);
		return new float2(MathF.Floor(float2.X + 0.5f) + MathF.Floor(m00 * x + 0.5f), MathF.Floor(float2.Y + 0.5f) + MathF.Floor(m11 * y + 0.5f));
	}

	private float2 ToGui(float sx, float sy)
	{
		float num = m00 * m11 - m01 * m10;
		float num2 = sx - translate.X;
		float num3 = sy - translate.Y;
		return new float2((m11 * num2 - m01 * num3) / num - offset.X, (m00 * num3 - m10 * num2) / num - offset.Y);
	}

	private static float Finite(float v)
	{
		if (!float.IsNaN(v))
		{
			return Math.Clamp(v, -10000000f, 10000000f);
		}
		return 0f;
	}

	private void ScreenBounds(Rect rect, out float2 min, out float2 max)
	{
		float x = Finite(rect.x);
		float y = Finite(rect.y);
		float x2 = Finite(rect.x + rect.width);
		float y2 = Finite(rect.y + rect.height);
		if (!rotated)
		{
			min = ToScreen(x, y);
			max = ToScreen(x2, y2);
			return;
		}
		float2 @float = ToScreen(x, y);
		float2 float2 = ToScreen(x2, y);
		float2 float3 = ToScreen(x2, y2);
		float2 float4 = ToScreen(x, y2);
		min = new float2(MathF.Min(MathF.Min(@float.X, float2.X), MathF.Min(float3.X, float4.X)), MathF.Min(MathF.Min(@float.Y, float2.Y), MathF.Min(float3.Y, float4.Y)));
		max = new float2(MathF.Max(MathF.Max(@float.X, float2.X), MathF.Max(float3.X, float4.X)), MathF.Max(MathF.Max(@float.Y, float2.Y), MathF.Max(float3.Y, float4.Y)));
	}

	private void AddImageRect(in ImTextureRef texRef, Rect dst, float2 uvMin, float2 uvMax, ImColor8 col)
	{
		if (!rotated)
		{
			if (dst.width < 0f)
			{
				dst.x += dst.width;
				dst.width = 0f - dst.width;
				ref float x = ref uvMin.X;
				ref float x2 = ref uvMax.X;
				float x3 = uvMax.X;
				float x4 = uvMin.X;
				x = x3;
				x2 = x4;
			}
			if (dst.height < 0f)
			{
				dst.y += dst.height;
				dst.height = 0f - dst.height;
				ref float x = ref uvMin.Y;
				ref float y = ref uvMax.Y;
				float x4 = uvMax.Y;
				float x3 = uvMin.Y;
				x = x4;
				y = x3;
			}
			float2 @float = ToScreen(dst.x, dst.y);
			float2 float2 = ToScreen(dst.x + dst.width, dst.y + dst.height) - @float;
			@float = new float2(MathF.Floor(@float.X + 0.51f), MathF.Floor(@float.Y + 0.51f));
			float2 pMax = @float + new float2(MathF.Max(1f, MathF.Floor(float2.X + 0.5f)), MathF.Max(1f, MathF.Floor(float2.Y + 0.5f)));
			drawList.AddImage(texRef, in @float, in pMax, new float2?(uvMin), new float2?(uvMax), col);
		}
		else
		{
			float2 a = ToScreen(dst.x, dst.y);
			float2 b = ToScreen(dst.x + dst.width, dst.y);
			float2 c = ToScreen(dst.x + dst.width, dst.y + dst.height);
			float2 d = ToScreen(dst.x, dst.y + dst.height);
			drawList.PushTexture(texRef);
			drawList.PrimReserve(6, 4);
			drawList.PrimQuadUV(in a, in b, in c, in d, in uvMin, new float2(uvMax.X, uvMin.Y), in uvMax, new float2(uvMin.X, uvMax.Y), col);
			drawList.PopTexture();
		}
	}

	public Vector2 ScreenToGuiPoint(Vector2 screenPoint)
	{
		float num = m00 * m11 - m01 * m10;
		float num2 = screenPoint.x - offset.X - translate.X;
		float num3 = screenPoint.y - offset.Y - translate.Y;
		return new Vector2((m11 * num2 - m01 * num3) / num, (m00 * num3 - m10 * num2) / num);
	}

	public Vector2 GuiToScreenPoint(Vector2 guiPoint)
	{
		return new Vector2(offset.X + m00 * guiPoint.x + m01 * guiPoint.y + translate.X, offset.Y + m10 * guiPoint.x + m11 * guiPoint.y + translate.Y);
	}

	private void UpdateEventMouse()
	{
		Event current = Event.current;
		if (current != null)
		{
			MouseOutsideHitClip = false;
			if (hitClipStack.Count > 0)
			{
				float4 @float = hitClipStack.Peek();
				MouseOutsideHitClip = physMouse.X < @float.X || physMouse.Y < @float.Y || physMouse.X > @float.Z || physMouse.Y > @float.W;
			}
			float2 float2 = ToGui(physMouse.X, physMouse.Y);
			current.mousePosition = new Vector2(float2.X, float2.Y);
		}
	}

	public void FillRect(Rect rect, UnityEngine.Color color)
	{
		if (frameActive && drawingEnabled && !(color.a <= 0f) && !ZeroArea(rect))
		{
			if (rotated)
			{
				drawList.AddQuadFilled(ToScreen(rect.x, rect.y), ToScreen(rect.x + rect.width, rect.y), ToScreen(rect.x + rect.width, rect.y + rect.height), ToScreen(rect.x, rect.y + rect.height), ToCol(in color));
				return;
			}
			float2 pMin = ToScreen(rect.x, rect.y);
			float2 pMax = ToScreen(rect.x + rect.width, rect.y + rect.height);
			drawList.AddRectFilled(in pMin, in pMax, ToCol(in color));
		}
	}

	public void DrawTextureQuad(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, UnityEngine.Texture? image, UnityEngine.Color color)
	{
		if (frameActive && drawingEnabled && !(color.a <= 0f) && (p2.x - p0.x) * (p3.y - p1.y) - (p2.y - p0.y) * (p3.x - p1.x) != 0f)
		{
			float2 a = ToScreen(p0.x, p0.y);
			float2 b = ToScreen(p1.x, p1.y);
			float2 c = ToScreen(p2.x, p2.y);
			float2 d = ToScreen(p3.x, p3.y);
			ImColor8 col = ToCol(in color);
			if (!TryResolveTexture(image, out var texRef))
			{
				drawList.AddQuadFilled(in a, in b, in c, in d, col);
				return;
			}
			drawList.PushTexture(texRef);
			drawList.PrimReserve(6, 4);
			drawList.PrimQuadUV(in a, in b, in c, in d, new float2(0f, 0f), new float2(1f, 0f), new float2(1f, 1f), new float2(0f, 1f), col);
			drawList.PopTexture();
		}
	}

	public void DrawPrimitives(int mode, ReadOnlySpan<GuiVertex> vertices, UnityEngine.Texture? texture, bool screenSpace)
	{
		if (!frameActive || !drawingEnabled)
		{
			return;
		}
		if (mode == 1 || mode == 2)
		{
			int num = ((mode != 1) ? 1 : 2);
			for (int i = 0; i + 1 < vertices.Length; i += num)
			{
				ref readonly GuiVertex v = ref vertices[i];
				ref readonly GuiVertex reference = ref vertices[i + 1];
				drawList.AddLine(PrimPosition(in v, screenSpace), PrimPosition(in reference, screenSpace), new ImColor8(reference.color.r, reference.color.g, reference.color.b, reference.color.a));
			}
			return;
		}
		bool flag = texture != null && (object)texture == host.TmpFontBackend?.GetAtlasTexture(0);
		ImTextureRef texRef = default(ImTextureRef);
		bool flag2 = flag || (texture != null && TryResolveTexture(texture, out texRef));
		float2 fontTexUvWhitePixel = ImGui.GetFontTexUvWhitePixel();
		if (flag2 && !flag)
		{
			drawList.PushTexture(texRef);
		}
		switch (mode)
		{
		case 7:
		{
			for (int k = 0; k + 3 < vertices.Length; k += 4)
			{
				drawList.PrimReserve(6, 6);
				PrimVertex(in vertices[k], flag2, fontTexUvWhitePixel, screenSpace);
				PrimVertex(in vertices[k + 1], flag2, fontTexUvWhitePixel, screenSpace);
				PrimVertex(in vertices[k + 2], flag2, fontTexUvWhitePixel, screenSpace);
				PrimVertex(in vertices[k], flag2, fontTexUvWhitePixel, screenSpace);
				PrimVertex(in vertices[k + 2], flag2, fontTexUvWhitePixel, screenSpace);
				PrimVertex(in vertices[k + 3], flag2, fontTexUvWhitePixel, screenSpace);
			}
			break;
		}
		case 4:
		{
			for (int l = 0; l + 2 < vertices.Length; l += 3)
			{
				drawList.PrimReserve(3, 3);
				PrimVertex(in vertices[l], flag2, fontTexUvWhitePixel, screenSpace);
				PrimVertex(in vertices[l + 1], flag2, fontTexUvWhitePixel, screenSpace);
				PrimVertex(in vertices[l + 2], flag2, fontTexUvWhitePixel, screenSpace);
			}
			break;
		}
		case 5:
		{
			for (int j = 0; j + 2 < vertices.Length; j++)
			{
				drawList.PrimReserve(3, 3);
				PrimVertex(in vertices[j], flag2, fontTexUvWhitePixel, screenSpace);
				PrimVertex(in vertices[j + 1], flag2, fontTexUvWhitePixel, screenSpace);
				PrimVertex(in vertices[j + 2], flag2, fontTexUvWhitePixel, screenSpace);
			}
			break;
		}
		}
		if (flag2 && !flag)
		{
			drawList.PopTexture();
		}
	}

	private float2 PrimPosition(in GuiVertex v, bool screenSpace)
	{
		if (!screenSpace)
		{
			return ToScreen(v.position.x, v.position.y);
		}
		return new float2(v.position.x, v.position.y);
	}

	private void PrimVertex(in GuiVertex v, bool textured, float2 white, bool screenSpace)
	{
		float2 uv = (textured ? new float2(v.uv.x, 1f - v.uv.y) : white);
		drawList.PrimVtx(PrimPosition(in v, screenSpace), in uv, new ImColor8(v.color.r, v.color.g, v.color.b, v.color.a));
	}

	public void FillRectRounded(Rect rect, UnityEngine.Color color, float rounding)
	{
		if (frameActive && drawingEnabled && !(color.a <= 0f) && !ZeroArea(rect))
		{
			ScreenBounds(rect, out var min, out var max);
			drawList.AddRectFilled(in min, in max, ToCol(in color), rounding * scale);
		}
	}

	public Vector2 MeasureText(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth)
	{
		int fontIndex2 = Math.Clamp(fontIndex, 0, host.Fonts.Count - 1);
		textLayout.Layout(fontIndex2, fontSize, fontStyle, text, wrapWidth, UnityEngine.Color.white);
		return new Vector2(textLayout.Width, textLayout.Height);
	}

	public float LineSpacing(int fontIndex, float fontSize)
	{
		return host.LineMetricsFor(Math.Clamp(fontIndex, 0, host.Fonts.Count - 1), fontSize).Spacing;
	}

	public void FontMetrics(int fontIndex, out float importSize, out float ascent, out float lineSpacing)
	{
		int fontIndex2 = Math.Clamp(fontIndex, 0, host.Fonts.Count - 1);
		LineMetrics lineMetrics = host.LineMetricsFor(fontIndex2, 0f);
		importSize = host.ImportSize(fontIndex2);
		ascent = lineMetrics.Ascent;
		lineSpacing = lineMetrics.Spacing;
	}

	public int RegisterFont(string name, byte[] fontData, float importSize, int renderingMode)
	{
		return host.RegisterFont(name, fontData, importSize, renderingMode);
	}

	public int TextIndexAt(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth, Vector2 local)
	{
		if (string.IsNullOrEmpty(text))
		{
			return 0;
		}
		int fontIndex2 = Math.Clamp(fontIndex, 0, host.Fonts.Count - 1);
		textLayout.Layout(fontIndex2, fontSize, fontStyle, text, wrapWidth, UnityEngine.Color.white);
		return textLayout.IndexAt(local);
	}

	public Vector2 TextCursorPos(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth, int index, out float lineHeight)
	{
		int fontIndex2 = Math.Clamp(fontIndex, 0, host.Fonts.Count - 1);
		lineHeight = host.LineMetricsFor(fontIndex2, fontSize).Pitch;
		if (string.IsNullOrEmpty(text))
		{
			return Vector2.zero;
		}
		textLayout.Layout(fontIndex2, fontSize, fontStyle, text, wrapWidth, UnityEngine.Color.white);
		lineHeight = textLayout.Lines[textLayout.LineOf(index)].Pitch;
		return textLayout.PositionOf(index);
	}

	public void TextSelectionRects(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth, int from, int to, List<Rect> rects)
	{
		rects.Clear();
		if (!string.IsNullOrEmpty(text) && to > from)
		{
			int fontIndex2 = Math.Clamp(fontIndex, 0, host.Fonts.Count - 1);
			textLayout.Layout(fontIndex2, fontSize, fontStyle, text, wrapWidth, UnityEngine.Color.white);
			Vector2 vector = textLayout.PositionOf(from);
			Vector2 vector2 = textLayout.PositionOf(to);
			int num = textLayout.LineOf(from);
			int num2 = textLayout.LineOf(to);
			for (int i = num; i <= num2 && i < textLayout.Lines.Count; i++)
			{
				TextLayout.Line line = textLayout.Lines[i];
				float num3 = ((i == num) ? vector.x : 0f);
				float num4 = ((i == num2) ? vector2.x : MathF.Max(line.Width, 4f));
				rects.Add(new Rect(num3, line.Top, MathF.Max(0f, num4 - num3), line.Pitch));
			}
		}
	}

	public void DrawText(Rect rect, string text, int fontIndex, float fontSize, FontStyle fontStyle, TextAnchor anchor, bool wordWrap, UnityEngine.Color color, Rect clipRect)
	{
		if (!frameActive || !drawingEnabled || string.IsNullOrEmpty(text))
		{
			return;
		}
		int fontIndex2 = Math.Clamp(fontIndex, 0, host.Fonts.Count - 1);
		FontStyle fontStyle2 = FontStyle.Normal;
		float num = 0f;
		ImFontBakedPtr baked = default(ImFontBakedPtr);
		textLayout.Layout(fontIndex2, fontSize, fontStyle, text, wordWrap ? rect.width : (-1f), color);
		float num2 = ((int)anchor / 3) switch
		{
			1 => rect.y + (rect.height - textLayout.Height) * 0.5f, 
			2 => rect.y + rect.height - textLayout.Height, 
			_ => rect.y, 
		};
		bool num3 = clipRect.width > 0f;
		bool flag = num3 && !rotated;
		bool flag2 = num3 && rotated && (textLayout.Height > clipRect.height + 0.5f || textLayout.Width > clipRect.width + 0.5f);
		float2 clipMin = default(float2);
		float2 @float = default(float2);
		if (flag)
		{
			clipMin = ToScreen(clipRect.x, clipRect.y);
			@float = ToScreen(clipRect.xMax, clipRect.yMax);
		}
		if (flag2)
		{
			PushClipRect(clipRect);
		}
		IReadOnlyList<TextLayout.Piece> pieces = textLayout.Pieces;
		IReadOnlyList<TextLayout.Line> lines = textLayout.Lines;
		for (int i = 0; i < lines.Count; i++)
		{
			TextLayout.Line line = lines[i];
			float num4 = ((int)anchor % 3) switch
			{
				1 => rect.x + (rect.width - line.Width) * 0.5f, 
				2 => rect.x + rect.width - line.Width, 
				_ => rect.x, 
			};
			float num5 = num2 + line.Top;
			float2 clipMax = @float;
			if (flag && num5 + line.FirstLine > clipRect.yMax + 0.5f)
			{
				if (i > 0)
				{
					continue;
				}
			}
			else
			{
				clipMax.Y = float.MaxValue;
			}
			float2 float2 = SnapLineOrigin(num4, num5);
			float num6 = MathF.Floor(line.Ascent * scale + 0.5f);
			int num7 = line.PieceStart + line.PieceCount;
			for (int j = line.PieceStart; j < num7; j++)
			{
				TextLayout.Piece piece = pieces[j];
				if (!piece.IsSpace)
				{
					ImColor8 col = ToCol(in piece.Color);
					float num8 = num4 + piece.X;
					int k = piece.Start;
					int num9 = piece.Start + piece.Length;
					if (baked.IsNull() || piece.Style != fontStyle2 || piece.Size != num)
					{
						fontStyle2 = piece.Style;
						num = piece.Size;
						baked = host.StyledFont(fontIndex2, piece.Style).GetFontBaked(piece.Size, scale);
					}
					float y = num5 + line.Ascent - baked.Ascent;
					float y2 = float2.Y + num6 - baked.Ascent * scale;
					int num10;
					for (; k < num9; k += num10)
					{
						num10 = textLayout.CodePointAt(in piece, k, out var width);
						uint codePoint = (uint)((num10 == 2) ? char.ConvertToUtf32(text[k], text[k + 1]) : text[k]);
						DrawGlyph(baked, codePoint, num8, y, new float2(float2.X + (num8 - num4) * scale, y2), col, flag, clipMin, clipMax);
						num8 += width;
					}
				}
			}
		}
		if (flag2)
		{
			PopClipRect();
		}
	}

	private void DrawGlyph(ImFontBakedPtr baked, uint codePoint, float x, float y, float2 pen, ImColor8 col, bool fineClip, float2 clipMin, float2 clipMax)
	{
		ImFontGlyphPtr imFontGlyphPtr = baked.FindGlyph(codePoint);
		if (imFontGlyphPtr.IsNull() || (imFontGlyphPtr.Colored & 2) == 0)
		{
			return;
		}
		if ((imFontGlyphPtr.Colored & 1) != 0)
		{
			col = col.AsUint() | 0xFFFFFF;
		}
		float2 uvA = new float2(imFontGlyphPtr.U0, imFontGlyphPtr.V0);
		float2 uvB = new float2(imFontGlyphPtr.U1, imFontGlyphPtr.V1);
		if (rotated)
		{
			drawList.PrimReserve(6, 4);
			drawList.PrimQuadUV(ToScreen(x + imFontGlyphPtr.X0, y + imFontGlyphPtr.Y0), ToScreen(x + imFontGlyphPtr.X1, y + imFontGlyphPtr.Y0), ToScreen(x + imFontGlyphPtr.X1, y + imFontGlyphPtr.Y1), ToScreen(x + imFontGlyphPtr.X0, y + imFontGlyphPtr.Y1), in uvA, new float2(uvB.X, uvA.Y), in uvB, new float2(uvA.X, uvB.Y), col);
			return;
		}
		float2 @float = new float2(pen.X + imFontGlyphPtr.X0 * scale, pen.Y + imFontGlyphPtr.Y0 * scale);
		@float = new float2(MathF.Floor(@float.X + 0.5f), MathF.Floor(@float.Y + 0.5f));
		float2 b = new float2(@float.X + (imFontGlyphPtr.X1 - imFontGlyphPtr.X0) * scale, @float.Y + (imFontGlyphPtr.Y1 - imFontGlyphPtr.Y0) * scale);
		if (fineClip)
		{
			if (@float.X >= clipMax.X || b.X <= clipMin.X)
			{
				return;
			}
			float num = (uvB.X - uvA.X) / (b.X - @float.X);
			if (@float.X < clipMin.X)
			{
				uvA.X += (clipMin.X - @float.X) * num;
				@float.X = clipMin.X;
			}
			if (b.X > clipMax.X)
			{
				uvB.X -= (b.X - clipMax.X) * num;
				b.X = clipMax.X;
			}
			if (@float.Y >= clipMax.Y)
			{
				return;
			}
			if (b.Y > clipMax.Y)
			{
				uvB.Y -= (b.Y - clipMax.Y) * (uvB.Y - uvA.Y) / (b.Y - @float.Y);
				b.Y = clipMax.Y;
			}
		}
		drawList.PrimReserve(6, 4);
		drawList.PrimRectUV(in @float, in b, in uvA, in uvB, col);
	}

	public void DrawTexture(Rect rect, UnityEngine.Texture image, ScaleMode scaleMode, UnityEngine.Color color)
	{
		if (!frameActive || !drawingEnabled || DrawsNothing(rect))
		{
			return;
		}
		if (!TryResolveTexture(image, out var texRef))
		{
			DrawPlaceholderPanel(rect, color);
			return;
		}
		FitImage(rect, image, scaleMode, out var dst, out var uvMin, out var uvMax);
		if (!rotated && image.width <= 2 && image.height <= 2 && scale != MathF.Round(scale) && IsUniform(image))
		{
			drawList.PushTexture(texRef);
			drawList.AddQuadFilled(ToScreen(dst.x, dst.y), ToScreen(dst.x + dst.width, dst.y), ToScreen(dst.x + dst.width, dst.y + dst.height), ToScreen(dst.x, dst.y + dst.height), ToCol(in color));
			drawList.PopTexture();
		}
		else
		{
			AddImageRect(in texRef, dst, uvMin, uvMax, ToCol(in color));
		}
	}

	private static bool IsUniform(UnityEngine.Texture image)
	{
		if (image.width == 1 && image.height == 1)
		{
			return true;
		}
		TexturePixelData texturePixelData = (image.CoreTexture as Waylith.Rendering.Texture2D)?.CpuData;
		if (texturePixelData != null && texturePixelData.Format == LoadedTextureFormat.Rgba32)
		{
			byte[][] levels = texturePixelData.Levels;
			if (levels != null && levels.Length > 0)
			{
				byte[] array = texturePixelData.Levels[0];
				if (array != null)
				{
					for (int i = 4; i < array.Length; i++)
					{
						if (array[i] != array[i & 3])
						{
							return false;
						}
					}
					return true;
				}
			}
		}
		return false;
	}

	public void DrawTextureWithTexCoords(Rect rect, UnityEngine.Texture? image, Rect texCoords, UnityEngine.Color color)
	{
		if (frameActive && drawingEnabled && !DrawsNothing(rect))
		{
			if (!TryResolveTexture(image, out var texRef))
			{
				DrawPlaceholderPanel(rect, color);
				return;
			}
			float2 uvMin = new float2(texCoords.x, 1f - (texCoords.y + texCoords.height));
			float2 uvMax = new float2(texCoords.x + texCoords.width, 1f - texCoords.y);
			AddImageRect(in texRef, rect, uvMin, uvMax, ToCol(in color));
		}
	}

	public void DrawTextureWithMaterial(Rect rect, UnityEngine.Texture? image, Rect texCoords, UnityEngine.Color color, UnityEngine.Material material, Waylith.Rendering.Material? passMaterial)
	{
		if (!frameActive || !drawingEnabled || image == null || material == null || passMaterial == null || DrawsNothing(rect))
		{
			return;
		}
		bool flag = passMaterial.Shader?.Program != null;
		if (flag)
		{
			EnsureMaterialTexturesResident(image, material);
		}
		if (!TryResolveTexture(image, out var texRef))
		{
			return;
		}
		int count = materialDraws.Count;
		MaterialUniformBytes uniforms = default(MaterialUniformBytes);
		MaterialTextureResourceSet<Waylith.Rendering.Texture> resources = default(MaterialTextureResourceSet<Waylith.Rendering.Texture>);
		MaterialStorageArray[] storageArrays = Array.Empty<MaterialStorageArray>();
		if (flag)
		{
			MaterialBindingResolver.ResolveMaterialBindingData(passMaterial, out uniforms, out resources, out bool _);
			storageArrays = passMaterial.StorageArrays;
			MaterialUniformSlot[] uniformSlots = passMaterial.Shader.MaterialLayout.UniformSlots;
			for (int i = 0; i < uniformSlots.Length; i++)
			{
				MaterialUniformSlot materialUniformSlot = uniformSlots[i];
				if (materialUniformSlot.Type == MaterialUniformType.TexelSize && materialUniformSlot.PropertyName == "_MainTex_TexelSize")
				{
					uniforms.Write(materialUniformSlot.Offset, MaterialBindingResolver.TexelSize(image.width, image.height));
				}
			}
		}
		materialDraws.Add(new ImGuiMaterialDraw(passMaterial, image.CoreTexture, texRef._TexID.Value, uniforms, resources, storageArrays));
		float2 uvMin = (flag ? new float2(texCoords.x, texCoords.y + texCoords.height) : new float2(texCoords.x, 1f - (texCoords.y + texCoords.height)));
		float2 uvMax = (flag ? new float2(texCoords.x + texCoords.width, texCoords.y) : new float2(texCoords.x + texCoords.width, 1f - texCoords.y));
		AddImageRect(new ImTextureRef
		{
			_TexID = (ImTextureID)(-count - 1)
		}, rect, uvMin, uvMax, ToCol(in color));
	}

	private static void EnsureMaterialTexturesResident(UnityEngine.Texture mainTexture, UnityEngine.Material material)
	{
		(mainTexture as UnityEngine.Texture2D)?.EnsureGpuResident();
		(material.GetTexture("_MaskTex") as UnityEngine.Texture2D)?.EnsureGpuResident();
	}

	private bool TryResolveTexture([NotNullWhen(true)] UnityEngine.Texture? image, out ImTextureRef texRef)
	{
		texRef = default(ImTextureRef);
		string sourcePath = null;
		GpuTextureAllocation gpuTexture;
		if (!(image is UnityEngine.Texture2D texture2D))
		{
			if (!(image is UnityEngine.RenderTexture renderTexture))
			{
				return Unresolved(image, (image == null) ? "null texture" : ("unsupported texture type " + image.GetType().Name));
			}
			gpuTexture = renderTexture.GpuTexture;
		}
		else
		{
			if (!texture2D.EnsureGpuResident())
			{
				return Unresolved(image, DescribeNonResident(texture2D));
			}
			gpuTexture = texture2D.GpuTexture;
			sourcePath = texture2D.CoreTexture?.SourcePath;
		}
		if (gpuTexture == null)
		{
			return Unresolved(image, "no gpu allocation");
		}
		if (!host.Renderer.TryGetTexRef(gpuTexture, sourcePath, out texRef))
		{
			return Unresolved(image, gpuTexture.IsDisposed ? "gpu allocation disposed" : "gpu allocation has no image view");
		}
		return true;
	}

	private static bool Unresolved(UnityEngine.Texture? image, string reason)
	{
		if (image == null || reportedUnresolved.TryGetValue(image, out object _))
		{
			return false;
		}
		reportedUnresolved.Add(image, reason);
		Waylith.Rendering.Texture2D texture2D = (image as UnityEngine.Texture2D)?.CoreTexture;
		VerboseLog.Log($"ImGui: cannot draw texture '{image.name}' ({image.width}x{image.height}, source {texture2D?.SourcePath ?? "none"}): {reason}");
		return false;
	}

	private static string DescribeNonResident(UnityEngine.Texture2D tex2d)
	{
		Waylith.Rendering.Texture2D coreTexture = tex2d.CoreTexture;
		if (coreTexture == null)
		{
			return "no core texture";
		}
		TexturePixelData cpuData = coreTexture.CpuData;
		if (cpuData == null)
		{
			return "no cpu data (destroyed or never built)";
		}
		if (cpuData.HasData)
		{
			return "cpu data present but upload produced no gpu resource (pending gpu build?)";
		}
		if (coreTexture.SourcePath == null)
		{
			return "cpu data released and no source path to rehydrate from";
		}
		return "cpu data released and texture cache miss for '" + coreTexture.SourcePath + "'";
	}

	private bool DrawsNothing(Rect rect)
	{
		if (ZeroArea(rect))
		{
			return true;
		}
		if (rotated)
		{
			return false;
		}
		float2 @float = ToScreen(rect.x, rect.y);
		float2 float2 = ToScreen(rect.x + rect.width, rect.y + rect.height);
		float2 clipRectMin = drawList.GetClipRectMin();
		float2 clipRectMax = drawList.GetClipRectMax();
		if (!(Math.Max(@float.X, float2.X) <= clipRectMin.X) && !(Math.Min(@float.X, float2.X) >= clipRectMax.X) && !(Math.Max(@float.Y, float2.Y) <= clipRectMin.Y))
		{
			return Math.Min(@float.Y, float2.Y) >= clipRectMax.Y;
		}
		return true;
	}

	private static bool ZeroArea(Rect rect)
	{
		if (rect.width != 0f)
		{
			return rect.height == 0f;
		}
		return true;
	}

	private static void FitImage(Rect rect, UnityEngine.Texture image, ScaleMode mode, out Rect dst, out float2 uvMin, out float2 uvMax)
	{
		uvMin = new float2(0f, 0f);
		uvMax = new float2(1f, 1f);
		dst = rect;
		float num = image.width;
		float num2 = image.height;
		if (num <= 0f || num2 <= 0f || rect.width <= 0f || rect.height <= 0f || mode == ScaleMode.StretchToFill)
		{
			return;
		}
		float num3 = rect.width / rect.height;
		float num4 = num / num2;
		switch (mode)
		{
		case ScaleMode.ScaleToFit:
			if (num4 > num3)
			{
				float num7 = rect.width / num4;
				dst = new Rect(rect.x, rect.y + (rect.height - num7) * 0.5f, rect.width, num7);
			}
			else
			{
				float num8 = rect.height * num4;
				dst = new Rect(rect.x + (rect.width - num8) * 0.5f, rect.y, num8, rect.height);
			}
			break;
		case ScaleMode.ScaleAndCrop:
			if (num4 > num3)
			{
				float num5 = num3 / num4;
				uvMin = new float2((1f - num5) * 0.5f, 0f);
				uvMax = new float2(1f - (1f - num5) * 0.5f, 1f);
			}
			else
			{
				float num6 = num4 / num3;
				uvMin = new float2(0f, (1f - num6) * 0.5f);
				uvMax = new float2(1f, 1f - (1f - num6) * 0.5f);
			}
			break;
		}
	}

	private void DrawPlaceholderPanel(Rect rect, UnityEngine.Color color)
	{
		if (frameActive && drawingEnabled)
		{
			ScreenBounds(rect, out var min, out var max);
			UnityEngine.Color c = color;
			c.a *= 0.15f;
			drawList.AddRectFilled(in min, in max, ToCol(in c));
			UnityEngine.Color c2 = color;
			c2.a = Math.Min(1f, c2.a * 0.6f + 0.2f);
			drawList.AddRect(in min, in max, ToCol(in c2));
		}
	}

	private static ImColor8 ToCol(in UnityEngine.Color c)
	{
		return new ImColor8(B(c.r), B(c.g), B(c.b), B(c.a));
		static byte B(float v)
		{
			return (byte)(Math.Clamp(v, 0f, 1f) * 255f + 0.5f);
		}
	}
}
