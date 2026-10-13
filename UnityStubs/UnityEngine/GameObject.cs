using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityStubs.Internal;
using Waylith.UnityCompat;

namespace UnityEngine;

public class GameObject : Object
{
	private static readonly Dictionary<PrimitiveType, Mesh> PrimitiveMeshes = new Dictionary<PrimitiveType, Mesh>();

	private static readonly ConcurrentDictionary<Type, Type[]> RequiredComponents = new ConcurrentDictionary<Type, Type[]>();

	public int layer { get; set; }

	internal bool IsAssetToken { get; }

	public bool activeInHierarchy => UnityRuntime.Current?.World.IsGameObjectActiveInHierarchy(this) ?? false;

	public Scene scene => new Scene(UnityRuntime.Current?.World.GetSceneName(this) ?? string.Empty);

	public Transform transform => (Transform)UnityRuntime.Current.World.GetTransform(this);

	public bool activeSelf => UnityRuntime.Current?.World.IsGameObjectActiveSelf(this) ?? true;

	public GameObject()
		: this("GameObject")
	{
	}

	private GameObject(string name, Transform transform, bool isAssetToken)
	{
		if (UnityRuntime.Current == null)
		{
			throw new InvalidOperationException("Unity runtime has not been initialized.");
		}
		IsAssetToken = isAssetToken;
		UnityRuntime.Current.World.InitializeGameObject(this, transform, name, isAssetToken);
		base.name = name;
	}

	public GameObject(string name, params Type[] components)
		: this(name, new Transform(), isAssetToken: false)
	{
		foreach (Type componentType in components)
		{
			AddComponent(componentType);
		}
	}

	public GameObject(string name)
		: this(name, Array.Empty<Type>())
	{
	}

	public static GameObject CreateAssetToken(string name)
	{
		return new GameObject(name, new Transform(), isAssetToken: true);
	}

	public T0 AddComponent<T0>()
	{
		if (UnityRuntime.Current == null)
		{
			return default(T0);
		}
		AddRequiredComponents(typeof(T0));
		return (T0)Attach(UnityRuntime.Current.World.AddComponent(this, typeof(T0)));
	}

	public Component AddComponent(Type componentType)
	{
		if (UnityRuntime.Current == null)
		{
			return null;
		}
		AddRequiredComponents(componentType);
		return (Component)Attach(UnityRuntime.Current.World.AddComponent(this, componentType));
	}

	private void AddRequiredComponents(Type componentType)
	{
		Type[] orAdd = RequiredComponents.GetOrAdd(componentType, delegate(Type type)
		{
			List<Type> list = new List<Type>();
			object[] customAttributes = type.GetCustomAttributes(typeof(RequireComponent), inherit: true);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				RequireComponent requireComponent = (RequireComponent)customAttributes[i];
				Type[] array = new Type[3] { requireComponent.m_Type0, requireComponent.m_Type1, requireComponent.m_Type2 };
				foreach (Type type2 in array)
				{
					if (type2 != null && !type2.IsAbstract && type2 != typeof(Transform) && !list.Contains(type2))
					{
						list.Add(type2);
					}
				}
			}
			return list.ToArray();
		});
		foreach (Type type3 in orAdd)
		{
			if (GetComponent(type3) == null)
			{
				AddComponent(type3);
			}
		}
	}

	private object Attach(object component)
	{
		if (component is ParticleSystem particleSystem)
		{
			particleSystem.Attach();
		}
		else if ((component is TrailRenderer || component is LineRenderer) ? true : false)
		{
			LineRendererDriver.Register((Renderer)component);
		}
		else
		{
			bool flag = ((component is MeshRenderer || component is SkinnedMeshRenderer) ? true : false);
			if (flag && !IsAssetToken)
			{
				MeshRendererDriver.Register((Renderer)component);
			}
		}
		return component;
	}

	public static GameObject CreatePrimitive(PrimitiveType type)
	{
		GameObject gameObject = new GameObject(type.ToString());
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		meshFilter.sharedMesh = GetOrCreatePrimitiveMesh(type);
		return gameObject;
	}

	public static GameObject Find(string name)
	{
		return (GameObject)(UnityRuntime.Current?.World.FindGameObject(name));
	}

	public T0 GetComponent<T0>()
	{
		if (UnityRuntime.Current == null)
		{
			return default(T0);
		}
		return (T0)UnityRuntime.Current.World.GetComponent(this, typeof(T0));
	}

	public Component GetComponent(Type type)
	{
		if (UnityRuntime.Current == null)
		{
			return null;
		}
		return UnityRuntime.Current.World.GetComponent(this, type) as Component;
	}

	public Component[] GetComponents(Type type)
	{
		if (UnityRuntime.Current == null)
		{
			return Array.Empty<Component>();
		}
		Array components = UnityRuntime.Current.World.GetComponents(this, type);
		Component[] array = new Component[components.Length];
		components.CopyTo(array, 0);
		return array;
	}

	public T0[] GetComponents<T0>()
	{
		if (UnityRuntime.Current == null)
		{
			return Array.Empty<T0>();
		}
		return (T0[])UnityRuntime.Current.World.GetComponents(this, typeof(T0));
	}

	public T0[] GetComponentsInChildren<T0>()
	{
		return GetComponentsInChildren<T0>(includeInactive: false);
	}

	public T0 GetComponentInChildren<T0>()
	{
		return GetComponentInChildren<T0>(includeInactive: false);
	}

	public T0 GetComponentInChildren<T0>(bool includeInactive)
	{
		if (UnityRuntime.Current == null)
		{
			return default(T0);
		}
		return (T0)UnityRuntime.Current.World.GetComponentInChildren(this, typeof(T0), includeInactive);
	}

	public T0[] GetComponentsInChildren<T0>(bool includeInactive)
	{
		if (UnityRuntime.Current == null)
		{
			return Array.Empty<T0>();
		}
		return (T0[])UnityRuntime.Current.World.GetComponentsInChildren(this, typeof(T0), includeInactive);
	}

	public void SetActive(bool value)
	{
		if (UnityRuntime.Current != null)
		{
			UnityRuntime.Current.World.SetGameObjectActive(this, value);
		}
	}

	public bool TryGetComponent<T0>(out T0 component)
	{
		if (UnityRuntime.Current != null && UnityRuntime.Current.World.TryGetComponent(this, typeof(T0), out object component2) && component2 is T0 val)
		{
			component = val;
			return true;
		}
		component = default(T0);
		return false;
	}

	internal static Mesh GetOrCreatePrimitiveMesh(PrimitiveType type)
	{
		if (PrimitiveMeshes.TryGetValue(type, out var value))
		{
			return value;
		}
		value = type switch
		{
			PrimitiveType.Sphere => CreateSpherePrimitiveMesh(), 
			PrimitiveType.Cube => CreateCubePrimitiveMesh(), 
			PrimitiveType.Quad => CreateQuadPrimitiveMesh(), 
			PrimitiveType.Plane => CreatePlanePrimitiveMesh(), 
			_ => throw new NotSupportedException($"Primitive type '{type}' is not implemented."), 
		};
		value.name = $"{type} Primitive";
		PrimitiveMeshes[type] = value;
		return value;
	}

	private static Mesh CreateCubePrimitiveMesh()
	{
		Vector3[] vertices2 = new Vector3[24];
		Vector3[] normals2 = new Vector3[24];
		Vector2[] array = new Vector2[24];
		int[] triangles2 = new int[36];
		int vertexIndex2 = 0;
		int triangleIndex2 = 0;
		AddFace(vertices2, normals2, array, triangles2, ref vertexIndex2, ref triangleIndex2, new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f), Vector3.forward);
		AddFace(vertices2, normals2, array, triangles2, ref vertexIndex2, ref triangleIndex2, new Vector3(0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), Vector3.back);
		AddFace(vertices2, normals2, array, triangles2, ref vertexIndex2, ref triangleIndex2, new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), Vector3.right);
		AddFace(vertices2, normals2, array, triangles2, ref vertexIndex2, ref triangleIndex2, new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, -0.5f), Vector3.left);
		AddFace(vertices2, normals2, array, triangles2, ref vertexIndex2, ref triangleIndex2, new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), Vector3.up);
		AddFace(vertices2, normals2, array, triangles2, ref vertexIndex2, ref triangleIndex2, new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f), Vector3.down);
		Mesh mesh = new Mesh();
		mesh.vertices = vertices2;
		mesh.normals = normals2;
		mesh.uv = array;
		mesh.triangles = triangles2;
		mesh.RecalculateBounds();
		return mesh;
		static void AddFace(Vector3[] vertices, Vector3[] normals, Vector2[] uvs, int[] triangles, ref int vertexIndex, ref int triangleIndex, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 normal)
		{
			vertices[vertexIndex] = p0;
			vertices[vertexIndex + 1] = p1;
			vertices[vertexIndex + 2] = p2;
			vertices[vertexIndex + 3] = p3;
			normals[vertexIndex] = normal;
			normals[vertexIndex + 1] = normal;
			normals[vertexIndex + 2] = normal;
			normals[vertexIndex + 3] = normal;
			uvs[vertexIndex] = new Vector2(0f, 0f);
			uvs[vertexIndex + 1] = new Vector2(1f, 0f);
			uvs[vertexIndex + 2] = new Vector2(1f, 1f);
			uvs[vertexIndex + 3] = new Vector2(0f, 1f);
			triangles[triangleIndex] = vertexIndex;
			triangles[triangleIndex + 1] = vertexIndex + 1;
			triangles[triangleIndex + 2] = vertexIndex + 2;
			triangles[triangleIndex + 3] = vertexIndex;
			triangles[triangleIndex + 4] = vertexIndex + 2;
			triangles[triangleIndex + 5] = vertexIndex + 3;
			vertexIndex += 4;
			triangleIndex += 6;
		}
	}

	private static Mesh CreateQuadPrimitiveMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[4]
		{
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(0.5f, -0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f),
			new Vector3(-0.5f, 0.5f, 0f)
		};
		mesh.normals = new Vector3[4]
		{
			Vector3.forward,
			Vector3.forward,
			Vector3.forward,
			Vector3.forward
		};
		mesh.uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f)
		};
		mesh.triangles = new int[6] { 0, 1, 2, 0, 2, 3 };
		mesh.RecalculateBounds();
		return mesh;
	}

	private static Mesh CreatePlanePrimitiveMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[4]
		{
			new Vector3(-5f, 0f, -5f),
			new Vector3(5f, 0f, -5f),
			new Vector3(5f, 0f, 5f),
			new Vector3(-5f, 0f, 5f)
		};
		mesh.normals = new Vector3[4]
		{
			Vector3.up,
			Vector3.up,
			Vector3.up,
			Vector3.up
		};
		mesh.uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f)
		};
		mesh.triangles = new int[6] { 0, 2, 1, 0, 3, 2 };
		mesh.RecalculateBounds();
		return mesh;
	}

	private static Mesh CreateSpherePrimitiveMesh()
	{
		Vector3[] array = new Vector3[425];
		Vector3[] array2 = new Vector3[425];
		Vector2[] array3 = new Vector2[425];
		int[] array4 = new int[2304];
		int num = 0;
		for (int i = 0; i <= 16; i++)
		{
			float num2 = (float)i / 16f;
			float f = num2 * (float)Math.PI;
			float y = Mathf.Cos(f) * 0.5f;
			float num3 = Mathf.Sin(f) * 0.5f;
			for (int j = 0; j <= 24; j++)
			{
				float num4 = (float)j / 24f;
				float f2 = num4 * (float)Math.PI * 2f;
				float x = Mathf.Cos(f2) * num3;
				float z = Mathf.Sin(f2) * num3;
				Vector3 vector = (array[num] = new Vector3(x, y, z));
				array2[num] = vector.normalized;
				array3[num] = new Vector2(num4, num2);
				num++;
			}
		}
		int num5 = 0;
		int num6 = 25;
		for (int k = 0; k < 16; k++)
		{
			for (int l = 0; l < 24; l++)
			{
				int num7 = k * num6 + l;
				int num8 = num7 + 1;
				int num9 = num7 + num6;
				int num10 = num9 + 1;
				array4[num5++] = num7;
				array4[num5++] = num9;
				array4[num5++] = num8;
				array4[num5++] = num8;
				array4[num5++] = num9;
				array4[num5++] = num10;
			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.uv = array3;
		mesh.triangles = array4;
		mesh.RecalculateBounds();
		return mesh;
	}
}
