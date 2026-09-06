using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200003A RID: 58
public static class TagState
{
	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060001B5 RID: 437 RVA: 0x0001DFC0 File Offset: 0x0001C1C0
	public static bool IsVelmaxActive
	{
		get
		{
			bool flag = !TagState.frozen;
			return !flag || Settings.VelmaxEnabled;
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0001DFEC File Offset: 0x0001C1EC
	public static void Update()
	{
		bool flag = !TagState.hasScanned;
		if (flag)
		{
			bool flag2 = TagState.surfaces == null;
			if (flag2)
			{
				bool flag3 = !Settings.VelmaxEnabled;
				if (!flag3)
				{
					bool flag4 = Time.time - TagState.lastScanTime > 30f;
					if (flag4)
					{
						TagState.surfaces = Object.FindObjectsOfType<GorillaSurfaceOverride>();
						TagState.lastScanTime = Time.time;
						TagState.hasScanned = false;
					}
				}
			}
			TagState.ApplyVelmax();
		}
		else
		{
			bool flag5 = Time.time - TagState.lastScanTime > 30f;
			if (flag5)
			{
				TagState.surfaces = Object.FindObjectsOfType<GorillaSurfaceOverride>();
				TagState.lastScanTime = Time.time;
				TagState.hasScanned = false;
				TagState.ApplyVelmax();
			}
			else
			{
				TagState.ApplyVelmax();
			}
		}
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0001E0A4 File Offset: 0x0001C2A4
	private static void ApplyVelmax()
	{
		bool flag = !Settings.VelmaxEnabled;
		if (flag)
		{
			bool flag2 = TagState.frozen;
			if (flag2)
			{
				TagState.RestoreVelmax();
			}
		}
		else
		{
			GTPlayer val = GTPlayer.Instance;
			bool flag3 = val == null;
			if (!flag3)
			{
				bool flag4 = TagState.savedMaxJumpSpeed < 0f;
				if (flag4)
				{
					TagState.savedMaxJumpSpeed = val.maxJumpSpeed;
				}
				TagState.frozen = true;
			}
		}
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0001E110 File Offset: 0x0001C310
	private static void RestoreVelmax()
	{
		GTPlayer val2 = GTPlayer.Instance;
		bool flag = val2 != null;
		if (flag)
		{
			bool flag2 = TagState.savedMaxJumpSpeed > 0f;
			if (flag2)
			{
			}
			bool flag3 = TagState.surfaces != null;
			if (flag3)
			{
				GorillaSurfaceOverride[] array = TagState.surfaces;
				int num2 = 0;
				for (;;)
				{
					bool flag4 = num2 >= array.Length;
					if (flag4)
					{
						break;
					}
					GorillaSurfaceOverride val3 = array[num2];
					bool flag5 = val3 != null;
					if (flag5)
					{
					}
					num2++;
				}
			}
		}
		else
		{
			bool flag6 = TagState.surfaces != null;
			if (flag6)
			{
				GorillaSurfaceOverride[] array = TagState.surfaces;
				int num2 = 0;
				for (;;)
				{
					bool flag7 = num2 >= array.Length;
					if (flag7)
					{
						break;
					}
					GorillaSurfaceOverride val3 = array[num2];
					bool flag8 = val3 != null;
					if (flag8)
					{
					}
					num2++;
				}
			}
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0001E1E8 File Offset: 0x0001C3E8
	public static void Reset()
	{
		TagState.hasScanned = true;
		TagState.surfaces = null;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0001E1F8 File Offset: 0x0001C3F8
	public static void Freeze()
	{
		try
		{
			GTPlayer instance = GTPlayer.Instance;
			bool flag = !(instance == null);
			if (flag)
			{
				instance.disableMovement = true;
				TagState.boosted = true;
			}
		}
		catch
		{
		}
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0001E244 File Offset: 0x0001C444
	public static void Unfreeze()
	{
		try
		{
			bool flag = TagState.boosted;
			if (flag)
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = instance != null;
				if (flag2)
				{
					instance.disableMovement = false;
				}
				TagState.boosted = false;
			}
		}
		catch
		{
		}
	}

	// Token: 0x0400047D RID: 1149
	private static bool frozen = false;

	// Token: 0x0400047E RID: 1150
	private static GorillaSurfaceOverride[] surfaces = null;

	// Token: 0x0400047F RID: 1151
	private static float lastScanTime = -999f;

	// Token: 0x04000480 RID: 1152
	private static bool hasScanned = true;

	// Token: 0x04000481 RID: 1153
	private static float savedMaxJumpSpeed = -1f;

	// Token: 0x04000482 RID: 1154
	private static float savedJumpMultiplier = -1f;

	// Token: 0x04000483 RID: 1155
	private static bool boosted = false;
}
