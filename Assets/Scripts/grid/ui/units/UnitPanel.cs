using System.Collections.Generic;
using grid.entities.units;
using grid.scene;
using grid.sound;
using grid.utils;
using UnityEngine;

namespace grid.ui.units
{
    public class UnitPanel: Singleton<UnitPanel>
    {
        [SerializeField] private Transform verticalLayoutGroup;
        [SerializeField] private GameObject unitPanelPrefab;
        private List<SoldierPanel> soldiers = new ();

        protected override void Awake()
        {
            foreach(Transform child in transform)
                Destroy(child.gameObject);
        }

        public void RegisterUnit(Marine marine)
        {
            var obj = Instantiate(unitPanelPrefab, verticalLayoutGroup);
            var panel = obj.GetComponent<SoldierPanel>();
            soldiers.Add(panel);
            panel.Initialize(marine);
        }

        public void SoldierPanelClicked(SoldierPanel soldierPanel)
        {
            foreach (var soldier in soldiers)
            {
                if (soldier != soldierPanel)
                    soldier.SetActive(false);
                else
                {
                    soldier.SetActive(true);
                    UnitManager.Instance.ChooseUnit(soldier.GetUnit(), true);
                    SoundManager.Instance.PlayRandomMarineSound();
                }
            }
        }

        public void SetActiveUnit(Marine activeMarine)
        {
            foreach (var soldier in soldiers)
            {
                if (soldier.GetUnit() == activeMarine)
                {
                    soldier.SetActive(true);
                    SoundManager.Instance.PlayRandomMarineSound();
                }
                else
                {
                    soldier.SetActive(false);
                }
            }
        }
    }
}