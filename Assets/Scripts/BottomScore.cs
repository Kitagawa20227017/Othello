// ---------------------------------------------------------  
// BottomScore.cs  
// 
// 一番下の階層のスコアを求める処理
//
// 作成日: 2024/4/9
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;

public class BottomScore : MonoBehaviour
{

    #region 変数  

    // AITurnOver取得用
    private AITurnOver _myScript = default;

    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        _myScript = this.gameObject.GetComponent<AITurnOver>();
    }

    /// <summary>
    /// 一番下の階層のスコアを求める
    /// </summary>
    /// <param name="surfacePlate">盤面情報</param>
    /// <param name="myPut">置けるマス</param>
    /// <param name="myStoneColer">AIの石の色</param>
    /// <param name="turnStoneColer">ターン中の石の色</param>
    /// <param name="conut">手数</param>
    /// <returns>最大スコア</returns>
    public int MaxScore(int[,] surfacePlate, int[,] myPut, int myStoneColer, int turnStoneColer, int conut)
    {
        // 初期設定

        // スコアの初期値
        const int SCORE_INITIAL = -10000;

        // スコア代入用
        int score = 0;

        // 小さい値を代入しておく 
        int maxScore = SCORE_INITIAL;
        
        //　スコア更新フラグ
        bool isUpdateScore = false;

        // 置けるマスを調べて見る
        for(int i = 0; i < myPut.GetLength(0);i++)
        {
            for(int j = 0;j< myPut.GetLength(1);j++)
            {
                // 置けるマスがあったとき
                if(myPut[i,j] == 1)
                {
                    // フラグを立てる
                    isUpdateScore = true;

                    // スコア代入
                    score = -_myScript.TurnOver(surfacePlate, i, j, myStoneColer, turnStoneColer, conut);
                }

                // スコアが更新されて最大スコアよりも大きいとき
                if (score > maxScore && isUpdateScore)
                {
                    isUpdateScore = false;
                    maxScore = score;
                }
            }
        }

        // 最大スコアを返す
        return maxScore;
    }


    #endregion

}
