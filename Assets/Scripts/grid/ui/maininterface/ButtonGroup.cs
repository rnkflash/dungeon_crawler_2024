using System;
using System.Collections.Generic;
using grid.entities;
using UnityEngine;

namespace grid.ui.maininterface
{

    public class EnemyButton : MonoBehaviour
    {
        private Entity entity;
        
        public void SetEntity(Entity entity)
        {
            this.entity = entity;
        }
    }
    
    public class EnemyButtonGroup: MonoBehaviour
    {
        public List<EnemyButton> buttons = new();
        public GameObject buttonPrefab;
        
        private void Awake()
        {
            Clear();
        }

        public void Clear()
        {
            foreach(Transform child in transform)
                Destroy(child.gameObject);
        }

        public void Init(List<Entity> enemies)
        {
            foreach (var entity in enemies)
            {
                var obj = Instantiate(buttonPrefab, transform);
                var enemyButton = obj.GetComponent<EnemyButton>();
                enemyButton.SetEntity(entity);
                buttons.Add(enemyButton);
            }
        }

    }
}