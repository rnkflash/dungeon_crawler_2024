using grid.entities;
using grid.scene;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace grid.utils
{
    public class EntityMovement : MonoBehaviour
    {
        private Entity entity;
        private Tilemap tilemap => TilemapManager.Instance.tilemap;

        private Entity Entity => entity ??= GetComponent<Entity>();

        [Button]
        [ButtonGroup("sync")]
        private void SyncTilePosition()
        {
            var tilePos = tilemap.WorldToCell(transform.position);
            Entity.TilePosition = tilePos;
        }

        [Button]
        [ButtonGroup("sync")]
        private void SyncTransformPosition(bool undo = true)
        {
            var worldPos = tilemap.CellToWorld(Entity.TilePosition);
            var delta = tilemap.cellSize / 2f;
            transform.position = new Vector3(worldPos.x, 0f, worldPos.z) + new Vector3(delta.x, 0, delta.y);
        }

        [Button(ButtonSizes.Large)]
        [ButtonGroup("w")]
        public void MoveForward()
        {
            var delta = DirectionExtensions.DirectionToTileDelta[Entity.Rotation];
            Entity.TilePosition += new Vector3Int(delta.x, delta.y, 0);
            SyncTransformPosition(false);
        }

        [Button(ButtonSizes.Large)]
        [ButtonGroup("ad")]
        public void MoveLeft()
        {
            var delta = DirectionExtensions.DirectionToTileDelta[Entity.Rotation.Left()];
            Entity.TilePosition += new Vector3Int(delta.x, delta.y, 0);
            SyncTransformPosition(false);
        }

        [ButtonGroup("ad")]
        public void MoveRight()
        {
            var delta = DirectionExtensions.DirectionToTileDelta[Entity.Rotation.Right()];
            Entity.TilePosition += new Vector3Int(delta.x, delta.y, 0);
            SyncTransformPosition(false);
        }

        [Button(ButtonSizes.Large)]
        [ButtonGroup("s")]
        public void MoveBackward()
        {
            var delta = DirectionExtensions.DirectionToTileDelta[Entity.Rotation.Back()];
            Entity.TilePosition += new Vector3Int(delta.x, delta.y, 0);
            SyncTransformPosition(false);
        }
    }
}
