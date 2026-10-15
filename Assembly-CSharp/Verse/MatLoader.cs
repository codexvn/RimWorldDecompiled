using System.Collections.Generic;
using UnityEngine;

namespace Verse;

public static class MatLoader
{
	private struct Request
	{
		public string path;

		public int renderQueue;

		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombine(0, path), renderQueue);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Request))
			{
				return false;
			}
			return Equals((Request)obj);
		}

		public bool Equals(Request other)
		{
			if (other.path == path)
			{
				return other.renderQueue == renderQueue;
			}
			return false;
		}

		public static bool operator ==(Request lhs, Request rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Request lhs, Request rhs)
		{
			return !(lhs == rhs);
		}

		public override string ToString()
		{
			return "MatLoader.Request(" + path + ", " + renderQueue + ")";
		}
	}

	private static Dictionary<Request, Material> dict = new Dictionary<Request, Material>();

	public static void ClearCache()
	{
		dict.Clear();
	}

	public static Material LoadMat(string matPath, int renderQueue = -1)
	{
		Request request = default(Request);
		request.path = matPath;
		request.renderQueue = renderQueue;
		Request key = request;
		if (!dict.TryGetValue(key, out var value))
		{
			Material material = TryLoadMaterialDef(matPath, -1, copy: false);
			if (material == null)
			{
				Log.Warning("Could not load material " + matPath);
				return BaseContent.BadMat;
			}
			value = MaterialAllocator.Create(material);
			value.name = "MatLoader.LoadMat";
			if (renderQueue != -1)
			{
				value.renderQueue = renderQueue;
			}
			dict.Add(key, value);
		}
		return value;
	}

	public static Material LoadMatDirect(string matPath)
	{
		return TryLoadMaterialDef(matPath, -1, copy: false) ?? BaseContent.BadMat;
	}

	private static Material TryLoadMaterialDef(string matPath, int renderQueue, bool copy)
	{
		if (MaterialDefUtility.TryGetByResourcePath(matPath, out var def))
		{
			if (copy || renderQueue != -1)
			{
				return def.CreateMaterialInstance(renderQueue);
			}
			return def.Material;
		}
		if (DefDatabase<MaterialDef>.DefCount == 0)
		{
			Material material = Material.CreateDeferred(() => ResolveDeferredMaterial(matPath));
			if (renderQueue != -1)
			{
				material.renderQueue = renderQueue;
			}
			return material;
		}
		return null;
	}

	private static Material ResolveDeferredMaterial(string matPath)
	{
		if (MaterialDefUtility.TryGetByResourcePath(matPath, out var def))
		{
			return def.Material;
		}
		if (DefDatabase<MaterialDef>.DefCount == 0)
		{
			return null;
		}
		Log.Error("No MaterialDef for material path '" + matPath + "'.");
		return BaseContent.BadMat;
	}
}
