using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Waylith.Assets;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;

namespace Waylith.AssetBake;

internal static class ContentPass
{
	private sealed record Owner(string Name, AssetPackWriter.Payload Payload, int Length, uint Crc, AssetPackCodec Codec);

	public static ContentResult Run(ContentPassInput input, List<Source> sources, AssetPack? previous, bool settingsUnchanged, AssetPackWriter writer)
	{
		AssetPack reusable = (settingsUnchanged ? previous : null);
		Dictionary<string, string> previousStamps = ((reusable != null) ? AssetPackManifest.ReadStamps(input.Output) : new Dictionary<string, string>());
		Dictionary<string, string> previousRoles = ((reusable != null) ? AssetPackManifest.ReadRoles(input.Output) : new Dictionary<string, string>());
		Dictionary<string, string> previousHashes = AssetPackManifest.ReadContentHashes(reusable);
		Dictionary<string, AssetPackTextureInfo.Info> previousInfo = AssetPackTextureInfo.Read(reusable);
		ConcurrentDictionary<string, Owner> owners = new ConcurrentDictionary<string, Owner>();
		ConcurrentDictionary<string, string> stamps = new ConcurrentDictionary<string, string>();
		ConcurrentDictionary<string, string> roles = new ConcurrentDictionary<string, string>();
		ConcurrentDictionary<string, string> hashes = new ConcurrentDictionary<string, string>();
		ConcurrentDictionary<string, AssetPackTextureInfo.Info> infos = new ConcurrentDictionary<string, AssetPackTextureInfo.Info>();
		ConcurrentQueue<string> errors = new ConcurrentQueue<string>();
		int reused = 0;
		int failed = 0;
		int done = 0;
		AtlasSources atlas = input.Atlas;
		ParallelOptions parallelOptions = new ParallelOptions
		{
			MaxDegreeOfParallelism = input.MaxDegreeOfParallelism,
			CancellationToken = input.Cancellation
		};
		Parallel.ForEach(sources, parallelOptions, delegate(Source source)
		{
			input.Cancellation.ThrowIfCancellationRequested();
			string text = AssetPackManifest.Stamp(new FileInfo(source.Path));
			string fullPath = Path.GetFullPath(source.Path);
			bool flag = atlas?.Tiles.Contains(fullPath) ?? false;
			bool flag2 = source.Kind == SourceKind.Raw;
			bool flag3 = flag2 && ContentSources.IsStored(source, input.StoredRawExtensions);
			AssetPackCodec assetPackCodec = ((!flag3) ? input.EntryCodec : AssetPackCodec.Store);
			object obj;
			if (!flag2)
			{
				if (!source.IsMesh)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					object value;
					if (!flag)
					{
						AtlasSources atlasSources = atlas;
						value = (((object)atlasSources != null && atlasSources.SampledSources.Contains(fullPath)) ? "sampled" : "image");
					}
					else
					{
						value = "tile";
					}
					defaultInterpolatedStringHandler.AppendFormatted((string?)value);
					defaultInterpolatedStringHandler.AppendLiteral(":");
					defaultInterpolatedStringHandler.AppendFormatted(input.Texture.Usage(fullPath));
					obj = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					obj = "mesh";
				}
			}
			else
			{
				obj = "raw";
			}
			string text2 = (string)obj;
			bool[] variants = source.Variants;
			foreach (bool flag4 in variants)
			{
				string name = ContentSources.EntryName(input.Base, source, flag4);
				try
				{
					AssetPackWriter.Payload payload = default(AssetPackWriter.Payload);
					int length = 0;
					uint crc = 0u;
					AssetPackTextureInfo.Info? info = null;
					string text3;
					if (reusable != null && previousStamps.TryGetValue(name, out string value2) && value2 == text && previousRoles.TryGetValue(name, out string value3) && value3 == text2 && previousHashes.TryGetValue(name, out string value4) && (source.Kind != 0 || previousInfo.ContainsKey(name)) && AssetPackWriter.TryReference(reusable, name, out payload, out length, out crc, out var codec) && codec == assetPackCodec)
					{
						text3 = value4;
						if (source.Kind == SourceKind.Texture)
						{
							info = previousInfo[name];
						}
						Interlocked.Increment(ref reused);
					}
					else
					{
						byte[] array;
						if (flag2)
						{
							array = File.ReadAllBytes(source.Path);
						}
						else if (source.IsMesh)
						{
							array = CpuMeshCache.EncodePackEntry(ParseMesh(source.Path, input.ParseMesh));
						}
						else
						{
							if (!GpuTextureService.ExtensionMatchesContent(source.Path))
							{
								throw new InvalidDataException("the file contents do not match its extension");
							}
							LoadedTextureData texture = GpuTextureService.LoadTextureForBake(source.Path, flag4, input.Texture);
							info = new AssetPackTextureInfo.Info(texture.Width, texture.Height, texture.MipCount, texture.Format, texture.Opaque, texture.Swizzle);
							if (flag)
							{
								texture = texture with
								{
									MipCount = 1,
									Levels = new byte[1][] { texture.Levels[0] }
								};
							}
							array = DdsFile.Encode(in texture, AlphaModeOf(in texture, flag4));
						}
						text3 = Convert.ToHexString(SHA256.HashData(array));
						length = array.Length;
						crc = AssetPackWriter.Crc32Of(array);
						payload = (owners.TryGetValue(OwnerKey(text3, assetPackCodec), out Owner value5) ? value5.Payload : writer.Store(flag3 ? array : AssetPackWriter.Compress(array, input.Codec, input.Level)));
					}
					Owner candidate = new Owner(name, payload, length, crc, assetPackCodec);
					owners.AddOrUpdate(OwnerKey(text3, assetPackCodec), candidate, (string _, Owner current) => (string.CompareOrdinal(name, current.Name) >= 0) ? current : candidate);
					stamps[name] = text;
					roles[name] = text2;
					hashes[name] = text3;
					if (info.HasValue)
					{
						AssetPackTextureInfo.Info valueOrDefault = info.GetValueOrDefault();
						infos[name] = valueOrDefault;
					}
				}
				catch (Exception ex) when (!(ex is OperationCanceledException))
				{
					errors.Enqueue(source.Path + ": " + ex.Message);
					Interlocked.Increment(ref failed);
				}
			}
			input.Progress?.Invoke($"{Interlocked.Increment(ref done)}/{sources.Count} {Path.GetRelativePath(input.Base, source.Path).Replace('\\', '/')}");
		});
		foreach (Owner value6 in owners.Values)
		{
			writer.Add(value6.Name, value6.Payload, value6.Length, value6.Crc, value6.Codec);
		}
		int linked = stamps.Count - owners.Count;
		Add(writer, "bake.manifest", AssetPackManifest.WriteContentHashes(hashes), input.Codec, input.Level);
		Add(writer, "textures.info", AssetPackTextureInfo.Write(infos), input.Codec, input.Level);
		return new ContentResult(stamps.Count, reused, linked, failed, stamps, roles, new global::_003C_003Ez__ReadOnlyArray<string>(Enumerable.ToArray(errors)));
	}

	private static CpuMeshData ParseMesh(string path, Func<string, CpuMeshData?>? hostParser)
	{
		return hostParser?.Invoke(path) ?? ObjMeshLoader.Parse(path).CpuStorage ?? throw new InvalidDataException("no vertex data in mesh");
	}

	public static void Add(AssetPackWriter writer, string name, byte[] data, AssetPackCodec codec, int level)
	{
		writer.Add(name, AssetPackWriter.Compress(data, codec, level), data.Length, AssetPackWriter.Crc32Of(data), (level > 0) ? codec : AssetPackCodec.Store);
	}

	private static string OwnerKey(string contentHash, AssetPackCodec codec)
	{
		if (codec != 0)
		{
			return contentHash;
		}
		return contentHash + ":store";
	}

	private static DdsAlphaMode AlphaModeOf(in LoadedTextureData texture, bool premultiplied)
	{
		if (!texture.Opaque)
		{
			if (!premultiplied)
			{
				return DdsAlphaMode.Straight;
			}
			return DdsAlphaMode.Premultiplied;
		}
		return DdsAlphaMode.Opaque;
	}
}
