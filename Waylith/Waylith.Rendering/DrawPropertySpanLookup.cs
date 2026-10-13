using System;
using System.Collections.Generic;
using System.Numerics;

namespace Waylith.Rendering;

public static class DrawPropertySpanLookup
{
	private static readonly HashSet<(int, DrawPropertyType, DrawPropertyType)> warnedMismatches = new HashSet<(int, DrawPropertyType, DrawPropertyType)>();

	public static bool TryGetFloat(ReadOnlySpan<DrawProperty> properties, int propertyId, out float value)
	{
		for (int num = properties.Length - 1; num >= 0; num--)
		{
			DrawProperty drawProperty = properties[num];
			if (drawProperty.PropertyId == propertyId && drawProperty.Type == DrawPropertyType.Float)
			{
				value = drawProperty.FloatValue;
				return true;
			}
		}
		value = 0f;
		return false;
	}

	public static bool TryGetInt(ReadOnlySpan<DrawProperty> properties, int propertyId, out int value)
	{
		for (int num = properties.Length - 1; num >= 0; num--)
		{
			DrawProperty drawProperty = properties[num];
			if (drawProperty.PropertyId == propertyId && drawProperty.Type == DrawPropertyType.Int)
			{
				value = drawProperty.IntValue;
				return true;
			}
		}
		value = 0;
		return false;
	}

	public static bool TryGetColor(ReadOnlySpan<DrawProperty> properties, int propertyId, out Vector4 value)
	{
		for (int num = properties.Length - 1; num >= 0; num--)
		{
			DrawProperty drawProperty = properties[num];
			if (drawProperty.PropertyId == propertyId)
			{
				if (drawProperty.Type == DrawPropertyType.Color)
				{
					value = drawProperty.VectorValue;
					return true;
				}
				WarnTypeMismatch(propertyId, DrawPropertyType.Color, drawProperty.Type);
			}
		}
		value = default(Vector4);
		return false;
	}

	public static bool TryGetVector(ReadOnlySpan<DrawProperty> properties, int propertyId, out Vector4 value)
	{
		for (int num = properties.Length - 1; num >= 0; num--)
		{
			DrawProperty drawProperty = properties[num];
			if (drawProperty.PropertyId == propertyId)
			{
				if (drawProperty.Type == DrawPropertyType.Vector)
				{
					value = drawProperty.VectorValue;
					return true;
				}
				WarnTypeMismatch(propertyId, DrawPropertyType.Vector, drawProperty.Type);
			}
		}
		value = default(Vector4);
		return false;
	}

	public static bool TryGetVectorOrColor(ReadOnlySpan<DrawProperty> properties, int propertyId, out Vector4 value)
	{
		for (int num = properties.Length - 1; num >= 0; num--)
		{
			DrawProperty drawProperty = properties[num];
			bool flag = drawProperty.PropertyId == propertyId;
			if (flag)
			{
				DrawPropertyType type = drawProperty.Type;
				bool flag2 = type - 2 <= DrawPropertyType.Int;
				flag = flag2;
			}
			if (flag)
			{
				value = drawProperty.VectorValue;
				return true;
			}
		}
		value = default(Vector4);
		return false;
	}

	public static bool TryGetMatrix(ReadOnlySpan<DrawProperty> properties, int propertyId, out Matrix4x4 value)
	{
		for (int num = properties.Length - 1; num >= 0; num--)
		{
			DrawProperty drawProperty = properties[num];
			if (drawProperty.PropertyId == propertyId && drawProperty.Type == DrawPropertyType.Matrix)
			{
				value = drawProperty.MatrixValue;
				return true;
			}
		}
		value = default(Matrix4x4);
		return false;
	}

	public static bool TryGetTexture<T>(ReadOnlySpan<DrawProperty> properties, int propertyId, out T? value) where T : class
	{
		for (int num = properties.Length - 1; num >= 0; num--)
		{
			DrawProperty drawProperty = properties[num];
			if (drawProperty.PropertyId == propertyId && drawProperty.Type == DrawPropertyType.Texture)
			{
				value = drawProperty.ReferenceValue as T;
				return true;
			}
		}
		value = null;
		return false;
	}

	private static void WarnTypeMismatch(int propertyId, DrawPropertyType expected, DrawPropertyType actual)
	{
		warnedMismatches.Add((propertyId, expected, actual));
	}
}
