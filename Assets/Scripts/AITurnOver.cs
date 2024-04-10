// ---------------------------------------------------------  
// MyScript3.cs  
// 
//
//
// 作成日: 
// 作成者: 
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class AITurnOver : MonoBehaviour
{

    #region 変数  

    [SerializeField,Header("読む手数")]
    private int _depth = 2;

    readonly private int[,] EVALUATION_VALUE = new int[,]
    {
        { 30,-12,  0, -1, -1,  0, -12,  30},
        {-12,-15, -3, -3, -3, -3, -15, -12},
        {  0, -3,  0, -1, -1,  0,  -3,   0},
        { -1, -3, -1, -1, -1, -1,  -3,  -1},
        { -1, -3, -1, -1, -1, -1,  -3,  -1},
        {  0, -3,  0, -1, -1,  0,  -3,   0},
        {-12,-15, -3, -3, -3, -3, -15, -12},
        { 30,-12,  0, -1, -1,  0, -12,  30},
    };

    private AISeach _myScript = default;

    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        _myScript = this.gameObject.GetComponent<AISeach>();
    }

    public int TurnOver(int[,] n, int x, int z, int myStoneColer,int turnStoneColer, int conut)
    {

        int[,] copy = new int[8, 8];
        for (int i = 0; i < n.GetLength(0); i++)
        {
            for (int j = 0; j < n.GetLength(1); j++)
            {
                copy[i, j] = n[i, j];
            }
        }
        
        conut++;
        // 現在のマスが範囲内で右隣に石があるとき
        if (z != n.GetLength(1) - 1 && n[x, z + 1] != turnStoneColer && n[x, z + 1] != 0)
        {
            n = Seach1(copy, x, z, turnStoneColer);
        }

        // 現在のマスが範囲内で左隣に石があるとき
        if (x != n.GetLength(0) - 1 && n[x + 1, z] != turnStoneColer && n[x + 1, z] != 0)
        {
            n = Seach2(copy, x, z, turnStoneColer);
        }

        // 現在のマスが範囲内で下に石があるとき
        if (z != 0 && n[x, z - 1] != turnStoneColer && n[x, z - 1] != 0)
        {
            n = Seach3(copy, x, z, turnStoneColer);
        }

        // 現在のマスが範囲内で上に石があるとき
        if (x != 0 && n[x - 1, z] != turnStoneColer && n[x - 1, z] != 0)
        {
            n = Seach4(copy, x, z, turnStoneColer);
        }

        // 現在のマスが範囲内で左下に石があるとき
        if (x != n.GetLength(0) - 1 && z != n.GetLength(1) - 1 && n[x + 1, z + 1] != turnStoneColer && n[x + 1, z + 1] != 0)
        {
            n = Seach5(copy, x, z, turnStoneColer);
        }

        // 現在のマスが範囲内で右上に石があるとき
        if (x != 0 && z != n.GetLength(1) - 1 && n[x - 1, z + 1] != turnStoneColer && n[x - 1, z + 1] != 0)
        {
            n = Seach6(copy, x, z, turnStoneColer);
        }

        // 現在のマスが範囲内で右下に石があるとき
        if (x != n.GetLength(0) - 1 && z != 0 && n[x + 1, z - 1] != turnStoneColer && n[x + 1, z - 1] != 0)
        {
            n = Seach7(copy, x, z, turnStoneColer);
        }

        // 現在のマスが範囲内で左上に石があるとき
        if (x != 0 && z != 0 && n[x - 1, z - 1] != turnStoneColer && n[x - 1, z - 1] != 0)
        {
            n = Seach8(copy, x, z, turnStoneColer);
        }

        n[x, z] = turnStoneColer;

        if (conut < _depth)
        {
            return _myScript.Seach(n, myStoneColer, -turnStoneColer,conut);
        }
        else
        {
            return Score(n,turnStoneColer);
        }
    }


    private int Score(int[,] n, int myStoneColer)
    {
        int score = -1000;
        int whiteScore = 0;
        int blackScore = 0;
        for (int i = 0; i < n.GetLength(0); i++)
        {
            for(int j = 0; j < n.GetLength(1);j++)
            {
                if(n[i,j] == -1)
                {
                    whiteScore += EVALUATION_VALUE[i, j];
                }
                else if(n[i,j] == 1)
                {
                    blackScore += EVALUATION_VALUE[i, j];
                }
            }
        }

        if(myStoneColer == -1)
        {
            score = whiteScore - blackScore;
        }
        else if (myStoneColer == 1)
        {
            score = blackScore - whiteScore;
        }
        return score;
    }

    /// <summary>
    /// 右隣の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private int[,] Seach1(int[,] n, int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = z + 2; i < n.GetLength(1); i++)
        {
            // 何もなかったとき
            if (n[x, i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (n[x, i] == coler)
            {
                a = i - z - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            n[x, z + i] = -n[x, z + i];
        }

        return n;
    }

    /// <summary>
    /// 左隣の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private int[,] Seach2(int[,] c, int x, int z, int coler)
    {
        int a = default;
        // 隣の色を見ていく
        for (int i = x + 2; i < c.GetLength(1); i++)
        {
            // 何もなかったとき
            if (c[i, z] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (c[i, z] == coler)
            {
                a = i - x - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            c[x + i, z] = -c[x + i, z];
        }
        return c;
    }

    /// <summary>
    /// 下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private int[,] Seach3(int[,] n, int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = z - 2; i > 0; i--)
        {
            // 何もなかったとき
            if (n[x, i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (n[x, i] == coler)
            {
                a = z - i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            n[x, z - i] = -n[x, z - i];
        }
        return n;
    }

    /// <summary>
    /// 上の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private int[,] Seach4(int[,] n, int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = x - 2; i > 0; i--)
        {
            // 何もなかったとき
            if (n[i, z] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (n[i, z] == coler)
            {
                a = x - i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            n[x - i, z] = -n[x - i, z];
        }
        return n;
    }

    /// <summary>
    /// 右下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private int[,] Seach5(int[,] n, int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < n.GetLength(1); i++)
        {
            if (x + i >= n.GetLength(0) || z + i >= n.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (n[x + i, z + i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (n[x + i, z + i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            n[x + i, z + i] = -n[x + i, z + i];
        }
        return n;
    }

    /// <summary>
    /// 右上の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private int[,] Seach6(int[,] n, int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < n.GetLength(1); i++)
        {
            if (x - i < 0 || z + i >= n.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (n[x - i, z + i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (n[x - i, z + i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            n[x - i, z + i] = -n[x - i, z + i];
        }
        return n;
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private int[,] Seach7(int[,] n, int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < n.GetLength(1); i++)
        {
            if (x + i >= n.GetLength(0) || z - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (n[x + i, z - i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (n[x + i, z - i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            n[x + i, z - i] = -n[x + i, z - i];
        }

        return n;
    }

    /// <summary>
    /// 左上の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private int[,] Seach8(int[,] n, int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < n.GetLength(1); i++)
        {
            if (x - i < 0 || z - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (n[x - i, z - i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (n[x - i, z - i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            n[x - i, z - i] = -n[x - i, z - i];
        }

        return n;
    }
    #endregion

}
