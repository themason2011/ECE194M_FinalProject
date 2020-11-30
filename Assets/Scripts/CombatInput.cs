// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/CombatInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CombatInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CombatInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CombatInput"",
    ""maps"": [
        {
            ""name"": ""SelectAction"",
            ""id"": ""6f16c116-34aa-44d5-b275-88fbd56f6ab6"",
            ""actions"": [
                {
                    ""name"": ""Move Down"",
                    ""type"": ""Button"",
                    ""id"": ""43abb21f-83ae-4737-83ae-63c6fd4f7be6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e4bcbdc9-7af5-41e5-a72e-a2608d762962"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // SelectAction
        m_SelectAction = asset.FindActionMap("SelectAction", throwIfNotFound: true);
        m_SelectAction_MoveDown = m_SelectAction.FindAction("Move Down", throwIfNotFound: true);
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

    // SelectAction
    private readonly InputActionMap m_SelectAction;
    private ISelectActionActions m_SelectActionActionsCallbackInterface;
    private readonly InputAction m_SelectAction_MoveDown;
    public struct SelectActionActions
    {
        private @CombatInput m_Wrapper;
        public SelectActionActions(@CombatInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveDown => m_Wrapper.m_SelectAction_MoveDown;
        public InputActionMap Get() { return m_Wrapper.m_SelectAction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SelectActionActions set) { return set.Get(); }
        public void SetCallbacks(ISelectActionActions instance)
        {
            if (m_Wrapper.m_SelectActionActionsCallbackInterface != null)
            {
                @MoveDown.started -= m_Wrapper.m_SelectActionActionsCallbackInterface.OnMoveDown;
                @MoveDown.performed -= m_Wrapper.m_SelectActionActionsCallbackInterface.OnMoveDown;
                @MoveDown.canceled -= m_Wrapper.m_SelectActionActionsCallbackInterface.OnMoveDown;
            }
            m_Wrapper.m_SelectActionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveDown.started += instance.OnMoveDown;
                @MoveDown.performed += instance.OnMoveDown;
                @MoveDown.canceled += instance.OnMoveDown;
            }
        }
    }
    public SelectActionActions @SelectAction => new SelectActionActions(this);
    public interface ISelectActionActions
    {
        void OnMoveDown(InputAction.CallbackContext context);
    }
}
