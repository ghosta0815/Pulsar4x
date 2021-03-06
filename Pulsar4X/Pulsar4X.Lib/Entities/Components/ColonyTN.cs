﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar4X.Entities.Components
{
    public class ColonyDefTN : ComponentDefTN
    {
        /// <summary>
        /// The number of colonists,pows, or rescuees that this component can hold.
        /// </summary>
        private int CryoBerths;
        public int cryoBerths
        {
            get { return CryoBerths; }
        }

        /// <summary>
        /// ColonydefTN defines the cryo storage components.
        /// </summary>
        /// <param name="Title">Name.</param>
        /// <param name="Size">Size in HS.</param>
        /// <param name="ComponentCost">Cost of the component.</param>
        /// <param name="CrewRequirement">Crew requirement of the component.</param>
        public ColonyDefTN(string Title, float Size, decimal ComponentCost, byte CrewRequirement)
        {
            Id = Guid.NewGuid();

            componentType = ComponentTypeTN.CryoStorage;

            Name = Title;
            size = Size;
            cost = ComponentCost;
            crew = CrewRequirement;

            minerialsCost = new decimal[Constants.Minerals.NO_OF_MINERIALS];
            for (int mineralIterator = 0; mineralIterator < (int)Constants.Minerals.MinerialNames.MinerialCount; mineralIterator++)
            {
                minerialsCost[mineralIterator] = 0;
            }
            minerialsCost[(int)Constants.Minerals.MinerialNames.Duranium] = cost * 0.25m;
            minerialsCost[(int)Constants.Minerals.MinerialNames.Mercassium] = cost * 0.75m;

            htk = 1;


            /// <summary>
            /// Cryoberths are 4 per ton, and size is in HS which is 50 tons.
            /// </summary>
            CryoBerths = (int)(size * Constants.ShipTN.TonsPerHS * 4.0f);

            isSalvaged = false;
            isObsolete = false;
            isMilitary = false;
            isDivisible = false;
            isElectronic = false;
        }
    }

    public class ColonyTN : ComponentTN
    {
        /// <summary>
        /// pointer to this colony component's definition.
        /// </summary>
        private ColonyDefTN ColonyDef;
        public ColonyDefTN colonyDef
        {
            get { return ColonyDef; }
        }

        /// <summary>
        /// Constructor for the actual component itself, not merely its definition.
        /// </summary>
        /// <param name="definition">The definition of this component which will contain information describing its workings.</param>
        public ColonyTN(ColonyDefTN definition)
        {
            ColonyDef = definition;

            Name = definition.Name;

            isDestroyed = false;
        }


    }
}
