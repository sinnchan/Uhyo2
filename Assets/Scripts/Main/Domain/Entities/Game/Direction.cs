using System;

namespace Main.Domain.Entities.Game
{
    public enum Direction
    {
        LeftTop,
        CenterTop,
        RightTop,
        LeftCenter,
        RightCenter,
        LeftBottom,
        CenterBottom,
        RightBottom
    }

    public static class DirectionExtend
    {
        public static Position GetDirectionValue(this Direction param)
        {
            return param switch
            {
                Direction.LeftTop => new Position(-1, -1),
                Direction.CenterTop => new Position(0, -1),
                Direction.RightTop => new Position(1, -1),
                Direction.LeftCenter => new Position(-1, 0),
                Direction.RightCenter => new Position(1, 0),
                Direction.LeftBottom => new Position(-1, 1),
                Direction.CenterBottom => new Position(0, 1),
                Direction.RightBottom => new Position(1, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(param), param, null)
            };
        }
    }
}
