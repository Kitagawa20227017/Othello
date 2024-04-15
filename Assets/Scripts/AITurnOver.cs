// ---------------------------------------------------------  
// AITurnOver.cs  
// 
// 石を裏返し評価を求める処理
// １つ下の階層を見るときはAISeach.Seachを呼んでいる
// 石の評価については、下記URLを参考にしています。
// https://uguisu.skr.jp/othello/5-1.html
//
// 作成日: 2024/4/9
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;

public class AITurnOver : MonoBehaviour
{

    #region 変数  

    #region 定数

    // 白の石の値
    private const int WHITE_STONE_INDEX = -1;

    // 黒の石の値
    private const int BLACK_STONE_INDEX = 1;

    // 盤面のマス数
    private const int MASS_NUMBER = 8;

    // 最大スコア初期値
    private const int INITIAL_VALUE = -1000;

    #endregion

    // マスの評価値
    readonly private int[,] EVALUATION_VALUE = new int[,]
    {
        { 40,-12,  0, -1, -1,  0, -12,  40},
        {-12,-15, -3, -3, -3, -3, -15, -12},
        {  0, -3,  0, -1, -1,  0,  -3,   0},
        { -1, -3, -1, -1, -1, -1,  -3,  -1},
        { -1, -3, -1, -1, -1, -1,  -3,  -1},
        {  0, -3,  0, -1, -1,  0,  -3,   0},
        {-12,-15, -3, -3, -3, -3, -15, -12},
        { 40,-12,  0, -1, -1,  0, -12,  40},
    };

    [SerializeField,Header("読む手数")]
    private int _depth = 2;

    // AISeach取得用
    private AISeach _AISeach = default;

    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        // 初期設定
        _AISeach = this.gameObject.GetComponent<AISeach>();
    }

    /// <summary>
    /// 石を裏返す処理
    /// </summary>
    /// <param name="surfacePlate">盤面の配列</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    /// <param name="myStoneColer">AIの石の色</param>
    /// <param name="turnStoneColer">ターン中の石の色</param>
    /// <param name="conut">手数</param>
    /// <returns>AIの合計スコア</returns>
    public int TurnOver(int[,] surfacePlate, int verticalAxis, int horizontalAxis, int myStoneColer,int turnStoneColer, int conut)
    {
        // コピー用の配列生成
        int[,] copy = new int[MASS_NUMBER, MASS_NUMBER];

        // 現在の盤面をコピー
        for (int i = 0; i < surfacePlate.GetLength(0); i++)
        {
            for (int j = 0; j < surfacePlate.GetLength(1); j++)
            {
                copy[i, j] = surfacePlate[i, j];
            }
        }
        
        // 手数を増やす
        conut++;

        // 現在のマスが範囲内で右隣に石があるとき
        if (horizontalAxis != surfacePlate.GetLength(1) - 1 && surfacePlate[verticalAxis, horizontalAxis + 1] != turnStoneColer && surfacePlate[verticalAxis, horizontalAxis + 1] != 0)
        {
            surfacePlate = Seach1(copy, verticalAxis, horizontalAxis, turnStoneColer);
        }

        // 現在のマスが範囲内で上に石があるとき
        if (verticalAxis != surfacePlate.GetLength(0) - 1 && surfacePlate[verticalAxis + 1, horizontalAxis] != turnStoneColer && surfacePlate[verticalAxis + 1, horizontalAxis] != 0)
        {
            surfacePlate = Seach2(copy, verticalAxis, horizontalAxis, turnStoneColer);
        }

        // 現在のマスが範囲内で左に石があるとき
        if (horizontalAxis != 0 && surfacePlate[verticalAxis, horizontalAxis - 1] != turnStoneColer && surfacePlate[verticalAxis, horizontalAxis - 1] != 0)
        {
            surfacePlate = Seach3(copy, verticalAxis, horizontalAxis, turnStoneColer);
        }

        // 現在のマスが範囲内で下に石があるとき
        if (verticalAxis != 0 && surfacePlate[verticalAxis - 1, horizontalAxis] != turnStoneColer && surfacePlate[verticalAxis - 1, horizontalAxis] != 0)
        {
            surfacePlate = Seach4(copy, verticalAxis, horizontalAxis, turnStoneColer);
        }

        // 現在のマスが範囲内で左下に石があるとき
        if (verticalAxis != surfacePlate.GetLength(0) - 1 && horizontalAxis != surfacePlate.GetLength(1) - 1 && surfacePlate[verticalAxis + 1, horizontalAxis + 1] != turnStoneColer && surfacePlate[verticalAxis + 1, horizontalAxis + 1] != 0)
        {
            surfacePlate = Seach5(copy, verticalAxis, horizontalAxis, turnStoneColer);
        }

        // 現在のマスが範囲内で右上に石があるとき
        if (verticalAxis != 0 && horizontalAxis != surfacePlate.GetLength(1) - 1 && surfacePlate[verticalAxis - 1, horizontalAxis + 1] != turnStoneColer && surfacePlate[verticalAxis - 1, horizontalAxis + 1] != 0)
        {
            surfacePlate = Seach6(copy, verticalAxis, horizontalAxis, turnStoneColer);
        }

        // 現在のマスが範囲内で右下に石があるとき
        if (verticalAxis != surfacePlate.GetLength(0) - 1 && horizontalAxis != 0 && surfacePlate[verticalAxis + 1, horizontalAxis - 1] != turnStoneColer && surfacePlate[verticalAxis + 1, horizontalAxis - 1] != 0)
        {
            surfacePlate = Seach7(copy, verticalAxis, horizontalAxis, turnStoneColer);
        }

        // 現在のマスが範囲内で左上に石があるとき
        if (verticalAxis != 0 && horizontalAxis != 0 && surfacePlate[verticalAxis - 1, horizontalAxis - 1] != turnStoneColer && surfacePlate[verticalAxis - 1, horizontalAxis - 1] != 0)
        {
            surfacePlate = Seach8(copy, verticalAxis, horizontalAxis, turnStoneColer);
        }

        // 石を置く
        surfacePlate[verticalAxis, horizontalAxis] = turnStoneColer;

        // 下の階層があるとき
        if (conut < _depth)
        {
            return _AISeach.Seach(surfacePlate, myStoneColer, -turnStoneColer,conut);
        }
        // 一番下の階層のとき
        else
        {
            return Score(surfacePlate,turnStoneColer);
        }
    }
    /// <summary>
    /// スコアを求める処理
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="myStoneColer">自分の石の色</param>
    /// <returns>スコア</returns>
    private int Score(int[,] surfacePlate, int myStoneColer)
    {
        // 初期設定

        // スコア代入用
        int score = INITIAL_VALUE;

        // 白の石の合計スコア
        int whiteScore = 0;

        // 黒の石の合計スコア
        int blackScore = 0;

        // スコア計算
        for (int i = 0; i < surfacePlate.GetLength(0); i++)
        {
            for(int j = 0; j < surfacePlate.GetLength(1);j++)
            {
                // 白の石のとき
                if(surfacePlate[i,j] == WHITE_STONE_INDEX)
                {
                    whiteScore += EVALUATION_VALUE[i, j];
                }
                // 黒の石のとき
                else if(surfacePlate[i,j] == BLACK_STONE_INDEX)
                {
                    blackScore += EVALUATION_VALUE[i, j];
                }
            }
        }

        // 自分の石が黒の石のとき
        if(myStoneColer == BLACK_STONE_INDEX)
        {
            score = whiteScore - blackScore;
        }
        // 自分の石が白の石のとき
        else if (myStoneColer == WHITE_STONE_INDEX)
        {
            score = blackScore - whiteScore;
        }

        // スコアを返す
        return score;
    }

    /// <summary>
    /// 右隣の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    /// <param name="coler">石の色</param>
    /// <returns>盤面情報</returns>
    private int[,] Seach1(int[,] surfacePlate, int verticalAxis, int horizontalAxis, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = horizontalAxis + 2; i < surfacePlate.GetLength(1); i++)
        {
            // 何もなかったとき
            if (surfacePlate[verticalAxis, i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (surfacePlate[verticalAxis, i] == coler)
            {
                a = i - horizontalAxis - 1;
                break;
            }
        }

        // 石を裏返す
        for (int i = 1; i <= a; i++)
        {
            // 盤面情報更新
            surfacePlate[verticalAxis, horizontalAxis + i] = -surfacePlate[verticalAxis, horizontalAxis + i];
        }
        // 盤面情報を返す
        return surfacePlate;
    }

    /// <summary>
    /// 左隣の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    /// <param name="coler">石の色</param>
    /// <returns>盤面情報</returns>
    private int[,] Seach2(int[,] surfacePlate, int verticalAxis, int horizontalAxis, int coler)
    {
        int a = default;
        // 隣の色を見ていく
        for (int i = verticalAxis + 2; i < surfacePlate.GetLength(1); i++)
        {
            // 何もなかったとき
            if (surfacePlate[i, horizontalAxis] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (surfacePlate[i, horizontalAxis] == coler)
            {
                a = i - verticalAxis - 1;
                break;
            }
        }

        // 石を裏返す
        for (int i = 1; i <= a; i++)
        {
            // 盤面情報更新
            surfacePlate[verticalAxis + i, horizontalAxis] = -surfacePlate[verticalAxis + i, horizontalAxis];
        }

        // 盤面情報を返す
        return surfacePlate;
    }

    /// <summary>
    /// 下の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    /// <param name="coler">石の色</param>
    /// <returns>盤面情報</returns>
    private int[,] Seach3(int[,] surfacePlate, int verticalAxis, int horizontalAxis, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = horizontalAxis - 2; i > 0; i--)
        {
            // 何もなかったとき
            if (surfacePlate[verticalAxis, i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (surfacePlate[verticalAxis, i] == coler)
            {
                a = horizontalAxis - i - 1;
                break;
            }
        }

        // 石を裏返す
        for (int i = 1; i <= a; i++)
        {
            // 盤面情報更新
            surfacePlate[verticalAxis, horizontalAxis - i] = -surfacePlate[verticalAxis, horizontalAxis - i];
        }

        // 盤面情報を返す
        return surfacePlate;
    }

    /// <summary>
    /// 上の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    /// <param name="coler">石の色</param>
    /// <returns>盤面情報</returns>
    private int[,] Seach4(int[,] surfacePlate, int verticalAxis, int horizontalAxis, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = verticalAxis - 2; i > 0; i--)
        {
            // 何もなかったとき
            if (surfacePlate[i, horizontalAxis] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (surfacePlate[i, horizontalAxis] == coler)
            {
                a = verticalAxis - i - 1;
                break;
            }
        }

        // 石を裏返す
        for (int i = 1; i <= a; i++)
        {
            // 盤面情報更新
            surfacePlate[verticalAxis - i, horizontalAxis] = -surfacePlate[verticalAxis - i, horizontalAxis];
        }

        // 盤面情報を返す
        return surfacePlate;
    }

    /// <summary>
    /// 右下の探索
    /// <param name="surfacePlate">盤面情報</param>
    /// </summary>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    /// <param name="coler">石の色</param>
    /// <returns>盤面情報</returns>
    private int[,] Seach5(int[,] surfacePlate, int verticalAxis, int horizontalAxis, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < surfacePlate.GetLength(1); i++)
        {
            if (verticalAxis + i >= surfacePlate.GetLength(0) || horizontalAxis + i >= surfacePlate.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (surfacePlate[verticalAxis + i, horizontalAxis + i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (surfacePlate[verticalAxis + i, horizontalAxis + i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        // 石を裏返す
        for (int i = 1; i <= a; i++)
        {
            // 盤面情報更新
            surfacePlate[verticalAxis + i, horizontalAxis + i] = -surfacePlate[verticalAxis + i, horizontalAxis + i];
        }

        // 盤面情報を返す
        return surfacePlate;
    }

    /// <summary>
    /// 右上の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    /// <param name="coler">石の色</param>
    /// <returns>盤面情報</returns>
    private int[,] Seach6(int[,] surfacePlate, int verticalAxis, int horizontalAxis, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < surfacePlate.GetLength(1); i++)
        {
            if (verticalAxis - i < 0 || horizontalAxis + i >= surfacePlate.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (surfacePlate[verticalAxis - i, horizontalAxis + i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (surfacePlate[verticalAxis - i, horizontalAxis + i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        // 石を裏返す
        for (int i = 1; i <= a; i++)
        {
            // 盤面情報更新
            surfacePlate[verticalAxis - i, horizontalAxis + i] = -surfacePlate[verticalAxis - i, horizontalAxis + i];
        }

        // 盤面情報を返す
        return surfacePlate;
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    /// <param name="coler">石の色</param>
    /// <returns>盤面情報</returns>
    private int[,] Seach7(int[,] surfacePlate, int verticalAxis, int horizontalAxis, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < surfacePlate.GetLength(1); i++)
        {
            if (verticalAxis + i >= surfacePlate.GetLength(0) || horizontalAxis - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (surfacePlate[verticalAxis + i, horizontalAxis - i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (surfacePlate[verticalAxis + i, horizontalAxis - i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        // 石を裏返す
        for (int i = 1; i <= a; i++)
        {
            // 盤面情報更新
            surfacePlate[verticalAxis + i, horizontalAxis - i] = -surfacePlate[verticalAxis + i, horizontalAxis - i];
        }

        // 盤面情報を返す
        return surfacePlate;
    }

    /// <summary>
    /// 左上の探索
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="verticalAxis">縦軸</param>
    /// <param name="horizontalAxis">横軸</param>
    /// <param name="coler">石の色</param>
    /// <returns>盤面情報</returns>
    private int[,] Seach8(int[,] surfacePlate, int verticalAxis, int horizontalAxis, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < surfacePlate.GetLength(1); i++)
        {
            if (verticalAxis - i < 0 || horizontalAxis - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (surfacePlate[verticalAxis - i, horizontalAxis - i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (surfacePlate[verticalAxis - i, horizontalAxis - i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        // 石を裏返す
        for (int i = 1; i <= a; i++)
        {
            // 盤面情報更新
            surfacePlate[verticalAxis - i, horizontalAxis - i] = -surfacePlate[verticalAxis - i, horizontalAxis - i];
        }

        // 盤面情報を返す
        return surfacePlate;
    }
    #endregion

}
