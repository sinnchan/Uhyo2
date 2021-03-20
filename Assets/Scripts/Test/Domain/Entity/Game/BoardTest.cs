using System;
using Main.Domain.Entities.Game;
using Xunit;

namespace Test.Domain.Entity.Game
{
    public class TestBoard
    {
        [Fact]
        public void IsEmptyTest()
        {
            var board = new Board();
            Assert.True(board.IsEmpty());
            board.PlacePiece(new Piece(PieceState.Black), new BoardPosition(1, 1));
            Assert.False(board.IsEmpty());
            board.Clear();
            Assert.True(board.IsEmpty());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(8, 1)]
        [InlineData(1, 8)]
        [InlineData(8, 8)]
        [InlineData(2, 2)]
        public void PlacePieceTest(int x, int y)
        {
            var board = new Board();
            board.PlacePiece(new Piece(PieceState.Black), new BoardPosition(x, y));
        }

        [Fact]
        public void IsFullTest()
        {
            var board = new Board();
            Assert.False(board.IsFull());
            Board.LoopAccessAll(position => { board.PlacePiece(Piece.CreateBlack(), position); });
            Assert.True(board.IsFull());
        }

        [Fact]
        public void CountPieceTest()
        {
            var board = new Board();
            Assert.Equal(Math.Pow(Board.Length, 2), board.Count(PieceState.Space));
        }
    }
}
