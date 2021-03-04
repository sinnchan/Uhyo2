using System;
using System.Collections.Generic;
using System.Linq;

namespace Src.Main.Domain.Entities.Game
{
    public class GameMaster
    {
        private readonly Board _board;
        private readonly PieceState _nowTurn;

        public GameMaster()
        {
            _board = new Board();
            _nowTurn = PieceState.Black;
            GameInit();
            GameEndFlag = false;
        }

        /// <summary>
        ///     テスト用コンストラクタ
        ///     もしかしたらロード機能で使うかも
        /// </summary>
        /// <param name="board"></param>
        public GameMaster(Board board)
        {
            _board = board;
            if (_board.IsEmpty())
            {
                GameInit();
                GameEndFlag = false;
                return;
            }

            // コマの数が偶数なら黒のターン。奇数なら代のターン。
            _nowTurn = (Board.Max - _board.Count(PieceState.Space)) % 2 == 0
                ? PieceState.Black
                : PieceState.White;
            GameEndFlag = ConfirmGameEnd();
        }

        public bool GameEndFlag { get; }

        /// <summary>
        ///     盤面初期化処理
        /// </summary>
        private void GameInit()
        {
            _board.Clear();
            _board.PlacePiece(Piece.CreateWhite(), new BoardPosition(3, 3));
            _board.PlacePiece(Piece.CreateBlack(), new BoardPosition(4, 3));
            _board.PlacePiece(Piece.CreateBlack(), new BoardPosition(3, 4));
            _board.PlacePiece(Piece.CreateWhite(), new BoardPosition(4, 4));
        }

        /// <summary>
        ///     駒の置ける場所を返します。
        /// </summary>
        /// <returns></returns>
        public List<BoardPosition> GetSuggestPositions()
        {
            var suggestList = new List<BoardPosition>();
            Board.LoopAccessAll(p =>
            {
                if (_board.GetPiece(p).State != PieceState.Space) return;
                if (FindTargetDirection(p).Any(direction => 0 < ScanToDirection(p, direction)))
                    suggestList.Add(p);
            });
            return suggestList;
        }

        /// <summary>
        /// ゲーム終了の判定
        /// </summary>
        /// <returns></returns>
        private bool ConfirmGameEnd()
        {
            return _board.IsFull()
                   || _board.Count(PieceState.Black) == 0
                   || _board.Count(PieceState.White) == 0;
        }

        /// <summary>
        ///     指定の位置から指定の方向へスキャンし
        ///     裏返せるコマの数を調べます。
        /// </summary>
        /// <param name="position">駒を置く位置</param>
        /// <param name="direction">確認する方向</param>
        /// <returns></returns>
        private int ScanToDirection(BoardPosition position, Direction direction)
        {
            var pointer = position;
            var count = 0;
            while (true)
            {
                var result = pointer.MoveTo(direction, 1);
                if (!result.valid) return 0;
                pointer = result.data;
                var pointerState = _board.GetPiece(pointer).State;
                if (pointerState == PieceState.Space) return 0;
                if (pointerState == _nowTurn) break;
                count++;
            }

            return count;
        }

        /// <summary>
        ///     指定したPositionの周りに相手の駒があるか調べます。
        /// </summary>
        /// <param name="position">調べたい位置</param>
        /// <returns>相手の駒がある方向のリスト</returns>
        private IEnumerable<Direction> FindTargetDirection(BoardPosition position)
        {
            return
            (
                from Direction direction in Enum.GetValues(typeof(Direction))
                let targetPointer = position.MoveTo(direction, 1)
                where targetPointer.valid
                let pointerPieceState = _board.GetPiece(targetPointer.data).State
                where ConfirmOpponentPiece(pointerPieceState)
                select direction
            ).ToList();
        }

        /// <summary>
        ///     相手の駒かどうか確認する
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool ConfirmOpponentPiece(PieceState state)
        {
            return state != PieceState.Space && state != _nowTurn;
        }
    }
}
