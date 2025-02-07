﻿using CommonControls;
using CommonControls.Common;
using CommonControls.Services.ToolCreation;
using Microsoft.Extensions.DependencyInjection;
using TextureEditor.ViewModels;
using TextureEditor.Views;

namespace TextureEditor
{
    public class TextureEditor_DependencyInjectionContainer : DependencyContainer
    {
        public override void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<TexturePreviewView>();
            serviceCollection.AddTransient<TextureEditorViewModel>();
            serviceCollection.AddTransient<IEditorViewModel, TextureEditorViewModel>();
        }

        public override void RegisterTools(IToolFactory factory)
        {
            factory.RegisterTool<TextureEditorViewModel, TexturePreviewView>(new ExtensionToTool(EditorEnums.Texture_Editor, new[] { ".dds", ".png", ".jpeg" }));
        }
    }
}
