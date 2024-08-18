using System.Collections.Generic;
using System.Linq;
using grid.entities;
using grid.utils;

namespace grid.scene
{
    public class EntityManager: Singleton<EntityManager>
    {
        private List<Entity> entities = new ();

        public void RegisterEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void UnregisterEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        public void RegisterEntities<T>(List<T> entities) where T : Entity
        {
            foreach (var entity in entities)
            {
                RegisterEntity(entity);
            }
        }

        public List<T> GetAll<T>() where T : Entity
        {
            return entities.OfType<T>().ToList();
        }
    }
}