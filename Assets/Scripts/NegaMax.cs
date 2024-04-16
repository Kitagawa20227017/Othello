// ---------------------------------------------------------  
// MiniMaxAI.cs  
// 
// NegaMax法によるAI
//
// 作成日: 2024/4/9
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;

public class NegaMax : MonoBehaviour
{

    #region 変数  

    #region 定数

    // 白の石の値
    private const int WHITE_STONE_INDEX = -1;

    // 黒の石の値
    private const int BLACK_STONE_INDEX = 1;

    // 石が置ける
    private const int PUT_STONE = 1;

    #endregion

    [SerializeField,Header("SurfacePlateオブジェクト")]
    private GameObject _surfacePlate = default;

    [SerializeField, Header("GameManagerオブジェクト")]
    private GameObject _gameManager = default;

    [SerializeField, Header("PutStoneオブジェクト")]
    private GameObject _putStone = default;

    // GameManager取得用
    private GameManager _gameManagerScript = default;

    // SeachPutPossible取得用
    private SeachPutPossible _seachPutPossible = default;

    // StoneControl取得用
    private StoneControl _stoneControl = default;

    // AITurnOver取得用
    private AITurnOver _AITurnOver = default;

    // 盤面情報コピー用
    private int[,] _mapCopy = new int[8, 8];

    // 石が置けるマスのコピー用
    private int[,] _myPutStone = new int[8, 8];

    // 自分の石の色
    private int _myColer = 0;

    // スコア代入用
    private int _score = 0;

    // 最大スコア代入用
    private int _maxScore = default;

    // 置く石の縦軸
    private int _verticalAxis = 0;

    // 置く石の横軸 
    private int _horizontalAxis = 0;

    // 置けるマスの数
    private int _myPutConut = 0;

    // 自分の石の色が白かどうか
    private bool _isWhite = false;

    // スコア更新フラグ
    private bool _isUpdataScore = false;

    #endregion
 
    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        // 初期設定
        _seachPutPossible = _surfacePlate.GetComponent<SeachPutPossible>();
        _stoneControl = _putStone.GetComponent<StoneControl>();
        _gameManagerScript = _gameManager.GetComponent<GameManager>();
        _AITurnOver = this.gameObject.GetComponent<AITurnOver>();
        if (_gameManagerScript.IsPlayerTurn == false)
        {
            _isWhite = true;
            _myColer = BLACK_STONE_INDEX;
        }
        else
        {
            _isWhite = false;
            _myColer = WHITE_STONE_INDEX;
        }
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>
    private void Update()
    {
        // 自分のターンが来たとき
        if (_gameManagerScript.IsPlayerTurn == false && !_gameManagerScript.IsFin)
        {
            // 盤面情報をコピー
            for(int i = 0; i < _mapCopy.GetLength(0); i++)
            {
                for(int j = 0; j < _mapCopy.GetLength(1); j++)
                {
                    _mapCopy[i, j] = _seachPutPossible.Map[i, j];
                }
            }
            // MinimMaxによる探索開始
            NegaMaxAI();
        }
    }

    /// <summary>
    /// NegaMaxによる探索
    /// </summary>
    private void NegaMaxAI()
    {
        // 初期化
        _score = 0;
        _maxScore = -10000;
        _verticalAxis = 0;
        _horizontalAxis = 0;
        _myPutConut = 0;
        _isUpdataScore = false;

        // 自分が黒のとき
        if (!_isWhite)
        {
            // 置ける場所をコピーする
            for (int i = 0; i < _seachPutPossible.Black.GetLength(0); i++)
            {
                for (int j = 0; j < _seachPutPossible.Black.GetLength(1); j++)
                {
                    _myPutStone[i, j] = _seachPutPossible.Black[i, j];
                    _myPutConut = _seachPutPossible.BlackCount;
                }
            }
        }
        // 自分が白のとき
        else
        {
            // 置ける場所をコピーする
            for (int i = 0; i < _seachPutPossible.White.GetLength(0); i++)
            {
                for (int j = 0; j < _seachPutPossible.White.GetLength(1); j++)
                {
                    _myPutStone[i, j] = _seachPutPossible.White[i, j];
                    _myPutConut = _seachPutPossible.WhiteCount;
                }
            }
        }

        // 置ける場所がなかったらスキップ
        if(_myPutConut == 0)
        {
            return;
        }

        // 探索開始
        for (int i = 0; i < _myPutStone.GetLength(0); i++)
        {
            for (int j = 0; j < _myPutStone.GetLength(0); j++)
            {
                // 石が置けるとき
                if (_myPutStone[i, j] == PUT_STONE)
                {
                    // スコアを計算する
                    _score = -_AITurnOver.TurnOver(_mapCopy,i,j,_myColer, _myColer, 0);
                    _isUpdataScore = true;
                }

                // 新しいスコアが最大スコアより大きいとき
                if (_isUpdataScore && _score > _maxScore)
                {
                    // 最大スコアの更新
                    _maxScore = _score;

                    // 最大スコアのマスの記録
                    _verticalAxis = i;
                    _horizontalAxis = j;
                }
                // 最大スコアが同じのときはランダムで位置を決める
                else if(_isUpdataScore && _score == _maxScore && Random.Range(0,2) == 0)
                {
                    // 最大スコアの更新
                    _maxScore = _score;

                    // 最大スコアのマスの記録
                    _verticalAxis = i;
                    _horizontalAxis = j;
                }
                _isUpdataScore = false;
            }
        }

        // 最大スコアのマスを渡す
        _stoneControl.PutStone(_verticalAxis, _horizontalAxis);
        return;

    }

    #endregion

}
