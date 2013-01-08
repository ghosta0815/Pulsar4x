﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Pulsar4X.Entities.Components
{
    public class ComponentDefListTN
    {
        /// <summary>
        /// Crew Quarter components.
        /// </summary>
        public BindingList<GeneralComponentDefTN> CrewQuarters { get; set; }

        /// <summary>
        /// Fuel Tank components.
        /// </summary>
        public BindingList<GeneralComponentDefTN> FuelStorage { get; set; }

        /// <summary>
        /// Engineering bay components.
        /// </summary>
        public BindingList<GeneralComponentDefTN> EngineeringSpaces { get; set; }

        /// <summary>
        /// other specialized general components such as the bridge.
        /// </summary>
        public BindingList<GeneralComponentDefTN> OtherComponents { get; set; }

        /// <summary>
        /// Engine Components.
        /// </summary>
        public BindingList<EngineDefTN> Engines { get; set; }

        /// <summary>
        /// Passive Sensor components.
        /// </summary>
        public BindingList<PassiveSensorDefTN> PassiveSensorDef { get; set; }

        public PassiveSensorDefTN DefaultPassives { get; set; }

        /// <summary>
        /// Active Sensor Components.
        /// </summary>
        public BindingList<ActiveSensorDefTN> ActiveSensorDef { get; set; }

        /// <summary>
        /// ComponentDefList creates lists for every TN component. ClassTN and Faction should eventually both use this, but only faction does right now.
        /// </summary>
        public ComponentDefListTN()
        {
            CrewQuarters = new BindingList<GeneralComponentDefTN>();
            FuelStorage = new BindingList<GeneralComponentDefTN>();
            EngineeringSpaces = new BindingList<GeneralComponentDefTN>();
            OtherComponents = new BindingList<GeneralComponentDefTN>();

            Engines = new BindingList<EngineDefTN>();
            PassiveSensorDef = new BindingList<PassiveSensorDefTN>();
            ActiveSensorDef = new BindingList<ActiveSensorDefTN>();

            DefaultPassives = new PassiveSensorDefTN("Default, Don't display this one.", 1.0f, 1, PassiveSensorType.Thermal, 1.0f, 1);
        }

        /// <summary>
        /// Every faction will start with some components defined and ready to use, though the engines and sensors shouldn't be here just yet.
        /// </summary>
        public void AddInitialComponents()
        {
            GeneralComponentDefTN CrewQ = new GeneralComponentDefTN("Crew Quarters", 1.0f, 0, 10.0m, GeneralType.Crew);
            GeneralComponentDefTN CrewQS = new GeneralComponentDefTN("Crew Quarters - Small", 0.2f, 0, 2.0m, GeneralType.Crew);
            GeneralComponentDefTN FuelT = new GeneralComponentDefTN("Fuel Storage", 1.0f, 0, 10.0m, GeneralType.Fuel);
            GeneralComponentDefTN FuelTS = new GeneralComponentDefTN("Fuel Storage - Small", 0.2f, 0, 3.0m, GeneralType.Fuel);
            GeneralComponentDefTN EBay = new GeneralComponentDefTN("Engineering Spaces", 1.0f, 5, 10.0m, GeneralType.Engineering);
            GeneralComponentDefTN Bridge = new GeneralComponentDefTN("Bridge", 1.0f, 5, 10.0m, GeneralType.Bridge);

            CrewQuarters.Add(CrewQ);
            CrewQuarters.Add(CrewQS);
            FuelStorage.Add(FuelT);
            FuelStorage.Add(FuelTS);
            EngineeringSpaces.Add(EBay);
            OtherComponents.Add(Bridge);

            /// <summary>
            /// These components aren't really basic, but I'll put them in anyway.
            /// </summary>
            EngineDefTN EngDef = new EngineDefTN("25 EP Nuclear Thermal Engine", 5, 1.0f, 1.0f, 1.0f, 1, 5, -1.0f);
            ActiveSensorDefTN ActDef = new ActiveSensorDefTN("Search 5M - 5000", 1.0f, 10, 5, 100, false, 1.0f, 1);
            PassiveSensorDefTN ThPasDef = new PassiveSensorDefTN("Thermal Sensor TH1-5", 1.0f, 5, PassiveSensorType.Thermal, 1.0f, 1);
            PassiveSensorDefTN EMPasDef = new PassiveSensorDefTN("EM Sensor EM1-5", 1.0f, 5, PassiveSensorType.EM, 1.0f, 1);

            Engines.Add(EngDef);
            ActiveSensorDef.Add(ActDef);
            PassiveSensorDef.Add(ThPasDef);
            PassiveSensorDef.Add(EMPasDef);
        }
    }
}