﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pulsar4X.ECSLib
{
    [Flags]
    public enum IndustryType
    {
        None                        = 0,
        InstallationConstruction    = 1 << 0,
        ComponentConstruction       = 1 << 1,
        ShipConstruction            = 1 << 2,
        FighterConstruction         = 1 << 3,
        OrdnanceConstruction        = 1 << 4,
        TerraformingRate            = 1 << 5,
        SalvageRate                 = 1 << 6,
        JPStabilizationRate         = 1 << 7,
        ResearchRate                = 1 << 8,
    }

    public class IndustryJob
    {
        public IndustryType IndustryType { get; internal set; }

        public Guid ItemGuid { get; internal set; }
        public uint NumberOrdered { get; set; }
        public uint NumberCompleted { get; internal set; }

        public float BPToNextCompletion { get; internal set; }
        public float BPPerItem { get; internal set; }

        [JsonProperty]
        internal Dictionary<Guid, float> materialsRequiredPerItem { get; set; } = new Dictionary<Guid, float>();
        [JsonIgnore]
        public ReadOnlyDictionary<Guid, float> MaterialsRequiredPerItem => new ReadOnlyDictionary<Guid, float>(materialsRequiredPerItem);
    }
}
