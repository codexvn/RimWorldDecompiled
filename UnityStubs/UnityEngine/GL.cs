using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityStubs.Gui;
using UnityStubs.Internal;

namespace UnityEngine;

public class GL
{
	public const int LINE_STRIP = 2;

	public const int LINES = 1;

	public const int QUADS = 7;

	public const int TRIANGLE_STRIP = 5;

	public const int TRIANGLES = 4;

	private static readonly List<GuiVertex> _vertices = new List<GuiVertex>();

	private static int _mode = -1;

	private static Color32 _color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	private static Vector2 _texCoord;

	private static readonly Stack<(Matrix4x4 matrix, bool absolute)> _matrixStack = new Stack<(Matrix4x4, bool)>();

	internal static Matrix4x4 GuiTransform { get; private set; } = Matrix4x4.identity;

	internal static bool GuiTransformActive { get; private set; }

	internal static bool GuiTransformAbsolute { get; private set; }

	public static Matrix4x4 modelview
	{
		get
		{
			return GuiTransform;
		}
		set
		{
			SetTransform(value);
		}
	}

	public static bool sRGBWrite
	{
		get
		{
			return true;
		}
		set
		{
			StubWarner.LogStub("sRGBWrite", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\GL.cs", 141);
		}
	}

	public static void Begin(int mode)
	{
		_mode = mode;
		_vertices.Clear();
	}

	public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor)
	{
		if (clearColor && RenderTexture.active != null)
		{
			RenderTexture.active.ClearPixels(backgroundColor);
		}
	}

	public static void Color(Color c)
	{
		_color = c;
	}

	public static void End()
	{
		int mode = _mode;
		_mode = -1;
		if (mode < 0 || _vertices.Count == 0)
		{
			return;
		}
		if (GuiBackend.Current == null)
		{
			StubWarner.LogStub("End", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\GL.cs", 38);
			_vertices.Clear();
			return;
		}
		Span<GuiVertex> span = CollectionsMarshal.AsSpan(_vertices);
		RenderTexture active = RenderTexture.active;
		if ((object)active != null && GuiTransformAbsolute && (active.width != Screen.width || active.height != Screen.height))
		{
			float num = (float)Screen.width / (float)active.width;
			float num2 = (float)Screen.height / (float)active.height;
			for (int i = 0; i < span.Length; i++)
			{
				span[i].position = new Vector2(span[i].position.x * num, span[i].position.y * num2);
			}
		}
		Texture texture = Material.CurrentImmediateSource?.mainTexture;
		GuiBackend.Current.DrawPrimitives(mode, span, texture, GuiTransformAbsolute);
		_vertices.Clear();
	}

	public static Matrix4x4 GetGPUProjectionMatrix(Matrix4x4 proj, bool renderIntoTexture)
	{
		return proj;
	}

	public static void Flush()
	{
	}

	public static void InvalidateState()
	{
		StubWarner.LogStub("InvalidateState", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\GL.cs", 61);
	}

	public static void Viewport(Rect pixelRect)
	{
		StubWarner.LogStub("Viewport", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\GL.cs", 62);
	}

	public static void LoadIdentity()
	{
		SetTransform(Matrix4x4.identity);
	}

	public static void LoadOrtho()
	{
		var (x, num) = TargetSize();
		SetTransform(Matrix4x4.Translate(new Vector3(0f, num, 0f)) * Matrix4x4.Scale(new Vector3(x, 0f - num, 1f)));
	}

	public static void LoadPixelMatrix()
	{
		var (right, top) = TargetSize();
		LoadPixelMatrix(0f, right, 0f, top);
	}

	public static void LoadPixelMatrix(float left, float right, float bottom, float top)
	{
		(float w, float h) tuple = TargetSize();
		float item = tuple.w;
		float item2 = tuple.h;
		float x = ((right == left) ? 1f : (item / (right - left)));
		float y = ((top == bottom) ? 1f : (item2 / (bottom - top)));
		SetTransform(Matrix4x4.Scale(new Vector3(x, y, 1f)) * Matrix4x4.Translate(new Vector3(0f - left, 0f - top, 0f)));
	}

	private static (float w, float h) TargetSize()
	{
		RenderTexture active = RenderTexture.active;
		(int, int) tuple = ((active != null) ? (active.width, active.height) : (Screen.width, Screen.height));
		return (w: tuple.Item1, h: tuple.Item2);
	}

	private static void SetTransform(Matrix4x4 m)
	{
		GuiTransform = m;
		GuiTransformActive = !m.isIdentity;
		GuiTransformAbsolute = true;
	}

	internal static Rect TransformRectBounds(Rect r)
	{
		if (!GuiTransformActive)
		{
			return r;
		}
		Matrix4x4 guiTransform = GuiTransform;
		Vector3 vector = guiTransform.MultiplyPoint3x4(new Vector3(r.xMin, r.yMin, 0f));
		Vector3 vector2 = guiTransform.MultiplyPoint3x4(new Vector3(r.xMax, r.yMin, 0f));
		Vector3 vector3 = guiTransform.MultiplyPoint3x4(new Vector3(r.xMax, r.yMax, 0f));
		Vector3 vector4 = guiTransform.MultiplyPoint3x4(new Vector3(r.xMin, r.yMax, 0f));
		float xmin = MathF.Min(MathF.Min(vector.x, vector2.x), MathF.Min(vector3.x, vector4.x));
		float xmax = MathF.Max(MathF.Max(vector.x, vector2.x), MathF.Max(vector3.x, vector4.x));
		float ymin = MathF.Min(MathF.Min(vector.y, vector2.y), MathF.Min(vector3.y, vector4.y));
		float ymax = MathF.Max(MathF.Max(vector.y, vector2.y), MathF.Max(vector3.y, vector4.y));
		return Rect.MinMaxRect(xmin, ymin, xmax, ymax);
	}

	public static void MultMatrix(Matrix4x4 m)
	{
		GuiTransform *= m;
		GuiTransformActive = !GuiTransform.isIdentity;
	}

	public static void PopMatrix()
	{
		if (_matrixStack.Count != 0)
		{
			(Matrix4x4 matrix, bool absolute) tuple = _matrixStack.Pop();
			GuiTransform = tuple.matrix;
			GuiTransformAbsolute = tuple.absolute;
			GuiTransformActive = !GuiTransform.isIdentity;
		}
	}

	public static void PushMatrix()
	{
		_matrixStack.Push((GuiTransform, GuiTransformAbsolute));
	}

	public static void LoadProjectionMatrix(Matrix4x4 mat)
	{
		StubWarner.LogStub("LoadProjectionMatrix", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\GL.cs", 138);
	}

	public static void TexCoord2(float x, float y)
	{
		_texCoord = new Vector2(x, y);
	}

	public static void TexCoord(Vector3 v)
	{
		_texCoord = new Vector2(v.x, v.y);
	}

	public static void Vertex3(float x, float y, float z)
	{
		Vertex(new Vector3(x, y, z));
	}

	public static void Vertex(Vector3 v)
	{
		if (_mode >= 0)
		{
			if (GuiTransformActive)
			{
				v = GuiTransform.MultiplyPoint3x4(v);
			}
			_vertices.Add(new GuiVertex
			{
				position = new Vector2(v.x, v.y),
				uv = _texCoord,
				color = _color
			});
		}
	}
}
