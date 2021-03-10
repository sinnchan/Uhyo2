using System;

namespace Src.Main.Domain.Entities.Game
{
    public enum PieceState
    {
        Black,
        White,
        Space
    }

    public static class PieceStateExtend
    {
        /// <summary>
        /// 反対の駒を返します。
        /// 空白なら空白を返します。
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PieceState Opposite(this PieceState state)
        {
            switch (state)
            {
                case PieceState.Black:
                    return PieceState.White;
                case PieceState.White:
                    return PieceState.Black;
                case PieceState.Space:
                    return PieceState.Space;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
