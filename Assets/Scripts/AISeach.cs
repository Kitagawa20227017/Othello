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

public class AISeach : MonoBehaviour
{

    #region 変数  

    #region const定数

    // 白の石の値
    private const int WHITE_STONE_INDEX = -1;

    // 黒の石の値
    private const int BLACK_STONE_INDEX = 1;

    // 盤面に置ける最大石数
    private const int STONE_MAX_SUM = 64;

    #endregion

    private MyScript4 _myScript4 = default;


    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start()
    {
        // 初期設定
        _myScript4 = this.gameObject.GetComponent<MyScript4>();
    }

    /// <summary>
    /// 探索処理
    /// </summary>
    public int Seach(int[,] n, int myStoneColer, int turnStoneColer, int conut)
    {
        // それぞれの石の置ける位置情報
        int[,] whiteSurfacePlate = new int[8, 8];
        int[,] blackSurfacePlate = new int[8, 8];
         int _whiteStoneConut = 0;
    int _blackStoneConut = 0;
    int stoneConut = 0;
        // 石が盤面全てに埋まっていたら終了
        foreach (int tem in n)
        {
            if (Mathf.Abs(tem) == 1)
            {
                stoneConut++;
            }
        }

        if (stoneConut >= 64)
        {
            return 0;
        }

        for (int i = 0; i < n.GetLength(0); i++)
        {
            for (int j = 0; j < n.GetLength(1); j++)
            {
                // 現在のマスが範囲内で石が置いてなく右隣に石があるとき
                if (j != n.GetLength(1) - 1 && n[i, j + 1] != 0 && n[i, j] == 0)
                {
                    Seach1(n, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左隣に石があるとき
                if (j != 0 && n[i, j - 1] != 0 && n[i, j] == 0)
                {
                    Seach2(n, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく下に石があるとき
                if (i != n.GetLength(0) - 1 && n[i + 1, j] != 0 && n[i, j] == 0)
                {
                    Seach3(n, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく上に石があるとき
                if (i != 0 && n[i - 1, j] != 0 && n[i, j] == 0)
                {
                    Seach4(n, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左下に石があるとき
                if (i != n.GetLength(0) - 1 && j != n.GetLength(1) - 1 && n[i + 1, j + 1] != 0 && n[i, j] == 0)
                {
                    Seach5(n, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく右上に石があるとき
                if (i != 0 && j != n.GetLength(1) - 1 && n[i - 1, j + 1] != 0 && n[i, j] == 0)
                {
                    Seach6(n, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく右下に石があるとき
                if (i != n.GetLength(0) - 1 && j != 0 && n[i + 1, j - 1] != 0 && n[i, j] == 0)
                {
                    Seach7(n, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左上に石があるとき
                if (i != 0 && j != 0 && n[i - 1, j - 1] != 0 && n[i, j] == 0)
                {
                    Seach8(n, whiteSurfacePlate,blackSurfacePlate, i, j);
                }
            }
        }

        // 白と黒の石がそれぞれ何個置けるか調べる
        for (int i = 0; i < whiteSurfacePlate.GetLength(0); i++)
        {
            for (int j = 0; j < whiteSurfacePlate.GetLength(1); j++)
            {
                _whiteStoneConut += whiteSurfacePlate[i, j];
                _blackStoneConut += blackSurfacePlate[i, j];
            }
        }

        // 白の置ける場所がないかつ白のターンのとき
        if (_whiteStoneConut == 0 && turnStoneColer == -1)
        {
            return 0;
        }
        // 黒の置ける場所がないかつ黒のターンのとき
        else if (_blackStoneConut == 0 && turnStoneColer == 1)
        {
            return 0;
        }

        if (turnStoneColer == -1)
        {
            return _myScript4.lll(n, whiteSurfacePlate, myStoneColer, turnStoneColer, conut);
        }
        else
        {
            return _myScript4.lll(n, blackSurfacePlate, myStoneColer, turnStoneColer, conut);
        }

    }

    /// <summary>
    /// 右隣の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach1(int[,] n,int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int x, int z)
    {
        // 右隣の色を見る
        int coler = n[x, z + 1];

        // 色を見た隣の色を見ていく
        for (int i = z + 2; i < n.GetLength(1); i++)
        {
            // 何もなかったとき
            if (n[x, i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (n[x, i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 左隣の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    ///
    ///
    private void Seach2(int[,] n, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int x, int z)
    {
        // 左隣の色を見る
        int coler = n[x, z - 1];

        // 色を見た隣の色を見ていく
        for (int i = z - 2; i > 0; i--)
        {
            // 何もなかったとき
            if (n[x, i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (n[x, i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach3(int[,] n, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int x, int z)
    {
        // 下の色を見る
        int coler = n[x + 1, z];

        // 色を見た隣の色を見ていく
        for (int i = x + 2; i < n.GetLength(0); i++)
        {
            // 何もなかったとき
            if (n[i, z] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (n[i, z] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 上の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach4(int[,] n, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int x, int z)
    {
        // 上の色を見る
        int coler = n[x - 1, z];

        // 色を見た隣の色を見ていく
        for (int i = x - 2; i > 0; i++)
        {
            // 何もなかったとき
            if (n[i, z] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (n[i, z] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach5(int[,] n, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int x, int z)
    {
        // 左下の色を見る
        int coler = n[x + 1, z + 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < n.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (x + i >= n.GetLength(0) || z + i >= n.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (n[x + i, z + i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (n[x + i, z + i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 右上の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach6(int[,] n, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int x, int z)
    {
        // 右上の色を見る
        int coler = n[x - 1, z + 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < n.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (x - i < 0 || z + i >= n.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (n[x - i, z + i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (n[x - i, z + i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 右下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach7(int[,] n, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int x, int z)
    {
        // 右下の色を見る
        int coler = n[x + 1, z - 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < n.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (x + i >= n.GetLength(0) || z - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (n[x + i, z - i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (n[x + i, z - i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, x, z);
                break;
            }
        }
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    private void Seach8(int[,] n, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int x, int z)
    {
        // 左下の色を見る
        int coler = n[x - 1, z - 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < n.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (x - i < 0 || z - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (n[x - i, z - i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (n[x - i, z - i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate,coler, x, z);
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
    private void PutStoneColer(int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int coler, int x, int z)
    {
        //  
        if (coler == BLACK_STONE_INDEX)
        {
            blackSurfacePlate[x, z] = 1;
        }
        //
        else if (coler == WHITE_STONE_INDEX)
        {
            whiteSurfacePlate[x, z] = 1;
        }
    }

    #endregion
}
