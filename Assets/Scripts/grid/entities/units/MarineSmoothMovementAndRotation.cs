using System;
using DG.Tweening;
using grid.enums;
using grid.scene;
using grid.utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace grid.entities.units
{
    public class MarineSmoothMovementAndRotation: MonoBehaviour
    {
        private const float Duration = 0.20f;
        private Marine marine;
        private Tween tween;
        private Tilemap tilemap => TilemapManager.Instance.tilemap;

        private void Start()
        {
            marine = GetComponent<Marine>();
        }

        private Vector3 GetUnitPosition(Vector3Int tilePosition)
        {
            var worldPos = tilemap.CellToWorld(tilePosition);
            var delta = tilemap.cellSize / 2f;
            return new Vector3(worldPos.x, 0f, worldPos.z) + new Vector3(delta.x, 0, delta.y);
        }

        public void MoveCommand(MovementCommands type, Action onComplete = null)
        {
            switch (type)
            {
                case MovementCommands.RotateLeft:
                    TweenRotate(marine.Rotation, -1, onComplete);
                    break;
                case MovementCommands.RotateRight:
                    TweenRotate(marine.Rotation, 1, onComplete);
                    break;
                case MovementCommands.MoveForward:
                    TweenMove(marine.TilePosition, marine.Rotation, onComplete);
                    break;
                case MovementCommands.MoveLeft:
                    TweenMove(marine.TilePosition, marine.Rotation.Left(), onComplete);
                    break;
                case MovementCommands.MoveRight:
                    TweenMove(marine.TilePosition, marine.Rotation.Right(), onComplete);
                    break;
                case MovementCommands.MoveBackward:
                    TweenMove(marine.TilePosition, marine.Rotation.Back(), onComplete);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        private void SyncTilePosition(Vector3Int tilePosition)
        {
            marine.TilePosition = tilePosition;
        }

        private void SyncRotation(Direction direction)
        {
            marine.Rotation = direction;
        }
        
        private Quaternion GetRotation(Direction direction)
        {
            var rotationDegrees = (int)direction * 45f;
            return Quaternion.Euler(0f, rotationDegrees, 0f);
        }

        private void TweenMove(Vector3Int fromTilePos, Direction direction, Action onComplete = null)
        {
            if (tween != null)
            {
                tween.Complete();
                tween = null;
            }
            
            var delta = DirectionExtensions.DirectionToTileDelta[direction];
            var tileDestination = fromTilePos + new Vector3Int(delta.x, delta.y, 0);
            var worldStart = GetUnitPosition(fromTilePos);
            var worldDestination = GetUnitPosition(tileDestination);
            
            SyncTilePosition(tileDestination);

            transform.position = worldStart;
            tween = transform.DOMove(worldDestination, Duration).OnComplete(() =>
            {
                tween = null;
                onComplete?.Invoke();
            });
        }

        private void TweenRotate(Direction from, int delta, Action onComplete = null)
        {
            if (tween != null)
            {
                tween.Complete();
                tween = null;
            }
            
            var destinationRotation = from.RotateDirection(delta);
            var destinationEuler = GetRotation(destinationRotation).eulerAngles;

            SyncRotation(destinationRotation);
            
            transform.rotation = GetRotation(from);
            transform.DORotate(destinationEuler, Duration).OnComplete(() =>
            {
                tween = null;
                onComplete?.Invoke();
            });
        }
    }
}