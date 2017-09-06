using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    // 速度
    public Vector2 SPEED = new Vector2(0.01f, 0.01f);
    public int wall_Left = 13;
    public int wall_Right = 450;
    public int wall_Bottom = 40;
    public int wall_Top = 230;
    public GameObject gameObj;
    Vector3 targetPos=Vector3.zero;
    Vector3 worldMousePos = Vector3.zero;
   public bool click;

    Animator anim;


    // Use this for initialization
    void Start()
    {
        //Animatorをキャッシュ
        anim = GetComponent<Animator>();
        click = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Hashtable hash = new Hashtable();
        ////hash.Add("position", worldMousePos);
        //hash.Add("time", 2.0f);
        //hash.Add("oncomplete", "OnCompleteCallback");
        //hash.Add("oncompleteparams", false);
        //hash.Add("oncompletetarget", this);


        // 移動処理
        //Move();
        //左クリックしたら、そっちの方向に移動 
        if (click)
        {
            //ジャンプアニメーションの開始
            anim.SetTrigger("run");
        }

        if (Input.GetMouseButton(0))
        {
            if (!click)
            {
                click = true;
            }
            else if (click)
            {

            }
            ////ジャンプアニメーションの開始
            //anim.SetBool("run 0", true);

            Hashtable hash = new Hashtable();
            //hash.Add("position", worldMousePos);
            hash.Add("time", 2.0f);
            hash.Add("oncomplete", "OnCompleteCallback");
            hash.Add("oncompleteparams", false);
            hash.Add("oncompletetarget", gameObject);

            //クリックした位置を目標位置に設定
            targetPos = Input.mousePosition;
            //ワールド座標に変換
            worldMousePos = Camera.main.ScreenToWorldPoint(targetPos);
            worldMousePos.z = 10f;

            hash.Add("position", worldMousePos);

            //動く
            iTween.MoveUpdate(this.gameObject, hash);
        }

        //壁処理（Mashf.Clamp(制限する座標値, 最小値, 最大値)
        this.transform.position = (new Vector3(Mathf.Clamp(this.transform.position.x, wall_Left, wall_Right),
           Mathf.Clamp(this.transform.position.y, wall_Bottom, wall_Top),
           this.transform.position.z));
    }

    // 移動関数
    void Move()
    {
        // 現在位置をPositionに代入
        Vector2 Position = transform.position;
        // 左キーを押し続けていたら
        if (Input.GetKey("left"))
        {
            // 代入したPositionに対して加算減算を行う
            Position.x -= SPEED.x;
        }
        else if (Input.GetKey("right"))
        { // 右キーを押し続けていたら
          // 代入したPositionに対して加算減算を行う
            Position.x += SPEED.x;
        }
        else if (Input.GetKey("up"))
        { // 上キーを押し続けていたら
          // 代入したPositionに対して加算減算を行う
            Position.y += SPEED.y;
        }
        else if (Input.GetKey("down"))
        { // 下キーを押し続けていたら
          // 代入したPositionに対して加算減算を行う
            Position.y -= SPEED.y;
        }
        // 現在の位置に加算減算を行ったPositionを代入する
        transform.position = Position;
    }

    void OnCompleteCallback(bool nextClick)
    {
        Debug.Log("nextClick");
        click = false;
    }

}
