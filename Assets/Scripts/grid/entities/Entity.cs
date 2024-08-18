using grid.enums;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace grid.entities
{
    public abstract class Entity: MonoBehaviour
    {
        [SerializeField] protected Vector3Int tilePosition;
        [SerializeField] protected Direction rotation;
        
        public Direction Rotation
        {
            get => rotation;
            set
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Unit.Rotation");
#endif
                rotation = value;
            }
        }

        public Vector3Int TilePosition
        {
            get => tilePosition;
            set
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Unit.TilePosition");
#endif
                tilePosition = value;
            }
        }
    }
}