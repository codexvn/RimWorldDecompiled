using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Brutal;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Assets;

namespace Waylith.Rendering.Compute;

public static class ComputeKernelCache
{
	private static DeviceEx device = null;

	private static readonly Dictionary<(string path, string entryPoint, int pushConstantSize, string bindingsKey, ShaderSourceLanguage language), ComputeKernel> cache = new Dictionary<(string, string, int, string, ShaderSourceLanguage), ComputeKernel>();

	private static readonly Lock cacheLock = new Lock();

	public static void Configure(DeviceEx deviceEx)
	{
		device = deviceEx;
	}

	public static ComputeKernel Load(string glslPath, string entryPoint, ReadOnlySpan<ComputeBinding> bindings, int pushConstantSize = 0, ShaderSourceLanguage language = ShaderSourceLanguage.Glsl)
	{
		(string, string, int, string, ShaderSourceLanguage) key = (glslPath, entryPoint, pushConstantSize, BuildBindingsKey(bindings), language);
		using (cacheLock.EnterScope())
		{
			if (cache.TryGetValue(key, out ComputeKernel value))
			{
				return value;
			}
			ComputeKernel computeKernel = Compile(glslPath, entryPoint, bindings, pushConstantSize, language);
			cache.Add(key, computeKernel);
			return computeKernel;
		}
	}

	public static ComputeKernel[] LoadBatch(string glslPath, string[] entryPoints, ReadOnlySpan<ComputeBinding> bindings, int pushConstantSize = 0, ShaderSourceLanguage language = ShaderSourceLanguage.Glsl)
	{
		string item = BuildBindingsKey(bindings);
		ComputeKernel[] array = new ComputeKernel[entryPoints.Length];
		using (cacheLock.EnterScope())
		{
			List<int> list = new List<int>(entryPoints.Length);
			for (int i = 0; i < entryPoints.Length; i++)
			{
				if (cache.TryGetValue((glslPath, entryPoints[i], pushConstantSize, item, language), out ComputeKernel value))
				{
					array[i] = value;
				}
				else
				{
					list.Add(i);
				}
			}
			if (list.Count == 0)
			{
				return array;
			}
			(string source, string sourceName) tuple = ReadSource(glslPath, language);
			string item2 = tuple.source;
			string item3 = tuple.sourceName;
			string[] array2 = new string[list.Count];
			for (int j = 0; j < list.Count; j++)
			{
				array2[j] = entryPoints[list[j]];
			}
			byte[][] array3 = RuntimeShaderCompiler.CompileBatch(item2, item3, array2, (language == ShaderSourceLanguage.Hlsl) ? RuntimeShaderLanguage.Hlsl : RuntimeShaderLanguage.Glsl, RuntimeShaderStage.Compute);
			for (int k = 0; k < list.Count; k++)
			{
				int num = list[k];
				ComputeKernel computeKernel = BuildKernelFromSpirv(array3[k], item3, bindings, pushConstantSize);
				cache.Add((glslPath, entryPoints[num], pushConstantSize, item, language), computeKernel);
				array[num] = computeKernel;
			}
			return array;
		}
	}

	public static void WarmCache(string glslPath, string[] entryPoints, ShaderSourceLanguage language)
	{
		var (source, sourceName) = ReadSource(glslPath, language);
		RuntimeShaderCompiler.CompileBatch(source, sourceName, entryPoints, (language == ShaderSourceLanguage.Hlsl) ? RuntimeShaderLanguage.Hlsl : RuntimeShaderLanguage.Glsl, RuntimeShaderStage.Compute);
	}

	public static void ReloadAll()
	{
		using (cacheLock.EnterScope())
		{
			if (cache.Count == 0)
			{
				return;
			}
			foreach (var (tuple2, computeKernel2) in cache)
			{
				try
				{
					var (newShaderModule, newPipeline) = CompileProgram(tuple2.Item1, tuple2.Item2, computeKernel2.PipelineLayout, tuple2.Item5);
					computeKernel2.ReplaceProgram(newShaderModule, newPipeline);
				}
				catch (Exception ex)
				{
					LogCategory log = RenderLog.Log;
					bool shouldAppend;
					LogString<Error> message = new LogString<Error>(41, 2, log, out shouldAppend);
					if (shouldAppend)
					{
						message.AppendLiteral("ComputeKernelCache: failed to reload '");
						message.AppendFormatted(tuple2.Item1);
						message.AppendLiteral("': ");
						message.AppendFormatted(ex.Message);
					}
					log.Error(message, "ReloadAll", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Compute\\ComputeKernelCache.cs", 122);
				}
			}
		}
	}

	public static void DisposeAll()
	{
		using (cacheLock.EnterScope())
		{
			foreach (ComputeKernel value in cache.Values)
			{
				value.Dispose();
			}
			cache.Clear();
		}
	}

	private static (VkShaderModule module, VkPipeline pipeline) CompileProgram(string glslPath, string entryPoint, VkPipelineLayout pipelineLayout, ShaderSourceLanguage language)
	{
		(string source, string sourceName) tuple = ReadSource(glslPath, language);
		string item = tuple.source;
		string item2 = tuple.sourceName;
		VkShaderModule vkShaderModule = RuntimeShaderCompiler.CompileModule(device, item, item2, entryPoint, (language == ShaderSourceLanguage.Hlsl) ? RuntimeShaderLanguage.Hlsl : RuntimeShaderLanguage.Glsl, RuntimeShaderStage.Compute);
		return (module: vkShaderModule, pipeline: CreateComputePipeline(vkShaderModule, pipelineLayout));
	}

	public static List<(string Name, byte[] Record, bool Reused)> CompileForPack(string glslPath, string[] entryPoints, ShaderSourceLanguage language, AssetPack? previous)
	{
		var (source, sourceName) = ReadSource(glslPath, language);
		return RuntimeShaderCompiler.CompileForPack(source, sourceName, entryPoints, (language == ShaderSourceLanguage.Hlsl) ? RuntimeShaderLanguage.Hlsl : RuntimeShaderLanguage.Glsl, RuntimeShaderStage.Compute, previous);
	}

	private static (string source, string sourceName) ReadSource(string glslPath, ShaderSourceLanguage language)
	{
		if (language == ShaderSourceLanguage.Hlsl)
		{
			string text = ShaderSourceLoader.ResolveShaderPath(glslPath);
			return (source: File.ReadAllText(text), sourceName: text);
		}
		return (source: ShaderSourceLoader.ReadShaderSource(glslPath), sourceName: glslPath);
	}

	private unsafe static VkPipeline CreateComputePipeline(VkShaderModule shaderModule, VkPipelineLayout pipelineLayout)
	{
		Span<VkPipeline> span = stackalloc VkPipeline[1];
		fixed (byte* name = "main"u8)
		{
			DeviceEx handle = device;
			VkComputePipelineCreateInfo reference = new VkComputePipelineCreateInfo
			{
				Layout = pipelineLayout,
				Stage = new VkPipelineShaderStageCreateInfo
				{
					Module = shaderModule,
					Stage = VkShaderStageFlags.ComputeBit,
					Name = name
				}
			};
			handle.CreateComputePipelines(default(VkPipelineCache), new ReadOnlySpan<VkComputePipelineCreateInfo>(in reference), device.VkAllocator, span);
		}
		return span[0];
	}

	private static ComputeKernel Compile(string glslPath, string entryPoint, ReadOnlySpan<ComputeBinding> bindings, int pushConstantSize, ShaderSourceLanguage language)
	{
		var (descriptorSetLayout, pipelineLayout) = CreateKernelLayout(bindings, pushConstantSize);
		var (shaderModule, pipeline) = CompileProgram(glslPath, entryPoint, pipelineLayout, language);
		return new ComputeKernel(device, shaderModule, descriptorSetLayout, pipelineLayout, pipeline, bindings.ToArray(), pushConstantSize);
	}

	private static ComputeKernel BuildKernelFromSpirv(byte[] spirv, string sourceName, ReadOnlySpan<ComputeBinding> bindings, int pushConstantSize)
	{
		(DescriptorSetLayoutEx descriptorSetLayout, VkPipelineLayout pipelineLayout) tuple = CreateKernelLayout(bindings, pushConstantSize);
		DescriptorSetLayoutEx item = tuple.descriptorSetLayout;
		VkPipelineLayout item2 = tuple.pipelineLayout;
		VkShaderModule shaderModule = RuntimeShaderCompiler.CreateShaderModule(device, spirv, sourceName);
		VkPipeline pipeline = CreateComputePipeline(shaderModule, item2);
		return new ComputeKernel(device, shaderModule, item, item2, pipeline, bindings.ToArray(), pushConstantSize);
	}

	private unsafe static (DescriptorSetLayoutEx descriptorSetLayout, VkPipelineLayout pipelineLayout) CreateKernelLayout(ReadOnlySpan<ComputeBinding> bindings, int pushConstantSize)
	{
		VkDescriptorSetLayoutBinding[] array = new VkDescriptorSetLayoutBinding[bindings.Length];
		for (int i = 0; i < bindings.Length; i++)
		{
			array[i] = new VkDescriptorSetLayoutBinding
			{
				Binding = bindings[i].Binding,
				DescriptorType = ToDescriptorType(bindings[i].Type),
				DescriptorCount = 1,
				StageFlags = VkShaderStageFlags.ComputeBit
			};
		}
		DescriptorSetLayoutEx descriptorSetLayoutEx;
		fixed (VkDescriptorSetLayoutBinding* pointer = array)
		{
			descriptorSetLayoutEx = device.CreateDescriptorSetLayout(new DescriptorSetLayoutEx.CreateInfo
			{
				Bindings = new Span<VkDescriptorSetLayoutBinding>(pointer, array.Length)
			}, device.VkAllocator);
		}
		VkDescriptorSetLayout reference = descriptorSetLayoutEx;
		VkPipelineLayout item;
		if (pushConstantSize > 0)
		{
			VkPushConstantRange vkPushConstantRange = new VkPushConstantRange
			{
				StageFlags = VkShaderStageFlags.ComputeBit,
				Offset = ByteSize.Zero,
				Size = ByteSize.Of(pushConstantSize)
			};
			DeviceEx inDevice = device;
			ReadOnlySpan<VkDescriptorSetLayout> setLayouts = new ReadOnlySpan<VkDescriptorSetLayout>(in reference);
			VkPushConstantRange reference2 = vkPushConstantRange;
			item = inDevice.CreatePipelineLayout(setLayouts, new ReadOnlySpan<VkPushConstantRange>(in reference2), device.VkAllocator);
		}
		else
		{
			item = device.CreatePipelineLayout(new ReadOnlySpan<VkDescriptorSetLayout>(in reference), default(ReadOnlySpan<VkPushConstantRange>), device.VkAllocator);
		}
		return (descriptorSetLayout: descriptorSetLayoutEx, pipelineLayout: item);
	}

	private static VkDescriptorType ToDescriptorType(ComputeBindingType type)
	{
		return type switch
		{
			ComputeBindingType.CombinedImageSampler => VkDescriptorType.CombinedImageSampler, 
			ComputeBindingType.StorageImage => VkDescriptorType.StorageImage, 
			ComputeBindingType.StorageBuffer => VkDescriptorType.StorageBuffer, 
			ComputeBindingType.SampledImage => VkDescriptorType.SampledImage, 
			_ => throw new ArgumentOutOfRangeException("type"), 
		};
	}

	private static string BuildBindingsKey(ReadOnlySpan<ComputeBinding> bindings)
	{
		StringBuilder stringBuilder = new StringBuilder(bindings.Length * 6);
		for (int i = 0; i < bindings.Length; i++)
		{
			stringBuilder.Append(bindings[i].Binding);
			stringBuilder.Append(':');
			stringBuilder.Append((int)bindings[i].Type);
			stringBuilder.Append('|');
		}
		return stringBuilder.ToString();
	}
}
