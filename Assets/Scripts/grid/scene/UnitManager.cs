using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using grid.entities.monsters;
using grid.entities.units;
using grid.ui.units;
using grid.utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace grid.scene
{
    public class UnitManager: Singleton<UnitManager>
    {
        public CinemachineBrain cinemachineBrain;
        [HideInInspector] public Marine activeMarine;

        private List<Marine> units = new();
        private List<Monster> enemies = new();

        public void Initialize()
        {
            cinemachineBrain.enabled = false;
            cinemachineBrain.m_DefaultBlend.m_Time = 0.0f;

            var monsters = EntityManager.Instance.GetAll<Monster>();
            AddEnemies(monsters);
            
            var marines = EntityManager.Instance.GetAll<Marine>();
            AddMarines(marines);
            
            if (units.Count > 0)
                ChooseUnit(units.First());
            
            cinemachineBrain.enabled = true;
        }

        public void AddEnemies(List<Monster> monsters)
        {
            foreach (var enemy in monsters)
            {
                enemies.Add(enemy);
                Monster.Initialize();
            }
        }
        
        public void AddMarines(List<Marine> marines)
        {
            foreach (var marine in marines)
            {
                units.Add(marine);
                marine.Initialize();
                UnitPanel.Instance.RegisterUnit(marine);
            }
        }

        public void ChooseUnit(Marine marine, bool fromUI = false)
        {
            if (activeMarine == marine) return;
            
            if (activeMarine != null)
                activeMarine.DeactivateHumanControl();
            
            activeMarine = marine;
            activeMarine.ActivateHumanControl();
            
            if (!fromUI)
                UnitPanel.Instance.SetActiveUnit(activeMarine);
        }

        public void NextActiveUnit()
        {
            if (units.Count <= 1) return;
            var index = units.FindIndex(i => i == activeMarine);
            var nextIndex = index + 1;
            if (nextIndex >= units.Count)
                nextIndex = 0;
            ChooseUnit(units[nextIndex]);
        }
    }
}