using System;

namespace Src.Main.Domain.Entities.Game
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
            switch (param)
            {
                case Direction.LeftTop:
                    return new Position(-1, -1);
                case Direction.CenterTop:
                    return new Position(0, -1);
                case Direction.RightTop:
                    return new Position(1, -1);
                case Direction.LeftCenter:
                    return new Position(-1, 0);
                case Direction.RightCenter:
                    return new Position(1, 0);
                case Direction.LeftBottom:
                    return new Position(-1, 1);
                case Direction.CenterBottom:
                    return new Position(0, 1);
                case Direction.RightBottom:
                    return new Position(1, 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(param), param, null);
            }
        }
    }
}
