using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public class PawnRenderNode
{
	[CompilerGenerated]
	private sealed class _003CGraphicsFor_003Ed__69 : IEnumerable<Graphic>, IEnumerable, IEnumerator<Graphic>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Graphic _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PawnRenderNode _003C_003E4__this;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		Graphic IEnumerator<Graphic>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGraphicsFor_003Ed__69(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			PawnRenderNode pawnRenderNode = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (pawnRenderNode.HasGraphic(pawnRenderNode.tree.pawn))
				{
					_003C_003E2__current = pawnRenderNode.GraphicFor(pawn);
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Graphic> IEnumerable<Graphic>.GetEnumerator()
		{
			_003CGraphicsFor_003Ed__69 _003CGraphicsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGraphicsFor_003Ed__ = this;
			}
			else
			{
				_003CGraphicsFor_003Ed__ = new _003CGraphicsFor_003Ed__69(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGraphicsFor_003Ed__.pawn = _003C_003E3__pawn;
			return _003CGraphicsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Graphic>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CStateGraphicsFor_003Ed__70 : IEnumerable<(GraphicStateDef state, Graphic graphic)>, IEnumerable, IEnumerator<(GraphicStateDef state, Graphic graphic)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (GraphicStateDef state, Graphic graphic) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		public PawnRenderNode _003C_003E4__this;

		private IEnumerator<(GraphicStateDef state, Graphic graphic)> _003C_003E7__wrap1;

		(GraphicStateDef state, Graphic graphic) IEnumerator<(GraphicStateDef, Graphic)>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CStateGraphicsFor_003Ed__70(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PawnRenderNode node = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					AnimationDef curAnimation = pawn.Drawer.renderer.CurAnimation;
					if (curAnimation == null)
					{
						return false;
					}
					_003C_003E7__wrap1 = curAnimation.GetGraphicStates(node).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					(GraphicStateDef, Graphic) current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(GraphicStateDef state, Graphic graphic)> IEnumerable<(GraphicStateDef, Graphic)>.GetEnumerator()
		{
			_003CStateGraphicsFor_003Ed__70 _003CStateGraphicsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStateGraphicsFor_003Ed__ = this;
			}
			else
			{
				_003CStateGraphicsFor_003Ed__ = new _003CStateGraphicsFor_003Ed__70(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CStateGraphicsFor_003Ed__.pawn = _003C_003E3__pawn;
			return _003CStateGraphicsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(GraphicStateDef, Graphic)>)this).GetEnumerator();
		}
	}

	public PawnRenderTree tree;

	protected Graphic primaryGraphic;

	protected GraphicMeshSet meshSet;

	protected readonly PawnRenderNodeProperties props;

	protected MaterialPropertyBlock matPropBlock;

	public PawnRenderNode parent;

	public PawnRenderNode[] children;

	public BodyPartRecord bodyPart;

	public Gene gene;

	public Hediff hediff;

	public Trait trait;

	public Apparel apparel;

	public bool requestRecache;

	private bool meshesInitialized;

	private bool initialized;

	private readonly List<Graphic> graphics = new List<Graphic>(1);

	protected Dictionary<GraphicStateDef, Graphic> graphicStateLookup;

	public bool debugEnabled = true;

	public Vector3 debugOffset = Vector3.zero;

	public float debugAngleOffset;

	public float debugScale = 1f;

	public float debugLayerOffset;

	public bool debugFlip;

	public Vector2 debugPivotOffset = DrawData.PivotCenter;

	private AnimationDef cachedAnimationDef;

	private BaseAnimationWorker cachedAnimationWorker;

	private AnimationPart cachedAnimationPart;

	public object cachedAnimationWorkerData;

	private Graphic lastDrawnGraphic;

	public const int MinLayer = -10;

	public const int SubIntervalCount = 100;

	public const float SubInterval = 0.0003658537f;

	public PawnRenderNodeProperties Props => props;

	public virtual Graphic PrimaryGraphic => primaryGraphic;

	public virtual IReadOnlyList<Graphic> Graphics => graphics;

	public PawnRenderNodeWorker Worker => props?.Worker;

	public MaterialPropertyBlock MatPropBlock => matPropBlock;

	public bool DebugEnabled
	{
		get
		{
			if (!debugEnabled)
			{
				return !Prefs.DevMode;
			}
			return true;
		}
	}

	public Vector3 DebugOffset
	{
		get
		{
			if (!Prefs.DevMode)
			{
				return Vector3.zero;
			}
			return debugOffset;
		}
	}

	public float DebugAngleOffset
	{
		get
		{
			if (!Prefs.DevMode)
			{
				return 0f;
			}
			return debugAngleOffset;
		}
	}

	protected virtual bool EnsureInitializationWithoutRecache => false;

	protected virtual Shader DefaultShader => ShaderDatabase.Cutout;

	public bool CheckMaterialEveryDrawRequest => graphics.Count > 1;

	public virtual bool RecacheRequested
	{
		get
		{
			if (requestRecache)
			{
				return true;
			}
			if (children != null)
			{
				for (int i = 0; i < children.Length; i++)
				{
					if (children[i].RecacheRequested)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public BaseAnimationWorker AnimationWorker
	{
		get
		{
			if (cachedAnimationWorker != null && cachedAnimationDef == tree.currentAnimation)
			{
				return cachedAnimationWorker;
			}
			cachedAnimationDef = tree.currentAnimation;
			cachedAnimationWorkerData = null;
			if (!tree.TryGetAnimationPartForNode(this, out var animationPart))
			{
				cachedAnimationWorker = null;
			}
			else
			{
				cachedAnimationWorker = GenWorker<BaseAnimationWorker>.Get(animationPart.WorkerType);
				cachedAnimationPart = animationPart;
			}
			return cachedAnimationWorker;
		}
	}

	public PawnRenderNode(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
	{
		this.props = props;
		this.tree = tree;
		meshSet = MeshSetFor(pawn);
		try
		{
			Init(pawn);
		}
		catch (Exception value)
		{
			Log.Error($"Exception when initializing node {this} for pawn {pawn}: {value}");
		}
	}

	private void Init(Pawn pawn)
	{
		if (props.tagDef != null)
		{
			pawn.Drawer.renderer.renderTree.SetTagNode(props.tagDef, this);
		}
		matPropBlock = new MaterialPropertyBlock();
		if (!props.children.NullOrEmpty())
		{
			children = new PawnRenderNode[props.children.Count];
			for (int i = 0; i < props.children.Count; i++)
			{
				PawnRenderNodeProperties pawnRenderNodeProperties = props.children[i];
				children[i] = (PawnRenderNode)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, pawn, pawnRenderNodeProperties, tree);
				children[i].parent = this;
			}
		}
	}

	public void AppendRequests(PawnDrawParms parms, List<PawnGraphicDrawRequest> requests)
	{
		if (!Props.Worker.CanDrawNow(this, parms))
		{
			return;
		}
		foreach (PawnRenderSubWorker subWorker in Props.SubWorkers)
		{
			if (!subWorker.CanDrawNowSub(this, parms))
			{
				return;
			}
		}
		int count = requests.Count;
		Props.Worker.AppendDrawRequests(this, parms, requests);
		bool flag = requests.Count > count;
		if (!(!Props.useGraphic || flag) || children.NullOrEmpty())
		{
			return;
		}
		for (int i = 0; i < children.Length; i++)
		{
			if (children[i].Worker.CanDrawNow(children[i], parms))
			{
				children[i].AppendRequests(parms, requests);
			}
		}
	}

	public void EnsureInitialized(PawnRenderFlags defaultRenderFlagsNow)
	{
		if (!EnsureInitializationWithoutRecache && initialized && !RecacheRequested)
		{
			return;
		}
		props.EnsureInitialized();
		graphics.Clear();
		primaryGraphic = null;
		foreach (Graphic item in GraphicsFor(tree.pawn))
		{
			if (primaryGraphic == null)
			{
				primaryGraphic = item;
			}
			graphics.Add(item);
		}
		foreach (var (graphicStateDef, graphic) in StateGraphicsFor(tree.pawn))
		{
			if (graphicStateLookup == null)
			{
				graphicStateLookup = new Dictionary<GraphicStateDef, Graphic>();
			}
			if (primaryGraphic == null)
			{
				primaryGraphic = graphic;
			}
			if (!graphicStateLookup.TryGetValue(graphicStateDef, out var value) || !graphicStateDef.TryGetDefaultGraphic(out var graphic2) || value == graphic2)
			{
				if (value != null)
				{
					graphics.Remove(value);
				}
				graphicStateLookup[graphicStateDef] = graphic;
				if (!graphics.Contains(graphic))
				{
					graphics.Add(graphic);
				}
			}
		}
		for (int i = 0; i < graphics.Count; i++)
		{
			if (graphics[i] != null)
			{
				EnsureMaterialVariantsInitialized(graphics[i]);
			}
		}
		if (!meshesInitialized)
		{
			EnsureMeshesInitialized();
		}
		if (!children.NullOrEmpty())
		{
			for (int j = 0; j < children.Length; j++)
			{
				if (children[j] == null)
				{
					Log.Warning("Tried to ensure initialized null PawnRenderNode.");
				}
				else
				{
					children[j].EnsureInitialized(defaultRenderFlagsNow);
				}
			}
		}
		initialized = true;
		meshesInitialized = true;
	}

	protected virtual void EnsureMaterialVariantsInitialized(Graphic g)
	{
		InitializeInvisibleMaterialVariant(g);
	}

	private void InitializeInvisibleMaterialVariant(Graphic g)
	{
		foreach (Rot4 allRotation in Rot4.AllRotations)
		{
			Material material = g.NodeGetMat(new PawnDrawParms
			{
				facing = allRotation,
				pawn = tree.pawn
			});
			if (material != null)
			{
				InvisibilityMatPool.GetInvisibleMat(material);
			}
		}
	}

	protected virtual void EnsureMeshesInitialized()
	{
		if (meshSet == null)
		{
			return;
		}
		foreach (Rot4 allRotation in Rot4.AllRotations)
		{
			Vector2 size = MeshPool.GetMetaData(meshSet.MeshAt(allRotation)).size;
			MeshPool.GridPlane(size);
			MeshPool.GridPlaneFlip(size);
		}
	}

	public void GetTransform(PawnDrawParms parms, out Vector3 offset, out Vector3 pivot, out Quaternion rotation, out Vector3 scale)
	{
		offset = Worker.OffsetFor(this, parms, out pivot);
		rotation = Worker.RotationFor(this, parms);
		scale = Worker.ScaleFor(this, parms);
		foreach (PawnRenderSubWorker subWorker in Props.SubWorkers)
		{
			subWorker.TransformOffset(this, parms, ref offset, ref pivot);
			subWorker.TransformRotation(this, parms, ref rotation);
			subWorker.TransformScale(this, parms, ref scale);
		}
		scale.y = 1f;
	}

	public virtual GraphicMeshSet MeshSetFor(Pawn pawn)
	{
		if (props.overrideMeshSize.HasValue)
		{
			return MeshPool.GetMeshSetForSize(props.overrideMeshSize.Value.x, props.overrideMeshSize.Value.y);
		}
		return HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(pawn);
	}

	public virtual Graphic GraphicForState(GraphicStateDef state)
	{
		Graphic value = null;
		graphicStateLookup?.TryGetValue(state, out value);
		if (value == null)
		{
			Log.ErrorOnce($"Trying to query graphic state {state.ToStringSafe()} on node {this.ToStringSafe()} for pawn {tree.pawn.LabelShort}, but no valid graphic can be found. Defaulting to primary graphic.", Gen.HashCombine(tree.pawn.thingIDNumber, 694623110));
			value = PrimaryGraphic;
		}
		return value;
	}

	public virtual Graphic GraphicFor(Pawn pawn)
	{
		string text = TexPathFor(pawn);
		if (text.NullOrEmpty())
		{
			return null;
		}
		Shader shader = ShaderFor(pawn);
		if (shader == null)
		{
			return null;
		}
		return GraphicDatabase.Get<Graphic_Multi>(text, shader, Vector2.one, ColorFor(pawn));
	}

	[IteratorStateMachine(typeof(_003CGraphicsFor_003Ed__69))]
	protected virtual IEnumerable<Graphic> GraphicsFor(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGraphicsFor_003Ed__69(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn
		};
	}

	[IteratorStateMachine(typeof(_003CStateGraphicsFor_003Ed__70))]
	protected virtual IEnumerable<(GraphicStateDef state, Graphic graphic)> StateGraphicsFor(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStateGraphicsFor_003Ed__70(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn
		};
	}

	public bool HasGraphic(Pawn pawn)
	{
		if (!props.useGraphic)
		{
			return false;
		}
		if (!props.rotDrawMode.HasFlag(pawn.Drawer.renderer.CurRotDrawMode))
		{
			return false;
		}
		return true;
	}

	public virtual Color ColorFor(Pawn pawn)
	{
		Color color;
		switch (props.colorType)
		{
		case PawnRenderNodeProperties.AttachmentColorType.Hair:
			if (pawn.story == null)
			{
				Log.ErrorOnce("Trying to set render node color to hair for " + pawn.LabelShort + " without pawn story. Defaulting to white.", Gen.HashCombine(pawn.thingIDNumber, 828310001));
				color = Color.white;
			}
			else
			{
				color = pawn.story.HairColor;
			}
			break;
		case PawnRenderNodeProperties.AttachmentColorType.Skin:
		{
			Color? statueColor = pawn.Drawer.renderer.StatueColor;
			if (statueColor.HasValue)
			{
				color = statueColor.Value;
			}
			else if (pawn.story == null)
			{
				Log.ErrorOnce("Trying to set render node color to skin for " + pawn.LabelShort + " without pawn story. Defaulting to white.", Gen.HashCombine(pawn.thingIDNumber, 228340903));
				color = Color.white;
			}
			else
			{
				color = pawn.story.SkinColor;
			}
			break;
		}
		default:
			color = props.color ?? Color.white;
			break;
		}
		color *= props.colorRGBPostFactor;
		if (props.useRottenColor && pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Rotting)
		{
			color = PawnRenderUtility.GetRottenColor(color);
		}
		return color;
	}

	public Shader ShaderFor(Pawn pawn)
	{
		if (props.shaderTypeDef?.Shader != null)
		{
			return props.shaderTypeDef.Shader;
		}
		if (pawn.Drawer.renderer.StatueColor.HasValue)
		{
			return DefaultShader;
		}
		if (props.useSkinShader)
		{
			Shader skinShader = ShaderUtility.GetSkinShader(pawn);
			if (skinShader != null)
			{
				return skinShader;
			}
		}
		return DefaultShader;
	}

	protected virtual string TexPathFor(Pawn pawn)
	{
		if (Props.bodyTypeGraphicPaths != null)
		{
			foreach (BodyTypeGraphicData bodyTypeGraphicPath in Props.bodyTypeGraphicPaths)
			{
				if (pawn.story.bodyType == bodyTypeGraphicPath.bodyType)
				{
					return bodyTypeGraphicPath.texturePath;
				}
			}
		}
		if (pawn.gender == Gender.Female)
		{
			if (!props.texPathsFemale.NullOrEmpty())
			{
				using (new RandBlock(TexSeedFor(pawn)))
				{
					return props.texPathsFemale.RandomElement();
				}
			}
			if (!props.texPathFemale.NullOrEmpty())
			{
				return props.texPathFemale;
			}
		}
		if (!props.texPaths.NullOrEmpty())
		{
			using (new RandBlock(TexSeedFor(pawn)))
			{
				return props.texPaths.RandomElement();
			}
		}
		return props.texPath;
	}

	protected virtual int TexSeedFor(Pawn pawn)
	{
		int texSeed = props.texSeed;
		texSeed += pawn.thingIDNumber;
		if (hediff != null)
		{
			texSeed += hediff.loadID;
		}
		if (trait != null)
		{
			texSeed += trait.def.index;
		}
		if (gene != null)
		{
			texSeed += gene.loadID;
		}
		return texSeed;
	}

	public virtual Mesh GetMesh(PawnDrawParms parms)
	{
		if (meshSet == null)
		{
			return null;
		}
		Mesh mesh = meshSet.MeshAt(parms.facing);
		if (FlipGraphic(parms))
		{
			mesh = MeshPool.GridPlaneFlip(mesh);
		}
		return mesh;
	}

	public virtual bool FlipGraphic(PawnDrawParms parms)
	{
		bool flag = props.flipGraphic;
		if (debugFlip)
		{
			flag = !flag;
		}
		BodyPartRecord bodyPartRecord = bodyPart;
		if (bodyPartRecord != null && bodyPartRecord.flipGraphic)
		{
			flag = !flag;
		}
		if (Props.drawData != null && Props.drawData.FlipForRot(parms.facing))
		{
			flag = !flag;
		}
		return flag;
	}

	public void AddChildren(PawnRenderNode[] newChildren)
	{
		if (children == null)
		{
			children = newChildren;
			return;
		}
		PawnRenderNode[] array = new PawnRenderNode[children.Length + newChildren.Length];
		int i;
		for (i = 0; i < children.Length; i++)
		{
			array[i] = children[i];
		}
		for (int j = 0; j < newChildren.Length; j++)
		{
			array[i + j] = newChildren[j];
		}
		children = array;
	}

	public bool TryGetAnimationOffset(PawnDrawParms parms, out Vector3 offset)
	{
		if (AnimationWorkerEnabled(parms))
		{
			offset = cachedAnimationDef.posRotScale * AnimationWorker.OffsetAtTick(tree.AnimationTick, cachedAnimationDef, this, cachedAnimationPart, parms);
			return true;
		}
		offset = Vector3.zero;
		return false;
	}

	public bool TryGetAnimationScale(PawnDrawParms parms, out Vector3 offset)
	{
		if (AnimationWorkerEnabled(parms))
		{
			offset = AnimationWorker.ScaleAtTick(tree.AnimationTick, cachedAnimationDef, this, cachedAnimationPart, parms);
			return true;
		}
		offset = Vector3.one;
		return false;
	}

	public bool TryGetAnimationRotation(PawnDrawParms parms, out float offset)
	{
		if (AnimationWorkerEnabled(parms))
		{
			offset = cachedAnimationDef.posRotScale * AnimationWorker.AngleAtTick(tree.AnimationTick, cachedAnimationDef, this, cachedAnimationPart, parms);
			return true;
		}
		offset = 0f;
		return false;
	}

	public bool TryGetAnimationGraphicState(PawnDrawParms parms, out GraphicStateDef state)
	{
		if (AnimationWorkerEnabled(parms))
		{
			state = AnimationWorker.GraphicStateAtTick(tree.AnimationTick, cachedAnimationDef, this, cachedAnimationPart, parms);
			return true;
		}
		state = null;
		return false;
	}

	public bool ContainsGraphicState(GraphicStateDef state)
	{
		if (graphicStateLookup != null)
		{
			return graphicStateLookup.ContainsKey(state);
		}
		return false;
	}

	public void TryAnimationPostDraw(PawnDrawParms parms, Matrix4x4 matrix)
	{
		BaseAnimationWorker animationWorker = AnimationWorker;
		if (animationWorker != null && animationWorker.Enabled(cachedAnimationDef, this, cachedAnimationPart, parms))
		{
			AnimationWorker.PostDraw(cachedAnimationDef, this, cachedAnimationPart, parms, matrix);
		}
	}

	public bool AnimationWorkerEnabled(PawnDrawParms parms)
	{
		return AnimationWorker?.Enabled(cachedAnimationDef, this, cachedAnimationPart, parms) ?? false;
	}

	public T GetAnimationWorkerData<T>() where T : new()
	{
		object obj = cachedAnimationWorkerData;
		if (obj is T)
		{
			return (T)obj;
		}
		T val = new T();
		cachedAnimationWorkerData = val;
		return val;
	}

	public override string ToString()
	{
		if (props != null)
		{
			return props.debugLabel;
		}
		return base.ToString();
	}
}
