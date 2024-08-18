using grid.entities;
using grid.enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace grid.utils
{
    public class EntityRotation : MonoBehaviour
    {
        private Entity entity;
        private Entity Entity => entity ??= GetComponent<Entity>();
        
        private void Start()
        {
            SetTransformRotation(transform, entity.Rotation);
        }

        [Button]
        private void PrintCurrentRotation()
        {
            Debug.Log($"rotationInt: {(int)Entity.Rotation} direction:{Entity.Rotation.ToString()}");
        }
        
        [Button]
        private void SyncCurrentRotation()
        {
            SetTransformRotation(transform, Entity.Rotation);
        }

        [Button(ButtonSizes.Large)]
        [ButtonGroup("qe")]
        public void RotateLeft()
        {
            var delta = -1;
            Entity.Rotation = Entity.Rotation.RotateDirection(delta);
            SetTransformRotation(transform, Entity.Rotation);
        }

        [ButtonGroup("qe")]
        public void RotateRight()
        {
            var delta = 1;
            Entity.Rotation = Entity.Rotation.RotateDirection(delta);
            SetTransformRotation(transform, Entity.Rotation);
        }

        private void SetTransformRotation(Transform transform, Direction direction)
        {
            var rotationDegrees = (int)direction * 45f;
            transform.rotation = Quaternion.Euler(0f, rotationDegrees, 0f);
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            SyncCurrentRotation();
        }
        #endif
    }
}
