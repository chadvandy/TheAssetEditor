﻿using CommonControls.Common;
using CommonControls.FileTypes.AnimationPack;
using CommonControls.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace AnimationEditor.Common.ReferenceModel
{
    public class SelectFragAndSlotViewModel : NotifyPropertyChangedImpl
    {
        string _currentSkeletonName = "";
        AssetViewModel _asset;

        public FilterCollection<IAnimationBinGenericFormat> FragmentList { get; set; }

        public FilterCollection<AnimationBinEntryGenericFormat> FragmentSlotList { get; set; }

        private readonly AssetViewModelBuilder _assetViewModelEditor;
        PackFileService _pfs;
        SkeletonAnimationLookUpHelper _skeletonAnimationLookUpHelper;
        SelectMetaViewModel _metaViewModel;
        private readonly ApplicationSettingsService _applicationSettings;

        public SelectFragAndSlotViewModel(AssetViewModelBuilder assetViewModelEditor, PackFileService pfs, SkeletonAnimationLookUpHelper skeletonAnimationLookUpHelper, AssetViewModel asset, SelectMetaViewModel metaViewModel, ApplicationSettingsService applicationSettings)
        {
            _assetViewModelEditor = assetViewModelEditor;
            _pfs = pfs;
            _skeletonAnimationLookUpHelper = skeletonAnimationLookUpHelper;
            _asset = asset;
            _metaViewModel = metaViewModel;
            _applicationSettings = applicationSettings;
            FragmentList = new FilterCollection<IAnimationBinGenericFormat>(null, (value) => FragmentSelected(value, FragmentSlotList, _asset.SkeletonName.Value))
            {
                SearchFilter = (value, rx) => { return rx.Match(value.FullPath).Success; }
            };
            FragmentSlotList = new FilterCollection<AnimationBinEntryGenericFormat>(null, FragmentSlotSelected)
            {
                SearchFilter = (value, rx) => { return rx.Match(value.SlotName).Success; }
            };

            OnSkeletonChange(_asset.Skeleton);
            Subscribe();
        }


        public void PreviewSelectedSlot()
        {
            if (FragmentList.SelectedItem != null && FragmentList.SelectedItem != null)
            {
                var animPack = FragmentList.SelectedItem.PackFileReference;
                var packFile = _pfs.FindFile(animPack.FileName);
                CommonControls.Editors.AnimationPack.AnimPackViewModel.ShowPreviewWinodow(packFile, _pfs, _skeletonAnimationLookUpHelper, FragmentList.SelectedItem.FullPath, _applicationSettings);
            }
        }

        void Subscribe()
        {
            _asset.AnimationChanged += OnAnimationChange;
            _asset.SkeletonChanged += OnSkeletonChange;
        }

        private void OnSkeletonChange(View3D.Animation.GameSkeleton newValue)
        {
            Unsubscribe();

            FragmentList.SelectedItem = null;
            FragmentSlotList.SelectedItem = null;

            if (newValue == null)
            {
                FragmentList.UpdatePossibleValues(new List<IAnimationBinGenericFormat>());
                FragmentSlotList.UpdatePossibleValues(new List<AnimationBinEntryGenericFormat>());
                _currentSkeletonName = "";
                return;
            }

            // Same skeleton again, should fix this propper.
            if (_currentSkeletonName == _asset.SkeletonName.Value)
                return;

            _currentSkeletonName = _asset.SkeletonName.Value;
            var skeletonName = Path.GetFileNameWithoutExtension(_asset.SkeletonName.Value);
            var allPossibleFragments = LoadFragmentsForSkeleton(skeletonName);
            FragmentList.UpdatePossibleValues(allPossibleFragments);

            Subscribe();
        }

        private void OnAnimationChange(View3D.Animation.AnimationClip newValue)
        {
            //throw new NotImplementedException();
        }

        void Unsubscribe()
        {
            _asset.AnimationChanged += OnAnimationChange;
            _asset.SkeletonChanged += OnSkeletonChange;
        }

        public List<IAnimationBinGenericFormat> LoadFragmentsForSkeleton(string skeletonName, bool onlyPacksThatCanBeSaved = false)
        {
            var outputFragments = new List<IAnimationBinGenericFormat>();
            var animPacks = _pfs.GetAllAnimPacks();
            foreach (var animPack in animPacks)
            {
                if (onlyPacksThatCanBeSaved == true)
                {
                    if (_pfs.GetPackFileContainer(animPack).IsCaPackFile)
                        continue;
                }

                var animPackFile = AnimationPackSerializer.Load(animPack, _pfs);
                var fragments = animPackFile.GetGenericAnimationSets(skeletonName);
                foreach (var fragment in fragments)
                    outputFragments.Add(fragment);
            }
            return outputFragments;
        }

        void FragmentSelected(IAnimationBinGenericFormat value, FilterCollection<AnimationBinEntryGenericFormat> animationSlotsCollection, string skeletonName)
        {
            if (value == null)
            {
                animationSlotsCollection.UpdatePossibleValues(null);
                return;
            }

            var newSkeletonName = value.SkeletonName;
            var existingSkeletonName = Path.GetFileNameWithoutExtension(skeletonName);
            if (newSkeletonName != existingSkeletonName)
            {
                MessageBox.Show("This fragment does not fit the current skeleton");
                return;
            }

            animationSlotsCollection.UpdatePossibleValues(value.Entries);
        }

        private void FragmentSlotSelected(AnimationBinEntryGenericFormat value)
        {
            if (value == null)
            {
                _assetViewModelEditor.SetAnimation(_asset, null);
                _assetViewModelEditor.SetMetaFile(_asset, null, null);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(value.AnimationFile) == false)
                {
                    var file = _pfs.FindFile(value.AnimationFile);
                    var animationRef = _skeletonAnimationLookUpHelper.FindAnimationRefFromPackFile(file, _pfs);
                    _assetViewModelEditor.SetAnimation(_asset, animationRef);
                }
                else
                {
                    _assetViewModelEditor.SetAnimation(_asset, null);
                }

                if (string.IsNullOrWhiteSpace(value.MetaFile) == false)
                    _metaViewModel.SelectedMetaFile = _pfs.FindFile(value.MetaFile);
                else
                    _metaViewModel.SelectedMetaFile = null;

                var persist = FragmentSlotList.PossibleValues.FirstOrDefault(x => x.SlotName == "PERSISTENT_METADATA_ALIVE");
                if (persist != null && string.IsNullOrWhiteSpace(persist.MetaFile) == false)
                    _metaViewModel.SelectedPersistMetaFile = _pfs.FindFile(persist.MetaFile);
                else
                    _metaViewModel.SelectedPersistMetaFile = null;

                if (_metaViewModel.SelectedPersistMetaFile == null)
                {
                    var persistFlying = FragmentSlotList.PossibleValues.FirstOrDefault(x => x.SlotName == "PERSISTENT_METADATA_FLYING");
                    if (persistFlying != null && string.IsNullOrWhiteSpace(persistFlying.MetaFile) == false)
                        _metaViewModel.SelectedPersistMetaFile = _pfs.FindFile(persistFlying.MetaFile);
                }
            }
        }
    }
}
