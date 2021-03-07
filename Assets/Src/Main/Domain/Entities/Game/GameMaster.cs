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

        public Result<List<BoardPosition>> Place(Piece piece, BoardPosition position)
        {
            // ターンの駒でない場合、
            // 置き場が空白でない場合、
            // ひっくり返せない場合はエラー。
            if (_nowTurn != piece.State
                || _board.GetPiece(position).State != PieceState.Space
                || !ConfirmPlaceablePosition(position))
                return new Result<List<BoardPosition>>(null, false);

            var turnOverPosition = new List<BoardPosition>();

            _board.PlacePiece(piece, position);
            foreach (var direction in FindTargetDirection(position))
            {
                var turnablePosition = ScanToDirection(position, direction);
                if (turnablePosition.Count <= 0) continue;
                turnOverPosition.AddRange(turnablePosition);
            }

            foreach (var boardPosition in turnOverPosition)
                _board.GetPiece(boardPosition).TurnOver();

            return new Result<List<BoardPosition>>(turnOverPosition, true);
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
                if (ConfirmPlaceablePosition(p))
                    suggestList.Add(p);
            });
            return suggestList;
        }

        /// <summary>
        ///     参照渡ししてclear()されると悲しいのでコピーを渡す。
        /// </summary>
        /// <returns></returns>
        public Board GetBoardData()
        {
            return _board.CreateCopy();
        }

        /// <summary>
        ///     指定の位置が駒を置けるのかを確かめる。
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool ConfirmPlaceablePosition(BoardPosition p)
        {
            return _board.GetPiece(p).State == PieceState.Space
                   && FindTargetDirection(p).Any(direction => 0 < ScanToDirection(p, direction).Count);
        }

        /// <summary>
        ///     ゲーム終了の判定
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
        ///     裏返せるコマの座標を返します。
        /// </summary>
        /// <param name="position">駒を置く位置</param>
        /// <param name="direction">確認する方向</param>
        /// <returns></returns>
        private List<BoardPosition> ScanToDirection(BoardPosition position, Direction direction)
        {
            var pointer = position;
            var turnableList = new List<BoardPosition>();
            while (true)
            {
                var result = pointer.MoveTo(direction, 1);
                if (!result.valid) return new List<BoardPosition>();
                pointer = result.data;
                var pointerState = _board.GetPiece(pointer).State;
                if (pointerState == PieceState.Space) return new List<BoardPosition>();
                if (pointerState == _nowTurn) break;
                turnableList.Add(pointer);
            }

            return turnableList;
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
