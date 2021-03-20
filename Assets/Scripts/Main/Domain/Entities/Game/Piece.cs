using System;

namespace Main.Domain.Entities.Game
{
    public class Piece
    {
        public Piece(PieceState pieceState)
        {
            State = pieceState;
        }

        public PieceState State { get; private set; }

        /// <summary>
        ///     白のコマを作成します。
        /// </summary>
        /// <returns></returns>
        public static Piece CreateWhite()
        {
            return new Piece(PieceState.White);
        }

        /// <summary>
        ///     黒のコマを作成します。
        /// </summary>
        /// <returns></returns>
        public static Piece CreateBlack()
        {
            return new Piece(PieceState.Black);
        }

        /// <summary>
        ///     コマをひっくり返す
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void TurnOver()
        {
            switch (State)
            {
                case PieceState.Black:
                    State = PieceState.White;
                    break;
                case PieceState.White:
                    State = PieceState.Black;
                    break;
                case PieceState.Space:
                    // do nothing...
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Pieceを空白に転生させます。
        /// </summary>
        public void Clear()
        {
            State = PieceState.Space;
        }

        public override string ToString()
        {
            return State switch
            {
                PieceState.Black => "●",
                PieceState.White => "○",
                PieceState.Space => ".",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
