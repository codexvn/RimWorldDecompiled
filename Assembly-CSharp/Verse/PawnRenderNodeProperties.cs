using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000626 RID: 1574
	[StaticConstructorOnStartup]
	public class PawnRenderNodeProperties
	{
		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x0600282E RID: 10286 RVA: 0x000D5127 File Offset: 0x000D3327
		public PawnRenderNodeWorker Worker
		{
			get
			{
				return GenWorker<PawnRenderNodeWorker>.Get(this.workerClass);
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x000D5134 File Offset: 0x000D3334
		public List<PawnRenderSubWorker> SubWorkers
		{
			get
			{
				List<PawnRenderSubWorker> result;
				if ((result = this.subWorkersInt) == null)
				{
					result = (this.subWorkersInt = PawnRenderNodeWorker.GetSubWorkerList(this.subworkerClasses));
				}
				return result;
			}
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x000D515F File Offset: 0x000D335F
		public void EnsureInitialized()
		{
			if (this.workerInt == null)
			{
				this.workerInt = GenWorker<PawnRenderNodeWorker>.Get(this.workerClass);
			}
			if (this.subWorkersInt == null)
			{
				this.subWorkersInt = PawnRenderNodeWorker.GetSubWorkerList(this.subworkerClasses);
			}
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x000D5194 File Offset: 0x000D3394
		public virtual void ResolveReferencesRecursive()
		{
			this.ResolveReferences();
			if (this.children != null)
			{
				foreach (PawnRenderNodeProperties pawnRenderNodeProperties in this.children)
				{
					pawnRenderNodeProperties.ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x000D51F4 File Offset: 0x000D33F4
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.nodeClass == null)
			{
				yield return "Node " + this.debugLabel + ": nodeClass is null.";
			}
			else if (!typeof(PawnRenderNode).IsAssignableFrom(this.nodeClass))
			{
				yield return "Node " + this.debugLabel + ": nodeClass is not a PawnRenderNode or subclass thereof.";
			}
			if (this.workerClass == null)
			{
				yield return "Node " + this.debugLabel + ": workerClass is null.";
			}
			else if (!typeof(PawnRenderNodeWorker).IsAssignableFrom(this.workerClass))
			{
				yield return "Node " + this.debugLabel + ": workerClass is not a PawnRenderNodeWorker or subclass thereof.";
			}
			if (this.children != null)
			{
				foreach (PawnRenderNodeProperties pawnRenderNodeProperties in this.children)
				{
					foreach (string text in pawnRenderNodeProperties.ConfigErrors())
					{
						yield return text;
					}
					IEnumerator<string> enumerator2 = null;
				}
				List<PawnRenderNodeProperties>.Enumerator enumerator = default(List<PawnRenderNodeProperties>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04002134 RID: 8500
		public Type workerClass = typeof(PawnRenderNodeWorker);

		// Token: 0x04002135 RID: 8501
		public List<Type> subworkerClasses;

		// Token: 0x04002136 RID: 8502
		public Type nodeClass = typeof(PawnRenderNode);

		// Token: 0x04002137 RID: 8503
		public PawnRenderNodeTagDef tagDef;

		// Token: 0x04002138 RID: 8504
		public PawnRenderNodeTagDef parentTagDef;

		// Token: 0x04002139 RID: 8505
		public List<PawnRenderNodeProperties> children;

		// Token: 0x0400213A RID: 8506
		[NoTranslate]
		public string texPath;

		// Token: 0x0400213B RID: 8507
		[NoTranslate]
		public List<string> texPaths;

		// Token: 0x0400213C RID: 8508
		[NoTranslate]
		public string texPathFemale;

		// Token: 0x0400213D RID: 8509
		[NoTranslate]
		public List<string> texPathsFemale;

		// Token: 0x0400213E RID: 8510
		public List<BodyTypeGraphicData> bodyTypeGraphicPaths;

		// Token: 0x0400213F RID: 8511
		public int texSeed;

		// Token: 0x04002140 RID: 8512
		public bool useGraphic = true;

		// Token: 0x04002141 RID: 8513
		public RenderSkipFlagDef skipFlag;

		// Token: 0x04002142 RID: 8514
		public RotDrawMode rotDrawMode = RotDrawMode.Fresh | RotDrawMode.Rotting | RotDrawMode.Dessicated;

		// Token: 0x04002143 RID: 8515
		public PawnRenderNodeProperties.RenderNodePawnType pawnType = PawnRenderNodeProperties.RenderNodePawnType.HumanlikeOnly;

		// Token: 0x04002144 RID: 8516
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x04002145 RID: 8517
		public Color? color;

		// Token: 0x04002146 RID: 8518
		public float colorRGBPostFactor = 1f;

		// Token: 0x04002147 RID: 8519
		public bool useRottenColor;

		// Token: 0x04002148 RID: 8520
		public bool useSkinShader;

		// Token: 0x04002149 RID: 8521
		public ShaderTypeDef shaderTypeDef;

		// Token: 0x0400214A RID: 8522
		public PawnRenderNodeProperties.AttachmentColorType colorType;

		// Token: 0x0400214B RID: 8523
		public float baseLayer;

		// Token: 0x0400214C RID: 8524
		public DrawData drawData;

		// Token: 0x0400214D RID: 8525
		public bool oppositeFacingLayerWhenFlipped;

		// Token: 0x0400214E RID: 8526
		public Vector2? overrideMeshSize;

		// Token: 0x0400214F RID: 8527
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04002150 RID: 8528
		[NoTranslate]
		public string anchorTag;

		// Token: 0x04002151 RID: 8529
		public PawnRenderNodeProperties.Side side;

		// Token: 0x04002152 RID: 8530
		public float narrowCrownHorizontalOffset;

		// Token: 0x04002153 RID: 8531
		public List<Rot4> visibleFacing;

		// Token: 0x04002154 RID: 8532
		public bool flipGraphic;

		// Token: 0x04002155 RID: 8533
		public bool rotateIndependently;

		// Token: 0x04002156 RID: 8534
		public string debugLabel;

		// Token: 0x04002157 RID: 8535
		public PawnOverlayDrawer.OverlayLayer overlayLayer;

		// Token: 0x04002158 RID: 8536
		public bool? overlayOverApparel;

		// Token: 0x04002159 RID: 8537
		private PawnRenderNodeWorker workerInt;

		// Token: 0x0400215A RID: 8538
		private List<PawnRenderSubWorker> subWorkersInt;

		// Token: 0x02000627 RID: 1575
		public enum Side
		{
			// Token: 0x0400215C RID: 8540
			Center,
			// Token: 0x0400215D RID: 8541
			Left,
			// Token: 0x0400215E RID: 8542
			Right
		}

		// Token: 0x02000628 RID: 1576
		public enum AttachmentColorType
		{
			// Token: 0x04002160 RID: 8544
			Custom,
			// Token: 0x04002161 RID: 8545
			Hair,
			// Token: 0x04002162 RID: 8546
			Skin
		}

		// Token: 0x02000629 RID: 1577
		public enum RenderNodePawnType
		{
			// Token: 0x04002164 RID: 8548
			Any,
			// Token: 0x04002165 RID: 8549
			HumanlikeOnly,
			// Token: 0x04002166 RID: 8550
			NonHumanlikeOnly
		}
	}
}
