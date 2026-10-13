using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using AssetsTools.NET.Texture;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.Video;
using Waylith.Logging;
using Waylith.Rendering;
using Waylith.UnityCompat;

namespace UnityStubs.Internal;

internal sealed class AssetBundleReader : IDisposable
{
	private readonly record struct MeshChannel(int Stream, int Offset, int Format, int Dimension);

	private sealed class PrefabNode
	{
		public string Name;

		public int Layer;

		public bool Active;

		public Vector3 Position;

		public Quaternion Rotation = Quaternion.identity;

		public Vector3 Scale = Vector3.one;

		public long TransformId;

		public readonly List<PrefabComponent> Components = new List<PrefabComponent>();

		public readonly List<PrefabNode> Children = new List<PrefabNode>();
	}

	private sealed record PrefabComponent(AssetClassID Class, AssetTypeValueField Field, UnityEngine.Material[] Materials, Mesh Mesh, long[] Bones = null, long RootBone = 0L);

	private readonly struct AssetRef
	{
		public readonly AssetsFileInstance File;

		public readonly long PathId;

		public readonly AssetClassID Class;

		public AssetRef(AssetsFileInstance file, long pathId, AssetClassID cls)
		{
			File = file;
			PathId = pathId;
			Class = cls;
		}
	}

	private const int ChannelPosition = 0;

	private const int ChannelNormal = 1;

	private const int ChannelTangent = 2;

	private const int ChannelColor = 3;

	private const int ChannelTexCoord0 = 4;

	private const int ChannelBlendWeight = 12;

	private const int ChannelBlendIndices = 13;

	private static readonly HashSet<(string, string)> WarnedModules = new HashSet<(string, string)>();

	private readonly object _sync = new object();

	private AssetsManager _manager;

	private readonly BundleFileInstance _bundle;

	private readonly string _bundleName;

	private readonly string _filePath;

	private readonly List<AssetsFileInstance> _files = new List<AssetsFileInstance>();

	private readonly List<KeyValuePair<string, AssetRef>> _container = new List<KeyValuePair<string, AssetRef>>();

	private readonly Dictionary<string, List<AssetRef>> _byPath = new Dictionary<string, List<AssetRef>>(StringComparer.OrdinalIgnoreCase);

	private readonly Dictionary<string, List<AssetRef>> _byName = new Dictionary<string, List<AssetRef>>(StringComparer.OrdinalIgnoreCase);

	private readonly Dictionary<(AssetsFileInstance file, long pathId), UnityEngine.Object> _loaded = new Dictionary<(AssetsFileInstance, long), UnityEngine.Object>();

	private readonly HashSet<(AssetsFileInstance file, long pathId)> _failed = new HashSet<(AssetsFileInstance, long)>();

	private readonly HashSet<string> _warnedClasses = new HashSet<string>();

	private string[] _assetNames;

	private bool _disposed;

	private bool _linuxSiblingProbed;

	private AssetBundleReader _linuxSibling;

	private Dictionary<string, AssetRef> _shadersByName;

	private static readonly Dictionary<string, string> LinuxFolderFor = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		["StandaloneWindows"] = "StandaloneLinux64",
		["StandaloneWindows64"] = "StandaloneLinux64",
		["StandaloneOSX"] = "StandaloneLinux64",
		["Win64"] = "Linux",
		["Windows"] = "Linux",
		["MacOS"] = "Linux",
		["Mac"] = "Linux"
	};

	private AssetsManager Manager => _manager;

	public string[] AssetNames
	{
		get
		{
			lock (_sync)
			{
				if (_assetNames != null)
				{
					return _assetNames;
				}
				List<string> list = new List<string>(_byPath.Count);
				foreach (KeyValuePair<string, AssetRef> item in _container)
				{
					if (!list.Contains(item.Key))
					{
						list.Add(item.Key);
					}
				}
				return _assetNames = list.ToArray();
			}
		}
	}

	public IEnumerable<UnityEngine.Object> LoadedObjects
	{
		get
		{
			lock (_sync)
			{
				return new List<UnityEngine.Object>(_loaded.Values);
			}
		}
	}

	private UnityEngine.Object BuildMesh(AssetTypeValueField bf)
	{
		string asString = bf["m_Name"].AsString;
		AssetTypeValueField assetTypeValueField = bf["m_VertexData"];
		int vertexCount = (int)assetTypeValueField["m_VertexCount"].AsUInt;
		byte[] data = assetTypeValueField["m_DataSize"].AsByteArray;
		if (bf["m_MeshCompression"].AsInt != 0 || (!bf["m_StreamData.path"].IsDummy && bf["m_StreamData.path"].AsString.Length > 0) || data == null || data.Length == 0 || vertexCount == 0)
		{
			if (_warnedClasses.Add("mesh:" + asString))
			{
				Debug.LogWarning($"AssetBundle '{_bundleName}': mesh '{asString}' is compressed, streamed or empty, which isn't supported, returning null.");
			}
			return null;
		}
		List<AssetTypeValueField> children = assetTypeValueField["m_Channels.Array"].Children;
		MeshChannel[] channels = new MeshChannel[children.Count];
		for (int i = 0; i < channels.Length; i++)
		{
			AssetTypeValueField assetTypeValueField2 = children[i];
			channels[i] = new MeshChannel(assetTypeValueField2["stream"].AsInt, assetTypeValueField2["offset"].AsInt, assetTypeValueField2["format"].AsInt, assetTypeValueField2["dimension"].AsInt & 0xF);
		}
		int num = 0;
		MeshChannel[] array = channels;
		for (int j = 0; j < array.Length; j++)
		{
			MeshChannel meshChannel = array[j];
			if (meshChannel.Dimension > 0)
			{
				num = Math.Max(num, meshChannel.Stream + 1);
			}
		}
		int[] streamStart = new int[num];
		int[] streamStride = new int[num];
		array = channels;
		for (int j = 0; j < array.Length; j++)
		{
			MeshChannel meshChannel2 = array[j];
			if (meshChannel2.Dimension > 0)
			{
				streamStride[meshChannel2.Stream] += meshChannel2.Dimension * FormatSize(meshChannel2.Format);
			}
		}
		int num2 = 0;
		for (int k = 0; k < num; k++)
		{
			streamStart[k] = num2;
			num2 = (num2 + vertexCount * streamStride[k] + 15) & -16;
		}
		Vector3[] vertices = Array.ConvertAll(Read(0), (Vector4 v) => new Vector3(v.x, v.y, v.z));
		Vector3[] normals = Array.ConvertAll(Read(1), (Vector4 v) => new Vector3(v.x, v.y, v.z));
		Vector4[] tangents = Read(2);
		bool opaque = channels.Length > 3 && channels[3].Dimension < 4;
		Color32[] colors = Array.ConvertAll(Read(3), (Converter<Vector4, Color32>)((Vector4 v) => new Color(v.x, v.y, v.z, opaque ? 1f : v.w)));
		Vector4[][] array2 = new Vector4[4][];
		for (int l = 0; l < array2.Length; l++)
		{
			array2[l] = Read(4 + l);
		}
		byte[] array3 = bf["m_IndexBuffer.Array"].AsByteArray ?? Array.Empty<byte>();
		bool flag = bf["m_IndexFormat"].AsInt == 1;
		List<AssetTypeValueField> children2 = bf["m_SubMeshes.Array"].Children;
		int[][] array4 = new int[children2.Count][];
		for (int m = 0; m < array4.Length; m++)
		{
			AssetTypeValueField assetTypeValueField3 = children2[m];
			int asInt = assetTypeValueField3["topology"].AsInt;
			if (asInt != 0)
			{
				if (_warnedClasses.Add("meshtopology:" + asString))
				{
					Debug.LogWarning($"AssetBundle '{_bundleName}': mesh '{asString}' has a {asInt} submesh, only triangles are read, left empty.");
				}
				array4[m] = Array.Empty<int>();
			}
			else
			{
				int asUInt = (int)assetTypeValueField3["firstByte"].AsUInt;
				int asUInt2 = (int)assetTypeValueField3["baseVertex"].AsUInt;
				int[] array5 = new int[assetTypeValueField3["indexCount"].AsUInt];
				for (int n = 0; n < array5.Length; n++)
				{
					array5[n] = asUInt2 + (flag ? BinaryPrimitives.ReadInt32LittleEndian(array3.AsSpan(asUInt + n * 4)) : BinaryPrimitives.ReadUInt16LittleEndian(array3.AsSpan(asUInt + n * 2)));
				}
				array4[m] = array5;
			}
		}
		Mesh mesh = Mesh.FromAssetData(asString, vertices, normals, tangents, colors, array2, array4, flag);
		List<AssetTypeValueField> children3 = bf["m_BindPose.Array"].Children;
		if (children3.Count > 0)
		{
			Matrix4x4[] array6 = new Matrix4x4[children3.Count];
			for (int num3 = 0; num3 < array6.Length; num3++)
			{
				AssetTypeValueField assetTypeValueField4 = children3[num3];
				for (int num4 = 0; num4 < 4; num4++)
				{
					for (int num5 = 0; num5 < 4; num5++)
					{
						array6[num3][num4, num5] = assetTypeValueField4["e" + num4 + num5].AsFloat;
					}
				}
			}
			mesh.bindposes = array6;
		}
		Vector4[] array7 = Read(12);
		Vector4[] array8 = Read(13);
		if (array8.Length != 0)
		{
			bool flag2 = array7.Length == 0;
			BoneWeight[] array9 = new BoneWeight[vertexCount];
			for (int num6 = 0; num6 < vertexCount; num6++)
			{
				Vector4 vector = (flag2 ? new Vector4(1f, 0f, 0f, 0f) : array7[num6]);
				Vector4 vector2 = array8[num6];
				array9[num6] = new BoneWeight
				{
					weight0 = vector.x,
					weight1 = vector.y,
					weight2 = vector.z,
					weight3 = vector.w,
					boneIndex0 = (int)vector2.x,
					boneIndex1 = (int)vector2.y,
					boneIndex2 = (int)vector2.z,
					boneIndex3 = (int)vector2.w
				};
			}
			mesh.boneWeights = array9;
		}
		else if (bf["m_VariableBoneCountWeights.m_Data.Array"].Children.Count > 0 && _warnedClasses.Add("meshweights:" + asString))
		{
			Debug.LogWarning($"AssetBundle '{_bundleName}': mesh '{asString}' stores unlimited bone weights, which isn't supported, it will draw unskinned.");
		}
		return mesh;
		Vector4[] Read(int channel)
		{
			if (channel >= channels.Length || channels[channel].Dimension == 0)
			{
				return Array.Empty<Vector4>();
			}
			MeshChannel meshChannel3 = channels[channel];
			int num7 = FormatSize(meshChannel3.Format);
			Vector4[] array10 = new Vector4[vertexCount];
			int num8 = streamStride[meshChannel3.Stream];
			int num9 = streamStart[meshChannel3.Stream] + meshChannel3.Offset;
			if (num9 + (vertexCount - 1) * num8 + meshChannel3.Dimension * num7 > data.Length)
			{
				throw new InvalidOperationException($"vertex channel {channel} runs past the vertex data");
			}
			for (int num10 = 0; num10 < vertexCount; num10++)
			{
				Span<byte> span = data.AsSpan(num9 + num10 * num8);
				Vector4 vector3 = default(Vector4);
				for (int num11 = 0; num11 < meshChannel3.Dimension && num11 < 4; num11++)
				{
					vector3[num11] = ReadComponent(span.Slice(num11 * num7), meshChannel3.Format);
				}
				array10[num10] = vector3;
			}
			return array10;
		}
	}

	private static int FormatSize(int format)
	{
		switch (format)
		{
		case 0:
		case 10:
		case 11:
			return 4;
		case 1:
		case 4:
		case 5:
		case 8:
		case 9:
			return 2;
		case 2:
		case 3:
		case 6:
		case 7:
			return 1;
		default:
			throw new NotSupportedException($"vertex format {format}");
		}
	}

	private static float ReadComponent(ReadOnlySpan<byte> at, int format)
	{
		return format switch
		{
			0 => BinaryPrimitives.ReadSingleLittleEndian(at), 
			1 => (float)BinaryPrimitives.ReadHalfLittleEndian(at), 
			2 => (float)(int)at[0] / 255f, 
			3 => Math.Max((float)(sbyte)at[0] / 127f, -1f), 
			4 => (float)(int)BinaryPrimitives.ReadUInt16LittleEndian(at) / 65535f, 
			5 => Math.Max((float)BinaryPrimitives.ReadInt16LittleEndian(at) / 32767f, -1f), 
			6 => (int)at[0], 
			7 => (sbyte)at[0], 
			8 => (int)BinaryPrimitives.ReadUInt16LittleEndian(at), 
			9 => BinaryPrimitives.ReadInt16LittleEndian(at), 
			10 => BinaryPrimitives.ReadUInt32LittleEndian(at), 
			11 => BinaryPrimitives.ReadInt32LittleEndian(at), 
			_ => throw new NotSupportedException($"vertex format {format}"), 
		};
	}

	private GameObject BuildGameObject(AssetsFileInstance file, AssetTypeValueField bf)
	{
		UnityWorld unityWorld = UnityRuntime.Current?.World;
		if (unityWorld == null)
		{
			return null;
		}
		PrefabNode root = ParsePrefabNode(file, bf);
		GameObject gameObject = BuildPrefab(root, token: true);
		unityWorld.RegisterCloneFactory(gameObject, delegate
		{
			GameObject gameObject2 = BuildPrefab(root, token: false);
			gameObject2.name = root.Name + "(Clone)";
			return gameObject2;
		});
		return gameObject;
	}

	private PrefabNode ParsePrefabNode(AssetsFileInstance file, AssetTypeValueField go)
	{
		PrefabNode prefabNode = new PrefabNode
		{
			Name = go["m_Name"].AsString,
			Layer = go["m_Layer"].AsInt,
			Active = go["m_IsActive"].AsBool
		};
		foreach (AssetTypeValueField child in go["m_Component.Array"].Children)
		{
			AssetTypeValueField assetTypeValueField = child["component"];
			if (assetTypeValueField.IsDummy)
			{
				assetTypeValueField = child["second"];
			}
			if (!TryResolvePPtr(file, assetTypeValueField["m_FileID"].AsInt, assetTypeValueField["m_PathID"].AsLong, out var target))
			{
				continue;
			}
			AssetTypeValueField baseField = Manager.GetBaseField(target.File, target.File.file.GetAssetInfo(target.PathId));
			switch (target.Class)
			{
			case AssetClassID.Transform:
				prefabNode.TransformId = target.PathId;
				prefabNode.Position = ReadVector3(baseField["m_LocalPosition"]);
				prefabNode.Rotation = ReadQuaternion(baseField["m_LocalRotation"]);
				prefabNode.Scale = ReadVector3(baseField["m_LocalScale"]);
				foreach (AssetTypeValueField child2 in baseField["m_Children.Array"].Children)
				{
					if (TryResolvePPtr(target.File, child2["m_FileID"].AsInt, child2["m_PathID"].AsLong, out var target2))
					{
						AssetTypeValueField assetTypeValueField2 = Manager.GetBaseField(target2.File, target2.File.file.GetAssetInfo(target2.PathId))["m_GameObject"];
						if (TryResolvePPtr(target2.File, assetTypeValueField2["m_FileID"].AsInt, assetTypeValueField2["m_PathID"].AsLong, out var target3))
						{
							prefabNode.Children.Add(ParsePrefabNode(target3.File, Manager.GetBaseField(target3.File, target3.File.file.GetAssetInfo(target3.PathId))));
						}
					}
				}
				break;
			case AssetClassID.MeshRenderer:
			case AssetClassID.MeshFilter:
			case AssetClassID.BoxCollider:
			case AssetClassID.CapsuleCollider:
			case AssetClassID.ParticleSystem:
			case AssetClassID.ParticleSystemRenderer:
				prefabNode.Components.Add(new PrefabComponent(target.Class, baseField, ResolveMaterials(target.File, baseField), ResolveMesh(target.File, baseField["m_Mesh"], target.Class == AssetClassID.MeshFilter), null, 0L));
				break;
			case AssetClassID.SkinnedMeshRenderer:
			{
				List<AssetTypeValueField> children = baseField["m_Bones.Array"].Children;
				long[] array = new long[children.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = children[i]["m_PathID"].AsLong;
				}
				prefabNode.Components.Add(new PrefabComponent(target.Class, baseField, ResolveMaterials(target.File, baseField), ResolveMesh(target.File, baseField["m_Mesh"], needed: true), array, baseField["m_RootBone.m_PathID"].AsLong));
				break;
			}
			default:
				if (_warnedClasses.Add("prefab:" + target.Class))
				{
					Debug.LogWarning($"AssetBundle '{_bundleName}': prefab '{prefabNode.Name}' has a {target.Class} component, which isn't implemented, skipped.");
				}
				break;
			}
		}
		return prefabNode;
	}

	private UnityEngine.Material[] ResolveMaterials(AssetsFileInstance file, AssetTypeValueField field)
	{
		AssetTypeValueField assetTypeValueField = field["m_Materials.Array"];
		if (assetTypeValueField.IsDummy)
		{
			return Array.Empty<UnityEngine.Material>();
		}
		List<UnityEngine.Material> list = new List<UnityEngine.Material>();
		foreach (AssetTypeValueField child in assetTypeValueField.Children)
		{
			if (TryResolvePPtr(file, child["m_FileID"].AsInt, child["m_PathID"].AsLong, out var target) && target.Class == AssetClassID.Material)
			{
				list.Add(Materialize(target) as UnityEngine.Material);
			}
		}
		return list.ToArray();
	}

	private Mesh ResolveMesh(AssetsFileInstance file, AssetTypeValueField pptr, bool needed)
	{
		if (pptr.IsDummy)
		{
			return null;
		}
		int asInt = pptr["m_FileID"].AsInt;
		long asLong = pptr["m_PathID"].AsLong;
		if (asLong == 0L)
		{
			return null;
		}
		List<AssetsFileExternal> externals = file.file.Metadata.Externals;
		PrimitiveType? primitiveType;
		if (asInt > 0 && asInt <= externals.Count && externals[asInt - 1].PathName.Contains("unity default resources", StringComparison.OrdinalIgnoreCase))
		{
			long num = asLong - 10202;
			if ((ulong)num > 8uL)
			{
				goto IL_00eb;
			}
			switch (num)
			{
			case 0L:
				break;
			case 4L:
				goto IL_00b9;
			case 5L:
				goto IL_00c3;
			case 6L:
				goto IL_00cd;
			case 7L:
				goto IL_00d7;
			case 8L:
				goto IL_00e1;
			default:
				goto IL_00eb;
			}
			primitiveType = PrimitiveType.Cube;
			goto IL_00f7;
		}
		goto IL_0111;
		IL_00cd:
		primitiveType = PrimitiveType.Capsule;
		goto IL_00f7;
		IL_00e1:
		primitiveType = PrimitiveType.Quad;
		goto IL_00f7;
		IL_00eb:
		primitiveType = null;
		goto IL_00f7;
		IL_00f7:
		PrimitiveType? primitiveType2 = primitiveType;
		if (primitiveType2.HasValue)
		{
			return GameObject.GetOrCreatePrimitiveMesh(primitiveType2.Value);
		}
		goto IL_0111;
		IL_0111:
		if (!needed)
		{
			return null;
		}
		if (!TryResolvePPtr(file, asInt, asLong, out var target))
		{
			return null;
		}
		return Materialize(target) as Mesh;
		IL_00d7:
		primitiveType = PrimitiveType.Plane;
		goto IL_00f7;
		IL_00c3:
		primitiveType = PrimitiveType.Sphere;
		goto IL_00f7;
		IL_00b9:
		primitiveType = PrimitiveType.Cylinder;
		goto IL_00f7;
	}

	private static GameObject BuildPrefab(PrefabNode root, bool token)
	{
		Dictionary<long, Transform> dictionary = new Dictionary<long, Transform>();
		List<(SkinnedMeshRenderer, PrefabComponent)> list = new List<(SkinnedMeshRenderer, PrefabComponent)>();
		GameObject result = BuildPrefabNode(root, token, dictionary, list);
		foreach (var item3 in list)
		{
			SkinnedMeshRenderer item = item3.Item1;
			PrefabComponent item2 = item3.Item2;
			Transform[] array = new Transform[item2.Bones.Length];
			for (int i = 0; i < array.Length; i++)
			{
				dictionary.TryGetValue(item2.Bones[i], out array[i]);
			}
			item.bones = array;
			if (dictionary.TryGetValue(item2.RootBone, out var value))
			{
				item.rootBone = value;
			}
		}
		return result;
	}

	private static GameObject BuildPrefabNode(PrefabNode node, bool token, Dictionary<long, Transform> transforms, List<(SkinnedMeshRenderer, PrefabComponent)> skinned)
	{
		GameObject gameObject = (token ? GameObject.CreateAssetToken(node.Name) : new GameObject(node.Name));
		gameObject.layer = node.Layer;
		Transform transform = gameObject.transform;
		transform.localPosition = node.Position;
		transform.localRotation = node.Rotation;
		transform.localScale = node.Scale;
		transforms[node.TransformId] = transform;
		List<ParticleSystem> list = new List<ParticleSystem>();
		foreach (PrefabComponent component in node.Components)
		{
			BuildComponent(gameObject, component, list, skinned);
		}
		foreach (PrefabNode child in node.Children)
		{
			BuildPrefabNode(child, token, transforms, skinned).transform.SetParent(transform, worldPositionStays: false);
		}
		if (!token)
		{
			foreach (ParticleSystem item in list)
			{
				item.Attach();
			}
		}
		if (!node.Active)
		{
			gameObject.SetActive(value: false);
		}
		return gameObject;
	}

	private static void BuildComponent(GameObject go, PrefabComponent component, List<ParticleSystem> systems, List<(SkinnedMeshRenderer, PrefabComponent)> skinned)
	{
		AssetTypeValueField field = component.Field;
		switch (component.Class)
		{
		case AssetClassID.MeshFilter:
			go.AddComponent<MeshFilter>().sharedMesh = component.Mesh;
			break;
		case AssetClassID.MeshRenderer:
		{
			MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
			meshRenderer.enabled = Bool(field["m_Enabled"], fallback: true);
			meshRenderer.sharedMaterials = component.Materials;
			break;
		}
		case AssetClassID.SkinnedMeshRenderer:
		{
			SkinnedMeshRenderer skinnedMeshRenderer = go.AddComponent<SkinnedMeshRenderer>();
			skinnedMeshRenderer.enabled = Bool(field["m_Enabled"], fallback: true);
			skinnedMeshRenderer.sharedMaterials = component.Materials;
			skinnedMeshRenderer.sharedMesh = component.Mesh;
			skinnedMeshRenderer.quality = (SkinQuality)field["m_Quality"].AsInt;
			skinnedMeshRenderer.updateWhenOffscreen = Bool(field["m_UpdateWhenOffscreen"], fallback: false);
			skinnedMeshRenderer.skinnedMotionVectors = Bool(field["m_SkinnedMotionVectors"], fallback: true);
			skinned.Add((skinnedMeshRenderer, component));
			break;
		}
		case AssetClassID.BoxCollider:
		{
			BoxCollider boxCollider = go.AddComponent<BoxCollider>();
			boxCollider.enabled = Bool(field["m_Enabled"], fallback: true);
			boxCollider.isTrigger = Bool(field["m_IsTrigger"], fallback: false);
			boxCollider.center = ReadVector3(field["m_Center"]);
			boxCollider.size = ReadVector3(field["m_Size"]);
			break;
		}
		case AssetClassID.CapsuleCollider:
		{
			CapsuleCollider capsuleCollider = go.AddComponent<CapsuleCollider>();
			capsuleCollider.enabled = Bool(field["m_Enabled"], fallback: true);
			capsuleCollider.isTrigger = Bool(field["m_IsTrigger"], fallback: false);
			capsuleCollider.center = ReadVector3(field["m_Center"]);
			capsuleCollider.radius = field["m_Radius"].AsFloat;
			capsuleCollider.height = field["m_Height"].AsFloat;
			capsuleCollider.direction = field["m_Direction"].AsInt;
			break;
		}
		case AssetClassID.ParticleSystemRenderer:
			BuildParticleRenderer(go.AddComponent<ParticleSystemRenderer>(), component);
			break;
		case AssetClassID.ParticleSystem:
		{
			ParticleSystem particleSystem = (ParticleSystem)UnityRuntime.Current.World.AddComponent(go, typeof(ParticleSystem));
			ReadParticleSystem(field, particleSystem);
			systems.Add(particleSystem);
			break;
		}
		}
	}

	private static void BuildParticleRenderer(ParticleSystemRenderer renderer, PrefabComponent component)
	{
		AssetTypeValueField field = component.Field;
		renderer.enabled = Bool(field["m_Enabled"], fallback: true);
		if (component.Materials.Length != 0)
		{
			renderer.sharedMaterial = component.Materials[0];
		}
		renderer.sortingOrder = field["m_SortingOrder"].AsInt;
		renderer.renderMode = (ParticleSystemRenderMode)field["m_RenderMode"].AsInt;
		renderer.sortMode = (ParticleSystemSortMode)field["m_SortMode"].AsInt;
		renderer.alignment = (ParticleSystemRenderSpace)field["m_RenderAlignment"].AsInt;
		renderer.minParticleSize = field["m_MinParticleSize"].AsFloat;
		renderer.maxParticleSize = field["m_MaxParticleSize"].AsFloat;
		renderer.cameraVelocityScale = field["m_CameraVelocityScale"].AsFloat;
		renderer.velocityScale = field["m_VelocityScale"].AsFloat;
		renderer.lengthScale = field["m_LengthScale"].AsFloat;
		renderer.sortingFudge = field["m_SortingFudge"].AsFloat;
		renderer.normalDirection = field["m_NormalDirection"].AsFloat;
		renderer.pivot = ReadVector3(field["m_Pivot"]);
		renderer.flip = ReadVector3(field["m_Flip"]);
		renderer.allowRoll = Bool(field["m_AllowRoll"], fallback: true);
		renderer.freeformStretching = Bool(field["m_FreeformStretching"], fallback: false);
		renderer.rotateWithStretchDirection = Bool(field["m_RotateWithStretchDirection"], fallback: true);
		renderer.enableGPUInstancing = Bool(field["m_EnableGPUInstancing"], fallback: false);
		if (component.Mesh != null)
		{
			renderer.mesh = component.Mesh;
		}
		if (Bool(field["m_UseCustomVertexStreams"], fallback: false))
		{
			List<ParticleSystemVertexStream> list = new List<ParticleSystemVertexStream>();
			byte[] asByteArray = field["m_VertexStreams.Array"].AsByteArray;
			foreach (byte item in asByteArray)
			{
				list.Add((ParticleSystemVertexStream)item);
			}
			renderer.SetActiveVertexStreams(list);
		}
	}

	private static void ReadParticleSystem(AssetTypeValueField f, ParticleSystem ps)
	{
		ParticleSystem.MainState mainState = ps.mainState;
		mainState.duration = f["lengthInSec"].AsFloat;
		mainState.simulationSpeed = f["simulationSpeed"].AsFloat;
		mainState.stopAction = (ParticleSystemStopAction)f["stopAction"].AsInt;
		mainState.cullingMode = (ParticleSystemCullingMode)f["cullingMode"].AsInt;
		mainState.ringBufferMode = (ParticleSystemRingBufferMode)f["ringBufferMode"].AsInt;
		mainState.emitterVelocityMode = (ParticleSystemEmitterVelocityMode)f["emitterVelocityMode"].AsInt;
		mainState.loop = f["looping"].AsBool;
		mainState.prewarm = f["prewarm"].AsBool;
		mainState.playOnAwake = f["playOnAwake"].AsBool;
		mainState.useUnscaledTime = f["useUnscaledTime"].AsBool;
		mainState.startDelay = ReadCurve(f["startDelay"]);
		mainState.simulationSpace = (ParticleSystemSimulationSpace)f["moveWithTransform"].AsInt;
		mainState.scalingMode = (ParticleSystemScalingMode)f["scalingMode"].AsInt;
		ps.useAutoRandomSeed = f["autoRandomSeed"].AsBool;
		ps.randomSeed = f["randomSeed"].AsUInt;
		AssetTypeValueField assetTypeValueField = f["InitialModule"];
		mainState.startLifetime = ReadCurve(assetTypeValueField["startLifetime"]);
		mainState.startSpeed = ReadCurve(assetTypeValueField["startSpeed"]);
		mainState.startColor = ReadGradient(assetTypeValueField["startColor"]);
		mainState.startSize = (mainState.startSizeX = ReadCurve(assetTypeValueField["startSize"]));
		mainState.startSizeY = ReadCurve(assetTypeValueField["startSizeY"]);
		mainState.startSizeZ = ReadCurve(assetTypeValueField["startSizeZ"]);
		mainState.startRotationX = ReadCurve(assetTypeValueField["startRotationX"]);
		mainState.startRotationY = ReadCurve(assetTypeValueField["startRotationY"]);
		mainState.startRotation = (mainState.startRotationZ = ReadCurve(assetTypeValueField["startRotation"]));
		mainState.flipRotation = assetTypeValueField["randomizeRotationDirection"].AsFloat;
		mainState.maxParticles = assetTypeValueField["maxNumParticles"].AsInt;
		mainState.startSize3D = assetTypeValueField["size3D"].AsBool;
		mainState.startRotation3D = assetTypeValueField["rotation3D"].AsBool;
		mainState.gravityModifier = ReadCurve(assetTypeValueField["gravityModifier"]);
		AssetTypeValueField assetTypeValueField2 = f["ShapeModule"];
		ParticleSystem.ShapeState shapeState = ps.shapeState;
		shapeState.enabled = assetTypeValueField2["enabled"].AsBool;
		shapeState.shapeType = (ParticleSystemShapeType)assetTypeValueField2["type"].AsInt;
		shapeState.angle = assetTypeValueField2["angle"].AsFloat;
		shapeState.length = assetTypeValueField2["length"].AsFloat;
		shapeState.boxThickness = ReadVector3(assetTypeValueField2["boxThickness"]);
		shapeState.radiusThickness = assetTypeValueField2["radiusThickness"].AsFloat;
		shapeState.donutRadius = assetTypeValueField2["donutRadius"].AsFloat;
		shapeState.position = ReadVector3(assetTypeValueField2["m_Position"]);
		shapeState.rotation = ReadVector3(assetTypeValueField2["m_Rotation"]);
		shapeState.scale = ReadVector3(assetTypeValueField2["m_Scale"]);
		shapeState.meshShapeType = (ParticleSystemMeshShapeType)assetTypeValueField2["placementMode"].AsInt;
		shapeState.normalOffset = assetTypeValueField2["m_MeshNormalOffset"].AsFloat;
		shapeState.useMeshColors = Bool(assetTypeValueField2["m_UseMeshColors"], fallback: true);
		shapeState.alignToDirection = assetTypeValueField2["alignToDirection"].AsBool;
		shapeState.randomDirectionAmount = assetTypeValueField2["randomDirectionAmount"].AsFloat;
		shapeState.sphericalDirectionAmount = assetTypeValueField2["sphericalDirectionAmount"].AsFloat;
		shapeState.randomPositionAmount = assetTypeValueField2["randomPositionAmount"].AsFloat;
		shapeState.radius = assetTypeValueField2["radius.value"].AsFloat;
		shapeState.arc = assetTypeValueField2["arc.value"].AsFloat;
		AssetTypeValueField assetTypeValueField3 = f["EmissionModule"];
		ParticleSystem.EmissionState emissionState = ps.emissionState;
		emissionState.enabled = assetTypeValueField3["enabled"].AsBool;
		emissionState.rateOverTime = ReadCurve(assetTypeValueField3["rateOverTime"]);
		emissionState.rateOverDistance = ReadCurve(assetTypeValueField3["rateOverDistance"]);
		List<ParticleSystem.Burst> list = new List<ParticleSystem.Burst>();
		foreach (AssetTypeValueField child in assetTypeValueField3["m_Bursts.Array"].Children)
		{
			list.Add(new ParticleSystem.Burst
			{
				time = child["time"].AsFloat,
				count = ReadCurve(child["countCurve"]),
				cycleCount = child["cycleCount"].AsInt,
				repeatInterval = child["repeatInterval"].AsFloat,
				probability = (child["probability"].IsDummy ? 1f : child["probability"].AsFloat)
			});
		}
		emissionState.bursts = list.ToArray();
		AssetTypeValueField assetTypeValueField4 = f["SizeModule"];
		ParticleSystem.SizeOverLifetimeState sizeState = ps.sizeState;
		sizeState.enabled = assetTypeValueField4["enabled"].AsBool;
		sizeState.size = (sizeState.x = ReadCurve(assetTypeValueField4["curve"]));
		sizeState.y = ReadCurve(assetTypeValueField4["y"]);
		sizeState.z = ReadCurve(assetTypeValueField4["z"]);
		sizeState.separateAxes = assetTypeValueField4["separateAxes"].AsBool;
		AssetTypeValueField assetTypeValueField5 = f["RotationModule"];
		ParticleSystem.RotationOverLifetimeState rotationState = ps.rotationState;
		rotationState.enabled = assetTypeValueField5["enabled"].AsBool;
		rotationState.x = ReadCurve(assetTypeValueField5["x"]);
		rotationState.y = ReadCurve(assetTypeValueField5["y"]);
		rotationState.z = ReadCurve(assetTypeValueField5["curve"]);
		rotationState.separateAxes = assetTypeValueField5["separateAxes"].AsBool;
		AssetTypeValueField assetTypeValueField6 = f["ColorModule"];
		ps.colorState.enabled = assetTypeValueField6["enabled"].AsBool;
		ps.colorState.color = ReadGradient(assetTypeValueField6["gradient"]);
		AssetTypeValueField assetTypeValueField7 = f["UVModule"];
		ParticleSystem.TextureSheetAnimationState textureSheetState = ps.textureSheetState;
		textureSheetState.enabled = assetTypeValueField7["enabled"].AsBool;
		textureSheetState.mode = (ParticleSystemAnimationMode)assetTypeValueField7["mode"].AsInt;
		textureSheetState.timeMode = (ParticleSystemAnimationTimeMode)assetTypeValueField7["timeMode"].AsInt;
		textureSheetState.fps = assetTypeValueField7["fps"].AsFloat;
		textureSheetState.frameOverTime = ReadCurve(assetTypeValueField7["frameOverTime"]);
		textureSheetState.startFrame = ReadCurve(assetTypeValueField7["startFrame"]);
		textureSheetState.speedRange = new Vector2(assetTypeValueField7["speedRange.x"].AsFloat, assetTypeValueField7["speedRange.y"].AsFloat);
		textureSheetState.numTilesX = assetTypeValueField7["tilesX"].AsInt;
		textureSheetState.numTilesY = assetTypeValueField7["tilesY"].AsInt;
		textureSheetState.animation = (ParticleSystemAnimationType)assetTypeValueField7["animationType"].AsInt;
		textureSheetState.rowIndex = assetTypeValueField7["rowIndex"].AsInt;
		textureSheetState.cycleCount = (int)assetTypeValueField7["cycles"].AsFloat;
		textureSheetState.uvChannelMask = assetTypeValueField7["uvChannelMask"].AsInt;
		textureSheetState.rowMode = (ParticleSystemAnimationRowMode)assetTypeValueField7["rowMode"].AsInt;
		if (textureSheetState.enabled && textureSheetState.mode != 0)
		{
			StubWarner.LogStub("ReadParticleSystem", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Internal\\AssetBundlePrefabs.cs", 397);
		}
		AssetTypeValueField assetTypeValueField8 = f["VelocityModule"];
		ParticleSystem.VelocityOverLifetimeState velocityState = ps.velocityState;
		velocityState.enabled = assetTypeValueField8["enabled"].AsBool;
		velocityState.x = ReadCurve(assetTypeValueField8["x"]);
		velocityState.y = ReadCurve(assetTypeValueField8["y"]);
		velocityState.z = ReadCurve(assetTypeValueField8["z"]);
		velocityState.orbitalX = ReadCurve(assetTypeValueField8["orbitalX"]);
		velocityState.orbitalY = ReadCurve(assetTypeValueField8["orbitalY"]);
		velocityState.orbitalZ = ReadCurve(assetTypeValueField8["orbitalZ"]);
		velocityState.orbitalOffsetX = ReadCurve(assetTypeValueField8["orbitalOffsetX"]);
		velocityState.orbitalOffsetY = ReadCurve(assetTypeValueField8["orbitalOffsetY"]);
		velocityState.orbitalOffsetZ = ReadCurve(assetTypeValueField8["orbitalOffsetZ"]);
		velocityState.radial = ReadCurve(assetTypeValueField8["radial"]);
		velocityState.speedModifier = ReadCurve(assetTypeValueField8["speedModifier"]);
		velocityState.space = (assetTypeValueField8["inWorldSpace"].AsBool ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local);
		AssetTypeValueField assetTypeValueField9 = f["ClampVelocityModule"];
		ParticleSystem.LimitVelocityOverLifetimeState limitVelocityState = ps.limitVelocityState;
		limitVelocityState.enabled = assetTypeValueField9["enabled"].AsBool;
		limitVelocityState.limitX = ReadCurve(assetTypeValueField9["x"]);
		limitVelocityState.limitY = ReadCurve(assetTypeValueField9["y"]);
		limitVelocityState.limitZ = ReadCurve(assetTypeValueField9["z"]);
		limitVelocityState.limit = ReadCurve(assetTypeValueField9["magnitude"]);
		limitVelocityState.separateAxes = assetTypeValueField9["separateAxis"].AsBool;
		limitVelocityState.space = (assetTypeValueField9["inWorldSpace"].AsBool ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local);
		limitVelocityState.multiplyDragByParticleSize = Bool(assetTypeValueField9["multiplyDragByParticleSize"], fallback: true);
		limitVelocityState.multiplyDragByParticleVelocity = Bool(assetTypeValueField9["multiplyDragByParticleVelocity"], fallback: true);
		limitVelocityState.dampen = assetTypeValueField9["dampen"].AsFloat;
		limitVelocityState.drag = ReadCurve(assetTypeValueField9["drag"]);
		AssetTypeValueField assetTypeValueField10 = f["NoiseModule"];
		ParticleSystem.NoiseState noiseState = ps.noiseState;
		noiseState.enabled = assetTypeValueField10["enabled"].AsBool;
		noiseState.strength = (noiseState.strengthX = ReadCurve(assetTypeValueField10["strength"]));
		noiseState.strengthY = ReadCurve(assetTypeValueField10["strengthY"]);
		noiseState.strengthZ = ReadCurve(assetTypeValueField10["strengthZ"]);
		noiseState.separateAxes = assetTypeValueField10["separateAxes"].AsBool;
		noiseState.frequency = assetTypeValueField10["frequency"].AsFloat;
		noiseState.damping = assetTypeValueField10["damping"].AsBool;
		noiseState.octaveCount = assetTypeValueField10["octaves"].AsInt;
		noiseState.octaveMultiplier = assetTypeValueField10["octaveMultiplier"].AsFloat;
		noiseState.octaveScale = assetTypeValueField10["octaveScale"].AsFloat;
		noiseState.quality = (ParticleSystemNoiseQuality)assetTypeValueField10["quality"].AsInt;
		noiseState.scrollSpeed = ReadCurve(assetTypeValueField10["scrollSpeed"]);
		noiseState.remapEnabled = assetTypeValueField10["remapEnabled"].AsBool;
		noiseState.positionAmount = ReadCurve(assetTypeValueField10["positionAmount"]);
		noiseState.rotationAmount = ReadCurve(assetTypeValueField10["rotationAmount"]);
		noiseState.sizeAmount = ReadCurve(assetTypeValueField10["sizeAmount"]);
		AssetTypeValueField assetTypeValueField11 = f["CollisionModule"];
		ParticleSystem.CollisionState collisionState = ps.collisionState;
		collisionState.enabled = assetTypeValueField11["enabled"].AsBool;
		collisionState.type = (ParticleSystemCollisionType)assetTypeValueField11["type"].AsInt;
		collisionState.mode = (ParticleSystemCollisionMode)assetTypeValueField11["collisionMode"].AsInt;
		collisionState.dampen = ReadCurve(assetTypeValueField11["m_Dampen"]);
		collisionState.bounce = ReadCurve(assetTypeValueField11["m_Bounce"]);
		collisionState.lifetimeLoss = ReadCurve(assetTypeValueField11["m_EnergyLossOnCollision"]);
		collisionState.collidesWith = (int)assetTypeValueField11["collidesWith.m_Bits"].AsUInt;
		collisionState.sendCollisionMessages = assetTypeValueField11["collisionMessages"].AsBool;
		string[] array = new string[12]
		{
			"InheritVelocityModule", "LifetimeByEmitterSpeedModule", "ForceModule", "ExternalForcesModule", "SizeBySpeedModule", "RotationBySpeedModule", "ColorBySpeedModule", "TriggerModule", "SubModule", "LightsModule",
			"TrailModule", "CustomDataModule"
		};
		foreach (string text in array)
		{
			if (Bool(f[text + ".enabled"], fallback: false) && WarnedModules.Add((ps.gameObject.name, text)))
			{
				Debug.LogWarning($"ParticleSystem '{ps.gameObject.name}': {text} is enabled but not simulated.");
			}
		}
	}

	private static ParticleSystem.MinMaxCurve ReadCurve(AssetTypeValueField f)
	{
		if (f.IsDummy)
		{
			return default(ParticleSystem.MinMaxCurve);
		}
		float asFloat = f["scalar"].AsFloat;
		return (ParticleSystemCurveMode)f["minMaxState"].AsInt switch
		{
			ParticleSystemCurveMode.TwoConstants => new ParticleSystem.MinMaxCurve(f["minScalar"].AsFloat, asFloat), 
			ParticleSystemCurveMode.Curve => new ParticleSystem.MinMaxCurve(asFloat, ReadAnimationCurve(f["maxCurve"])), 
			ParticleSystemCurveMode.TwoCurves => new ParticleSystem.MinMaxCurve(asFloat, ReadAnimationCurve(f["minCurve"]), ReadAnimationCurve(f["maxCurve"])), 
			_ => new ParticleSystem.MinMaxCurve(asFloat), 
		};
	}

	private static AnimationCurve ReadAnimationCurve(AssetTypeValueField f)
	{
		List<Keyframe> list = new List<Keyframe>();
		foreach (AssetTypeValueField child in f["m_Curve.Array"].Children)
		{
			list.Add(new Keyframe(child["time"].AsFloat, child["value"].AsFloat, child["inSlope"].AsFloat, child["outSlope"].AsFloat));
		}
		return new AnimationCurve(list.ToArray());
	}

	private static ParticleSystem.MinMaxGradient ReadGradient(AssetTypeValueField f)
	{
		if (f.IsDummy)
		{
			return new ParticleSystem.MinMaxGradient(Color.white);
		}
		switch ((ParticleSystemGradientMode)f["minMaxState"].AsInt)
		{
		case ParticleSystemGradientMode.Gradient:
			return new ParticleSystem.MinMaxGradient(ReadColorGradient(f["maxGradient"]));
		case ParticleSystemGradientMode.TwoColors:
			return new ParticleSystem.MinMaxGradient(ReadColor(f["minColor"]), ReadColor(f["maxColor"]));
		case ParticleSystemGradientMode.TwoGradients:
			return new ParticleSystem.MinMaxGradient(ReadColorGradient(f["minGradient"]), ReadColorGradient(f["maxGradient"]));
		case ParticleSystemGradientMode.RandomColor:
		{
			ParticleSystem.MinMaxGradient result = new ParticleSystem.MinMaxGradient(ReadColorGradient(f["maxGradient"]));
			result.mode = ParticleSystemGradientMode.RandomColor;
			return result;
		}
		default:
			return new ParticleSystem.MinMaxGradient(ReadColor(f["maxColor"]));
		}
	}

	private static Gradient ReadColorGradient(AssetTypeValueField f)
	{
		GradientColorKey[] array = new GradientColorKey[Math.Clamp(f["m_NumColorKeys"].AsInt, 0, 8)];
		for (int i = 0; i < array.Length; i++)
		{
			Color color = ReadColor(f["key" + i]);
			array[i] = new GradientColorKey(new Color(color.r, color.g, color.b, 1f), (float)f["ctime" + i].AsInt / 65535f);
		}
		GradientAlphaKey[] array2 = new GradientAlphaKey[Math.Clamp(f["m_NumAlphaKeys"].AsInt, 0, 8)];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = new GradientAlphaKey(f["key" + j + ".a"].AsFloat, (float)f["atime" + j].AsInt / 65535f);
		}
		Gradient gradient = new Gradient();
		gradient.mode = (GradientMode)f["m_Mode"].AsInt;
		gradient.SetKeys(array, array2);
		return gradient;
	}

	private static Color ReadColor(AssetTypeValueField f)
	{
		return new Color(f["r"].AsFloat, f["g"].AsFloat, f["b"].AsFloat, f["a"].AsFloat);
	}

	private static Vector3 ReadVector3(AssetTypeValueField f)
	{
		if (!f.IsDummy)
		{
			return new Vector3(f["x"].AsFloat, f["y"].AsFloat, f["z"].AsFloat);
		}
		return Vector3.zero;
	}

	private static Quaternion ReadQuaternion(AssetTypeValueField f)
	{
		return new Quaternion(f["x"].AsFloat, f["y"].AsFloat, f["z"].AsFloat, f["w"].AsFloat);
	}

	private static bool Bool(AssetTypeValueField f, bool fallback)
	{
		if (!f.IsDummy)
		{
			AssetTypeValue value = f.Value;
			if (value == null || value.ValueType != AssetValueType.Bool)
			{
				return f.AsInt != 0;
			}
			return f.AsBool;
		}
		return fallback;
	}

	private AssetBundleReader(BundleFileInstance bundle, string bundleName, string filePath)
	{
		_bundle = bundle;
		_bundleName = bundleName;
		_filePath = filePath;
	}

	public static AssetBundleReader Open(Stream stream, string path, bool fromFile = false)
	{
		AssetsManager assetsManager = new AssetsManager();
		bool flag = fromFile && stream.Position == 0;
		BundleFileInstance bundleFileInstance;
		if (flag && BundleCache.TryOpen(assetsManager, path, out var bundle))
		{
			stream.Dispose();
			bundleFileInstance = bundle;
		}
		else
		{
			bundleFileInstance = assetsManager.LoadBundleFile(stream, path, unpackIfPacked: false);
			if (bundleFileInstance.file.DataIsCompressed)
			{
				string tempFileName = Path.GetTempFileName();
				FileStream fileStream = new FileStream(tempFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete, 65536, FileOptions.DeleteOnClose);
				bundleFileInstance.file = BundleHelper.UnpackBundleToStream(bundleFileInstance.file, fileStream);
				fileStream.Flush();
				FileStream fileStream2 = (flag ? new FileStream(tempFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete, 65536) : null);
				if (!OperatingSystem.IsWindows())
				{
					File.Delete(tempFileName);
				}
				if (fileStream2 != null)
				{
					BundleCache.StoreLater(path, fileStream2);
				}
			}
		}
		AssetBundleReader assetBundleReader = new AssetBundleReader(bundleFileInstance, Path.GetFileName(path), fromFile ? path : null);
		assetBundleReader.Init(assetsManager);
		return assetBundleReader;
	}

	private void Init(AssetsManager manager)
	{
		_manager = manager;
		List<AssetBundleDirectoryInfo> directoryInfos = _bundle.file.BlockAndDirInfo.DirectoryInfos;
		for (int i = 0; i < directoryInfos.Count; i++)
		{
			if (_bundle.file.IsAssetsFile(i) && !directoryInfos[i].Name.EndsWith(".resS", StringComparison.OrdinalIgnoreCase) && !directoryInfos[i].Name.EndsWith(".resource", StringComparison.OrdinalIgnoreCase))
			{
				try
				{
					_files.Add(Manager.LoadAssetsFileFromBundle(_bundle, i, loadDeps: true));
				}
				catch (Exception ex)
				{
					Debug.LogWarning($"AssetBundle '{_bundleName}': could not read assets file '{directoryInfos[i].Name}': {ex.Message}");
				}
			}
		}
		foreach (AssetsFileInstance file in _files)
		{
			foreach (AssetFileInfo item in file.file.GetAssetsOfType(AssetClassID.AssetBundle))
			{
				AssetTypeValueField baseField;
				try
				{
					baseField = Manager.GetBaseField(file, item);
				}
				catch (Exception ex2)
				{
					Debug.LogWarning("AssetBundle '" + _bundleName + "': could not read its container: " + ex2.Message);
					continue;
				}
				AssetTypeValueField assetTypeValueField = baseField["m_Container"];
				if (assetTypeValueField.IsDummy)
				{
					continue;
				}
				foreach (AssetTypeValueField child in assetTypeValueField["Array"].Children)
				{
					string asString = child["first"].AsString;
					AssetTypeValueField assetTypeValueField2 = child["second"]["asset"];
					if (TryResolvePPtr(file, assetTypeValueField2["m_FileID"].AsInt, assetTypeValueField2["m_PathID"].AsLong, out var target))
					{
						Add(asString, target);
					}
				}
			}
		}
		if (_container.Count != 0)
		{
			return;
		}
		foreach (AssetsFileInstance file2 in _files)
		{
			foreach (AssetFileInfo assetInfo in file2.file.AssetInfos)
			{
				AssetClassID typeId = (AssetClassID)assetInfo.TypeId;
				if (!(ClassToType(typeId) == null))
				{
					string text = ReadName(file2, assetInfo);
					if (!string.IsNullOrEmpty(text))
					{
						Add(text, new AssetRef(file2, assetInfo.PathId, typeId));
					}
				}
			}
		}
	}

	private void Add(string key, AssetRef target)
	{
		_container.Add(new KeyValuePair<string, AssetRef>(key, target));
		if (!_byPath.TryGetValue(key, out var value))
		{
			value = (_byPath[key] = new List<AssetRef>());
		}
		value.Add(target);
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(key);
		if (!string.IsNullOrEmpty(fileNameWithoutExtension) && !string.Equals(fileNameWithoutExtension, key, StringComparison.OrdinalIgnoreCase))
		{
			if (!_byName.TryGetValue(fileNameWithoutExtension, out var value2))
			{
				value2 = (_byName[fileNameWithoutExtension] = new List<AssetRef>());
			}
			value2.Add(target);
		}
	}

	private bool TryResolvePPtr(AssetsFileInstance relativeTo, int fileId, long pathId, out AssetRef target)
	{
		target = default(AssetRef);
		if (pathId == 0L)
		{
			return false;
		}
		if (fileId != 0)
		{
			try
			{
				AssetExternal extAsset = Manager.GetExtAsset(relativeTo, fileId, pathId, onlyGetInfo: true);
				if (extAsset.file == null || extAsset.info == null)
				{
					return false;
				}
				target = new AssetRef(extAsset.file, pathId, (AssetClassID)extAsset.info.TypeId);
				return true;
			}
			catch
			{
				return false;
			}
		}
		AssetFileInfo assetInfo = relativeTo.file.GetAssetInfo(pathId);
		if (assetInfo == null)
		{
			return false;
		}
		target = new AssetRef(relativeTo, pathId, (AssetClassID)assetInfo.TypeId);
		return true;
	}

	public bool Contains(string name)
	{
		lock (_sync)
		{
			return Lookup(name) != null;
		}
	}

	private List<AssetRef> Lookup(string name)
	{
		if (_byPath.TryGetValue(name, out var value))
		{
			return value;
		}
		string text = name.Replace('\\', '/');
		if (_byPath.TryGetValue(text, out value))
		{
			return value;
		}
		if (text.IndexOf('/') < 0 && _byName.TryGetValue(Path.GetFileNameWithoutExtension(text), out value))
		{
			return value;
		}
		return null;
	}

	public UnityEngine.Object Load(string name, Type type)
	{
		lock (_sync)
		{
			List<AssetRef> list = Lookup(name);
			if (list == null)
			{
				return null;
			}
			AssetRef? assetRef = null;
			foreach (AssetRef item in list)
			{
				Type type2 = TypeOf(item);
				if (!(type2 == null) && type.IsAssignableFrom(type2) && (!assetRef.HasValue || (assetRef.Value.Class == AssetClassID.Sprite && item.Class != AssetClassID.Sprite)))
				{
					assetRef = item;
				}
			}
			return (!assetRef.HasValue) ? null : Materialize(assetRef.Value);
		}
	}

	public UnityEngine.Object[] LoadWithSubAssets(string name, Type type)
	{
		lock (_sync)
		{
			List<AssetRef> list = Lookup(name);
			if (list == null)
			{
				return Array.Empty<UnityEngine.Object>();
			}
			List<UnityEngine.Object> list2 = new List<UnityEngine.Object>();
			foreach (AssetRef item in list)
			{
				Type type2 = TypeOf(item);
				if (!(type2 == null) && type.IsAssignableFrom(type2))
				{
					UnityEngine.Object @object = Materialize(item);
					if (@object != null)
					{
						list2.Add(@object);
					}
				}
			}
			return list2.ToArray();
		}
	}

	public UnityEngine.Object[] LoadAll(Type type)
	{
		lock (_sync)
		{
			List<UnityEngine.Object> list = new List<UnityEngine.Object>();
			HashSet<(AssetsFileInstance, long)> hashSet = new HashSet<(AssetsFileInstance, long)>();
			foreach (KeyValuePair<string, AssetRef> item in _container)
			{
				AssetRef value = item.Value;
				if (!hashSet.Add((value.File, value.PathId)))
				{
					continue;
				}
				Type type2 = TypeOf(value);
				if (!(type2 == null) && type.IsAssignableFrom(type2))
				{
					UnityEngine.Object @object = Materialize(value);
					if (@object != null)
					{
						list.Add(@object);
					}
				}
			}
			return list.ToArray();
		}
	}

	private UnityEngine.Object Materialize(AssetRef asset)
	{
		(AssetsFileInstance, long) tuple = (asset.File, asset.PathId);
		if (_loaded.TryGetValue(tuple, out var value))
		{
			if (value != null)
			{
				return value;
			}
			_loaded.Remove(tuple);
		}
		if (_failed.Contains(tuple))
		{
			return null;
		}
		UnityEngine.Object @object = null;
		try
		{
			AssetFileInfo assetInfo = asset.File.file.GetAssetInfo(asset.PathId);
			if (assetInfo != null)
			{
				AssetTypeValueField baseField = Manager.GetBaseField(asset.File, assetInfo);
				@object = asset.Class switch
				{
					AssetClassID.Texture2D => BuildTexture(asset.File, baseField), 
					AssetClassID.Sprite => BuildSprite(asset.File, baseField), 
					AssetClassID.AudioClip => BuildAudioClip(baseField), 
					AssetClassID.VideoClip => BuildVideoClip(baseField), 
					AssetClassID.TextAsset => BuildTextAsset(baseField), 
					AssetClassID.Shader => BuildShader(asset.PathId, baseField), 
					AssetClassID.Material => BuildMaterial(asset.File, baseField), 
					AssetClassID.Font => BuildFont(baseField), 
					AssetClassID.Mesh => BuildMesh(baseField), 
					AssetClassID.MonoBehaviour => BuildScriptObject(asset.File, baseField), 
					AssetClassID.GameObject => BuildGameObject(asset.File, baseField), 
					_ => Unsupported(asset.Class, baseField), 
				};
			}
		}
		catch (Exception value2)
		{
			Debug.LogError($"AssetBundle '{_bundleName}': failed to load {asset.Class} asset (path id {asset.PathId}): {value2}");
		}
		if (@object != null)
		{
			_loaded[tuple] = @object;
		}
		else
		{
			_failed.Add(tuple);
		}
		return @object;
	}

	private UnityEngine.Object Unsupported(AssetClassID cls, AssetTypeValueField bf)
	{
		string text = (bf["m_Name"].IsDummy ? "?" : bf["m_Name"].AsString);
		if (_warnedClasses.Add(cls.ToString() + ":" + text))
		{
			Debug.LogWarning($"AssetBundle '{_bundleName}': {cls} asset '{text}' is not implemented, returning null.");
		}
		return null;
	}

	private UnityEngine.Object BuildFont(AssetTypeValueField bf)
	{
		AssetTypeValueField assetTypeValueField = bf["m_FontData.Array"];
		if (assetTypeValueField.IsDummy || assetTypeValueField.Children.Count == 0)
		{
			return Unsupported(AssetClassID.Font, bf);
		}
		byte[] array = new byte[assetTypeValueField.Children.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)assetTypeValueField.Children[i].AsSByte;
		}
		Font font = new Font();
		font.name = bf["m_Name"].AsString;
		AssetTypeValueField assetTypeValueField2 = bf["m_FontRenderingMode"];
		font.SetFontData(array, bf["m_FontSize"].AsFloat, (!assetTypeValueField2.IsDummy) ? assetTypeValueField2.AsInt : 0);
		return font;
	}

	private UnityEngine.Texture2D BuildTexture(AssetsFileInstance file, AssetTypeValueField bf)
	{
		TextureFile textureFile = TextureFile.ReadTextureFile(bf);
		byte[] array = textureFile.FillPictureData(file);
		if (array == null || array.Length == 0)
		{
			Debug.LogWarning($"AssetBundle '{_bundleName}': texture '{textureFile.m_Name}' has no pixel data.");
			return null;
		}
		UnityEngine.Texture2D texture2D = AssetBundleTextures.Create(textureFile, array, _bundleName);
		if (texture2D == null)
		{
			return null;
		}
		texture2D.name = textureFile.m_Name;
		UnityEngine.Texture2D texture2D2 = texture2D;
		texture2D2.filterMode = textureFile.m_TextureSettings.m_FilterMode switch
		{
			0 => UnityEngine.FilterMode.Point, 
			2 => UnityEngine.FilterMode.Trilinear, 
			_ => UnityEngine.FilterMode.Bilinear, 
		};
		texture2D.anisoLevel = Math.Max(1, textureFile.m_TextureSettings.m_Aniso);
		texture2D2 = texture2D;
		texture2D2.wrapMode = textureFile.m_TextureSettings.m_WrapU switch
		{
			1 => UnityEngine.TextureWrapMode.Clamp, 
			2 => UnityEngine.TextureWrapMode.Mirror, 
			3 => UnityEngine.TextureWrapMode.MirrorOnce, 
			_ => UnityEngine.TextureWrapMode.Repeat, 
		};
		return texture2D;
	}

	private Sprite BuildSprite(AssetsFileInstance file, AssetTypeValueField bf)
	{
		AssetTypeValueField assetTypeValueField = bf["m_RD"];
		UnityEngine.Texture2D texture = null;
		if (!assetTypeValueField.IsDummy)
		{
			AssetTypeValueField assetTypeValueField2 = assetTypeValueField["texture"];
			if (TryResolvePPtr(file, assetTypeValueField2["m_FileID"].AsInt, assetTypeValueField2["m_PathID"].AsLong, out var target))
			{
				texture = Materialize(target) as UnityEngine.Texture2D;
			}
		}
		Rect rect = ReadRect(bf["m_Rect"]);
		Rect rect2 = (assetTypeValueField.IsDummy ? rect : ReadRect(assetTypeValueField["textureRect"]));
		AssetTypeValueField assetTypeValueField3 = bf["m_Pivot"];
		AssetTypeValueField assetTypeValueField4 = bf["m_Border"];
		float num = (bf["m_PixelsToUnits"].IsDummy ? 100f : bf["m_PixelsToUnits"].AsFloat);
		AssetTypeValueField assetTypeValueField5 = (assetTypeValueField.IsDummy ? null : assetTypeValueField["textureRectOffset"]);
		return new Sprite
		{
			name = bf["m_Name"].AsString,
			texture = texture,
			rect = rect,
			textureRect = ((rect2.width > 0f) ? rect2 : rect),
			textureRectOffset = (assetTypeValueField5.IsDummy ? Vector2.zero : new Vector2(assetTypeValueField5["x"].AsFloat, assetTypeValueField5["y"].AsFloat)),
			pivot = (assetTypeValueField3.IsDummy ? new Vector2(0.5f, 0.5f) : new Vector2(assetTypeValueField3["x"].AsFloat, assetTypeValueField3["y"].AsFloat)),
			pixelsPerUnit = ((num <= 0f) ? 100f : num),
			border = (assetTypeValueField4.IsDummy ? Vector4.zero : new Vector4(assetTypeValueField4["x"].AsFloat, assetTypeValueField4["y"].AsFloat, assetTypeValueField4["z"].AsFloat, assetTypeValueField4["w"].AsFloat))
		};
	}

	private VideoClip BuildVideoClip(AssetTypeValueField bf)
	{
		string asString = bf["m_Name"].AsString;
		AssetTypeValueField assetTypeValueField = bf["m_ExternalResources"];
		if (assetTypeValueField.IsDummy || assetTypeValueField["m_Size"].AsULong == 0L)
		{
			Debug.LogWarning($"AssetBundle '{_bundleName}': video clip '{asString}' has no media data.");
			return null;
		}
		string asString2 = assetTypeValueField["m_Source"].AsString;
		string fileName = Path.GetFileName(asString2.StartsWith("archive:/", StringComparison.Ordinal) ? asString2.Substring(9) : asString2);
		AssetBundleDirectoryInfo dirInfo = BundleHelper.GetDirInfo(_bundle.file, fileName);
		if (dirInfo == null)
		{
			Debug.LogWarning($"AssetBundle '{_bundleName}': video clip '{asString}' references missing resource '{asString2}'.");
			return null;
		}
		AssetsFileReader dataReader = _bundle.file.DataReader;
		dataReader.Position = dirInfo.Offset + (long)assetTypeValueField["m_Offset"].AsULong;
		return new VideoClip
		{
			name = asString,
			Data = dataReader.ReadBytes((int)assetTypeValueField["m_Size"].AsULong)
		};
	}

	private AudioClip BuildAudioClip(AssetTypeValueField bf)
	{
		string asString = bf["m_Name"].AsString;
		byte[] array = null;
		AssetTypeValueField assetTypeValueField = bf["m_Resource"];
		if (!assetTypeValueField.IsDummy && assetTypeValueField["m_Size"].AsULong != 0)
		{
			string asString2 = assetTypeValueField["m_Source"].AsString;
			string fileName = Path.GetFileName(asString2.StartsWith("archive:/", StringComparison.Ordinal) ? asString2.Substring(9) : asString2);
			AssetBundleDirectoryInfo dirInfo = BundleHelper.GetDirInfo(_bundle.file, fileName);
			if (dirInfo == null)
			{
				Debug.LogWarning($"AssetBundle '{_bundleName}': audio clip '{asString}' references missing resource '{asString2}'.");
				return null;
			}
			AssetsFileReader dataReader = _bundle.file.DataReader;
			dataReader.Position = dirInfo.Offset + (long)assetTypeValueField["m_Offset"].AsULong;
			array = dataReader.ReadBytes((int)assetTypeValueField["m_Size"].AsULong);
		}
		else if (!bf["m_AudioData"].IsDummy)
		{
			array = bf["m_AudioData"].AsByteArray;
		}
		if (array == null || array.Length == 0)
		{
			Debug.LogWarning($"AssetBundle '{_bundleName}': audio clip '{asString}' has no sample data.");
			return null;
		}
		return AudioClip.LoadFromMemory(array, asString);
	}

	private UnityEngine.Shader BuildShader(long pathId, AssetTypeValueField bf)
	{
		string text = ShaderName(bf);
		string failure;
		UnityShaderAsset unityShaderAsset = UnityShaderAsset.Read(bf, out failure);
		if (unityShaderAsset == null && _filePath != null && UnityShaderAsset.VulkanPlatformIndex(bf) < 0)
		{
			unityShaderAsset = ReadLinuxSiblingShader(pathId, text, out failure);
		}
		if (unityShaderAsset == null)
		{
			Debug.LogWarning($"AssetBundle '{_bundleName}': shader '{text}' can't be used ({failure}), it draws with the engine default.");
			return UnityEngine.Shader.Unsupported(text);
		}
		Waylith.Rendering.Shader[] extraPasses;
		Waylith.Rendering.Shader shader = AssetBundleShaders.Build(unityShaderAsset, _bundleName, out extraPasses);
		if (shader != null)
		{
			return UnityEngine.Shader.FromEngineShader(unityShaderAsset.Name, shader, extraPasses);
		}
		return UnityEngine.Shader.Unsupported(text);
	}

	private static string ShaderName(AssetTypeValueField bf)
	{
		if (!bf["m_ParsedForm"]["m_Name"].IsDummy)
		{
			return bf["m_ParsedForm"]["m_Name"].AsString;
		}
		return "?";
	}

	private UnityShaderAsset ReadLinuxSiblingShader(long pathId, string name, out string failure)
	{
		if (!_linuxSiblingProbed)
		{
			_linuxSiblingProbed = true;
			string text = LinuxSiblingPath(_filePath);
			if (text != null && File.Exists(text))
			{
				FileStream fileStream = null;
				try
				{
					fileStream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read);
					_linuxSibling = Open(fileStream, text);
					VerboseLog.Log($"AssetBundle '{_bundleName}': has no Vulkan shaders, taking them from adjacent Linux bundle '{text}'.");
				}
				catch (Exception ex)
				{
					fileStream?.Dispose();
					Debug.LogWarning($"AssetBundle '{_bundleName}': could not read adjacent Linux bundle '{text}': {ex.Message}");
				}
			}
		}
		if (_linuxSibling == null)
		{
			failure = "no Vulkan variant in this bundle and no adjacent Linux bundle beside it";
			return null;
		}
		return _linuxSibling.ReadShaderAsset(pathId, name, out failure);
	}

	private UnityShaderAsset ReadShaderAsset(long pathId, string name, out string failure)
	{
		lock (_sync)
		{
			AssetTypeValueField assetTypeValueField = null;
			foreach (AssetsFileInstance file in _files)
			{
				AssetFileInfo assetInfo = file.file.GetAssetInfo(pathId);
				if (assetInfo != null && assetInfo.TypeId == 48)
				{
					AssetTypeValueField baseField = Manager.GetBaseField(file, assetInfo);
					if (ShaderName(baseField) == name)
					{
						assetTypeValueField = baseField;
					}
					break;
				}
			}
			if (assetTypeValueField == null)
			{
				if (_shadersByName == null)
				{
					Dictionary<string, AssetRef> dictionary = new Dictionary<string, AssetRef>(StringComparer.Ordinal);
					foreach (AssetsFileInstance file2 in _files)
					{
						foreach (AssetFileInfo item in file2.file.GetAssetsOfType(AssetClassID.Shader))
						{
							dictionary.TryAdd(ShaderName(Manager.GetBaseField(file2, item)), new AssetRef(file2, item.PathId, AssetClassID.Shader));
						}
					}
					_shadersByName = dictionary;
				}
				if (!_shadersByName.TryGetValue(name, out var value))
				{
					failure = "no Vulkan variant in this bundle and no shader of that name in adjacent Linux bundle '" + _bundleName + "'";
					return null;
				}
				assetTypeValueField = Manager.GetBaseField(value.File, value.File.file.GetAssetInfo(value.PathId));
			}
			if (UnityShaderAsset.VulkanPlatformIndex(assetTypeValueField) < 0)
			{
				failure = "neither this bundle nor adjacent Linux Bundle '" + _bundleName + "' has a Vulkan variant (built for GLCore only?)";
				return null;
			}
			UnityShaderAsset unityShaderAsset = UnityShaderAsset.Read(assetTypeValueField, out failure);
			if (unityShaderAsset == null)
			{
				failure = "adjacent Linux bundle '" + _bundleName + "': " + failure;
			}
			return unityShaderAsset;
		}
	}

	private static string LinuxSiblingPath(string path)
	{
		string[] array = path.Split(RuntimeHelpers.CreateSpan<char>((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/));
		int num = array.Length - 1;
		bool flag = false;
		if (array[num].EndsWith("_win", StringComparison.Ordinal) || array[num].EndsWith("_mac", StringComparison.Ordinal))
		{
			string text = array[num];
			array[num] = text.Substring(0, text.Length - 4) + "_linux";
			flag = true;
		}
		for (int i = 0; i < num; i++)
		{
			if (LinuxFolderFor.TryGetValue(array[i], out var value))
			{
				array[i] = value;
				flag = true;
			}
		}
		if (!flag)
		{
			return null;
		}
		return string.Join(Path.DirectorySeparatorChar, array);
	}

	private UnityEngine.Material BuildMaterial(AssetsFileInstance file, AssetTypeValueField bf)
	{
		string asString = bf["m_Name"].AsString;
		AssetTypeValueField assetTypeValueField = bf["m_Shader"];
		UnityEngine.Shader shader = null;
		if (TryResolvePPtr(file, assetTypeValueField["m_FileID"].AsInt, assetTypeValueField["m_PathID"].AsLong, out var target) && target.Class == AssetClassID.Shader)
		{
			shader = Materialize(target) as UnityEngine.Shader;
		}
		if (shader == null || !shader.isSupported)
		{
			Debug.LogWarning($"AssetBundle '{_bundleName}': material '{asString}' has no usable shader, returning null.");
			return null;
		}
		UnityEngine.Material material = new UnityEngine.Material(shader)
		{
			name = asString
		};
		AssetTypeValueField assetTypeValueField2 = bf["m_SavedProperties"];
		foreach (AssetTypeValueField child in assetTypeValueField2["m_TexEnvs"]["Array"].Children)
		{
			string name = PropertyName(child["first"]);
			AssetTypeValueField assetTypeValueField3 = child["second"];
			AssetTypeValueField assetTypeValueField4 = assetTypeValueField3["m_Texture"];
			if (TryResolvePPtr(file, assetTypeValueField4["m_FileID"].AsInt, assetTypeValueField4["m_PathID"].AsLong, out var target2) && Materialize(target2) is UnityEngine.Texture value)
			{
				material.SetTexture(name, value);
			}
			material.SetTextureScale(name, new Vector2(assetTypeValueField3["m_Scale"]["x"].AsFloat, assetTypeValueField3["m_Scale"]["y"].AsFloat));
			material.SetTextureOffset(name, new Vector2(assetTypeValueField3["m_Offset"]["x"].AsFloat, assetTypeValueField3["m_Offset"]["y"].AsFloat));
		}
		foreach (AssetTypeValueField child2 in assetTypeValueField2["m_Floats"]["Array"].Children)
		{
			material.SetFloat(PropertyName(child2["first"]), child2["second"].AsFloat);
		}
		if (!assetTypeValueField2["m_Ints"].IsDummy)
		{
			foreach (AssetTypeValueField child3 in assetTypeValueField2["m_Ints"]["Array"].Children)
			{
				material.SetFloat(PropertyName(child3["first"]), child3["second"].AsInt);
			}
		}
		foreach (AssetTypeValueField child4 in assetTypeValueField2["m_Colors"]["Array"].Children)
		{
			AssetTypeValueField assetTypeValueField5 = child4["second"];
			material.SetColor(PropertyName(child4["first"]), new Color(assetTypeValueField5["r"].AsFloat, assetTypeValueField5["g"].AsFloat, assetTypeValueField5["b"].AsFloat, assetTypeValueField5["a"].AsFloat));
		}
		AssetTypeValueField assetTypeValueField6 = bf["m_CustomRenderQueue"];
		if (!assetTypeValueField6.IsDummy && assetTypeValueField6.AsInt >= 0)
		{
			material.renderQueue = assetTypeValueField6.AsInt;
		}
		return material;
	}

	private static string PropertyName(AssetTypeValueField first)
	{
		if (first.Value == null)
		{
			return first["name"].AsString;
		}
		return first.AsString;
	}

	private static TextAsset BuildTextAsset(AssetTypeValueField bf)
	{
		AssetTypeValueField assetTypeValueField = bf["m_Script"];
		return new TextAsset(assetTypeValueField.IsDummy ? string.Empty : assetTypeValueField.AsString)
		{
			name = bf["m_Name"].AsString
		};
	}

	private static Rect ReadRect(AssetTypeValueField f)
	{
		if (!f.IsDummy)
		{
			return new Rect(f["x"].AsFloat, f["y"].AsFloat, f["width"].AsFloat, f["height"].AsFloat);
		}
		return default(Rect);
	}

	private string ReadName(AssetsFileInstance file, AssetFileInfo info)
	{
		try
		{
			AssetTypeValueField baseField = Manager.GetBaseField(file, info);
			return baseField["m_Name"].IsDummy ? null : baseField["m_Name"].AsString;
		}
		catch
		{
			return null;
		}
	}

	private Type TypeOf(AssetRef asset)
	{
		if (asset.Class != AssetClassID.MonoBehaviour)
		{
			return ClassToType(asset.Class);
		}
		return ScriptType(asset);
	}

	private Type ScriptType(AssetRef asset)
	{
		try
		{
			AssetFileInfo assetInfo = asset.File.file.GetAssetInfo(asset.PathId);
			return (assetInfo == null) ? null : ScriptType(asset.File, Manager.GetBaseField(asset.File, assetInfo));
		}
		catch
		{
			return null;
		}
	}

	private Type ScriptType(AssetsFileInstance file, AssetTypeValueField bf)
	{
		AssetTypeValueField assetTypeValueField = bf["m_Script"];
		if (assetTypeValueField.IsDummy || !TryResolvePPtr(file, assetTypeValueField["m_FileID"].AsInt, assetTypeValueField["m_PathID"].AsLong, out var target) || target.Class != AssetClassID.MonoScript)
		{
			return null;
		}
		AssetTypeValueField baseField = Manager.GetBaseField(target.File, target.File.file.GetAssetInfo(target.PathId));
		string asString = baseField["m_Namespace"].AsString;
		string asString2 = baseField["m_ClassName"].AsString;
		if (asString == "TMPro" && asString2 == "TMP_FontAsset")
		{
			return typeof(TMP_FontAsset);
		}
		return null;
	}

	private UnityEngine.Object BuildScriptObject(AssetsFileInstance file, AssetTypeValueField bf)
	{
		if (ScriptType(file, bf) == typeof(TMP_FontAsset))
		{
			return BuildTmpFontAsset(file, bf);
		}
		return Unsupported(AssetClassID.MonoBehaviour, bf);
	}

	private TMP_FontAsset BuildTmpFontAsset(AssetsFileInstance file, AssetTypeValueField bf)
	{
		AssetTypeValueField assetTypeValueField = bf["m_FaceInfo"];
		string asString = assetTypeValueField["m_FamilyName"].AsString;
		Font font = null;
		AssetTypeValueField assetTypeValueField2 = bf["m_SourceFontFile"];
		if (!assetTypeValueField2.IsDummy && TryResolvePPtr(file, assetTypeValueField2["m_FileID"].AsInt, assetTypeValueField2["m_PathID"].AsLong, out var target) && target.Class == AssetClassID.Font)
		{
			font = Materialize(target) as Font;
		}
		if ((object)font == null)
		{
			font = FindFontByFamily(asString);
		}
		string asString2 = bf["m_Name"].AsString;
		if (font == null)
		{
			Debug.LogWarning($"AssetBundle '{_bundleName}': TMP font asset '{asString2}' has no source font in the bundle, using the default font.");
		}
		TMP_FontAsset tMP_FontAsset = new TMP_FontAsset(font?.GameFontIndex ?? 2)
		{
			name = asString2,
			sourceFontFile = font,
			hashCode = bf["hashCode"].AsInt,
			materialHashCode = bf["materialHashCode"].AsInt,
			faceInfo = new FaceInfo
			{
				faceIndex = assetTypeValueField["m_FaceIndex"].AsInt,
				familyName = asString,
				styleName = assetTypeValueField["m_StyleName"].AsString,
				pointSize = assetTypeValueField["m_PointSize"].AsInt,
				scale = assetTypeValueField["m_Scale"].AsFloat,
				unitsPerEM = assetTypeValueField["m_UnitsPerEM"].AsInt,
				lineHeight = assetTypeValueField["m_LineHeight"].AsFloat,
				ascentLine = assetTypeValueField["m_AscentLine"].AsFloat,
				capLine = assetTypeValueField["m_CapLine"].AsFloat,
				meanLine = assetTypeValueField["m_MeanLine"].AsFloat,
				baseline = assetTypeValueField["m_Baseline"].AsFloat,
				descentLine = assetTypeValueField["m_DescentLine"].AsFloat,
				superscriptOffset = assetTypeValueField["m_SuperscriptOffset"].AsFloat,
				superscriptSize = assetTypeValueField["m_SuperscriptSize"].AsFloat,
				subscriptOffset = assetTypeValueField["m_SubscriptOffset"].AsFloat,
				subscriptSize = assetTypeValueField["m_SubscriptSize"].AsFloat,
				underlineOffset = assetTypeValueField["m_UnderlineOffset"].AsFloat,
				underlineThickness = assetTypeValueField["m_UnderlineThickness"].AsFloat,
				strikethroughOffset = assetTypeValueField["m_StrikethroughOffset"].AsFloat,
				strikethroughThickness = assetTypeValueField["m_StrikethroughThickness"].AsFloat,
				tabWidth = assetTypeValueField["m_TabWidth"].AsFloat
			}
		};
		foreach (AssetTypeValueField child in bf["m_GlyphTable.Array"].Children)
		{
			AssetTypeValueField assetTypeValueField3 = child["m_Metrics"];
			AssetTypeValueField assetTypeValueField4 = child["m_GlyphRect"];
			tMP_FontAsset.glyphTable.Add(new Glyph(child["m_Index"].AsUInt, new GlyphMetrics(assetTypeValueField3["m_Width"].AsFloat, assetTypeValueField3["m_Height"].AsFloat, assetTypeValueField3["m_HorizontalBearingX"].AsFloat, assetTypeValueField3["m_HorizontalBearingY"].AsFloat, assetTypeValueField3["m_HorizontalAdvance"].AsFloat), new GlyphRect(assetTypeValueField4["m_X"].AsInt, assetTypeValueField4["m_Y"].AsInt, assetTypeValueField4["m_Width"].AsInt, assetTypeValueField4["m_Height"].AsInt))
			{
				scale = child["m_Scale"].AsFloat,
				atlasIndex = child["m_AtlasIndex"].AsInt
			});
		}
		Dictionary<uint, Glyph> dictionary = new Dictionary<uint, Glyph>();
		foreach (Glyph item in tMP_FontAsset.glyphTable)
		{
			dictionary[item.index] = item;
		}
		foreach (AssetTypeValueField child2 in bf["m_CharacterTable.Array"].Children)
		{
			uint asUInt = child2["m_GlyphIndex"].AsUInt;
			dictionary.TryGetValue(asUInt, out var value);
			tMP_FontAsset.characterTable.Add(new TMP_Character(child2["m_Unicode"].AsUInt, asUInt)
			{
				textAsset = tMP_FontAsset,
				glyph = value,
				scale = child2["m_Scale"].AsFloat
			});
		}
		return tMP_FontAsset;
	}

	private Font FindFontByFamily(string family)
	{
		if (string.IsNullOrEmpty(family))
		{
			return null;
		}
		foreach (AssetsFileInstance file in _files)
		{
			foreach (AssetFileInfo item in file.file.GetAssetsOfType(AssetClassID.Font))
			{
				AssetTypeValueField baseField = Manager.GetBaseField(file, item);
				bool flag = string.Equals(baseField["m_Name"].AsString, family, StringComparison.OrdinalIgnoreCase);
				if (!flag && !baseField["m_FontNames.Array"].IsDummy)
				{
					foreach (AssetTypeValueField child in baseField["m_FontNames.Array"].Children)
					{
						flag |= string.Equals(child.AsString, family, StringComparison.OrdinalIgnoreCase);
					}
				}
				if (flag && baseField["m_FontData.Array"].Children.Count > 0)
				{
					return Materialize(new AssetRef(file, item.PathId, AssetClassID.Font)) as Font;
				}
			}
		}
		return null;
	}

	private static Type ClassToType(AssetClassID cls)
	{
		return cls switch
		{
			AssetClassID.Texture2D => typeof(UnityEngine.Texture2D), 
			AssetClassID.Sprite => typeof(Sprite), 
			AssetClassID.AudioClip => typeof(AudioClip), 
			AssetClassID.VideoClip => typeof(VideoClip), 
			AssetClassID.TextAsset => typeof(TextAsset), 
			AssetClassID.Shader => typeof(UnityEngine.Shader), 
			AssetClassID.Material => typeof(UnityEngine.Material), 
			AssetClassID.Mesh => typeof(Mesh), 
			AssetClassID.GameObject => typeof(GameObject), 
			AssetClassID.Font => typeof(Font), 
			_ => null, 
		};
	}

	public void Dispose()
	{
		lock (_sync)
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
			_loaded.Clear();
			_files.Clear();
			_linuxSibling?.Dispose();
			try
			{
				Manager.UnloadAll();
			}
			catch
			{
			}
		}
	}
}
