using System;

namespace Src.Main.Domain.Entities.Game
{
    public class Position
    {
        public const int Max = 8;

        public Position(int x, int y)
        {
            if (CheckArg(x) && CheckArg(y))
            {
                X = x;
                Y = y;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public int X { get; }
        public int Y { get; }

        private static bool CheckArg(int value)
        {
            return 0 < value && value <= Max;
        }
    }
}
