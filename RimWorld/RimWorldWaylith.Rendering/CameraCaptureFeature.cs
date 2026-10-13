using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityStubs.Rendering;
using Verse;
using Waylith.Graphics;
using Waylith.Rendering;

namespace RimWorldWaylith.Rendering;

public sealed class CameraCaptureFeature : IRenderFeature, IDisposable
{
	private readonly TransparentDepthOrder depthOrder = new TransparentDepthOrder();

	private readonly GraphicsDevice graphicsDevice;

	private readonly CameraTextureBaker baker;

	private readonly int waterDepthMask = UnityEngine.LayerMask.GetMask("WaterDepth");

	private readonly List<Camera> captureCameras = new List<Camera>();

	private readonly List<FinalDrawItem> items = new List<FinalDrawItem>();

	private readonly List<FinalDrawItem> rented = new List<FinalDrawItem>();

	public bool IsActive => false;

	public IReadOnlyList<IRenderPass> Passes => Array.Empty<IRenderPass>();

	public CameraCaptureFeature(GraphicsDevice graphicsDevice, CameraTextureBaker baker)
	{
		this.graphicsDevice = graphicsDevice;
		this.baker = baker;
	}

	public object? CaptureFrame(in FrameTiming timing)
	{
		captureCameras.Clear();
		if (LongEventState.EventWithoutStandardWindowRunning)
		{
			return null;
		}
		Camera[] array = UnityEngine.Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
		foreach (Camera camera in array)
		{
			if (camera.enabled && !(camera.targetTexture == null) && camera.cullingMask != waterDepthMask)
			{
				captureCameras.Add(camera);
			}
		}
		if (captureCameras.Count == 0)
		{
			return null;
		}
		captureCameras.Sort((Camera a, Camera b) => a.depth.CompareTo(b.depth));
		MeshDrawSubmissionSnapshot meshDrawSubmissionSnapshot = MeshDrawSubmission.CaptureFlushedSnapshot(graphicsDevice);
		DrawItemFactory drawItemMaker = MeshDrawSubmission.DrawItemMaker;
		if (drawItemMaker == null)
		{
			return null;
		}
		string globalKeywordKey = UnityEngine.Shader.GlobalKeywordKey;
		MeshDrawCommand[] commands = meshDrawSubmissionSnapshot.Commands;
		foreach (Camera captureCamera in captureCameras)
		{
			Camera.onPreCull?.Invoke(captureCamera);
			Camera.onPreRender?.Invoke(captureCamera);
			int cullingMask = captureCamera.cullingMask;
			items.Clear();
			rented.Clear();
			depthOrder.Compute(commands, meshDrawSubmissionSnapshot.CommandCount, CameraViewFactory.GetView(captureCamera).View);
			for (int j = 0; j < meshDrawSubmissionSnapshot.CommandCount; j++)
			{
				FinalDrawItem finalDrawItem = drawItemMaker.TryMake(in commands[j], meshDrawSubmissionSnapshot.PropertyArena, globalKeywordKey, j, null, depthOrder[j]);
				if (finalDrawItem != null)
				{
					if (Wanted(finalDrawItem, cullingMask))
					{
						items.Add(finalDrawItem);
						rented.Add(finalDrawItem);
					}
					else
					{
						FinalDrawItem.Return(finalDrawItem);
					}
				}
			}
			for (int k = 0; k < meshDrawSubmissionSnapshot.PersistentSources.Count; k++)
			{
				IReadOnlyList<FinalDrawItem> drawItemsReadOnly = meshDrawSubmissionSnapshot.PersistentSources[k].DrawItemsReadOnly;
				for (int l = 0; l < drawItemsReadOnly.Count; l++)
				{
					if (Wanted(drawItemsReadOnly[l], cullingMask))
					{
						items.Add(drawItemsReadOnly[l]);
					}
				}
			}
			items.Sort(DrawSortKeyComparer.Instance);
			Color? clearColor = ((captureCamera.clearFlags == CameraClearFlags.Color || captureCamera.clearFlags == CameraClearFlags.Skybox) ? new Color?(captureCamera.backgroundColor) : ((Color?)null));
			baker.RenderItems(captureCamera.targetTexture, items, captureCamera, captureCamera.rect, clearColor, ResolveSunDirection());
			for (int m = 0; m < rented.Count; m++)
			{
				FinalDrawItem.Return(rented[m]);
			}
			items.Clear();
			rented.Clear();
			Camera.onPostRender?.Invoke(captureCamera);
		}
		return null;
	}

	private bool Wanted(FinalDrawItem draw, int cullingMask)
	{
		if (draw.PassId == RenderPassIds.WaterDepth)
		{
			return false;
		}
		return ((1 << draw.Layer) & cullingMask) != 0;
	}

	private static System.Numerics.Vector3? ResolveSunDirection()
	{
		if ((UnityEngine.Shader.TryGetGlobalFloat(ShaderPropertyIDs.PlanetSunLightEnabled, out var value) && !(value > 0.5f)) || !UnityEngine.Shader.TryGetGlobalVector(ShaderPropertyIDs.PlanetSunLightDirection, out var value2))
		{
			return null;
		}
		System.Numerics.Vector3 value3 = new System.Numerics.Vector3(value2.x, value2.y, value2.z);
		if (!(value3.LengthSquared() > 1E-06f))
		{
			return null;
		}
		return System.Numerics.Vector3.Normalize(value3);
	}

	public void OnRenderTargetRebuilt(IRenderTarget renderTarget)
	{
	}

	public void Dispose()
	{
	}

	object? IRenderFeature.CaptureFrame(in FrameTiming timing)
	{
		return CaptureFrame(in timing);
	}
}
