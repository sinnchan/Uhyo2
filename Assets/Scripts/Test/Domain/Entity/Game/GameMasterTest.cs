using System.Collections.Generic;
using System.Linq;
using Main.Domain.Entities.Game;
using Test.Util;
using Xunit;
using static Test.Util.TestUtil;

namespace Scripts.Test.Domain.Entity.Game
{
    public class GameMasterTest
    {
        [Theory]
        [ClassData(typeof(GetSuggestPositionsTestData))]
        public void GetSuggestPositionsTest(PieceState turn, Board board, List<BoardPosition> expected)
        {
            var gameMaster = new GameMaster(board, turn);
            var actual = gameMaster.GetSuggestPositions(turn);

            //リストのもっとスマートなアサーション無いの…？
            Assert.Equal(expected.Count, actual.Count);
            for (var i = 0; i < expected.Count; i++)
                Assert.True(expected[i].IsEqualTo(actual[i]), $"expected{expected[i]} : actual{actual[i]}");
        }

        [Theory]
        [ClassData(typeof(PlaceTestData))]
        public void PlaceTest(
            PieceState turn,
            Piece piece,
            BoardPosition placePosition,
            Board testBoard,
            Board expectedBoard,
            List<BoardPosition> expectedPositions)
        {
            var gameMaster = new GameMaster(testBoard, turn);
            var placeResult = gameMaster.Place(piece, placePosition);
            var actual = gameMaster.GetBoardData();

            Assert.True(placeResult.valid);
            Assert.Equal(expectedPositions.Count, placeResult.data.Count);
            foreach (var boardPosition in placeResult.data)
                Assert.True(expectedPositions.Exists(position => position.IsEqualTo(boardPosition)));

            Assert.Equal(expectedBoard.VisualizeData(), actual.VisualizeData());
        }

        [Theory]
        [ClassData(typeof(PlaceErrorTestData))]
        public void PlaceErrorTest(
            PieceState turn,
            Piece piece,
            BoardPosition placePosition,
            Board testBoard)
        {
            var gameMaster = new GameMaster(testBoard, turn);
            var placeResult = gameMaster.Place(piece, placePosition);
            var actual = gameMaster.GetBoardData();

            Assert.False(placeResult.valid);
            Assert.Null(placeResult.data);
            Assert.Equal(testBoard.VisualizeData(), actual.VisualizeData());
        }

        /// <summary>
        ///     最短全白テスト
        /// </summary>
        [Theory]
        [ClassData(typeof(GamePlayTestData))]
        public void PlayTest1(List<(Piece piece, BoardPosition position)> gameProgressList)
        {
            var gameMaster = new GameMaster();
            foreach (var progress in gameProgressList)
            {
                var result = gameMaster.Place(progress.piece, progress.position);
                Assert.True(result.valid, progress.ToString());
                if (progress != gameProgressList.Last())
                    Assert.False(gameMaster.GameEndFlag);
                else
                    Assert.True(gameMaster.GameEndFlag);
            }
        }
    }

    public class GetSuggestPositionsTestData : AbstractTestData
    {
        public GetSuggestPositionsTestData()
        {
            // 黒のターンで置ける場所テスト
            testData.Add(new object[]
            {
                PieceState.Black,
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
            testData.Add(new object[]
            {
                PieceState.Black,
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
            testData.Add(new object[]
            {
                PieceState.Black,
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
            testData.Add(new object[]
            {
                PieceState.White,
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
            testData.Add(new object[]
            {
                PieceState.White,
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
                    new BoardPosition(4, 7)
                }
            });
            testData.Add(new object[]
            {
                PieceState.White,
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
                    new BoardPosition(3, 2)
                }
            });
        }
    }

    public class PlaceTestData : AbstractTestData
    {
        public PlaceTestData()
        {
            testData.Add(new object[]
            {
                PieceState.Black,
                Piece.CreateBlack(),
                new BoardPosition(3, 2),
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
                new Board(new[,]
                {
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, B, _, _, _, _},
                    {_, _, _, B, B, _, _, _},
                    {_, _, _, B, W, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _}
                }),
                new List<BoardPosition>
                {
                    new BoardPosition(3, 3)
                }
            });
            testData.Add(new object[]
            {
                PieceState.Black,
                Piece.CreateBlack(),
                new BoardPosition(0, 5),
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
                new Board(new[,]
                {
                    {B, _, W, _, _, _, _, _},
                    {B, _, _, W, B, B, _, _},
                    {B, W, B, B, W, B, W, _},
                    {_, B, B, W, W, B, W, W},
                    {W, B, B, W, W, W, _, _},
                    {B, B, B, _, W, _, _, _},
                    {_, _, W, _, _, _, _, _},
                    {_, _, _, _, _, _, _, _}
                }),
                new List<BoardPosition>
                {
                    new BoardPosition(2, 3),
                    new BoardPosition(1, 4),
                    new BoardPosition(1, 5)
                }
            });
            testData.Add(new object[]
            {
                PieceState.White,
                Piece.CreateWhite(),
                new BoardPosition(3, 2),
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
                new Board(new[,]
                {
                    {_, _, _, _, _, _, _, _},
                    {_, _, _, _, _, B, _, _},
                    {_, B, B, W, W, W, W, W},
                    {B, B, B, W, W, W, W, W},
                    {B, B, W, W, W, W, W, W},
                    {B, B, B, W, W, B, W, W},
                    {W, B, W, W, B, W, W, W},
                    {B, B, W, W, W, W, W, W}
                }),
                new List<BoardPosition>
                {
                    new BoardPosition(4, 2),
                    new BoardPosition(5, 2),
                    new BoardPosition(4, 3),
                    new BoardPosition(3, 3),
                    new BoardPosition(3, 4)
                }
            });
        }
    }

    public class PlaceErrorTestData : AbstractTestData
    {
        public PlaceErrorTestData()
        {
            testData.Add(new object[]
            {
                PieceState.Black,
                Piece.CreateWhite(),
                new BoardPosition(3, 2),
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
                })
            });
            testData.Add(new object[]
            {
                PieceState.Black,
                Piece.CreateBlack(),
                new BoardPosition(2, 2),
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
                })
            });
            testData.Add(new object[]
            {
                PieceState.Black,
                Piece.CreateBlack(),
                new BoardPosition(5, 3),
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
                })
            });
        }
    }

    public class GamePlayTestData : AbstractTestData
    {
        public GamePlayTestData()
        {
            testData.Add(new object[]
            {
                // 全部白で終了
                new List<(Piece piece, BoardPosition position)>
                {
                    (Piece.CreateBlack(), new BoardPosition(5, 4)),
                    (Piece.CreateWhite(), new BoardPosition(5, 5)),
                    (Piece.CreateBlack(), new BoardPosition(4, 5)),
                    (Piece.CreateWhite(), new BoardPosition(5, 3)),
                    (Piece.CreateBlack(), new BoardPosition(4, 2)),
                    (Piece.CreateWhite(), new BoardPosition(3, 1)),
                    (Piece.CreateBlack(), new BoardPosition(3, 2)),
                    (Piece.CreateWhite(), new BoardPosition(3, 5)),
                    (Piece.CreateBlack(), new BoardPosition(2, 3)),
                    (Piece.CreateWhite(), new BoardPosition(1, 3))
                }
            });
            testData.Add(new object[]
            {
                // 全部黒で終了
                new List<(Piece piece, BoardPosition position)>
                {
                    (Piece.CreateBlack(), new BoardPosition(5, 4)),
                    (Piece.CreateWhite(), new BoardPosition(3, 5)),
                    (Piece.CreateBlack(), new BoardPosition(2, 4)),
                    (Piece.CreateWhite(), new BoardPosition(5, 3)),
                    (Piece.CreateBlack(), new BoardPosition(4, 6)),
                    (Piece.CreateWhite(), new BoardPosition(5, 5)),
                    (Piece.CreateBlack(), new BoardPosition(6, 4)),
                    (Piece.CreateWhite(), new BoardPosition(4, 5)),
                    (Piece.CreateBlack(), new BoardPosition(4, 2))
                }
            });
            testData.Add(new object[]
            {
                // スキップありフルテスト
                new List<(Piece piece, BoardPosition position)>
                {
                    (Piece.CreateBlack(), new BoardPosition(4, 5)),
                    (Piece.CreateWhite(), new BoardPosition(5, 3)),
                    (Piece.CreateBlack(), new BoardPosition(4, 2)),
                    (Piece.CreateWhite(), new BoardPosition(3, 5)),
                    (Piece.CreateBlack(), new BoardPosition(2, 5)),
                    (Piece.CreateWhite(), new BoardPosition(2, 4)),
                    (Piece.CreateBlack(), new BoardPosition(3, 2)),
                    (Piece.CreateWhite(), new BoardPosition(2, 3)),
                    (Piece.CreateBlack(), new BoardPosition(5, 4)),
                    (Piece.CreateWhite(), new BoardPosition(5, 5)),
                    (Piece.CreateBlack(), new BoardPosition(6, 4)),
                    (Piece.CreateWhite(), new BoardPosition(3, 6)),
                    (Piece.CreateBlack(), new BoardPosition(2, 6)),
                    (Piece.CreateWhite(), new BoardPosition(4, 6)),
                    (Piece.CreateBlack(), new BoardPosition(1, 4)),
                    (Piece.CreateWhite(), new BoardPosition(1, 5)),
                    (Piece.CreateBlack(), new BoardPosition(3, 7)),
                    (Piece.CreateWhite(), new BoardPosition(2, 2)),
                    (Piece.CreateBlack(), new BoardPosition(1, 3)),
                    (Piece.CreateWhite(), new BoardPosition(0, 3)),
                    (Piece.CreateBlack(), new BoardPosition(0, 5)),
                    (Piece.CreateWhite(), new BoardPosition(2, 7)),
                    (Piece.CreateBlack(), new BoardPosition(1, 2)),
                    (Piece.CreateWhite(), new BoardPosition(4, 7)),
                    (Piece.CreateBlack(), new BoardPosition(1, 6)),
                    (Piece.CreateWhite(), new BoardPosition(3, 1)),
                    (Piece.CreateBlack(), new BoardPosition(2, 1)),
                    (Piece.CreateWhite(), new BoardPosition(1, 7)),
                    (Piece.CreateBlack(), new BoardPosition(4, 1)),
                    (Piece.CreateWhite(), new BoardPosition(5, 7)),
                    (Piece.CreateBlack(), new BoardPosition(5, 6)),
                    (Piece.CreateWhite(), new BoardPosition(6, 7)),
                    (Piece.CreateBlack(), new BoardPosition(6, 6)),
                    (Piece.CreateWhite(), new BoardPosition(0, 2)),
                    (Piece.CreateBlack(), new BoardPosition(0, 4)),
                    (Piece.CreateWhite(), new BoardPosition(0, 6)),
                    (Piece.CreateWhite(), new BoardPosition(0, 7)),
                    (Piece.CreateWhite(), new BoardPosition(7, 7)),
                    (Piece.CreateBlack(), new BoardPosition(7, 6)),
                    (Piece.CreateWhite(), new BoardPosition(6, 5)),
                    (Piece.CreateWhite(), new BoardPosition(1, 1)),
                    (Piece.CreateBlack(), new BoardPosition(0, 1)),
                    (Piece.CreateWhite(), new BoardPosition(3, 0)),
                    (Piece.CreateBlack(), new BoardPosition(2, 0)),
                    (Piece.CreateWhite(), new BoardPosition(0, 0)),
                    (Piece.CreateBlack(), new BoardPosition(4, 0)),
                    (Piece.CreateWhite(), new BoardPosition(7, 3)),
                    (Piece.CreateBlack(), new BoardPosition(7, 5)),
                    (Piece.CreateWhite(), new BoardPosition(1, 0)),
                    (Piece.CreateWhite(), new BoardPosition(5, 0)),
                    (Piece.CreateBlack(), new BoardPosition(5, 1)),
                    (Piece.CreateWhite(), new BoardPosition(7, 4)),
                    (Piece.CreateBlack(), new BoardPosition(6, 3)),
                    (Piece.CreateWhite(), new BoardPosition(7, 2)),
                    (Piece.CreateWhite(), new BoardPosition(6, 1)),
                    (Piece.CreateBlack(), new BoardPosition(6, 0)),
                    (Piece.CreateWhite(), new BoardPosition(5, 2)),
                    (Piece.CreateBlack(), new BoardPosition(6, 2)),
                    (Piece.CreateWhite(), new BoardPosition(7, 1)),
                    (Piece.CreateWhite(), new BoardPosition(7, 0))
                }
            });
        }
    }
}
