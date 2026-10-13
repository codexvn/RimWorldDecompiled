using System;
using UnityEngine.Rendering;
using UnityStubs.Internal;

namespace UnityEngine;

public class Renderer : Component
{
	private Material _sharedMaterial;

	private Material _materialInstance;

	private Material[] _otherMaterials = Array.Empty<Material>();

	private bool _otherMaterialsInstanced;

	internal bool DrawnByOwner;

	public Material material
	{
		get
		{
			if ((object)_materialInstance == null)
			{
				_materialInstance = ((_sharedMaterial != null) ? new Material(_sharedMaterial) : CreateDefaultMaterial());
			}
			return _materialInstance;
		}
		set
		{
			_materialInstance = value;
			_sharedMaterial = value;
		}
	}

	internal Material renderMaterial => _materialInstance ?? _sharedMaterial;

	public Material sharedMaterial
	{
		get
		{
			return _sharedMaterial ?? (_sharedMaterial = CreateDefaultMaterial());
		}
		set
		{
			_sharedMaterial = value;
			_materialInstance = null;
		}
	}

	public bool enabled { get; set; } = true;

	public bool receiveShadows { get; set; } = true;

	public ShadowCastingMode shadowCastingMode { get; set; } = ShadowCastingMode.On;

	public int sortingOrder { get; set; }

	public LightProbeUsage lightProbeUsage { get; set; } = LightProbeUsage.BlendProbes;

	public Material[] sharedMaterials
	{
		get
		{
			if (renderMaterial == null && _otherMaterials.Length == 0)
			{
				return Array.Empty<Material>();
			}
			Material[] array = new Material[1 + _otherMaterials.Length];
			array[0] = renderMaterial;
			_otherMaterials.CopyTo(array, 1);
			return array;
		}
		set
		{
			if (value == null)
			{
				value = Array.Empty<Material>();
			}
			_sharedMaterial = ((value.Length != 0) ? value[0] : null);
			_materialInstance = null;
			_otherMaterials = ((value.Length > 1) ? value[1..] : Array.Empty<Material>());
			_otherMaterialsInstanced = false;
		}
	}

	public Material[] materials
	{
		get
		{
			if (!_otherMaterialsInstanced)
			{
				for (int i = 0; i < _otherMaterials.Length; i++)
				{
					if (_otherMaterials[i] != null)
					{
						_otherMaterials[i] = new Material(_otherMaterials[i]);
					}
				}
				_otherMaterialsInstanced = true;
			}
			Material[] array = new Material[1 + _otherMaterials.Length];
			array[0] = material;
			_otherMaterials.CopyTo(array, 1);
			return array;
		}
		set
		{
			if (value != null && value.Length > 0)
			{
				material = value[0];
				_otherMaterials = value[1..];
				_otherMaterialsInstanced = true;
			}
		}
	}

	public Bounds bounds
	{
		get
		{
			StubWarner.LogStub("bounds", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Renderer.cs", 111);
			return default(Bounds);
		}
	}

	internal void DrawSubMeshes(Mesh mesh, Matrix4x4 matrix)
	{
		int subMeshCount = mesh.subMeshCount;
		if (subMeshCount == 0)
		{
			return;
		}
		int layer = base.gameObject.layer;
		for (int i = 0; i <= _otherMaterials.Length; i++)
		{
			Material material = ((i == 0) ? renderMaterial : _otherMaterials[i - 1]);
			if (material != null)
			{
				Graphics.DrawMesh(mesh, matrix, material, layer, null, Math.Min(i, subMeshCount - 1), null);
			}
		}
	}

	private static Material CreateDefaultMaterial()
	{
		return new Material((Shader)null)
		{
			name = "Default Renderer Material"
		};
	}
}
