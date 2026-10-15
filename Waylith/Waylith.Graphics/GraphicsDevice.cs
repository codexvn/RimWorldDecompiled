using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Brutal;
using Brutal.GlfwApi;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.Strings;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;
using Waylith.Rendering.Compute;

namespace Waylith.Graphics;

public sealed class GraphicsDevice : IDisposable
{
	private readonly record struct QueueSelection(PhysicalDevice Device, int GraphicsFamilyIndex, int PresentFamilyIndex, long Score);

	private const string DeviceOverrideEnvVar = "WAYLITH_VULKAN_DEVICE";

	private const string VramBudgetEnvVar = "WAYLITH_VRAM_BUDGET_MB";

	private readonly object _graphicsQueueSyncRoot = new object();

	private readonly object _videoDecodeQueueSyncRoot = new object();

	private bool _disposed;

	private static readonly LogCategory Logger = LogCategory.Make("GraphicsDevice");

	private static Action<string> OnError = delegate(string s)
	{
		RenderLog.Log.Error(s, "OnError", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 32);
	};

	private static Action<string> OnWarning = delegate(string s)
	{
		RenderLog.Log.Warning(s, "OnWarning", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 33);
	};

	private VkDebugUtilsMessengerEXT _debugMessenger;

	public const string ExtFullScreenExclusiveExtensionName = "VK_EXT_full_screen_exclusive";

	private static readonly string[] VideoDecodeExtensions = new string[4] { "VK_KHR_synchronization2", "VK_KHR_video_queue", "VK_KHR_video_decode_queue", "VK_KHR_video_decode_h264" };

	public Instance Instance { get; }

	public PhysicalDevice PhysicalDevice { get; }

	public int ApiVersion { get; }

	public static int InstanceApiVersion { get; private set; } = 4202496;

	public DeviceEx Device { get; }

	public VmaAllocator Allocator { get; }

	public Queue GraphicsQueue { get; }

	public Queue PresentQueue { get; }

	public int GraphicsFamilyIndex { get; }

	public int PresentFamilyIndex { get; }

	public Queue? VideoDecodeQueue { get; }

	public int VideoDecodeFamilyIndex { get; }

	public IReadOnlyCollection<string> InstanceEnabledExtensions { get; }

	public IReadOnlyCollection<string> InstanceEnabledLayers { get; }

	public IReadOnlyCollection<string> DeviceEnabledExtensions { get; }

	public bool DualSourceBlendEnabled { get; }

	public bool ValidationEnabled { get; }

	public MeshUploader MeshUploads { get; }

	public bool MemoryBudgetEnabled { get; }

	public bool FullscreenExclusiveDisallowed { get; }

	public long VramLimitBytes { get; }

	public VulkanShaderLayoutCache ShaderLayoutCache { get; }

	internal object GraphicsQueueSyncRoot => _graphicsQueueSyncRoot;

	internal Action? BeforeSubmit { get; set; }

	public static IReadOnlyList<GpuDescription> EnumeratedGpus { get; private set; } = Array.Empty<GpuDescription>();

	private unsafe GraphicsDevice(Instance instance, PhysicalDevice physicalDevice, DeviceEx device, int graphicsFamilyIndex, int presentFamilyIndex, int videoDecodeFamilyIndex, HashSet<string> instanceEnabledExtensions, HashSet<string> instanceEnabledLayers, HashSet<string> deviceEnabledExtensions, bool dualSourceBlendEnabled, bool validationEnabled, int framesInFlight, bool disallowFullscreenExclusive)
	{
		Instance = instance;
		PhysicalDevice = physicalDevice;
		Device = device;
		GraphicsFamilyIndex = graphicsFamilyIndex;
		PresentFamilyIndex = presentFamilyIndex;
		InstanceEnabledExtensions = new HashSet<string>(instanceEnabledExtensions, StringComparer.Ordinal);
		InstanceEnabledLayers = new HashSet<string>(instanceEnabledLayers, StringComparer.Ordinal);
		DeviceEnabledExtensions = new HashSet<string>(deviceEnabledExtensions, StringComparer.Ordinal);
		DualSourceBlendEnabled = dualSourceBlendEnabled;
		ValidationEnabled = validationEnabled;
		GraphicsQueue = device.GetQueue(graphicsFamilyIndex, 0);
		PresentQueue = device.GetQueue(presentFamilyIndex, 0);
		VideoDecodeFamilyIndex = videoDecodeFamilyIndex;
		bool shouldAppend;
		LogString<Information> message;
		LogCategory logger;
		if (videoDecodeFamilyIndex >= 0)
		{
			VideoDecodeQueue = device.GetQueue(videoDecodeFamilyIndex, 0);
			logger = Logger;
			LogCategory logCategory = logger;
			message = new LogString<Information>(45, 1, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Vulkan Video decode enabled on queue family ");
				message.AppendFormatted(videoDecodeFamilyIndex);
				message.AppendLiteral(".");
			}
			logCategory.Info(message, ".ctor", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 68);
		}
		int apiVersion = physicalDevice.GetProperties().ApiVersion;
		ApiVersion = VulkanVersion.Min(InstanceApiVersion, apiVersion);
		logger = Logger;
		LogCategory logCategory2 = logger;
		message = new LogString<Information>(47, 3, logger, out shouldAppend);
		if (shouldAppend)
		{
			message.AppendLiteral("Effective Vulkan version ");
			message.AppendFormatted(VulkanVersion.Format(ApiVersion));
			message.AppendLiteral(" (instance ");
			message.AppendFormatted(VulkanVersion.Format(InstanceApiVersion));
			message.AppendLiteral(", device ");
			message.AppendFormatted(VulkanVersion.Format(apiVersion));
			message.AppendLiteral(").");
		}
		logCategory2.Info(message, ".ctor", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 72);
		string text = (VulkanVersion.AtLeast(ApiVersion, 4198400) ? "spirv_1_3" : "spirv_1_0");
		RuntimeShaderCompiler.ConfigureTargetProfile(text);
		if (text != "spirv_1_3")
		{
			logger = Logger;
			LogCategory logCategory3 = logger;
			LogString<Warning> message2 = new LogString<Warning>(86, 2, logger, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendLiteral("Vulkan ");
				message2.AppendFormatted(VulkanVersion.Format(ApiVersion));
				message2.AppendLiteral(" device, shaders will be compiled for ");
				message2.AppendFormatted(text);
				message2.AppendLiteral(" and baked shader packs will not be used.");
			}
			logCategory3.Warning(message2, ".ctor", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 76);
		}
		MemoryBudgetEnabled = deviceEnabledExtensions.Contains("VK_EXT_memory_budget") && (VulkanVersion.AtLeast(ApiVersion, 4198400) || instance.IsExtensionEnabled("VK_KHR_get_physical_device_properties2"));
		FullscreenExclusiveDisallowed = disallowFullscreenExclusive && instance.IsExtensionEnabled("VK_KHR_get_surface_capabilities2") && deviceEnabledExtensions.Contains("VK_EXT_full_screen_exclusive");
		if (disallowFullscreenExclusive)
		{
			Logger.Info(FullscreenExclusiveDisallowed ? "Fullscreen exclusive disallowed, swapchains will be created with VK_FULL_SCREEN_EXCLUSIVE_DISALLOWED_EXT." : "Fullscreen exclusive disallow requested but VK_EXT_full_screen_exclusive or VK_KHR_get_surface_capabilities2 is unavailable, ignoring.", ".ctor", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 84);
		}
		VramLimitBytes = ReadVramLimitOverride();
		Allocator = CreateAllocator(instance, physicalDevice, device, ApiVersion, MemoryBudgetEnabled, VramLimitBytes);
		Logger.Info(MemoryBudgetEnabled ? "VK_EXT_memory_budget enabled, VRAM budget figures are driver-reported." : "VK_EXT_memory_budget unavailable, VRAM budget figures are VMA estimates.", ".ctor", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 89);
		MeshUploads = new MeshUploader(this, framesInFlight);
		ShaderLayoutCache = new VulkanShaderLayoutCache(device);
		GpuInfo.Configure(physicalDevice, GetDeviceLocalMemoryBytes(physicalDevice), ApiVersion);
		if (instance.IsExtensionEnabled("VK_EXT_debug_utils"))
		{
			_debugMessenger = instance.CreateDebugUtilsMessengerEXT(VulkanHelpers.BuildDefaultDebugMessengerCreateInfo(new PfnVkDebugUtilsMessengerCallbackEXT((delegate* unmanaged[Cdecl]<VkDebugUtilsMessageSeverityFlagsEXT, VkDebugUtilsMessageTypeFlagsEXT, VkDebugUtilsMessengerCallbackDataEXT*, void*, Bool32>)(delegate*<VkDebugUtilsMessageSeverityFlagsEXT, VkDebugUtilsMessageTypeFlagsEXT, VkDebugUtilsMessengerCallbackDataEXT*, void*, Bool32>)(&DebugUtilsCallback))), null);
		}
	}

	public unsafe static Instance CreateInstance(string applicationName, string engineName, bool disallowFullscreenExclusive = false)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(applicationName + "\0");
		byte[] bytes2 = Encoding.UTF8.GetBytes(engineName + "\0");
		int num = VulkanVersion.QueryLoaderVersion();
		InstanceApiVersion = VulkanVersion.Min(4202496, num);
		bool shouldAppend;
		if (!VulkanVersion.AtLeast(InstanceApiVersion, 4202496))
		{
			LogCategory logger = Logger;
			LogString<Warning> message = new LogString<Warning>(42, 2, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Vulkan loader is ");
				message.AppendFormatted(VulkanVersion.Format(num));
				message.AppendLiteral(", requesting a ");
				message.AppendFormatted(VulkanVersion.Format(InstanceApiVersion));
				message.AppendLiteral(" instance.");
			}
			logger.Warning(message, "CreateInstance", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 139);
		}
		HashSet<string> enabledInstanceExtensions = GetEnabledInstanceExtensions(disallowFullscreenExclusive);
		HashSet<string> enabledInstanceLayers = GetEnabledInstanceLayers(out shouldAppend, emitUnsupportedWarning: true);
		bool flag = enabledInstanceExtensions.Contains("VK_EXT_debug_utils");
		if (!flag)
		{
			Logger.Warning("VK_EXT_debug_utils unavailable, running without Vulkan debug messages and object names.", "CreateInstance", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 144);
		}
		fixed (byte* applicationName2 = bytes)
		{
			fixed (byte* engineName2 = bytes2)
			{
				VkApplicationInfo vkApplicationInfo = new VkApplicationInfo();
				vkApplicationInfo.ApplicationName = applicationName2;
				vkApplicationInfo.ApplicationVersion = 0;
				vkApplicationInfo.EngineName = engineName2;
				vkApplicationInfo.EngineVersion = 0;
				vkApplicationInfo.ApiVersion = InstanceApiVersion;
				VkApplicationInfo appInfo = vkApplicationInfo;
				VkInstanceCreateFlags platformRequiredInstanceCreateFlags = VulkanHelpers.GetPlatformRequiredInstanceCreateFlags();
				VkDebugUtilsMessengerCreateInfoEXT vkDebugUtilsMessengerCreateInfoEXT = VulkanHelpers.BuildDefaultDebugMessengerCreateInfo(new PfnVkDebugUtilsMessengerCallbackEXT((delegate* unmanaged[Cdecl]<VkDebugUtilsMessageSeverityFlagsEXT, VkDebugUtilsMessageTypeFlagsEXT, VkDebugUtilsMessengerCallbackDataEXT*, void*, Bool32>)(delegate*<VkDebugUtilsMessageSeverityFlagsEXT, VkDebugUtilsMessageTypeFlagsEXT, VkDebugUtilsMessengerCallbackDataEXT*, void*, Bool32>)(&DebugUtilsCallback)));
				VkValidationFeatureEnableEXT* enabledValidationFeatures = stackalloc VkValidationFeatureEnableEXT[2]
				{
					VkValidationFeatureEnableEXT.BestPracticesEXT,
					VkValidationFeatureEnableEXT.SynchronizationValidationEXT
				};
				VkValidationFeaturesEXT vkValidationFeaturesEXT = new VkValidationFeaturesEXT();
				vkValidationFeaturesEXT.EnabledValidationFeatureCount = 2;
				vkValidationFeaturesEXT.EnabledValidationFeatures = enabledValidationFeatures;
				vkValidationFeaturesEXT.DisabledValidationFeatureCount = 0;
				VkValidationFeaturesEXT vkValidationFeaturesEXT2 = vkValidationFeaturesEXT;
				void* next = (flag ? (&vkDebugUtilsMessengerCreateInfoEXT) : null);
				if (Environment.CommandLine.Contains("-EnableVulkanValidation"))
				{
					RenderLog.Log.Info("Enabling vulkan validation features", "CreateInstance", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 178);
					if (flag)
					{
						vkDebugUtilsMessengerCreateInfoEXT.Next = &vkValidationFeaturesEXT2;
					}
					else
					{
						next = &vkValidationFeaturesEXT2;
					}
				}
				Instance.CreateInfo createInfo = default(Instance.CreateInfo);
				createInfo.AppInfo = appInfo;
				createInfo.Flags = platformRequiredInstanceCreateFlags;
				createInfo.EnabledExtensions = enabledInstanceExtensions;
				createInfo.EnabledLayers = enabledInstanceLayers;
				createInfo.Next = next;
				return Vulkan.CreateInstance(createInfo);
			}
		}
	}

	public static GraphicsDevice Create(Instance instance, VkSurfaceKHR surface, int framesInFlight = 2, GpuPreference preference = default(GpuPreference), bool enableVideoDecode = false, bool disallowFullscreenExclusive = false)
	{
		HashSet<string> deviceExtensions;
		QueueSelection queueSelection = SelectQueueFamilies(instance, surface, preference, out deviceExtensions);
		int videoDecodeFamilyIndex = ((enableVideoDecode && HasCoreProperties2(queueSelection.Device)) ? FindVideoDecodeFamily(queueSelection.Device) : (-1));
		DeviceEx device = CreateDevice(queueSelection, deviceExtensions, videoDecodeFamilyIndex, disallowFullscreenExclusive);
		HashSet<string> enabledInstanceExtensions = GetEnabledInstanceExtensions(disallowFullscreenExclusive);
		bool validationEnabled;
		HashSet<string> enabledInstanceLayers = GetEnabledInstanceLayers(out validationEnabled, emitUnsupportedWarning: false);
		return new GraphicsDevice(instance, queueSelection.Device, device, queueSelection.GraphicsFamilyIndex, queueSelection.PresentFamilyIndex, videoDecodeFamilyIndex, enabledInstanceExtensions, enabledInstanceLayers, deviceExtensions, queueSelection.Device.GetFeatures().DualSrcBlend, validationEnabled, framesInFlight, disallowFullscreenExclusive);
	}

	[UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private unsafe static Bool32 DebugUtilsCallback(VkDebugUtilsMessageSeverityFlagsEXT messageSeverity, VkDebugUtilsMessageTypeFlagsEXT messageTypes, VkDebugUtilsMessengerCallbackDataEXT* pCallbackData, void* pUserData)
	{
		if (pCallbackData == null)
		{
			return false;
		}
		string value = SafeUtf8(pCallbackData->Message);
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append('[').Append(MessageTypeLabel(messageTypes)).Append("] ");
		string value2 = SafeUtf8(pCallbackData->MessageIdName);
		if (!string.IsNullOrEmpty(value2))
		{
			stringBuilder.Append('[').Append(value2).Append("] ");
		}
		stringBuilder.Append(value);
		AppendObjectTable(stringBuilder, pCallbackData);
		if (messageTypes.HasFlag(VkDebugUtilsMessageTypeFlagsEXT.ValidationBitEXT))
		{
			stringBuilder.Append('\n').Append(Environment.StackTrace).Append('\n');
		}
		string text = stringBuilder.ToString();
		if (messageSeverity.HasFlag(VkDebugUtilsMessageSeverityFlagsEXT.ErrorBitEXT))
		{
			OnError(text);
			return false;
		}
		if (messageSeverity.HasFlag(VkDebugUtilsMessageSeverityFlagsEXT.WarningBitEXT))
		{
			OnWarning(text);
			return false;
		}
		if (VulkanHelpers.VerboseLoggingEnabled)
		{
			if (messageSeverity.HasFlag(VkDebugUtilsMessageSeverityFlagsEXT.InfoBitEXT))
			{
				RenderLog.Log.Info(text, "DebugUtilsCallback", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 267);
			}
			else if (messageSeverity.HasFlag(VkDebugUtilsMessageSeverityFlagsEXT.VerboseBitEXT))
			{
				RenderLog.Log.Debug(text, "DebugUtilsCallback", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 269);
			}
		}
		return false;
	}

	private unsafe static void AppendObjectTable(StringBuilder builder, VkDebugUtilsMessengerCallbackDataEXT* pCallbackData)
	{
		int objectCount = pCallbackData->ObjectCount;
		if (objectCount <= 0 || pCallbackData->Objects == null)
		{
			return;
		}
		builder.Append("\nObjects: ").Append(objectCount);
		for (int i = 0; i < objectCount; i++)
		{
			ref VkDebugUtilsObjectNameInfoEXT reference = ref pCallbackData->Objects[i];
			builder.Append("\n    [").Append(i).Append("] Vk")
				.Append(reference.ObjectType)
				.Append(" 0x")
				.Append(((ulong)reference.ObjectHandle).ToString("x"));
			string value = SafeUtf8(reference.ObjectName);
			if (!string.IsNullOrEmpty(value))
			{
				builder.Append('[').Append(value).Append(']');
			}
		}
	}

	private unsafe static string SafeUtf8(byte* ptr)
	{
		if (ptr != null)
		{
			return Utf8.GetString(ptr);
		}
		return string.Empty;
	}

	private static string MessageTypeLabel(VkDebugUtilsMessageTypeFlagsEXT types)
	{
		if (types.HasFlag(VkDebugUtilsMessageTypeFlagsEXT.ValidationBitEXT))
		{
			return "Validation";
		}
		if (types.HasFlag(VkDebugUtilsMessageTypeFlagsEXT.PerformanceBitEXT))
		{
			return "Performance";
		}
		return "General";
	}

	public static void SetErrorHandler(Action<string> onError)
	{
		OnError = onError;
	}

	public static void SetWarningHandler(Action<string> onWarning)
	{
		OnWarning = onWarning;
	}

	public void ExecuteOneShot(Action<CommandBuffer> record)
	{
		DeviceEx device = Device;
		VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = GraphicsFamilyIndex,
			Flags = VkCommandPoolCreateFlags.TransientBit
		};
		VkCommandPool commandPool = device.CreateCommandPool(in pCreateInfo, Device.VkAllocator);
		VkFence vkFence = default(VkFence);
		try
		{
			CommandBuffer commandBuffer = Device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
			{
				CommandPool = commandPool,
				Level = VkCommandBufferLevel.Primary
			});
			commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
			record(commandBuffer);
			commandBuffer.End();
			vkFence = Device.CreateFence(new VkFenceCreateInfo(), Device.VkAllocator);
			CommandBuffer reference = commandBuffer;
			SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference), default(Span<VkSemaphore>), vkFence);
			Device.WaitForFence(vkFence, -1);
		}
		finally
		{
			if (!vkFence.IsNull())
			{
				Device.DestroyFence(vkFence, Device.VkAllocator);
			}
			Device.DestroyCommandPool(commandPool, Device.VkAllocator);
		}
	}

	public void SubmitGraphics(Span<VkSemaphore> waitSemaphores, Span<VkPipelineStageFlags> waitStages, Span<CommandBuffer> commandBuffers, Span<VkSemaphore> signalSemaphores, VkFence fence)
	{
		BeforeSubmit?.Invoke();
		lock (_graphicsQueueSyncRoot)
		{
			GraphicsQueue.Submit(waitSemaphores, waitStages, commandBuffers, signalSemaphores, fence);
		}
	}

	public void SubmitVideoDecode(Span<CommandBuffer> commandBuffers, VkFence fence)
	{
		lock (_videoDecodeQueueSyncRoot)
		{
			VideoDecodeQueue.Submit(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), commandBuffers, default(Span<VkSemaphore>), fence);
		}
	}

	public void WaitIdleAndRun(Action afterIdle)
	{
		lock (_graphicsQueueSyncRoot)
		{
			Device.WaitIdle();
			afterIdle();
		}
	}

	public VkResult SubmitGraphicsAndPresent(Span<VkSemaphore> waitSemaphores, Span<VkPipelineStageFlags> waitStages, Span<CommandBuffer> commandBuffers, Span<VkSemaphore> signalSemaphores, VkFence fence, Func<VkResult> present)
	{
		BeforeSubmit?.Invoke();
		lock (_graphicsQueueSyncRoot)
		{
			GraphicsQueue.Submit(waitSemaphores, waitStages, commandBuffers, signalSemaphores, fence);
			return present();
		}
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			try
			{
				Device.WaitIdle();
			}
			catch
			{
			}
			MeshUploads.Shutdown();
			ShaderLayoutCache.Dispose();
			TextureResourceManager.ProcessDeferredDisposals(long.MaxValue);
			ComputeBufferResourceManager.DisposeAllImmediate();
			Device.DestroyVmaAllocator(Allocator);
			Device.Dispose();
			if (!_debugMessenger.IsNull())
			{
				Instance.DestroyDebugUtilsMessengerEXT(_debugMessenger, null);
			}
			Instance.Dispose();
		}
	}

	private unsafe static int FindVideoDecodeFamily(PhysicalDevice physicalDevice)
	{
		if (!VideoDecodeExtensions.All(physicalDevice.SupportsExtension))
		{
			return -1;
		}
		VkPhysicalDeviceSynchronization2Features vkPhysicalDeviceSynchronization2Features = new VkPhysicalDeviceSynchronization2Features();
		VkPhysicalDeviceFeatures2 vkPhysicalDeviceFeatures = new VkPhysicalDeviceFeatures2();
		vkPhysicalDeviceFeatures.Next = &vkPhysicalDeviceSynchronization2Features;
		VkPhysicalDeviceFeatures2 pFeatures = vkPhysicalDeviceFeatures;
		physicalDevice.GetFeatures2(ref pFeatures);
		if (!vkPhysicalDeviceSynchronization2Features.Synchronization2)
		{
			return -1;
		}
		Span<VkQueueFamilyProperties> span = stackalloc VkQueueFamilyProperties[physicalDevice.GetQueueFamilyProperties_Count()];
		physicalDevice.GetQueueFamilyProperties_Write(span);
		for (int i = 0; i < span.Length; i++)
		{
			if (span[i].QueueCount >= 1 && (span[i].QueueFlags & VkQueueFlags.VideoDecodeBitKHR) != 0)
			{
				return i;
			}
		}
		return -1;
	}

	private unsafe static DeviceEx CreateDevice(QueueSelection queueSelection, HashSet<string> deviceExtensions, int videoDecodeFamilyIndex, bool disallowFullscreenExclusive)
	{
		VulkanHelpers.AddMandatoryIfSupportedDeviceExtensions(deviceExtensions, queueSelection.Device);
		if (queueSelection.Device.SupportsExtension("VK_EXT_memory_budget"))
		{
			deviceExtensions.Add("VK_EXT_memory_budget");
		}
		if (disallowFullscreenExclusive && queueSelection.Device.SupportsExtension("VK_EXT_full_screen_exclusive"))
		{
			deviceExtensions.Add("VK_EXT_full_screen_exclusive");
		}
		VkPhysicalDeviceFeatures features = queueSelection.Device.GetFeatures();
		VkPhysicalDeviceFeatures vkPhysicalDeviceFeatures = new VkPhysicalDeviceFeatures();
		vkPhysicalDeviceFeatures.FillModeNonSolid = features.FillModeNonSolid;
		vkPhysicalDeviceFeatures.SamplerAnisotropy = features.SamplerAnisotropy;
		vkPhysicalDeviceFeatures.DualSrcBlend = features.DualSrcBlend;
		vkPhysicalDeviceFeatures.IndependentBlend = features.IndependentBlend;
		VkPhysicalDeviceFeatures vkPhysicalDeviceFeatures2 = vkPhysicalDeviceFeatures;
		float num = 1f;
		List<int> list = new List<int> { queueSelection.GraphicsFamilyIndex };
		if (!list.Contains(queueSelection.PresentFamilyIndex))
		{
			list.Add(queueSelection.PresentFamilyIndex);
		}
		VkPhysicalDeviceSynchronization2Features vkPhysicalDeviceSynchronization2Features = new VkPhysicalDeviceSynchronization2Features();
		vkPhysicalDeviceSynchronization2Features.Synchronization2 = true;
		VkPhysicalDeviceSynchronization2Features vkPhysicalDeviceSynchronization2Features2 = vkPhysicalDeviceSynchronization2Features;
		if (videoDecodeFamilyIndex >= 0)
		{
			deviceExtensions.UnionWith(VideoDecodeExtensions);
			if (!list.Contains(videoDecodeFamilyIndex))
			{
				list.Add(videoDecodeFamilyIndex);
			}
		}
		VkDeviceQueueCreateInfo[] array = new VkDeviceQueueCreateInfo[list.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new VkDeviceQueueCreateInfo
			{
				QueueFamilyIndex = list[i],
				QueueCount = 1,
				QueuePriorities = &num
			};
		}
		return queueSelection.Device.CreateDevice(new DeviceEx.CreateInfo
		{
			Extensions = deviceExtensions,
			Features = &vkPhysicalDeviceFeatures2,
			QueueCreateInfos = array,
			Next = ((videoDecodeFamilyIndex >= 0) ? (&vkPhysicalDeviceSynchronization2Features2) : null)
		});
	}

	private unsafe static VmaAllocator CreateAllocator(Instance instance, PhysicalDevice physicalDevice, DeviceEx device, int apiVersion, bool memoryBudget, long vramLimitBytes)
	{
		VkPhysicalDeviceMemoryProperties memoryProperties = physicalDevice.GetMemoryProperties();
		ByteSize* ptr = stackalloc ByteSize[System.Math.Max(1, memoryProperties.MemoryHeapCount)];
		for (int i = 0; i < memoryProperties.MemoryHeapCount; i++)
		{
			bool flag = (global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryHeap_16, VkMemoryHeap>(ref memoryProperties.MemoryHeaps, 16)[i].Flags & VkMemoryHeapFlags.DeviceLocalBit) != 0;
			ptr[i] = new ByteSize((flag && vramLimitBytes > 0) ? ((nuint)vramLimitBytes) : UIntPtr.MaxValue);
		}
		VmaVulkanFunctions vmaVulkanFunctions = default(VmaVulkanFunctions);
		vmaVulkanFunctions.VkGetInstanceProcAddr = Vulkan.GetInstanceProcAddr;
		vmaVulkanFunctions.VkGetDeviceProcAddr = device.PInvoke.vkGetDeviceProcAddr;
		VmaVulkanFunctions vmaVulkanFunctions2 = vmaVulkanFunctions;
		if (VulkanVersion.AtLeast(apiVersion, 4198400) && (device.GetProcAddr(new byte[30]
		{
			118, 107, 71, 101, 116, 73, 109, 97, 103, 101,
			77, 101, 109, 111, 114, 121, 82, 101, 113, 117,
			105, 114, 101, 109, 101, 110, 116, 115, 50, 0
		}).Invoke == (delegate* unmanaged[Cdecl]<void>)null || device.GetProcAddr(new byte[31]
		{
			118, 107, 71, 101, 116, 66, 117, 102, 102, 101,
			114, 77, 101, 109, 111, 114, 121, 82, 101, 113,
			117, 105, 114, 101, 109, 101, 110, 116, 115, 50,
			0
		}).Invoke == (delegate* unmanaged[Cdecl]<void>)null || device.GetProcAddr(new byte[19]
		{
			118, 107, 66, 105, 110, 100, 73, 109, 97, 103,
			101, 77, 101, 109, 111, 114, 121, 50, 0
		}).Invoke == (delegate* unmanaged[Cdecl]<void>)null || device.GetProcAddr(new byte[20]
		{
			118, 107, 66, 105, 110, 100, 66, 117, 102, 102,
			101, 114, 77, 101, 109, 111, 114, 121, 50, 0
		}).Invoke == (delegate* unmanaged[Cdecl]<void>)null))
		{
			LogCategory logger = Logger;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(102, 1, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Vulkan ");
				message.AppendFormatted(VulkanVersion.Format(apiVersion));
				message.AppendLiteral(" device does not expose the core 1.1 memory entry points. Making VMA fall back to the 1.0 ones.");
			}
			logger.Warning(message, "CreateAllocator", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 544);
			apiVersion = 4194304;
		}
		return device.CreateVmaAllocator(new VmaAllocatorCreateInfo
		{
			Flags = (memoryBudget ? VmaAllocatorCreateFlags.ExtMemoryBudgetBit : VmaAllocatorCreateFlags.None),
			Instance = instance,
			PhysicalDevice = physicalDevice,
			Device = device,
			VulkanApiVersion = (uint)VulkanVersion.WithoutPatch(apiVersion),
			PVulkanFunctions = &vmaVulkanFunctions2,
			PreferredLargeHeapBlockSize = new ByteSize(67108864u),
			PHeapSizeLimit = ((vramLimitBytes > 0) ? ptr : null)
		});
	}

	private static long ReadVramLimitOverride()
	{
		string environmentVariable = Environment.GetEnvironmentVariable("WAYLITH_VRAM_BUDGET_MB");
		if (string.IsNullOrWhiteSpace(environmentVariable))
		{
			return 0L;
		}
		bool shouldAppend;
		LogCategory log;
		LogString<Warning> message;
		if (!long.TryParse(environmentVariable, out var result) || result <= 0)
		{
			log = RenderLog.Log;
			LogCategory logCategory = log;
			message = new LogString<Warning>(48, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendFormatted("WAYLITH_VRAM_BUDGET_MB");
				message.AppendLiteral("='");
				message.AppendFormatted(environmentVariable);
				message.AppendLiteral("' is not a positive number of MB, ignoring it.");
			}
			logCategory.Warning(message, "ReadVramLimitOverride", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 570);
			return 0L;
		}
		log = RenderLog.Log;
		LogCategory logCategory2 = log;
		message = new LogString<Warning>(31, 2, log, out shouldAppend);
		if (shouldAppend)
		{
			message.AppendFormatted("WAYLITH_VRAM_BUDGET_MB");
			message.AppendLiteral(" caps device-local VRAM at ");
			message.AppendFormatted(result);
			message.AppendLiteral(" MB.");
		}
		logCategory2.Warning(message, "ReadVramLimitOverride", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 574);
		return result * 1024 * 1024;
	}

	private static HashSet<string> GetEnabledInstanceExtensions(bool disallowFullscreenExclusive)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		VulkanHelpers.AddPlatformRequiredExtensions(hashSet);
		VulkanHelpers.AddSurfaceExtensions(hashSet);
		AddGlfwRequiredExtensions(hashSet);
		if (IsInstanceExtensionAvailable("VK_EXT_debug_utils"))
		{
			hashSet.Add("VK_EXT_debug_utils");
		}
		if (IsInstanceExtensionAvailable("VK_KHR_get_physical_device_properties2"))
		{
			hashSet.Add("VK_KHR_get_physical_device_properties2");
		}
		if (disallowFullscreenExclusive && IsInstanceExtensionAvailable("VK_KHR_get_surface_capabilities2"))
		{
			hashSet.Add("VK_KHR_get_surface_capabilities2");
		}
		return hashSet;
	}

	private unsafe static bool IsInstanceExtensionAvailable(string name)
	{
		Span<VkExtensionProperties> span = stackalloc VkExtensionProperties[Vulkan.EnumerateInstanceExtensionProperties_Count(default(ReadOnlySpan<byte>))];
		Vulkan.EnumerateInstanceExtensionProperties_Write(default(ReadOnlySpan<byte>), span);
		for (int i = 0; i < span.Length; i++)
		{
			VkExtensionProperties vkExtensionProperties = span[i];
			if (SafeUtf8(vkExtensionProperties.ExtensionName) == name)
			{
				return true;
			}
		}
		return false;
	}

	private static HashSet<string> GetEnabledInstanceLayers(out bool validationEnabled, bool emitUnsupportedWarning)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		validationEnabled = false;
		if (Environment.CommandLine.Contains("-EnableVulkanValidation"))
		{
			if (VulkanHelpers.Validation.IsSupported)
			{
				RenderLog.Log.Info("Enabling vulkan validation layer", "GetEnabledInstanceLayers", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 617);
				VulkanHelpers.AddValidationLayers(hashSet);
				validationEnabled = true;
			}
			else if (emitUnsupportedWarning)
			{
				RenderLog.Log.Warning("VK_LAYER_KHRONOS_validation is not available; Vulkan validation messages are disabled for this run.", "GetEnabledInstanceLayers", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 623);
			}
		}
		return hashSet;
	}

	private static QueueSelection SelectQueueFamilies(Instance instance, VkSurfaceKHR surface, GpuPreference preference, out HashSet<string> deviceExtensions)
	{
		deviceExtensions = new HashSet<string>();
		VulkanHelpers.AddSwapchainExtensions(deviceExtensions);
		string environmentVariable = Environment.GetEnvironmentVariable("WAYLITH_VULKAN_DEVICE");
		bool flag = !string.IsNullOrWhiteSpace(environmentVariable);
		bool flag2 = false;
		List<GpuDescription> list = new List<GpuDescription>();
		List<(GpuDescription, QueueSelection)> list2 = new List<(GpuDescription, QueueSelection)>();
		int num = -1;
		PhysicalDevice[] physicalDevices = instance.PhysicalDevices;
		foreach (PhysicalDevice physicalDevice in physicalDevices)
		{
			num++;
			VkPhysicalDeviceProperties properties = physicalDevice.GetProperties();
			string deviceName = GetDeviceName(properties);
			string text = DescribeDriver(physicalDevice);
			long deviceLocalMemoryBytes = GetDeviceLocalMemoryBytes(physicalDevice);
			string deviceUuid = GetDeviceUuid(physicalDevice);
			QueueSelection queueSelection = default(QueueSelection);
			string text2 = ((!deviceExtensions.All(physicalDevice.SupportsExtension)) ? "missing a required device extension (e.g. swapchain)" : ((physicalDevice.GetSurfaceFormatsKHR_Count(surface) == 0) ? "no surface formats for this window" : ((physicalDevice.GetSurfacePresentModesKHR_Count(surface) == 0) ? "no present modes for this window" : (TrySelectQueueFamilies(physicalDevice, surface, num, out queueSelection) ? null : "no queue families for graphics and for presentation to this surface"))));
			GpuDescription gpuDescription = new GpuDescription
			{
				Index = num,
				Name = deviceName,
				Driver = text,
				ApiVersion = properties.ApiVersion,
				DeviceType = properties.DeviceType,
				VramBytes = deviceLocalMemoryBytes,
				Uuid = deviceUuid,
				RejectionReason = text2
			};
			list.Add(gpuDescription);
			bool flag3 = flag && deviceName.Contains(environmentVariable, StringComparison.OrdinalIgnoreCase);
			string value = $"GPU [{num}] '{deviceName}' ({properties.DeviceType}, Vulkan {VulkanVersion.Format(properties.ApiVersion)}, driver {text}, {FormatVram(deviceLocalMemoryBytes)} device-local, uuid {deviceUuid})" + (flag3 ? " [matches WAYLITH_VULKAN_DEVICE]" : string.Empty);
			bool shouldAppend;
			if (text2 != null)
			{
				LogCategory logger = Logger;
				LogCategory logCategory = logger;
				LogString<Information> message = new LogString<Information>(13, 2, logger, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendFormatted(value);
					message.AppendLiteral(" - skipped: ");
					message.AppendFormatted(text2);
					message.AppendLiteral(".");
				}
				logCategory.Info(message, "SelectQueueFamilies", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 693);
				if (flag3)
				{
					flag2 = true;
				}
			}
			else
			{
				LogCategory logger = Logger;
				LogCategory logCategory2 = logger;
				LogString<Information> message = new LogString<Information>(19, 2, logger, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendFormatted(value);
					message.AppendLiteral(" - usable (score ");
					message.AppendFormatted(queueSelection.Score);
					message.AppendLiteral(").");
				}
				logCategory2.Info(message, "SelectQueueFamilies", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 699);
				list2.Add((gpuDescription, queueSelection));
			}
		}
		EnumeratedGpus = list;
		if (list2.Count == 0)
		{
			if (flag && flag2)
			{
				WarnOverrideUnusable(environmentVariable);
			}
			string text3 = ((list.Count == 0) ? "No Vulkan devices were enumerated, which usually means no working Vulkan driver is installed." : ("Devices found: " + string.Join("; ", list.Select((GpuDescription g) => $"'{g.Name}' ({g.DeviceType}, Vulkan {VulkanVersion.Format(g.ApiVersion)}, driver {g.Driver}): {g.RejectionReason}")) + "."));
			throw new InvalidOperationException("Could not find a Vulkan device that supports graphics and presentation. " + text3 + " The most common scenario is a laptop with two GPUs, in which case you need working drivers for both, including the integrated one.");
		}
		(GpuDescription, QueueSelection) tuple = list2[0];
		for (int j = 1; j < list2.Count; j++)
		{
			if (list2[j].Item2.Score > tuple.Item2.Score)
			{
				tuple = list2[j];
			}
		}
		tuple.Item1.IsAutomaticChoice = true;
		if (flag)
		{
			foreach (var (gpuDescription2, selection2) in list2)
			{
				if (gpuDescription2.Name.Contains(environmentVariable, StringComparison.OrdinalIgnoreCase))
				{
					return Finalize(gpuDescription2, selection2, "forced via WAYLITH_VULKAN_DEVICE");
				}
			}
			if (flag2)
			{
				WarnOverrideUnusable(environmentVariable);
			}
			else
			{
				RenderLog.Log.Warning("WAYLITH_VULKAN_DEVICE='" + environmentVariable + "' did not match any usable Vulkan device; falling back to automatic selection.", "SelectQueueFamilies", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 736);
			}
		}
		if (preference.IsSet)
		{
			foreach (var (gpuDescription3, selection3) in list2)
			{
				if (gpuDescription3.Uuid.Length > 0 && string.Equals(gpuDescription3.Uuid, preference.Uuid, StringComparison.OrdinalIgnoreCase))
				{
					return Finalize(gpuDescription3, selection3, "user graphics setting");
				}
			}
			foreach (var (gpuDescription4, selection4) in list2)
			{
				if (string.Equals(gpuDescription4.Name, preference.Name, StringComparison.OrdinalIgnoreCase))
				{
					return Finalize(gpuDescription4, selection4, "user graphics setting (matched by name)");
				}
			}
			GpuDescription gpuDescription5 = list.FirstOrDefault((GpuDescription g) => !g.Usable && ((g.Uuid.Length > 0 && string.Equals(g.Uuid, preference.Uuid, StringComparison.OrdinalIgnoreCase)) || string.Equals(g.Name, preference.Name, StringComparison.OrdinalIgnoreCase)));
			if (gpuDescription5 != null)
			{
				RenderLog.Log.Warning($"Preferred GPU '{gpuDescription5.Name}' is present but unusable ({gpuDescription5.RejectionReason}); " + "falling back to automatic selection.", "SelectQueueFamilies", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 762);
			}
			else
			{
				RenderLog.Log.Warning($"Preferred GPU '{preference.Name}' (uuid {preference.Uuid}) was not found; " + "falling back to automatic selection.", "SelectQueueFamilies", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 766);
			}
		}
		return Finalize(tuple.Item1, tuple.Item2, $"best automatic match of {list.Count} device(s)");
		static QueueSelection Finalize(GpuDescription gpu, QueueSelection selection, string reason)
		{
			gpu.IsActive = true;
			LogCategory logger2 = Logger;
			bool shouldAppend2;
			LogString<Information> message2 = new LogString<Information>(29, 3, logger2, out shouldAppend2);
			if (shouldAppend2)
			{
				message2.AppendLiteral("Selected GPU '");
				message2.AppendFormatted(gpu.Name);
				message2.AppendLiteral("' (driver ");
				message2.AppendFormatted(gpu.Driver);
				message2.AppendLiteral(") - ");
				message2.AppendFormatted(reason);
				message2.AppendLiteral(".");
			}
			logger2.Info(message2, "SelectQueueFamilies", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 776);
			return selection;
		}
	}

	private static void WarnOverrideUnusable(string requestedDevice)
	{
		RenderLog.Log.Warning("WAYLITH_VULKAN_DEVICE='" + requestedDevice + "' matched a device that cannot render and present to this window, so it was ignored. On hybrid-GPU laptops the discrete GPU usually cannot present directly; launch through your platform's GPU offload mechanism (e.g. PRIME / DRI_PRIME / the NVIDIA control panel) to route the whole app onto it instead.", "WarnOverrideUnusable", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Graphics\\GraphicsDevice.cs", 783);
	}

	private static bool TrySelectQueueFamilies(PhysicalDevice physicalDevice, VkSurfaceKHR surface, int enumerationIndex, out QueueSelection queueSelection)
	{
		Span<VkQueueFamilyProperties> span = stackalloc VkQueueFamilyProperties[physicalDevice.GetQueueFamilyProperties_Count()];
		physicalDevice.GetQueueFamilyProperties_Write(span);
		int? num = null;
		int? num2 = null;
		for (int i = 0; i < span.Length; i++)
		{
			if (span[i].QueueCount >= 1)
			{
				if (!num.HasValue && (span[i].QueueFlags & VkQueueFlags.GraphicsBit) != 0)
				{
					num = i;
				}
				if (!num2.HasValue && (bool)physicalDevice.GetSurfaceSupportKHR(i, surface))
				{
					num2 = i;
				}
				if (num.HasValue && num2.HasValue)
				{
					break;
				}
			}
		}
		if (!num.HasValue || !num2.HasValue)
		{
			queueSelection = default(QueueSelection);
			return false;
		}
		long score = ComputeDeviceScore(physicalDevice, enumerationIndex);
		queueSelection = new QueueSelection(physicalDevice, num.Value, num2.Value, score);
		return true;
	}

	private unsafe static void AddGlfwRequiredExtensions(HashSet<string> extensions)
	{
		uint count;
		byte** requiredInstanceExtensions = Glfw.GetRequiredInstanceExtensions(out count);
		extensions.EnsureCapacity(extensions.Count + (int)count);
		for (int i = 0; i < count; i++)
		{
			extensions.Add(Utf8.GetString(requiredInstanceExtensions[i]));
		}
	}

	private static long ComputeDeviceScore(PhysicalDevice physicalDevice, int enumerationIndex)
	{
		long num = physicalDevice.GetProperties().DeviceType switch
		{
			VkPhysicalDeviceType.DiscreteGpu => 4, 
			VkPhysicalDeviceType.IntegratedGpu => 3, 
			VkPhysicalDeviceType.VirtualGpu => 2, 
			VkPhysicalDeviceType.Cpu => 1, 
			_ => 0, 
		};
		long num2 = System.Math.Min(GetDeviceLocalMemoryBytes(physicalDevice) >> 30, 1048575L);
		int num3 = System.Math.Max(0, 4095 - enumerationIndex);
		return (num << 56) + (num2 << 12) + num3;
	}

	private unsafe static string GetDeviceUuid(PhysicalDevice physicalDevice)
	{
		if (!VulkanVersion.AtLeast(physicalDevice.GetProperties().ApiVersion, 4198400) && !physicalDevice.SupportsExtension("VK_KHR_external_memory_capabilities"))
		{
			return string.Empty;
		}
		VkPhysicalDeviceIdProperties vkPhysicalDeviceIdProperties = new VkPhysicalDeviceIdProperties();
		VkPhysicalDeviceProperties2 vkPhysicalDeviceProperties = new VkPhysicalDeviceProperties2();
		vkPhysicalDeviceProperties.Next = &vkPhysicalDeviceIdProperties;
		VkPhysicalDeviceProperties2 properties = vkPhysicalDeviceProperties;
		if (!TryGetProperties2(physicalDevice, ref properties))
		{
			return string.Empty;
		}
		return Convert.ToHexString(new ReadOnlySpan<byte>(vkPhysicalDeviceIdProperties.DeviceUUID, 16));
	}

	private static bool HasCoreProperties2(PhysicalDevice physicalDevice)
	{
		return VulkanVersion.AtLeast(VulkanVersion.Min(InstanceApiVersion, physicalDevice.GetProperties().ApiVersion), 4198400);
	}

	public static bool TryGetProperties2(PhysicalDevice physicalDevice, ref VkPhysicalDeviceProperties2 properties)
	{
		if (HasCoreProperties2(physicalDevice))
		{
			physicalDevice.GetProperties2(ref properties);
		}
		else
		{
			if (!physicalDevice.Instance.IsExtensionEnabled("VK_KHR_get_physical_device_properties2"))
			{
				return false;
			}
			physicalDevice.GetProperties2KHR(ref properties);
		}
		return true;
	}

	private static long GetDeviceLocalMemoryBytes(PhysicalDevice physicalDevice)
	{
		VkPhysicalDeviceMemoryProperties memoryProperties = physicalDevice.GetMemoryProperties();
		long num = 0L;
		for (int i = 0; i < memoryProperties.MemoryHeapCount; i++)
		{
			VkMemoryHeap vkMemoryHeap = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryHeap_16, VkMemoryHeap>(ref memoryProperties.MemoryHeaps, 16)[i];
			if ((vkMemoryHeap.Flags & VkMemoryHeapFlags.DeviceLocalBit) != 0)
			{
				num += (long)vkMemoryHeap.Size.Value;
			}
		}
		return num;
	}

	private unsafe static string GetDeviceName(VkPhysicalDeviceProperties properties)
	{
		return Utf8.GetString(properties.DeviceName);
	}

	private unsafe static string DescribeDriver(PhysicalDevice physicalDevice)
	{
		VkPhysicalDeviceProperties properties = physicalDevice.GetProperties();
		if (!VulkanVersion.AtLeast(properties.ApiVersion, 4202496) && !physicalDevice.SupportsExtension("VK_KHR_driver_properties"))
		{
			return $"unknown (Vulkan {VulkanVersion.Format(properties.ApiVersion)}, driver version 0x{properties.DriverVersion:X})";
		}
		VkPhysicalDeviceDriverProperties vkPhysicalDeviceDriverProperties = new VkPhysicalDeviceDriverProperties();
		VkPhysicalDeviceProperties2 vkPhysicalDeviceProperties = new VkPhysicalDeviceProperties2();
		vkPhysicalDeviceProperties.Next = &vkPhysicalDeviceDriverProperties;
		VkPhysicalDeviceProperties2 properties2 = vkPhysicalDeviceProperties;
		if (TryGetProperties2(physicalDevice, ref properties2))
		{
			return $"{vkPhysicalDeviceDriverProperties.DriverID} '{Utf8.GetString(vkPhysicalDeviceDriverProperties.DriverName).TrimEnd('\0')}' '{Utf8.GetString(vkPhysicalDeviceDriverProperties.DriverInfo).TrimEnd('\0')}'";
		}
		return $"unknown (Vulkan {VulkanVersion.Format(properties.ApiVersion)}, driver version 0x{properties.DriverVersion:X})";
	}

	private static string FormatVram(long bytes)
	{
		return $"{bytes / 1048576} MB";
	}
}
