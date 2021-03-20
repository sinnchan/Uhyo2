using System;

namespace Main.Domain.Entities.Game
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
        ///     反対の駒を返します。
        ///     空白なら空白を返します。
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PieceState Opposite(this PieceState state)
        {
            return state switch
            {
                PieceState.Black => PieceState.White,
                PieceState.White => PieceState.Black,
                PieceState.Space => PieceState.Space,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }

        public static bool IsOppositeTo(this PieceState state, PieceState target)
        {
            return state != PieceState.Space && state != target;
        }
    }
}
