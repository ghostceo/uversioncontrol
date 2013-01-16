﻿// Copyright (c) <2012> <Playdead>
// This file is subject to the MIT License as seen in the trunk of this repository
// Maintained by: <Kristian Kjems> <kristian.kjems+UnityVC@gmail.com>
// This file is with a few exceptions handling everything relating to Unity .meta files.
// It adds .meta on outgoing commands and removes .meta files from GetFilteredAssets

using System;
using System.Collections.Generic;
using System.Linq;

namespace VersionControl
{
    public static class VersionControlStatusExtension
    {
        private static readonly ComposedString meta = new ComposedString(VCCAddMetaFiles.meta);
        public static VersionControlStatus MetaStatus(this VersionControlStatus vcs)
        {
            return vcs.assetPath.EndsWith(meta) ? vcs : VCCommands.Instance.GetAssetStatus(vcs.assetPath + meta);
        }
    }

    [Serializable]
    public class VCCAddMetaFiles : VCCDecorator
    {
        public const string meta = ".meta";
        private const string assetsFolder = "Assets/";

        public VCCAddMetaFiles(IVersionControlCommands vcc) : base(vcc) { }

        public override bool RequestStatus(IEnumerable<string> assets, StatusLevel statusLevel)
        {
            return base.RequestStatus(AddMeta(assets), statusLevel);
        }

        public override bool Status(IEnumerable<string> assets, StatusLevel statusLevel)
        {
            return base.Status(AddMeta(assets), statusLevel);
        }

        public override bool Update(IEnumerable<string> assets = null)
        {
            return base.Update(AddMeta(assets));
        }

        public override bool Commit(IEnumerable<string> assets, string commitMessage = "")
        {
            return base.Commit(AddMeta(assets), commitMessage);
        }

        public override bool Add(IEnumerable<string> assets)
        {
            return base.Add(AddMeta(assets));
        }

        public override bool Revert(IEnumerable<string> assets)
        {
            return base.Revert(AddMeta(assets));
        }

        public override bool Delete(IEnumerable<string> assets, OperationMode mode)
        {
            return base.Delete(AddMeta(assets), mode);
        }

        public override bool Move(string from, string to)
        {
            return base.Move(from, to) && base.Move(from + meta, to + meta);
        }

        public override bool Resolve(IEnumerable<string> assets, ConflictResolution conflictResolution)
        {
            return base.Resolve(AddMeta(assets), conflictResolution);
        }

        public override IEnumerable<VersionControlStatus> GetFilteredAssets(Func<VersionControlStatus, bool> filter)
        {
            return RemoveMetaPostFix(base.GetFilteredAssets(filter));
        }

        public override void RemoveFromDatabase(IEnumerable<string> assets)
        {
            base.RemoveFromDatabase(AddMeta(assets));
        }

        private static IEnumerable<string> AddMeta(IEnumerable<string> assets)
        {
            if (assets == null || !assets.Any()) return assets;
            return assets
                .Where(ap => !ap.EndsWith(meta) && ap.StartsWith(assetsFolder))
                .Select(ap => ap + meta)
                .Concat(assets)
                .Distinct()
                .OrderBy(s => s.Length)
                .ToArray();
        }

        public static IEnumerable<VersionControlStatus> RemoveMetaPostFix(IEnumerable<VersionControlStatus> assets)
        {
            return assets.Where(status => !status.assetPath.EndsWith(meta));
        }
    }
}
