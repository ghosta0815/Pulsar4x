﻿using Pulsar4X.ECSLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Pulsar4X.ViewModel
{
    public class ColonyScreenVM : IViewModel
    {
        private Entity _colonyEntity;
        private ColonyInfoDB ColonyInfo { get { return _colonyEntity.GetDataBlob<ColonyInfoDB>(); } }
        private Entity FactionEntity { get { return _colonyEntity.GetDataBlob<OwnedDB>().ObjectOwner; } }

        private ObservableCollection<FacilityVM> _facilities;
        public ObservableCollection<FacilityVM> Facilities
        {
            get { return _facilities; }
        }

        private readonly ObservableDictionary<string, long> _species = new ObservableDictionary<string, long>();
        public ObservableDictionary<string, long> Species { get { return _species; } }

        public PlanetMineralDepositVM PlanetMineralDepositVM { get; set; }
        public RawMineralStockpileVM RawMineralStockpileVM { get; set; }
        public RefinedMatsStockpileVM RefinedMatsStockpileVM { get; set; }

        public RefineryAbilityVM RefineryAbilityVM { get; set; }
        public ConstructionAbilityVM ConstructionAbilityVM { get; set; }

        public ColonyResearchVM ColonyResearchVM { get; set; }





        public string ColonyName
        {
            get { return _colonyEntity.GetDataBlob<NameDB>().GetName(FactionEntity); }
            set
            {
                _colonyEntity.GetDataBlob<NameDB>().SetName(FactionEntity, value);
                OnPropertyChanged();
            }
        }


        public ColonyScreenVM(GameVM gameVM, Entity colonyEntity, StaticDataStore staticData)
        {

            gameVM.DateChangedEvent += GameVM_DateChangedEvent;
            _colonyEntity = colonyEntity;
            _facilities = new ObservableCollection<FacilityVM>();
            ComponentInstancesDB instaces = colonyEntity.GetDataBlob<ComponentInstancesDB>();
            foreach (var installation in instaces.SpecificInstances)
            {
                Facilities.Add(new FacilityVM(installation.Key, instaces));
            }


            UpdatePop();




            PlanetMineralDepositVM = new PlanetMineralDepositVM(staticData, _colonyEntity.GetDataBlob<ColonyInfoDB>().PlanetEntity);

            RawMineralStockpileVM = new RawMineralStockpileVM(staticData, _colonyEntity);

            RefinedMatsStockpileVM = new RefinedMatsStockpileVM(staticData, _colonyEntity);
            
            RefineryAbilityVM = new RefineryAbilityVM(staticData, _colonyEntity);

            ConstructionAbilityVM = new ConstructionAbilityVM(staticData, _colonyEntity);

            ColonyResearchVM = new ColonyResearchVM(staticData, _colonyEntity);
        }

        private void UpdatePop()
        {
            _species.Clear();
            foreach (var kvp in ColonyInfo.Population)
            {
                string name = kvp.Key.GetDataBlob<NameDB>().DefaultName;
                _species.Add(name, kvp.Value);
            }
        }

        private void GameVM_DateChangedEvent(DateTime oldDate, DateTime newDate)
        {
            Refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Refresh(bool partialRefresh = false)
        {
            PlanetMineralDepositVM.Refresh();
            RawMineralStockpileVM.Refresh();
            RefinedMatsStockpileVM.Refresh();
            RefineryAbilityVM.Refresh();
            ConstructionAbilityVM.Refresh();
            UpdatePop();
            foreach (var facilityvm in Facilities)
            {
                facilityvm.Refresh();
            }
        }
    }

    
    public class PlanetMineralDepositVM : IViewModel
    {
        private Entity _planetEntity;
        private SystemBodyDB systemBodyInfo { get { return _planetEntity.GetDataBlob<SystemBodyDB>(); } }
        private StaticDataStore _staticData;
        private readonly ObservableDictionary<Guid, PlanetMineralInfoVM> _mineralDeposits = new ObservableDictionary<Guid, PlanetMineralInfoVM>();
        public ObservableDictionary<Guid, PlanetMineralInfoVM> MineralDeposits { get { return _mineralDeposits; } }



        public PlanetMineralDepositVM(StaticDataStore staticData, Entity planetEntity)
        {
            _planetEntity = planetEntity;
            _staticData = staticData;
            Initialise();
        }

        private void Initialise()
        {
            var minerals = systemBodyInfo.Minerals;
            _mineralDeposits.Clear();
            foreach (var kvp in minerals)
            {
                MineralSD mineral = _staticData.Minerals[kvp.Key];
                if (!_mineralDeposits.ContainsKey(kvp.Key))
                    _mineralDeposits.Add(kvp.Key, new PlanetMineralInfoVM(mineral.Name, kvp.Value));
            }
            OnPropertyChanged(nameof(MineralDeposits));

        }




        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Refresh(bool partialRefresh = false)
        {
            if (systemBodyInfo.Minerals.Count != MineralDeposits.Count)
                Initialise();
            else
                foreach (var mineralvm in MineralDeposits.Values)
                {
                    mineralvm.Refresh();
                }
            OnPropertyChanged();
        }
    }

    public class PlanetMineralInfoVM : IViewModel
    {
        private MineralDepositInfo _mineralDepositInfo;

        public string Mineral { get; private set; }
        public int Amount { get { return _mineralDepositInfo.Amount; } }
        public double Accessability { get { return _mineralDepositInfo.Accessibility; } }

        public PlanetMineralInfoVM(string name, MineralDepositInfo deposit)
        {
            Mineral = name;
            _mineralDepositInfo = deposit;           
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Refresh(bool partialRefresh = false)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Amount)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Accessability)));

            }
        }
    }


    public class RawMineralStockpileVM : IViewModel
    {
        private Entity _colonyEntity;
        private ColonyInfoDB ColonyInfo { get { return _colonyEntity.GetDataBlob<ColonyInfoDB>(); } }
        private Dictionary<Guid, MineralSD> _mineralDictionary = new Dictionary<Guid, MineralSD>();

        private readonly ObservableDictionary<Guid, RawMineralInfoVM> _mineralStockpile = new ObservableDictionary<Guid, RawMineralInfoVM>();
        public ObservableDictionary<Guid, RawMineralInfoVM> MineralStockpile
        {
            get { return _mineralStockpile; }
        }

        public RawMineralStockpileVM(StaticDataStore staticData, Entity colonyEntity)
        {

            foreach (var mineral in staticData.Minerals.Values)
            {
                _mineralDictionary.Add(mineral.ID, mineral);
            }
            _colonyEntity = colonyEntity;
            Initialise();
        }

        private void Initialise()
        {
            var rawMinerals = ColonyInfo.MineralStockpile;
            _mineralStockpile.Clear();
            foreach (var kvp in rawMinerals)
            {
                MineralSD mineral = _mineralDictionary[kvp.Key];
                if(!MineralStockpile.ContainsKey(kvp.Key))
                    _mineralStockpile.Add(kvp.Key, new RawMineralInfoVM(kvp.Key, mineral.Name, ColonyInfo));             
            }
            OnPropertyChanged(nameof(MineralStockpile));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Refresh(bool partialRefresh = false)
        {
            if (ColonyInfo.MineralStockpile.Count != MineralStockpile.Count)
                Initialise();
            else
                foreach (var mineral in MineralStockpile)
                {
                    mineral.Value.Refresh();
                    
                }
            OnPropertyChanged();
        }
    }
    public class RawMineralInfoVM : IViewModel
    {
        private ColonyInfoDB _colonyInfo;
        private Guid _guid;
        public string Mineral { get; private set; }
        public int Amount { get { return _colonyInfo.MineralStockpile[_guid]; } }

        public RawMineralInfoVM(Guid guid, string name, ColonyInfoDB colonyInfo)
        {
            _guid = guid;
            Mineral = name;
            _colonyInfo = colonyInfo;           
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Refresh(bool partialRefresh = false)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
        }
    }

    public class RefinedMatsStockpileVM : IViewModel
    {

        private Entity _colonyEntity;
        private ColonyInfoDB ColonyInfo { get { return _colonyEntity.GetDataBlob<ColonyInfoDB>(); } }
        private Dictionary<Guid, RefinedMaterialSD> _materialsDictionary;


        private readonly ObservableDictionary<Guid, RefinedMatInfoVM> _materialStockpile = new ObservableDictionary<Guid, RefinedMatInfoVM>();
        public ObservableDictionary<Guid, RefinedMatInfoVM> MaterialStockpile
        {
            get { return _materialStockpile; }
        }


        public RefinedMatsStockpileVM(StaticDataStore staticData, Entity colonyEntity)
        {
            _materialsDictionary = staticData.RefinedMaterials;
            _colonyEntity = colonyEntity;
            Initialise();
        }

        private void Initialise()
        {
            var mats = _colonyEntity.GetDataBlob<ColonyInfoDB>().RefinedStockpile;            
            foreach (var kvp in mats)
            {
                RefinedMaterialSD mat = _materialsDictionary[kvp.Key];
                if(!_materialStockpile.ContainsKey(kvp.Key))
                    _materialStockpile.Add(kvp.Key, new RefinedMatInfoVM(kvp.Key, mat.Name, ColonyInfo));
                OnPropertyChanged();
            }
           
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Refresh(bool partialRefresh = false)
        {
            if (ColonyInfo.MineralStockpile.Count != MaterialStockpile.Count)
                Initialise();
            else
                foreach (var item in MaterialStockpile)
                {
                    item.Value.Refresh();
                }
            OnPropertyChanged();
        }
    }
    public class RefinedMatInfoVM : IViewModel
    {
        private ColonyInfoDB _colonyInfo;
        private Guid _guid;
        public string Material { get; private set; }
        public int Amount { get { return _colonyInfo.RefinedStockpile[_guid]; } }

        public RefinedMatInfoVM(Guid guid, string name, ColonyInfoDB colonyInfo)
        {
            _guid = guid;
            Material = name;
            _colonyInfo = colonyInfo;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Refresh(bool partialRefresh = false)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
        }
    }


    public class FacilityVM : IViewModel
    {
        private Entity _facilityEntity;
        //private ColonyInfoDB _colonyInfo;
        private ComponentInstancesDB _componentInstancesDB;

        public string Name { get { return _facilityEntity.GetDataBlob<NameDB>().DefaultName; } }
        public int Count
        {
            get { return _componentInstancesDB.SpecificInstances[_facilityEntity].Count; }//_colonyInfo.Installations[_facilityEntity];}
        }
        public int WorkersRequired { get { return _facilityEntity.GetDataBlob<ComponentInfoDB>().CrewRequrements * Count; } }

        public FacilityVM()
        {
        }

        public FacilityVM(Entity facilityEntity, ComponentInstancesDB componentInstances)
        {
            _facilityEntity = facilityEntity;
            //_colonyInfo = colonyInfo;
            _componentInstancesDB = componentInstances;
            Refresh();

        }
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void Refresh(bool partialRefresh = false)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
        }
    }
}
