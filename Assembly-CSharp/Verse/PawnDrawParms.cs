using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005F0 RID: 1520
	public struct PawnDrawParms
	{
		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060026E7 RID: 9959 RVA: 0x000CE56E File Offset: 0x000CC76E
		public bool DrawNow
		{
			get
			{
				return this.flags.FlagSet(PawnRenderFlags.DrawNow);
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060026E8 RID: 9960 RVA: 0x000CE57C File Offset: 0x000CC77C
		public bool Portrait
		{
			get
			{
				return this.flags.FlagSet(PawnRenderFlags.Portrait);
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x000CE58A File Offset: 0x000CC78A
		public bool Cache
		{
			get
			{
				return this.flags.FlagSet(PawnRenderFlags.Cache);
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060026EA RID: 9962 RVA: 0x000CE599 File Offset: 0x000CC799
		public bool Statue
		{
			get
			{
				return this.flags.FlagSet(PawnRenderFlags.Statue);
			}
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000CE5AC File Offset: 0x000CC7AC
		public static PawnDrawParms DefaultFor(Pawn pawn)
		{
			PawnDrawParms @default = PawnDrawParms.Default;
			@default.pawn = pawn;
			@default.facing = pawn.Rotation;
			@default.dead = pawn.Dead;
			@default.crawling = pawn.Crawling;
			@default.swimming = pawn.Swimming;
			return @default;
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x000CE5FC File Offset: 0x000CC7FC
		public bool ShouldRecache(PawnDrawParms other)
		{
			return this.pawn == null || this.facing != other.facing || this.rotDrawMode != other.rotDrawMode || this.posture != other.posture || this.flags != other.flags || this.bed != other.bed || this.skipFlags != other.skipFlags || this.flipHead != other.flipHead || this.coveredInFoam != other.coveredInFoam || this.carriedThing != other.carriedThing || this.dead != other.dead || this.crawling != other.crawling || this.swimming != other.swimming || this.statueColor != other.statueColor;
		}

		// Token: 0x04002043 RID: 8259
		public Pawn pawn;

		// Token: 0x04002044 RID: 8260
		public Matrix4x4 matrix;

		// Token: 0x04002045 RID: 8261
		public Rot4 facing;

		// Token: 0x04002046 RID: 8262
		public RotDrawMode rotDrawMode;

		// Token: 0x04002047 RID: 8263
		public PawnPosture posture;

		// Token: 0x04002048 RID: 8264
		public PawnRenderFlags flags;

		// Token: 0x04002049 RID: 8265
		public Color tint;

		// Token: 0x0400204A RID: 8266
		public Building_Bed bed;

		// Token: 0x0400204B RID: 8267
		public ulong skipFlags;

		// Token: 0x0400204C RID: 8268
		public bool flipHead;

		// Token: 0x0400204D RID: 8269
		public bool coveredInFoam;

		// Token: 0x0400204E RID: 8270
		public bool dead;

		// Token: 0x0400204F RID: 8271
		public bool crawling;

		// Token: 0x04002050 RID: 8272
		public bool swimming;

		// Token: 0x04002051 RID: 8273
		public Thing carriedThing;

		// Token: 0x04002052 RID: 8274
		public Color? statueColor;

		// Token: 0x04002053 RID: 8275
		public static readonly PawnDrawParms Default = new PawnDrawParms
		{
			facing = Rot4.South,
			rotDrawMode = RotDrawMode.Fresh,
			posture = PawnPosture.Standing,
			flags = (PawnRenderFlags.Headgear | PawnRenderFlags.Clothes),
			tint = Color.white
		};
	}
}
