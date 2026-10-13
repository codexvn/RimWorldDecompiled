using System;
using System.Collections.Generic;

namespace UnityStubs.Internal;

internal sealed class UnityShaderBindingPlan
{
	public const int MaterialSet = 2;

	public const int FrameSet = 0;

	public const int FrameFragmentBinding = 1;

	public const int FrameViewProjOffset = 80;

	public const int FrameWorldToCameraOffset = 16;

	public readonly Dictionary<string, int> CBufferBase = new Dictionary<string, int>(StringComparer.Ordinal);

	public readonly Dictionary<int, int> ImageBinding = new Dictionary<int, int>();

	public readonly Dictionary<int, int> SamplerBinding = new Dictionary<int, int>();

	public readonly Dictionary<string, int> StorageBinding = new Dictionary<string, int>(StringComparer.Ordinal);

	public readonly Dictionary<string, int> MovedBlock = new Dictionary<string, int>(StringComparer.Ordinal);

	public int UniformSize;

	public static string BlockSlotName(string cbuffer)
	{
		return "$Block:" + cbuffer;
	}

	public static UnityShaderBindingPlan Build(UnityShaderPass pass)
	{
		UnityShaderBindingPlan unityShaderBindingPlan = new UnityShaderBindingPlan();
		List<UnityShaderCBuffer> list = new List<UnityShaderCBuffer>();
		int num = 0;
		foreach (UnityShaderBinding binding in pass.Bindings)
		{
			if (binding.Kind == UnityShaderBindingKind.ConstantBuffer && !list.Exists((UnityShaderCBuffer b) => b.Name == binding.Name))
			{
				UnityShaderCBuffer unityShaderCBuffer = pass.CBuffers.Find((UnityShaderCBuffer b) => b.Name == binding.Name);
				if (unityShaderCBuffer != null)
				{
					list.Add(unityShaderCBuffer);
					num += (unityShaderCBuffer.Size + 15) & -16;
				}
			}
		}
		List<UnityShaderCBuffer> list2 = new List<UnityShaderCBuffer>(list);
		list2.Sort((UnityShaderCBuffer a, UnityShaderCBuffer b) => b.Size.CompareTo(a.Size));
		foreach (UnityShaderCBuffer item in list2)
		{
			if (num <= 512)
			{
				break;
			}
			unityShaderBindingPlan.MovedBlock[item.Name] = 0;
			num -= (item.Size + 15) & -16;
		}
		foreach (UnityShaderCBuffer item2 in list)
		{
			if (!unityShaderBindingPlan.MovedBlock.ContainsKey(item2.Name))
			{
				unityShaderBindingPlan.CBufferBase[item2.Name] = unityShaderBindingPlan.UniformSize;
				unityShaderBindingPlan.UniformSize += (item2.Size + 15) & -16;
			}
		}
		int num2 = 1;
		foreach (UnityShaderBinding binding2 in pass.Bindings)
		{
			if (binding2.Kind == UnityShaderBindingKind.Texture)
			{
				int key = binding2.PackedIndex & 0xFFFFFF;
				if (!unityShaderBindingPlan.ImageBinding.ContainsKey(key))
				{
					unityShaderBindingPlan.ImageBinding[key] = num2++;
				}
				if (binding2.HasSeparateSampler && !unityShaderBindingPlan.SamplerBinding.ContainsKey(binding2.PackedSampler & 0xFFFFFF))
				{
					unityShaderBindingPlan.SamplerBinding[binding2.PackedSampler & 0xFFFFFF] = num2++;
				}
			}
		}
		foreach (UnityShaderBinding binding3 in pass.Bindings)
		{
			if (binding3.Kind == UnityShaderBindingKind.Buffer && !unityShaderBindingPlan.StorageBinding.ContainsKey(binding3.Name))
			{
				unityShaderBindingPlan.StorageBinding[binding3.Name] = num2++;
			}
		}
		foreach (UnityShaderCBuffer item3 in list)
		{
			if (unityShaderBindingPlan.MovedBlock.ContainsKey(item3.Name))
			{
				Dictionary<string, int> movedBlock = unityShaderBindingPlan.MovedBlock;
				string name = item3.Name;
				int value = (unityShaderBindingPlan.StorageBinding[BlockSlotName(item3.Name)] = num2++);
				movedBlock[name] = value;
			}
		}
		foreach (UnityShaderCBuffer cBuffer in pass.CBuffers)
		{
			if (!unityShaderBindingPlan.CBufferBase.ContainsKey(cBuffer.Name))
			{
				continue;
			}
			foreach (UnityShaderParam param in cBuffer.Params)
			{
				if (param.ArraySize > 0 && !param.IsMatrix && !unityShaderBindingPlan.StorageBinding.ContainsKey(param.Name))
				{
					unityShaderBindingPlan.StorageBinding[param.Name] = num2++;
				}
			}
		}
		return unityShaderBindingPlan;
	}
}
