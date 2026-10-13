using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003154 RID: 12628
	public class Dialog_DebugRenderTree : Window
	{
		// Token: 0x17002E2E RID: 11822
		// (get) Token: 0x06011C3A RID: 72762 RVA: 0x0052149D File Offset: 0x0051F69D
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 600f);
			}
		}

		// Token: 0x17002E2F RID: 11823
		// (get) Token: 0x06011C3B RID: 72763 RVA: 0x0019D0E8 File Offset: 0x0019B2E8
		protected override float Margin
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06011C3C RID: 72764 RVA: 0x005214B0 File Offset: 0x0051F6B0
		public Dialog_DebugRenderTree(Pawn pawn) : base(null)
		{
			this.doCloseX = true;
			this.preventCameraMotion = false;
			this.closeOnAccept = false;
			this.draggable = true;
			this.Init(pawn);
		}

		// Token: 0x06011C3D RID: 72765 RVA: 0x00521500 File Offset: 0x0051F700
		public void Init(Pawn pawn)
		{
			this.currentNode = null;
			this.scrollPosition = Vector2.zero;
			this.scrollHeight = 0f;
			this.nodeExpanded.Clear();
			this.pawn = pawn;
			this.tree = pawn.Drawer.renderer.renderTree;
			this.currentAnimation = this.tree.currentAnimation;
			this.optionalTitle = pawn.LabelShortCap + " (" + pawn.RaceProps.renderTree.defName + ")";
			this.drawParms = new PawnDrawParms
			{
				pawn = pawn,
				rotDrawMode = RotDrawMode.Fresh,
				flags = (PawnRenderFlags.Headgear | PawnRenderFlags.Clothes)
			};
			PawnRenderNode rootNode = this.tree.rootNode;
			if (rootNode == null)
			{
				return;
			}
			this.AddNode(rootNode, null);
		}

		// Token: 0x06011C3E RID: 72766 RVA: 0x005215D0 File Offset: 0x0051F7D0
		private void AddNode(PawnRenderNode node, PawnRenderNode parent)
		{
			if (parent != null && !this.nodeExpanded.ContainsKey(parent))
			{
				this.nodeExpanded.Add(parent, true);
			}
			if (!node.children.NullOrEmpty<PawnRenderNode>())
			{
				foreach (PawnRenderNode node2 in node.children)
				{
					this.AddNode(node2, node);
				}
			}
		}

		// Token: 0x06011C3F RID: 72767 RVA: 0x0052162C File Offset: 0x0051F82C
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			Pawn pawn = Find.Selector.SelectedPawns.FirstOrDefault<Pawn>();
			if (pawn != null && pawn != this.pawn)
			{
				this.Init(pawn);
			}
			this.drawParms.facing = this.pawn.Rotation;
			this.drawParms.posture = this.pawn.GetPosture();
			this.drawParms.bed = this.pawn.CurrentBed();
			this.drawParms.coveredInFoam = this.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam;
			Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
			this.drawParms.carriedThing = ((carryTracker != null) ? carryTracker.CarriedThing : null);
			this.drawParms.dead = this.pawn.Dead;
			this.drawParms.rotDrawMode = this.pawn.Drawer.renderer.CurRotDrawMode;
		}

		// Token: 0x06011C40 RID: 72768 RVA: 0x00521721 File Offset: 0x0051F921
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			this.LeftRect(inRect.LeftHalf());
			this.RightRect(inRect.RightHalf());
		}

		// Token: 0x06011C41 RID: 72769 RVA: 0x00521744 File Offset: 0x0051F944
		private void LeftRect(Rect inRect)
		{
			if (this.pawn == null || this.tree == null || !this.tree.Resolved)
			{
				this.Close(true);
				return;
			}
			Widgets.DrawMenuSection(inRect);
			inRect = inRect.ContractedBy(this.Margin / 2f);
			Widgets.HorizontalSlider(new Rect(inRect.x + this.Margin, inRect.y, inRect.width - this.Margin * 2f, 25f), ref this.alpha, FloatRange.ZeroToOne, "Alpha " + this.alpha.ToStringPercent(), 0.01f);
			this.pawn.Drawer.renderer.renderTree.debugTint = new Color?(Color.white.ToTransparent(this.alpha));
			inRect.yMin += 25f + this.Margin;
			Rect rect = new Rect(inRect.x + this.Margin, inRect.y, "Animation".GetWidthCached() + 4f, 25f);
			using (new TextBlock(GameFont.Tiny, TextAnchor.MiddleLeft))
			{
				Widgets.Label(rect, "Animation");
			}
			Rect rect2 = new Rect(rect.xMax + 4f, rect.y, inRect.width - rect.width - this.Margin * 2f, 25f);
			Widgets.DrawLightHighlight(rect2);
			Widgets.DrawHighlightIfMouseover(rect2);
			using (new TextBlock(TextAnchor.MiddleCenter))
			{
				Widgets.Label(rect2, (this.currentAnimation == null) ? "None" : this.currentAnimation.defName);
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("None", delegate()
				{
					this.currentAnimation = null;
					this.pawn.Drawer.renderer.SetAnimation(null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				foreach (AnimationDef animationDef in DefDatabase<AnimationDef>.AllDefsListForReading)
				{
					AnimationDef def = animationDef;
					list.Add(new FloatMenuOption(animationDef.defName, delegate()
					{
						this.currentAnimation = def;
						this.pawn.Drawer.renderer.SetAnimation(def);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			inRect.yMin += 25f + this.Margin;
			using (new TextBlock(GameFont.Tiny))
			{
				Widgets.CheckboxLabeled(new Rect(inRect.x + this.Margin, inRect.y, inRect.width - this.Margin * 2f, 25f), "Show all nodes", ref this.showAll, false, null, null, false, false);
			}
			inRect.yMin += 25f + this.Margin;
			Widgets.BeginScrollView(inRect, ref this.scrollPosition, new Rect(0f, 0f, inRect.width - 16f, this.scrollHeight), true);
			float num = 0f;
			this.ListNode(this.tree.rootNode, 0, ref num, inRect.width - 16f);
			if (Event.current.type == EventType.Layout)
			{
				this.scrollHeight = num;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06011C42 RID: 72770 RVA: 0x00521B04 File Offset: 0x0051FD04
		private void RightRect(Rect inRect)
		{
			Widgets.DrawMenuSection(inRect);
			inRect = inRect.ContractedBy(this.Margin / 2f);
			Widgets.BeginGroup(inRect);
			Rect rect = new Rect(0f, 0f, inRect.width, Text.LineHeight);
			Rect rect2 = new Rect(this.Margin, rect.height + this.Margin, inRect.width - this.Margin * 2f, 30f);
			Widgets.DrawLightHighlight(rect);
			if (this.currentNode != null)
			{
				using (new TextBlock(TextAnchor.MiddleCenter))
				{
					Widgets.Label(rect, this.currentNode.ToString().Truncate(inRect.width, null));
				}
				Widgets.Label(rect2, this.currentNode.Worker.GetType().Name.Truncate(inRect.width, null));
				rect2.y += rect2.height;
				bool debugEnabled = this.currentNode.debugEnabled;
				Widgets.CheckboxLabeled(rect2, "Enabled", ref debugEnabled, false, null, null, false, false);
				if (debugEnabled != this.currentNode.debugEnabled)
				{
					this.currentNode.debugEnabled = debugEnabled;
					this.currentNode.requestRecache = true;
				}
				rect2.y += rect2.height;
				Vector3 vector2;
				Vector3 vector = this.currentNode.Worker.OffsetFor(this.currentNode, this.drawParms, out vector2);
				vector.y = this.currentNode.Worker.AltitudeFor(this.currentNode, this.drawParms);
				float num = this.currentNode.Props.baseLayer;
				if (this.currentNode.Props.drawData != null)
				{
					num = this.currentNode.Props.drawData.LayerForRot(this.drawParms.facing, num);
				}
				num += this.currentNode.debugLayerOffset;
				Widgets.Label(rect2, "Offset " + vector.ToString("F2") + " Layer " + num.ToString());
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugOffset.x, Dialog_DebugRenderTree.OffsetRange, "X: " + this.currentNode.debugOffset.x.ToString(), 0.05f);
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugOffset.z, Dialog_DebugRenderTree.OffsetRange, "Z: " + this.currentNode.debugOffset.z.ToString(), 0.05f);
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugLayerOffset, Dialog_DebugRenderTree.LayerRange, "Layer: " + this.currentNode.debugLayerOffset.ToString(), 1f);
				rect2.y += rect2.height;
				rect2.y += this.Margin;
				Widgets.Label(rect2, string.Concat(new string[]
				{
					"Pivot (",
					vector2.x.ToStringPercent(),
					", ",
					vector2.z.ToStringPercent(),
					")"
				}));
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugPivotOffset.x, FloatRange.ZeroToOne, "X: " + this.currentNode.debugPivotOffset.x.ToStringPercent(), 0.01f);
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugPivotOffset.y, FloatRange.ZeroToOne, "Y: " + this.currentNode.debugPivotOffset.y.ToStringPercent(), 0.01f);
				rect2.y += rect2.height;
				rect2.y += this.Margin;
				float y = this.currentNode.Worker.RotationFor(this.currentNode, this.drawParms).eulerAngles.y;
				Widgets.Label(rect2, "Rotation " + y.ToString("F0") + "°");
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugAngleOffset, Dialog_DebugRenderTree.AngleRange, "Angle: " + this.currentNode.debugAngleOffset.ToString("F0") + "°", 1f);
				rect2.y += rect2.height;
				Vector3 vector3 = this.currentNode.Worker.ScaleFor(this.currentNode, this.drawParms);
				rect2.y += this.Margin;
				Rect rect3 = rect2;
				string str = "Scale ";
				Vector3 vector4 = vector3;
				Widgets.Label(rect3, str + vector4.ToString());
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugScale, Dialog_DebugRenderTree.ScaleRange, "Scale: " + this.currentNode.debugScale.ToStringPercent("F0"), 0.01f);
				rect2.y += rect2.height;
				rect2.y += this.Margin;
				bool debugFlip = this.currentNode.debugFlip;
				bool flag = this.currentNode.FlipGraphic(this.drawParms);
				Widgets.CheckboxLabeled(rect2, string.Format("Flip ({0})", flag), ref debugFlip, false, null, null, false, false);
				if (debugFlip != this.currentNode.debugFlip)
				{
					this.currentNode.debugFlip = debugFlip;
					this.currentNode.requestRecache = true;
				}
				rect2.y += rect2.height;
				if ((this.currentNode.debugAngleOffset != 0f || this.currentNode.debugScale != 1f || this.currentNode.debugOffset != Vector3.zero || this.currentNode.debugLayerOffset != 0f || !this.currentNode.debugEnabled || this.currentNode.debugPivotOffset != DrawData.PivotCenter) && Widgets.ButtonText(new Rect(0f, inRect.height - 25f, inRect.width, 25f), "Reset", true, true, true, null))
				{
					this.currentNode.debugAngleOffset = 0f;
					this.currentNode.debugScale = 1f;
					this.currentNode.debugOffset = Vector3.zero;
					this.currentNode.debugLayerOffset = 0f;
					this.currentNode.debugEnabled = true;
					this.currentNode.requestRecache = true;
					this.currentNode.debugPivotOffset = DrawData.PivotCenter;
				}
			}
			else
			{
				using (new TextBlock(TextAnchor.MiddleCenter))
				{
					Widgets.Label(rect, "No node selected");
				}
			}
			Widgets.EndGroup();
		}

		// Token: 0x06011C43 RID: 72771 RVA: 0x005222B4 File Offset: 0x005204B4
		private void ListNode(PawnRenderNode node, int indent, ref float curY, float width)
		{
			if (!this.showAll && !node.Worker.ShouldListOnGraph(node, this.drawParms))
			{
				return;
			}
			Rect rect = new Rect((float)(indent + 1) * 12f, curY, width, 25f)
			{
				xMax = width
			}.ContractedBy(2f);
			Widgets.DrawHighlight(rect);
			Widgets.DrawHighlightIfMouseover(rect);
			if (this.currentNode == node)
			{
				Widgets.DrawHighlight(rect);
			}
			Rect rect2 = new Rect(rect.x, rect.y, rect.height, rect.height);
			Widgets.DrawLightHighlight(rect2);
			if (node.Props.useGraphic)
			{
				Graphic primaryGraphic = node.PrimaryGraphic;
				Texture texture;
				if (primaryGraphic == null)
				{
					texture = null;
				}
				else
				{
					Material material = primaryGraphic.MatAt(Rot4.South, null);
					texture = ((material != null) ? material.mainTexture : null);
				}
				Texture texture2 = texture;
				if (texture2 != null)
				{
					GUI.DrawTexture(rect2, texture2);
				}
			}
			Rect rect3 = new Rect(rect2.xMax + 4f, rect.y, rect.width - rect.height - 4f, rect.height);
			if (!node.Worker.CanDrawNow(node, this.drawParms) || (node.Props.useGraphic && node.PrimaryGraphic == null))
			{
				GUI.color = ColoredText.SubtleGrayColor;
			}
			Widgets.Label(rect3, node.ToString().Truncate(rect3.width, null));
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.currentNode = node;
			}
			if (!node.children.NullOrEmpty<PawnRenderNode>())
			{
				Rect rect4 = new Rect((float)indent * 12f, curY + 6.5f, 12f, 12f);
				Widgets.DrawHighlightIfMouseover(rect4);
				if (Widgets.ButtonImage(rect4, this.nodeExpanded[node] ? TexButton.Minus : TexButton.Plus, true, null))
				{
					this.nodeExpanded[node] = !this.nodeExpanded[node];
				}
			}
			curY += 25f;
			if (!node.children.NullOrEmpty<PawnRenderNode>() && this.nodeExpanded[node])
			{
				foreach (PawnRenderNode node2 in node.children)
				{
					this.ListNode(node2, indent + 1, ref curY, width);
				}
			}
		}

		// Token: 0x06011C44 RID: 72772 RVA: 0x005224F8 File Offset: 0x005206F8
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 initialSize = this.InitialSize;
			this.windowRect = new Rect(5f, 5f, initialSize.x, initialSize.y).Rounded();
		}

		// Token: 0x0400B749 RID: 46921
		public Pawn pawn;

		// Token: 0x0400B74A RID: 46922
		private PawnRenderTree tree;

		// Token: 0x0400B74B RID: 46923
		private Vector2 scrollPosition;

		// Token: 0x0400B74C RID: 46924
		private float scrollHeight;

		// Token: 0x0400B74D RID: 46925
		private PawnRenderNode currentNode;

		// Token: 0x0400B74E RID: 46926
		private float alpha = 1f;

		// Token: 0x0400B74F RID: 46927
		private PawnDrawParms drawParms;

		// Token: 0x0400B750 RID: 46928
		private Dictionary<PawnRenderNode, bool> nodeExpanded = new Dictionary<PawnRenderNode, bool>();

		// Token: 0x0400B751 RID: 46929
		private bool showAll;

		// Token: 0x0400B752 RID: 46930
		private AnimationDef currentAnimation;

		// Token: 0x0400B753 RID: 46931
		private const float NodeHeight = 25f;

		// Token: 0x0400B754 RID: 46932
		private const float IndentWidth = 12f;

		// Token: 0x0400B755 RID: 46933
		private const float IndentHeightOffset = 6.5f;

		// Token: 0x0400B756 RID: 46934
		private static readonly FloatRange AngleRange = new FloatRange(-180f, 180f);

		// Token: 0x0400B757 RID: 46935
		private static readonly FloatRange ScaleRange = new FloatRange(0.01f, 10f);

		// Token: 0x0400B758 RID: 46936
		private static readonly FloatRange OffsetRange = new FloatRange(-10f, 10f);

		// Token: 0x0400B759 RID: 46937
		private static readonly FloatRange LayerRange = new FloatRange(-100f, 100f);
	}
}
