using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000013 RID: 19
public static class FingerMovement
{
	// Token: 0x0600007E RID: 126 RVA: 0x0000B714 File Offset: 0x00009914
	public static void FreezeInputs()
	{
		try
		{
			ControllerInputPoller instance = ControllerInputPoller.instance;
			bool flag = !(instance == null);
			if (flag)
			{
				instance.leftControllerGripFloat = 0f;
				instance.rightControllerGripFloat = 0f;
				instance.leftControllerIndexFloat = 0f;
				instance.rightControllerIndexFloat = 0f;
				instance.leftControllerPrimaryButton = false;
				instance.leftControllerSecondaryButton = false;
				instance.rightControllerPrimaryButton = false;
				instance.rightControllerSecondaryButton = false;
				instance.leftControllerPrimaryButtonTouch = false;
				instance.leftControllerSecondaryButtonTouch = false;
				instance.rightControllerPrimaryButtonTouch = false;
				instance.rightControllerSecondaryButtonTouch = false;
			}
		}
		catch
		{
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x0000B7B8 File Offset: 0x000099B8
	public static void ApplyLongArms()
	{
		bool noFingerMovementEnabled = Settings.NoFingerMovementEnabled;
		if (noFingerMovementEnabled)
		{
			FingerMovement.FreezeInputs();
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x0000B7DC File Offset: 0x000099DC
	public static void ApplyLongArmsBypass()
	{
		bool noFingerMovementEnabled = Settings.NoFingerMovementEnabled;
		if (noFingerMovementEnabled)
		{
			FingerMovement.FreezeInputs();
		}
	}

	// Token: 0x06000081 RID: 129 RVA: 0x0000B800 File Offset: 0x00009A00
	public static void FakeQuestMenu()
	{
		bool flag = !Settings.FakeQuestMenuEnabled;
		if (!flag)
		{
			bool flag2 = !Settings.IsPressed(Settings.FakeQuestMenuKeybind);
			if (flag2)
			{
				bool flag3 = !FingerMovement.longArmsActive;
				if (!flag3)
				{
					try
					{
						GTPlayer instance = GTPlayer.Instance;
						bool flag4 = instance != null;
						if (flag4)
						{
							instance.inOverlay = false;
							instance.disableMovement = false;
						}
					}
					catch
					{
					}
					FingerMovement.longArmsActive = false;
				}
			}
			else
			{
				FingerMovement.longArmsActive = true;
				try
				{
					GTPlayer instance2 = GTPlayer.Instance;
					bool flag5 = !(instance2 == null);
					if (flag5)
					{
						bool noFingerMovementEnabled = Settings.NoFingerMovementEnabled;
						if (noFingerMovementEnabled)
						{
							FingerMovement.FreezeInputs();
						}
						instance2.inOverlay = true;
						instance2.disableMovement = true;
						Transform transform = Camera.main.transform;
						Vector3 position = transform.position;
						Vector3 position2 = position + transform.forward * 0.3f + -transform.right * 0.15f + transform.up * -0.1f;
						Vector3 position3 = position + transform.forward * 0.3f + transform.right * 0.15f + transform.up * -0.1f;
						instance2.LeftHand.controllerTransform.position = position2;
						instance2.RightHand.controllerTransform.position = position3;
						instance2.LeftHand.controllerTransform.rotation = transform.rotation * Quaternion.Euler(-55f, 90f, 0f);
						instance2.RightHand.controllerTransform.rotation = transform.rotation * Quaternion.Euler(-55f, -49f, 0f);
					}
				}
				catch
				{
				}
			}
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x0000BA34 File Offset: 0x00009C34
	public static void FakeReportMenu()
	{
		bool flag = !Settings.FakeReportMenuEnabled;
		if (!flag)
		{
			bool flag2 = !Settings.IsPressed(Settings.FakeReportMenuKeybind);
			if (flag2)
			{
				bool flag3 = FingerMovement.bypassActive;
				if (flag3)
				{
					FingerMovement.CloseFakeMenu();
				}
				FingerMovement.bypassActive = false;
			}
			else
			{
				FingerMovement.bypassActive = true;
				try
				{
					GTPlayer instance = GTPlayer.Instance;
					bool flag4 = !(instance == null);
					if (flag4)
					{
						bool noFingerMovementEnabled = Settings.NoFingerMovementEnabled;
						if (noFingerMovementEnabled)
						{
							FingerMovement.FreezeInputs();
						}
						instance.inOverlay = true;
						Transform val = ((Camera.main != null) ? Camera.main.transform : null);
						bool flag5 = !(val == null);
						if (flag5)
						{
							Vector3 position = val.position;
							Vector3 position2 = position + val.forward * 0.3f + -val.right * 0.15f + val.up * -0.1f;
							Vector3 position3 = position + val.forward * 0.3f + val.right * 0.15f + val.up * -0.1f;
							instance.LeftHand.controllerTransform.position = position2;
							instance.RightHand.controllerTransform.position = position3;
						}
					}
				}
				catch
				{
				}
			}
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x0000BBD4 File Offset: 0x00009DD4
	private static void CloseFakeMenu()
	{
		try
		{
			GTPlayer instance = GTPlayer.Instance;
			bool flag = instance != null;
			if (flag)
			{
				instance.inOverlay = false;
			}
		}
		catch
		{
		}
	}

	// Token: 0x06000084 RID: 132 RVA: 0x0000BC18 File Offset: 0x00009E18
	public static void FakePowerOff()
	{
		bool flag2 = !Settings.FakePowerOffEnabled;
		if (!flag2)
		{
			bool flag = Settings.IsPressed(Settings.FakePowerOffKeybind);
			try
			{
				bool flag3 = flag;
				if (flag3)
				{
					bool flag4 = FingerMovement.leftRestPos == Vector3.zero;
					if (flag4)
					{
						FingerMovement.leftRestPos = GorillaTagger.Instance.rigidbody.transform.position;
						FingerMovement.rightRestPos = GorillaTagger.Instance.rigidbody.linearVelocity;
					}
					VRRig.LocalRig.enabled = false;
					GorillaTagger.Instance.rigidbody.transform.position = FingerMovement.leftRestPos;
					GorillaTagger.Instance.rigidbody.linearVelocity = FingerMovement.rightRestPos;
				}
				else
				{
					bool flag5 = FingerMovement.leftRestPos != Vector3.zero;
					if (flag5)
					{
						VRRig.LocalRig.enabled = true;
						FingerMovement.leftRestPos = Vector3.zero;
					}
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x040001AE RID: 430
	private static Vector3 leftRestPos = Vector3.zero;

	// Token: 0x040001AF RID: 431
	private static Vector3 rightRestPos = Vector3.zero;

	// Token: 0x040001B0 RID: 432
	private static bool longArmsActive = false;

	// Token: 0x040001B1 RID: 433
	private static bool bypassActive = false;
}
