using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    // 速度
    public Vector2 SPEED = new Vector2(0.01f, 0.01f);

    //壁判定用
    public int wall_Left = 13;
    public int wall_Right = 450;
    public int wall_Bottom = 40;
    public int wall_Top = 230;

    //移動用ポジション（初期値は(0,0,0)）
    Vector3 targetPos = Vector3.zero;
    Vector3 worldMousePos = Vector3.zero;

    //アニメーション用フラグ
    public bool click;

    //Animator
    Animator anim;


    // Use this for initialization
    void Start()
    {
        //Animatorをキャッシュ
        anim = GetComponent<Animator>();
        //フラグはfalse
        click = false;
    }

    // Update is called once per frame
    void Update()
    {
        //歩くアニメーション(clickフラグがtrueなら動く、falseなら動かない)
        anim.SetBool("run 0", click);
        ////移動中なら
        //if (click)
        //{
        //    //anim.SetTrigger("run");
        //}

        //左クリックしたら、そっちの方向に移動 
        if (Input.GetMouseButton(0))
        {
            //移動開始
            click = true;

            //クリックした位置を目標位置に設定
            targetPos = Input.mousePosition;

            //ワールド座標に変換
            worldMousePos = Camera.main.ScreenToWorldPoint(targetPos);
            worldMousePos.z = 10f;

            //動く
            iTween.MoveTo(this.gameObject, iTween.Hash(
                "position", worldMousePos,
                "time", 0.5,
                "oncomplete", "OnCompleteCallback",
                "oncompletetarget", this.gameObject,
                "easeType", "linear"));
        }

            //if(this.transform.position == worldMousePos)
            //{
            //    click = false;
            //}

        //壁処理（Mashf.Clamp(制限する座標値, 最小値, 最大値)
        this.transform.position = (new Vector3(Mathf.Clamp(this.transform.position.x, wall_Left, wall_Right),
           Mathf.Clamp(this.transform.position.y, wall_Bottom, wall_Top),
           this.transform.position.z));
    }

    //動くのが終わった後に呼ばれる関数
    void OnCompleteCallback()
    {
        //デバック用
        Debug.Log("animatingStop");
        //移動フラグをfalse1
        click = false;
    }

}
