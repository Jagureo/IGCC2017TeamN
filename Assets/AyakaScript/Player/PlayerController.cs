using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    // 速度
    public Vector2 SPEED = new Vector2(0.01f, 0.01f);

    //壁判定用
    public int wall_Left = 200;
    public int wall_Right = 460;
    public int wall_Bottom = 40;
    public int wall_Top = 230;

    //性別(0:Boy、1:Girl）
    public int gender;

    //移動用ポジション（初期値は(0,0,0)）
    Vector3 targetPos = Vector3.zero;
    Vector3 worldMousePos = Vector3.zero;

    //アニメーション用フラグ
    public bool click;
    public bool left;

    //Animator
    Animator anim;

    //衝突しているか否か
    public bool collisionFlug = false;
    string tagName = null;

    // Use this for initialization
    void Start()
    {
        //Animatorをキャッシュ
        anim = GetComponent<Animator>();
        //フラグはfalse
        click = false;
        left = false;

        //デバック用（性別を男性にした）
        gender = 0;

        //性別をセット
        anim.SetInteger("gender", gender);
    }

    // Update is called once per frame
    void Update()
    {
        //歩くアニメーション(clickフラグがtrueなら動く、falseなら動かない)
        anim.SetBool("run 0", click);
        anim.SetBool("run 1", left);

        ////移動中なら
        //if (click)
        //{
        //    //anim.SetTrigger("run");
        //}

        //左クリックしたら、そっちの方向に移動 
        if (Input.GetMouseButton(0))
        {
            ////移動開始
            //click = true;
            if (!collisionFlug)
            {
                //クリックした位置を目標位置に設定
                targetPos = Input.mousePosition;

                //ワールド座標に変換
                worldMousePos = Camera.main.ScreenToWorldPoint(targetPos);
                worldMousePos.z = 10f;

                //マウスの座標と現在の座標から、どの方向に動いているか確認
                //Left
                if (worldMousePos.x < transform.position.x)
                {
                    //移動開始
                    left = true;
                }
                //Right
                else
                {
                    //移動開始
                    click = true;
                }

                //動く
                iTween.MoveTo(this.gameObject, iTween.Hash(
                    "position", worldMousePos,
                    "time", 0.5f,
                    "oncomplete", "OnCompleteCallback",
                    "oncompletetarget", this.gameObject,
                    "easeType", "linear"));
            }
            else
            {
                //クリックして、オブジェクトがあったら
                GameObject obj = getClickObject();
                if(obj != null)
                {
                    //タグが、衝突判定と同じタグだったら
                    if (obj.tag == tagName)
                    {
                        Debug.Log("取得");
                        tagName = null;
                    }
                }
            }
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
        //Debug.Log("animatingStop");
        //移動フラグをfalse
        click = false;
        left = false;
    }


    //---------------------------------------------//   
    //  衝突判定
    //---------------------------------------------//   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //tagName = collision.gameObject.tag;
        //Vector2 position = new Vector2(0, 0);
        //position = collision.contacts[0].point;
        //Debug.Log(position);
        //collisionFlug = true;
        //position = new Vector2(position.x, position.y);
        //iTween.MoveTo(this.gameObject, iTween.Hash(
        //    "position", position,
        //    "time", 0.01f,
        //    "oncomplete", "OnCompleteCallback",
        //    "oncompletetarget", this.gameObject,
        //    "easeType", "linear"));
        //Debug.Log("衝突");
    }


    //衝突、動くのが終わった後に呼ばれる関数
    void OnCollision()
    {
        //デバック用
        //Debug.Log("animatingStop");
        //移動フラグをfalse
        click = false;
        left = false;

    }

    // 左クリックしたオブジェクトを取得する関数(2D)
    private GameObject getClickObject()
    {
        GameObject result = null;
        // 左クリックされた場所のオブジェクトを取得
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
            if (collition2d)
            {
                result = collition2d.transform.gameObject;
            }
        }
        return result;
    }

}
