using Src.Main.Domain.Entities.Game;

namespace Src.Test
{
    public class TestUtil
    {
        public static Piece B
        {
            get => Piece.CreateBlack();
        }

        public static Piece W
        {
            get => Piece.CreateWhite();
        }

        public static Piece _
        {
            get => new Piece(PieceState.Space);
        }
    }
}
