using System;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x0200003F RID: 63
public static class VrInput
{
	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060001D0 RID: 464 RVA: 0x0001FFB2 File Offset: 0x0001E1B2
	// (set) Token: 0x060001D1 RID: 465 RVA: 0x0001FFB9 File Offset: 0x0001E1B9
	public static bool IsOculus { get; set; }

	// Token: 0x060001D2 RID: 466 RVA: 0x0001FFC4 File Offset: 0x0001E1C4
	private static bool ReadButton(XRNode node, InputFeatureUsage<bool> usage, SteamVR_Action_Boolean steamAction, SteamVR_Input_Sources source)
	{
		bool flag;
		try
		{
			bool isOculus = VrInput.IsOculus;
			if (isOculus)
			{
				bool value = false;
				InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(usage, ref value);
				flag = value;
			}
			else
			{
				flag = steamAction.GetState(source);
			}
		}
		catch
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00020018 File Offset: 0x0001E218
	private static Vector2 ReadAxis(XRNode node, SteamVR_Action_Vector2 steamAction, SteamVR_Input_Sources source)
	{
		Vector2 vector;
		try
		{
			bool isOculus = VrInput.IsOculus;
			if (isOculus)
			{
				Vector2 value = Vector2.zero;
				InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.primary2DAxis, ref value);
				vector = value;
			}
			else
			{
				vector = steamAction.GetAxis(source);
			}
		}
		catch
		{
			vector = Vector2.zero;
		}
		return vector;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00020078 File Offset: 0x0001E278
	public static bool RightJoystickClick()
	{
		return VrInput.ReadButton(5, CommonUsages.primary2DAxisClick, SteamVR_Actions.gorillaTag_RightJoystickClick, 2);
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0002008B File Offset: 0x0001E28B
	public static bool LeftJoystickClick()
	{
		return VrInput.ReadButton(4, CommonUsages.primary2DAxisClick, SteamVR_Actions.gorillaTag_LeftJoystickClick, 1);
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0002009E File Offset: 0x0001E29E
	public static bool RightTrigger()
	{
		return VrInput.ReadButton(5, CommonUsages.triggerButton, SteamVR_Actions.gorillaTag_RightTriggerClick, 2);
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x000200B1 File Offset: 0x0001E2B1
	public static bool LeftTrigger()
	{
		return VrInput.ReadButton(4, CommonUsages.triggerButton, SteamVR_Actions.gorillaTag_LeftTriggerClick, 1);
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x000200C4 File Offset: 0x0001E2C4
	public static bool RightGrip()
	{
		return VrInput.ReadButton(5, CommonUsages.gripButton, SteamVR_Actions.gorillaTag_RightGripClick, 2);
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x000200D7 File Offset: 0x0001E2D7
	public static bool LeftGrip()
	{
		return VrInput.ReadButton(4, CommonUsages.gripButton, SteamVR_Actions.gorillaTag_LeftGripClick, 1);
	}

	// Token: 0x060001DA RID: 474 RVA: 0x000200EA File Offset: 0x0001E2EA
	public static bool RightPrimary()
	{
		return VrInput.ReadButton(5, CommonUsages.primaryButton, SteamVR_Actions.gorillaTag_RightPrimaryClick, 2);
	}

	// Token: 0x060001DB RID: 475 RVA: 0x000200FD File Offset: 0x0001E2FD
	public static bool LeftPrimary()
	{
		return VrInput.ReadButton(4, CommonUsages.primaryButton, SteamVR_Actions.gorillaTag_LeftPrimaryClick, 1);
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00020110 File Offset: 0x0001E310
	public static bool RightSecondary()
	{
		return VrInput.ReadButton(5, CommonUsages.secondaryButton, SteamVR_Actions.gorillaTag_RightSecondaryClick, 2);
	}

	// Token: 0x060001DD RID: 477 RVA: 0x00020123 File Offset: 0x0001E323
	public static bool LeftSecondary()
	{
		return VrInput.ReadButton(4, CommonUsages.secondaryButton, SteamVR_Actions.gorillaTag_LeftSecondaryClick, 1);
	}

	// Token: 0x060001DE RID: 478 RVA: 0x00020136 File Offset: 0x0001E336
	public static Vector2 RightJoystickAxis()
	{
		return VrInput.ReadAxis(5, SteamVR_Actions.gorillaTag_RightJoystick2DAxis, 2);
	}

	// Token: 0x060001DF RID: 479 RVA: 0x00020144 File Offset: 0x0001E344
	public static Vector2 LeftJoystickAxis()
	{
		return VrInput.ReadAxis(4, SteamVR_Actions.gorillaTag_LeftJoystick2DAxis, 1);
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00020154 File Offset: 0x0001E354
	public static bool GetButton(string button, bool isLeft)
	{
		try
		{
			ControllerInputPoller instance = ControllerInputPoller.instance;
			bool flag = instance == null;
			if (flag)
			{
				return false;
			}
			string text = button.ToLower();
			string text2 = text;
			if (text2 == "grip")
			{
				return isLeft ? instance.leftGrab : instance.rightGrab;
			}
			if (text2 == "trigger")
			{
				return isLeft ? (instance.leftControllerIndexFloat > 0.5f) : (instance.rightControllerIndexFloat > 0.5f);
			}
			if (text2 == "primary")
			{
				return isLeft ? instance.leftControllerPrimaryButton : instance.rightControllerPrimaryButton;
			}
			if (text2 == "secondary")
			{
				return isLeft ? instance.leftControllerSecondaryButton : instance.rightControllerSecondaryButton;
			}
		}
		catch
		{
		}
		return false;
	}

	// Token: 0x04000494 RID: 1172
	private const XRNode RightHandNode = 5;

	// Token: 0x04000495 RID: 1173
	private const XRNode LeftHandNode = 4;

	// Token: 0x04000496 RID: 1174
	private const SteamVR_Input_Sources RightHandSource = 2;

	// Token: 0x04000497 RID: 1175
	private const SteamVR_Input_Sources LeftHandSource = 1;
}
