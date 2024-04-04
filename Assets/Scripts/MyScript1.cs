// ---------------------------------------------------------  
// MyScript1.cs  
// 
//
//
// 作成日: 
// 作成者: 
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class MyScript1 : MonoBehaviour
{

    #region 変数  

    #region const定数

    // 白の石の向き
    private const int WHITE_STONE_ANGLE = 270;

    // 黒の石の向き
    private const int BLACK_STONE_ANGLE = 90;

    // 白の石の値
    private const int WHITE_STONE_INDEX = -1;

    // 黒の石の値
    private const int BLACK_STONE_INDEX = 1;

    // 盤面に置ける最大石数
    private const int STONE_MAX_SUM = 64;

    #endregion

    [SerializeField, Header("GameManagerオブジェクト")]
    private GameObject _gameManager = default;

    // GameManagerスクリプト格納用
    private GameManager _gameManagerScript = default;

    // Transform取得用
    private Transform _parentTransform = default;

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
    private void Start()
    {
        // 初期設定
        _parentTransform = this.transform;
        _gameManagerScript = _gameManager.GetComponent<GameManager>();
        //Seach();
    }

    /// <summary>
    /// 探索処理
    /// </summary>
    public void Seach(int[,] a)
    {
        // 石が盤面全てに埋まっていたら終了
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
            string s = default;
            if (_blackStoneSum > _whiteStoneSum)
            {
                s = "黒勝ち";
            }
            else if (_blackStoneSum > _whiteStoneSum)
            {
                s = "白勝ち";
            }
            else if (_blackStoneSum == _whiteStoneSum)
            {
                s = "引き分け";
            }
            _gameManagerScript.nnnn(s);
            return;
        }

        // 初期化
        _whiteStoneConut = 0;
        _blackStoneConut = 0;
        for (int i = 0; i < _blackSurfacePlate.GetLength(0); i++)
        {
            for (int j = 0; j < _blackSurfacePlate.GetLength(1); j++)
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
            if (verticalAxis < chlid.localPosition.x)
            {
                verticalAxis++;
            }
            else if (verticalAxis > chlid.localPosition.x)
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
                if (i != _surfacePlate.GetLength(0) - 1 && j != _surfacePlate.GetLength(1) - 1 && _surfacePlate[i + 1, j + 1] != 0 && _surfacePlate[i, j] == 0)
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
        for (int i = 0; i < _whiteSurfacePlate.GetLength(0); i++)
        {
            for (int j = 0; j < _whiteSurfacePlate.GetLength(1); j++)
            {
                _whiteStoneConut += _whiteSurfacePlate[i, j];
                _blackStoneConut += _blackSurfacePlate[i, j];
            }
        }

        // 白の置ける場所がないかつ白のターンのとき
        if (_whiteStoneConut == 0 && !_gameManagerScript.IsTurn)
        {
            _gameManagerScript.aa();
        }
        // 黒の置ける場所がないかつ黒のターンのとき
        else if (_blackStoneConut == 0 && _gameManagerScript.IsTurn)
        {
            _gameManagerScript.aa();
        }
    }

    /// <summary>
    /// 右隣の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach1(int x, int z)
    {
        // 右隣の色を見る
        int coler = _surfacePlate[x, z + 1];

        // 色を見た隣の色を見ていく
        for (int i = z + 2; i < _surfacePlate.GetLength(1); i++)
        {
            // 何もなかったとき
            if (_surfacePlate[x, i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[x, i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 左隣の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach2(int x, int z)
    {
        // 左隣の色を見る
        int coler = _surfacePlate[x, z - 1];

        // 色を見た隣の色を見ていく
        for (int i = z - 2; i > 0; i--)
        {
            // 何もなかったとき
            if (_surfacePlate[x, i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[x, i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach3(int x, int z)
    {
        // 下の色を見る
        int coler = _surfacePlate[x + 1, z];

        // 色を見た隣の色を見ていく
        for (int i = x + 2; i < _surfacePlate.GetLength(0); i++)
        {
            // 何もなかったとき
            if (_surfacePlate[i, z] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[i, z] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 上の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach4(int x, int z)
    {
        // 上の色を見る
        int coler = _surfacePlate[x - 1, z];

        // 色を見た隣の色を見ていく
        for (int i = x - 2; i > 0; i++)
        {
            // 何もなかったとき
            if (_surfacePlate[i, z] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[i, z] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach5(int x, int z)
    {
        // 左下の色を見る
        int coler = _surfacePlate[x + 1, z + 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (x + i >= _surfacePlate.GetLength(0) || z + i >= _surfacePlate.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (_surfacePlate[x + i, z + i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[x + i, z + i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 右上の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach6(int x, int z)
    {
        // 右上の色を見る
        int coler = _surfacePlate[x - 1, z + 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (x - i < 0 || z + i >= _surfacePlate.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (_surfacePlate[x - i, z + i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[x - i, z + i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 右下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach7(int x, int z)
    {
        // 右下の色を見る
        int coler = _surfacePlate[x + 1, z - 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (x + i >= _surfacePlate.GetLength(0) || z - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (_surfacePlate[x + i, z - i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[x + i, z - i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach8(int x, int z)
    {
        // 左下の色を見る
        int coler = _surfacePlate[x - 1, z - 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (x - i < 0 || z - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (_surfacePlate[x - i, z - i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (_surfacePlate[x - i, z - i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 置ける色の判定
    /// </summary>
    /// <param name="coler">石の色</param>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void PutStoneColer(int coler, int x, int z)
    {
        //  
        if (coler == BLACK_STONE_INDEX)
        {
            _blackSurfacePlate[x, z] = 1;
        }
        //
        else if (coler == WHITE_STONE_INDEX)
        {
            _whiteSurfacePlate[x, z] = 1;
        }
    }

    #endregion
}
