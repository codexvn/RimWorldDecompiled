using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E28 RID: 3624
	public class SubSoundDef : Editable
	{
		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06005B6C RID: 23404 RVA: 0x001D3FA8 File Offset: 0x001D21A8
		public FloatRange Duration
		{
			get
			{
				float num = float.PositiveInfinity;
				float num2 = float.NegativeInfinity;
				foreach (ResolvedGrain resolvedGrain in this.resolvedGrains)
				{
					num = Mathf.Min(num, resolvedGrain.duration);
					num2 = Mathf.Max(num2, resolvedGrain.duration);
				}
				if (num == float.PositiveInfinity || num2 == float.NegativeInfinity)
				{
					return new FloatRange(0f, 0f);
				}
				return new FloatRange(num / Mathf.Abs(this.pitchRange.min), num2 / Mathf.Abs(this.pitchRange.max));
			}
		}

		// Token: 0x06005B6D RID: 23405 RVA: 0x001D4064 File Offset: 0x001D2264
		public virtual void TryPlay(SoundInfo info)
		{
			if (this.resolvedGrains.Count == 0)
			{
				string[] array = new string[5];
				array[0] = "Cannot play ";
				int num = 1;
				SoundDef soundDef = this.parentDef;
				array[num] = ((soundDef != null) ? soundDef.ToString() : null);
				array[2] = " (subSound ";
				array[3] = ((this != null) ? this.ToString() : null);
				array[4] = "_: No resolved grains.";
				Log.Error(string.Concat(array));
				return;
			}
			if (!Find.SoundRoot.oneShotManager.CanAddPlayingOneShot(this.parentDef, info))
			{
				return;
			}
			if (Current.Game != null && !this.gameSpeedRange.Includes((int)Find.TickManager.CurTimeSpeed))
			{
				return;
			}
			ResolvedGrain resolvedGrain = this.RandomizedResolvedGrain();
			ResolvedGrain_Clip resolvedGrain_Clip = resolvedGrain as ResolvedGrain_Clip;
			if (resolvedGrain_Clip != null)
			{
				if (SampleOneShot.TryMakeAndPlay(this, resolvedGrain_Clip.clip, info) == null)
				{
					return;
				}
				SoundSlotManager.Notify_Played(this.parentDef.slot, resolvedGrain_Clip.clip.length);
			}
			this.Notify_GrainPlayed(resolvedGrain);
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x001D4148 File Offset: 0x001D2348
		public void Notify_GrainPlayed(ResolvedGrain chosenGrain)
		{
			if (this.distinctResolvedGrainsCount > 1)
			{
				if (this.repeatMode == RepeatSelectMode.NeverLastHalf)
				{
					while (this.recentlyPlayedResolvedGrains.Count >= this.numToAvoid)
					{
						this.recentlyPlayedResolvedGrains.Dequeue();
					}
					if (this.recentlyPlayedResolvedGrains.Count < this.numToAvoid)
					{
						this.recentlyPlayedResolvedGrains.Enqueue(chosenGrain);
						return;
					}
				}
				else if (this.repeatMode == RepeatSelectMode.NeverTwice)
				{
					this.lastPlayedResolvedGrain = chosenGrain;
				}
			}
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x001D41B8 File Offset: 0x001D23B8
		public bool IsSameOrHasSameTag(SubSoundDef other)
		{
			return this == other || (!this.tag.NullOrEmpty() && !other.tag.NullOrEmpty() && this.tag == other.tag);
		}

		// Token: 0x06005B70 RID: 23408 RVA: 0x001D41F0 File Offset: 0x001D23F0
		public ResolvedGrain RandomizedResolvedGrain()
		{
			ResolvedGrain chosenGrain = null;
			for (;;)
			{
				chosenGrain = this.resolvedGrains.RandomElement<ResolvedGrain>();
				if (this.distinctResolvedGrainsCount <= 1)
				{
					break;
				}
				if (this.repeatMode == RepeatSelectMode.NeverLastHalf)
				{
					if (!this.recentlyPlayedResolvedGrains.Any((ResolvedGrain g) => g.Equals(chosenGrain)))
					{
						break;
					}
				}
				else if (this.repeatMode != RepeatSelectMode.NeverTwice || !chosenGrain.Equals(this.lastPlayedResolvedGrain))
				{
					break;
				}
			}
			return chosenGrain;
		}

		// Token: 0x06005B71 RID: 23409 RVA: 0x001D426A File Offset: 0x001D246A
		public float RandomizedVolume()
		{
			return this.volumeRange.RandomInRange / 100f;
		}

		// Token: 0x06005B72 RID: 23410 RVA: 0x001D427D File Offset: 0x001D247D
		public override void ResolveReferences()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.resolvedGrains.Clear();
				foreach (AudioGrain audioGrain in this.grains)
				{
					foreach (ResolvedGrain item in audioGrain.GetResolvedGrains())
					{
						this.resolvedGrains.Add(item);
					}
				}
				this.distinctResolvedGrainsCount = this.resolvedGrains.Distinct<ResolvedGrain>().Count<ResolvedGrain>();
				this.numToAvoid = Mathf.FloorToInt((float)this.distinctResolvedGrainsCount / 2f);
				if (this.distinctResolvedGrainsCount >= 6)
				{
					this.numToAvoid++;
				}
				if (this.resolvedGrains.Count == 0)
				{
					Log.Error(string.Format("{0} SubSound {1} has no resolvedGrains.", this.parentDef.defName, this));
				}
			});
		}

		// Token: 0x06005B73 RID: 23411 RVA: 0x001D4290 File Offset: 0x001D2490
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.sustainAttack + this.sustainRelease > this.sustainLoopDurationRange.TrueMin && this.sustainLoopDurationRange != FloatRange.Zero)
			{
				yield return "Attack + release < min loop duration. Sustain samples will cut off.";
			}
			if (this.distRange.min > this.distRange.max)
			{
				yield return "Dist range min/max are reversed.";
			}
			if (this.gameSpeedRange.max == 0)
			{
				yield return "gameSpeedRange should have max value greater than 0";
			}
			if (this.gameSpeedRange.min > this.gameSpeedRange.max)
			{
				yield return "gameSpeedRange min/max are reversed.";
			}
			foreach (SoundParameterMapping soundParameterMapping in this.paramMappings)
			{
				SubSoundDef.<>c__DisplayClass38_0 CS$<>8__locals1 = new SubSoundDef.<>c__DisplayClass38_0();
				if (soundParameterMapping.inParam == null || soundParameterMapping.outParam == null)
				{
					yield return "At least one parameter mapping is missing an in or out parameter.";
					break;
				}
				SubSoundDef.<>c__DisplayClass38_0 CS$<>8__locals2 = CS$<>8__locals1;
				SoundParamTarget outParam = soundParameterMapping.outParam;
				CS$<>8__locals2.neededFilter = ((outParam != null) ? outParam.NeededFilterType : null);
				if (CS$<>8__locals1.neededFilter != null && !this.filters.Any((SoundFilter fil) => fil.GetType() == CS$<>8__locals1.neededFilter))
				{
					string str = "A parameter wants to modify the ";
					Type neededFilter = CS$<>8__locals1.neededFilter;
					yield return str + ((neededFilter != null) ? neededFilter.ToString() : null) + " filter, but this sound doesn't have it.";
				}
			}
			List<SoundParameterMapping>.Enumerator enumerator2 = default(List<SoundParameterMapping>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005B74 RID: 23412 RVA: 0x001D42A0 File Offset: 0x001D24A0
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04004244 RID: 16964
		public const string DefaultName = "UnnamedSubSoundDef";

		// Token: 0x04004245 RID: 16965
		[Description("A name to help you identify the sound.")]
		[DefaultValue("UnnamedSubSoundDef")]
		[MayTranslate]
		public string name = "UnnamedSubSoundDef";

		// Token: 0x04004246 RID: 16966
		[Description("Whether this sound plays on the camera or in the world.\n\nThis must match what the game expects from the sound Def with this name.")]
		[DefaultValue(false)]
		public bool onCamera;

		// Token: 0x04004247 RID: 16967
		[Description("Whether to mute this subSound while the game is paused (either by the pausing in play or by opening a menu)")]
		[DefaultValue(false)]
		public bool muteWhenPaused;

		// Token: 0x04004248 RID: 16968
		[Description("Whether this subSound's tempo should be affected by the current tick rate.")]
		[DefaultValue(false)]
		public bool tempoAffectedByGameSpeed;

		// Token: 0x04004249 RID: 16969
		[Description("The sound grains used for this sample. The game will choose one of these randomly when the sound plays. Sustainers choose one for each sample as it begins.")]
		public List<AudioGrain> grains = new List<AudioGrain>();

		// Token: 0x0400424A RID: 16970
		[EditSliderRange(0f, 100f)]
		[Description("This sound will play at a random volume inside this range.\n\nSustainers will choose a different random volume for each sample.")]
		[DefaultFloatRange(50f, 50f)]
		public FloatRange volumeRange = new FloatRange(50f, 50f);

		// Token: 0x0400424B RID: 16971
		[EditSliderRange(0.05f, 2f)]
		[Description("This sound will play at a random pitch inside this range.\n\nSustainers will choose a different random pitch for each sample.")]
		[DefaultFloatRange(1f, 1f)]
		public FloatRange pitchRange = FloatRange.One;

		// Token: 0x0400424C RID: 16972
		[EditSliderRange(0f, 200f)]
		[Description("This sound will play max volume when it is under minDistance from the camera.\n\nIt will fade out linearly until the camera distance reaches its max.")]
		[DefaultFloatRange(25f, 70f)]
		public FloatRange distRange = new FloatRange(25f, 70f);

		// Token: 0x0400424D RID: 16973
		[Description("When the sound chooses the next grain, you may use this setting to have it avoid repeating the last grain, or avoid repeating any of the grains in the last X played, X being half the total number of grains defined.")]
		[DefaultValue(RepeatSelectMode.NeverLastHalf)]
		public RepeatSelectMode repeatMode = RepeatSelectMode.NeverLastHalf;

		// Token: 0x0400424E RID: 16974
		[Description("Mappings between game parameters (like fire size or wind speed) and properties of the sound.")]
		[DefaultEmptyList(typeof(SoundParameterMapping))]
		public List<SoundParameterMapping> paramMappings = new List<SoundParameterMapping>();

		// Token: 0x0400424F RID: 16975
		[Description("The filters to be applied to this sound.")]
		[DefaultEmptyList(typeof(SoundFilter))]
		public List<SoundFilter> filters = new List<SoundFilter>();

		// Token: 0x04004250 RID: 16976
		[Description("A range of possible times between when this sound is triggered and when it will actually start playing.")]
		[DefaultFloatRange(0f, 0f)]
		public FloatRange startDelayRange = FloatRange.Zero;

		// Token: 0x04004251 RID: 16977
		[Description("A range of game speeds this sound can be played on.")]
		public IntRange gameSpeedRange = new IntRange(0, 999);

		// Token: 0x04004252 RID: 16978
		[Description("One shots sharing the same tag are treated as the same sound when determining importance.")]
		[NoTranslate]
		public string tag;

		// Token: 0x04004253 RID: 16979
		[Description("If true, each sample in the sustainer will be looped and ended only after sustainerLoopDurationRange. If not, the sounds will just play once and end after their own length.")]
		[DefaultValue(true)]
		public bool sustainLoop = true;

		// Token: 0x04004254 RID: 16980
		[EditSliderRange(0f, 10f)]
		[Description("The range of durations that individual looped samples in the sustainer will have. Each sample ends after a time randomly chosen in this range.\n\nOnly used if the sustainer is looped.")]
		[DefaultFloatRange(9999f, 9999f)]
		public FloatRange sustainLoopDurationRange = new FloatRange(9999f, 9999f);

		// Token: 0x04004255 RID: 16981
		[EditSliderRange(-2f, 2f)]
		[Description("The time between when one sample ends and the next starts.\n\nSet to negative if you wish samples to overlap.")]
		[LoadAlias("sustainInterval")]
		[DefaultFloatRange(0f, 0f)]
		public FloatRange sustainIntervalRange = FloatRange.Zero;

		// Token: 0x04004256 RID: 16982
		[Description("The time between when one sample ends and the next starts.\n\nSet to negative if you wish samples to overlap.")]
		[LoadAlias("sustainIntervalFactorByAggregateSize")]
		public SimpleCurve sustainIntervalFactorByAggregateSize;

		// Token: 0x04004257 RID: 16983
		[EditSliderRange(0f, 2f)]
		[Description("The fade-in time of each sample. The sample will start at 0 volume and fade in over this number of seconds.")]
		[DefaultValue(0f)]
		public float sustainAttack;

		// Token: 0x04004258 RID: 16984
		[Description("Skip the attack on the first sustainer sample.")]
		[DefaultValue(true)]
		public bool sustainSkipFirstAttack = true;

		// Token: 0x04004259 RID: 16985
		[EditSliderRange(0f, 2f)]
		[Description("The fade-out time of each sample. At this number of seconds before the sample ends, it will start fading out. Its volume will be zero at the moment it finishes fading out.")]
		[DefaultValue(0f)]
		public float sustainRelease;

		// Token: 0x0400425A RID: 16986
		[Description("Whether to start the sample at a random point in the sound.")]
		[DefaultValue(false)]
		public bool randomStartPoint;

		// Token: 0x0400425B RID: 16987
		[DefaultValue(true)]
		public bool canVacuumDampen = true;

		// Token: 0x0400425C RID: 16988
		[Unsaved(false)]
		public SoundDef parentDef;

		// Token: 0x0400425D RID: 16989
		[Unsaved(false)]
		private List<ResolvedGrain> resolvedGrains = new List<ResolvedGrain>();

		// Token: 0x0400425E RID: 16990
		[Unsaved(false)]
		private ResolvedGrain lastPlayedResolvedGrain;

		// Token: 0x0400425F RID: 16991
		[Unsaved(false)]
		private int numToAvoid;

		// Token: 0x04004260 RID: 16992
		[Unsaved(false)]
		private int distinctResolvedGrainsCount;

		// Token: 0x04004261 RID: 16993
		[Unsaved(false)]
		private Queue<ResolvedGrain> recentlyPlayedResolvedGrains = new Queue<ResolvedGrain>();
	}
}
