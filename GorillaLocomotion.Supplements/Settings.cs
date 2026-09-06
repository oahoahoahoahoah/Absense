using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
public static class Settings
{
	// Token: 0x0600016F RID: 367 RVA: 0x0001967C File Offset: 0x0001787C
	public static bool IsPressed(ActivationButton button)
	{
		bool flag = button == ActivationButton.Always;
		bool flag2;
		if (flag)
		{
			flag2 = true;
		}
		else
		{
			try
			{
				ControllerInputPoller instance = ControllerInputPoller.instance;
				bool flag3 = instance != null;
				if (flag3)
				{
					switch (button)
					{
					case ActivationButton.LeftGrip:
						return instance.leftGrab;
					case ActivationButton.RightGrip:
						return instance.rightGrab;
					case ActivationButton.BothGrip:
						return instance.leftGrab && instance.rightGrab;
					case ActivationButton.LeftTrigger:
						return instance.leftControllerIndexFloat > 0.7f;
					case ActivationButton.RightTrigger:
						return instance.rightControllerIndexFloat > 0.7f;
					case ActivationButton.BothTrigger:
						return instance.leftControllerIndexFloat > 0.7f && instance.rightControllerIndexFloat > 0.7f;
					case ActivationButton.LeftPrimary:
						return instance.leftControllerPrimaryButton;
					case ActivationButton.RightPrimary:
						return instance.rightControllerPrimaryButton;
					case ActivationButton.BothPrimary:
						return instance.leftControllerPrimaryButton && instance.rightControllerPrimaryButton;
					case ActivationButton.LeftSecondary:
						return instance.leftControllerSecondaryButton;
					case ActivationButton.RightSecondary:
						return instance.rightControllerSecondaryButton;
					case ActivationButton.BothSecondary:
						return instance.leftControllerSecondaryButton && instance.rightControllerSecondaryButton;
					case ActivationButton.LeftJoystickClick:
						return VrInput.LeftJoystickClick();
					case ActivationButton.RightJoystickClick:
						return VrInput.RightJoystickClick();
					case ActivationButton.BothJoystickClick:
						return VrInput.LeftJoystickClick() && VrInput.RightJoystickClick();
					}
				}
			}
			catch
			{
			}
			switch (button)
			{
			case ActivationButton.LeftGrip:
				flag2 = VrInput.LeftGrip();
				break;
			case ActivationButton.RightGrip:
				flag2 = VrInput.RightGrip();
				break;
			case ActivationButton.BothGrip:
			{
				bool flag4 = VrInput.LeftGrip();
				flag2 = flag4 && VrInput.RightGrip();
				break;
			}
			case ActivationButton.LeftTrigger:
				flag2 = VrInput.LeftTrigger();
				break;
			case ActivationButton.RightTrigger:
				flag2 = VrInput.RightTrigger();
				break;
			case ActivationButton.BothTrigger:
			{
				bool flag5 = VrInput.LeftTrigger();
				flag2 = flag5 && VrInput.RightTrigger();
				break;
			}
			case ActivationButton.LeftPrimary:
				flag2 = VrInput.LeftPrimary();
				break;
			case ActivationButton.RightPrimary:
				flag2 = VrInput.RightPrimary();
				break;
			case ActivationButton.BothPrimary:
			{
				bool flag6 = VrInput.LeftPrimary();
				flag2 = flag6 && VrInput.RightPrimary();
				break;
			}
			case ActivationButton.LeftSecondary:
				flag2 = VrInput.LeftSecondary();
				break;
			case ActivationButton.RightSecondary:
				flag2 = VrInput.RightSecondary();
				break;
			case ActivationButton.BothSecondary:
			{
				bool flag7 = VrInput.LeftSecondary();
				flag2 = flag7 && VrInput.RightSecondary();
				break;
			}
			case ActivationButton.LeftJoystickClick:
				flag2 = VrInput.LeftJoystickClick();
				break;
			case ActivationButton.RightJoystickClick:
				flag2 = VrInput.RightJoystickClick();
				break;
			case ActivationButton.BothJoystickClick:
			{
				bool flag8 = VrInput.LeftJoystickClick();
				flag2 = flag8 && VrInput.RightJoystickClick();
				break;
			}
			default:
				flag2 = false;
				break;
			}
		}
		return flag2;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0001995C File Offset: 0x00017B5C
	public static bool IsPressed(Keybind key)
	{
		bool flag = key == Keybind.Always;
		bool flag2;
		if (flag)
		{
			flag2 = true;
		}
		else
		{
			try
			{
				ControllerInputPoller instance = ControllerInputPoller.instance;
				bool flag3 = instance != null;
				if (flag3)
				{
					switch (key)
					{
					case Keybind.RightTrigger:
						return instance.rightControllerIndexFloat > 0.7f;
					case Keybind.LeftTrigger:
						return instance.leftControllerIndexFloat > 0.7f;
					case Keybind.RightGrip:
						return instance.rightGrab;
					case Keybind.LeftGrip:
						return instance.leftGrab;
					case Keybind.RightSecondary:
						return instance.rightControllerSecondaryButton;
					case Keybind.LeftSecondary:
						return instance.leftControllerSecondaryButton;
					case Keybind.RightPrimary:
						return instance.rightControllerPrimaryButton;
					case Keybind.LeftPrimary:
						return instance.leftControllerPrimaryButton;
					case Keybind.RightJoystickClick:
						return VrInput.RightJoystickClick();
					case Keybind.LeftJoystickClick:
						return VrInput.LeftJoystickClick();
					case Keybind.BothTrigger:
						return instance.leftControllerIndexFloat > 0.7f && instance.rightControllerIndexFloat > 0.7f;
					case Keybind.BothGrip:
						return instance.leftGrab && instance.rightGrab;
					case Keybind.BothSecondary:
						return instance.leftControllerSecondaryButton && instance.rightControllerSecondaryButton;
					case Keybind.BothPrimary:
						return instance.leftControllerPrimaryButton && instance.rightControllerPrimaryButton;
					case Keybind.BothJoystickClick:
						return VrInput.LeftJoystickClick() && VrInput.RightJoystickClick();
					}
				}
			}
			catch
			{
			}
			switch (key)
			{
			case Keybind.RightTrigger:
				flag2 = VrInput.RightTrigger();
				break;
			case Keybind.LeftTrigger:
				flag2 = VrInput.LeftTrigger();
				break;
			case Keybind.RightGrip:
				flag2 = VrInput.RightGrip();
				break;
			case Keybind.LeftGrip:
				flag2 = VrInput.LeftGrip();
				break;
			case Keybind.RightSecondary:
				flag2 = VrInput.RightSecondary();
				break;
			case Keybind.LeftSecondary:
				flag2 = VrInput.LeftSecondary();
				break;
			case Keybind.RightPrimary:
				flag2 = VrInput.RightPrimary();
				break;
			case Keybind.LeftPrimary:
				flag2 = VrInput.LeftPrimary();
				break;
			case Keybind.RightJoystickClick:
				flag2 = VrInput.RightJoystickClick();
				break;
			case Keybind.LeftJoystickClick:
				flag2 = VrInput.LeftJoystickClick();
				break;
			case Keybind.BothTrigger:
			{
				bool flag4 = VrInput.LeftTrigger();
				flag2 = flag4 && VrInput.RightTrigger();
				break;
			}
			case Keybind.BothGrip:
			{
				bool flag5 = VrInput.LeftGrip();
				flag2 = flag5 && VrInput.RightGrip();
				break;
			}
			case Keybind.BothSecondary:
			{
				bool flag6 = VrInput.LeftSecondary();
				flag2 = flag6 && VrInput.RightSecondary();
				break;
			}
			case Keybind.BothPrimary:
			{
				bool flag7 = VrInput.LeftPrimary();
				flag2 = flag7 && VrInput.RightPrimary();
				break;
			}
			case Keybind.BothJoystickClick:
			{
				bool flag8 = VrInput.LeftJoystickClick();
				flag2 = flag8 && VrInput.RightJoystickClick();
				break;
			}
			default:
				flag2 = false;
				break;
			}
		}
		return flag2;
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00019C44 File Offset: 0x00017E44
	public static string GetKeybindLabel(Keybind key)
	{
		string text;
		switch (key)
		{
		case Keybind.RightTrigger:
			text = "R Trigger";
			break;
		case Keybind.LeftTrigger:
			text = "L Trigger";
			break;
		case Keybind.RightGrip:
			text = "R Grip";
			break;
		case Keybind.LeftGrip:
			text = "L Grip";
			break;
		case Keybind.RightSecondary:
			text = "R Secondary";
			break;
		case Keybind.LeftSecondary:
			text = "L Secondary";
			break;
		case Keybind.RightPrimary:
			text = "R Primary";
			break;
		case Keybind.LeftPrimary:
			text = "L Primary";
			break;
		case Keybind.RightJoystickClick:
			text = "R Joystick";
			break;
		case Keybind.LeftJoystickClick:
			text = "L Joystick";
			break;
		case Keybind.BothTrigger:
			text = "Both Trigger";
			break;
		case Keybind.BothGrip:
			text = "Both Grip";
			break;
		case Keybind.BothSecondary:
			text = "Both Secondary";
			break;
		case Keybind.BothPrimary:
			text = "Both Primary";
			break;
		case Keybind.BothJoystickClick:
			text = "Both Joystick";
			break;
		case Keybind.Always:
			text = "Always";
			break;
		default:
			text = "Unknown";
			break;
		}
		return text;
	}

	// Token: 0x0400029B RID: 667
	public static MenuColorTheme CurrentTheme = MenuColorTheme.Starry;

	// Token: 0x0400029C RID: 668
	public static bool RoundedCorners = true;

	// Token: 0x0400029D RID: 669
	public static bool PullModEnabled = true;

	// Token: 0x0400029E RID: 670
	public static float PullStrength = 0.7f;

	// Token: 0x0400029F RID: 671
	public static float PullThreshold = 0.7f;

	// Token: 0x040002A0 RID: 672
	public static float PullTpTime = 0.011f;

	// Token: 0x040002A1 RID: 673
	public static ActivationButton PullActivation = ActivationButton.LeftJoystickClick;

	// Token: 0x040002A2 RID: 674
	public static bool PullModToggleMode = false;

	// Token: 0x040002A3 RID: 675
	public static bool PullModToggleActive = false;

	// Token: 0x040002A4 RID: 676
	public static bool PullAntiCheatBypass = false;

	// Token: 0x040002A5 RID: 677
	public static bool PullObstacleDetection = false;

	// Token: 0x040002A6 RID: 678
	public static float PullObstacleRadius = 0.5f;

	// Token: 0x040002A7 RID: 679
	public static bool WallPullEnabled = false;

	// Token: 0x040002A8 RID: 680
	public static bool TagFreezeTpEnabled = false;

	// Token: 0x040002A9 RID: 681
	public static float TagFreezeTpMultiplier = 2f;

	// Token: 0x040002AA RID: 682
	public static bool TestPullModEnabled = false;

	// Token: 0x040002AB RID: 683
	public static float TestPullStrength = 0.7f;

	// Token: 0x040002AC RID: 684
	public static float TestPullThreshold = 0.7f;

	// Token: 0x040002AD RID: 685
	public static float TestPullTpTime = 0.011f;

	// Token: 0x040002AE RID: 686
	public static ActivationButton TestPullActivation = ActivationButton.LeftJoystickClick;

	// Token: 0x040002AF RID: 687
	public static bool TestPullModToggleMode = false;

	// Token: 0x040002B0 RID: 688
	public static bool TestPullModToggleActive = false;

	// Token: 0x040002B1 RID: 689
	public static bool TestPullRaycastEnabled = false;

	// Token: 0x040002B2 RID: 690
	public static float TestPullSmoothing = 0.5f;

	// Token: 0x040002B3 RID: 691
	public static bool TestPull2Enabled = false;

	// Token: 0x040002B4 RID: 692
	public static float TestPull2Strength = 0.7f;

	// Token: 0x040002B5 RID: 693
	public static float TestPull2Smoothing = 0.3f;

	// Token: 0x040002B6 RID: 694
	public static ActivationButton TestPull2Activation = ActivationButton.LeftJoystickClick;

	// Token: 0x040002B7 RID: 695
	public static bool TestPull2ToggleMode = false;

	// Token: 0x040002B8 RID: 696
	public static bool TestPull2ToggleActive = false;

	// Token: 0x040002B9 RID: 697
	public static bool TestPull2RaycastEnabled = true;

	// Token: 0x040002BA RID: 698
	public static bool TestPull2GroundedOnly = true;

	// Token: 0x040002BB RID: 699
	public static int PullModMode = 0;

	// Token: 0x040002BC RID: 700
	public static float PullRandomiseMin = 0f;

	// Token: 0x040002BD RID: 701
	public static float PullRandomiseMax = 0f;

	// Token: 0x040002BE RID: 702
	public static bool WallPullModEnabled = false;

	// Token: 0x040002BF RID: 703
	public static float WallPullModStrength = 0.7f;

	// Token: 0x040002C0 RID: 704
	public static float WallPullModTpTime = 0.011f;

	// Token: 0x040002C1 RID: 705
	public static float WallPullModSmoothing = 0.3f;

	// Token: 0x040002C2 RID: 706
	public static float WallPullModVelThreshold = 0.5f;

	// Token: 0x040002C3 RID: 707
	public static ActivationButton WallPullModActivation = ActivationButton.RightGrip;

	// Token: 0x040002C4 RID: 708
	public static bool WallPullModToggleMode = false;

	// Token: 0x040002C5 RID: 709
	public static bool WallPullModToggleActive = false;

	// Token: 0x040002C6 RID: 710
	public static bool MainPullEnabled = false;

	// Token: 0x040002C7 RID: 711
	public static float MainPullStrength = 0.7f;

	// Token: 0x040002C8 RID: 712
	public static float MainPullTpTime = 0.011f;

	// Token: 0x040002C9 RID: 713
	public static float MainPullSmoothing = 0.3f;

	// Token: 0x040002CA RID: 714
	public static ActivationButton MainPullActivation = ActivationButton.LeftJoystickClick;

	// Token: 0x040002CB RID: 715
	public static bool MainPullToggleMode = false;

	// Token: 0x040002CC RID: 716
	public static bool MainPullToggleActive = false;

	// Token: 0x040002CD RID: 717
	public static bool MainPullRaycastEnabled = false;

	// Token: 0x040002CE RID: 718
	public static bool MainPullVelThresholdEnabled = false;

	// Token: 0x040002CF RID: 719
	public static float MainPullVelThreshold = 0.5f;

	// Token: 0x040002D0 RID: 720
	public static bool MainPullThresholdEnabled = false;

	// Token: 0x040002D1 RID: 721
	public static float MainPullThresholdTime = 50f;

	// Token: 0x040002D2 RID: 722
	public static bool MainPullPredictEnabled = false;

	// Token: 0x040002D3 RID: 723
	public static int MainPullPredictPoints = 4;

	// Token: 0x040002D4 RID: 724
	public static float MainPullPredictCheckStart = 0.5f;

	// Token: 0x040002D5 RID: 725
	public static float MainPullPredictCheckEnd = 1f;

	// Token: 0x040002D6 RID: 726
	public static int MainPullPredictStopBack = 1;

	// Token: 0x040002D7 RID: 727
	public static bool MainPullPredictFromHand = false;

	// Token: 0x040002D8 RID: 728
	public static bool MainPullSurfaceAlignEnabled = false;

	// Token: 0x040002D9 RID: 729
	public static float MainPullSurfaceAlignRadius = 0.6f;

	// Token: 0x040002DA RID: 730
	public static bool MainPullWallPullEnabled = false;

	// Token: 0x040002DB RID: 731
	public static Keybind MainPullWallPullKeybind = Keybind.RightGrip;

	// Token: 0x040002DC RID: 732
	public static float MainPullWallPullStrength = 0.7f;

	// Token: 0x040002DD RID: 733
	public static float MainPullWallPullTpTime = 0.011f;

	// Token: 0x040002DE RID: 734
	public static float MainPullWallPullSmoothing = 0.3f;

	// Token: 0x040002DF RID: 735
	public static bool OneFramePullEnabled = false;

	// Token: 0x040002E0 RID: 736
	public static float OneFramePullStrength = 1f;

	// Token: 0x040002E1 RID: 737
	public static float OneFramePullThreshold = 0.5f;

	// Token: 0x040002E2 RID: 738
	public static ActivationButton OneFramePullActivation = ActivationButton.LeftJoystickClick;

	// Token: 0x040002E3 RID: 739
	public static bool OneFramePullToggleMode = false;

	// Token: 0x040002E4 RID: 740
	public static bool OneFramePullToggleActive = false;

	// Token: 0x040002E5 RID: 741
	public static bool RealPullEnabled = false;

	// Token: 0x040002E6 RID: 742
	public static float RealPullDistance = 0.9f;

	// Token: 0x040002E7 RID: 743
	public static float RealPullThreshold = 0.5f;

	// Token: 0x040002E8 RID: 744
	public static ActivationButton RealPullActivation = ActivationButton.LeftJoystickClick;

	// Token: 0x040002E9 RID: 745
	public static bool RealPullToggleMode = false;

	// Token: 0x040002EA RID: 746
	public static bool RealPullToggleActive = false;

	// Token: 0x040002EB RID: 747
	public static float RealPullDownAmount = 0.2f;

	// Token: 0x040002EC RID: 748
	public static bool RealPullSlopeFix = false;

	// Token: 0x040002ED RID: 749
	public static bool PullV3Enabled = false;

	// Token: 0x040002EE RID: 750
	public static float PullV3Strength = 1f;

	// Token: 0x040002EF RID: 751
	public static float PullV3Threshold = 0.5f;

	// Token: 0x040002F0 RID: 752
	public static float PullV3TpTime = 0.013f;

	// Token: 0x040002F1 RID: 753
	public static ActivationButton PullV3Activation = ActivationButton.LeftJoystickClick;

	// Token: 0x040002F2 RID: 754
	public static bool PullV3ToggleMode = false;

	// Token: 0x040002F3 RID: 755
	public static bool PullV3ToggleActive = false;

	// Token: 0x040002F4 RID: 756
	public static bool PullV3LeftHand = true;

	// Token: 0x040002F5 RID: 757
	public static bool PullV3RightHand = true;

	// Token: 0x040002F6 RID: 758
	public static bool PullV3MidPullRequired = true;

	// Token: 0x040002F7 RID: 759
	public static float PullV3ResetTime = 1.5f;

	// Token: 0x040002F8 RID: 760
	public static int PullV3MaxStacks = 4;

	// Token: 0x040002F9 RID: 761
	public static float PullV3FreezeStrength = 6.5f;

	// Token: 0x040002FA RID: 762
	public static bool RemoveWindBarrierEnabled = false;

	// Token: 0x040002FB RID: 763
	public static bool NoArmCapEnabled = false;

	// Token: 0x040002FC RID: 764
	public static bool ArmCapEnabled = false;

	// Token: 0x040002FD RID: 765
	public static float ArmCapValue = 1.5f;

	// Token: 0x040002FE RID: 766
	public static bool WallWalkEnabled = false;

	// Token: 0x040002FF RID: 767
	public static float WallWalkDistance = 0.5f;

	// Token: 0x04000300 RID: 768
	public static float WallWalkPower = 2f;

	// Token: 0x04000301 RID: 769
	public static bool WallWalkLeftHand = true;

	// Token: 0x04000302 RID: 770
	public static bool WallWalkRightHand = true;

	// Token: 0x04000303 RID: 771
	public static Keybind WallWalkKeybind = Keybind.RightGrip;

	// Token: 0x04000304 RID: 772
	public static bool CGTWallWalkEnabled = false;

	// Token: 0x04000305 RID: 773
	public static float CGTWallWalkDistance = 0.5f;

	// Token: 0x04000306 RID: 774
	public static float CGTWallWalkPower = 2f;

	// Token: 0x04000307 RID: 775
	public static float CGTWallWalkReach = 1f;

	// Token: 0x04000308 RID: 776
	public static Keybind CGTWallWalkKeybind = Keybind.RightGrip;

	// Token: 0x04000309 RID: 777
	public static bool WallWalkIncludeTrees = true;

	// Token: 0x0400030A RID: 778
	public static bool WallWalkBlacklistTrees = false;

	// Token: 0x0400030B RID: 779
	public static bool WallWalkBlacklistWalls = false;

	// Token: 0x0400030C RID: 780
	public static bool WallWalkBlacklistSlippery = false;

	// Token: 0x0400030D RID: 781
	public static bool WallWalkPerHandBinds = false;

	// Token: 0x0400030E RID: 782
	public static Keybind WallWalkLeftKeybind = Keybind.LeftGrip;

	// Token: 0x0400030F RID: 783
	public static Keybind WallWalkRightKeybind = Keybind.RightGrip;

	// Token: 0x04000310 RID: 784
	public static bool DCFlickEnabled = false;

	// Token: 0x04000311 RID: 785
	public static bool DownControllerEnabled = false;

	// Token: 0x04000312 RID: 786
	public static Keybind DownControllerKeybind = Keybind.LeftGrip;

	// Token: 0x04000313 RID: 787
	public static float DownControllerDistance = 10f;

	// Token: 0x04000314 RID: 788
	public static float DownControllerSpeed = 10f;

	// Token: 0x04000315 RID: 789
	public static bool DcFlickAutoEnabled = false;

	// Token: 0x04000316 RID: 790
	public static float DcFlickAutoHoldTime = 0.3f;

	// Token: 0x04000317 RID: 791
	public static bool DcFlickAutoLOS = false;

	// Token: 0x04000318 RID: 792
	public static float DcFlickAutoMaxDistance = 10f;

	// Token: 0x04000319 RID: 793
	public static float DcFlickAutoFov = 90f;

	// Token: 0x0400031A RID: 794
	public static Keybind DcFlickAutoKeybind = Keybind.RightTrigger;

	// Token: 0x0400031B RID: 795
	public static bool TagAuraEnabled = true;

	// Token: 0x0400031C RID: 796
	public static float TagAuraDistance = 10f;

	// Token: 0x0400031D RID: 797
	public static Keybind TagAuraKeybind = Keybind.RightJoystickClick;

	// Token: 0x0400031E RID: 798
	public static bool TagAuraFovEnabled = false;

	// Token: 0x0400031F RID: 799
	public static float TagAuraFov = 90f;

	// Token: 0x04000320 RID: 800
	public static bool HitboxExpanderEnabled = false;

	// Token: 0x04000321 RID: 801
	public static float HitboxExpanderSize = 0.5f;

	// Token: 0x04000322 RID: 802
	public static float HitboxExpanderOpacity = 0.5f;

	// Token: 0x04000323 RID: 803
	public static bool PSAEnabled = false;

	// Token: 0x04000324 RID: 804
	public static float PSASpeed = 1f;

	// Token: 0x04000325 RID: 805
	public static Keybind PSAKeybind = Keybind.RightTrigger;

	// Token: 0x04000326 RID: 806
	public static bool VeloPSAEnabled = false;

	// Token: 0x04000327 RID: 807
	public static float VeloPSASpeed = 1f;

	// Token: 0x04000328 RID: 808
	public static Keybind VeloPSAKeybind = Keybind.RightGrip;

	// Token: 0x04000329 RID: 809
	public static float VeloPSAMinSpeed = 0.12f;

	// Token: 0x0400032A RID: 810
	public static bool VeloPSAStickyDirection = false;

	// Token: 0x0400032B RID: 811
	public static bool VeloPSAAirTurnAssist = false;

	// Token: 0x0400032C RID: 812
	public static float VeloPSAAirTurnBlend = 0.6f;

	// Token: 0x0400032D RID: 813
	public static bool BurstPSAEnabled = false;

	// Token: 0x0400032E RID: 814
	public static float BurstPSASpeed = 1f;

	// Token: 0x0400032F RID: 815
	public static Keybind BurstPSAKeybind = Keybind.RightGrip;

	// Token: 0x04000330 RID: 816
	public static float BurstPSAMinSpeed = 0.12f;

	// Token: 0x04000331 RID: 817
	public static bool BurstPSAStickyDirection = false;

	// Token: 0x04000332 RID: 818
	public static bool BurstPSAAirTurnAssist = false;

	// Token: 0x04000333 RID: 819
	public static float BurstPSAAirTurnBlend = 0.6f;

	// Token: 0x04000334 RID: 820
	public static float BurstPSADuration = 200f;

	// Token: 0x04000335 RID: 821
	public static bool HighJumpEnabled = false;

	// Token: 0x04000336 RID: 822
	public static float HighJumpSpeed = 5f;

	// Token: 0x04000337 RID: 823
	public static Keybind HighJumpKeybind = Keybind.RightPrimary;

	// Token: 0x04000338 RID: 824
	public static bool RecRoomEnabled = false;

	// Token: 0x04000339 RID: 825
	public static float RecRoomSpeed = 1f;

	// Token: 0x0400033A RID: 826
	public static Keybind RecRoomForwardKeybind = Keybind.RightSecondary;

	// Token: 0x0400033B RID: 827
	public static Keybind RecRoomBackwardKeybind = Keybind.LeftSecondary;

	// Token: 0x0400033C RID: 828
	public static bool PSASkiddedEnabled = false;

	// Token: 0x0400033D RID: 829
	public static float PSASkiddedSpeed = 8.8f;

	// Token: 0x0400033E RID: 830
	public static Keybind PSASkiddedKeybind = Keybind.RightSecondary;

	// Token: 0x0400033F RID: 831
	public static Keybind PSASkiddedBackwardKeybind = Keybind.LeftSecondary;

	// Token: 0x04000340 RID: 832
	public static MoveDirection PSASkiddedDirection = MoveDirection.Forward;

	// Token: 0x04000341 RID: 833
	public static float PSASkiddedGravityStrength = 1f;

	// Token: 0x04000342 RID: 834
	public static bool PSASkiddedFakeGlitchwalk = false;

	// Token: 0x04000343 RID: 835
	public static bool PSASkiddedRecRoom = false;

	// Token: 0x04000344 RID: 836
	public static float PSASkiddedRecRoomSpeed = 2.5f;

	// Token: 0x04000345 RID: 837
	public static DiagonalDirection PSASkiddedRecRoomDirection = DiagonalDirection.LeftDiagonal;

	// Token: 0x04000346 RID: 838
	public static bool Cr1ptsPSAEnabled = false;

	// Token: 0x04000347 RID: 839
	public static float Cr1ptsPSAStrength = 0.5f;

	// Token: 0x04000348 RID: 840
	public static float Cr1ptsPSALerpSpeed = 0.35f;

	// Token: 0x04000349 RID: 841
	public static Keybind Cr1ptsPSAKeybind = Keybind.LeftJoystickClick;

	// Token: 0x0400034A RID: 842
	public static bool VelmaxEnabled = false;

	// Token: 0x0400034B RID: 843
	public static float VelmaxMaxJumpSpeed = 6.5f;

	// Token: 0x0400034C RID: 844
	public static float VelmaxJumpMultiplier = 1.1f;

	// Token: 0x0400034D RID: 845
	public static int CurrentTimeOfDay = 0;

	// Token: 0x0400034E RID: 846
	public static int CurrentWeather = 0;

	// Token: 0x0400034F RID: 847
	public static bool ForceTagFreezeEnabled = false;

	// Token: 0x04000350 RID: 848
	public static Keybind ForceTagFreezeKeybind = Keybind.LeftGrip;

	// Token: 0x04000351 RID: 849
	public static bool PanicEnabled = false;

	// Token: 0x04000352 RID: 850
	public static Keybind PanicKeybind = Keybind.LeftSecondary;

	// Token: 0x04000353 RID: 851
	public static bool PanicActive = false;

	// Token: 0x04000354 RID: 852
	public static bool LongArmsEnabled = false;

	// Token: 0x04000355 RID: 853
	public static float LongArmsAmount = 1.2f;

	// Token: 0x04000356 RID: 854
	public static bool LongArmsBypassEnabled = false;

	// Token: 0x04000357 RID: 855
	public static Keybind LongArmsBypassKeybind = Keybind.LeftSecondary;

	// Token: 0x04000358 RID: 856
	public static bool LongArmsBypassLeftHand = true;

	// Token: 0x04000359 RID: 857
	public static bool LongArmsBypassRightHand = true;

	// Token: 0x0400035A RID: 858
	public static bool LongArmsBypassSmoothEnabled = false;

	// Token: 0x0400035B RID: 859
	public static float LongArmsBypassSmoothness = 12f;

	// Token: 0x0400035C RID: 860
	public static bool LongArmsBypassYOffsetEnabled = false;

	// Token: 0x0400035D RID: 861
	public static float LongArmsBypassYOffset = 0f;

	// Token: 0x0400035E RID: 862
	public static bool GrayScreenEnabled = false;

	// Token: 0x0400035F RID: 863
	public static Keybind GrayScreenKeybind = Keybind.RightPrimary;

	// Token: 0x04000360 RID: 864
	public static HandSide GrayScreenDirection = HandSide.Left;

	// Token: 0x04000361 RID: 865
	public static float GrayScreenDuration = 0.5f;

	// Token: 0x04000362 RID: 866
	public static float GrayScreenArmDistance = 3f;

	// Token: 0x04000363 RID: 867
	public static float GrayScreenArmSpread = 1.2f;

	// Token: 0x04000364 RID: 868
	public static bool HzSliderEnabled = false;

	// Token: 0x04000365 RID: 869
	public static int TargetHz = 72;

	// Token: 0x04000366 RID: 870
	public static bool NoSlipEnabled = false;

	// Token: 0x04000367 RID: 871
	public static bool SlipSlapEnabled = false;

	// Token: 0x04000368 RID: 872
	public static float SlipSlapMultiplier = 1f;

	// Token: 0x04000369 RID: 873
	public static bool SurfaceSlipEnabled = false;

	// Token: 0x0400036A RID: 874
	public static float SurfaceSlipWalls = 0f;

	// Token: 0x0400036B RID: 875
	public static float SurfaceSlipLowerSlippery = 0.55f;

	// Token: 0x0400036C RID: 876
	public static float SurfaceSlipUpperSlippery = 0.85f;

	// Token: 0x0400036D RID: 877
	public static bool VelmaxBypassEnabled = false;

	// Token: 0x0400036E RID: 878
	public static Keybind VelmaxBypassKeybind = Keybind.LeftGrip;

	// Token: 0x0400036F RID: 879
	public static bool WorldScaleBypassEnabled = false;

	// Token: 0x04000370 RID: 880
	public static Keybind WorldScaleBypassKeybind = Keybind.RightGrip;

	// Token: 0x04000371 RID: 881
	public static bool FakeOculusMenuEnabled = false;

	// Token: 0x04000372 RID: 882
	public static Keybind FakeOculusMenuKeybind = Keybind.LeftPrimary;

	// Token: 0x04000373 RID: 883
	public static bool NoFingerMovementEnabled = false;

	// Token: 0x04000374 RID: 884
	public static bool FakeQuestMenuEnabled = false;

	// Token: 0x04000375 RID: 885
	public static Keybind FakeQuestMenuKeybind = Keybind.LeftPrimary;

	// Token: 0x04000376 RID: 886
	public static bool FakeReportMenuEnabled = false;

	// Token: 0x04000377 RID: 887
	public static Keybind FakeReportMenuKeybind = Keybind.LeftSecondary;

	// Token: 0x04000378 RID: 888
	public static bool FakePowerOffEnabled = false;

	// Token: 0x04000379 RID: 889
	public static Keybind FakePowerOffKeybind = Keybind.LeftJoystickClick;

	// Token: 0x0400037A RID: 890
	public static bool PredsEnabled = false;

	// Token: 0x0400037B RID: 891
	public static float PredsAmount = 0f;

	// Token: 0x0400037C RID: 892
	public static bool PredsAlwaysOn = false;

	// Token: 0x0400037D RID: 893
	public static float PredsAlwaysAmount = 30f;

	// Token: 0x0400037E RID: 894
	public static HandSelection PredsHand = HandSelection.Both;

	// Token: 0x0400037F RID: 895
	public static bool PredsLeftHandEnabled = true;

	// Token: 0x04000380 RID: 896
	public static bool PredsRightHandEnabled = true;

	// Token: 0x04000381 RID: 897
	public static bool AntiPredEnabled = false;

	// Token: 0x04000382 RID: 898
	public static bool VisualiseServerEnabled = false;

	// Token: 0x04000383 RID: 899
	public static bool VisualiseClientEnabled = false;

	// Token: 0x04000384 RID: 900
	public static bool DesyncEnabled = false;

	// Token: 0x04000385 RID: 901
	public static LagMode DesyncMode = LagMode.Delay;

	// Token: 0x04000386 RID: 902
	public static float DesyncDelayMs = 100f;

	// Token: 0x04000387 RID: 903
	public static bool DesyncVisualise = false;

	// Token: 0x04000388 RID: 904
	public static Keybind DesyncLagSwitchKeybind = Keybind.RightJoystickClick;

	// Token: 0x04000389 RID: 905
	public static float DesyncFakeLagFreezeMs = 500f;

	// Token: 0x0400038A RID: 906
	public static float DesyncFakeLagIntervalMs = 1000f;

	// Token: 0x0400038B RID: 907
	public static bool ESPEnabled = false;

	// Token: 0x0400038C RID: 908
	public static float ESPColorHue = 0f;

	// Token: 0x0400038D RID: 909
	public static float ESPColorSat = 1f;

	// Token: 0x0400038E RID: 910
	public static float ESPColorVal = 1f;

	// Token: 0x0400038F RID: 911
	public static bool TracersEnabled = false;

	// Token: 0x04000390 RID: 912
	public static bool HitboxesEnabled = false;

	// Token: 0x04000391 RID: 913
	public static bool NameTagsEnabled = false;

	// Token: 0x04000392 RID: 914
	public static bool CornerESPEnabled = false;

	// Token: 0x04000393 RID: 915
	public static bool BoneESPEnabled = false;

	// Token: 0x04000394 RID: 916
	public static bool ChamsEnabled = false;

	// Token: 0x04000395 RID: 917
	public static float ChamsColorHue = 0f;

	// Token: 0x04000396 RID: 918
	public static float ChamsColorSat = 1f;

	// Token: 0x04000397 RID: 919
	public static float ChamsColorVal = 1f;

	// Token: 0x04000398 RID: 920
	public static bool ChamsColorCoded = true;

	// Token: 0x04000399 RID: 921
	public static float ChamsTaggedColorAHue = 0f;

	// Token: 0x0400039A RID: 922
	public static float ChamsTaggedColorASat = 1f;

	// Token: 0x0400039B RID: 923
	public static float ChamsTaggedColorAVal = 1f;

	// Token: 0x0400039C RID: 924
	public static float ChamsTaggedColorBHue = 0f;

	// Token: 0x0400039D RID: 925
	public static float ChamsTaggedColorBSat = 0f;

	// Token: 0x0400039E RID: 926
	public static float ChamsTaggedColorBVal = 0f;

	// Token: 0x0400039F RID: 927
	public static bool RecolorTaggedEnabled = false;

	// Token: 0x040003A0 RID: 928
	public static float RecolorTaggedColorHue = 0f;

	// Token: 0x040003A1 RID: 929
	public static float RecolorTaggedColorSat = 1f;

	// Token: 0x040003A2 RID: 930
	public static float RecolorTaggedColorVal = 1f;

	// Token: 0x040003A3 RID: 931
	public static bool RGBESPEnabled = false;

	// Token: 0x040003A4 RID: 932
	public static int BoxESPMode = 0;

	// Token: 0x040003A5 RID: 933
	public static bool FillESPEnabled = false;

	// Token: 0x040003A6 RID: 934
	public static float FillESPOpacity = 0.3f;

	// Token: 0x040003A7 RID: 935
	public static float FillESPColorHue = 0f;

	// Token: 0x040003A8 RID: 936
	public static float FillESPColorSat = 1f;

	// Token: 0x040003A9 RID: 937
	public static float FillESPColorVal = 1f;

	// Token: 0x040003AA RID: 938
	public static bool DistanceESPEnabled = false;

	// Token: 0x040003AB RID: 939
	public static bool DistanceESPHUD = false;

	// Token: 0x040003AC RID: 940
	public static bool GameSenseRadarEnabled = false;

	// Token: 0x040003AD RID: 941
	public static bool BeaconsEnabled = false;

	// Token: 0x040003AE RID: 942
	public static float BeaconWidth = 0.08f;

	// Token: 0x040003AF RID: 943
	public static bool ChinaHatESPEnabled = false;

	// Token: 0x040003B0 RID: 944
	public static bool RingESPEnabled = false;

	// Token: 0x040003B1 RID: 945
	public static bool VibrationAlertsEnabled = false;

	// Token: 0x040003B2 RID: 946
	public static float VibrationAlertDistance = 5f;

	// Token: 0x040003B3 RID: 947
	public static float VibrationAlertStrength = 0.8f;

	// Token: 0x040003B4 RID: 948
	public static bool PlayerGlowEnabled = false;

	// Token: 0x040003B5 RID: 949
	public static float PlayerGlowColorHue = 0f;

	// Token: 0x040003B6 RID: 950
	public static float PlayerGlowIntensity = 1f;

	// Token: 0x040003B7 RID: 951
	public static bool BreadcrumbsEnabled = false;

	// Token: 0x040003B8 RID: 952
	public static float BreadcrumbSpacing = 1f;

	// Token: 0x040003B9 RID: 953
	public static float BreadcrumbSize = 0.08f;

	// Token: 0x040003BA RID: 954
	public static float BreadcrumbColorHue = 0.33f;

	// Token: 0x040003BB RID: 955
	public static float BreadcrumbColorSat = 1f;

	// Token: 0x040003BC RID: 956
	public static float BreadcrumbColorVal = 1f;

	// Token: 0x040003BD RID: 957
	public static bool HandOffsetLeftEnabled = false;

	// Token: 0x040003BE RID: 958
	public static float HandOffsetLeftX = 0f;

	// Token: 0x040003BF RID: 959
	public static float HandOffsetLeftY = 0f;

	// Token: 0x040003C0 RID: 960
	public static float HandOffsetLeftZ = 0f;

	// Token: 0x040003C1 RID: 961
	public static bool HandOffsetRightEnabled = false;

	// Token: 0x040003C2 RID: 962
	public static float HandOffsetRightX = 0f;

	// Token: 0x040003C3 RID: 963
	public static float HandOffsetRightY = 0f;

	// Token: 0x040003C4 RID: 964
	public static float HandOffsetRightZ = 0f;

	// Token: 0x040003C5 RID: 965
	public static bool KillFeedEnabled = false;

	// Token: 0x040003C6 RID: 966
	public static float KillFeedDuration = 5f;

	// Token: 0x040003C7 RID: 967
	public static bool KillFeedWorldSpace = false;

	// Token: 0x040003C8 RID: 968
	public static bool TickRateEnabled = false;

	// Token: 0x040003C9 RID: 969
	public static float TickRate = 1f;

	// Token: 0x040003CA RID: 970
	public static bool SoundESPEnabled = false;

	// Token: 0x040003CB RID: 971
	public static float SoundESPDistance = 10f;

	// Token: 0x040003CC RID: 972
	public static float SoundESPVolume = 0.7f;

	// Token: 0x040003CD RID: 973
	public static float SoundESPCooldown = 1f;

	// Token: 0x040003CE RID: 974
	public static int SoundESPPreset = 1;

	// Token: 0x040003CF RID: 975
	public static string SoundESPCustomPath = "";

	// Token: 0x040003D0 RID: 976
	public static bool SlideControlEnabled = false;

	// Token: 0x040003D1 RID: 977
	public static float SlideControlAmount = 0.5f;

	// Token: 0x040003D2 RID: 978
	public static bool TagNotificationEnabled = false;

	// Token: 0x040003D3 RID: 979
	public static float TagNotificationDistance = 3f;

	// Token: 0x040003D4 RID: 980
	public static bool NameTagsModEnabled = false;

	// Token: 0x040003D5 RID: 981
	public static float NameTagFontSize = 2f;

	// Token: 0x040003D6 RID: 982
	public static float NameTagOffset = 0.5f;

	// Token: 0x040003D7 RID: 983
	public static float NameTagRenderDistance = 50f;

	// Token: 0x040003D8 RID: 984
	public static bool PlatformTagsEnabled = false;

	// Token: 0x040003D9 RID: 985
	public static float PlatformTagFontSize = 2f;

	// Token: 0x040003DA RID: 986
	public static float PlatformTagOffset = 0.7f;

	// Token: 0x040003DB RID: 987
	public static bool FPSTagsModEnabled = false;

	// Token: 0x040003DC RID: 988
	public static float FPSTagFontSize = 2f;

	// Token: 0x040003DD RID: 989
	public static float FPSTagOffset = 0.9f;

	// Token: 0x040003DE RID: 990
	public static bool RemoveLeavesEnabled = false;

	// Token: 0x040003DF RID: 991
	public static bool WASDFlyEnabled = false;

	// Token: 0x040003E0 RID: 992
	public static float WASDFlySpeed = 8f;

	// Token: 0x040003E1 RID: 993
	public static Keybind WASDFlyKeybind = Keybind.Always;

	// Token: 0x040003E2 RID: 994
	public static bool KayFlockEnabled = false;

	// Token: 0x040003E3 RID: 995
	public static float KayFlockSpeed = 10f;

	// Token: 0x040003E4 RID: 996
	public static Keybind KayFlockKeybind = Keybind.RightGrip;

	// Token: 0x040003E5 RID: 997
	public static bool AutoBranchEnabled = false;

	// Token: 0x040003E6 RID: 998
	public static Keybind AutoBranchRecordKeybind = Keybind.LeftPrimary;

	// Token: 0x040003E7 RID: 999
	public static Keybind AutoBranchStopKeybind = Keybind.LeftSecondary;

	// Token: 0x040003E8 RID: 1000
	public static Keybind AutoBranchReplayKeybind = Keybind.RightPrimary;

	// Token: 0x040003E9 RID: 1001
	public static bool AutoBranchLoop = false;

	// Token: 0x040003EA RID: 1002
	public static bool AutoBranchMovePlayer = true;

	// Token: 0x040003EB RID: 1003
	public static bool AutoBranchSmartMode = false;

	// Token: 0x040003EC RID: 1004
	public static float AutoBranchMatchRadius = 6f;

	// Token: 0x040003ED RID: 1005
	public static float AutoBranchBlendTime = 0.3f;

	// Token: 0x040003EE RID: 1006
	public static bool AutoBranchPathFinder = false;

	// Token: 0x040003EF RID: 1007
	public static bool TrailsEnabled = false;

	// Token: 0x040003F0 RID: 1008
	public static float TrailMinSpeed = 1f;

	// Token: 0x040003F1 RID: 1009
	public static float TrailSpeedScale = 8f;

	// Token: 0x040003F2 RID: 1010
	public static float TrailMinTime = 0.15f;

	// Token: 0x040003F3 RID: 1011
	public static float TrailMaxTime = 1.2f;

	// Token: 0x040003F4 RID: 1012
	public static float TrailWidth = 0.25f;

	// Token: 0x040003F5 RID: 1013
	public static bool TrailUsePlayerColor = true;

	// Token: 0x040003F6 RID: 1014
	public static float TrailColorHue = 0f;

	// Token: 0x040003F7 RID: 1015
	public static float TrailColorSat = 1f;

	// Token: 0x040003F8 RID: 1016
	public static float TrailColorVal = 1f;

	// Token: 0x040003F9 RID: 1017
	public static bool SkyboxEnabled = false;

	// Token: 0x040003FA RID: 1018
	public static int SkyboxMode = 0;

	// Token: 0x040003FB RID: 1019
	public static float SkyboxColorHue = 0.58f;

	// Token: 0x040003FC RID: 1020
	public static float SkyboxColorSat = 0.5f;

	// Token: 0x040003FD RID: 1021
	public static float SkyboxColorVal = 0.9f;

	// Token: 0x040003FE RID: 1022
	public static float SkyboxColor2Hue = 0.75f;

	// Token: 0x040003FF RID: 1023
	public static float SkyboxColor2Sat = 0.5f;

	// Token: 0x04000400 RID: 1024
	public static float SkyboxColor2Val = 0.6f;

	// Token: 0x04000401 RID: 1025
	public static string SkyboxImagePath = "";

	// Token: 0x04000402 RID: 1026
	public static float SkyboxGradientSpeed = 0.15f;

	// Token: 0x04000403 RID: 1027
	public static int SkyboxStarCount = 150;

	// Token: 0x04000404 RID: 1028
	public static float SkyboxStarSize = 1f;

	// Token: 0x04000405 RID: 1029
	public static bool JewishMusicEnabled = true;

	// Token: 0x04000406 RID: 1030
	public static bool ShowMenu = false;

	// Token: 0x04000407 RID: 1031
	public static int CurrentTab = 0;

	// Token: 0x04000408 RID: 1032
	public static bool ShowBuggyFeatures = false;

	// Token: 0x04000409 RID: 1033
	public static KeyCode MenuKey = 277;

	// Token: 0x0400040A RID: 1034
	public static MenuStyle CurrentMenuTheme = MenuStyle.Absense;

	// Token: 0x0400040B RID: 1035
	public static float MenuBgColorHue = 0.667f;

	// Token: 0x0400040C RID: 1036
	public static float MenuBgColorSat = 0.5f;

	// Token: 0x0400040D RID: 1037
	public static float MenuBgColorVal = 0.04f;

	// Token: 0x0400040E RID: 1038
	public static float WireframeColorHue = 0.72f;

	// Token: 0x0400040F RID: 1039
	public static float WireframeColorSat = 0.45f;

	// Token: 0x04000410 RID: 1040
	public static float WireframeColorVal = 0.98f;

	// Token: 0x04000411 RID: 1041
	public static int PongBallCount = 1;

	// Token: 0x04000412 RID: 1042
	public static bool PongAIEnabled = false;
}
