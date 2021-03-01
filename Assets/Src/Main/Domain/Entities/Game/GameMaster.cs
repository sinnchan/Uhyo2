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
            GameEndFlag = board.IsFull(); //todo game終了判定が必要
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
        public List<Position> GetSuggestPositions()
        {
            var suggestList = new List<Position>();
            Board.LoopAccessAll(p =>
            {
                if (_board.GetPiece(p).State != PieceState.Space) return;
                var targetDirection = FindTargetDirection(p);
                suggestList.AddRange
                (
                    from direction in targetDirection
                    select ScanToDirection(p, direction)
                    into qty
                    where 0 < qty
                    select p
                );
            });
            return null;
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
            //todo
            return 0;
        }

        /// <summary>
        ///     次のポインターを返します。
        ///     次がない場合はvalidでfalseを返します。
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static Result<BoardPosition> GetNextPointer(BoardPosition current, Direction target)
        {
            return current.MoveTo(target, 1);
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
