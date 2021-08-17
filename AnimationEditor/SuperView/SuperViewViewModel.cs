﻿using AnimationEditor.PropCreator.ViewModels;
using Common;
using CommonControls.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimationEditor.SuperView
{


    public class SuperViewViewModel : BaseAnimationViewModel
    {
        public SuperViewViewModel(PackFileService pfs, SkeletonAnimationLookUpHelper skeletonHelper) : base(pfs, skeletonHelper, "not_in_use1", "not_in_use2")
        {
            DisplayName = "Super view";
            Pfs = pfs;
        }

        public PackFileService Pfs { get; }

        public override void Initialize()
        {
            MainModelView.IsControlVisible.Value = false;
            ReferenceModelView.IsControlVisible.Value = false;
            ReferenceModelView.Data.IsSelectable = false;

            Editor = new Editor();
            //var propAsset = Scene.AddCompnent(new AssetViewModel(_pfs, "Generated", Color.Black, Scene));
            //Player.RegisterAsset(propAsset);
            //Editor = new Editor(_pfs, _skeletonHelper, MainModelView.Data, ReferenceModelView.Data, propAsset, Scene);
        }
    }

    public static class SuperViewViewModel_Debug
    {
        public static void CreateDamselEditor(IEditorCreator creator, IToolFactory toolFactory, PackFileService packfileService)
        {
            var editorView = toolFactory.CreateEditorViewModel<SuperViewViewModel>();
            //editorView.MainInput = new AnimationToolInput()
            //{
            //    //Mesh = packfileService.FindFile(@"variantmeshes\variantmeshdefinitions\wef_bladesinger.variantmeshdefinition"),
            //    //Mesh = packfileService.FindFile(@"variantmeshes\variantmeshdefinitions\ogr_maneater_base.variantmeshdefinition"),
            //    //Mesh = packfileService.FindFile(@"variantmeshes\variantmeshdefinitions\grn_savage_orc_base.variantmeshdefinition")
            //    //Mesh = packfileService.FindFile(@"variantmeshes\variantmeshdefinitions\chs_giants.variantmeshdefinition")
            //    Mesh = packfileService.FindFile(@"variantmeshes\variantmeshdefinitions\dwf_giant_slayers.variantmeshdefinition")
            //    //Animation = packfileService.FindFile(@"animations\battle\humanoid01b\sword_and_chalice\stand\hu1b_swch_stand_idle_04.anim")
            //    //Mesh = packfileService.FindFile(@"variantmeshes\variantmeshdefinitions\grn_forest_goblins_base.variantmeshdefinition")
            //};
            //
            //
            //editorView.RefInput = new AnimationToolInput()
            //{
            //    Mesh = packfileService.FindFile(@"variantmeshes\variantmeshdefinitions\emp_archer_ror.variantmeshdefinition"),
            //    Animation = packfileService.FindFile(@"animations\battle\humanoid01\sword_and_pistol\missile_attacks\hu1_swp_missile_attack_aim_to_shootready_01.anim")
            //
            //    //Mesh = packfileService.FindFile(@"variantmeshes\variantmeshdefinitions\3k_dlc05_unit_metal_camp_crushers.variantmeshdefinition"),
            //    //Animation = packfileService.FindFile(@"animations\battle\character\male01\infantry\inf_1h095bow\special_ability\special_ability_arrow_storm_01\inf_1h095bow_arrow_storm_01.anim")
            //};

            creator.CreateEmptyEditor(editorView);
        }
    }
}
