using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Brutal;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics;
using Waylith.Rendering;
using Waylith.Rendering.Compute;
using Waylith.VideoDecode.H264;

namespace Waylith.VideoDecode;

internal sealed class VulkanH264Decoder : IDisposable
{
	public struct Output
	{
		public GpuTextureAllocation Image;

		public long Pts;
	}

	private const int MaxSlots = 17;

	private const int MinSessionExtent = 1024;

	private const int AmdVendorId = 4098;

	private const long FenceTimeoutNs = 5000000000L;

	private const string ConvertKernelPath = "Compute/Nv12ToRgba.comp.glsl";

	private static readonly ComputeBinding[] ConvertBindings = new ComputeBinding[3]
	{
		new ComputeBinding(0, ComputeBindingType.CombinedImageSampler),
		new ComputeBinding(1, ComputeBindingType.CombinedImageSampler),
		new ComputeBinding(2, ComputeBindingType.StorageImage)
	};

	private readonly GraphicsDevice graphicsDevice;

	private readonly DeviceEx device;

	private readonly VmaAllocator allocator;

	private readonly H264Parser parser = new H264Parser();

	private readonly H264PictureManager pictures = new H264PictureManager();

	private readonly Dictionary<int, byte[]> spsNals = new Dictionary<int, byte[]>();

	private readonly Dictionary<int, byte[]> ppsNals = new Dictionary<int, byte[]>();

	private readonly List<H264Picture> outputs = new List<H264Picture>();

	private readonly List<H264Picture> references = new List<H264Picture>();

	private readonly List<(int Offset, int Length)> sliceNals = new List<(int, int)>();

	private readonly int nalLengthSize;

	private unsafe readonly VkVideoDecodeH264ProfileInfoKHR* codecProfile;

	private unsafe readonly VkVideoProfileInfoKHR* profile;

	private unsafe readonly VkVideoProfileListInfoKHR* profileList;

	private H264Sps sessionSps;

	private VkVideoSessionKHR session;

	private VkVideoSessionParametersKHR parameters;

	private VkDeviceMemory[] sessionMemory = Array.Empty<VkDeviceMemory>();

	private bool parametersDirty;

	private bool hasParameters;

	private bool sessionNeedsReset;

	private bool coincide;

	private int slotCount;

	private int imageWidth;

	private int imageHeight;

	private long bitstreamAlignment;

	private ImageAllocationInfo[] dpbImages = Array.Empty<ImageAllocationInfo>();

	private VkImageView[] dpbViews = Array.Empty<VkImageView>();

	private VkImageLayout[] dpbLayouts = Array.Empty<VkImageLayout>();

	private ImageAllocationInfo outputImage;

	private VkImageView outputView;

	private VkImageLayout outputLayout;

	private VkImageView[] lumaViews = Array.Empty<VkImageView>();

	private VkImageView[] chromaViews = Array.Empty<VkImageView>();

	private BufferAllocationInfo bitstream;

	private AllocationMapInfo bitstreamMapInfo;

	private unsafe byte* bitstreamMapped;

	private long bitstreamCapacity;

	private ComputeKernel convertKernel;

	private VkDescriptorPool descriptorPool;

	private VkCommandPool decodePool;

	private VkCommandPool graphicsPool;

	private CommandBuffer decodeCommands;

	private CommandBuffer graphicsCommands;

	private VkFence fence;

	public Func<GpuTextureAllocation> AcquireImage;

	public Action<GpuTextureAllocation> ReleaseImage;

	public int Width { get; }

	public int Height { get; }

	public int ReorderDepth { get; }

	public unsafe VulkanH264Decoder(GraphicsDevice graphicsDevice, ReadOnlySpan<byte> avcC)
	{
		this.graphicsDevice = graphicsDevice;
		device = graphicsDevice.Device;
		allocator = graphicsDevice.Allocator;
		codecProfile = (VkVideoDecodeH264ProfileInfoKHR*)NativeMemory.AllocZeroed((nuint)sizeof(VkVideoDecodeH264ProfileInfoKHR));
		profile = (VkVideoProfileInfoKHR*)NativeMemory.AllocZeroed((nuint)sizeof(VkVideoProfileInfoKHR));
		profileList = (VkVideoProfileListInfoKHR*)NativeMemory.AllocZeroed((nuint)sizeof(VkVideoProfileListInfoKHR));
		if (avcC.Length < 7)
		{
			throw new H264Exception("h264: truncated avcC");
		}
		nalLengthSize = (avcC[4] & 3) + 1;
		int num = 6;
		int num2 = avcC[5] & 0x1F;
		for (int i = 0; i < num2; i++)
		{
			int num3 = (avcC[num] << 8) | avcC[num + 1];
			AcceptParameterSet(avcC.Slice(num + 2, num3));
			num += 2 + num3;
		}
		byte b = avcC[num++];
		for (int j = 0; j < b; j++)
		{
			int num4 = (avcC[num] << 8) | avcC[num + 1];
			AcceptParameterSet(avcC.Slice(num + 2, num4));
			num += 2 + num4;
		}
		H264Sps h264Sps = parser.ActiveSps.Values.FirstOrDefault() ?? throw new H264Exception("h264: avcC carries no sps");
		CheckSupported(h264Sps);
		pictures.Configure(h264Sps);
		Width = h264Sps.VisibleWidth;
		Height = h264Sps.VisibleHeight;
		ReorderDepth = pictures.MaxNumReorderFrames + 1;
	}

	private static void CheckSupported(H264Sps sps)
	{
		if (!sps.FrameMbsOnly)
		{
			throw new H264Exception("h264: interlaced streams are not supported");
		}
		if (sps.ChromaFormatIdc != 1 || sps.BitDepthLumaMinus8 != 0 || sps.BitDepthChromaMinus8 != 0)
		{
			throw new H264Exception("h264: only 8 bit 4:2:0 is supported");
		}
	}

	private void AcceptParameterSet(ReadOnlySpan<byte> nal)
	{
		bool num = (nal[0] & 0x1F) == 7;
		Dictionary<int, byte[]> dictionary = (num ? spsNals : ppsNals);
		int key = (num ? parser.ParseSps(nal).SeqParameterSetId : parser.ParsePps(nal).PicParameterSetId);
		if (!dictionary.TryGetValue(key, out var value) || !nal.SequenceEqual(value))
		{
			dictionary[key] = nal.ToArray();
			parametersDirty = true;
		}
	}

	public void Prepare()
	{
		if (sessionSps == null)
		{
			CreateSession(parser.ActiveSps.Values.First());
		}
	}

	public void Decode(ReadOnlySpan<byte> sample, long pts, List<Output> output)
	{
		outputs.Clear();
		sliceNals.Clear();
		H264SliceHeader h264SliceHeader = null;
		bool flag = true;
		int num = 0;
		while (num + nalLengthSize <= sample.Length)
		{
			int num2 = 0;
			for (int i = 0; i < nalLengthSize; i++)
			{
				num2 = (num2 << 8) | sample[num + i];
			}
			num += nalLengthSize;
			if (num2 <= 0 || num + num2 > sample.Length)
			{
				break;
			}
			ReadOnlySpan<byte> nal = sample.Slice(num, num2);
			int num3 = nal[0] & 0x1F;
			if ((uint)(num3 - 7) <= 1u)
			{
				AcceptParameterSet(nal);
			}
			else if ((num3 == 1 || num3 == 5) ? true : false)
			{
				H264SliceHeader h264SliceHeader2 = parser.ParseSliceHeader(nal);
				if (h264SliceHeader == null)
				{
					h264SliceHeader = h264SliceHeader2;
				}
				flag &= h264SliceHeader2.IsISlice || h264SliceHeader2.IsSISlice;
				sliceNals.Add((num, num2));
			}
			num += num2;
		}
		if (h264SliceHeader != null)
		{
			DecodePicture(sample, h264SliceHeader, flag, pts);
		}
		TakeOutputs(output);
	}

	public void Flush(List<Output> output)
	{
		outputs.Clear();
		pictures.Flush(outputs);
		TakeOutputs(output);
	}

	public void Reset()
	{
		foreach (H264Picture item in pictures.Dpb)
		{
			if (!item.Outputted && item.Payload is GpuTextureAllocation obj)
			{
				ReleaseImage(obj);
			}
		}
		pictures.Reset();
		sessionNeedsReset = true;
	}

	private void TakeOutputs(List<Output> output)
	{
		foreach (H264Picture output2 in outputs)
		{
			output.Add(new Output
			{
				Image = (GpuTextureAllocation)output2.Payload,
				Pts = output2.Pts
			});
			output2.Payload = null;
		}
		outputs.Clear();
	}

	private void DecodePicture(ReadOnlySpan<byte> sample, H264SliceHeader header, bool allIntra, long pts)
	{
		H264Pps h264Pps = parser.ActivePps[header.PicParameterSetId];
		H264Sps h264Sps = parser.ActiveSps[h264Pps.SeqParameterSetId];
		if (sessionSps == null)
		{
			CreateSession(h264Sps);
		}
		else if (h264Sps.CodedWidth != sessionSps.CodedWidth || h264Sps.CodedHeight != sessionSps.CodedHeight || H264PictureManager.ComputeDpbSize(h264Sps) != pictures.MaxDpbSize)
		{
			throw new H264Exception("h264: mid stream resolution or dpb size changes are not supported");
		}
		if (parametersDirty)
		{
			VkVideoSessionParametersKHR videoSessionParameters = parameters;
			parameters = H264VulkanParameters.Create(device, session, parser);
			if (hasParameters)
			{
				device.DestroyVideoSessionParametersKHR(videoSessionParameters, device.VkAllocator);
			}
			hasParameters = true;
			parametersDirty = false;
		}
		H264Picture h264Picture = pictures.BeginPicture(h264Sps, header, outputs);
		h264Picture.Pts = pts;
		Span<bool> span = stackalloc bool[17];
		references.Clear();
		foreach (H264Picture item in pictures.Dpb)
		{
			if (item.Ref)
			{
				span[item.Slot] = true;
				references.Add(item);
			}
		}
		h264Picture.Slot = ((ReadOnlySpan<bool>)span.Slice(0, slotCount)).IndexOf(value: false);
		if (h264Picture.Slot < 0)
		{
			throw new H264Exception("h264: no free dpb slot");
		}
		int[] sliceOffsets;
		long bitstreamSize = WriteBitstream(sample, out sliceOffsets);
		RecordAndSubmitDecode(h264Sps, h264Pps, header, h264Picture, allIntra, bitstreamSize, sliceOffsets);
		Convert(h264Sps, h264Picture, (GpuTextureAllocation)(h264Picture.Payload = AcquireImage()));
		pictures.FinishPicture(h264Picture, outputs);
	}

	private unsafe long WriteBitstream(ReadOnlySpan<byte> sample, out int[] sliceOffsets)
	{
		long num = 0L;
		foreach (var sliceNal in sliceNals)
		{
			int item = sliceNal.Length;
			num += 3 + item;
		}
		long num2 = (num + bitstreamAlignment - 1) / bitstreamAlignment * bitstreamAlignment;
		if (num2 > bitstreamCapacity)
		{
			FreeBitstream();
			bitstreamCapacity = System.Math.Max(num2 * 2, 1048576L);
			VmaAllocator vmaAllocator = allocator;
			VkBufferCreateInfo bufferCreateInfo = new VkBufferCreateInfo
			{
				Size = ByteSize.Of(bitstreamCapacity),
				Usage = VkBufferUsageFlags.VideoDecodeSrcBitKHR,
				SharingMode = VkSharingMode.Exclusive,
				Next = profileList
			};
			AllocationInfo.CreateInfo allocateCreateInfo = new AllocationInfo.CreateInfo
			{
				Preference = MemoryPreference.PreferCpu,
				RequiredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
				PreferredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
				Priority = 1f
			};
			bitstream = vmaAllocator.AllocateBufferChecked("VideoBitstream", in bufferCreateInfo, in allocateCreateInfo);
			GpuMemoryTracker.Add(GpuMemoryCategory.Staging, in bitstream, null, "VideoBitstream");
			bitstreamMapInfo = new AllocationMapInfo(bitstream.AllocationInfo)
			{
				Region = new MapRegion
				{
					Offset = ByteSize.Zero,
					Size = bitstream.BufferSize
				}
			};
			bitstreamMapped = (byte*)(nint)allocator.MapMemory(in bitstreamMapInfo);
		}
		sliceOffsets = new int[sliceNals.Count];
		Span<byte> span = new Span<byte>(bitstreamMapped, (int)num2);
		int num3 = 0;
		for (int i = 0; i < sliceNals.Count; i++)
		{
			var (start, num4) = sliceNals[i];
			sliceOffsets[i] = num3;
			span[num3] = 0;
			span[num3 + 1] = 0;
			span[num3 + 2] = 1;
			sample.Slice(start, num4).CopyTo(span.Slice(num3 + 3));
			num3 += 3 + num4;
		}
		span.Slice(num3).Clear();
		return num2;
	}

	private void FreeBitstream()
	{
		if (bitstreamCapacity != 0L)
		{
			allocator.UnmapMemory(in bitstreamMapInfo);
			GpuMemoryTracker.Remove(GpuMemoryCategory.Staging, in bitstream);
			allocator.FreeAllocation(in bitstream);
			bitstreamCapacity = 0L;
		}
	}

	private static VkImageMemoryBarrier2 Barrier(VkImage image, VkImageLayout from, VkImageLayout to, bool decodeSource, bool decodeDestination)
	{
		VkImageMemoryBarrier2 result = new VkImageMemoryBarrier2();
		result.SrcStageMask = (VkPipelineStageFlags2)(decodeSource ? 67108864 : 0);
		result.SrcAccessMask = (decodeSource ? (VkAccessFlags2.VideoDecodeReadBitKHR | VkAccessFlags2.VideoDecodeWriteBitKHR) : VkAccessFlags2.None);
		result.DstStageMask = (VkPipelineStageFlags2)(decodeDestination ? 67108864 : 0);
		result.DstAccessMask = (decodeDestination ? (VkAccessFlags2.VideoDecodeReadBitKHR | VkAccessFlags2.VideoDecodeWriteBitKHR) : VkAccessFlags2.None);
		result.OldLayout = from;
		result.NewLayout = to;
		result.SrcQueueFamilyIndex = -1;
		result.DstQueueFamilyIndex = -1;
		result.Image = image;
		result.SubresourceRange = new VkImageSubresourceRange
		{
			AspectMask = VkImageAspectFlags.ColorBit,
			LevelCount = 1,
			LayerCount = 1
		};
		return result;
	}

	private void WaitForFence()
	{
		VkResult vkResult = device.WaitForFence(fence, unchecked((nint)5000000000L));
		if (vkResult != 0)
		{
			throw new InvalidOperationException($"video fence wait returned {vkResult}");
		}
		DeviceEx handle = device;
		VkFence reference = fence;
		handle.ResetFences(new ReadOnlySpan<VkFence>(in reference));
	}

	private unsafe void RecordAndSubmitDecode(H264Sps sps, H264Pps pps, H264SliceHeader header, H264Picture pic, bool allIntra, long bitstreamSize, int[] sliceOffsets)
	{
		VkExtent2D vkExtent2D = new VkExtent2D();
		vkExtent2D.Width = imageWidth;
		vkExtent2D.Height = imageHeight;
		VkExtent2D codedExtent = vkExtent2D;
		CommandBuffer commandBuffer = decodeCommands;
		commandBuffer.Reset();
		commandBuffer.Begin();
		VkImageMemoryBarrier2* ptr = stackalloc VkImageMemoryBarrier2[18];
		int num = 0;
		for (int i = 0; i < slotCount; i++)
		{
			if (dpbLayouts[i] != VkImageLayout.VideoDecodeDpbKHR)
			{
				ptr[num++] = Barrier(dpbImages[i].Image, dpbLayouts[i], VkImageLayout.VideoDecodeDpbKHR, decodeSource: false, decodeDestination: true);
				dpbLayouts[i] = VkImageLayout.VideoDecodeDpbKHR;
			}
		}
		if (!coincide && outputLayout != VkImageLayout.VideoDecodeDstKHR)
		{
			ptr[num++] = Barrier(outputImage.Image, VkImageLayout.Undefined, VkImageLayout.VideoDecodeDstKHR, decodeSource: false, decodeDestination: true);
			outputLayout = VkImageLayout.VideoDecodeDstKHR;
		}
		if (num > 0)
		{
			VkDependencyInfo pDependencyInfo = new VkDependencyInfo
			{
				ImageMemoryBarrierCount = num,
				ImageMemoryBarriers = ptr
			};
			commandBuffer.PipelineBarrier2KHR(in pDependencyInfo);
		}
		int num2 = references.Count + 1;
		VkVideoPictureResourceInfoKHR* ptr2 = stackalloc VkVideoPictureResourceInfoKHR[17];
		StdVideoDecodeH264ReferenceInfo* ptr3 = stackalloc StdVideoDecodeH264ReferenceInfo[17];
		VkVideoDecodeH264DpbSlotInfoKHR* ptr4 = stackalloc VkVideoDecodeH264DpbSlotInfoKHR[17];
		VkVideoReferenceSlotInfoKHR* ptr5 = stackalloc VkVideoReferenceSlotInfoKHR[17];
		for (int j = 0; j < num2; j++)
		{
			H264Picture h264Picture = ((j == 0) ? pic : references[j - 1]);
			ptr2[j] = new VkVideoPictureResourceInfoKHR
			{
				ImageViewBinding = dpbViews[h264Picture.Slot],
				CodedExtent = codedExtent
			};
			ptr3[j] = H264VulkanParameters.ReferenceInfo(h264Picture);
			ptr4[j] = new VkVideoDecodeH264DpbSlotInfoKHR
			{
				StdReferenceInfo = ptr3 + j
			};
			ptr5[j] = new VkVideoReferenceSlotInfoKHR
			{
				Next = ptr4 + j,
				SlotIndex = ((j == 0) ? (-1) : h264Picture.Slot),
				PictureResource = ptr2 + j
			};
		}
		commandBuffer.BeginVideoCodingKHR(new VkVideoBeginCodingInfoKHR
		{
			VideoSession = session,
			VideoSessionParameters = parameters,
			ReferenceSlotCount = num2,
			ReferenceSlots = ptr5
		});
		if (sessionNeedsReset)
		{
			VkVideoCodingControlInfoKHR pCodingControlInfo = new VkVideoCodingControlInfoKHR
			{
				Flags = VkVideoCodingControlFlagsKHR.ResetBitKHR
			};
			commandBuffer.ControlVideoCodingKHR(in pCodingControlInfo);
			sessionNeedsReset = false;
		}
		ptr5->SlotIndex = pic.Slot;
		StdVideoDecodeH264PictureInfo stdVideoDecodeH264PictureInfo = new StdVideoDecodeH264PictureInfo();
		stdVideoDecodeH264PictureInfo.Flags = new StdVideoDecodeH264PictureInfoFlags
		{
			IdrPicFlag = pic.Idr,
			IsIntra = allIntra,
			IsReference = pic.Ref
		};
		stdVideoDecodeH264PictureInfo.SeqParameterSetId = (byte)sps.SeqParameterSetId;
		stdVideoDecodeH264PictureInfo.PicParameterSetId = (byte)pps.PicParameterSetId;
		stdVideoDecodeH264PictureInfo.FrameNum = (ushort)header.FrameNum;
		stdVideoDecodeH264PictureInfo.IdrPicId = (ushort)header.IdrPicId;
		stdVideoDecodeH264PictureInfo.PicOrderCnt = new int2(pic.TopFieldOrderCnt, pic.BottomFieldOrderCnt);
		StdVideoDecodeH264PictureInfo stdVideoDecodeH264PictureInfo2 = stdVideoDecodeH264PictureInfo;
		fixed (int* sliceOffsets2 = sliceOffsets)
		{
			VkVideoDecodeH264PictureInfoKHR vkVideoDecodeH264PictureInfoKHR = new VkVideoDecodeH264PictureInfoKHR();
			vkVideoDecodeH264PictureInfoKHR.StdPictureInfo = &stdVideoDecodeH264PictureInfo2;
			vkVideoDecodeH264PictureInfoKHR.SliceCount = sliceOffsets.Length;
			vkVideoDecodeH264PictureInfoKHR.SliceOffsets = sliceOffsets2;
			VkVideoDecodeH264PictureInfoKHR vkVideoDecodeH264PictureInfoKHR2 = vkVideoDecodeH264PictureInfoKHR;
			VkVideoDecodeInfoKHR pDecodeInfo = new VkVideoDecodeInfoKHR
			{
				SrcBuffer = bitstream.VkBuffer,
				SrcBufferOffset = ByteSize.Zero,
				SrcBufferRange = ByteSize.Of(bitstreamSize),
				DstPictureResource = (coincide ? (*ptr2) : new VkVideoPictureResourceInfoKHR
				{
					ImageViewBinding = outputView,
					CodedExtent = codedExtent
				}),
				SetupReferenceSlot = ptr5,
				ReferenceSlotCount = references.Count,
				ReferenceSlots = ((references.Count > 0) ? (ptr5 + 1) : null),
				Next = &vkVideoDecodeH264PictureInfoKHR2
			};
			commandBuffer.DecodeVideoKHR(in pDecodeInfo);
			commandBuffer.EndVideoCodingKHR(new VkVideoEndCodingInfoKHR());
			VkImageMemoryBarrier2 vkImageMemoryBarrier;
			if (coincide)
			{
				vkImageMemoryBarrier = Barrier(dpbImages[pic.Slot].Image, VkImageLayout.VideoDecodeDpbKHR, VkImageLayout.ShaderReadOnlyOptimal, decodeSource: true, decodeDestination: false);
				dpbLayouts[pic.Slot] = VkImageLayout.ShaderReadOnlyOptimal;
			}
			else
			{
				vkImageMemoryBarrier = Barrier(outputImage.Image, VkImageLayout.VideoDecodeDstKHR, VkImageLayout.ShaderReadOnlyOptimal, decodeSource: true, decodeDestination: false);
				outputLayout = VkImageLayout.ShaderReadOnlyOptimal;
			}
			commandBuffer.PipelineBarrier2KHR(new VkDependencyInfo
			{
				ImageMemoryBarrierCount = 1,
				ImageMemoryBarriers = &vkImageMemoryBarrier
			});
			commandBuffer.End();
			GraphicsDevice obj = graphicsDevice;
			CommandBuffer reference = commandBuffer;
			obj.SubmitVideoDecode(new Span<CommandBuffer>(ref reference), fence);
			WaitForFence();
		}
	}

	private void Convert(H264Sps sps, H264Picture pic, GpuTextureAllocation target)
	{
		int num = (coincide ? pic.Slot : 0);
		CommandBuffer commandBuffer = graphicsCommands;
		device.ResetDescriptorPool(descriptorPool, 0);
		commandBuffer.Reset();
		commandBuffer.Begin();
		ComputeDispatch computeDispatch = ComputeDispatch.Rent(convertKernel, (sps.VisibleWidth + 7) / 8, (sps.VisibleHeight + 7) / 8);
		computeDispatch.SetSampledView(0, lumaViews[num], ComputeDispatchQueue.PointClampSampler);
		computeDispatch.SetSampledView(1, chromaViews[num], ComputeDispatchQueue.BilinearClampSampler);
		computeDispatch.SetStorageImage(2, target);
		computeDispatch.SetPushConstants<Nv12ToRgbaPush>(Nv12ToRgbaPush.ForStream(sps));
		ComputeDispatchQueue.RecordDispatch(commandBuffer, device, descriptorPool, computeDispatch);
		ComputeDispatch.Return(computeDispatch);
		commandBuffer.End();
		GraphicsDevice obj = graphicsDevice;
		CommandBuffer reference = commandBuffer;
		obj.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference), default(Span<VkSemaphore>), fence);
		WaitForFence();
	}

	private unsafe void CreateSession(H264Sps sps)
	{
		CheckSupported(sps);
		pictures.Configure(sps);
		*codecProfile = new VkVideoDecodeH264ProfileInfoKHR
		{
			StdProfileIdc = (StdVideoH264ProfileIdc)sps.ProfileIdc,
			PictureLayout = VkVideoDecodeH264PictureLayoutFlagsKHR.None
		};
		*profile = new VkVideoProfileInfoKHR
		{
			VideoCodecOperation = VkVideoCodecOperationFlagsKHR.DecodeH264BitKHR,
			ChromaSubsampling = VkVideoChromaSubsamplingFlagsKHR._420BitKHR,
			ChromaBitDepth = VkVideoComponentBitDepthFlagsKHR._8BitKHR,
			LumaBitDepth = VkVideoComponentBitDepthFlagsKHR._8BitKHR,
			Next = codecProfile
		};
		*profileList = new VkVideoProfileListInfoKHR
		{
			ProfileCount = 1,
			Profiles = profile
		};
		PhysicalDevice physicalDevice = graphicsDevice.PhysicalDevice;
		VkVideoDecodeH264CapabilitiesKHR vkVideoDecodeH264CapabilitiesKHR = new VkVideoDecodeH264CapabilitiesKHR();
		VkVideoDecodeCapabilitiesKHR vkVideoDecodeCapabilitiesKHR = new VkVideoDecodeCapabilitiesKHR
		{
			Next = &vkVideoDecodeH264CapabilitiesKHR
		};
		VkVideoCapabilitiesKHR pCapabilities = new VkVideoCapabilitiesKHR
		{
			Next = &vkVideoDecodeCapabilitiesKHR
		};
		physicalDevice.GetVideoCapabilitiesKHR(profile, ref pCapabilities);
		if (sps.CodedWidth > pCapabilities.MaxCodedExtent.Width || sps.CodedHeight > pCapabilities.MaxCodedExtent.Height)
		{
			throw new H264Exception($"h264: coded size {sps.CodedWidth}x{sps.CodedHeight} is beyond what the driver decodes ({pCapabilities.MaxCodedExtent.Width}x{pCapabilities.MaxCodedExtent.Height})");
		}
		int num = ((physicalDevice.GetProperties().VendorID == 4098) ? System.Math.Max(pCapabilities.MinCodedExtent.Width, 64) : pCapabilities.MinCodedExtent.Width);
		if (sps.CodedWidth < num)
		{
			throw new H264Exception($"h264: coded width {sps.CodedWidth} is below the hardware minimum of {num}");
		}
		imageWidth = sps.CodedWidth;
		imageHeight = System.Math.Max(sps.CodedHeight, pCapabilities.MinCodedExtent.Height);
		coincide = (vkVideoDecodeCapabilitiesKHR.Flags & VkVideoDecodeCapabilityFlagsKHR.DpbAndOutputCoincideBitKHR) != 0;
		slotCount = System.Math.Min(pictures.MaxDpbSize + 1, pCapabilities.MaxDpbSlots);
		bitstreamAlignment = System.Math.Max((long)(int)pCapabilities.MinBitstreamBufferSizeAlignment, (long)(int)pCapabilities.MinBitstreamBufferOffsetAlignment);
		VkImageUsageFlags usage = (coincide ? (VkImageUsageFlags.SampledBit | VkImageUsageFlags.VideoDecodeDstBitKHR | VkImageUsageFlags.VideoDecodeDpbBitKHR) : VkImageUsageFlags.VideoDecodeDpbBitKHR);
		VkFormat vkFormat = PickFormat(usage);
		VkImageUsageFlags usage2 = VkImageUsageFlags.SampledBit | VkImageUsageFlags.VideoDecodeDstBitKHR;
		VkFormat vkFormat2 = (coincide ? vkFormat : PickFormat(usage2));
		VkExtent2D vkExtent2D = new VkExtent2D();
		vkExtent2D.Width = System.Math.Min(System.Math.Max(imageWidth, 1024), pCapabilities.MaxCodedExtent.Width);
		vkExtent2D.Height = System.Math.Min(System.Math.Max(imageHeight, 1024), pCapabilities.MaxCodedExtent.Height);
		VkExtent2D maxCodedExtent = vkExtent2D;
		VkExtensionProperties stdHeaderVersion = pCapabilities.StdHeaderVersion;
		session = device.CreateVideoSessionKHR(new VkVideoSessionCreateInfoKHR
		{
			VideoProfile = profile,
			QueueFamilyIndex = graphicsDevice.VideoDecodeFamilyIndex,
			PictureFormat = vkFormat2,
			MaxCodedExtent = maxCodedExtent,
			MaxDpbSlots = slotCount,
			MaxActiveReferencePictures = System.Math.Min(System.Math.Min(pictures.MaxDpbSize, slotCount), pCapabilities.MaxActiveReferencePictures),
			ReferencePictureFormat = vkFormat,
			StdHeaderVersion = &stdHeaderVersion
		}, device.VkAllocator);
		sessionSps = sps;
		BindSessionMemory();
		sessionNeedsReset = true;
		dpbImages = new ImageAllocationInfo[slotCount];
		dpbViews = new VkImageView[slotCount];
		dpbLayouts = new VkImageLayout[slotCount];
		for (int i = 0; i < slotCount; i++)
		{
			dpbImages[i] = AllocateImage("VideoDpb" + i, vkFormat, usage);
			dpbViews[i] = PictureView(dpbImages[i].Image, vkFormat);
			dpbLayouts[i] = VkImageLayout.Undefined;
		}
		if (coincide)
		{
			lumaViews = new VkImageView[slotCount];
			chromaViews = new VkImageView[slotCount];
			for (int j = 0; j < slotCount; j++)
			{
				lumaViews[j] = PlaneView(dpbImages[j].Image, VkFormat.R8UNorm, VkImageAspectFlags.Plane0Bit);
				chromaViews[j] = PlaneView(dpbImages[j].Image, VkFormat.R8G8UNorm, VkImageAspectFlags.Plane1Bit);
			}
		}
		else
		{
			outputImage = AllocateImage("VideoOutput", vkFormat2, usage2);
			outputView = PictureView(outputImage.Image, vkFormat2);
			outputLayout = VkImageLayout.Undefined;
			lumaViews = new VkImageView[1] { PlaneView(outputImage.Image, VkFormat.R8UNorm, VkImageAspectFlags.Plane0Bit) };
			chromaViews = new VkImageView[1] { PlaneView(outputImage.Image, VkFormat.R8G8UNorm, VkImageAspectFlags.Plane1Bit) };
		}
		convertKernel = ComputeKernelCache.Load("Compute/Nv12ToRgba.comp.glsl", "main", ConvertBindings, sizeof(Nv12ToRgbaPush));
		descriptorPool = ComputeDispatchQueue.CreateDescriptorPool(1);
		decodePool = device.CreateCommandPool(new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = graphicsDevice.VideoDecodeFamilyIndex,
			Flags = VkCommandPoolCreateFlags.ResetCommandBufferBit
		}, device.VkAllocator);
		graphicsPool = device.CreateCommandPool(new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = graphicsDevice.GraphicsFamilyIndex,
			Flags = VkCommandPoolCreateFlags.ResetCommandBufferBit
		}, device.VkAllocator);
		decodeCommands = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
		{
			CommandPool = decodePool,
			Level = VkCommandBufferLevel.Primary
		});
		graphicsCommands = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
		{
			CommandPool = graphicsPool,
			Level = VkCommandBufferLevel.Primary
		});
		fence = device.CreateFence(new VkFenceCreateInfo(), device.VkAllocator);
	}

	private unsafe ImageAllocationInfo AllocateImage(string label, VkFormat format, VkImageUsageFlags usage)
	{
		int* ptr = stackalloc int[2] { graphicsDevice.GraphicsFamilyIndex, graphicsDevice.VideoDecodeFamilyIndex };
		bool flag = *ptr != ptr[1];
		VmaAllocator vmaAllocator = allocator;
		VkImageCreateInfo imageCreateInfo = new VkImageCreateInfo
		{
			Flags = (((usage & VkImageUsageFlags.SampledBit) != 0) ? (VkImageCreateFlags.MutableFormatBit | VkImageCreateFlags.ExtendedUsageBit) : VkImageCreateFlags.None),
			ImageType = VkImageType._2D,
			Format = format,
			Extent = new VkExtent3D(imageWidth, imageHeight, 1),
			MipLevels = 1,
			ArrayLayers = 1,
			Samples = VkSampleCountFlags._1Bit,
			Tiling = VkImageTiling.Optimal,
			Usage = usage,
			SharingMode = (flag ? VkSharingMode.Concurrent : VkSharingMode.Exclusive),
			QueueFamilyIndexCount = (flag ? 2 : 0),
			QueueFamilyIndices = (flag ? ptr : null),
			InitialLayout = VkImageLayout.Undefined,
			Next = profileList
		};
		ImageAllocationInfo allocation = vmaAllocator.AllocateImageChecked(label, in imageCreateInfo, new AllocationInfo.CreateInfo
		{
			Preference = MemoryPreference.PreferGpu,
			RequiredProperties = VkMemoryPropertyFlags.DeviceLocalBit,
			PreferredProperties = VkMemoryPropertyFlags.DeviceLocalBit,
			Priority = 1f
		});
		GpuMemoryTracker.Add(GpuMemoryCategory.Other, in allocation, null, label);
		return allocation;
	}

	private VkImageView PictureView(VkImage image, VkFormat format)
	{
		DeviceEx handle = device;
		VkImageViewCreateInfo pCreateInfo = new VkImageViewCreateInfo
		{
			ViewType = VkImageViewType._2D,
			Image = image,
			Format = format,
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				LevelCount = 1,
				LayerCount = 1
			}
		};
		return handle.CreateImageView(in pCreateInfo, device.VkAllocator);
	}

	private unsafe VkImageView PlaneView(VkImage image, VkFormat format, VkImageAspectFlags plane)
	{
		VkImageViewUsageCreateInfo vkImageViewUsageCreateInfo = new VkImageViewUsageCreateInfo();
		vkImageViewUsageCreateInfo.Usage = VkImageUsageFlags.SampledBit;
		VkImageViewUsageCreateInfo vkImageViewUsageCreateInfo2 = vkImageViewUsageCreateInfo;
		DeviceEx handle = device;
		VkImageViewCreateInfo pCreateInfo = new VkImageViewCreateInfo
		{
			ViewType = VkImageViewType._2D,
			Image = image,
			Format = format,
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = plane,
				LevelCount = 1,
				LayerCount = 1
			},
			Next = &vkImageViewUsageCreateInfo2
		};
		return handle.CreateImageView(in pCreateInfo, device.VkAllocator);
	}

	private unsafe VkFormat PickFormat(VkImageUsageFlags usage)
	{
		VkPhysicalDeviceVideoFormatInfoKHR vkPhysicalDeviceVideoFormatInfoKHR = new VkPhysicalDeviceVideoFormatInfoKHR();
		vkPhysicalDeviceVideoFormatInfoKHR.ImageUsage = usage;
		vkPhysicalDeviceVideoFormatInfoKHR.Next = profileList;
		VkPhysicalDeviceVideoFormatInfoKHR pVideoFormatInfo = vkPhysicalDeviceVideoFormatInfoKHR;
		PhysicalDevice physicalDevice = graphicsDevice.PhysicalDevice;
		VkVideoFormatPropertiesKHR[] array = new VkVideoFormatPropertiesKHR[physicalDevice.GetVideoFormatPropertiesKHR_Count(in pVideoFormatInfo)];
		Array.Fill(array, new VkVideoFormatPropertiesKHR());
		physicalDevice.GetVideoFormatPropertiesKHR_Write(in pVideoFormatInfo, array);
		VkVideoFormatPropertiesKHR[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			VkVideoFormatPropertiesKHR vkVideoFormatPropertiesKHR = array2[i];
			if (vkVideoFormatPropertiesKHR.Format == VkFormat.G8B8R82Plane420UNorm && vkVideoFormatPropertiesKHR.ImageTiling == VkImageTiling.Optimal)
			{
				return vkVideoFormatPropertiesKHR.Format;
			}
		}
		throw new H264Exception("h264: driver offers no NV12 format for " + usage);
	}

	private void BindSessionMemory()
	{
		int videoSessionMemoryRequirementsKHR_Count = device.GetVideoSessionMemoryRequirementsKHR_Count(session);
		VkVideoSessionMemoryRequirementsKHR[] array = new VkVideoSessionMemoryRequirementsKHR[videoSessionMemoryRequirementsKHR_Count];
		Array.Fill(array, new VkVideoSessionMemoryRequirementsKHR());
		device.GetVideoSessionMemoryRequirementsKHR_Write(session, array);
		VkBindVideoSessionMemoryInfoKHR[] array2 = new VkBindVideoSessionMemoryInfoKHR[videoSessionMemoryRequirementsKHR_Count];
		sessionMemory = new VkDeviceMemory[videoSessionMemoryRequirementsKHR_Count];
		for (int i = 0; i < videoSessionMemoryRequirementsKHR_Count; i++)
		{
			VkVideoSessionMemoryRequirementsKHR vkVideoSessionMemoryRequirementsKHR = array[i];
			sessionMemory[i] = device.AllocateMemory(new VkMemoryAllocateInfo
			{
				MemoryTypeIndex = graphicsDevice.PhysicalDevice.FindMemoryType(vkVideoSessionMemoryRequirementsKHR.MemoryRequirements.MemoryTypeBits, VkMemoryPropertyFlags.None),
				AllocationSize = vkVideoSessionMemoryRequirementsKHR.MemoryRequirements.Size
			}, device.VkAllocator);
			array2[i] = new VkBindVideoSessionMemoryInfoKHR
			{
				Memory = sessionMemory[i],
				MemoryBindIndex = vkVideoSessionMemoryRequirementsKHR.MemoryBindIndex,
				MemorySize = vkVideoSessionMemoryRequirementsKHR.MemoryRequirements.Size
			};
		}
		device.BindVideoSessionMemoryKHR(session, array2);
	}

	private void FreeImage(ImageAllocationInfo image)
	{
		GpuMemoryTracker.Remove(GpuMemoryCategory.Other, in image);
		allocator.FreeAllocation(in image);
	}

	public unsafe void Dispose()
	{
		if (sessionSps != null)
		{
			foreach (H264Picture item in pictures.Dpb)
			{
				if (!item.Outputted && item.Payload is GpuTextureAllocation obj)
				{
					ReleaseImage(obj);
				}
			}
			if (!fence.IsNull())
			{
				device.DestroyCommandPool(decodePool, device.VkAllocator);
				device.DestroyCommandPool(graphicsPool, device.VkAllocator);
				device.DestroyDescriptorPool(descriptorPool, device.VkAllocator);
				device.DestroyFence(fence, device.VkAllocator);
			}
			VkImageView[] array = lumaViews;
			foreach (VkImageView imageView in array)
			{
				device.DestroyImageView(imageView, device.VkAllocator);
			}
			array = chromaViews;
			foreach (VkImageView imageView2 in array)
			{
				device.DestroyImageView(imageView2, device.VkAllocator);
			}
			for (int j = 0; j < dpbViews.Length; j++)
			{
				if (!dpbViews[j].IsNull())
				{
					device.DestroyImageView(dpbViews[j], device.VkAllocator);
					FreeImage(dpbImages[j]);
				}
			}
			if (!outputView.IsNull())
			{
				device.DestroyImageView(outputView, device.VkAllocator);
				FreeImage(outputImage);
			}
			if (hasParameters)
			{
				device.DestroyVideoSessionParametersKHR(parameters, device.VkAllocator);
			}
			device.DestroyVideoSessionKHR(session, device.VkAllocator);
			VkDeviceMemory[] array2 = sessionMemory;
			foreach (VkDeviceMemory memory in array2)
			{
				device.FreeMemory(memory, device.VkAllocator);
			}
			FreeBitstream();
			sessionSps = null;
		}
		NativeMemory.Free(codecProfile);
		NativeMemory.Free(profile);
		NativeMemory.Free(profileList);
	}
}
