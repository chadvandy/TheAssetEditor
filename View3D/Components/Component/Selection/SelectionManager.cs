﻿using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using Serilog;
using System;
using System.Collections.Generic;
using View3D.Components.Gizmo;
using View3D.Components.Input;
using View3D.Components.Rendering;
using View3D.Rendering;
using View3D.Rendering.Geometry;
using View3D.Rendering.RenderItems;
using View3D.Scene;
using View3D.SceneNodes;

namespace View3D.Components.Component.Selection
{
    public delegate void SelectionChangedDelegate(ISelectionState state);
    public class SelectionManager : BaseComponent
    {
        public event SelectionChangedDelegate SelectionChanged;

        ILogger _logger = Logging.Create<SelectionManager>();
        ISelectionState _currentState;
        RenderEngineComponent _renderEngine;

        LineMeshRender BoundingBoxRenderer;
        VertexInstanceMesh VertexRenderer;

        public SelectionManager(WpfGame game ) : base(game) {}

        public override void Initialize()
        {
            CreateSelectionSate(GeometrySelectionMode.Object);
            _renderEngine = GetComponent<RenderEngineComponent>();


            BoundingBoxRenderer = new LineMeshRender(Game.Content);
            VertexRenderer = new VertexInstanceMesh(GraphicsDevice, Game.Content);

            base.Initialize();
        }

        public ISelectionState CreateSelectionSate(GeometrySelectionMode mode)
        {
            if (_currentState != null)
            {
                _currentState.Clear();
                _currentState.SelectionChanged -= SelectionManager_SelectionChanged;
            }

            switch (mode)
            {
                case GeometrySelectionMode.Object:
                    _currentState = new ObjectSelectionState();
                    break;

                case GeometrySelectionMode.Face:
                    _currentState = new FaceSelectionState();
                    break;

                case GeometrySelectionMode.Vertex:
                    _currentState = new VertexSelectionState();
                    break;

                default:
                    throw new Exception();
            }

            _currentState.SelectionChanged += SelectionManager_SelectionChanged;
            return _currentState;
        }

        public ISelectionState GetState()
        {
            return _currentState;
        }

        public ISelectionState GetStateCopy()
        {
            return _currentState.Clone();
        }

        public void SetState(ISelectionState state)
        {
            _currentState.SelectionChanged -= SelectionManager_SelectionChanged;
            _currentState = state;
            _currentState.SelectionChanged += SelectionManager_SelectionChanged;
            _currentState.Restore();
        }

        private void SelectionManager_SelectionChanged(ISelectionState state)
        {
            SelectionChanged?.Invoke(state);
        }

        public override void Draw(GameTime gameTime)
        {
            var selectionState = GetState();

            if (selectionState is ObjectSelectionState objectSelectionState)
            {
                foreach (var item in objectSelectionState.CurrentSelection())
                {
                    BoundingBoxRenderer.CreateFromBoundingBox(item.Geometry.BoundingBox);
                    _renderEngine.AddRenderItem(RenderBuckedId.Selection, new LineRenderItem() { World = item.ModelMatrix, LineMesh = BoundingBoxRenderer });
                }
            }

            if (selectionState is FaceSelectionState selectionFaceState && selectionFaceState.RenderObject is MeshNode meshNode)
            {
                _renderEngine.AddRenderItem(RenderBuckedId.Selection, new FaceRenderItem() { Node = meshNode, World = meshNode.ModelMatrix, SelectedFaces = selectionFaceState.CurrentSelection() });
                _renderEngine.AddRenderItem(RenderBuckedId.Wireframe, new WireFrameRenderItem() { World = meshNode.ModelMatrix, Node = meshNode });
            }

            if (selectionState is VertexSelectionState selectionVertexState && selectionVertexState.RenderObject != null)
            {
                var vertexObject = selectionVertexState.RenderObject as MeshNode;
                _renderEngine.AddRenderItem(RenderBuckedId.Selection, new VertexRenderItem() { Node = vertexObject, World = vertexObject.ModelMatrix, SelectedVertices = selectionVertexState.SelectedVertices, VertexRenderer = VertexRenderer });
                _renderEngine.AddRenderItem(RenderBuckedId.Wireframe, new WireFrameRenderItem() { World = Matrix.Identity, Node = vertexObject });
            }

            base.Draw(gameTime);
        }
    }
}

