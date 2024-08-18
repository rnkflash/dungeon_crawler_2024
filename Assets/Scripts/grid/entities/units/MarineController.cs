using grid.enums;
using grid.scene;
using grid.utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace grid.entities.units
{
    public class MarineController : MonoBehaviour
    {
        private MarineSmoothMovementAndRotation marineMovementAndRotation;
        private Marine marine;
        private UnitActionsQueue actionsQueue;
        private Tilemap tilemap => TilemapManager.Instance.tilemap;

        private void Start()
        {
            marine = GetComponent<Marine>();
            marineMovementAndRotation = GetComponent<MarineSmoothMovementAndRotation>();
            actionsQueue = new UnitActionsQueue();
        }

        public void TurnLeft()
        {
            actionsQueue.Add(new UnitAction(
                () => true,
                onComplete => { marineMovementAndRotation.MoveCommand(MovementCommands.RotateLeft, onComplete); }
            )).Next();
        }

        public void TurnRight()
        {
            actionsQueue.Add(new UnitAction(
                () => true,
                onComplete => { marineMovementAndRotation.MoveCommand(MovementCommands.RotateRight, onComplete); }
            )).Next();
        }

        public void MoveForward()
        {
            actionsQueue.Add(new UnitAction(
                () => CheckTile(GetDirectionTile(marine.Rotation)),
                onComplete => { marineMovementAndRotation.MoveCommand(MovementCommands.MoveForward, onComplete); }
            )).Next();
        }

        public void MoveBackward()
        {
            actionsQueue.Add(new UnitAction(
                () => CheckTile(GetDirectionTile(marine.Rotation.Back())),
                onComplete => { marineMovementAndRotation.MoveCommand(MovementCommands.MoveBackward, onComplete); }
            )).Next();
        }

        public void MoveLeft()
        {
            actionsQueue.Add(new UnitAction(
                () => CheckTile(GetDirectionTile(marine.Rotation.Left())),
                onComplete => { marineMovementAndRotation.MoveCommand(MovementCommands.MoveLeft, onComplete); }
            )).Next();
        }

        public void MoveRight()
        {
            actionsQueue.Add(new UnitAction(
                () => CheckTile(GetDirectionTile(marine.Rotation.Right())),
                onComplete => { marineMovementAndRotation.MoveCommand(MovementCommands.MoveRight, onComplete); }
            )).Next();
        }

        private Vector3Int GetDirectionTile(Direction direction)
        {
            var delta = DirectionExtensions.DirectionToTileDelta[direction];
            return marine.TilePosition + new Vector3Int(delta.x, delta.y, 0);
        }

        private bool CheckTile(Vector3Int tilePos)
        {
            var walkableMap = tilemap;
            return walkableMap.HasTile(tilePos);
        }
    }
}