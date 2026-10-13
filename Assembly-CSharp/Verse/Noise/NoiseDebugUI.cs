using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000D9A RID: 3482
	public static class NoiseDebugUI
	{
		// Token: 0x17000E1A RID: 3610
		// (set) Token: 0x060058DA RID: 22746 RVA: 0x001C9E83 File Offset: 0x001C8083
		public static IntVec2 RenderSize
		{
			set
			{
				NoiseRenderer.renderSize = value;
			}
		}

		// Token: 0x060058DB RID: 22747 RVA: 0x001C9E8C File Offset: 0x001C808C
		public static void StoreTexture(Texture2D texture, string name)
		{
			NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(texture, name);
			NoiseDebugUI.noises2D.Add(item);
		}

		// Token: 0x060058DC RID: 22748 RVA: 0x001C9EAC File Offset: 0x001C80AC
		public static void StoreNoiseRender(ModuleBase noise, string name, IntVec2 renderSize)
		{
			NoiseDebugUI.RenderSize = renderSize;
			NoiseDebugUI.StoreNoiseRender(noise, name);
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x001C9EBC File Offset: 0x001C80BC
		public static void StoreNoiseRender(ModuleBase noise, string name)
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(noise, name);
			NoiseDebugUI.noises2D.Add(item);
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x001C9EEC File Offset: 0x001C80EC
		public static void StorePlanetNoise(ModuleBase noise, string name)
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			NoiseDebugUI.NoisePlanet item = new NoiseDebugUI.NoisePlanet(noise, name);
			NoiseDebugUI.planetNoises.Add(item);
		}

		// Token: 0x060058DF RID: 22751 RVA: 0x001C9F1C File Offset: 0x001C811C
		public static void NoiseDebugOnGUI()
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			if (Widgets.ButtonText(new Rect(0f, 40f, 200f, 30f), "Clear noise renders", true, true, true, null))
			{
				NoiseDebugUI.Clear();
			}
			if (Widgets.ButtonText(new Rect(200f, 40f, 200f, 30f), "Hide noise renders", true, true, true, null))
			{
				DebugViewSettings.drawRecordedNoise = false;
			}
			if (WorldRendererUtility.WorldSelected)
			{
				if (NoiseDebugUI.planetNoises.Any<NoiseDebugUI.NoisePlanet>() && Widgets.ButtonText(new Rect(400f, 40f, 200f, 30f), "Next planet noise", true, true, true, null))
				{
					if (Event.current.button == 1)
					{
						if (NoiseDebugUI.currentPlanetNoise == null || NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == -1)
						{
							NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.Count - 1];
						}
						else if (NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == 0)
						{
							NoiseDebugUI.currentPlanetNoise = null;
						}
						else
						{
							NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) - 1];
						}
					}
					else if (NoiseDebugUI.currentPlanetNoise == null || NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == -1)
					{
						NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[0];
					}
					else if (NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == NoiseDebugUI.planetNoises.Count - 1)
					{
						NoiseDebugUI.currentPlanetNoise = null;
					}
					else
					{
						NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) + 1];
					}
				}
				if (NoiseDebugUI.currentPlanetNoise != null)
				{
					Rect rect = new Rect(605f, 40f, 300f, 30f);
					Text.Font = GameFont.Medium;
					Widgets.Label(rect, NoiseDebugUI.currentPlanetNoise.name);
					Text.Font = GameFont.Small;
				}
			}
			float num = 0f;
			float num2 = 90f;
			Text.Font = GameFont.Tiny;
			foreach (NoiseDebugUI.Noise2D noise2D in NoiseDebugUI.noises2D)
			{
				Texture2D texture = noise2D.Texture;
				if (num + (float)texture.width + 5f > (float)UI.screenWidth)
				{
					num = 0f;
					num2 += (float)(texture.height + 5 + 25);
				}
				GUI.DrawTexture(new Rect(num, num2, (float)texture.width, (float)texture.height), texture);
				Rect rect2 = new Rect(num, num2 - 15f, (float)texture.width, (float)texture.height);
				GUI.color = Color.black;
				Widgets.Label(rect2, noise2D.name);
				GUI.color = Color.white;
				Widgets.Label(new Rect(rect2.x + 1f, rect2.y + 1f, rect2.width, rect2.height), noise2D.name);
				num += (float)(texture.width + 5);
			}
		}

		// Token: 0x060058E0 RID: 22752 RVA: 0x001CA254 File Offset: 0x001C8454
		public static void RenderPlanetNoise()
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			if (NoiseDebugUI.currentPlanetNoise == null)
			{
				return;
			}
			if (NoiseDebugUI.planetNoiseMesh == null)
			{
				List<int> triangles;
				SphereGenerator.Generate(6, 100.3f, Vector3.forward, 360f, out NoiseDebugUI.planetNoiseMeshVerts, out triangles);
				NoiseDebugUI.planetNoiseMesh = new Mesh();
				NoiseDebugUI.planetNoiseMesh.name = "NoiseDebugUI";
				NoiseDebugUI.planetNoiseMesh.SetVertices(NoiseDebugUI.planetNoiseMeshVerts);
				NoiseDebugUI.planetNoiseMesh.SetTriangles(triangles, 0);
				NoiseDebugUI.lastDrawnPlanetNoise = null;
			}
			if (NoiseDebugUI.lastDrawnPlanetNoise != NoiseDebugUI.currentPlanetNoise)
			{
				NoiseDebugUI.UpdatePlanetNoiseVertexColors();
				NoiseDebugUI.lastDrawnPlanetNoise = NoiseDebugUI.currentPlanetNoise;
			}
			Graphics.DrawMesh(NoiseDebugUI.planetNoiseMesh, Vector3.zero, Quaternion.identity, WorldMaterials.VertexColor, WorldCameraManager.WorldLayer);
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x001CA314 File Offset: 0x001C8514
		public static void Clear()
		{
			for (int i = 0; i < NoiseDebugUI.noises2D.Count; i++)
			{
				UnityEngine.Object.Destroy(NoiseDebugUI.noises2D[i].Texture);
			}
			NoiseDebugUI.noises2D.Clear();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x001CA35C File Offset: 0x001C855C
		public static void ClearPlanetNoises()
		{
			NoiseDebugUI.planetNoises.Clear();
			NoiseDebugUI.currentPlanetNoise = null;
			NoiseDebugUI.lastDrawnPlanetNoise = null;
			if (NoiseDebugUI.planetNoiseMesh != null)
			{
				Mesh localPlanetNoiseMesh = NoiseDebugUI.planetNoiseMesh;
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					UnityEngine.Object.Destroy(localPlanetNoiseMesh);
				});
				NoiseDebugUI.planetNoiseMesh = null;
			}
		}

		// Token: 0x060058E3 RID: 22755 RVA: 0x001CA3B4 File Offset: 0x001C85B4
		private static void UpdatePlanetNoiseVertexColors()
		{
			NoiseDebugUI.planetNoiseMeshColors.Clear();
			for (int i = 0; i < NoiseDebugUI.planetNoiseMeshVerts.Count; i++)
			{
				byte b = (byte)Mathf.Clamp((NoiseDebugUI.currentPlanetNoise.noise.GetValue(NoiseDebugUI.planetNoiseMeshVerts[i]) * 0.5f + 0.5f) * 255f, 0f, 255f);
				NoiseDebugUI.planetNoiseMeshColors.Add(new Color32(b, b, b, byte.MaxValue));
			}
			NoiseDebugUI.planetNoiseMesh.SetColors(NoiseDebugUI.planetNoiseMeshColors);
		}

		// Token: 0x04004064 RID: 16484
		private static List<NoiseDebugUI.Noise2D> noises2D = new List<NoiseDebugUI.Noise2D>();

		// Token: 0x04004065 RID: 16485
		private static List<NoiseDebugUI.NoisePlanet> planetNoises = new List<NoiseDebugUI.NoisePlanet>();

		// Token: 0x04004066 RID: 16486
		private static Mesh planetNoiseMesh;

		// Token: 0x04004067 RID: 16487
		private static NoiseDebugUI.NoisePlanet currentPlanetNoise;

		// Token: 0x04004068 RID: 16488
		private static NoiseDebugUI.NoisePlanet lastDrawnPlanetNoise;

		// Token: 0x04004069 RID: 16489
		private static List<Color32> planetNoiseMeshColors = new List<Color32>();

		// Token: 0x0400406A RID: 16490
		private static List<Vector3> planetNoiseMeshVerts;

		// Token: 0x02000D9B RID: 3483
		private class Noise2D
		{
			// Token: 0x17000E1B RID: 3611
			// (get) Token: 0x060058E5 RID: 22757 RVA: 0x001CA463 File Offset: 0x001C8663
			public Texture2D Texture
			{
				get
				{
					if (this.tex == null)
					{
						this.tex = NoiseRenderer.NoiseRendered(this.noise);
					}
					return this.tex;
				}
			}

			// Token: 0x060058E6 RID: 22758 RVA: 0x001CA48A File Offset: 0x001C868A
			public Noise2D(Texture2D tex, string name)
			{
				this.tex = tex;
				this.name = name;
			}

			// Token: 0x060058E7 RID: 22759 RVA: 0x001CA4A0 File Offset: 0x001C86A0
			public Noise2D(ModuleBase noise, string name)
			{
				this.noise = noise;
				this.name = name;
			}

			// Token: 0x0400406B RID: 16491
			public string name;

			// Token: 0x0400406C RID: 16492
			private Texture2D tex;

			// Token: 0x0400406D RID: 16493
			private ModuleBase noise;
		}

		// Token: 0x02000D9C RID: 3484
		private class NoisePlanet
		{
			// Token: 0x060058E8 RID: 22760 RVA: 0x001CA4B6 File Offset: 0x001C86B6
			public NoisePlanet(ModuleBase noise, string name)
			{
				this.name = name;
				this.noise = noise;
			}

			// Token: 0x0400406E RID: 16494
			public string name;

			// Token: 0x0400406F RID: 16495
			public ModuleBase noise;
		}
	}
}
