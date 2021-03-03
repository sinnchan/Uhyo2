using System.Collections;
using System.Collections.Generic;
using Src.Main.Domain.Entities.Game;
using Xunit;

namespace Src.Test.Domain.Entity.Game
{
    public class GameMasterTest
    {
        [Theory]
        [ClassData(typeof(TestBoardData))]
        public void GetSuggestPositionsTest(Board board, List<BoardPosition> expected)
        {
            var gameMaster = new GameMaster(board);
            var actual = gameMaster.GetSuggestPositions();

            //リストのもっとスマートなアサーション無いの…？
            Assert.Equal(expected.Count, actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.True(expected[i].IsEqualTo(actual[i]), $"expected{expected[i]} : actual{actual[i]}");
            }
        }
    }

    public class TestBoardData : IEnumerable<object[]>
    {
        List<object[]> _testData = new List<object[]>();
        public IEnumerator<object[]> GetEnumerator() => _testData.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

        public TestBoardData()
        {
            // 黒のターンで置ける場所テスト
            _testData.Add(new object[]
            {
                new Board(new[,]
                {
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, W, B, _, _, _},
                    {_, _, _, B, W, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _}
                }),
                new List<BoardPosition>
                {
                    new BoardPosition(3, 2),
                    new BoardPosition(2, 3),
                    new BoardPosition(5, 4),
                    new BoardPosition(4, 5)
                }
            });
            _testData.Add(new object[]
            {
                new Board(new[,]
                {
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, W, W, W, _, _, _},
                    {_, _, B, B, W, _, _, _},
                    {_, _, _, B, W, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _}
                }),
                new List<BoardPosition>
                {
                    new BoardPosition(1, 1),
                    new BoardPosition(2, 1),
                    new BoardPosition(3, 1),
                    new BoardPosition(4, 1),
                    new BoardPosition(5, 1),
                    new BoardPosition(5, 2),
                    new BoardPosition(5, 3),
                    new BoardPosition(5, 4),
                    new BoardPosition(5, 5)
                }
            });
            _testData.Add(new object[]
            {
                new Board(new[,]
                {
                    {B, _, W, _, _, _, _, _},
                    {B, _, _, W, B, B, _, _},
                    {B, W, B, B, W, B, W, _},
                    {_, B, W, W, W, B, W, W},
                    {W, W, B, W, W, W, _, _},
                    {_, W, B, _, W, _, _, _},
                    {_, _, W, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _}
                }),
                new List<BoardPosition>
                {
                    new BoardPosition(3, 0),
                    new BoardPosition(4, 0),
                    new BoardPosition(1, 1),
                    new BoardPosition(2, 1),
                    new BoardPosition(7, 1),
                    new BoardPosition(7, 2),
                    new BoardPosition(0, 3),
                    new BoardPosition(6, 4),
                    new BoardPosition(7, 4),
                    new BoardPosition(0, 5),
                    new BoardPosition(3, 5),
                    new BoardPosition(5, 5),
                    new BoardPosition(6, 5),
                    new BoardPosition(0, 6),
                    new BoardPosition(1, 6),
                    new BoardPosition(4, 6),
                    new BoardPosition(5, 6),
                    new BoardPosition(2, 7)
                }
            });

            // 白のターンで置ける場所テスト
            _testData.Add(new object[]
            {
                new Board(new[,]
                {
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, B, _, _, _, _, _},
                    {_, _, _, B, B, _, _, _},
                    {_, _, _, W, W, B, _, _},
                    {_, _, _, W, B, _, B, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _}
                }),
                new List<BoardPosition>
                {
                    new BoardPosition(1, 1),
                    new BoardPosition(3, 2),
                    new BoardPosition(4, 2),
                    new BoardPosition(5, 2),
                    new BoardPosition(6, 4),
                    new BoardPosition(5, 5),
                    new BoardPosition(4, 6),
                    new BoardPosition(5, 6)
                }
            });
            _testData.Add(new object[]
            {
                new Board(new[,]
                {
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, B, _, W, _, _, _},
                    {_, _, _, B, B, W, _, _},
                    {_, _, B, W, W, W, W, _},
                    {_, _, _, B, B, W, B, _},
                    {_, _, _, _, B, _, _, _},
                    {_, _, _, _, _, _, _, _}
                }),
                new List<BoardPosition>
                {
                    new BoardPosition(1, 1),
                    new BoardPosition(3, 2),
                    new BoardPosition(5, 2),
                    new BoardPosition(2, 3),
                    new BoardPosition(1, 4),
                    new BoardPosition(1, 5),
                    new BoardPosition(2, 5),
                    new BoardPosition(7, 5),
                    new BoardPosition(2, 6),
                    new BoardPosition(3, 6),
                    new BoardPosition(5, 6),
                    new BoardPosition(6, 6),
                    new BoardPosition(7, 6),
                    new BoardPosition(3, 7),
                    new BoardPosition(4, 7),
                }
            });
            _testData.Add(new object[]
            {
                new Board(new[,]
                {
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, B, _, _},
                    {_, B, B, _, B, B, W, W},
                    {B, B, B, B, B, W, W, W},
                    {B, B, W, B, W, W, W, W},
                    {B, B, B, W, W, B, W, W},
                    {W, B, W, W, B, W, W, W},
                    {B, B, W, W, W, W, W, W}
                }),
                new List<BoardPosition>
                {
                    new BoardPosition(4, 0),
                    new BoardPosition(5, 0),
                    new BoardPosition(6, 0),
                    new BoardPosition(0, 1),
                    new BoardPosition(1, 1),
                    new BoardPosition(2, 1),
                    new BoardPosition(3, 1),
                    new BoardPosition(4, 1),
                    new BoardPosition(0, 2),
                    new BoardPosition(3, 2),
                }
            });
        }
    }
}
