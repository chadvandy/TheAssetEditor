﻿using CommonControls.Common.MenuSystem;
using KitbasherEditor.ViewModels.MenuBarViews;
using System.Windows.Input;
using View3D.Components.Gizmo;

namespace KitbasherEditor.ViewModels.UiCommands
{
    internal class ScaleGizmoUpCommand : IKitbasherUiCommand
    {
        public string ToolTip { get; set; } = "Decrease Gizmo size";
        public ActionEnabledRule EnabledRule => ActionEnabledRule.Always;
        public Hotkey HotKey { get; } = new Hotkey(Key.Add, ModifierKeys.None);

        GizmoComponent _gizmoComponent;

     
        public ScaleGizmoUpCommand(GizmoComponent gizmoComponent)
        {
            _gizmoComponent = gizmoComponent;
        }

        public void Execute() => _gizmoComponent.ModifyGizmoScale(-0.5f);
    }

    internal class ScaleGizmoDownCommand : IKitbasherUiCommand
    {
        public string ToolTip { get; set; } = "Increase Gizmo size";
        public ActionEnabledRule EnabledRule => ActionEnabledRule.Always;
        public Hotkey HotKey { get; } = new Hotkey(Key.Add, ModifierKeys.None);

        GizmoComponent _gizmoComponent;


        public ScaleGizmoDownCommand(GizmoComponent gizmoComponent)
        {
            _gizmoComponent = gizmoComponent;
        }

        public void Execute() => _gizmoComponent.ModifyGizmoScale(0.5f);
    }
}
