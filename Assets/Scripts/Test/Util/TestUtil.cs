using Scripts.Main.Domain.Entities.Game;

namespace Scripts.Test.Util
{
    public static class TestUtil
    {
        public static Piece B => Piece.CreateBlack();

        public static Piece W => Piece.CreateWhite();

        public static Piece _ => new Piece(PieceState.Space);
    }
}
