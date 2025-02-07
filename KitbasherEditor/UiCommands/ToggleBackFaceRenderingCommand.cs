﻿using CommonControls.Common.MenuSystem;
using CommonControls.Events.UiCommands;
using KitbasherEditor.ViewModels.MenuBarViews;
using System.Windows.Input;
using View3D.Components.Rendering;

namespace KitbasherEditor.ViewModels.UiCommands
{
    public class ToggleBackFaceRenderingCommand : IKitbasherUiCommand
    {
        public string ToolTip { get; set; } = "Toggle backface rendering";
        public ActionEnabledRule EnabledRule => ActionEnabledRule.Always;
        public Hotkey HotKey { get; } = null;

        private readonly RenderEngineComponent _renderEngineComponent;

        public ToggleBackFaceRenderingCommand(RenderEngineComponent renderEngineComponent)
        {
            _renderEngineComponent = renderEngineComponent;
        }

        public void Execute() => _renderEngineComponent.ToggelBackFaceRendering();
    }
}
