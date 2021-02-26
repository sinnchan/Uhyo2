using System;

namespace Src.Main.Domain.Entities.Game
{
    public class Piece
    {
        public Piece(PieceColor color)
        {
            Color = color;
        }

        public PieceColor Color { get; private set; }

        /// <summary>
        ///     白のコマを作成します。
        /// </summary>
        /// <returns></returns>
        public static Piece CreateWhite()
        {
            return new Piece(PieceColor.White);
        }

        /// <summary>
        ///     黒のコマを作成します。
        /// </summary>
        /// <returns></returns>
        public static Piece CreateBlack()
        {
            return new Piece(PieceColor.Black);
        }

        /// <summary>
        ///     コマをひっくり返す
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void TurnOver()
        {
            switch (Color)
            {
                case PieceColor.Black:
                    Color = PieceColor.White;
                    break;
                case PieceColor.White:
                    Color = PieceColor.Black;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
