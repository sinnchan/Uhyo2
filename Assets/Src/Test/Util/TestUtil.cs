using Src.Main.Domain.Entities.Game;

namespace Src.Test
{
    public class TestUtil
    {
        public static Piece B => Piece.CreateBlack();

        public static Piece W => Piece.CreateWhite();

        public static Piece _ => new Piece(PieceState.Space);
    }
}
