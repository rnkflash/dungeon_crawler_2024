using System.Linq;
using grid.entities;
using grid.entities.units;
using UnityEngine;

namespace grid.scene
{
    public class SceneInitializer: MonoBehaviour
    {
        private void Start()
        {
            var entities = FindObjectsOfType(typeof(Entity)).OfType<Entity>().ToList();
            EntityManager.Instance.RegisterEntities(entities);
            UnitManager.Instance.Initialize();
        }
    }
}