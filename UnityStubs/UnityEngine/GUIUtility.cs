using System;
using System.Collections.Generic;
using Brutal.GlfwApi;
using UnityStubs.Gui;

namespace UnityEngine;

public class GUIUtility
{
	private const int FirstControlId = 1000;

	private static int nextControlId = 1000;

	private static readonly Dictionary<(Type, int), object> stateObjects = new Dictionary<(Type, int), object>();

	public static int keyboardControl { get; set; }

	public static int hotControl { get; set; }

	internal static int NextControlId
	{
		get
		{
			return nextControlId;
		}
		set
		{
			nextControlId = value;
		}
	}

	public static string systemCopyBuffer
	{
		get
		{
			return Glfw.Clipboard ?? string.Empty;
		}
		set
		{
			Glfw.Clipboard = value ?? string.Empty;
		}
	}

	public static Vector2 ScreenToGUIPoint(Vector2 screenPoint)
	{
		return GuiBackend.Current?.ScreenToGuiPoint(screenPoint) ?? screenPoint;
	}

	public static Rect GUIToScreenRect(Rect guiRect)
	{
		Vector2 vector = GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
		Vector2 vector2 = GUIToScreenPoint(new Vector2(guiRect.xMax, guiRect.yMax));
		return new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y);
	}

	public static int GetControlID(FocusType focus, Rect position)
	{
		return nextControlId++;
	}

	public static int GetControlID(FocusType focus)
	{
		return nextControlId++;
	}

	public static int GetControlID(int hint, FocusType focus)
	{
		return nextControlId++;
	}

	public static int GetControlID(int hint, FocusType focus, Rect position)
	{
		return nextControlId++;
	}

	public static int GetControlID(GUIContent contents, FocusType focus)
	{
		return nextControlId++;
	}

	public static int GetControlID(GUIContent contents, FocusType focus, Rect position)
	{
		return nextControlId++;
	}

	public static void ResetControlIds()
	{
		ResetControlIds(1000);
	}

	public static void ResetControlIds(int firstControlId)
	{
		nextControlId = firstControlId;
		GUI.ResetNamedControlOccurrences();
	}

	public static object GetStateObject(Type t, int controlID)
	{
		if (!stateObjects.TryGetValue((t, controlID), out var value))
		{
			value = (stateObjects[(t, controlID)] = Activator.CreateInstance(t));
		}
		return value;
	}

	public static object QueryStateObject(Type t, int controlID)
	{
		if (!stateObjects.TryGetValue((t, controlID), out var value))
		{
			return null;
		}
		return value;
	}

	public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
	{
		return GuiBackend.Current?.GuiToScreenPoint(guiPoint) ?? guiPoint;
	}

	public static void ScaleAroundPivot(Vector2 scale, Vector2 pivotPoint)
	{
		Matrix4x4 matrix = GUI.matrix;
		GUI.matrix = Matrix4x4.identity;
		Vector2 vector = (Vector2)matrix.MultiplyPoint3x4(GUIToScreenPoint(Vector2.zero)) + pivotPoint;
		Vector3 vector2 = new Vector3(vector.x, vector.y, 0f);
		GUI.matrix = Matrix4x4.TRS(vector2, Quaternion.identity, new Vector3(scale.x, scale.y, 1f)) * Matrix4x4.TRS(-vector2, Quaternion.identity, Vector3.one) * matrix;
	}

	public static void RotateAroundPivot(float angle, Vector2 pivotPoint)
	{
		Matrix4x4 matrix = GUI.matrix;
		GUI.matrix = Matrix4x4.identity;
		Vector2 vector = (Vector2)matrix.MultiplyPoint3x4(GUIToScreenPoint(Vector2.zero)) + pivotPoint;
		Vector3 vector2 = new Vector3(vector.x, vector.y, 0f);
		GUI.matrix = Matrix4x4.TRS(vector2, Quaternion.Euler(0f, 0f, angle), Vector3.one) * Matrix4x4.TRS(-vector2, Quaternion.identity, Vector3.one) * matrix;
	}
}
