using System;
using System.Reflection;
using GorillaLocomotion;

// Token: 0x02000005 RID: 5
public static class ArmCap
{
	// Token: 0x06000003 RID: 3 RVA: 0x0000206C File Offset: 0x0000026C
	private static void EnsureReflection()
	{
		bool flag = ArmCap.reflectionReady || ArmCap.reflectionFailed;
		if (!flag)
		{
			try
			{
				Type playerType = typeof(GTPlayer);
				ArmCap.leftHandField = playerType.GetField("leftHand", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				ArmCap.rightHandField = playerType.GetField("rightHand", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo fieldInfo = ArmCap.leftHandField;
				Type handType = ((fieldInfo != null) ? fieldInfo.FieldType : null);
				ArmCap.maxArmLengthField = ((handType != null) ? handType.GetField("maxArmLength", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : null);
				ArmCap.reflectionReady = ArmCap.leftHandField != null && ArmCap.rightHandField != null && ArmCap.maxArmLengthField != null;
				ArmCap.reflectionFailed = !ArmCap.reflectionReady;
			}
			catch
			{
				ArmCap.reflectionFailed = true;
			}
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002148 File Offset: 0x00000348
	private static void SetHandMaxArm(FieldInfo handField, GTPlayer player, float value)
	{
		object hand = handField.GetValue(player);
		bool flag = hand == null;
		if (!flag)
		{
			ArmCap.maxArmLengthField.SetValue(hand, value);
			handField.SetValue(player, hand);
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002184 File Offset: 0x00000384
	public static void Apply()
	{
		try
		{
			GTPlayer player = GTPlayer.Instance;
			bool flag = player == null;
			if (!flag)
			{
				ArmCap.EnsureReflection();
				bool unlimited = !Settings.NoArmCapEnabled;
				bool shouldCap = unlimited || Settings.ArmCapEnabled;
				bool flag2 = shouldCap;
				if (flag2)
				{
					bool flag3 = !ArmCap.active;
					if (flag3)
					{
						ArmCap.savedMaxArmLength = player.maxArmLength;
						ArmCap.active = true;
					}
					float maxArmLength = (unlimited ? 999999f : Settings.ArmCapValue);
					player.maxArmLength = maxArmLength;
					bool flag4 = ArmCap.reflectionReady;
					if (flag4)
					{
						ArmCap.SetHandMaxArm(ArmCap.leftHandField, player, maxArmLength);
						ArmCap.SetHandMaxArm(ArmCap.rightHandField, player, maxArmLength);
					}
				}
				else
				{
					bool flag5 = !ArmCap.active;
					if (!flag5)
					{
						float restoredLength = ((ArmCap.savedMaxArmLength > 0f) ? ArmCap.savedMaxArmLength : 1.5f);
						player.maxArmLength = restoredLength;
						bool flag6 = ArmCap.reflectionReady;
						if (flag6)
						{
							ArmCap.SetHandMaxArm(ArmCap.leftHandField, player, restoredLength);
							ArmCap.SetHandMaxArm(ArmCap.rightHandField, player, restoredLength);
						}
						ArmCap.active = false;
					}
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000022B8 File Offset: 0x000004B8
	public static float GetCurrentArmLength()
	{
		float num;
		try
		{
			GTPlayer player = GTPlayer.Instance;
			bool flag = player == null;
			if (flag)
			{
				num = -1f;
			}
			else
			{
				ArmCap.EnsureReflection();
				bool flag2 = !ArmCap.reflectionReady;
				if (flag2)
				{
					num = -2f;
				}
				else
				{
					object hand = ArmCap.leftHandField.GetValue(player);
					num = ((hand == null) ? (-3f) : ((float)ArmCap.maxArmLengthField.GetValue(hand)));
				}
			}
		}
		catch
		{
			num = -4f;
		}
		return num;
	}

	// Token: 0x04000013 RID: 19
	private static bool active;

	// Token: 0x04000014 RID: 20
	private static float savedMaxArmLength = -1f;

	// Token: 0x04000015 RID: 21
	private static FieldInfo leftHandField;

	// Token: 0x04000016 RID: 22
	private static FieldInfo rightHandField;

	// Token: 0x04000017 RID: 23
	private static FieldInfo maxArmLengthField;

	// Token: 0x04000018 RID: 24
	private static bool reflectionReady;

	// Token: 0x04000019 RID: 25
	private static bool reflectionFailed;
}
