using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B2F RID: 6959
	public class WorldObjectDef : Def
	{
		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x06009B64 RID: 39780 RVA: 0x002D17AC File Offset: 0x002CF9AC
		public Material Material
		{
			get
			{
				if (this.texture.NullOrEmpty())
				{
					return null;
				}
				if (this.material == null)
				{
					string texPath = this.texture;
					ShaderTypeDef shaderTypeDef = this.shader;
					this.material = MaterialPool.MatFrom(texPath, ((shaderTypeDef != null) ? shaderTypeDef.Shader : null) ?? ShaderDatabase.WorldOverlayTransparentLit, 3550);
				}
				return this.material;
			}
		}

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x06009B65 RID: 39781 RVA: 0x002D180D File Offset: 0x002CFA0D
		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (this.expandingIconTexture.NullOrEmpty())
					{
						return null;
					}
					this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
				}
				return this.expandingIconTextureInt;
			}
		}

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x06009B66 RID: 39782 RVA: 0x002D1844 File Offset: 0x002CFA44
		public Texture2D ExpandingIconTextureMask
		{
			get
			{
				if (this.expandingIconTextureMaskInt == null)
				{
					if (this.expandingIconMaskTexture.NullOrEmpty())
					{
						return null;
					}
					this.expandingIconTextureMaskInt = ContentFinder<Texture2D>.Get(this.expandingIconMaskTexture, true);
				}
				return this.expandingIconTextureMaskInt;
			}
		}

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x06009B67 RID: 39783 RVA: 0x002D187B File Offset: 0x002CFA7B
		public Material MapEdgeMaterial
		{
			get
			{
				if (this.mapEdgeTexture.NullOrEmpty())
				{
					return null;
				}
				if (this.material == null)
				{
					this.material = MaterialPool.MatFrom(this.mapEdgeTexture, ShaderDatabase.MapEdgeTerrain);
				}
				return this.material;
			}
		}

		// Token: 0x06009B68 RID: 39784 RVA: 0x002D18B8 File Offset: 0x002CFAB8
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.inspectorTabs != null)
			{
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					if (this.inspectorTabsResolved == null)
					{
						this.inspectorTabsResolved = new List<InspectTabBase>();
					}
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate inspector tab of type ";
						Type type = this.inspectorTabs[i];
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x06009B69 RID: 39785 RVA: 0x002D196C File Offset: 0x002CFB6C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		// Token: 0x06009B6A RID: 39786 RVA: 0x002D19A7 File Offset: 0x002CFBA7
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (string text2 in this.comps[i].ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
				num = i;
			}
			if (this.expandMore && !this.expandingIcon)
			{
				yield return "has expandMore but doesn't have any expanding icon";
			}
			yield break;
			yield break;
		}

		// Token: 0x04006BBF RID: 27583
		public Type worldObjectClass = typeof(WorldObject);

		// Token: 0x04006BC0 RID: 27584
		public bool canHaveFaction = true;

		// Token: 0x04006BC1 RID: 27585
		public bool saved = true;

		// Token: 0x04006BC2 RID: 27586
		public bool canBePlayerHome;

		// Token: 0x04006BC3 RID: 27587
		public bool treatAsPlayerHome;

		// Token: 0x04006BC4 RID: 27588
		public bool canResizeToGravship;

		// Token: 0x04006BC5 RID: 27589
		public bool requiresSignalJammerToReach;

		// Token: 0x04006BC6 RID: 27590
		public IntVec3? overrideMapSize;

		// Token: 0x04006BC7 RID: 27591
		public List<WorldObjectCompProperties> comps = new List<WorldObjectCompProperties>();

		// Token: 0x04006BC8 RID: 27592
		public bool canHaveMap = true;

		// Token: 0x04006BC9 RID: 27593
		public bool validLaunchTarget = true;

		// Token: 0x04006BCA RID: 27594
		public bool allowCaravanIncidentsWhichGenerateMap;

		// Token: 0x04006BCB RID: 27595
		public bool isTempIncidentMapOwner;

		// Token: 0x04006BCC RID: 27596
		public List<IncidentTargetTagDef> IncidentTargetTags;

		// Token: 0x04006BCD RID: 27597
		public bool selectable = true;

		// Token: 0x04006BCE RID: 27598
		public bool neverMultiSelect;

		// Token: 0x04006BCF RID: 27599
		public MapGeneratorDef mapGenerator;

		// Token: 0x04006BD0 RID: 27600
		public List<Type> inspectorTabs;

		// Token: 0x04006BD1 RID: 27601
		[Unsaved(false)]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x04006BD2 RID: 27602
		public bool useDynamicDrawer;

		// Token: 0x04006BD3 RID: 27603
		public bool expandingIcon;

		// Token: 0x04006BD4 RID: 27604
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x04006BD5 RID: 27605
		[NoTranslate]
		public string expandingIconMaskTexture;

		// Token: 0x04006BD6 RID: 27606
		public float expandingIconPriority;

		// Token: 0x04006BD7 RID: 27607
		[NoTranslate]
		public string texture;

		// Token: 0x04006BD8 RID: 27608
		[NoTranslate]
		public string mapEdgeTexture;

		// Token: 0x04006BD9 RID: 27609
		[Unsaved(false)]
		private Material material;

		// Token: 0x04006BDA RID: 27610
		[Unsaved(false)]
		private Material expandingMaterial;

		// Token: 0x04006BDB RID: 27611
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		// Token: 0x04006BDC RID: 27612
		[Unsaved(false)]
		private Texture2D expandingIconTextureMaskInt;

		// Token: 0x04006BDD RID: 27613
		public bool expandMore;

		// Token: 0x04006BDE RID: 27614
		public bool rotateGraphicWhenTraveling;

		// Token: 0x04006BDF RID: 27615
		public Color? expandingIconColor;

		// Token: 0x04006BE0 RID: 27616
		public float expandingIconDrawSize = 1f;

		// Token: 0x04006BE1 RID: 27617
		public bool fullyExpandedInSpace;

		// Token: 0x04006BE2 RID: 27618
		public ShaderTypeDef shader;

		// Token: 0x04006BE3 RID: 27619
		public ShaderTypeDef expandingShader;

		// Token: 0x04006BE4 RID: 27620
		public bool blockExitGridUntilBattleIsWon;

		// Token: 0x04006BE5 RID: 27621
		public RulePackDef nameMaker;
	}
}
