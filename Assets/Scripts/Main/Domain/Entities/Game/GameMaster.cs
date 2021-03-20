using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Domain.Entities.Game
{
    public class GameMaster
    {
        private readonly Board _board;
        private PieceState _nowTurn;

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
        /// <param name="turn"></param>
        public GameMaster(Board board, PieceState turn)
        {
            _board = board;
            _nowTurn = turn;
            if (_board.IsEmpty())
            {
                GameInit();
                GameEndFlag = false;
                return;
            }

            GameEndFlag = ConfirmGameEnd();
        }

        public bool GameEndFlag { get; private set; }

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
        ///     駒を置きます。
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="position"></param>
        /// <returns>裏返した駒の位置</returns>
        public Result<List<BoardPosition>> Place(Piece piece, BoardPosition position)
        {
            // ターンの駒でない場合、
            // 置き場が空白でない場合、
            // ひっくり返せない場合はエラー。
            if (_nowTurn != piece.State
                || _board.GetPiece(position).State != PieceState.Space
                || !ConfirmPlaceablePosition(_nowTurn, position))
                return new Result<List<BoardPosition>>(null, false);

            var turnOverPosition = new List<BoardPosition>();

            _board.PlacePiece(piece, position);
            foreach (var direction in FindTargetDirection(_nowTurn, position))
            {
                var turnablePosition = ScanToDirection(_nowTurn, position, direction);
                if (turnablePosition.Count <= 0) continue;
                turnOverPosition.AddRange(turnablePosition);
            }

            foreach (var boardPosition in turnOverPosition)
                _board.GetPiece(boardPosition).TurnOver();

            // 次のターンの人が置けるならターンを変えて続行
            if (CanBePlaced(_nowTurn.Opposite()))
                _nowTurn = _nowTurn.Opposite();
            // 次のターンの人が置けないならスキップ(ターンはそのまま)
            // 両者おけないならゲーム終了
            else if (!CanBePlaced(_nowTurn)) GameEndFlag = true;

            return new Result<List<BoardPosition>>(turnOverPosition, true);
        }

        /// <summary>
        ///     指定したターンの人が置けるか確認します
        /// </summary>
        /// <param name="turn"></param>
        /// <returns></returns>
        public bool CanBePlaced(PieceState turn)
        {
            return 0 < GetSuggestPositions(turn).Count;
        }

        /// <summary>
        ///     駒の置ける場所を返します。
        /// </summary>
        /// <returns></returns>
        public List<BoardPosition> GetSuggestPositions(PieceState turn)
        {
            var suggestList = new List<BoardPosition>();
            Board.LoopAccessAll(p =>
            {
                if (ConfirmPlaceablePosition(turn, p))
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
        /// <param name="turn">ターンの指定</param>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool ConfirmPlaceablePosition(PieceState turn, BoardPosition p)
        {
            return _board.GetPiece(p).State == PieceState.Space
                   && FindTargetDirection(turn, p).Any(direction => 0 < ScanToDirection(turn, p, direction).Count);
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
        /// <param name="turn">ターン指定</param>
        /// <param name="position">駒を置く位置</param>
        /// <param name="direction">確認する方向</param>
        /// <returns></returns>
        private List<BoardPosition> ScanToDirection(PieceState turn, BoardPosition position, Direction direction)
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
                if (pointerState == turn) break;
                turnableList.Add(pointer);
            }

            return turnableList;
        }

        /// <summary>
        ///     指定したPositionの周りに相手の駒があるか調べます。
        /// </summary>
        /// <param name="turn">ターン指定</param>
        /// <param name="position">調べたい位置</param>
        /// <returns>相手の駒がある方向のリスト</returns>
        private IEnumerable<Direction> FindTargetDirection(PieceState turn, BoardPosition position)
        {
            return
            (
                from Direction direction in Enum.GetValues(typeof(Direction))
                let targetPointer = position.MoveTo(direction, 1)
                where targetPointer.valid
                let pointerPieceState = _board.GetPiece(targetPointer.data).State
                where pointerPieceState.IsOppositeTo(turn)
                select direction
            ).ToList();
        }
    }
}
