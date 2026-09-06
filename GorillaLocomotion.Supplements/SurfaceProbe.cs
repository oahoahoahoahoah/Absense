using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000036 RID: 54
public static class SurfaceProbe
{
	// Token: 0x06000197 RID: 407 RVA: 0x0001C254 File Offset: 0x0001A454
	public static void Update()
	{
		try
		{
			GTPlayer player = GTPlayer.Instance;
			bool flag = player == null;
			if (!flag)
			{
				bool sliding = SurfaceProbe.CaptureHand(player.LeftHand, ref SurfaceProbe.leftSurface, ref SurfaceProbe.leftVelMult, ref SurfaceProbe.leftCaptured) || SurfaceProbe.CaptureHand(player.RightHand, ref SurfaceProbe.rightSurface, ref SurfaceProbe.rightVelMult, ref SurfaceProbe.rightCaptured);
				SurfaceProbe.ReleaseHand(ref SurfaceProbe.leftSurface, ref SurfaceProbe.leftVelMult, ref SurfaceProbe.leftCaptured, player.LeftHand);
				SurfaceProbe.ReleaseHand(ref SurfaceProbe.rightSurface, ref SurfaceProbe.rightVelMult, ref SurfaceProbe.rightCaptured, player.RightHand);
				bool flag2 = !Settings.SlipSlapEnabled;
				if (flag2)
				{
					bool flag3 = SurfaceProbe.boosted;
					if (flag3)
					{
						SurfaceProbe.RestoreJump(player);
					}
				}
				else
				{
					bool flag4 = sliding;
					if (flag4)
					{
						bool flag5 = SurfaceProbe.savedMaxJumpSpeed < 0f;
						if (flag5)
						{
							SurfaceProbe.savedMaxJumpSpeed = player.maxJumpSpeed;
							SurfaceProbe.savedJumpMultiplier = player.jumpMultiplier;
						}
						float mult = Mathf.Max(1f, Settings.SlipSlapMultiplier);
						float scaled = 1f + (mult - 1f) * 0.5f;
						player.maxJumpSpeed = SurfaceProbe.savedMaxJumpSpeed * scaled;
						player.jumpMultiplier = SurfaceProbe.savedJumpMultiplier * scaled;
						SurfaceProbe.boosted = true;
					}
					else
					{
						bool flag6 = SurfaceProbe.boosted;
						if (flag6)
						{
							SurfaceProbe.RestoreJump(player);
						}
					}
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0001C3C8 File Offset: 0x0001A5C8
	private static bool CaptureHand(GTPlayer.HandState hand, ref GorillaSurfaceOverride surface, ref float velMult, ref bool captured)
	{
		GorillaSurfaceOverride surfaceOverride = hand.surfaceOverride;
		bool sliding = surfaceOverride != null && surfaceOverride.slidePercentageOverride > 0f && hand.isColliding;
		bool flag = sliding && !captured;
		if (flag)
		{
			surface = surfaceOverride;
			velMult = surfaceOverride.extraVelMultiplier;
			captured = true;
		}
		return sliding;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0001C424 File Offset: 0x0001A624
	private static void ReleaseHand(ref GorillaSurfaceOverride surface, ref float velMult, ref bool captured, GTPlayer.HandState hand)
	{
		GorillaSurfaceOverride surfaceOverride = hand.surfaceOverride;
		bool sliding = surfaceOverride != null && surfaceOverride.slidePercentageOverride > 0f && hand.isColliding;
		bool flag = !sliding & captured;
		if (flag)
		{
			bool flag2 = surface != null;
			if (flag2)
			{
				surface.extraVelMultiplier = velMult;
			}
			surface = null;
			captured = false;
		}
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0001C484 File Offset: 0x0001A684
	private static void RestoreJump(GTPlayer player)
	{
		bool flag = SurfaceProbe.savedMaxJumpSpeed > 0f;
		if (flag)
		{
			player.maxJumpSpeed = SurfaceProbe.savedMaxJumpSpeed;
			player.jumpMultiplier = SurfaceProbe.savedJumpMultiplier;
		}
		SurfaceProbe.boosted = false;
	}

	// Token: 0x0400043A RID: 1082
	private static GorillaSurfaceOverride leftSurface;

	// Token: 0x0400043B RID: 1083
	private static float leftVelMult;

	// Token: 0x0400043C RID: 1084
	private static bool leftCaptured;

	// Token: 0x0400043D RID: 1085
	private static GorillaSurfaceOverride rightSurface;

	// Token: 0x0400043E RID: 1086
	private static float rightVelMult;

	// Token: 0x0400043F RID: 1087
	private static bool rightCaptured;

	// Token: 0x04000440 RID: 1088
	private static float savedMaxJumpSpeed = -1f;

	// Token: 0x04000441 RID: 1089
	private static float savedJumpMultiplier = -1f;

	// Token: 0x04000442 RID: 1090
	private static bool boosted = false;
}
