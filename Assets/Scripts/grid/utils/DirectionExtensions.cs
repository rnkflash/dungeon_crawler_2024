using System;
using System.Collections.Generic;
using grid.enums;
using UnityEngine;

namespace grid.utils
{
    public static class DirectionExtensions
    {
        public static int DirectionsCount = Enum.GetValues(typeof(Direction)).Length;
        
        public static Dictionary<Direction, Vector2Int> DirectionToTileDelta = new()
        {
            { Direction.N, new Vector2Int(0,1) },
            { Direction.NE, new Vector2Int(1,1) },
            { Direction.E, new Vector2Int(1,0) },
            { Direction.SE, new Vector2Int(1,-1) },
            { Direction.S, new Vector2Int(0,-1) },
            { Direction.SW, new Vector2Int(-1,-1) },
            { Direction.W, new Vector2Int(-1,0) },
            { Direction.NW, new Vector2Int(-1,1) }
        };
        
        public static Direction RotateDirection(this Direction currrentDirection, int delta)
        {
            var currentInt = (int)currrentDirection;
            delta %= DirectionsCount;
            var newInt = currentInt + delta;
            if (newInt < 0)
                newInt = newInt + DirectionsCount;
            else
                newInt = newInt % DirectionsCount;
            return (Direction)newInt;
        }

        public static Direction Back(this Direction rotation)
        {
            return rotation.RotateDirection(DirectionExtensions.DirectionsCount / 2);
        }

        public static Direction Left(this Direction rotation)
        {
            return rotation.RotateDirection(-2);
        }
        public static Direction Right(this Direction rotation)
        {
            return rotation.RotateDirection(2);
        }
    }
}