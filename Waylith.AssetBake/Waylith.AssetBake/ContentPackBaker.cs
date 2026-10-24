using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Waylith.Assets;
using Waylith.Rendering;

namespace Waylith.AssetBake;

public static class ContentPackBaker
{
	public static ContentBakeResult Bake(ContentBakeRequest request, Action<string>? progress = null, CancellationToken cancellation = default(CancellationToken))
	{
		List<Source> sources = ContentSources.Select(ContentSources.Enumerate(request.Roots), request.PremultipliedPaths);
		ContentSources.AddRaw(sources, request.RawFiles);
		BakeFingerprint bakeFingerprint = BakeFingerprint.ComputeContent($"{request.Codec} {request.Level} {Describe(request.Texture)} premul:{string.Join('|', request.PremultipliedPaths)} mesh:{1} obj:{1} store:{string.Join('|', request.StoredRawExtensions)}", request.CodeIdentity, sources, request.Alignment);
		BakeFingerprint fingerprint;
		AssetPack assetPack = OpenPrevious(request.Output, out fingerprint, progress);
		if (assetPack != null && fingerprint?.Textures == bakeFingerprint.Textures && fingerprint.Layout == bakeFingerprint.Layout)
		{
			int count = assetPack.ContentHashes.Count;
			assetPack.Dispose();
			return new ContentBakeResult(Succeeded: true, count, count, 0, 0, Array.Empty<string>(), Unchanged: true);
		}
		bool settingsUnchanged = fingerprint?.Settings == bakeFingerprint.Settings;
		ContentPassInput input = new ContentPassInput
		{
			Output = request.Output,
			Base = Path.GetFullPath(request.Base),
			Level = request.Level,
			Codec = request.Codec,
			Texture = request.Texture,
			StoredRawExtensions = request.StoredRawExtensions,
			MaxDegreeOfParallelism = ((request.MaxDegreeOfParallelism > 0) ? request.MaxDegreeOfParallelism : System.Math.Max(1, Environment.ProcessorCount / 2)),
			Progress = progress,
			Cancellation = cancellation
		};
		Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(request.Output)));
		string text = request.Output + ".tmp";
		ContentResult contentResult;
		try
		{
			using AssetPackWriter writer = new AssetPackWriter(text, request.Alignment);
			contentResult = ContentPass.Run(input, sources, assetPack, settingsUnchanged, writer);
		}
		catch
		{
			assetPack?.Dispose();
			File.Delete(text);
			throw;
		}
		finally
		{
			if (request.Texture.EncodeOnGpu)
			{
				TextureBlockEncoder.ReleasePixelEncodeContexts();
			}
		}
		assetPack?.Dispose();
		if (contentResult.Failed > 0)
		{
			File.Delete(text);
			return new ContentBakeResult(Succeeded: false, contentResult.Entries, contentResult.Reused, contentResult.Linked, contentResult.Failed, contentResult.Errors, Unchanged: false);
		}
		if (!request.WriteStamps)
		{
			File.Delete(AssetPackManifest.StampsPath(request.Output));
		}
		if (!request.WriteRoles)
		{
			File.Delete(AssetPackManifest.RolesPath(request.Output));
		}
		bool unchanged = Commit(text, request.Output, request.WriteStamps ? contentResult.Stamps : null, request.WriteRoles ? contentResult.Roles : null, bakeFingerprint);
		return new ContentBakeResult(Succeeded: true, contentResult.Entries, contentResult.Reused, contentResult.Linked, 0, Array.Empty<string>(), unchanged);
	}

	internal static string Describe(TextureBakeSettings texture)
	{
		IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
		DefaultInterpolatedStringHandler handler = new DefaultInterpolatedStringHandler(38, 9, invariantCulture);
		handler.AppendFormatted(texture.Target);
		handler.AppendLiteral(" q");
		handler.AppendFormatted(texture.Quality);
		handler.AppendLiteral(" bc7:");
		handler.AppendFormatted((uint)texture.Bc7Flags, "x");
		handler.AppendLiteral(" raw:");
		handler.AppendFormatted(texture.RawMaxSize);
		handler.AppendLiteral(" bc1:");
		handler.AppendFormatted(texture.AllowBc1);
		handler.AppendLiteral(" skip:");
		handler.AppendFormatted(string.Join('|', texture.RawPaths));
		handler.AppendLiteral(" rdo:");
		handler.AppendFormatted(string.Join('|', texture.RdoRules));
		handler.AppendLiteral(" mip:");
		handler.AppendFormatted(texture.MipFilter);
		handler.AppendLiteral(" gpu:");
		handler.AppendFormatted(texture.EncodeOnGpu);
		return string.Create(invariantCulture, ref handler) + ((texture.SelectTarget != null) ? " select:custom" : "") + ((texture.UsageFor != null) ? " usage:custom" : "") + ((texture.BorderMipMaps != null) ? " border:custom" : "");
	}

	internal static AssetPack? OpenPrevious(string output, out BakeFingerprint? fingerprint, Action<string>? log)
	{
		AssetPack assetPack;
		try
		{
			assetPack = AssetPack.TryOpen(output);
		}
		catch (Exception ex) when (((ex is InvalidDataException || ex is IOException) ? 1 : 0) != 0)
		{
			log?.Invoke(output + " is damaged (" + ex.Message + "); baking it from scratch");
			assetPack = null;
		}
		fingerprint = ((assetPack == null) ? null : BakeFingerprint.Read(output));
		if (assetPack != null)
		{
			string text = FirstDamagedEntry(assetPack);
			if (text != null)
			{
				log?.Invoke(output + " has a damaged entry (" + text + "); baking it from scratch");
				assetPack.Dispose();
				assetPack = null;
				fingerprint = null;
			}
		}
		return assetPack;
	}

	internal static bool Commit(string temporaryPath, string output, IReadOnlyDictionary<string, string>? stamps, IReadOnlyDictionary<string, string>? roles, BakeFingerprint fingerprint)
	{
		File.Delete(BakeFingerprint.PathFor(output));
		bool num = SameContents(temporaryPath, output);
		if (num)
		{
			File.Delete(temporaryPath);
		}
		else
		{
			File.Move(temporaryPath, output, overwrite: true);
		}
		if (stamps != null)
		{
			AssetPackManifest.WriteStamps(output, stamps);
		}
		if (roles != null)
		{
			AssetPackManifest.WriteRoles(output, roles);
		}
		fingerprint.Write(output);
		return num;
	}

	private static bool SameContents(string path, string otherPath)
	{
		if (!File.Exists(otherPath) || new FileInfo(path).Length != new FileInfo(otherPath).Length)
		{
			return false;
		}
		using FileStream fileStream = File.OpenRead(path);
		using FileStream fileStream2 = File.OpenRead(otherPath);
		byte[] array = new byte[1048576];
		byte[] array2 = new byte[1048576];
		int num;
		while ((num = fileStream.ReadAtLeast(array, array.Length, throwOnEndOfStream: false)) > 0)
		{
			fileStream2.ReadExactly(array2, 0, num);
			if (!((ReadOnlySpan<byte>)array.AsSpan(0, num)).SequenceEqual((ReadOnlySpan<byte>)array2.AsSpan(0, num)))
			{
				return false;
			}
		}
		return true;
	}

	private static string? FirstDamagedEntry(AssetPack pack)
	{
		string damaged = null;
		Parallel.ForEach(pack.Names, delegate(string name, ParallelLoopState loop)
		{
			if (!pack.TryRent(name, out byte[] buffer, out int _))
			{
				damaged = name;
				loop.Stop();
			}
			else
			{
				AssetPack.Return(buffer);
			}
		});
		return damaged;
	}
}
