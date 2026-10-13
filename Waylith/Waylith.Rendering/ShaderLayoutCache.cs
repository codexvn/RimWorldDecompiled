using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Waylith.Diagnostics;
using Waylith.Graphics.Meshes;
using Waylith.Utils;

namespace Waylith.Rendering;

internal static class ShaderLayoutCache
{
	private enum Kind : byte
	{
		Material,
		VertexInput
	}

	internal readonly record struct Dependency(string Path, long Length, long WriteTimeUtcTicks);

	internal readonly record struct Include(string IncluderDirectory, string Name, string ResolvedPath)
	{
		public bool IsCurrent()
		{
			return ShaderSourceLoader.ResolveIncludePath(IncluderDirectory, Name) == ResolvedPath;
		}
	}

	private const uint Magic = 1280917836u;

	private const string Extension = ".layout-cache";

	private static readonly string VersionDirectoryName = typeof(ShaderMetadataParser).Module.ModuleVersionId.ToString("N");

	private static readonly TimeSpan StaleVersionAge = TimeSpan.FromDays(7);

	private static int staleSweepStarted;

	internal static bool TryLoadMaterialLayout(string fileName, out MaterialLayout layout)
	{
		layout = null;
		if (!TryOpen(fileName, Kind.Material, out BinaryReader reader))
		{
			return false;
		}
		using (reader)
		{
			try
			{
				layout = ReadMaterialLayout(reader);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}

	internal static bool TryLoadVertexInputLayout(string fileName, out ShaderVertexInputLayout layout)
	{
		layout = null;
		if (!TryOpen(fileName, Kind.VertexInput, out BinaryReader reader))
		{
			return false;
		}
		using (reader)
		{
			try
			{
				layout = ReadVertexInputLayout(reader);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}

	internal static void Store(string fileName, MaterialLayout layout, IReadOnlyList<Dependency> dependencies, IReadOnlyList<Include> includes)
	{
		if (layout.HasErrors || layout.UniformBlocks.Length != 0)
		{
			return;
		}
		Write(fileName, Kind.Material, dependencies, includes, delegate(BinaryWriter writer)
		{
			writer.Write(layout.UniformSize);
			writer.Write(layout.UniformSlots.Length);
			MaterialUniformSlot[] uniformSlots = layout.UniformSlots;
			for (int i = 0; i < uniformSlots.Length; i++)
			{
				MaterialUniformSlot materialUniformSlot = uniformSlots[i];
				writer.Write(materialUniformSlot.Offset);
				writer.Write((byte)materialUniformSlot.Type);
				writer.Write(materialUniformSlot.PropertyName ?? string.Empty);
				WriteUniformValue(writer, materialUniformSlot.DefaultValue);
				writer.Write(materialUniformSlot.TextureResourceSlot);
				writer.Write(materialUniformSlot.IsColor);
				writer.Write(materialUniformSlot.HasAuthoredDefault);
			}
			writer.Write(layout.TextureSlots.Length);
			MaterialTextureSlot[] textureSlots = layout.TextureSlots;
			for (int i = 0; i < textureSlots.Length; i++)
			{
				MaterialTextureSlot materialTextureSlot = textureSlots[i];
				writer.Write(materialTextureSlot.ResourceSlot);
				writer.Write(materialTextureSlot.Binding);
				writer.Write(materialTextureSlot.PropertyName ?? string.Empty);
				writer.Write(materialTextureSlot.VertexVisible);
				writer.Write((byte)materialTextureSlot.Kind);
				writer.Write((byte)materialTextureSlot.Default);
			}
			writer.Write(layout.StorageBufferSlots.Length);
			MaterialStorageBufferSlot[] storageBufferSlots = layout.StorageBufferSlots;
			for (int i = 0; i < storageBufferSlots.Length; i++)
			{
				MaterialStorageBufferSlot materialStorageBufferSlot = storageBufferSlots[i];
				writer.Write(materialStorageBufferSlot.Binding);
				writer.Write(materialStorageBufferSlot.PropertyName ?? string.Empty);
			}
		});
	}

	internal static void Store(string fileName, ShaderVertexInputLayout layout, IReadOnlyList<Dependency> dependencies, IReadOnlyList<Include> includes)
	{
		if (layout.HasErrors)
		{
			return;
		}
		Write(fileName, Kind.VertexInput, dependencies, includes, delegate(BinaryWriter writer)
		{
			writer.Write(layout.Slots.Count);
			foreach (ShaderVertexInputSlot slot in layout.Slots)
			{
				writer.Write(slot.location);
				writer.Write((int)slot.semantic);
				writer.Write(slot.required);
				writer.Write(slot.defaultOffsetBytes);
				writer.Write(slot.defaultValue.X);
				writer.Write(slot.defaultValue.Y);
				writer.Write(slot.defaultValue.Z);
				writer.Write(slot.defaultValue.W);
			}
			writer.Write(layout.InstanceSlots.Count);
			foreach (ShaderInstanceInputSlot instanceSlot in layout.InstanceSlots)
			{
				writer.Write(instanceSlot.location);
				writer.Write((int)instanceSlot.semantic);
				writer.Write(instanceSlot.required);
			}
		});
	}

	private static MaterialLayout ReadMaterialLayout(BinaryReader reader)
	{
		int uniformSize = reader.ReadInt32();
		MaterialUniformSlot[] array = new MaterialUniformSlot[reader.ReadInt32()];
		for (int i = 0; i < array.Length; i++)
		{
			int offset = reader.ReadInt32();
			MaterialUniformType type = (MaterialUniformType)reader.ReadByte();
			string text = reader.ReadString();
			MaterialUniformValue defaultValue = ReadUniformValue(reader);
			int textureResourceSlot = reader.ReadInt32();
			bool isColor = reader.ReadBoolean();
			bool hasAuthoredDefault = reader.ReadBoolean();
			array[i] = new MaterialUniformSlot(offset, type, text, ShaderPropertyId.GetOrCreate(text), defaultValue, textureResourceSlot, isColor, hasAuthoredDefault);
		}
		MaterialTextureSlot[] array2 = new MaterialTextureSlot[reader.ReadInt32()];
		for (int j = 0; j < array2.Length; j++)
		{
			int resourceSlot = reader.ReadInt32();
			int binding = reader.ReadInt32();
			string text2 = reader.ReadString();
			bool vertexVisible = reader.ReadBoolean();
			MaterialTextureKind kind = (MaterialTextureKind)reader.ReadByte();
			MaterialTextureDefault @default = (MaterialTextureDefault)reader.ReadByte();
			array2[j] = new MaterialTextureSlot(resourceSlot, binding, text2, ShaderPropertyId.GetOrCreate(text2), vertexVisible, kind, @default);
		}
		MaterialStorageBufferSlot[] array3 = new MaterialStorageBufferSlot[reader.ReadInt32()];
		for (int k = 0; k < array3.Length; k++)
		{
			int binding2 = reader.ReadInt32();
			string text3 = reader.ReadString();
			array3[k] = new MaterialStorageBufferSlot(binding2, text3, ShaderPropertyId.GetOrCreate(text3));
		}
		return new MaterialLayout(array, array2, uniformSize, array3);
	}

	private static ShaderVertexInputLayout ReadVertexInputLayout(BinaryReader reader)
	{
		ShaderVertexInputSlot[] array = new ShaderVertexInputSlot[reader.ReadInt32()];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new ShaderVertexInputSlot
			{
				location = reader.ReadInt32(),
				semantic = (VertexAttributeSemantic)reader.ReadInt32(),
				required = reader.ReadBoolean(),
				defaultOffsetBytes = reader.ReadInt32(),
				defaultValue = new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle())
			};
		}
		ShaderInstanceInputSlot[] array2 = new ShaderInstanceInputSlot[reader.ReadInt32()];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = new ShaderInstanceInputSlot
			{
				location = reader.ReadInt32(),
				semantic = (ShaderInstanceInputSemantic)reader.ReadInt32(),
				required = reader.ReadBoolean()
			};
		}
		return new ShaderVertexInputLayout(array, array2);
	}

	private static void WriteUniformValue(BinaryWriter writer, MaterialUniformValue value)
	{
		writer.Write((byte)value.Type);
		Matrix4x4 matrix4x = value.Matrix4x4;
		writer.Write(matrix4x.M11);
		writer.Write(matrix4x.M12);
		writer.Write(matrix4x.M13);
		writer.Write(matrix4x.M14);
		writer.Write(matrix4x.M21);
		writer.Write(matrix4x.M22);
		writer.Write(matrix4x.M23);
		writer.Write(matrix4x.M24);
		writer.Write(matrix4x.M31);
		writer.Write(matrix4x.M32);
		writer.Write(matrix4x.M33);
		writer.Write(matrix4x.M34);
		writer.Write(matrix4x.M41);
		writer.Write(matrix4x.M42);
		writer.Write(matrix4x.M43);
		writer.Write(matrix4x.M44);
	}

	private static MaterialUniformValue ReadUniformValue(BinaryReader reader)
	{
		byte type = reader.ReadByte();
		Matrix4x4 value = new Matrix4x4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		return MaterialUniformValue.FromMatrix((MaterialUniformType)type, value);
	}

	private static bool TryOpen(string fileName, Kind kind, out BinaryReader reader)
	{
		reader = null;
		string text = TryGetEntryPath(fileName, kind);
		if (text == null || ColdBoot.Enabled)
		{
			return false;
		}
		BinaryReader binaryReader = null;
		try
		{
			if (!File.Exists(text))
			{
				return false;
			}
			binaryReader = new BinaryReader(File.OpenRead(text), Encoding.UTF8, leaveOpen: false);
			if (binaryReader.ReadUInt32() != 1280917836 || !DependenciesAreCurrent(binaryReader))
			{
				binaryReader.Dispose();
				return false;
			}
			reader = binaryReader;
			return true;
		}
		catch (Exception)
		{
			binaryReader?.Dispose();
			return false;
		}
	}

	private static bool DependenciesAreCurrent(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		if ((num < 0 || num > 4096) ? true : false)
		{
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			string fileName = reader.ReadString();
			long num2 = reader.ReadInt64();
			long num3 = reader.ReadInt64();
			FileInfo fileInfo = new FileInfo(fileName);
			if (!fileInfo.Exists || fileInfo.Length != num2 || fileInfo.LastWriteTimeUtc.Ticks != num3)
			{
				return false;
			}
		}
		int num4 = reader.ReadInt32();
		if ((num4 < 0 || num4 > 4096) ? true : false)
		{
			return false;
		}
		for (int j = 0; j < num4; j++)
		{
			if (!new Include(reader.ReadString(), reader.ReadString(), reader.ReadString()).IsCurrent())
			{
				return false;
			}
		}
		return true;
	}

	private static void Write(string fileName, Kind kind, IReadOnlyList<Dependency> dependencies, IReadOnlyList<Include> includes, Action<BinaryWriter> writePayload)
	{
		string text = TryGetEntryPath(fileName, kind);
		if (text == null || dependencies.Count == 0)
		{
			return;
		}
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
			{
				binaryWriter.Write(1280917836u);
				binaryWriter.Write(dependencies.Count);
				foreach (Dependency dependency in dependencies)
				{
					binaryWriter.Write(dependency.Path);
					binaryWriter.Write(dependency.Length);
					binaryWriter.Write(dependency.WriteTimeUtcTicks);
				}
				binaryWriter.Write(includes.Count);
				foreach (Include include in includes)
				{
					binaryWriter.Write(include.IncluderDirectory);
					binaryWriter.Write(include.Name);
					binaryWriter.Write(include.ResolvedPath);
				}
				writePayload(binaryWriter);
			}
			CacheFileWriter.Write(text, memoryStream.ToArray());
		}
		catch (Exception)
		{
		}
	}

	private static string? TryGetEntryPath(string fileName, Kind kind)
	{
		string cacheDirectoryPath = RuntimeShaderCompiler.CacheDirectoryPath;
		if (cacheDirectoryPath == null || string.IsNullOrWhiteSpace(fileName))
		{
			return null;
		}
		Span<byte> destination = stackalloc byte[32];
		string s = $"{fileName.Replace('\\', '/')}|{kind}";
		SHA256.HashData(Encoding.UTF8.GetBytes(s), destination);
		string text = Path.Combine(cacheDirectoryPath, "Layouts");
		StartStaleVersionSweep(text);
		return Path.Combine(text, VersionDirectoryName, Convert.ToHexStringLower(destination.Slice(0, 16)) + ".layout-cache");
	}

	private static void StartStaleVersionSweep(string layoutsDirectory)
	{
		if (Interlocked.Exchange(ref staleSweepStarted, 1) != 0)
		{
			return;
		}
		Task.Run(delegate
		{
			string[] fileSystemEntries;
			try
			{
				string path = Path.Combine(layoutsDirectory, VersionDirectoryName);
				Directory.CreateDirectory(path);
				Directory.SetLastWriteTimeUtc(path, DateTime.UtcNow);
				fileSystemEntries = Directory.GetFileSystemEntries(layoutsDirectory);
			}
			catch (Exception)
			{
				return;
			}
			DateTime dateTime = DateTime.UtcNow - StaleVersionAge;
			string[] array = fileSystemEntries;
			foreach (string path2 in array)
			{
				if (!(Path.GetFileName(path2) == VersionDirectoryName))
				{
					try
					{
						if (Directory.Exists(path2))
						{
							if (Directory.GetLastWriteTimeUtc(path2) < dateTime)
							{
								Directory.Delete(path2, recursive: true);
							}
						}
						else if (File.GetLastWriteTimeUtc(path2) < dateTime)
						{
							File.Delete(path2);
						}
					}
					catch (Exception)
					{
					}
				}
			}
		});
	}
}
