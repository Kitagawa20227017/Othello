// ---------------------------------------------------------  
// SeachPutPossible.cs  
//
// 設置可能マスを調べる  
//
// 作成日: 2024/4/1
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;

public class SeachPutPossible : MonoBehaviour
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

    // 石が置ける
    private const int PUT_STONE = 1;

    // 盤面に置ける最大石数
    private const int STONE_MAX_SUM = 64;

    #endregion

    [SerializeField,Header("GameManagerオブジェクト")]
    private GameObject _gameManager = default;

    [SerializeField]
    private GameObject _putStone = default;

    // GameManagerスクリプト格納用
    private GameManager _gameManagerScript = default;

    // Transform取得用
    private Transform _parentTransform = default;
    private Transform _putStoneTransform = default;

    // 盤面情報
    private int[,] _surfacePlate = new int[8, 8];

    // それぞれの石の置ける位置情報
    private int[,] _whiteSurfacePlate = new int[8, 8];
    private int[,] _blackSurfacePlate = new int[8, 8];

    // それぞれの石の置ける数 
    private int _whiteStoneConut = 0;
    private int _blackStoneConut = 0;

    //  それぞれの石の合計
    private int _whiteStoneSum = 0;
    private int _blackStoneSum = 0;

    // 終了フラグ
    private bool _isFin = false;

    #endregion

    #region プロパティ  

    public int[,] Map
    {
        get => _surfacePlate;
    }

    public int[,] Black
    {
        get => _blackSurfacePlate;
    }

    public int BlackCount
    {
        get => _blackStoneConut;
    }

    public int WhiteCount
    {
        get => _whiteStoneConut;
    }

    public int[,] White 
    {
        get => _whiteSurfacePlate;
    }

    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        // 初期設定
        _parentTransform = this.transform;
        _putStoneTransform = _putStone.transform;
        _gameManagerScript = _gameManager.GetComponent<GameManager>();
        Seach();
    }

    /// <summary>
    /// 探索処理
    /// </summary>
    public void Seach()
    {
        if (_isFin)
        {
            return;
        }

        // 初期化
        _whiteStoneConut = 0;
        _blackStoneConut = 0;

        // 初期化
        for (int i = 0; i < _blackSurfacePlate.GetLength(0);i++)
        {
            for(int j = 0; j < _blackSurfacePlate.GetLength(1);j++)
            {
                _blackSurfacePlate[i, j] = 0;
                _whiteSurfacePlate[i, j] = 0;
                _surfacePlate[i, j] = 0;
            }
        }

        // 子オブジェクト探索
        foreach (Transform chlid in _parentTransform)
        {
            // 座標をintにする
            int verticalAxis = (int)chlid.localPosition.x;
            int horizontalAxis = (int)chlid.localPosition.z;
            
            // 値がずれたら補正
            if(verticalAxis < chlid.localPosition.x)
            {
                verticalAxis++;
            }
            else if(verticalAxis > chlid.localPosition.x)
            {
                verticalAxis--;
            }

            // 値がずれたら補正
            if (horizontalAxis < chlid.localPosition.z)
            {
                horizontalAxis++;
            }
            else if (horizontalAxis > chlid.localPosition.z)
            {
                horizontalAxis--;
            }

            // 石が表か裏かを見る
            switch (chlid.transform.eulerAngles.x)
            {
                case BLACK_STONE_ANGLE:
                    _surfacePlate[verticalAxis, horizontalAxis] = BLACK_STONE_INDEX;
                    break;

                case WHITE_STONE_ANGLE:
                    _surfacePlate[verticalAxis, horizontalAxis] = WHITE_STONE_INDEX;
                    break;
            }
        }

        // 探索
        for (int i = 0; i < _surfacePlate.GetLength(0); i++)
        {
            for (int j = 0; j < _surfacePlate.GetLength(1); j++)
            {
                // 現在のマスが範囲内で石が置いてなく右隣に石があるとき
                if (j != _surfacePlate.GetLength(1) - 1 && _surfacePlate[i, j + 1] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach1(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左隣に石があるとき
                if (j != 0 && _surfacePlate[i, j - 1] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach2(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく下に石があるとき
                if (i != _surfacePlate.GetLength(0) - 1 && _surfacePlate[i + 1, j] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach3(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく上に石があるとき
                if (i != 0 && _surfacePlate[i - 1, j] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach4(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左下に石があるとき
                if (i != _surfacePlate.GetLength(0) - 1 && j != _surfacePlate.GetLength(1) - 1 &&  _surfacePlate[i + 1, j + 1] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach5(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく右上に石があるとき
                if (i != 0 && j != _surfacePlate.GetLength(1) - 1 && _surfacePlate[i - 1, j + 1] != 0 && _surfacePlate[i, j] == 0)
                {
                   Seach6(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく右下に石があるとき
                if (i != _surfacePlate.GetLength(0) - 1 && j != 0 && _surfacePlate[i + 1, j - 1] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach7(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左上に石があるとき
                if (i != 0 && j != 0 && _surfacePlate[i - 1, j - 1] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach8(i, j);
                }
            }
        }

        // 白と黒の石がそれぞれ何個置けるか調べる
        for(int i = 0; i < _whiteSurfacePlate.GetLength(0); i++)
        {
            for(int j = 0; j < _whiteSurfacePlate.GetLength(1); j++)
            {
                _whiteStoneConut += _whiteSurfacePlate[i, j];
                _blackStoneConut += _blackSurfacePlate[i, j];
            }
        }


        if (_parentTransform.childCount >= STONE_MAX_SUM)
        {
            // それぞれの石が何個か数える
            foreach (int stone in _surfacePlate)
            {
                if (stone == BLACK_STONE_INDEX)
                {
                    _blackStoneSum++;
                }
                else if (stone == WHITE_STONE_INDEX)
                {
                    _whiteStoneSum++;
                }

            }

            // 置けるマスのオブジェクトを非アクティブ化
            foreach (Transform chlid in _putStoneTransform)
            {
                chlid.gameObject.SetActive(false);
            }

            int outCome = 0;
            if (_blackStoneSum > _whiteStoneSum)
            {
                outCome = 1;
            }
            else if (_blackStoneSum < _whiteStoneSum)
            {
                outCome = - 1;
            }
            else if (_blackStoneSum == _whiteStoneSum)
            {
                outCome = 0;
            }
            _gameManagerScript.GameEnd(outCome);
            _isFin = true;
            return;
        }

        // 白の置ける場所がないかつ白のターンのとき
        if (_whiteStoneConut == 0 && !_gameManagerScript.IsTurn)
        {
            _gameManagerScript.TurnChange();
            return;
        }
        // 黒の置ける場所がないかつ黒のターンのとき
        else if (_blackStoneConut == 0 && _gameManagerScript.IsTurn)
        {
            _gameManagerScript.TurnChange();
            return;
        }
    }

    /// <summary>
    /// 右隣の探索
    /// </summary>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach1(int verticalAxis,int horizontalAxis)
    {
        // 右隣の色を見る
        int coler = _surfacePlate[verticalAxis, horizontalAxis + 1];

        // 色を見た隣の色を見ていく
        for(int i = horizontalAxis + 2; i < _surfacePlate.GetLength(1); i++)
        {
            // 何もなかったとき
            if(_surfacePlate[verticalAxis,i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[verticalAxis, i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 左隣の探索
    /// </summary>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach2(int verticalAxis, int horizontalAxis)
    {
        // 左隣の色を見る
        int coler = _surfacePlate[verticalAxis, horizontalAxis - 1];

        // 色を見た隣の色を見ていく
        for (int i = horizontalAxis - 2; i >= 0; i--)
        {
            // 何もなかったとき
            if (_surfacePlate[verticalAxis, i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[verticalAxis, i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 下の探索
    /// </summary>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach3(int verticalAxis, int horizontalAxis)
    {
        // 下の色を見る
        int coler = _surfacePlate[verticalAxis + 1, horizontalAxis];

        // 色を見た隣の色を見ていく
        for (int i = verticalAxis + 2; i < _surfacePlate.GetLength(0); i++)
        {
            // 何もなかったとき
            if (_surfacePlate[i, horizontalAxis] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[i, horizontalAxis] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 上の探索
    /// </summary>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach4(int verticalAxis, int horizontalAxis)
    {
        // 上の色を見る
        int coler = _surfacePlate[verticalAxis - 1, horizontalAxis];

        // 色を見た隣の色を見ていく
        for (int i = verticalAxis - 2; i >= 0; i--)
        {
            // 何もなかったとき
            if (_surfacePlate[i, horizontalAxis] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[i, horizontalAxis] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach5(int verticalAxis, int horizontalAxis)
    {
        // 左下の色を見る
        int coler = _surfacePlate[verticalAxis + 1, horizontalAxis + 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if(verticalAxis + i >= _surfacePlate.GetLength(0) || horizontalAxis + i >= _surfacePlate.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (_surfacePlate[verticalAxis + i,horizontalAxis + i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[verticalAxis + i, horizontalAxis + i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 右上の探索
    /// </summary>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach6(int verticalAxis, int horizontalAxis)
    {
        // 右上の色を見る
        int coler = _surfacePlate[verticalAxis - 1, horizontalAxis + 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (verticalAxis - i < 0 || horizontalAxis + i >= _surfacePlate.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (_surfacePlate[verticalAxis - i, horizontalAxis + i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[verticalAxis - i, horizontalAxis + i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 右下の探索
    /// </summary>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach7(int verticalAxis, int horizontalAxis)
    {
        // 右下の色を見る
        int coler = _surfacePlate[verticalAxis + 1, horizontalAxis - 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (verticalAxis + i >= _surfacePlate.GetLength(0) || horizontalAxis - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (_surfacePlate[verticalAxis + i, horizontalAxis - i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[verticalAxis + i, horizontalAxis - i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach8(int verticalAxis, int horizontalAxis)
    {
        // 左下の色を見る
        int coler = _surfacePlate[verticalAxis - 1, horizontalAxis - 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (verticalAxis - i < 0 || horizontalAxis - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (_surfacePlate[verticalAxis - i, horizontalAxis - i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[verticalAxis - i, horizontalAxis - i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 置ける色の判定
    /// </summary>
    /// <param name="coler">石の色</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void PutStoneColer(int coler,int verticalAxis,int horizontalAxis)
    {
        //  
        if(coler == BLACK_STONE_INDEX)
        {
            _blackSurfacePlate[verticalAxis, horizontalAxis] = 1;
        }
        //
        else if(coler == WHITE_STONE_INDEX)
        {
            _whiteSurfacePlate[verticalAxis, horizontalAxis] = 1;
        }
    }

    #endregion

}
