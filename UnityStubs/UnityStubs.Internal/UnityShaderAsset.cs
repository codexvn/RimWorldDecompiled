using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AssetsTools.NET;
using AssetsTools.NET.Extra.Decompressors.LZ4;

namespace UnityStubs.Internal;

internal sealed class UnityShaderAsset
{
	private sealed class BlobReader
	{
		private readonly byte[] data;

		private int at;

		public BlobReader(byte[] data)
		{
			this.data = data;
		}

		public int Int()
		{
			if (at + 4 > data.Length)
			{
				throw new EndOfStreamException();
			}
			int result = BitConverter.ToInt32(data, at);
			at += 4;
			return result;
		}

		public void Skip(int bytes)
		{
			at += bytes;
		}

		public string String()
		{
			byte[] bytes = Bytes();
			return Encoding.UTF8.GetString(bytes);
		}

		public byte[] Bytes()
		{
			int num = Int();
			if (num < 0 || at + num > data.Length)
			{
				throw new EndOfStreamException();
			}
			byte[] result = data.AsSpan(at, num).ToArray();
			at += (num + 3) & -4;
			return result;
		}
	}

	private const int PassTypeGrab = 2;

	private const int PlatformVulkan = 18;

	private const int ProgramTypeSpirv = 25;

	private const int FirstAttributeTarget = 13;

	public string Name;

	public readonly List<UnityShaderProperty> Properties = new List<UnityShaderProperty>();

	public readonly Dictionary<string, string> SubShaderTags = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	public UnityShaderPass Pass;

	public readonly List<UnityShaderPass> Passes = new List<UnityShaderPass>();

	public string GrabTexture;

	public static UnityShaderAsset Read(AssetTypeValueField bf, out string failure)
	{
		failure = null;
		AssetTypeValueField assetTypeValueField = bf["m_ParsedForm"];
		if (assetTypeValueField.IsDummy)
		{
			failure = "no parsed form (bundle built before Unity 5.5?)";
			return null;
		}
		UnityShaderAsset unityShaderAsset = new UnityShaderAsset
		{
			Name = assetTypeValueField["m_Name"].AsString
		};
		foreach (AssetTypeValueField child in assetTypeValueField["m_PropInfo"]["m_Props"]["Array"].Children)
		{
			UnityShaderProperty unityShaderProperty = new UnityShaderProperty
			{
				Name = child["m_Name"].AsString,
				Type = (UnityShaderPropertyType)child["m_Type"].AsInt,
				DefaultTexture = (child["m_DefTexture"]["m_DefaultName"].IsDummy ? null : child["m_DefTexture"]["m_DefaultName"].AsString)
			};
			for (int i = 0; i < 4; i++)
			{
				unityShaderProperty.Default[i] = child[$"m_DefValue[{i}]"].AsFloat;
			}
			unityShaderAsset.Properties.Add(unityShaderProperty);
		}
		int num = VulkanPlatformIndex(bf);
		if (num < 0)
		{
			failure = "no Vulkan variant in this bundle (not a linux bundle?)";
			return null;
		}
		byte[][] entries;
		try
		{
			entries = ReadBlobEntries(bf, num);
		}
		catch (Exception ex)
		{
			failure = "unreadable program blob: " + ex.Message;
			return null;
		}
		foreach (AssetTypeValueField child2 in assetTypeValueField["m_SubShaders"]["Array"].Children)
		{
			List<UnityShaderPass> list = new List<UnityShaderPass>();
			string grabTexture = null;
			foreach (AssetTypeValueField child3 in child2["m_Passes"]["Array"].Children)
			{
				if (child3["m_Type"].AsInt == 2 && list.Count == 0)
				{
					AssetTypeValueField assetTypeValueField2 = child3["m_TextureName"];
					grabTexture = ((assetTypeValueField2.IsDummy || string.IsNullOrEmpty(assetTypeValueField2.AsString)) ? "_GrabTexture" : assetTypeValueField2.AsString);
				}
				if (child3["m_Type"].AsInt == 0)
				{
					UnityShaderPass unityShaderPass = ReadPass(unityShaderAsset, child3, entries, ref failure);
					if (unityShaderPass != null)
					{
						list.Add(unityShaderPass);
					}
				}
			}
			if (list.Count != 0)
			{
				unityShaderAsset.Pass = list[0];
				unityShaderAsset.Passes.AddRange(list);
				unityShaderAsset.GrabTexture = grabTexture;
				ReadTags(child2["m_Tags"], unityShaderAsset.SubShaderTags);
				failure = null;
				return unityShaderAsset;
			}
		}
		if (failure == null)
		{
			failure = "no pass with a SPIR-V program";
		}
		return null;
	}

	public static int VulkanPlatformIndex(AssetTypeValueField bf)
	{
		List<AssetTypeValueField> children = bf["platforms"]["Array"].Children;
		for (int num = children.Count - 1; num >= 0; num--)
		{
			if (children[num].AsInt == 18)
			{
				return num;
			}
		}
		return -1;
	}

	private static UnityShaderPass ReadPass(UnityShaderAsset asset, AssetTypeValueField passField, byte[][] entries, ref string failure)
	{
		if (!TryPickProgram(passField["progVertex"], out var blobIndex, out var paramBlobIndex, out var keywordIndices))
		{
			return null;
		}
		if (blobIndex >= entries.Length || paramBlobIndex >= entries.Length)
		{
			failure = "program blob index out of range";
			return null;
		}
		UnityShaderPass unityShaderPass = new UnityShaderPass
		{
			State = ReadState(asset, passField["m_State"])
		};
		try
		{
			BlobReader blobReader = new BlobReader(entries[blobIndex]);
			int num = blobReader.Int();
			if (blobReader.Int() != 25)
			{
				failure = "program blob is not SPIR-V";
				return null;
			}
			blobReader.Skip((num >= 201608000) ? 16 : 12);
			int num2 = blobReader.Int();
			for (int i = 0; i < num2; i++)
			{
				blobReader.String();
			}
			if (num >= 201806000 && num < 202012000)
			{
				int num3 = blobReader.Int();
				for (int j = 0; j < num3; j++)
				{
					blobReader.String();
				}
			}
			if (!ReadStages(blobReader.Bytes(), unityShaderPass, ref failure))
			{
				return null;
			}
			blobReader.Int();
			int num4 = blobReader.Int();
			for (int k = 0; k < num4; k++)
			{
				int item = blobReader.Int();
				int num5 = blobReader.Int();
				unityShaderPass.VertexChannels.Add((item, num5 - 13));
			}
			if (num >= 202012000)
			{
				blobReader = new BlobReader(entries[paramBlobIndex]);
				blobReader.Int();
			}
			ReadParameters(blobReader, unityShaderPass);
		}
		catch (Exception ex) when (((ex is EndOfStreamException || ex is ArgumentException || ex is IndexOutOfRangeException) ? 1 : 0) != 0)
		{
			failure = "truncated program blob";
			return null;
		}
		unityShaderPass.Keywords = ResolveKeywords(passField, keywordIndices);
		return unityShaderPass;
	}

	private static bool ReadStages(byte[] code, UnityShaderPass pass, ref string failure)
	{
		for (int i = 0; i < 5; i++)
		{
			int num = 4 + i * 8;
			if (num + 8 > code.Length)
			{
				break;
			}
			int num2 = BitConverter.ToInt32(code, num);
			int num3 = BitConverter.ToInt32(code, num + 4);
			if (num3 > 0 && num2 >= 0 && num2 + num3 <= code.Length)
			{
				byte[] array = (SmolV.IsSmolV(code.AsSpan(num2, num3)) ? SmolV.Decode(code.AsSpan(num2, num3)) : code.AsSpan(num2, num3).ToArray());
				if (array == null)
				{
					failure = "SMOL-V decode failed";
					return false;
				}
				switch (SpirvModule.ExecutionModelOf(array))
				{
				case 0:
					pass.VertexSpirv = array;
					break;
				case 4:
					pass.FragmentSpirv = array;
					break;
				default:
					failure = "uses geometry or tessellation stages";
					return false;
				case -1:
					break;
				}
			}
		}
		if (pass.VertexSpirv != null && pass.FragmentSpirv != null)
		{
			return true;
		}
		failure = "missing vertex or fragment stage";
		return false;
	}

	private static void ReadParameters(BlobReader reader, UnityShaderPass pass)
	{
		int num = reader.Int();
		for (int i = 0; i < num; i++)
		{
			UnityShaderCBuffer unityShaderCBuffer = new UnityShaderCBuffer
			{
				Name = reader.String(),
				Size = reader.Int()
			};
			ReadParamList(reader, unityShaderCBuffer.Params);
			int num2 = reader.Int();
			for (int j = 0; j < num2; j++)
			{
				reader.String();
				reader.Skip(12);
				ReadParamList(reader, null);
			}
			if (i > 0)
			{
				pass.CBuffers.Add(unityShaderCBuffer);
			}
		}
		int num3 = reader.Int();
		for (int k = 0; k < num3; k++)
		{
			string name = reader.String();
			int num4 = reader.Int();
			int packedIndex = reader.Int();
			int packedSampler = reader.Int();
			switch (num4)
			{
			case 0:
				reader.Int();
				pass.Bindings.Add(new UnityShaderBinding
				{
					Name = name,
					Kind = UnityShaderBindingKind.Texture,
					PackedIndex = packedIndex,
					PackedSampler = packedSampler
				});
				break;
			case 1:
				pass.Bindings.Add(new UnityShaderBinding
				{
					Name = name,
					Kind = UnityShaderBindingKind.ConstantBuffer,
					PackedIndex = packedIndex
				});
				break;
			case 2:
				pass.Bindings.Add(new UnityShaderBinding
				{
					Name = name,
					Kind = UnityShaderBindingKind.Buffer,
					PackedIndex = packedIndex
				});
				break;
			default:
				return;
			}
		}
	}

	private static void ReadParamList(BlobReader reader, List<UnityShaderParam> into)
	{
		int num = reader.Int();
		for (int i = 0; i < num; i++)
		{
			UnityShaderParam unityShaderParam = new UnityShaderParam
			{
				Name = reader.String()
			};
			reader.Int();
			unityShaderParam.Rows = reader.Int();
			unityShaderParam.Columns = reader.Int();
			unityShaderParam.IsMatrix = reader.Int() != 0;
			unityShaderParam.ArraySize = reader.Int();
			unityShaderParam.Offset = reader.Int();
			into?.Add(unityShaderParam);
		}
	}

	private static bool TryPickProgram(AssetTypeValueField program, out int blobIndex, out int paramBlobIndex, out List<int> keywordIndices)
	{
		blobIndex = (paramBlobIndex = -1);
		keywordIndices = null;
		AssetTypeValueField assetTypeValueField = program["m_PlayerSubPrograms"];
		if (!assetTypeValueField.IsDummy)
		{
			List<AssetTypeValueField> children = assetTypeValueField["Array"].Children;
			List<AssetTypeValueField> children2 = program["m_ParameterBlobIndices"]["Array"].Children;
			for (int i = 0; i < children.Count; i++)
			{
				List<AssetTypeValueField> children3 = children[i]["Array"].Children;
				for (int j = 0; j < children3.Count; j++)
				{
					if (children3[j]["m_GpuProgramType"].AsInt == 25)
					{
						List<int> list = ReadIndices(children3[j]["m_KeywordIndices"]);
						if (keywordIndices == null || list.Count < keywordIndices.Count)
						{
							keywordIndices = list;
							blobIndex = (int)children3[j]["m_BlobIndex"].AsUInt;
							paramBlobIndex = (int)children2[i]["Array"].Children[j].AsUInt;
						}
					}
				}
			}
			return keywordIndices != null;
		}
		foreach (AssetTypeValueField child in program["m_SubPrograms"]["Array"].Children)
		{
			if (child["m_GpuProgramType"].AsInt == 25)
			{
				List<int> list2 = ReadIndices(child["m_KeywordIndices"]);
				list2.AddRange(ReadIndices(child["m_GlobalKeywordIndices"]));
				list2.AddRange(ReadIndices(child["m_LocalKeywordIndices"]));
				if (keywordIndices == null || list2.Count < keywordIndices.Count)
				{
					keywordIndices = list2;
					blobIndex = (paramBlobIndex = (int)child["m_BlobIndex"].AsUInt);
				}
			}
		}
		return keywordIndices != null;
	}

	private static List<int> ReadIndices(AssetTypeValueField field)
	{
		List<int> list = new List<int>();
		if (field.IsDummy)
		{
			return list;
		}
		foreach (AssetTypeValueField child in field["Array"].Children)
		{
			list.Add((int)child.AsUInt);
		}
		return list;
	}

	private static string[] ResolveKeywords(AssetTypeValueField passField, List<int> indices)
	{
		if (indices == null || indices.Count == 0)
		{
			return Array.Empty<string>();
		}
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		AssetTypeValueField assetTypeValueField = passField["m_NameIndices"];
		if (!assetTypeValueField.IsDummy)
		{
			foreach (AssetTypeValueField child in assetTypeValueField["Array"].Children)
			{
				dictionary[child["second"].AsInt] = child["first"].AsString;
			}
		}
		string[] array = new string[indices.Count];
		for (int i = 0; i < indices.Count; i++)
		{
			array[i] = (dictionary.TryGetValue(indices[i], out var value) ? value : ("#" + indices[i]));
		}
		return array;
	}

	private static UnityShaderState ReadState(UnityShaderAsset asset, AssetTypeValueField s)
	{
		UnityShaderState unityShaderState = new UnityShaderState();
		if (s.IsDummy)
		{
			return unityShaderState;
		}
		AssetTypeValueField assetTypeValueField = s["rtBlend0"];
		unityShaderState.SrcBlend = Value(asset, assetTypeValueField["srcBlend"], unityShaderState.SrcBlend);
		unityShaderState.DstBlend = Value(asset, assetTypeValueField["destBlend"], unityShaderState.DstBlend);
		unityShaderState.SrcBlendAlpha = Value(asset, assetTypeValueField["srcBlendAlpha"], unityShaderState.SrcBlendAlpha);
		unityShaderState.DstBlendAlpha = Value(asset, assetTypeValueField["destBlendAlpha"], unityShaderState.DstBlendAlpha);
		unityShaderState.BlendOp = Value(asset, assetTypeValueField["blendOp"], unityShaderState.BlendOp);
		unityShaderState.ColorMask = Value(asset, assetTypeValueField["colMask"], unityShaderState.ColorMask);
		unityShaderState.ZTest = Value(asset, s["zTest"], unityShaderState.ZTest);
		unityShaderState.ZWrite = Value(asset, s["zWrite"], unityShaderState.ZWrite);
		unityShaderState.Culling = Value(asset, s["culling"], unityShaderState.Culling);
		unityShaderState.OffsetFactor = Value(asset, s["offsetFactor"], 0f);
		unityShaderState.OffsetUnits = Value(asset, s["offsetUnits"], 0f);
		unityShaderState.StencilRef = Value(asset, s["stencilRef"], 0f);
		unityShaderState.StencilReadMask = Value(asset, s["stencilReadMask"], 255f);
		unityShaderState.StencilWriteMask = Value(asset, s["stencilWriteMask"], 255f);
		AssetTypeValueField assetTypeValueField2 = s["stencilOp"];
		unityShaderState.StencilComp = Value(asset, assetTypeValueField2["comp"], 8f);
		unityShaderState.StencilPass = Value(asset, assetTypeValueField2["pass"], 0f);
		unityShaderState.StencilFail = Value(asset, assetTypeValueField2["fail"], 0f);
		unityShaderState.StencilZFail = Value(asset, assetTypeValueField2["zFail"], 0f);
		ReadTags(s["m_Tags"], unityShaderState.Tags);
		return unityShaderState;
	}

	private static float Value(UnityShaderAsset asset, AssetTypeValueField f, float fallback)
	{
		if (f.IsDummy)
		{
			return fallback;
		}
		string text = (f["name"].IsDummy ? null : f["name"].AsString);
		if (!string.IsNullOrEmpty(text) && text != "<noninit>")
		{
			foreach (UnityShaderProperty property in asset.Properties)
			{
				if (property.Name == text)
				{
					return property.Default[0];
				}
			}
		}
		return f["val"].AsFloat;
	}

	private static void ReadTags(AssetTypeValueField tagMap, Dictionary<string, string> into)
	{
		if (tagMap.IsDummy || tagMap["tags"].IsDummy)
		{
			return;
		}
		foreach (AssetTypeValueField child in tagMap["tags"]["Array"].Children)
		{
			into[child["first"].AsString] = child["second"].AsString;
		}
	}

	private static byte[][] ReadBlobEntries(AssetTypeValueField bf, int platformIndex)
	{
		byte[] buffer = ReadBytes(bf["compressedBlob"]);
		List<uint> list = ReadUIntList(bf["offsets"]["Array"].Children[platformIndex]);
		List<uint> list2 = ReadUIntList(bf["compressedLengths"]["Array"].Children[platformIndex]);
		List<uint> list3 = ReadUIntList(bf["decompressedLengths"]["Array"].Children[platformIndex]);
		byte[][] array = new byte[list.Count][];
		for (int i = 0; i < list.Count; i++)
		{
			array[i] = new byte[list3[i]];
			using Lz4DecoderStream lz4DecoderStream = new Lz4DecoderStream(new MemoryStream(buffer, (int)list[i], (int)list2[i]));
			int num;
			for (int j = 0; j < array[i].Length; j += num)
			{
				num = lz4DecoderStream.Read(array[i], j, array[i].Length - j);
				if (num <= 0)
				{
					throw new EndOfStreamException("LZ4 segment ended early");
				}
			}
		}
		BlobReader blobReader = new BlobReader(array[0]);
		byte[][] array2 = new byte[blobReader.Int()][];
		for (int k = 0; k < array2.Length; k++)
		{
			int start = blobReader.Int();
			int length = blobReader.Int();
			int num2 = blobReader.Int();
			array2[k] = array[num2].AsSpan(start, length).ToArray();
		}
		return array2;
	}

	private static List<uint> ReadUIntList(AssetTypeValueField perPlatform)
	{
		List<uint> list = new List<uint>();
		foreach (AssetTypeValueField child in perPlatform["Array"].Children)
		{
			list.Add(child.AsUInt);
		}
		return list;
	}

	private static byte[] ReadBytes(AssetTypeValueField field)
	{
		AssetTypeValue value = field.Value;
		if (value != null && value.ValueType == AssetValueType.ByteArray)
		{
			return field.AsByteArray;
		}
		AssetTypeValueField assetTypeValueField = field["Array"];
		AssetTypeValue value2 = assetTypeValueField.Value;
		if (value2 != null && value2.ValueType == AssetValueType.ByteArray)
		{
			return assetTypeValueField.AsByteArray;
		}
		byte[] array = new byte[assetTypeValueField.Children.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)assetTypeValueField.Children[i].AsUInt;
		}
		return array;
	}
}
