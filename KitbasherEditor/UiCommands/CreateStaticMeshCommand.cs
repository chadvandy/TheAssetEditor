﻿using CommonControls.Common.MenuSystem;
using KitbasherEditor.ViewModels.MenuBarViews;
using System.Collections.Generic;
using View3D.Commands;
using View3D.Commands.Object;
using View3D.Components.Component;
using View3D.Components.Component.Selection;
using View3D.SceneNodes;
using MessageBox = System.Windows.MessageBox;

namespace KitbasherEditor.ViewModels.UiCommands
{
    public class CreateStaticMeshCommand : IKitbasherUiCommand
    {
        public string ToolTip { get; set; } = "Convert the selected mesh at at the given animation frame into a static mesh";
        public ActionEnabledRule EnabledRule => ActionEnabledRule.AtleastOneObjectSelected;
        public Hotkey HotKey { get; } = null;

        private readonly AnimationsContainerComponent _animationsContainerComponent;
        private readonly SelectionManager _selectionManager;
        private readonly CommandFactory _commandFactory;
        private readonly SceneManager _sceneManager;

        public CreateStaticMeshCommand(AnimationsContainerComponent animationsContainerComponent, SelectionManager selectionManager, CommandFactory commandFactory, SceneManager sceneManager)
        {
            _animationsContainerComponent = animationsContainerComponent;
            _selectionManager = selectionManager;
            _commandFactory = commandFactory;
            _sceneManager = sceneManager;
        }

        public void Execute()
        {
            // Get the frame
            var animationPlayers = _animationsContainerComponent;
            var mainPlayer = animationPlayers.Get("MainPlayer");

            var frame = mainPlayer.GetCurrentAnimationFrame();
            if (frame == null)
            {
                MessageBox.Show("An animation must be playing for this tool to work");
                return;
            }

            var state = _selectionManager.GetState<ObjectSelectionState>();
            var selectedObjects = state.SelectedObjects();
            List<Rmv2MeshNode> meshes = new List<Rmv2MeshNode>();

            GroupNode groupNodeContainer = new GroupNode("staticMesh");
            var root = _sceneManager.GetNodeByName<MainEditableNode>(SpecialNodes.EditableModel);
            var lod0 = root.GetLodNodes()[0];
            lod0.AddObject(groupNodeContainer);
            foreach (var obj in selectedObjects)
            {
                if (obj is Rmv2MeshNode meshNode)
                {
                    var cpy = SceneNodeHelper.CloneNode(meshNode);
                    groupNodeContainer.AddObject(cpy);
                    meshes.Add(cpy);
                }
            }

            _commandFactory.Create<CreateAnimatedMeshPoseCommand>()
                .IsUndoable(false)
                .Configure(x => x.Configure(meshes, frame, true))
                .BuildAndExecute();
        }
    }
}
