using System;
using System.Numerics;
using System.Threading;
using Waylith.Rendering;

namespace UnityStubs.Internal;

internal static class TextureST
{
	private static volatile int[] _textureIds = new int[256];

	private static readonly Lock GrowLock = new Lock();

	public static bool TryGetTextureId(int propertyId, out int textureId)
	{
		int[] textureIds = _textureIds;
		textureId = (((uint)propertyId < (uint)textureIds.Length) ? textureIds[propertyId] : 0);
		if (textureId == 0)
		{
			textureId = Resolve(propertyId);
		}
		return textureId > 0;
	}

	public static bool Is(int propertyId)
	{
		int textureId;
		return TryGetTextureId(propertyId, out textureId);
	}

	public static Vector4 Reorder(int propertyId, Vector4 value)
	{
		if (!Is(propertyId))
		{
			return value;
		}
		return new Vector4(value.Z, value.W, value.X, value.Y);
	}

	private static int Resolve(int propertyId)
	{
		if (!ShaderPropertyId.TryGetName(propertyId, out string name))
		{
			return -1;
		}
		int num;
		if (name.Length <= 3 || !name.EndsWith("_ST", StringComparison.Ordinal))
		{
			num = -1;
		}
		else
		{
			string text = name;
			num = ShaderPropertyId.GetOrCreate(text.Substring(0, text.Length - 3));
		}
		int num2 = num;
		using (GrowLock.EnterScope())
		{
			int[] array = _textureIds;
			if (propertyId >= array.Length)
			{
				int[] array2 = new int[Math.Max(propertyId + 1, array.Length * 2)];
				array.CopyTo(array2, 0);
				array = array2;
			}
			array[propertyId] = num2;
			_textureIds = array;
			return num2;
		}
	}
}
