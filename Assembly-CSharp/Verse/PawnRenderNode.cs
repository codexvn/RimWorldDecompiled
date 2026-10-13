using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000623 RID: 1571
	public class PawnRenderNode
	{
		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060027F2 RID: 10226 RVA: 0x000D3ED0 File Offset: 0x000D20D0
		public PawnRenderNodeProperties Props
		{
			get
			{
				return this.props;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060027F3 RID: 10227 RVA: 0x000D3ED8 File Offset: 0x000D20D8
		public virtual Graphic PrimaryGraphic
		{
			get
			{
				return this.primaryGraphic;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x000D3EE0 File Offset: 0x000D20E0
		public virtual IReadOnlyList<Graphic> Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060027F5 RID: 10229 RVA: 0x000D3EE8 File Offset: 0x000D20E8
		public PawnRenderNodeWorker Worker
		{
			get
			{
				PawnRenderNodeProperties pawnRenderNodeProperties = this.props;
				if (pawnRenderNodeProperties == null)
				{
					return null;
				}
				return pawnRenderNodeProperties.Worker;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000D3EFB File Offset: 0x000D20FB
		public MaterialPropertyBlock MatPropBlock
		{
			get
			{
				return this.matPropBlock;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060027F7 RID: 10231 RVA: 0x000D3F03 File Offset: 0x000D2103
		public bool DebugEnabled
		{
			get
			{
				return this.debugEnabled || !Prefs.DevMode;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000D3F17 File Offset: 0x000D2117
		public Vector3 DebugOffset
		{
			get
			{
				if (!Prefs.DevMode)
				{
					return Vector3.zero;
				}
				return this.debugOffset;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060027F9 RID: 10233 RVA: 0x000D3F2C File Offset: 0x000D212C
		public float DebugAngleOffset
		{
			get
			{
				if (!Prefs.DevMode)
				{
					return 0f;
				}
				return this.debugAngleOffset;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool EnsureInitializationWithoutRecache
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060027FB RID: 10235 RVA: 0x00031607 File Offset: 0x0002F807
		protected virtual Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060027FC RID: 10236 RVA: 0x000D3F41 File Offset: 0x000D2141
		public bool CheckMaterialEveryDrawRequest
		{
			get
			{
				return this.graphics.Count > 1;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060027FD RID: 10237 RVA: 0x000D3F54 File Offset: 0x000D2154
		public virtual bool RecacheRequested
		{
			get
			{
				if (this.requestRecache)
				{
					return true;
				}
				if (this.children != null)
				{
					for (int i = 0; i < this.children.Length; i++)
					{
						if (this.children[i].RecacheRequested)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060027FE RID: 10238 RVA: 0x000D3F98 File Offset: 0x000D2198
		public BaseAnimationWorker AnimationWorker
		{
			get
			{
				if (this.cachedAnimationWorker != null && this.cachedAnimationDef == this.tree.currentAnimation)
				{
					return this.cachedAnimationWorker;
				}
				this.cachedAnimationDef = this.tree.currentAnimation;
				this.cachedAnimationWorkerData = null;
				AnimationPart animationPart;
				if (!this.tree.TryGetAnimationPartForNode(this, out animationPart))
				{
					this.cachedAnimationWorker = null;
				}
				else
				{
					this.cachedAnimationWorker = GenWorker<BaseAnimationWorker>.Get(animationPart.WorkerType);
					this.cachedAnimationPart = animationPart;
				}
				return this.cachedAnimationWorker;
			}
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000D4018 File Offset: 0x000D2218
		public PawnRenderNode(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
		{
			this.props = props;
			this.tree = tree;
			this.meshSet = this.MeshSetFor(pawn);
			try
			{
				this.Init(pawn);
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Exception when initializing node {0} for pawn {1}: {2}", this, pawn, arg));
			}
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000D40A8 File Offset: 0x000D22A8
		private void Init(Pawn pawn)
		{
			if (this.props.tagDef != null)
			{
				pawn.Drawer.renderer.renderTree.SetTagNode(this.props.tagDef, this);
			}
			this.matPropBlock = new MaterialPropertyBlock();
			if (!this.props.children.NullOrEmpty<PawnRenderNodeProperties>())
			{
				this.children = new PawnRenderNode[this.props.children.Count];
				for (int i = 0; i < this.props.children.Count; i++)
				{
					PawnRenderNodeProperties pawnRenderNodeProperties = this.props.children[i];
					this.children[i] = (PawnRenderNode)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
					{
						pawn,
						pawnRenderNodeProperties,
						this.tree
					});
					this.children[i].parent = this;
				}
			}
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000D4188 File Offset: 0x000D2388
		public void AppendRequests(PawnDrawParms parms, List<PawnGraphicDrawRequest> requests)
		{
			if (!this.Props.Worker.CanDrawNow(this, parms))
			{
				return;
			}
			using (List<PawnRenderSubWorker>.Enumerator enumerator = this.Props.SubWorkers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.CanDrawNowSub(this, parms))
					{
						return;
					}
				}
			}
			int count = requests.Count;
			this.Props.Worker.AppendDrawRequests(this, parms, requests);
			bool flag = requests.Count > count;
			if ((!this.Props.useGraphic || flag) && !this.children.NullOrEmpty<PawnRenderNode>())
			{
				for (int i = 0; i < this.children.Length; i++)
				{
					if (this.children[i].Worker.CanDrawNow(this.children[i], parms))
					{
						this.children[i].AppendRequests(parms, requests);
					}
				}
			}
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000D4280 File Offset: 0x000D2480
		public void EnsureInitialized(PawnRenderFlags defaultRenderFlagsNow)
		{
			if (!this.EnsureInitializationWithoutRecache && this.initialized && !this.RecacheRequested)
			{
				return;
			}
			this.props.EnsureInitialized();
			this.graphics.Clear();
			this.primaryGraphic = null;
			foreach (Graphic item in this.GraphicsFor(this.tree.pawn))
			{
				if (this.primaryGraphic == null)
				{
					this.primaryGraphic = item;
				}
				this.graphics.Add(item);
			}
			foreach (ValueTuple<GraphicStateDef, Graphic> valueTuple in this.StateGraphicsFor(this.tree.pawn))
			{
				GraphicStateDef item2 = valueTuple.Item1;
				Graphic item3 = valueTuple.Item2;
				if (this.graphicStateLookup == null)
				{
					this.graphicStateLookup = new Dictionary<GraphicStateDef, Graphic>();
				}
				if (this.primaryGraphic == null)
				{
					this.primaryGraphic = item3;
				}
				Graphic graphic;
				Graphic graphic2;
				if (!this.graphicStateLookup.TryGetValue(item2, out graphic) || !item2.TryGetDefaultGraphic(out graphic2) || graphic == graphic2)
				{
					if (graphic != null)
					{
						this.graphics.Remove(graphic);
					}
					this.graphicStateLookup[item2] = item3;
					if (!this.graphics.Contains(item3))
					{
						this.graphics.Add(item3);
					}
				}
			}
			for (int i = 0; i < this.graphics.Count; i++)
			{
				if (this.graphics[i] != null)
				{
					this.EnsureMaterialVariantsInitialized(this.graphics[i]);
				}
			}
			if (!this.meshesInitialized)
			{
				this.EnsureMeshesInitialized();
			}
			if (!this.children.NullOrEmpty<PawnRenderNode>())
			{
				for (int j = 0; j < this.children.Length; j++)
				{
					if (this.children[j] == null)
					{
						Log.Warning("Tried to ensure initialized null PawnRenderNode.");
					}
					else
					{
						this.children[j].EnsureInitialized(defaultRenderFlagsNow);
					}
				}
			}
			this.initialized = true;
			this.meshesInitialized = true;
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000D4498 File Offset: 0x000D2698
		protected virtual void EnsureMaterialVariantsInitialized(Graphic g)
		{
			this.InitializeInvisibleMaterialVariant(g);
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000D44A4 File Offset: 0x000D26A4
		private void InitializeInvisibleMaterialVariant(Graphic g)
		{
			foreach (Rot4 facing in Rot4.AllRotations)
			{
				Material material = g.NodeGetMat(new PawnDrawParms
				{
					facing = facing,
					pawn = this.tree.pawn
				});
				if (material != null)
				{
					InvisibilityMatPool.GetInvisibleMat(material);
				}
			}
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000D4524 File Offset: 0x000D2724
		protected virtual void EnsureMeshesInitialized()
		{
			if (this.meshSet == null)
			{
				return;
			}
			foreach (Rot4 rot in Rot4.AllRotations)
			{
				Vector2 size = MeshPool.GetMetaData(this.meshSet.MeshAt(rot)).size;
				MeshPool.GridPlane(size);
				MeshPool.GridPlaneFlip(size);
			}
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000D4598 File Offset: 0x000D2798
		public void GetTransform(PawnDrawParms parms, out Vector3 offset, out Vector3 pivot, out Quaternion rotation, out Vector3 scale)
		{
			offset = this.Worker.OffsetFor(this, parms, out pivot);
			rotation = this.Worker.RotationFor(this, parms);
			scale = this.Worker.ScaleFor(this, parms);
			foreach (PawnRenderSubWorker pawnRenderSubWorker in this.Props.SubWorkers)
			{
				pawnRenderSubWorker.TransformOffset(this, parms, ref offset, ref pivot);
				pawnRenderSubWorker.TransformRotation(this, parms, ref rotation);
				pawnRenderSubWorker.TransformScale(this, parms, ref scale);
			}
			scale.y = 1f;
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000D4650 File Offset: 0x000D2850
		public virtual GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			if (this.props.overrideMeshSize != null)
			{
				return MeshPool.GetMeshSetForSize(this.props.overrideMeshSize.Value.x, this.props.overrideMeshSize.Value.y);
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x000D46B0 File Offset: 0x000D28B0
		public virtual Graphic GraphicForState(GraphicStateDef state)
		{
			Graphic graphic = null;
			Dictionary<GraphicStateDef, Graphic> dictionary = this.graphicStateLookup;
			if (dictionary != null)
			{
				dictionary.TryGetValue(state, out graphic);
			}
			if (graphic == null)
			{
				Log.ErrorOnce(string.Concat(new string[]
				{
					"Trying to query graphic state ",
					state.ToStringSafe<GraphicStateDef>(),
					" on node ",
					this.ToStringSafe<PawnRenderNode>(),
					" for pawn ",
					this.tree.pawn.LabelShort,
					", but no valid graphic can be found. Defaulting to primary graphic."
				}), Gen.HashCombine<int>(this.tree.pawn.thingIDNumber, 694623110));
				graphic = this.PrimaryGraphic;
			}
			return graphic;
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000D4750 File Offset: 0x000D2950
		public virtual Graphic GraphicFor(Pawn pawn)
		{
			string text = this.TexPathFor(pawn);
			if (text.NullOrEmpty())
			{
				return null;
			}
			Shader shader = this.ShaderFor(pawn);
			if (shader == null)
			{
				return null;
			}
			return GraphicDatabase.Get<Graphic_Multi>(text, shader, Vector2.one, this.ColorFor(pawn));
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000D4795 File Offset: 0x000D2995
		protected virtual IEnumerable<Graphic> GraphicsFor(Pawn pawn)
		{
			if (this.HasGraphic(this.tree.pawn))
			{
				yield return this.GraphicFor(pawn);
			}
			yield break;
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000D47AC File Offset: 0x000D29AC
		[return: TupleElementNames(new string[]
		{
			"state",
			"graphic"
		})]
		protected virtual IEnumerable<ValueTuple<GraphicStateDef, Graphic>> StateGraphicsFor(Pawn pawn)
		{
			AnimationDef curAnimation = pawn.Drawer.renderer.CurAnimation;
			if (curAnimation == null)
			{
				yield break;
			}
			foreach (ValueTuple<GraphicStateDef, Graphic> valueTuple in curAnimation.GetGraphicStates(this))
			{
				yield return valueTuple;
			}
			IEnumerator<ValueTuple<GraphicStateDef, Graphic>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x000D47C3 File Offset: 0x000D29C3
		public bool HasGraphic(Pawn pawn)
		{
			return this.props.useGraphic && this.props.rotDrawMode.HasFlag(pawn.Drawer.renderer.CurRotDrawMode);
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000D4804 File Offset: 0x000D2A04
		public virtual Color ColorFor(Pawn pawn)
		{
			Color color;
			switch (this.props.colorType)
			{
			case PawnRenderNodeProperties.AttachmentColorType.Hair:
				if (pawn.story == null)
				{
					Log.ErrorOnce("Trying to set render node color to hair for " + pawn.LabelShort + " without pawn story. Defaulting to white.", Gen.HashCombine<int>(pawn.thingIDNumber, 828310001));
					color = Color.white;
					goto IL_101;
				}
				color = pawn.story.HairColor;
				goto IL_101;
			case PawnRenderNodeProperties.AttachmentColorType.Skin:
			{
				Color? statueColor = pawn.Drawer.renderer.StatueColor;
				if (statueColor != null)
				{
					color = statueColor.Value;
					goto IL_101;
				}
				if (pawn.story == null)
				{
					Log.ErrorOnce("Trying to set render node color to skin for " + pawn.LabelShort + " without pawn story. Defaulting to white.", Gen.HashCombine<int>(pawn.thingIDNumber, 228340903));
					color = Color.white;
					goto IL_101;
				}
				color = pawn.story.SkinColor;
				goto IL_101;
			}
			}
			color = (this.props.color ?? Color.white);
			IL_101:
			color *= this.props.colorRGBPostFactor;
			if (this.props.useRottenColor && pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Rotting)
			{
				color = PawnRenderUtility.GetRottenColor(color);
			}
			return color;
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000D494C File Offset: 0x000D2B4C
		public Shader ShaderFor(Pawn pawn)
		{
			ShaderTypeDef shaderTypeDef = this.props.shaderTypeDef;
			if (((shaderTypeDef != null) ? shaderTypeDef.Shader : null) != null)
			{
				return this.props.shaderTypeDef.Shader;
			}
			if (pawn.Drawer.renderer.StatueColor != null)
			{
				return this.DefaultShader;
			}
			if (this.props.useSkinShader)
			{
				Shader skinShader = ShaderUtility.GetSkinShader(pawn);
				if (skinShader != null)
				{
					return skinShader;
				}
			}
			return this.DefaultShader;
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000D49D0 File Offset: 0x000D2BD0
		protected virtual string TexPathFor(Pawn pawn)
		{
			if (this.Props.bodyTypeGraphicPaths != null)
			{
				foreach (BodyTypeGraphicData bodyTypeGraphicData in this.Props.bodyTypeGraphicPaths)
				{
					if (pawn.story.bodyType == bodyTypeGraphicData.bodyType)
					{
						return bodyTypeGraphicData.texturePath;
					}
				}
			}
			if (pawn.gender == Gender.Female)
			{
				if (!this.props.texPathsFemale.NullOrEmpty<string>())
				{
					using (new RandBlock(this.TexSeedFor(pawn)))
					{
						return this.props.texPathsFemale.RandomElement<string>();
					}
				}
				if (!this.props.texPathFemale.NullOrEmpty())
				{
					return this.props.texPathFemale;
				}
			}
			if (!this.props.texPaths.NullOrEmpty<string>())
			{
				using (new RandBlock(this.TexSeedFor(pawn)))
				{
					return this.props.texPaths.RandomElement<string>();
				}
			}
			return this.props.texPath;
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x000D4B1C File Offset: 0x000D2D1C
		protected virtual int TexSeedFor(Pawn pawn)
		{
			int num = this.props.texSeed;
			num += pawn.thingIDNumber;
			if (this.hediff != null)
			{
				num += this.hediff.loadID;
			}
			if (this.trait != null)
			{
				num += (int)this.trait.def.index;
			}
			if (this.gene != null)
			{
				num += this.gene.loadID;
			}
			return num;
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x000D4B88 File Offset: 0x000D2D88
		public virtual Mesh GetMesh(PawnDrawParms parms)
		{
			if (this.meshSet == null)
			{
				return null;
			}
			Mesh mesh = this.meshSet.MeshAt(parms.facing);
			if (this.FlipGraphic(parms))
			{
				mesh = MeshPool.GridPlaneFlip(mesh);
			}
			return mesh;
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x000D4BC4 File Offset: 0x000D2DC4
		public virtual bool FlipGraphic(PawnDrawParms parms)
		{
			bool flag = this.props.flipGraphic;
			if (this.debugFlip)
			{
				flag = !flag;
			}
			BodyPartRecord bodyPartRecord = this.bodyPart;
			if (bodyPartRecord != null && bodyPartRecord.flipGraphic)
			{
				flag = !flag;
			}
			if (this.Props.drawData != null && this.Props.drawData.FlipForRot(parms.facing))
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x000D4C30 File Offset: 0x000D2E30
		public void AddChildren(PawnRenderNode[] newChildren)
		{
			if (this.children == null)
			{
				this.children = newChildren;
				return;
			}
			PawnRenderNode[] array = new PawnRenderNode[this.children.Length + newChildren.Length];
			int i;
			for (i = 0; i < this.children.Length; i++)
			{
				array[i] = this.children[i];
			}
			for (int j = 0; j < newChildren.Length; j++)
			{
				array[i + j] = newChildren[j];
			}
			this.children = array;
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x000D4C9C File Offset: 0x000D2E9C
		public bool TryGetAnimationOffset(PawnDrawParms parms, out Vector3 offset)
		{
			if (this.AnimationWorkerEnabled(parms))
			{
				offset = this.cachedAnimationDef.posRotScale * this.AnimationWorker.OffsetAtTick(this.tree.AnimationTick, this.cachedAnimationDef, this, this.cachedAnimationPart, parms);
				return true;
			}
			offset = Vector3.zero;
			return false;
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x000D4CFC File Offset: 0x000D2EFC
		public bool TryGetAnimationScale(PawnDrawParms parms, out Vector3 offset)
		{
			if (this.AnimationWorkerEnabled(parms))
			{
				offset = this.AnimationWorker.ScaleAtTick(this.tree.AnimationTick, this.cachedAnimationDef, this, this.cachedAnimationPart, parms);
				return true;
			}
			offset = Vector3.one;
			return false;
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000D4D4C File Offset: 0x000D2F4C
		public bool TryGetAnimationRotation(PawnDrawParms parms, out float offset)
		{
			if (this.AnimationWorkerEnabled(parms))
			{
				offset = this.cachedAnimationDef.posRotScale * this.AnimationWorker.AngleAtTick(this.tree.AnimationTick, this.cachedAnimationDef, this, this.cachedAnimationPart, parms);
				return true;
			}
			offset = 0f;
			return false;
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000D4D9E File Offset: 0x000D2F9E
		public bool TryGetAnimationGraphicState(PawnDrawParms parms, out GraphicStateDef state)
		{
			if (this.AnimationWorkerEnabled(parms))
			{
				state = this.AnimationWorker.GraphicStateAtTick(this.tree.AnimationTick, this.cachedAnimationDef, this, this.cachedAnimationPart, parms);
				return true;
			}
			state = null;
			return false;
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000D4DD5 File Offset: 0x000D2FD5
		public bool ContainsGraphicState(GraphicStateDef state)
		{
			return this.graphicStateLookup != null && this.graphicStateLookup.ContainsKey(state);
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000D4DED File Offset: 0x000D2FED
		public void TryAnimationPostDraw(PawnDrawParms parms, Matrix4x4 matrix)
		{
			BaseAnimationWorker animationWorker = this.AnimationWorker;
			if (animationWorker != null && animationWorker.Enabled(this.cachedAnimationDef, this, this.cachedAnimationPart, parms))
			{
				this.AnimationWorker.PostDraw(this.cachedAnimationDef, this, this.cachedAnimationPart, parms, matrix);
			}
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000D4E2B File Offset: 0x000D302B
		public bool AnimationWorkerEnabled(PawnDrawParms parms)
		{
			BaseAnimationWorker animationWorker = this.AnimationWorker;
			return animationWorker != null && animationWorker.Enabled(this.cachedAnimationDef, this, this.cachedAnimationPart, parms);
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000D4E4C File Offset: 0x000D304C
		public T GetAnimationWorkerData<T>() where T : new()
		{
			object obj = this.cachedAnimationWorkerData;
			if (obj is T)
			{
				return (T)((object)obj);
			}
			T t = Activator.CreateInstance<T>();
			this.cachedAnimationWorkerData = t;
			return t;
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000D4E84 File Offset: 0x000D3084
		public override string ToString()
		{
			if (this.props != null)
			{
				return this.props.debugLabel;
			}
			return base.ToString();
		}

		// Token: 0x04002107 RID: 8455
		public PawnRenderTree tree;

		// Token: 0x04002108 RID: 8456
		protected Graphic primaryGraphic;

		// Token: 0x04002109 RID: 8457
		protected GraphicMeshSet meshSet;

		// Token: 0x0400210A RID: 8458
		protected readonly PawnRenderNodeProperties props;

		// Token: 0x0400210B RID: 8459
		protected MaterialPropertyBlock matPropBlock;

		// Token: 0x0400210C RID: 8460
		public PawnRenderNode parent;

		// Token: 0x0400210D RID: 8461
		public PawnRenderNode[] children;

		// Token: 0x0400210E RID: 8462
		public BodyPartRecord bodyPart;

		// Token: 0x0400210F RID: 8463
		public Gene gene;

		// Token: 0x04002110 RID: 8464
		public Hediff hediff;

		// Token: 0x04002111 RID: 8465
		public Trait trait;

		// Token: 0x04002112 RID: 8466
		public Apparel apparel;

		// Token: 0x04002113 RID: 8467
		public bool requestRecache;

		// Token: 0x04002114 RID: 8468
		private bool meshesInitialized;

		// Token: 0x04002115 RID: 8469
		private bool initialized;

		// Token: 0x04002116 RID: 8470
		private readonly List<Graphic> graphics = new List<Graphic>(1);

		// Token: 0x04002117 RID: 8471
		protected Dictionary<GraphicStateDef, Graphic> graphicStateLookup;

		// Token: 0x04002118 RID: 8472
		public bool debugEnabled = true;

		// Token: 0x04002119 RID: 8473
		public Vector3 debugOffset = Vector3.zero;

		// Token: 0x0400211A RID: 8474
		public float debugAngleOffset;

		// Token: 0x0400211B RID: 8475
		public float debugScale = 1f;

		// Token: 0x0400211C RID: 8476
		public float debugLayerOffset;

		// Token: 0x0400211D RID: 8477
		public bool debugFlip;

		// Token: 0x0400211E RID: 8478
		public Vector2 debugPivotOffset = DrawData.PivotCenter;

		// Token: 0x0400211F RID: 8479
		private AnimationDef cachedAnimationDef;

		// Token: 0x04002120 RID: 8480
		private BaseAnimationWorker cachedAnimationWorker;

		// Token: 0x04002121 RID: 8481
		private AnimationPart cachedAnimationPart;

		// Token: 0x04002122 RID: 8482
		public object cachedAnimationWorkerData;

		// Token: 0x04002123 RID: 8483
		private Graphic lastDrawnGraphic;

		// Token: 0x04002124 RID: 8484
		public const int MinLayer = -10;

		// Token: 0x04002125 RID: 8485
		public const int SubIntervalCount = 100;

		// Token: 0x04002126 RID: 8486
		public const float SubInterval = 0.0003658537f;
	}
}
