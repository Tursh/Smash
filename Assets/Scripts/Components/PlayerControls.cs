// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""3988458c-a04c-4433-b6ee-bb439cc7461c"",
            ""actions"": [
                {
                    ""name"": ""LeftJoystick"",
                    ""type"": ""Value"",
                    ""id"": ""22fc026e-8f7b-40ce-8a18-fd46b1e60dc4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightJoystick"",
                    ""type"": ""Value"",
                    ""id"": ""6d24483d-1266-406c-be90-015bf1a77c9d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""A"",
                    ""type"": ""Button"",
                    ""id"": ""a0c7a0d6-8f85-4eda-901c-53713d16cd10"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""B"",
                    ""type"": ""Button"",
                    ""id"": ""1b3d4676-74f3-4e13-8a4f-c17a494b9511"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""X"",
                    ""type"": ""Button"",
                    ""id"": ""abb6cdde-bd9e-4294-9e6a-3f182c0380d5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Y"",
                    ""type"": ""Button"",
                    ""id"": ""a6b1bece-d72e-4764-b37f-cf28ab2e9c13"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LB"",
                    ""type"": ""Button"",
                    ""id"": ""176346bd-4e4d-46de-89df-b3dcd3af9944"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RB"",
                    ""type"": ""Button"",
                    ""id"": ""c62df6d9-3b5c-449d-bb1e-0b4f9f6204d6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LT"",
                    ""type"": ""Value"",
                    ""id"": ""2aa5f684-11c9-4dc1-a0c3-8da5f2fff3c5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RT"",
                    ""type"": ""Value"",
                    ""id"": ""11d66751-6b07-401d-baac-3bd97e757d10"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPad"",
                    ""type"": ""Value"",
                    ""id"": ""4d749434-b905-449a-8739-7918549cb9e4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""8acc98ec-5a83-4e5f-8b73-4b084763793d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""f7519607-129e-4666-8160-c3c99a338285"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftJoystickPress"",
                    ""type"": ""Button"",
                    ""id"": ""c6623e1b-75f5-4891-90ce-570fa4cacab7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightJoystickPress"",
                    ""type"": ""Button"",
                    ""id"": ""9a04ed8f-9085-4801-a33a-086dba8a0620"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyA"",
                    ""type"": ""Button"",
                    ""id"": ""b5a8dd48-7eb7-4504-aed2-b05d924f730c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""KeyD"",
                    ""type"": ""Button"",
                    ""id"": ""e3be107f-6f15-4565-84e5-b1260f1890a1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""KeySpace"",
                    ""type"": ""Button"",
                    ""id"": ""6a4e3ecb-5e54-4bc4-955b-d3f96cfcd50b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5a6b285f-f55c-42c3-877c-2f9d896c4c46"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80874f28-f600-4468-8e26-fbe2d9c0eae5"",
                    ""path"": ""<XInputController>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53aeda7c-0f15-4adf-bf32-00e887fc2488"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""A"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a247d688-bb01-4494-8246-a9ce2d296d4b"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""B"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d7180fff-37f9-4ef3-a47e-b4363aa9834a"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45f5e58b-3036-44f5-9333-e6eb9bbeab68"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Y"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4161e37-9302-4a1c-8d8c-c0a7c8b46324"",
                    ""path"": ""<XInputController>/leftShoulder"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b355ba1f-09f8-477c-b9e3-9a3bb050e4fd"",
                    ""path"": ""<XInputController>/rightShoulder"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d2e88df9-587d-4c17-8e6c-579e58fd3d3f"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2197e880-d2ab-46e1-bfb4-a02c668d72db"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85652ecb-7521-4546-8598-a305a2b163e3"",
                    ""path"": ""<XInputController>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd498bca-13d4-463a-b102-b4e7e3055029"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a3db7f3-5f3d-4297-8b44-8eeab86fa479"",
                    ""path"": ""<XInputController>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17501e3d-0337-4893-8d5d-d5e5931e8bfc"",
                    ""path"": ""<XInputController>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftJoystickPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e202d3ef-c076-4801-8086-550340143f92"",
                    ""path"": ""<XInputController>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightJoystickPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5f0bc3c-4d05-4210-ae84-f2d32302e318"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyA"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfa2142f-619f-495e-ac13-c811a6cbf6a1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53b8a0f9-b4ac-4a35-a998-cc0c898fb24f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeySpace"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""0fa2b654-c809-41da-ae30-4684e7a10ba6"",
            ""actions"": [
                {
                    ""name"": ""RightStick"",
                    ""type"": ""Value"",
                    ""id"": ""bfa4b9ba-3ebc-4102-8f7a-0fad0d911349"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftStick"",
                    ""type"": ""Value"",
                    ""id"": ""714f1d50-d675-4615-8548-395b4d04c5cf"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8ce32c91-72f6-48a2-b2de-7787710358a8"",
                    ""path"": ""<XInputController>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9bb94544-70cd-4d58-9aea-de8ea9d5e5fc"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(max=1)"",
                    ""groups"": """",
                    ""action"": ""LeftStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_LeftJoystick = m_Gameplay.FindAction("LeftJoystick", throwIfNotFound: true);
        m_Gameplay_RightJoystick = m_Gameplay.FindAction("RightJoystick", throwIfNotFound: true);
        m_Gameplay_A = m_Gameplay.FindAction("A", throwIfNotFound: true);
        m_Gameplay_B = m_Gameplay.FindAction("B", throwIfNotFound: true);
        m_Gameplay_X = m_Gameplay.FindAction("X", throwIfNotFound: true);
        m_Gameplay_Y = m_Gameplay.FindAction("Y", throwIfNotFound: true);
        m_Gameplay_LB = m_Gameplay.FindAction("LB", throwIfNotFound: true);
        m_Gameplay_RB = m_Gameplay.FindAction("RB", throwIfNotFound: true);
        m_Gameplay_LT = m_Gameplay.FindAction("LT", throwIfNotFound: true);
        m_Gameplay_RT = m_Gameplay.FindAction("RT", throwIfNotFound: true);
        m_Gameplay_DPad = m_Gameplay.FindAction("DPad", throwIfNotFound: true);
        m_Gameplay_Start = m_Gameplay.FindAction("Start", throwIfNotFound: true);
        m_Gameplay_Select = m_Gameplay.FindAction("Select", throwIfNotFound: true);
        m_Gameplay_LeftJoystickPress = m_Gameplay.FindAction("LeftJoystickPress", throwIfNotFound: true);
        m_Gameplay_RightJoystickPress = m_Gameplay.FindAction("RightJoystickPress", throwIfNotFound: true);
        m_Gameplay_KeyA = m_Gameplay.FindAction("KeyA", throwIfNotFound: true);
        m_Gameplay_KeyD = m_Gameplay.FindAction("KeyD", throwIfNotFound: true);
        m_Gameplay_KeySpace = m_Gameplay.FindAction("KeySpace", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_RightStick = m_Menu.FindAction("RightStick", throwIfNotFound: true);
        m_Menu_LeftStick = m_Menu.FindAction("LeftStick", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_LeftJoystick;
    private readonly InputAction m_Gameplay_RightJoystick;
    private readonly InputAction m_Gameplay_A;
    private readonly InputAction m_Gameplay_B;
    private readonly InputAction m_Gameplay_X;
    private readonly InputAction m_Gameplay_Y;
    private readonly InputAction m_Gameplay_LB;
    private readonly InputAction m_Gameplay_RB;
    private readonly InputAction m_Gameplay_LT;
    private readonly InputAction m_Gameplay_RT;
    private readonly InputAction m_Gameplay_DPad;
    private readonly InputAction m_Gameplay_Start;
    private readonly InputAction m_Gameplay_Select;
    private readonly InputAction m_Gameplay_LeftJoystickPress;
    private readonly InputAction m_Gameplay_RightJoystickPress;
    private readonly InputAction m_Gameplay_KeyA;
    private readonly InputAction m_Gameplay_KeyD;
    private readonly InputAction m_Gameplay_KeySpace;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftJoystick => m_Wrapper.m_Gameplay_LeftJoystick;
        public InputAction @RightJoystick => m_Wrapper.m_Gameplay_RightJoystick;
        public InputAction @A => m_Wrapper.m_Gameplay_A;
        public InputAction @B => m_Wrapper.m_Gameplay_B;
        public InputAction @X => m_Wrapper.m_Gameplay_X;
        public InputAction @Y => m_Wrapper.m_Gameplay_Y;
        public InputAction @LB => m_Wrapper.m_Gameplay_LB;
        public InputAction @RB => m_Wrapper.m_Gameplay_RB;
        public InputAction @LT => m_Wrapper.m_Gameplay_LT;
        public InputAction @RT => m_Wrapper.m_Gameplay_RT;
        public InputAction @DPad => m_Wrapper.m_Gameplay_DPad;
        public InputAction @Start => m_Wrapper.m_Gameplay_Start;
        public InputAction @Select => m_Wrapper.m_Gameplay_Select;
        public InputAction @LeftJoystickPress => m_Wrapper.m_Gameplay_LeftJoystickPress;
        public InputAction @RightJoystickPress => m_Wrapper.m_Gameplay_RightJoystickPress;
        public InputAction @KeyA => m_Wrapper.m_Gameplay_KeyA;
        public InputAction @KeyD => m_Wrapper.m_Gameplay_KeyD;
        public InputAction @KeySpace => m_Wrapper.m_Gameplay_KeySpace;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @LeftJoystick.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftJoystick;
                @LeftJoystick.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftJoystick;
                @LeftJoystick.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftJoystick;
                @RightJoystick.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightJoystick;
                @RightJoystick.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightJoystick;
                @RightJoystick.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightJoystick;
                @A.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnA;
                @A.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnA;
                @A.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnA;
                @B.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnB;
                @B.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnB;
                @B.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnB;
                @X.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnX;
                @X.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnX;
                @X.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnX;
                @Y.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnY;
                @Y.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnY;
                @Y.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnY;
                @LB.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLB;
                @LB.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLB;
                @LB.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLB;
                @RB.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRB;
                @RB.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRB;
                @RB.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRB;
                @LT.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLT;
                @LT.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLT;
                @LT.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLT;
                @RT.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRT;
                @RT.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRT;
                @RT.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRT;
                @DPad.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDPad;
                @DPad.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDPad;
                @DPad.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDPad;
                @Start.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Select.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSelect;
                @LeftJoystickPress.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftJoystickPress;
                @LeftJoystickPress.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftJoystickPress;
                @LeftJoystickPress.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftJoystickPress;
                @RightJoystickPress.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightJoystickPress;
                @RightJoystickPress.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightJoystickPress;
                @RightJoystickPress.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightJoystickPress;
                @KeyA.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKeyA;
                @KeyA.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKeyA;
                @KeyA.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKeyA;
                @KeyD.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKeyD;
                @KeyD.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKeyD;
                @KeyD.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKeyD;
                @KeySpace.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKeySpace;
                @KeySpace.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKeySpace;
                @KeySpace.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnKeySpace;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftJoystick.started += instance.OnLeftJoystick;
                @LeftJoystick.performed += instance.OnLeftJoystick;
                @LeftJoystick.canceled += instance.OnLeftJoystick;
                @RightJoystick.started += instance.OnRightJoystick;
                @RightJoystick.performed += instance.OnRightJoystick;
                @RightJoystick.canceled += instance.OnRightJoystick;
                @A.started += instance.OnA;
                @A.performed += instance.OnA;
                @A.canceled += instance.OnA;
                @B.started += instance.OnB;
                @B.performed += instance.OnB;
                @B.canceled += instance.OnB;
                @X.started += instance.OnX;
                @X.performed += instance.OnX;
                @X.canceled += instance.OnX;
                @Y.started += instance.OnY;
                @Y.performed += instance.OnY;
                @Y.canceled += instance.OnY;
                @LB.started += instance.OnLB;
                @LB.performed += instance.OnLB;
                @LB.canceled += instance.OnLB;
                @RB.started += instance.OnRB;
                @RB.performed += instance.OnRB;
                @RB.canceled += instance.OnRB;
                @LT.started += instance.OnLT;
                @LT.performed += instance.OnLT;
                @LT.canceled += instance.OnLT;
                @RT.started += instance.OnRT;
                @RT.performed += instance.OnRT;
                @RT.canceled += instance.OnRT;
                @DPad.started += instance.OnDPad;
                @DPad.performed += instance.OnDPad;
                @DPad.canceled += instance.OnDPad;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @LeftJoystickPress.started += instance.OnLeftJoystickPress;
                @LeftJoystickPress.performed += instance.OnLeftJoystickPress;
                @LeftJoystickPress.canceled += instance.OnLeftJoystickPress;
                @RightJoystickPress.started += instance.OnRightJoystickPress;
                @RightJoystickPress.performed += instance.OnRightJoystickPress;
                @RightJoystickPress.canceled += instance.OnRightJoystickPress;
                @KeyA.started += instance.OnKeyA;
                @KeyA.performed += instance.OnKeyA;
                @KeyA.canceled += instance.OnKeyA;
                @KeyD.started += instance.OnKeyD;
                @KeyD.performed += instance.OnKeyD;
                @KeyD.canceled += instance.OnKeyD;
                @KeySpace.started += instance.OnKeySpace;
                @KeySpace.performed += instance.OnKeySpace;
                @KeySpace.canceled += instance.OnKeySpace;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_RightStick;
    private readonly InputAction m_Menu_LeftStick;
    public struct MenuActions
    {
        private @PlayerControls m_Wrapper;
        public MenuActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @RightStick => m_Wrapper.m_Menu_RightStick;
        public InputAction @LeftStick => m_Wrapper.m_Menu_LeftStick;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @RightStick.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightStick;
                @RightStick.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightStick;
                @RightStick.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightStick;
                @LeftStick.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftStick;
                @LeftStick.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftStick;
                @LeftStick.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftStick;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RightStick.started += instance.OnRightStick;
                @RightStick.performed += instance.OnRightStick;
                @RightStick.canceled += instance.OnRightStick;
                @LeftStick.started += instance.OnLeftStick;
                @LeftStick.performed += instance.OnLeftStick;
                @LeftStick.canceled += instance.OnLeftStick;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnLeftJoystick(InputAction.CallbackContext context);
        void OnRightJoystick(InputAction.CallbackContext context);
        void OnA(InputAction.CallbackContext context);
        void OnB(InputAction.CallbackContext context);
        void OnX(InputAction.CallbackContext context);
        void OnY(InputAction.CallbackContext context);
        void OnLB(InputAction.CallbackContext context);
        void OnRB(InputAction.CallbackContext context);
        void OnLT(InputAction.CallbackContext context);
        void OnRT(InputAction.CallbackContext context);
        void OnDPad(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnLeftJoystickPress(InputAction.CallbackContext context);
        void OnRightJoystickPress(InputAction.CallbackContext context);
        void OnKeyA(InputAction.CallbackContext context);
        void OnKeyD(InputAction.CallbackContext context);
        void OnKeySpace(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnRightStick(InputAction.CallbackContext context);
        void OnLeftStick(InputAction.CallbackContext context);
    }
}
