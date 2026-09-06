using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000031 RID: 49
public static class SessionStats
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000160 RID: 352 RVA: 0x000193D0 File Offset: 0x000175D0
	// (set) Token: 0x06000161 RID: 353 RVA: 0x000193E7 File Offset: 0x000175E7
	public static float DistanceTraveled { get; private set; }

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000162 RID: 354 RVA: 0x000193F0 File Offset: 0x000175F0
	// (set) Token: 0x06000163 RID: 355 RVA: 0x00019407 File Offset: 0x00017607
	public static float TopSpeed { get; private set; }

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000164 RID: 356 RVA: 0x00019410 File Offset: 0x00017610
	// (set) Token: 0x06000165 RID: 357 RVA: 0x00019427 File Offset: 0x00017627
	public static int TagsGiven { get; private set; }

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000166 RID: 358 RVA: 0x00019430 File Offset: 0x00017630
	// (set) Token: 0x06000167 RID: 359 RVA: 0x00019447 File Offset: 0x00017647
	public static int TimesInfected { get; private set; }

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000168 RID: 360 RVA: 0x00019450 File Offset: 0x00017650
	// (set) Token: 0x06000169 RID: 361 RVA: 0x00019467 File Offset: 0x00017667
	public static float ElapsedTime { get; private set; }

	// Token: 0x0600016A RID: 362 RVA: 0x00019470 File Offset: 0x00017670
	public static void Update()
	{
		bool flag2 = GorillaTagger.Instance == null || GorillaTagger.Instance.offlineVRRig == null;
		if (!flag2)
		{
			SessionStats.ElapsedTime += Time.deltaTime;
			Vector3 position = GorillaTagger.Instance.bodyCollider.transform.position;
			bool flag3 = !SessionStats.hasLastPosition;
			if (flag3)
			{
				SessionStats.lastPosition = position;
				SessionStats.hasLastPosition = true;
			}
			else
			{
				float num = Vector3.Distance(position, SessionStats.lastPosition);
				bool flag4 = num < 10f;
				if (flag4)
				{
					SessionStats.DistanceTraveled += num;
					float num2 = num / Time.deltaTime;
					bool flag5 = num2 > SessionStats.TopSpeed;
					if (flag5)
					{
						SessionStats.TopSpeed = num2;
					}
				}
				SessionStats.lastPosition = position;
				bool flag = SessionStats.IsInfected(GorillaTagger.Instance.offlineVRRig);
				bool flag6 = flag && !SessionStats.wasInfected;
				if (flag6)
				{
					SessionStats.TimesInfected++;
				}
				SessionStats.wasInfected = flag;
			}
		}
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00019578 File Offset: 0x00017778
	public static void AddTag()
	{
		SessionStats.TagsGiven++;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00019588 File Offset: 0x00017788
	private static bool IsInfected(VRRig rig)
	{
		bool flag = !(rig.mainSkin != null);
		bool flag2;
		if (flag)
		{
			flag2 = false;
		}
		else
		{
			bool flag3 = !(rig.mainSkin.material != null);
			flag2 = !flag3 && rig.mainSkin.material.name.ToLower().Contains("fected");
		}
		return flag2;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x000195F0 File Offset: 0x000177F0
	public static void Reset()
	{
		SessionStats.DistanceTraveled = 0f;
		SessionStats.TopSpeed = 0f;
		SessionStats.TagsGiven = 0;
		SessionStats.TimesInfected = 0;
		SessionStats.ElapsedTime = 0f;
		SessionStats.hasLastPosition = false;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00019628 File Offset: 0x00017828
	public static string FormatElapsed()
	{
		int num = (int)SessionStats.ElapsedTime;
		int num2 = num / 3600;
		int num3 = num % 3600;
		int num4 = num3 / 60;
		int num5 = num % 60;
		return string.Format("{0:D2}:{1:D2}:{2:D2}", num2, num4, num5);
	}

	// Token: 0x04000293 RID: 659
	[CompilerGenerated]
	private static float distanceField;

	// Token: 0x04000294 RID: 660
	[CompilerGenerated]
	private static float topSpeedField;

	// Token: 0x04000295 RID: 661
	[CompilerGenerated]
	private static int tagsField;

	// Token: 0x04000296 RID: 662
	[CompilerGenerated]
	private static int infectedField;

	// Token: 0x04000297 RID: 663
	[CompilerGenerated]
	private static float elapsedField;

	// Token: 0x04000298 RID: 664
	private static Vector3 lastPosition;

	// Token: 0x04000299 RID: 665
	private static bool hasLastPosition;

	// Token: 0x0400029A RID: 666
	private static bool wasInfected;
}
