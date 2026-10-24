using System;
using System.Collections.Generic;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;

namespace Waylith.AssetBake;

public abstract class AssetBakeHost
{
	public static readonly BcRdoRule[] DefaultRdoRules = new BcRdoRule[4]
	{
		new BcRdoRule(BcEncodeTarget.Bc1, "", 3f, 256),
		new BcRdoRule(BcEncodeTarget.Bc4, "", 1f, 128),
		new BcRdoRule(BcEncodeTarget.Bc5, "", 2f, 256),
		new BcRdoRule(BcEncodeTarget.Bc7, "", 0.5f, 128)
	};

	public virtual IEnumerable<string> VersionedAssemblies => Array.Empty<string>();

	public virtual IEnumerable<string> ShaderPlanAssemblies => Array.Empty<string>();

	public virtual IEnumerable<string> ShaderSourceFiles => Array.Empty<string>();

	public virtual int RawMaxSize => 512;

	public virtual IEnumerable<string> RawPaths => Array.Empty<string>();

	public virtual bool ResolvesAtlasSources => false;

	public virtual IEnumerable<string> SmallRawPaths => Array.Empty<string>();

	public virtual IEnumerable<BcRdoRule> RdoRules => DefaultRdoRules;

	public virtual string Usage => "";

	public abstract ShaderWarmupPlan BuildShaderPlan(AssetBakeOptions options);

	public virtual AtlasSources? ResolveAtlasSources(AssetBakeOptions options)
	{
		return null;
	}

	public virtual IEnumerable<string> AtlasSourceInputs(AssetBakeOptions options)
	{
		return Array.Empty<string>();
	}

	public virtual IEnumerable<RawSource> RawFiles(AssetBakeOptions options)
	{
		return Array.Empty<RawSource>();
	}

	public virtual CpuMeshData? ParseMesh(string path, AssetBakeOptions options)
	{
		return null;
	}

	public virtual bool TryParseOption(string name, string value)
	{
		return false;
	}

	public virtual string Settings(AssetBakeOptions options)
	{
		return "";
	}
}
