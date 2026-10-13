using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003075 RID: 12405
	[StaticConstructorOnStartup]
	public static class PortraitsCache
	{
		// Token: 0x060116A5 RID: 71333 RVA: 0x00500FCC File Offset: 0x004FF1CC
		public static RenderTexture Get(Pawn pawn, Vector2 size, Rot4 rotation, Vector3 cameraOffset = default(Vector3), float cameraZoom = 1f, bool supersample = true, bool compensateForUIScale = true, bool renderHeadgear = true, bool renderClothes = true, IReadOnlyDictionary<Apparel, Color> overrideApparelColors = null, Color? overrideHairColor = null, bool stylingStation = false, PawnHealthState? healthStateOverride = null)
		{
			if (supersample)
			{
				size *= 1.25f;
			}
			if (compensateForUIScale)
			{
				size *= Prefs.UIScale;
			}
			PortraitsCache.PortraitParams portraitParams = new PortraitsCache.PortraitParams(size, cameraOffset, cameraZoom, rotation, renderHeadgear, renderClothes, overrideApparelColors, overrideHairColor, stylingStation, healthStateOverride);
			Dictionary<Pawn, PortraitsCache.CachedPortrait> orCreateCachedPortraitsWithParams = PortraitsCache.GetOrCreateCachedPortraitsWithParams(portraitParams);
			PortraitsCache.CachedPortrait cachedPortrait;
			if (orCreateCachedPortraitsWithParams.TryGetValue(pawn, out cachedPortrait))
			{
				if (!cachedPortrait.RenderTexture.IsCreated())
				{
					cachedPortrait.RenderTexture.Create();
					portraitParams.RenderPortrait(pawn, cachedPortrait.RenderTexture);
				}
				else if (cachedPortrait.Dirty)
				{
					portraitParams.RenderPortrait(pawn, cachedPortrait.RenderTexture);
				}
				orCreateCachedPortraitsWithParams.Remove(pawn);
				orCreateCachedPortraitsWithParams.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, false, Time.time));
				return cachedPortrait.RenderTexture;
			}
			RenderTexture renderTexture = PortraitsCache.NewRenderTexture(size);
			portraitParams.RenderPortrait(pawn, renderTexture);
			orCreateCachedPortraitsWithParams.Add(pawn, new PortraitsCache.CachedPortrait(renderTexture, false, Time.time));
			return renderTexture;
		}

		// Token: 0x060116A6 RID: 71334 RVA: 0x005010B8 File Offset: 0x004FF2B8
		public static void SetDirty(Pawn pawn)
		{
			foreach (KeyValuePair<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>> keyValuePair in PortraitsCache.cachedPortraits)
			{
				PortraitsCache.PortraitParams portraitParams;
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary;
				keyValuePair.Deconstruct(out portraitParams, out dictionary);
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary2 = dictionary;
				PortraitsCache.CachedPortrait cachedPortrait;
				if (dictionary2.TryGetValue(pawn, out cachedPortrait) && !cachedPortrait.Dirty)
				{
					dictionary2.Remove(pawn);
					dictionary2.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
			}
		}

		// Token: 0x060116A7 RID: 71335 RVA: 0x00501148 File Offset: 0x004FF348
		public static void PortraitsCacheUpdate()
		{
			PortraitsCache.RemoveExpiredCachedPortraits();
			PortraitsCache.SetAnimatedPortraitsDirty();
		}

		// Token: 0x060116A8 RID: 71336 RVA: 0x00501154 File Offset: 0x004FF354
		public static void Clear()
		{
			foreach (KeyValuePair<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>> keyValuePair in PortraitsCache.cachedPortraits)
			{
				PortraitsCache.PortraitParams portraitParams;
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary;
				keyValuePair.Deconstruct(out portraitParams, out dictionary);
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair2 in dictionary)
				{
					PortraitsCache.DestroyRenderTexture(keyValuePair2.Value.RenderTexture);
				}
			}
			PortraitsCache.cachedPortraits.Clear();
			for (int i = 0; i < PortraitsCache.renderTexturesPool.Count; i++)
			{
				PortraitsCache.DestroyRenderTexture(PortraitsCache.renderTexturesPool[i]);
			}
			PortraitsCache.renderTexturesPool.Clear();
		}

		// Token: 0x060116A9 RID: 71337 RVA: 0x00501238 File Offset: 0x004FF438
		private static Dictionary<Pawn, PortraitsCache.CachedPortrait> GetOrCreateCachedPortraitsWithParams(PortraitsCache.PortraitParams portraitParams)
		{
			Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary;
			if (!PortraitsCache.cachedPortraits.TryGetValue(portraitParams, out dictionary))
			{
				dictionary = new Dictionary<Pawn, PortraitsCache.CachedPortrait>(new PawnEqualityComparer());
				PortraitsCache.cachedPortraits.Add(portraitParams, dictionary);
			}
			return dictionary;
		}

		// Token: 0x060116AA RID: 71338 RVA: 0x0050126C File Offset: 0x004FF46C
		private static void DestroyRenderTexture(RenderTexture rt)
		{
			rt.DiscardContents();
			UnityEngine.Object.Destroy(rt);
		}

		// Token: 0x060116AB RID: 71339 RVA: 0x0050127C File Offset: 0x004FF47C
		private static void RemoveExpiredCachedPortraits()
		{
			foreach (KeyValuePair<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>> keyValuePair in PortraitsCache.cachedPortraits)
			{
				PortraitsCache.PortraitParams portraitParams;
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary;
				keyValuePair.Deconstruct(out portraitParams, out dictionary);
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary2 = dictionary;
				PortraitsCache.toRemove.Clear();
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair2 in dictionary2)
				{
					if (keyValuePair2.Value.Expired)
					{
						PortraitsCache.toRemove.Add(keyValuePair2.Key);
						PortraitsCache.renderTexturesPool.Add(keyValuePair2.Value.RenderTexture);
					}
				}
				for (int i = 0; i < PortraitsCache.toRemove.Count; i++)
				{
					dictionary2.Remove(PortraitsCache.toRemove[i]);
				}
				PortraitsCache.toRemove.Clear();
			}
		}

		// Token: 0x060116AC RID: 71340 RVA: 0x00501390 File Offset: 0x004FF590
		private static void SetAnimatedPortraitsDirty()
		{
			foreach (KeyValuePair<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>> keyValuePair in PortraitsCache.cachedPortraits)
			{
				PortraitsCache.PortraitParams portraitParams;
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary;
				keyValuePair.Deconstruct(out portraitParams, out dictionary);
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary2 = dictionary;
				PortraitsCache.toSetDirty.Clear();
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair2 in dictionary2)
				{
					if (PortraitsCache.IsAnimated(keyValuePair2.Key) && !keyValuePair2.Value.Dirty)
					{
						PortraitsCache.toSetDirty.Add(keyValuePair2.Key);
					}
				}
				for (int i = 0; i < PortraitsCache.toSetDirty.Count; i++)
				{
					PortraitsCache.CachedPortrait cachedPortrait = dictionary2[PortraitsCache.toSetDirty[i]];
					dictionary2.Remove(PortraitsCache.toSetDirty[i]);
					dictionary2.Add(PortraitsCache.toSetDirty[i], new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
				PortraitsCache.toSetDirty.Clear();
			}
		}

		// Token: 0x060116AD RID: 71341 RVA: 0x005014EC File Offset: 0x004FF6EC
		private static RenderTexture NewRenderTexture(Vector2 size)
		{
			int num = PortraitsCache.renderTexturesPool.FindLastIndex((RenderTexture x) => x.width == (int)size.x && x.height == (int)size.y);
			if (num != -1)
			{
				RenderTexture result = PortraitsCache.renderTexturesPool[num];
				PortraitsCache.renderTexturesPool.RemoveAt(num);
				return result;
			}
			return new RenderTexture((int)size.x, (int)size.y, 24)
			{
				name = "Portrait",
				useMipMap = false,
				filterMode = FilterMode.Bilinear
			};
		}

		// Token: 0x060116AE RID: 71342 RVA: 0x00501570 File Offset: 0x004FF770
		private static bool IsAnimated(Pawn pawn)
		{
			return Current.ProgramState == ProgramState.Playing && pawn.Drawer.renderer.flasher.FlashingNowOrRecently;
		}

		// Token: 0x0400B46D RID: 46189
		private static List<RenderTexture> renderTexturesPool = new List<RenderTexture>();

		// Token: 0x0400B46E RID: 46190
		private static Dictionary<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>> cachedPortraits = new Dictionary<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>>(new PortraitsCache.PortraitParamsEqualityComparer());

		// Token: 0x0400B46F RID: 46191
		private const float SupersampleScale = 1.25f;

		// Token: 0x0400B470 RID: 46192
		private static List<Pawn> toRemove = new List<Pawn>();

		// Token: 0x0400B471 RID: 46193
		private static List<Pawn> toSetDirty = new List<Pawn>();

		// Token: 0x02003076 RID: 12406
		private struct CachedPortrait
		{
			// Token: 0x17002D11 RID: 11537
			// (get) Token: 0x060116B0 RID: 71344 RVA: 0x005015C0 File Offset: 0x004FF7C0
			// (set) Token: 0x060116B1 RID: 71345 RVA: 0x005015C8 File Offset: 0x004FF7C8
			public RenderTexture RenderTexture { readonly get; private set; }

			// Token: 0x17002D12 RID: 11538
			// (get) Token: 0x060116B2 RID: 71346 RVA: 0x005015D1 File Offset: 0x004FF7D1
			// (set) Token: 0x060116B3 RID: 71347 RVA: 0x005015D9 File Offset: 0x004FF7D9
			public bool Dirty { readonly get; private set; }

			// Token: 0x17002D13 RID: 11539
			// (get) Token: 0x060116B4 RID: 71348 RVA: 0x005015E2 File Offset: 0x004FF7E2
			// (set) Token: 0x060116B5 RID: 71349 RVA: 0x005015EA File Offset: 0x004FF7EA
			public float LastUseTime { readonly get; private set; }

			// Token: 0x17002D14 RID: 11540
			// (get) Token: 0x060116B6 RID: 71350 RVA: 0x005015F3 File Offset: 0x004FF7F3
			public bool Expired
			{
				get
				{
					return Time.time - this.LastUseTime > 1f;
				}
			}

			// Token: 0x060116B7 RID: 71351 RVA: 0x00501608 File Offset: 0x004FF808
			public CachedPortrait(RenderTexture renderTexture, bool dirty, float lastUseTime)
			{
				this = default(PortraitsCache.CachedPortrait);
				this.RenderTexture = renderTexture;
				this.Dirty = dirty;
				this.LastUseTime = lastUseTime;
			}

			// Token: 0x0400B475 RID: 46197
			private const float CacheDuration = 1f;
		}

		// Token: 0x02003077 RID: 12407
		private class PortraitParamsEqualityComparer : IEqualityComparer<PortraitsCache.PortraitParams>
		{
			// Token: 0x060116B8 RID: 71352 RVA: 0x00501626 File Offset: 0x004FF826
			public bool Equals(PortraitsCache.PortraitParams x, PortraitsCache.PortraitParams y)
			{
				return x.Equals(y);
			}

			// Token: 0x060116B9 RID: 71353 RVA: 0x00501630 File Offset: 0x004FF830
			public int GetHashCode(PortraitsCache.PortraitParams obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x02003078 RID: 12408
		private readonly struct PortraitParams : IEquatable<PortraitsCache.PortraitParams>
		{
			// Token: 0x060116BB RID: 71355 RVA: 0x00501640 File Offset: 0x004FF840
			public PortraitParams(Vector2 size, Vector3 cameraOffset, float cameraZoom, Rot4 rotation, bool renderHeadgear = true, bool renderClothes = true, IReadOnlyDictionary<Apparel, Color> overrideApparelColors = null, Color? overrideHairColor = null, bool stylingStation = false, PawnHealthState? overrideHealthState = null)
			{
				this.size = size;
				this.cameraOffset = cameraOffset;
				this.cameraZoom = cameraZoom;
				this.rotation = rotation;
				this.renderHeadgear = renderHeadgear;
				this.renderClothes = renderClothes;
				this.overrideApparelColors = overrideApparelColors;
				this.overrideHairColor = overrideHairColor;
				this.stylingStation = stylingStation;
				this.overrideHealthState = overrideHealthState;
				this.cachedHashCode = (size.GetHashCode() ^ cameraOffset.GetHashCode() ^ cameraZoom.GetHashCode() ^ rotation.GetHashCode() ^ renderHeadgear.GetHashCode() ^ renderClothes.GetHashCode() ^ stylingStation.GetHashCode() ^ ((overrideHairColor != null) ? overrideHairColor.GetValueOrDefault().GetHashCode() : 0) ^ ((overrideHealthState != null) ? overrideHealthState.GetValueOrDefault().GetHashCode() : 0) ^ GenCollection.DictHashCode<Apparel, Color>(overrideApparelColors));
			}

			// Token: 0x060116BC RID: 71356 RVA: 0x00501738 File Offset: 0x004FF938
			public override bool Equals(object obj)
			{
				if (obj is PortraitsCache.PortraitParams)
				{
					PortraitsCache.PortraitParams other = (PortraitsCache.PortraitParams)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x060116BD RID: 71357 RVA: 0x00501760 File Offset: 0x004FF960
			public bool Equals(PortraitsCache.PortraitParams other)
			{
				if (other.size == this.size && other.cameraOffset == this.cameraOffset && other.cameraZoom == this.cameraZoom && other.rotation == this.rotation && other.renderHeadgear == this.renderHeadgear && other.renderClothes == this.renderClothes && other.stylingStation == this.stylingStation && other.overrideHairColor == this.overrideHairColor)
				{
					PawnHealthState? pawnHealthState = other.overrideHealthState;
					PawnHealthState? pawnHealthState2 = this.overrideHealthState;
					if (pawnHealthState.GetValueOrDefault() == pawnHealthState2.GetValueOrDefault() & pawnHealthState != null == (pawnHealthState2 != null))
					{
						return GenCollection.DictsEqual<Apparel, Color>(other.overrideApparelColors, this.overrideApparelColors);
					}
				}
				return false;
			}

			// Token: 0x060116BE RID: 71358 RVA: 0x00501879 File Offset: 0x004FFA79
			public override int GetHashCode()
			{
				return this.cachedHashCode;
			}

			// Token: 0x060116BF RID: 71359 RVA: 0x00501884 File Offset: 0x004FFA84
			public void RenderPortrait(Pawn pawn, RenderTexture renderTexture)
			{
				float angle = 0f;
				Vector3 positionOffset = default(Vector3);
				if ((this.overrideHealthState ?? pawn.health.State) != PawnHealthState.Mobile && !pawn.ageTracker.CurLifeStage.alwaysDowned)
				{
					angle = 85f;
					positionOffset.x -= 0.18f;
					positionOffset.z -= 0.18f;
				}
				Find.PawnCacheRenderer.RenderPawn(pawn, renderTexture, this.cameraOffset, this.cameraZoom, angle, this.rotation, pawn.health.hediffSet.HasHead, this.renderHeadgear, this.renderClothes, true, positionOffset, this.overrideApparelColors, this.overrideHairColor, this.stylingStation);
			}

			// Token: 0x0400B476 RID: 46198
			public readonly Vector2 size;

			// Token: 0x0400B477 RID: 46199
			public readonly Vector3 cameraOffset;

			// Token: 0x0400B478 RID: 46200
			public readonly float cameraZoom;

			// Token: 0x0400B479 RID: 46201
			public readonly Rot4 rotation;

			// Token: 0x0400B47A RID: 46202
			public readonly bool renderHeadgear;

			// Token: 0x0400B47B RID: 46203
			public readonly bool renderClothes;

			// Token: 0x0400B47C RID: 46204
			public readonly bool stylingStation;

			// Token: 0x0400B47D RID: 46205
			public readonly IReadOnlyDictionary<Apparel, Color> overrideApparelColors;

			// Token: 0x0400B47E RID: 46206
			public readonly Color? overrideHairColor;

			// Token: 0x0400B47F RID: 46207
			public readonly PawnHealthState? overrideHealthState;

			// Token: 0x0400B480 RID: 46208
			private readonly int cachedHashCode;
		}
	}
}
