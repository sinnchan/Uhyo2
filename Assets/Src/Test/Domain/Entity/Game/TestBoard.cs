using System;
using NUnit.Framework;
using Src.Main.Domain.Entities.Game;

namespace Src.Test.Domain.Entity.Game
{
    [TestFixture]
    public class TestBoard
    {
        [Test]
        public void IsEmptyTest()
        {
            var board = new Board();
            Assert.AreEqual(board.IsEmpty(), true);
            board.PlacePiece(new Piece(PieceColor.Black), new Position(1, 1));
            Assert.AreEqual(board.IsEmpty(), false);
            board.Clear();
            Assert.AreEqual(board.IsEmpty(), true);
        }

        [TestCase(-1, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(9, 9)]
        public void PlacePieceErrorTest(int x, int y)
        {
            var board = new Board();

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                board.PlacePiece(new Piece(PieceColor.Black), new Position(x, y));
            });
        }

        [TestCase(1, 1)]
        [TestCase(8, 1)]
        [TestCase(1, 8)]
        [TestCase(8, 8)]
        [TestCase(2, 2)]
        public void PlacePieceTest(int x, int y)
        {
            var board = new Board();
            board.PlacePiece(new Piece(PieceColor.Black), new Position(x, y));
        }

        [Test]
        public void IsFullTest()
        {
            var board = new Board();
            Assert.AreEqual(board.IsFull(), false);
            Board.LoopAccessAll((x, y) => { board.Data[y, x] = Piece.CreateBlack(); });
            Assert.AreEqual(board.IsFull(), true);
        }
    }
}
