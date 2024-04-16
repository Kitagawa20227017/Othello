// ---------------------------------------------------------  
// AISeach.cs  
// 
// 盤面情報更新処理
//
// 作成日: 2024/4/9
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;

public class AISeach : MonoBehaviour
{

    #region 変数  

    #region 定数

    // 白の石の値
    private const int WHITE_STONE_INDEX = -1;

    // 黒の石の値
    private const int BLACK_STONE_INDEX = 1;

    // 盤面に置ける最大石数
    private const int STONE_MAX_SUM = 64;

    // 盤面のマス数
    private const int MASS_NUMBER = 8;

    // 石が置ける
    private const int PUT_STONE = 1;

    #endregion

    // BottomScore取得用
    private BottomScore _bottomScore = default;

    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start()
    {
        // 初期設定
        _bottomScore = this.gameObject.GetComponent<BottomScore>();
    }

    /// <summary>
    /// 探索処理
    /// </summary>
    public int Seach(int[,] surfacePlate, int myStoneColer, int turnStoneColer, int conut)
    {
        // それぞれの石の置ける位置情報
        int[,] whiteSurfacePlate = new int[MASS_NUMBER, MASS_NUMBER];
        int[,] blackSurfacePlate = new int[MASS_NUMBER, MASS_NUMBER];
        int _whiteStoneConut = 0;
        int _blackStoneConut = 0;
        int stoneConut = 0;
        // 石が盤面全てに埋まっていたら終了
        foreach (int tem in surfacePlate)
        {
            if (Mathf.Abs(tem) == PUT_STONE)
            {
                stoneConut++;
            }
        }

        // 石が埋まっているとき
        if (stoneConut >= STONE_MAX_SUM)
        {
            return 0;
        }

        // 石の探索
        for (int i = 0; i < surfacePlate.GetLength(0); i++)
        {
            for (int j = 0; j < surfacePlate.GetLength(1); j++)
            {
                // 現在のマスが範囲内で石が置いてなく右隣に石があるとき
                if (j != surfacePlate.GetLength(1) - 1 && surfacePlate[i, j + 1] != 0 && surfacePlate[i, j] == 0)
                {
                    Seach1(surfacePlate, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左隣に石があるとき
                if (j != 0 && surfacePlate[i, j - 1] != 0 && surfacePlate[i, j] == 0)
                {
                    Seach2(surfacePlate, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく下に石があるとき
                if (i != surfacePlate.GetLength(0) - 1 && surfacePlate[i + 1, j] != 0 && surfacePlate[i, j] == 0)
                {
                    Seach3(surfacePlate, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく上に石があるとき
                if (i != 0 && surfacePlate[i - 1, j] != 0 && surfacePlate[i, j] == 0)
                {
                    Seach4(surfacePlate, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左下に石があるとき
                if (i != surfacePlate.GetLength(0) - 1 && j != surfacePlate.GetLength(1) - 1 && surfacePlate[i + 1, j + 1] != 0 && surfacePlate[i, j] == 0)
                {
                    Seach5(surfacePlate, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく右上に石があるとき
                if (i != 0 && j != surfacePlate.GetLength(1) - 1 && surfacePlate[i - 1, j + 1] != 0 && surfacePlate[i, j] == 0)
                {
                    Seach6(surfacePlate, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく右下に石があるとき
                if (i != surfacePlate.GetLength(0) - 1 && j != 0 && surfacePlate[i + 1, j - 1] != 0 && surfacePlate[i, j] == 0)
                {
                    Seach7(surfacePlate, whiteSurfacePlate, blackSurfacePlate, i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左上に石があるとき
                if (i != 0 && j != 0 && surfacePlate[i - 1, j - 1] != 0 && surfacePlate[i, j] == 0)
                {
                    Seach8(surfacePlate, whiteSurfacePlate, blackSurfacePlate, i, j);
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
        if (_whiteStoneConut == 0 && turnStoneColer == WHITE_STONE_INDEX)
        {
            return 0;
        }
        // 黒の置ける場所がないかつ黒のターンのとき
        else if (_blackStoneConut == 0 && turnStoneColer == BLACK_STONE_INDEX)
        {
            return 0;
        }

        // 白のターンのとき
        if (turnStoneColer == WHITE_STONE_INDEX)
        {
            return _bottomScore.MaxScore(surfacePlate, blackSurfacePlate, myStoneColer, turnStoneColer, conut);
        }
        // 黒のターンのとき
        else
        {
            return _bottomScore.MaxScore(surfacePlate, whiteSurfacePlate, myStoneColer, turnStoneColer, conut);
        }

    }

    /// <summary>
    /// 右隣の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="whiteSurfacePlate">白の置けるマスの格納</param>
    /// <param name="blackSurfacePlate">黒の置けるマスの格納</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach1(int[,] surfacePlate,int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int verticalAxis, int horizontalAxis)
    {
        // 右隣の色を見る
        int coler = surfacePlate[verticalAxis, horizontalAxis + 1];

        // 色を見た隣の色を見ていく
        for (int i = horizontalAxis + 2; i < surfacePlate.GetLength(1); i++)
        {
            // 何もなかったとき
            if (surfacePlate[verticalAxis, i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (surfacePlate[verticalAxis, i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 左隣の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="whiteSurfacePlate">白の置けるマスの格納</param>
    /// <param name="blackSurfacePlate">黒の置けるマスの格納</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach2(int[,] surfacePlate, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int verticalAxis, int horizontalAxis)
    {
        // 左隣の色を見る
        int coler = surfacePlate[verticalAxis, horizontalAxis - 1];

        // 色を見た隣の色を見ていく
        for (int i = horizontalAxis - 2; i > 0; i--)
        {
            // 何もなかったとき
            if (surfacePlate[verticalAxis, i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (surfacePlate[verticalAxis, i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 下の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="whiteSurfacePlate">白の置けるマスの格納</param>
    /// <param name="blackSurfacePlate">黒の置けるマスの格納</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach3(int[,] surfacePlate, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int verticalAxis, int horizontalAxis)
    {
        // 下の色を見る
        int coler = surfacePlate[verticalAxis + 1, horizontalAxis];

        // 色を見た隣の色を見ていく
        for (int i = verticalAxis + 2; i < surfacePlate.GetLength(0); i++)
        {
            // 何もなかったとき
            if (surfacePlate[i, horizontalAxis] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (surfacePlate[i, horizontalAxis] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 上の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="whiteSurfacePlate">白の置けるマスの格納</param>
    /// <param name="blackSurfacePlate">黒の置けるマスの格納</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach4(int[,] surfacePlate, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int verticalAxis, int horizontalAxis)
    {
        // 上の色を見る
        int coler = surfacePlate[verticalAxis - 1, horizontalAxis];

        // 色を見た隣の色を見ていく
        for (int i = verticalAxis - 2; i > 0; i--)
        {
            // 何もなかったとき
            if (surfacePlate[i, horizontalAxis] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (surfacePlate[i, horizontalAxis] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="whiteSurfacePlate">白の置けるマスの格納</param>
    /// <param name="blackSurfacePlate">黒の置けるマスの格納</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach5(int[,] surfacePlate, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int verticalAxis, int horizontalAxis)
    {
        // 左下の色を見る
        int coler = surfacePlate[verticalAxis + 1, horizontalAxis + 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (verticalAxis + i >= surfacePlate.GetLength(0) || horizontalAxis + i >= surfacePlate.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (surfacePlate[verticalAxis + i, horizontalAxis + i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (surfacePlate[verticalAxis + i, horizontalAxis + i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 右上の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="whiteSurfacePlate">白の置けるマスの格納</param>
    /// <param name="blackSurfacePlate">黒の置けるマスの格納</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach6(int[,] surfacePlate, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int verticalAxis, int horizontalAxis)
    {
        // 右上の色を見る
        int coler = surfacePlate[verticalAxis - 1, horizontalAxis + 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (verticalAxis - i < 0 || horizontalAxis + i >= surfacePlate.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (surfacePlate[verticalAxis - i, horizontalAxis + i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (surfacePlate[verticalAxis - i, horizontalAxis + i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 右下の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="whiteSurfacePlate">白の置けるマスの格納</param>
    /// <param name="blackSurfacePlate">黒の置けるマスの格納</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach7(int[,] surfacePlate, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int verticalAxis, int horizontalAxis)
    {
        // 右下の色を見る
        int coler = surfacePlate[verticalAxis + 1, horizontalAxis - 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (verticalAxis + i >= surfacePlate.GetLength(0) || horizontalAxis - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (surfacePlate[verticalAxis + i, horizontalAxis - i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (surfacePlate[verticalAxis + i, horizontalAxis - i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate, coler, verticalAxis, horizontalAxis);
                break;
            }
        }
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="whiteSurfacePlate">白の置けるマスの格納</param>
    /// <param name="blackSurfacePlate">黒の置けるマスの格納</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    private void Seach8(int[,] surfacePlate, int[,] whiteSurfacePlate, int[,] blackSurfacePlate, int verticalAxis, int horizontalAxis)
    {
        // 左下の色を見る
        int coler = surfacePlate[verticalAxis - 1, horizontalAxis - 1];

        // 色を見た隣の色を見ていく
        for (int i = 2; i < surfacePlate.GetLength(0); i++)
        {
            // 範囲外なら探索をやめる
            if (verticalAxis - i < 0 || horizontalAxis - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (surfacePlate[verticalAxis - i, horizontalAxis - i] == 0)
            {
                break;
            }
            // 別の色(白なら黒、黒なら白)があったとき
            else if (surfacePlate[verticalAxis - i, horizontalAxis - i] == -coler)
            {
                // 位置と色を渡す
                PutStoneColer(whiteSurfacePlate, blackSurfacePlate,coler, verticalAxis, horizontalAxis);
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
            blackSurfacePlate[x, z] = PUT_STONE;
        }
        //
        else if (coler == WHITE_STONE_INDEX)
        {
            whiteSurfacePlate[x, z] = PUT_STONE;
        }
    }

    #endregion
}
