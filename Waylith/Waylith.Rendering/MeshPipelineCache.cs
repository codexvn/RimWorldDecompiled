using System;
using System.Collections.Generic;
using System.Numerics;
using Brutal;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

public sealed class MeshPipelineCache : IDisposable
{
	private readonly record struct MeshPipelineKey(VkRenderPass RenderPass, VkSampleCountFlags SampleCount, long ShaderVariantId, PipelineState PipelineState, int LayoutId, bool Instanced, byte PassId)
	{
		public override int GetHashCode()
		{
			return HashCode.Combine(RenderPass.VkHandle, SampleCount, ShaderVariantId, PipelineState, LayoutId, Instanced, PassId);
		}

		public bool Equals(MeshPipelineKey other)
		{
			if (RenderPass.VkHandle == other.RenderPass.VkHandle && SampleCount == other.SampleCount && ShaderVariantId == other.ShaderVariantId && PipelineState == other.PipelineState && LayoutId == other.LayoutId && Instanced == other.Instanced)
			{
				return PassId == other.PassId;
			}
			return false;
		}
	}

	private readonly DeviceEx device;

	private readonly VulkanShaderLayoutCache shaderLayoutCache;

	private readonly ShaderModuleCache shaderModules;

	private VkPipelineCache pipelineCache;

	private readonly Dictionary<MeshPipelineKey, VkPipeline> pipelines = new Dictionary<MeshPipelineKey, VkPipeline>();

	public MeshPipelineCache(DeviceEx device, VulkanShaderLayoutCache shaderLayoutCache, ShaderModuleCache shaderModules)
	{
		this.device = device;
		this.shaderLayoutCache = shaderLayoutCache;
		this.shaderModules = shaderModules;
		pipelineCache = device.CreatePipelineCache(new VkPipelineCacheCreateInfo(), device.VkAllocator);
	}

	public unsafe VkPipeline GetOrCreatePipeline(in VkDescriptorSetLayout frameDescriptorSetLayout, in VkDescriptorSetLayout globalTextureDescriptorSetLayout, MaterialBinding mat, VertexLayout layout, in MeshTopology topology, in PipelineState pipelineState, in VkRenderPass renderPass, in VkSampleCountFlags sampleCount, int colorAttachmentCount = 1, byte passId = 0, string? diagnosticPrefix = null, bool instanced = false)
	{
		if (topology != 0)
		{
			LogDiagnostic(diagnosticPrefix, $"pipeline rejected: topology={topology} is not supported for material='{mat.DebugName}'.");
			return default(VkPipeline);
		}
		MeshPipelineKey key = new MeshPipelineKey(renderPass, sampleCount, mat.ShaderVariantId, pipelineState, layout.Id, instanced, passId);
		if (pipelines.TryGetValue(key, out var value))
		{
			return value;
		}
		VkPipelineLayout orCreatePipelineLayout = shaderLayoutCache.GetOrCreatePipelineLayout(mat.Shader, frameDescriptorSetLayout, globalTextureDescriptorSetLayout);
		ShaderVertexInputLayout vertexInputLayout = mat.ShaderVariant.Shader.VertexInputLayout;
		bool flag = vertexInputLayout.InstanceSlots.Count != 0;
		Span<VkVertexInputBindingDescription> bindings = stackalloc VkVertexInputBindingDescription[layout.StreamCount + 1 + (flag ? 1 : 0)];
		int vertexBindingDescriptionCount = FillBindingDescriptions(layout, vertexInputLayout, flag, bindings);
		Span<VkVertexInputAttributeDescription> attributes = stackalloc VkVertexInputAttributeDescription[vertexInputLayout.Slots.Count + vertexInputLayout.InstanceSlots.Count * 4];
		if (!TryFillAttributeDescriptions(layout, mat.ShaderVariant, attributes, out var count, diagnosticPrefix))
		{
			LogDiagnostic(diagnosticPrefix, $"pipeline rejected: vertex attributes did not match shader; material='{mat.DebugName}' shader='{mat.Shader.Name}' layoutId={layout.Id}.");
			return default(VkPipeline);
		}
		Shader overrideShader;
		ShaderVariantKey shaderVariantKey = (mat.Shader.TryGetPassOverride(passId, out overrideShader) ? new ShaderVariantKey(overrideShader, mat.ShaderVariant.DynamicKeywordKeyId, mat.ShaderVariant.DynamicKeywordKey, overrideShader.UseTexture, outlineMrt: false, instanced) : (instanced ? new ShaderVariantKey(mat.ShaderVariant.Shader, mat.ShaderVariant.DynamicKeywordKeyId, mat.ShaderVariant.DynamicKeywordKey, mat.ShaderVariant.UseTexture, outlineMrt: false, instanced: true) : mat.ShaderVariant));
		bool usedFallback;
		VkShaderModule module = ((usedFallback = mat.Shader.HasErrors || shaderVariantKey.Shader.HasErrors) ? shaderModules.GetOrCreateMagentaVertexShader() : shaderModules.GetOrCreateVertexShader(shaderVariantKey, out usedFallback));
		VkShaderModule module2;
		if (usedFallback)
		{
			module2 = shaderModules.GetOrCreateMagentaFragmentShader();
		}
		else if (colorAttachmentCount == 0)
		{
			module2 = (shaderVariantKey.UseTexture ? shaderModules.GetOrCreateDepthOnlyAlphaTestFragmentShader() : shaderModules.GetOrCreateDepthOnlyFragmentShader());
		}
		else
		{
			ShaderVariantKey variant = ((colorAttachmentCount > 1) ? new ShaderVariantKey(shaderVariantKey.Shader, shaderVariantKey.DynamicKeywordKeyId, shaderVariantKey.DynamicKeywordKey, shaderVariantKey.UseTexture, outlineMrt: true, shaderVariantKey.Instanced) : shaderVariantKey);
			module2 = shaderModules.GetOrCreateFragmentShader(variant);
		}
		byte* ptr = stackalloc byte[5];
		*ptr = 109;
		ptr[1] = 97;
		ptr[2] = 105;
		ptr[3] = 110;
		ptr[4] = 0;
		VkPipelineShaderStageCreateInfo* ptr2 = stackalloc VkPipelineShaderStageCreateInfo[2];
		*ptr2 = new VkPipelineShaderStageCreateInfo
		{
			Stage = VkShaderStageFlags.VertexBit,
			Module = module,
			Name = ptr
		};
		ptr2[1] = new VkPipelineShaderStageCreateInfo
		{
			Stage = VkShaderStageFlags.FragmentBit,
			Module = module2,
			Name = ptr
		};
		fixed (VkVertexInputBindingDescription* vertexBindingDescriptions = bindings)
		{
			fixed (VkVertexInputAttributeDescription* vertexAttributeDescriptions = attributes)
			{
				VkPipelineVertexInputStateCreateInfo vkPipelineVertexInputStateCreateInfo = new VkPipelineVertexInputStateCreateInfo();
				vkPipelineVertexInputStateCreateInfo.VertexBindingDescriptionCount = vertexBindingDescriptionCount;
				vkPipelineVertexInputStateCreateInfo.VertexBindingDescriptions = vertexBindingDescriptions;
				vkPipelineVertexInputStateCreateInfo.VertexAttributeDescriptionCount = count;
				vkPipelineVertexInputStateCreateInfo.VertexAttributeDescriptions = vertexAttributeDescriptions;
				VkPipelineVertexInputStateCreateInfo vkPipelineVertexInputStateCreateInfo2 = vkPipelineVertexInputStateCreateInfo;
				VkPipelineInputAssemblyStateCreateInfo vkPipelineInputAssemblyStateCreateInfo = new VkPipelineInputAssemblyStateCreateInfo();
				vkPipelineInputAssemblyStateCreateInfo.Topology = VkPrimitiveTopology.TriangleList;
				vkPipelineInputAssemblyStateCreateInfo.PrimitiveRestartEnable = false;
				VkPipelineInputAssemblyStateCreateInfo vkPipelineInputAssemblyStateCreateInfo2 = vkPipelineInputAssemblyStateCreateInfo;
				VkPipelineViewportStateCreateInfo vkPipelineViewportStateCreateInfo = new VkPipelineViewportStateCreateInfo();
				vkPipelineViewportStateCreateInfo.ViewportCount = 1;
				vkPipelineViewportStateCreateInfo.Viewports = null;
				vkPipelineViewportStateCreateInfo.ScissorCount = 1;
				vkPipelineViewportStateCreateInfo.Scissors = null;
				VkPipelineViewportStateCreateInfo vkPipelineViewportStateCreateInfo2 = vkPipelineViewportStateCreateInfo;
				Span<VkDynamicState> span = stackalloc VkDynamicState[2];
				span[0] = VkDynamicState.Viewport;
				span[1] = VkDynamicState.Scissor;
				fixed (VkDynamicState* dynamicStates = span)
				{
					VkPipelineRasterizationStateCreateInfo vkPipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfo();
					vkPipelineRasterizationStateCreateInfo.DepthClampEnable = false;
					vkPipelineRasterizationStateCreateInfo.RasterizerDiscardEnable = false;
					vkPipelineRasterizationStateCreateInfo.PolygonMode = VkPolygonMode.Fill;
					vkPipelineRasterizationStateCreateInfo.CullMode = pipelineState.Cull switch
					{
						FaceCull.Back => VkCullModeFlags.BackBit, 
						FaceCull.Front => VkCullModeFlags.FrontBit, 
						_ => VkCullModeFlags.None, 
					};
					vkPipelineRasterizationStateCreateInfo.FrontFace = VkFrontFace.Clockwise;
					vkPipelineRasterizationStateCreateInfo.DepthBiasEnable = pipelineState.DepthBias.Enabled;
					vkPipelineRasterizationStateCreateInfo.DepthBiasConstantFactor = pipelineState.DepthBias.ConstantFactor;
					vkPipelineRasterizationStateCreateInfo.DepthBiasClamp = 0f;
					vkPipelineRasterizationStateCreateInfo.DepthBiasSlopeFactor = pipelineState.DepthBias.SlopeFactor;
					vkPipelineRasterizationStateCreateInfo.LineWidth = 1f;
					VkPipelineRasterizationStateCreateInfo vkPipelineRasterizationStateCreateInfo2 = vkPipelineRasterizationStateCreateInfo;
					VkPipelineMultisampleStateCreateInfo vkPipelineMultisampleStateCreateInfo = new VkPipelineMultisampleStateCreateInfo();
					vkPipelineMultisampleStateCreateInfo.RasterizationSamples = sampleCount;
					vkPipelineMultisampleStateCreateInfo.AlphaToCoverageEnable = false;
					vkPipelineMultisampleStateCreateInfo.AlphaToOneEnable = false;
					VkPipelineMultisampleStateCreateInfo vkPipelineMultisampleStateCreateInfo2 = vkPipelineMultisampleStateCreateInfo;
					StencilState stencil = pipelineState.Stencil;
					VkStencilOpState vkStencilOpState = new VkStencilOpState();
					vkStencilOpState.FailOp = ToVkStencilOp(stencil.FailOp);
					vkStencilOpState.PassOp = ToVkStencilOp(stencil.PassOp);
					vkStencilOpState.DepthFailOp = ToVkStencilOp(stencil.DepthFailOp);
					vkStencilOpState.CompareOp = ToVkCompareOp(stencil.Compare);
					vkStencilOpState.CompareMask = stencil.CompareMask;
					vkStencilOpState.WriteMask = stencil.WriteMask;
					vkStencilOpState.Reference = stencil.Reference;
					VkStencilOpState vkStencilOpState2 = vkStencilOpState;
					VkPipelineDepthStencilStateCreateInfo vkPipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfo();
					vkPipelineDepthStencilStateCreateInfo.DepthTestEnable = pipelineState.Depth.Test;
					vkPipelineDepthStencilStateCreateInfo.DepthWriteEnable = pipelineState.Depth.Write;
					vkPipelineDepthStencilStateCreateInfo.DepthCompareOp = VkCompareOp.GreaterOrEqual;
					vkPipelineDepthStencilStateCreateInfo.DepthBoundsTestEnable = false;
					vkPipelineDepthStencilStateCreateInfo.StencilTestEnable = stencil.Enabled;
					vkPipelineDepthStencilStateCreateInfo.Front = vkStencilOpState2;
					vkPipelineDepthStencilStateCreateInfo.Back = vkStencilOpState2;
					VkPipelineDepthStencilStateCreateInfo vkPipelineDepthStencilStateCreateInfo2 = vkPipelineDepthStencilStateCreateInfo;
					BlendState blend = pipelineState.Blend;
					VkColorComponentFlags colorWriteMask = (pipelineState.ColorWrite ? (VkColorComponentFlags.RBit | VkColorComponentFlags.GBit | VkColorComponentFlags.BBit | VkColorComponentFlags.ABit) : VkColorComponentFlags.None);
					VkPipelineColorBlendAttachmentState vkPipelineColorBlendAttachmentState = new VkPipelineColorBlendAttachmentState();
					vkPipelineColorBlendAttachmentState.BlendEnable = blend.Enabled;
					vkPipelineColorBlendAttachmentState.SrcColorBlendFactor = ToVkBlendFactor(blend.SrcColor);
					vkPipelineColorBlendAttachmentState.DstColorBlendFactor = ToVkBlendFactor(blend.DstColor);
					vkPipelineColorBlendAttachmentState.ColorBlendOp = ToVkBlendOp(blend.ColorOp);
					vkPipelineColorBlendAttachmentState.SrcAlphaBlendFactor = ToVkBlendFactor(blend.SrcAlpha);
					vkPipelineColorBlendAttachmentState.DstAlphaBlendFactor = ToVkBlendFactor(blend.DstAlpha);
					vkPipelineColorBlendAttachmentState.AlphaBlendOp = ToVkBlendOp(blend.AlphaOp);
					vkPipelineColorBlendAttachmentState.ColorWriteMask = colorWriteMask;
					VkPipelineColorBlendAttachmentState vkPipelineColorBlendAttachmentState2 = vkPipelineColorBlendAttachmentState;
					vkPipelineColorBlendAttachmentState = new VkPipelineColorBlendAttachmentState();
					vkPipelineColorBlendAttachmentState.BlendEnable = false;
					vkPipelineColorBlendAttachmentState.ColorWriteMask = colorWriteMask;
					VkPipelineColorBlendAttachmentState vkPipelineColorBlendAttachmentState3 = vkPipelineColorBlendAttachmentState;
					VkPipelineColorBlendAttachmentState* ptr3 = stackalloc VkPipelineColorBlendAttachmentState[(colorAttachmentCount <= 0) ? 1 : colorAttachmentCount];
					if (colorAttachmentCount > 0)
					{
						*ptr3 = vkPipelineColorBlendAttachmentState2;
					}
					for (int i = 1; i < colorAttachmentCount; i++)
					{
						ptr3[i] = vkPipelineColorBlendAttachmentState3;
					}
					VkPipelineColorBlendStateCreateInfo vkPipelineColorBlendStateCreateInfo = new VkPipelineColorBlendStateCreateInfo();
					vkPipelineColorBlendStateCreateInfo.LogicOpEnable = false;
					vkPipelineColorBlendStateCreateInfo.AttachmentCount = colorAttachmentCount;
					vkPipelineColorBlendStateCreateInfo.Attachments = ((colorAttachmentCount > 0) ? ptr3 : null);
					VkPipelineColorBlendStateCreateInfo vkPipelineColorBlendStateCreateInfo2 = vkPipelineColorBlendStateCreateInfo;
					VkPipelineDynamicStateCreateInfo vkPipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo();
					vkPipelineDynamicStateCreateInfo.DynamicStateCount = span.Length;
					vkPipelineDynamicStateCreateInfo.DynamicStates = dynamicStates;
					VkPipelineDynamicStateCreateInfo vkPipelineDynamicStateCreateInfo2 = vkPipelineDynamicStateCreateInfo;
					VkGraphicsPipelineCreateInfo vkGraphicsPipelineCreateInfo = new VkGraphicsPipelineCreateInfo();
					vkGraphicsPipelineCreateInfo.StageCount = 2;
					vkGraphicsPipelineCreateInfo.Stages = ptr2;
					vkGraphicsPipelineCreateInfo.VertexInputState = &vkPipelineVertexInputStateCreateInfo2;
					vkGraphicsPipelineCreateInfo.InputAssemblyState = &vkPipelineInputAssemblyStateCreateInfo2;
					vkGraphicsPipelineCreateInfo.TessellationState = null;
					vkGraphicsPipelineCreateInfo.ViewportState = &vkPipelineViewportStateCreateInfo2;
					vkGraphicsPipelineCreateInfo.RasterizationState = &vkPipelineRasterizationStateCreateInfo2;
					vkGraphicsPipelineCreateInfo.MultisampleState = &vkPipelineMultisampleStateCreateInfo2;
					vkGraphicsPipelineCreateInfo.DepthStencilState = &vkPipelineDepthStencilStateCreateInfo2;
					vkGraphicsPipelineCreateInfo.ColorBlendState = &vkPipelineColorBlendStateCreateInfo2;
					vkGraphicsPipelineCreateInfo.DynamicState = &vkPipelineDynamicStateCreateInfo2;
					vkGraphicsPipelineCreateInfo.Layout = orCreatePipelineLayout;
					vkGraphicsPipelineCreateInfo.RenderPass = renderPass;
					vkGraphicsPipelineCreateInfo.Subpass = 0;
					VkGraphicsPipelineCreateInfo vkGraphicsPipelineCreateInfo2 = vkGraphicsPipelineCreateInfo;
					Span<VkPipeline> span2 = stackalloc VkPipeline[1];
					DeviceEx handle = device;
					VkPipelineCache vkPipelineCache = pipelineCache;
					VkGraphicsPipelineCreateInfo reference = vkGraphicsPipelineCreateInfo2;
					VkResult vkResult = handle.CreateGraphicsPipelines(vkPipelineCache, new ReadOnlySpan<VkGraphicsPipelineCreateInfo>(in reference), device.VkAllocator, span2);
					if (vkResult != 0)
					{
						LogDiagnostic(diagnosticPrefix, $"pipeline creation failed: result={vkResult} material='{mat.DebugName}' shader='{mat.Shader.Name}' variant='{ShaderModuleCache.FormatVariantName(mat.ShaderVariant)}' layoutId={layout.Id} sampleCount={sampleCount} depthTest={pipelineState.Depth.Test} depthWrite={pipelineState.Depth.Write} blend={pipelineState.Blend.Enabled}.");
						return default(VkPipeline);
					}
					VkPipeline vkPipeline = span2[0];
					pipelines.Add(key, vkPipeline);
					return vkPipeline;
				}
			}
		}
	}

	private unsafe static int FillBindingDescriptions(VertexLayout layout, ShaderVertexInputLayout vertexInputLayout, bool includeInstanceBinding, Span<VkVertexInputBindingDescription> bindings)
	{
		for (int i = 0; i < layout.StreamCount; i++)
		{
			bindings[i] = new VkVertexInputBindingDescription
			{
				Binding = i,
				Stride = ByteSize.Of(layout.GetStreamStride(i)),
				InputRate = VkVertexInputRate.Vertex
			};
		}
		int optionalFallbackBinding = GetOptionalFallbackBinding(layout);
		bindings[optionalFallbackBinding] = new VkVertexInputBindingDescription
		{
			Binding = optionalFallbackBinding,
			Stride = ByteSize.Of(OptionalVertexInputDefaults.GetStrideBytes(vertexInputLayout)),
			InputRate = VkVertexInputRate.Instance
		};
		if (!includeInstanceBinding)
		{
			return layout.StreamCount + 1;
		}
		int instanceBinding = GetInstanceBinding(layout);
		bindings[instanceBinding] = new VkVertexInputBindingDescription
		{
			Binding = instanceBinding,
			Stride = ByteSize.Of(sizeof(Matrix4x4)),
			InputRate = VkVertexInputRate.Instance
		};
		return layout.StreamCount + 2;
	}

	private static bool TryFillAttributeDescriptions(VertexLayout layout, ShaderVariantKey variant, Span<VkVertexInputAttributeDescription> attributes, out int count, string? diagnosticPrefix = null)
	{
		count = 0;
		bool shouldAppend;
		foreach (ShaderVertexInputSlot slot in variant.Shader.VertexInputLayout.Slots)
		{
			if (!layout.TryGetAttribute(slot.semantic, out var attribute))
			{
				if (slot.required)
				{
					LogCategory log = RenderLog.Log;
					LogCategory logCategory = log;
					LogString<Warning> message = new LogString<Warning>(106, 5, log, out shouldAppend);
					if (shouldAppend)
					{
						message.AppendFormatted(FormatPrefix(diagnosticPrefix));
						message.AppendLiteral("Mesh pipeline rejected shader '");
						message.AppendFormatted(variant.Shader.Name);
						message.AppendLiteral("' because required vertex input ");
						message.AppendFormatted(slot.semantic);
						message.AppendLiteral(" at location ");
						message.AppendFormatted(slot.location);
						message.AppendLiteral(" is missing from mesh layout ");
						message.AppendFormatted(FormatVertexLayout(layout));
						message.AppendLiteral(".");
					}
					logCategory.Warning(message, "TryFillAttributeDescriptions", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Materials\\MeshPipelineCache.cs", 367);
					shouldAppend = false;
					return shouldAppend;
				}
				if (!OptionalVertexInputDefaults.TryGetFormat(slot.semantic, out var format))
				{
					RenderLog.Log.Warning($"{FormatPrefix(diagnosticPrefix)}Mesh pipeline rejected shader '{variant.Shader.Name}' because optional vertex input {slot.semantic} at location {slot.location} is missing from mesh layout {FormatVertexLayout(layout)} " + "and has no renderer default value.", "TryFillAttributeDescriptions", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Materials\\MeshPipelineCache.cs", 375);
					shouldAppend = false;
					return shouldAppend;
				}
				attributes[count++] = new VkVertexInputAttributeDescription
				{
					Location = slot.location,
					Binding = GetOptionalFallbackBinding(layout),
					Format = format,
					Offset = ByteSize.Of(slot.defaultOffsetBytes)
				};
			}
			else
			{
				attributes[count++] = new VkVertexInputAttributeDescription
				{
					Location = slot.location,
					Binding = attribute.Stream,
					Format = ToVkVertexFormat(attribute),
					Offset = ByteSize.Of(attribute.OffsetBytes)
				};
			}
		}
		int instanceBinding = GetInstanceBinding(layout);
		foreach (ShaderInstanceInputSlot instanceSlot in variant.Shader.VertexInputLayout.InstanceSlots)
		{
			if (instanceSlot.semantic == ShaderInstanceInputSemantic.ModelMatrix)
			{
				AddModelMatrixAttributes(attributes, ref count, instanceSlot.location, instanceBinding);
				continue;
			}
			LogCategory log = RenderLog.Log;
			LogCategory logCategory2 = log;
			bool shouldAppend2;
			LogString<Warning> message = new LogString<Warning>(74, 3, log, out shouldAppend2);
			if (shouldAppend2)
			{
				message.AppendFormatted(FormatPrefix(diagnosticPrefix));
				message.AppendLiteral("Mesh pipeline rejected shader '");
				message.AppendFormatted(variant.Shader.Name);
				message.AppendLiteral("' because instance input ");
				message.AppendFormatted(instanceSlot.semantic);
				message.AppendLiteral(" is not supported.");
			}
			logCategory2.Warning(message, "TryFillAttributeDescriptions", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Materials\\MeshPipelineCache.cs", 411);
			shouldAppend = false;
			return shouldAppend;
		}
		return true;
	}

	public static int GetOptionalFallbackBinding(VertexLayout layout)
	{
		return layout.StreamCount;
	}

	public static int GetInstanceBinding(VertexLayout layout)
	{
		return layout.StreamCount + 1;
	}

	private static void AddModelMatrixAttributes(Span<VkVertexInputAttributeDescription> attributes, ref int count, int location, int binding)
	{
		for (int i = 0; i < 4; i++)
		{
			attributes[count++] = new VkVertexInputAttributeDescription
			{
				Location = location + i,
				Binding = binding,
				Format = VkFormat.R32G32B32A32SFloat,
				Offset = ByteSize.Of(i * 16)
			};
		}
	}

	private static string FormatVertexLayout(VertexLayout layout)
	{
		if (layout.Attributes.Length == 0)
		{
			return "<empty>";
		}
		string[] array = new string[layout.Attributes.Length];
		for (int i = 0; i < layout.Attributes.Length; i++)
		{
			VertexAttribute vertexAttribute = layout.Attributes[i];
			array[i] = $"{vertexAttribute.Semantic}:{vertexAttribute.Format}x{vertexAttribute.Dimension}@s{vertexAttribute.Stream}+{vertexAttribute.OffsetBytes}";
		}
		return string.Join(", ", array);
	}

	private static void LogDiagnostic(string? diagnosticPrefix, string message)
	{
		if (diagnosticPrefix != null)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message2 = new LogString<Warning>(1, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendFormatted(diagnosticPrefix);
				message2.AppendLiteral(" ");
				message2.AppendFormatted(message);
			}
			log.Warning(message2, "LogDiagnostic", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Materials\\MeshPipelineCache.cs", 463);
		}
	}

	private static string FormatPrefix(string? diagnosticPrefix)
	{
		if (diagnosticPrefix != null)
		{
			return diagnosticPrefix + " ";
		}
		return string.Empty;
	}

	private static VkFormat ToVkVertexFormat(VertexAttribute attribute)
	{
		VertexAttributeFormat format = attribute.Format;
		int dimension = attribute.Dimension;
		switch (format)
		{
		case VertexAttributeFormat.Float32:
			switch (dimension)
			{
			case 2:
				return VkFormat.R32G32SFloat;
			case 3:
				return VkFormat.R32G32B32SFloat;
			case 4:
				return VkFormat.R32G32B32A32SFloat;
			}
			break;
		case VertexAttributeFormat.Float16:
			switch (dimension)
			{
			case 2:
				return VkFormat.R16G16SFloat;
			case 4:
				return VkFormat.R16G16B16A16SFloat;
			}
			break;
		case VertexAttributeFormat.UNorm8:
			if (dimension != 4)
			{
				break;
			}
			return VkFormat.R8G8B8A8UNorm;
		case VertexAttributeFormat.UInt8:
			if (dimension != 4)
			{
				break;
			}
			return VkFormat.R8G8B8A8UInt;
		}
		throw new NotSupportedException($"Unsupported mesh attribute format {attribute.Format}x{attribute.Dimension}.");
	}

	private static VkStencilOp ToVkStencilOp(StencilOp op)
	{
		return op switch
		{
			StencilOp.Keep => VkStencilOp.Keep, 
			StencilOp.Zero => VkStencilOp.Zero, 
			StencilOp.Replace => VkStencilOp.Replace, 
			StencilOp.IncrementClamp => VkStencilOp.IncrementAndClamp, 
			StencilOp.DecrementClamp => VkStencilOp.DecrementAndClamp, 
			StencilOp.Invert => VkStencilOp.Invert, 
			StencilOp.IncrementWrap => VkStencilOp.IncrementAndWrap, 
			StencilOp.DecrementWrap => VkStencilOp.DecrementAndWrap, 
			_ => VkStencilOp.Keep, 
		};
	}

	private static VkCompareOp ToVkCompareOp(StencilCompare compare)
	{
		return compare switch
		{
			StencilCompare.Never => VkCompareOp.Never, 
			StencilCompare.Less => VkCompareOp.Less, 
			StencilCompare.Equal => VkCompareOp.Equal, 
			StencilCompare.LessOrEqual => VkCompareOp.LessOrEqual, 
			StencilCompare.Greater => VkCompareOp.Greater, 
			StencilCompare.NotEqual => VkCompareOp.NotEqual, 
			StencilCompare.GreaterOrEqual => VkCompareOp.GreaterOrEqual, 
			StencilCompare.Always => VkCompareOp.Always, 
			_ => VkCompareOp.Always, 
		};
	}

	private static VkBlendFactor ToVkBlendFactor(BlendFactor factor)
	{
		return factor switch
		{
			BlendFactor.Zero => VkBlendFactor.Zero, 
			BlendFactor.One => VkBlendFactor.One, 
			BlendFactor.SrcAlpha => VkBlendFactor.SrcAlpha, 
			BlendFactor.OneMinusSrcAlpha => VkBlendFactor.OneMinusSrcAlpha, 
			BlendFactor.SrcColor => VkBlendFactor.SrcColor, 
			BlendFactor.DstColor => VkBlendFactor.DstColor, 
			BlendFactor.OneMinusSrcColor => VkBlendFactor.OneMinusSrcColor, 
			BlendFactor.OneMinusDstColor => VkBlendFactor.OneMinusDstColor, 
			_ => VkBlendFactor.One, 
		};
	}

	private static VkBlendOp ToVkBlendOp(BlendOp op)
	{
		return op switch
		{
			BlendOp.Add => VkBlendOp.Add, 
			BlendOp.Subtract => VkBlendOp.Subtract, 
			BlendOp.ReverseSubtract => VkBlendOp.ReverseSubtract, 
			BlendOp.Min => VkBlendOp.Min, 
			BlendOp.Max => VkBlendOp.Max, 
			_ => VkBlendOp.Add, 
		};
	}

	public void InvalidateAll()
	{
		foreach (VkPipeline value in pipelines.Values)
		{
			device.DestroyPipeline(value, device.VkAllocator);
		}
		pipelines.Clear();
	}

	public void Dispose()
	{
		InvalidateAll();
		if (!pipelineCache.Equals(default(VkPipelineCache)))
		{
			device.DestroyPipelineCache(pipelineCache, device.VkAllocator);
			pipelineCache = default(VkPipelineCache);
		}
	}
}
