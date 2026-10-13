using System;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Rendering.Compute;

public sealed class ComputeKernel : IDisposable
{
	private readonly DeviceEx device;

	private VkShaderModule shaderModule;

	private readonly ComputeBinding[] bindings;

	private bool disposed;

	internal DescriptorSetLayoutEx DescriptorSetLayout { get; }

	public VkPipelineLayout PipelineLayout { get; }

	public VkPipeline Pipeline { get; private set; }

	public ReadOnlySpan<ComputeBinding> Bindings => bindings;

	public int PushConstantSize { get; }

	internal ComputeKernel(DeviceEx device, VkShaderModule shaderModule, DescriptorSetLayoutEx descriptorSetLayout, VkPipelineLayout pipelineLayout, VkPipeline pipeline, ComputeBinding[] bindings, int pushConstantSize)
	{
		this.device = device;
		this.shaderModule = shaderModule;
		this.bindings = bindings;
		DescriptorSetLayout = descriptorSetLayout;
		PipelineLayout = pipelineLayout;
		Pipeline = pipeline;
		PushConstantSize = pushConstantSize;
	}

	internal void ReplaceProgram(VkShaderModule newShaderModule, VkPipeline newPipeline)
	{
		device.DestroyPipeline(Pipeline, device.VkAllocator);
		device.DestroyShaderModule(shaderModule, device.VkAllocator);
		shaderModule = newShaderModule;
		Pipeline = newPipeline;
	}

	public void Dispose()
	{
		if (!disposed)
		{
			disposed = true;
			device.DestroyPipeline(Pipeline, device.VkAllocator);
			device.DestroyPipelineLayout(PipelineLayout, device.VkAllocator);
			device.DestroyDescriptorSetLayout(DescriptorSetLayout, device.VkAllocator);
			device.DestroyShaderModule(shaderModule, device.VkAllocator);
		}
	}
}
