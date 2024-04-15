// ---------------------------------------------------------  
// StoneControl.cs  
//   
// 設置
//
// 作成日: 2024/4/10
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class StoneControl : MonoBehaviour
{

    #region 変数  

    #region 定数

    // 白の石の向き
    private const int WHITE_STONE_ANGLE = 270;

    // 黒の石の向き
    private const int BLACK_STONE_ANGLE = 90;

    // 白の石の値
    private const int WHITE_STONE_INDEX = -1;

    // 黒の石の値
    private const int BLACK_STONE_INDEX = 1;

    // 石が置けるマス
    private const int PUT_STONE = 1;

    // 1つ隣
    private const int ONE_NEIGHBOR = 1;

    // 2つ隣
    private const int TWO_NEIGHBOR = 2;

    #endregion

    [SerializeField,Header("SurfacePlateオブジェクト")]
    private GameObject _surfacePlate = default;

    [SerializeField, Header("GameManegerオブジェクト")]
    private GameObject _gameManeger = default;

    [SerializeField,Header("Stonesオブジェクト")]
    private Transform _stones = default;

    // 石が置けるマス
    private GameObject[,] _putStones = new GameObject[8, 8];

    // Transform取得用
    private Transform _parentTransform = default;

    // SeachPutPossible取得用
    private SeachPutPossible _seachPutPossible = default;

    // GameManager取得用
    private GameManager _gameManagerScript = default;

    // TurnOver取得用
    private TurnOver _turnOver = default;
    
    // アクティブにするオブジェクトの格納
    private GameObject _gameObject = default;

    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        // 初期設定
        _turnOver = _surfacePlate.GetComponent<TurnOver>();
        _parentTransform = this.gameObject.transform;
        _seachPutPossible = _surfacePlate.GetComponent<SeachPutPossible>();
        _gameManagerScript = _gameManeger.GetComponent<GameManager>();
        foreach(Transform chlid in _parentTransform)
        {
            int verticalAxis = Mathf.FloorToInt(chlid.localPosition.x);
            int horizontalAxis = Mathf.FloorToInt(chlid.localPosition.z);
            _putStones[verticalAxis, horizontalAxis] = chlid.gameObject;
            chlid.gameObject.SetActive(false);
        }
        WhitePutStone();
    }

    /// <summary>
    /// 白の石が置けるマスの表示
    /// </summary>
    public void WhitePutStone()
    {
        // マスが埋まっていたら終了
        if(_gameManagerScript.IsFin)
        {
            return;
        }

        // 置けるマスのアクティブ化
        for(int i = 0; i < _seachPutPossible.White.GetLength(0); i++)
        {
            for (int j = 0; j < _seachPutPossible.White.GetLength(1); j++)
            {
                if(_seachPutPossible.White[i,j] == PUT_STONE)
                {
                    _putStones[i, j].SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// 黒の石が置けるマスの表示
    /// </summary>
    public void BlackPutStone()
    {
        // マスが埋まっていたら終了
        if (_gameManagerScript.IsFin)
        {
            return;
        }

        // 置けるマスのアクティブ化
        for (int i = 0; i < _seachPutPossible.Black.GetLength(0); i++)
        {
            for (int j = 0; j < _seachPutPossible.Black.GetLength(1); j++)
            {
                if (_seachPutPossible.Black[i, j] == PUT_STONE)
                {
                    _putStones[i, j].SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// 石が置く処理
    /// </summary>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    public void PutStone(int verticalAxis,int horizontalAxis)
    {
        // 自分が黒のとき
        if(!_gameManagerScript.IsTurn)
        {
            // 置けるマスの非アクティブ化
            foreach (Transform chlid in _parentTransform)
            {
                chlid.gameObject.SetActive(false);
            }

            // 石を１つ取得
            foreach (Transform chlid in _stones)
            {
                _gameObject = chlid.gameObject;
                break;
            }

            // 親オブジェクトを変更
            _gameObject.transform.parent = _surfacePlate.transform;
            
            // 指定のマスに移動
            _gameObject.transform.localPosition = new Vector3(verticalAxis, 0, horizontalAxis);
            
            // 石を黒の向きに変える
            _gameObject.transform.eulerAngles = new Vector3(BLACK_STONE_ANGLE, 0, 0);
            
            // 石のアクティブ化
            _gameObject.SetActive(true);
            
            // 盤面情報更新
            _seachPutPossible.Map[verticalAxis, horizontalAxis] = BLACK_STONE_INDEX;
            
            // 裏返す
            _turnOver.Seach(verticalAxis,horizontalAxis,BLACK_STONE_INDEX);

        }
        // 自分が白のとき
        else if(_gameManagerScript.IsTurn)
        {
            // 置けるマスの非アクティブ化
            foreach (Transform chlid in _parentTransform)
            {
                chlid.gameObject.SetActive(false);
            }

            // 石を１つ取得
            foreach (Transform chlid in _stones)
            {
                _gameObject = chlid.gameObject;
                break;
            }

            // 親オブジェクトを変更
            _gameObject.transform.parent = _surfacePlate.transform;

            // 指定のマスに移動
            _gameObject.transform.localPosition = new Vector3(verticalAxis, 0, horizontalAxis);

            // 石を白の向きに変える
            _gameObject.transform.eulerAngles = new Vector3(WHITE_STONE_ANGLE, 0, 0);

            // 石のアクティブ化
            _gameObject.SetActive(true);

            // 盤面情報更新
            _seachPutPossible.Map[verticalAxis, horizontalAxis] = WHITE_STONE_INDEX;

            // 裏返す
            _turnOver.Seach(verticalAxis, horizontalAxis, WHITE_STONE_INDEX);

        }
    }

    #endregion

}
