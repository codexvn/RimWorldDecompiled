using System;
using System.Collections.Generic;
using RimWorld;
using Unity.Burst;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005F8 RID: 1528
	public class PawnRenderTree
	{
		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002722 RID: 10018 RVA: 0x000CFDB2 File Offset: 0x000CDFB2
		public bool Resolved
		{
			get
			{
				return this.rootNode != null;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002723 RID: 10019 RVA: 0x000CFDC0 File Offset: 0x000CDFC0
		public Graphic BodyGraphic
		{
			get
			{
				this.TrySetupGraphIfNeeded();
				PawnRenderNode pawnRenderNode;
				if (!this.nodesByTag.TryGetValue(PawnRenderNodeTagDefOf.Body, out pawnRenderNode))
				{
					return null;
				}
				if (pawnRenderNode == null)
				{
					return null;
				}
				return pawnRenderNode.PrimaryGraphic;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002724 RID: 10020 RVA: 0x000CFDF4 File Offset: 0x000CDFF4
		public Graphic HeadGraphic
		{
			get
			{
				this.TrySetupGraphIfNeeded();
				PawnRenderNode pawnRenderNode;
				if (!this.nodesByTag.TryGetValue(PawnRenderNodeTagDefOf.Head, out pawnRenderNode))
				{
					return null;
				}
				if (pawnRenderNode == null)
				{
					return null;
				}
				return pawnRenderNode.PrimaryGraphic;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002725 RID: 10021 RVA: 0x000CFE28 File Offset: 0x000CE028
		public int AnimationTick
		{
			get
			{
				if (this.currentAnimation == null || this.currentAnimation.durationTicks <= 0)
				{
					return 0;
				}
				int num = GenTicks.TicksGame - this.animationStartTick;
				if (this.currentAnimation.loopMode != LoopMode.Clamp)
				{
					return GenMath.PositiveMod(num, this.currentAnimation.durationTicks);
				}
				if (num <= this.currentAnimation.durationTicks)
				{
					return num;
				}
				return this.currentAnimation.durationTicks;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002726 RID: 10022 RVA: 0x000CFE98 File Offset: 0x000CE098
		public bool AnimationFinished
		{
			get
			{
				if (this.currentAnimation == null || this.currentAnimation.durationTicks <= 0)
				{
					return false;
				}
				int num = GenTicks.TicksGame - this.animationStartTick;
				return this.currentAnimation.loopMode == LoopMode.End && num > this.currentAnimation.durationTicks;
			}
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000CFEE8 File Offset: 0x000CE0E8
		public PawnRenderTree(Pawn pawn)
		{
			this.pawn = pawn;
			if (PawnRenderTree.dynamicNodeTypeInstances.Count == 0)
			{
				PawnRenderTree.InitializeDynamicNodeTypes();
			}
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000CFF58 File Offset: 0x000CE158
		private static void InitializeDynamicNodeTypes()
		{
			foreach (Type type in typeof(DynamicPawnRenderNodeSetup).AllSubclassesNonAbstract())
			{
				PawnRenderTree.dynamicNodeTypeInstances.Add((DynamicPawnRenderNodeSetup)Activator.CreateInstance(type));
			}
			PawnRenderTree.dynamicNodeTypeInstances.Sort(delegate(DynamicPawnRenderNodeSetup x, DynamicPawnRenderNodeSetup y)
			{
				if (x.SetupAfter != null && x.SetupAfter.Contains(y.GetType()))
				{
					return 1;
				}
				if (y.SetupAfter != null && y.SetupAfter.Contains(x.GetType()))
				{
					return -1;
				}
				return 0;
			});
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000CFFEC File Offset: 0x000CE1EC
		public void EnsureInitialized(PawnRenderFlags defaultRenderFlagsNow)
		{
			this.TrySetupGraphIfNeeded();
			PawnRenderNode pawnRenderNode = this.rootNode;
			if (pawnRenderNode == null)
			{
				return;
			}
			pawnRenderNode.EnsureInitialized(defaultRenderFlagsNow);
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000D0008 File Offset: 0x000CE208
		public void ParallelPreDraw(PawnDrawParms parms)
		{
			if (!this.HasValidPropsAndRenderTree)
			{
				return;
			}
			this.AdjustParms(ref parms);
			if (this.oldParms.ShouldRecache(parms) || this.rootNode.RecacheRequested)
			{
				this.TraverseTree(delegate(PawnRenderNode node)
				{
					node.requestRecache = false;
				});
				this.drawRequests.Clear();
				this.rootNode.AppendRequests(parms, this.drawRequests);
				this.oldParms = parms;
			}
			for (int i = 0; i < this.drawRequests.Count; i++)
			{
				PawnGraphicDrawRequest pawnGraphicDrawRequest = this.drawRequests[i];
				Matrix4x4 preDrawnComputedMatrix;
				if (!this.TryGetMatrix(pawnGraphicDrawRequest.node, parms, out preDrawnComputedMatrix))
				{
					Log.ErrorOnce(string.Format("Failed to get matrix for {0} on {1}.", pawnGraphicDrawRequest.node, this.pawn), Gen.HashCombine<int>(174383246, this.pawn.GetHashCode()));
					return;
				}
				if (pawnGraphicDrawRequest.node.CheckMaterialEveryDrawRequest)
				{
					pawnGraphicDrawRequest.material = pawnGraphicDrawRequest.node.Worker.GetFinalizedMaterial(pawnGraphicDrawRequest.node, parms);
				}
				pawnGraphicDrawRequest.preDrawnComputedMatrix = preDrawnComputedMatrix;
				this.drawRequests[i] = pawnGraphicDrawRequest;
			}
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000D0138 File Offset: 0x000CE338
		public void Draw(PawnDrawParms parms)
		{
			if (!this.Resolved)
			{
				Log.ErrorOnce(string.Format("Attempted to draw {0} without a resolved render tree.", this.pawn), Gen.HashCombine<int>(174383246, this.pawn.GetHashCode()));
				return;
			}
			using (new ProfilerBlock("PawnRenderTree.Draw"))
			{
				for (int i = 0; i < this.drawRequests.Count; i++)
				{
					PawnGraphicDrawRequest pawnGraphicDrawRequest = this.drawRequests[i];
					Material material = pawnGraphicDrawRequest.material;
					if (material != null)
					{
						pawnGraphicDrawRequest.node.Worker.PreDraw(pawnGraphicDrawRequest.node, material, parms);
						MaterialPropertyBlock materialPropertyBlock = pawnGraphicDrawRequest.node.Worker.GetMaterialPropertyBlock(pawnGraphicDrawRequest.node, material, parms);
						foreach (PawnRenderSubWorker pawnRenderSubWorker in pawnGraphicDrawRequest.node.Props.SubWorkers)
						{
							pawnRenderSubWorker.EditMaterialPropertyBlock(pawnGraphicDrawRequest.node, material, parms, ref materialPropertyBlock);
						}
						GenDraw.DrawMeshNowOrLater(pawnGraphicDrawRequest.mesh, pawnGraphicDrawRequest.preDrawnComputedMatrix, material, parms.DrawNow, materialPropertyBlock);
						materialPropertyBlock.Clear();
					}
					pawnGraphicDrawRequest.node.Worker.PostDraw(pawnGraphicDrawRequest.node, parms, pawnGraphicDrawRequest.mesh, pawnGraphicDrawRequest.preDrawnComputedMatrix);
				}
			}
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000D02AC File Offset: 0x000CE4AC
		public bool GetRootTPRS(PawnDrawParms parms, out Vector3 offset, out Vector3 pivot, out Quaternion rotation, out Vector3 scale)
		{
			offset = Vector3.zero;
			pivot = Vector3.zero;
			rotation = Quaternion.identity;
			scale = Vector3.zero;
			if (this.rootNode == null || this.nodeAncestors == null)
			{
				return false;
			}
			List<PawnRenderNode> list;
			bool result = this.nodeAncestors.TryGetValue(this.rootNode, out list);
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 b;
				Vector3 b2;
				Quaternion rhs;
				Vector3 b3;
				list[i].GetTransform(parms, out b, out b2, out rhs, out b3);
				offset += b;
				pivot += b2;
				rotation *= rhs;
				scale += b3;
			}
			return result;
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000D0384 File Offset: 0x000CE584
		public bool TryGetMatrix(PawnRenderNode node, PawnDrawParms parms, out Matrix4x4 matrix)
		{
			matrix = parms.matrix;
			List<PawnRenderNode> list;
			if (!this.nodeAncestors.TryGetValue(node, out list))
			{
				this.SetDirty();
				this.TrySetupGraphIfNeeded();
				if (!this.nodeAncestors.TryGetValue(node, out list))
				{
					return false;
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 vector;
				Vector3 vector2;
				Quaternion quaternion;
				Vector3 vector3;
				list[i].GetTransform(parms, out vector, out vector2, out quaternion, out vector3);
				bool canRotate = !node.Props.rotateIndependently || list[i] == node;
				this.ComputeMatrix(ref matrix, vector, vector2, quaternion, vector3, canRotate);
			}
			float num = node.Worker.AltitudeFor(node, parms);
			if (num != 0f)
			{
				matrix *= Matrix4x4.Translate(Vector3.up * num);
			}
			return true;
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x000D0458 File Offset: 0x000CE658
		[BurstCompile]
		private void ComputeMatrix(ref Matrix4x4 matrix, in Vector3 offset, in Vector3 pivot, in Quaternion rotation, in Vector3 scale, bool canRotate)
		{
			if (offset != Vector3.zero)
			{
				matrix *= Matrix4x4.Translate(offset);
			}
			if (pivot != Vector3.zero)
			{
				matrix *= Matrix4x4.Translate(pivot);
			}
			if (canRotate && rotation != Quaternion.identity)
			{
				matrix *= Matrix4x4.Rotate(rotation);
			}
			if (scale != Vector3.one)
			{
				matrix *= Matrix4x4.Scale(scale);
			}
			if (pivot != Vector3.zero)
			{
				matrix *= Matrix4x4.Translate(pivot).inverse;
			}
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x000D055C File Offset: 0x000CE75C
		private void AdjustParms(ref PawnDrawParms parms)
		{
			if (this.debugTint != null)
			{
				parms.tint *= this.debugTint.Value;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				if (parms.crawling && !parms.Portrait && parms.facing == Rot4.South)
				{
					parms.facing = Rot4.North;
					parms.flipHead = true;
				}
				if (this.pawn.apparel != null && PawnRenderNodeWorker_Apparel_Head.HeadgearVisible(parms))
				{
					foreach (Apparel apparel in this.pawn.apparel.WornApparel)
					{
						if (apparel.def.apparel.renderSkipFlags != null)
						{
							using (List<RenderSkipFlagDef>.Enumerator enumerator2 = apparel.def.apparel.renderSkipFlags.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									RenderSkipFlagDef renderSkipFlagDef = enumerator2.Current;
									if (renderSkipFlagDef != RenderSkipFlagDefOf.None)
									{
										parms.skipFlags |= renderSkipFlagDef;
									}
								}
								goto IL_1A8;
							}
							goto IL_120;
						}
						goto IL_120;
						IL_1A8:
						if (apparel.def.apparel.forceEyesVisibleForRotations.Contains(parms.facing.AsInt))
						{
							parms.skipFlags &= ~RenderSkipFlagDefOf.Eyes;
							continue;
						}
						continue;
						IL_120:
						if (apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))
						{
							parms.skipFlags |= RenderSkipFlagDefOf.Hair;
						}
						if (apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead))
						{
							parms.skipFlags |= RenderSkipFlagDefOf.Hair;
							parms.skipFlags |= RenderSkipFlagDefOf.Beard;
							parms.skipFlags |= RenderSkipFlagDefOf.Eyes;
							goto IL_1A8;
						}
						goto IL_1A8;
					}
				}
				if (this.pawn.genes != null && !this.pawn.genes.TattoosVisible)
				{
					parms.skipFlags |= RenderSkipFlagDefOf.Tattoos;
				}
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x000D07CC File Offset: 0x000CE9CC
		private bool HasValidPropsAndRenderTree
		{
			get
			{
				PawnRenderTreeDef renderTree = this.pawn.RaceProps.renderTree;
				Type left;
				if (renderTree == null)
				{
					left = null;
				}
				else
				{
					PawnRenderNodeProperties root = renderTree.root;
					left = ((root != null) ? root.nodeClass : null);
				}
				return left != null;
			}
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000D07FC File Offset: 0x000CE9FC
		private void TrySetupGraphIfNeeded()
		{
			if (this.Resolved)
			{
				return;
			}
			PawnRenderTreeDef renderTree = this.pawn.RaceProps.renderTree;
			PawnRenderNodeProperties pawnRenderNodeProperties = (renderTree != null) ? renderTree.root : null;
			if (((pawnRenderNodeProperties != null) ? pawnRenderNodeProperties.nodeClass : null) == null)
			{
				return;
			}
			this.SetDirty();
			using (new ProfilerBlock("TrySetupGraph"))
			{
				try
				{
					this.rootNode = (PawnRenderNode)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
					{
						this.pawn,
						pawnRenderNodeProperties,
						this
					});
					this.SetupDynamicNodes();
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Exception setting up dynamic nodes for {0}: {1}", this.pawn, arg));
				}
				foreach (KeyValuePair<PawnRenderNodeTagDef, List<PawnRenderNode>> keyValuePair in this.tmpChildTagNodes)
				{
					PawnRenderNodeTagDef pawnRenderNodeTagDef;
					List<PawnRenderNode> list;
					keyValuePair.Deconstruct(out pawnRenderNodeTagDef, out list);
					PawnRenderNodeTagDef key = pawnRenderNodeTagDef;
					List<PawnRenderNode> list2 = list;
					PawnRenderNode pawnRenderNode;
					if (this.nodesByTag.TryGetValue(key, out pawnRenderNode))
					{
						pawnRenderNode.AddChildren(list2.ToArray());
					}
				}
				if (Prefs.DevMode)
				{
					Dialog_DebugRenderTree dialog_DebugRenderTree = Find.WindowStack.WindowOfType<Dialog_DebugRenderTree>();
					if (dialog_DebugRenderTree != null && dialog_DebugRenderTree.pawn == this.pawn)
					{
						dialog_DebugRenderTree.Init(this.pawn);
					}
				}
				this.InitializeAncestors();
			}
			this.tmpChildTagNodes.Clear();
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x000D0984 File Offset: 0x000CEB84
		public bool TryGetNodeByTag(PawnRenderNodeTagDef tag, out PawnRenderNode node)
		{
			return this.nodesByTag.TryGetValue(tag, out node);
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000D0994 File Offset: 0x000CEB94
		private void SetupDynamicNodes()
		{
			foreach (DynamicPawnRenderNodeSetup dynamicPawnRenderNodeSetup in PawnRenderTree.dynamicNodeTypeInstances)
			{
				if (!dynamicPawnRenderNodeSetup.HumanlikeOnly || this.pawn.RaceProps.Humanlike)
				{
					foreach (ValueTuple<PawnRenderNode, PawnRenderNode> valueTuple in dynamicPawnRenderNodeSetup.GetDynamicNodes(this.pawn, this))
					{
						PawnRenderNode item = valueTuple.Item1;
						PawnRenderNode item2 = valueTuple.Item2;
						this.AddChild(item, item2);
					}
				}
			}
			foreach (ThingComp thingComp in this.pawn.AllComps)
			{
				List<PawnRenderNode> list = thingComp.CompRenderNodes();
				if (list != null)
				{
					foreach (PawnRenderNode pawnRenderNode in list)
					{
						if (this.ShouldAddNodeToTree((pawnRenderNode != null) ? pawnRenderNode.Props : null))
						{
							this.AddChild(pawnRenderNode, null);
						}
					}
				}
			}
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000D0AF0 File Offset: 0x000CECF0
		private void InitializeAncestors()
		{
			this.TraverseTree(delegate(PawnRenderNode node)
			{
				if (!this.nodeAncestors.ContainsKey(node))
				{
					this.nodeAncestors.Add(node, new List<PawnRenderNode>());
				}
				for (PawnRenderNode pawnRenderNode = node; pawnRenderNode != null; pawnRenderNode = pawnRenderNode.parent)
				{
					this.nodeAncestors[node].Add(pawnRenderNode);
				}
				this.nodeAncestors[node].Reverse();
			});
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x000D0B04 File Offset: 0x000CED04
		private void TraverseTree(Action<PawnRenderNode> action)
		{
			try
			{
				this.nodeQueue.Enqueue(this.rootNode);
				while (this.nodeQueue.Count > 0)
				{
					PawnRenderNode pawnRenderNode = this.nodeQueue.Dequeue();
					if (pawnRenderNode == null)
					{
						Log.ErrorOnce(string.Format("Node is null - you must called EnsureGraphicsInitialized() on the drawn dynamic thing {0} before drawing it.", this.pawn), Gen.HashCombine<int>(1743846, this.pawn.GetHashCode()));
						break;
					}
					action(pawnRenderNode);
					if (pawnRenderNode.children != null)
					{
						foreach (PawnRenderNode item in pawnRenderNode.children)
						{
							this.nodeQueue.Enqueue(item);
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Exception traversing pawn render node tree {0}: {1}", this.pawn, arg));
			}
			finally
			{
				this.nodeQueue.Clear();
			}
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x000D0BE8 File Offset: 0x000CEDE8
		public bool ShouldAddNodeToTree(PawnRenderNodeProperties props)
		{
			if (props == null)
			{
				return false;
			}
			switch (props.pawnType)
			{
			case PawnRenderNodeProperties.RenderNodePawnType.HumanlikeOnly:
				return this.pawn.RaceProps.Humanlike;
			case PawnRenderNodeProperties.RenderNodePawnType.NonHumanlikeOnly:
				return !this.pawn.RaceProps.Humanlike;
			}
			return true;
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x000D0C3C File Offset: 0x000CEE3C
		private void AddChild(PawnRenderNode child, PawnRenderNode parent)
		{
			if (parent == null)
			{
				PawnRenderNode pawnRenderNode;
				if (child.Props.parentTagDef != null && this.nodesByTag.TryGetValue(child.Props.parentTagDef, out pawnRenderNode))
				{
					parent = pawnRenderNode;
				}
				else
				{
					parent = this.rootNode;
				}
			}
			if (parent.Props.tagDef != null)
			{
				List<PawnRenderNode> list;
				if (this.tmpChildTagNodes.TryGetValue(parent.Props.tagDef, out list))
				{
					list.Add(child);
				}
				else
				{
					this.tmpChildTagNodes.Add(parent.Props.tagDef, new List<PawnRenderNode>
					{
						child
					});
				}
			}
			child.parent = parent;
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x000D0CD9 File Offset: 0x000CEED9
		public void SetTagNode(PawnRenderNodeTagDef tag, PawnRenderNode node)
		{
			this.nodesByTag[tag] = node;
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000D0CE8 File Offset: 0x000CEEE8
		public void SetDirty()
		{
			this.nodeAncestors.Clear();
			this.drawRequests.Clear();
			this.rootNode = null;
			this.nodesByTag.Clear();
			this.oldParms = default(PawnDrawParms);
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000D0D1E File Offset: 0x000CEF1E
		public bool TryGetAnimationPartForNode(PawnRenderNode node, out AnimationPart animationPart)
		{
			animationPart = null;
			return this.currentAnimation != null && node.Props.tagDef != null && node.tree.currentAnimation.TryGetPartForNodeTag(node.Props.tagDef, out animationPart);
		}

		// Token: 0x04002079 RID: 8313
		public Pawn pawn;

		// Token: 0x0400207A RID: 8314
		public PawnRenderNode rootNode;

		// Token: 0x0400207B RID: 8315
		private PawnDrawParms oldParms;

		// Token: 0x0400207C RID: 8316
		public AnimationDef currentAnimation;

		// Token: 0x0400207D RID: 8317
		public int animationStartTick = -99999;

		// Token: 0x0400207E RID: 8318
		public Color? debugTint;

		// Token: 0x0400207F RID: 8319
		private readonly List<PawnGraphicDrawRequest> drawRequests = new List<PawnGraphicDrawRequest>();

		// Token: 0x04002080 RID: 8320
		private readonly Dictionary<PawnRenderNodeTagDef, PawnRenderNode> nodesByTag = new Dictionary<PawnRenderNodeTagDef, PawnRenderNode>();

		// Token: 0x04002081 RID: 8321
		private readonly Dictionary<PawnRenderNode, List<PawnRenderNode>> nodeAncestors = new Dictionary<PawnRenderNode, List<PawnRenderNode>>();

		// Token: 0x04002082 RID: 8322
		private readonly Dictionary<PawnRenderNodeTagDef, List<PawnRenderNode>> tmpChildTagNodes = new Dictionary<PawnRenderNodeTagDef, List<PawnRenderNode>>();

		// Token: 0x04002083 RID: 8323
		private static readonly List<DynamicPawnRenderNodeSetup> dynamicNodeTypeInstances = new List<DynamicPawnRenderNodeSetup>();

		// Token: 0x04002084 RID: 8324
		private const float ApparelLayerShellNorth = 88f;

		// Token: 0x04002085 RID: 8325
		private readonly Queue<PawnRenderNode> nodeQueue = new Queue<PawnRenderNode>();
	}
}
