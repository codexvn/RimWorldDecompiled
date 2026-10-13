using System.Numerics;

namespace Waylith.Rendering;

public readonly struct DrawProperty
{
	public int PropertyId { get; }

	public DrawPropertyType Type { get; }

	public float FloatValue { get; }

	public int IntValue { get; }

	public Vector4 VectorValue { get; }

	public Matrix4x4 MatrixValue { get; }

	public object? ReferenceValue { get; }

	private DrawProperty(int propertyId, DrawPropertyType type, float floatValue, int intValue, Vector4 vectorValue, Matrix4x4 matrixValue, object? referenceValue)
	{
		PropertyId = propertyId;
		Type = type;
		FloatValue = floatValue;
		IntValue = intValue;
		VectorValue = vectorValue;
		MatrixValue = matrixValue;
		ReferenceValue = referenceValue;
	}

	public static DrawProperty Float(int propertyId, float value)
	{
		return new DrawProperty(propertyId, DrawPropertyType.Float, value, 0, default(Vector4), default(Matrix4x4), null);
	}

	public static DrawProperty Int(int propertyId, int value)
	{
		return new DrawProperty(propertyId, DrawPropertyType.Int, 0f, value, default(Vector4), default(Matrix4x4), null);
	}

	public static DrawProperty Vector(int propertyId, Vector4 value)
	{
		return new DrawProperty(propertyId, DrawPropertyType.Vector, 0f, 0, value, default(Matrix4x4), null);
	}

	public static DrawProperty Color(int propertyId, Vector4 value)
	{
		return new DrawProperty(propertyId, DrawPropertyType.Color, 0f, 0, value, default(Matrix4x4), null);
	}

	public static DrawProperty Matrix(int propertyId, Matrix4x4 value)
	{
		return new DrawProperty(propertyId, DrawPropertyType.Matrix, 0f, 0, default(Vector4), value, null);
	}

	public static DrawProperty Texture(int propertyId, object? value)
	{
		return new DrawProperty(propertyId, DrawPropertyType.Texture, 0f, 0, default(Vector4), default(Matrix4x4), value);
	}
}
