using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //取得した際の画像変更用
    SpriteRenderer FloorSpriteRenderer;
    SpriteRenderer BedSpriteRenderer;
    SpriteRenderer SidetableSpriteRenderer;
    SpriteRenderer DeskSpriteRenderer;
    SpriteRenderer ChestSpriteRenderer;
    SpriteRenderer ToolboxSpriteRenderer;
    SpriteRenderer DoorSpriteRenderer;

    public Sprite BedPut;
    public Sprite SidetablePut;
    public Sprite DeskPut;
    public Sprite ChestPut;
    public Sprite Chest2Put;
    public Sprite ToolboxPut;
    public Sprite Toolbox2Put;
    public Sprite DoorPut;

    public Sprite FloorBoy;
    public Sprite BedBoy;
    public Sprite SidetableBoy;
    public Sprite DeskBoy;
    public Sprite ChestBoy;
    public Sprite ToolboxBoy;
    public Sprite DoorBoy;

    public Sprite FloorLady;
    public Sprite BedLady;
    public Sprite SidetableLady;
    public Sprite DeskLady;
    public Sprite ChestLady;
    public Sprite ToolboxLady;
    public Sprite DoorLady;

    public Sprite BedPutLady;
    public Sprite SidetablePutLady;
    public Sprite DeskPutLady;
    public Sprite ChestPutLady;
    public Sprite Chest2PutLady;
    public Sprite ToolboxPutLady;
    public Sprite Toolbox2PutLady;
    public Sprite DoorPutLady;


    GameObject[] floors;
    GameObject[] beds;
    GameObject[] sidetables;
    GameObject[] desks;
    GameObject[] chests;
    GameObject[] toolboxs;
    GameObject[] doors;

    bool chestGet = false;
    bool toolboxGet = false;
    int toolboxTime = 0;
    int chestTime = 0;


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
    public bool right;
    public bool left;
    public bool up;
    public bool down;


    //Animator
    Animator anim;

    //衝突しているか否か
    public bool collisionFlug = false;
    string tagName = null;


    // Use this for initialization
    void Start()
    {
        //画像変更用の各種設定関数
        SpriteInitialize();

        //Animatorをキャッシュ
        anim = GetComponent<Animator>();
        //フラグはfalse
        right = false;
        left = false;

        //デバック用（性別を男性にした）
        gender = 0;

        //性別をセット
        anim.SetInteger("gender", gender);
        //画像を性別に合わせる
        SpriteGender();
    }

    // Update is called once per frame
    void Update()
    {
        //歩くアニメーション(rightフラグがtrueなら動く、falseなら動かない)
        anim.SetBool("run 0", right);
        anim.SetBool("run 1", left);
        anim.SetBool("run 2", up);
        anim.SetBool("run 3", down);


        ////移動中なら
        //if (right)
        //{
        //    //anim.SetTrigger("run");
        //}

        //左クリックしたら、そっちの方向に移動 
        if (Input.GetMouseButton(0))
        {
            if (!collisionFlug)
            {
                ////移動開始
                //right = true;
                //クリックした位置を目標位置に設定
                targetPos = Input.mousePosition;

                //ワールド座標に変換
                worldMousePos = Camera.main.ScreenToWorldPoint(targetPos);
                worldMousePos.z = 10f;

                // 自分とターゲットとなる相手との方向を求める
                Vector3 direction = (this.transform.position - worldMousePos).normalized;

                MoveAngle(direction);

                //マウスの座標と現在の座標から、どの方向に動いているか確認
                ////Left
                //if (worldMousePos.x < transform.position.x)
                //{
                //    //移動開始
                //    left = true;
                //}
                ////Right
                //else
                //{
                //    //移動開始
                //    right = true;
                //}

                //動く
                iTween.MoveTo(this.gameObject, iTween.Hash(
                    "position", worldMousePos,
                    "time", 0.5f,
                    "oncomplete", "OnCompleteCallback",
                    "oncompletetarget", this.gameObject,
                    "easeType", "linear"));
            }
        }
        //if (Input.GetMouseButton(0))
        //{
        //    //クリックして、オブジェクトがあったら
        //    GameObject obj = getrightObject();
        //    if (obj != null)
        //    {
        //        //タグが、衝突判定と同じタグだったら
        //        if (obj.tag == tagName)
        //        {
        //            Debug.Log("取得");
        //            tagName = null;
        //        }
        //    }

        //}

        //if(this.transform.position == worldMousePos)
        //{
        //    right = false;
        //}

        //棚のアニメーション
        if (chestGet)
        {
            ChestChangeSprite();
        }
        //ツールボックスのアニメーション
        if (toolboxGet)
        {
            ToolboxChangeSprite();
        }

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
        right = false;
        left = false;
        up = false;
        down = false;

    }


    //---------------------------------------------//   
    //  衝突判定
    //---------------------------------------------//   

    private void OnCollisionStay2D(Collision2D collision)
    {
        collisionFlug = true;
        tagName = collision.gameObject.tag;
        if (Input.GetMouseButton(0))
        {
            //クリックして、オブジェクトがあったら
            GameObject obj = getrightObject();
            if (obj != null)
            {
                //タグが、衝突判定と同じタグだったら
                if (obj.tag == tagName)
                {
                    Debug.Log("取得");
                    anim.SetTrigger("reach");
                    if (gender == 0)
                    {
                        switch (tagName)
                        {
                            case "Bed":
                                BedSpriteRenderer.sprite = BedPut;
                                break;
                            case "sidetable":
                                SidetableSpriteRenderer.sprite = SidetablePut;
                                break;
                            case "desk":
                                DeskSpriteRenderer.sprite = DeskPut;
                                break;
                            case "chest":
                                ChestSpriteRenderer.sprite = ChestPut;
                                chestGet = true;
                                break;
                            case "toolbox":
                                ToolboxSpriteRenderer.sprite = ToolboxPut;
                                toolboxGet = true;
                                break;
                            case "door":
                                doors[0].transform.position = new Vector3(446f, doors[0].transform.position.y);
                                DoorSpriteRenderer.sprite = DoorPut;
                                break;
                        }
                        tagName = null;
                    }

                    else if (gender == 1)
                    {
                        switch (tagName)
                        {
                            case "Bed":
                                BedSpriteRenderer.sprite = BedPutLady;
                                break;
                            case "sidetable":
                                SidetableSpriteRenderer.sprite = SidetablePutLady;
                                break;
                            case "desk":
                                DeskSpriteRenderer.sprite = DeskPutLady;
                                break;
                            case "chest":
                                //chests[0].transform.position = new Vector3(chests[0].transform.position.x, 254f);
                                ChestSpriteRenderer.sprite = ChestPutLady;
                                chestGet = true;
                                break;
                            case "toolbox":
                                ToolboxSpriteRenderer.sprite = ToolboxPutLady;
                                toolboxGet = true;
                                break;
                            case "door":
                                doors[0].transform.position = new Vector3(446f, doors[0].transform.position.y);
                                DoorSpriteRenderer.sprite = DoorPutLady;
                                break;
                        }
                        tagName = null;
                    }

                }
            }
            else
            {
                collisionFlug = false;
            }
        }
        else
        {
            Vector2 position = new Vector2(0, 0);
            position = collision.contacts[0].point;
            //Debug.Log(position);
            collisionFlug = true;
            position = new Vector2(position.x, position.y);
            MoveAngle(position);
            iTween.MoveTo(this.gameObject, iTween.Hash(
                "position", position,
                "time", 0.01f,
                "oncomplete", "OnCompleteCallback",
                "oncompletetarget", this.gameObject,
                "easeType", "linear"));
        }
    }


    //衝突、動くのが終わった後に呼ばれる関数
    void OnCollision()
    {
        //デバック用
        //Debug.Log("animatingStop");
        //移動フラグをfalse
        right = false;
        left = false;
        up = false;
        down = false;
    }

    //移動の際の方向取得
    void MoveAngle(Vector3 direction)
    {
        float angle = Mathf.Atan2(-direction.y, -direction.x);
        angle *= Mathf.Rad2Deg;
        angle = (angle + 360.0f) % 360.0f;
        Debug.Log(angle);
        if ((angle > 315.0f) || (angle < 45.0f))
        {
            Debug.Log("右に動く");
            right = true;
        }
        else
        {
            if ((angle >= 45.0f) && (angle <= 135.0f))
            {
                Debug.Log("上に動く");
                up = true;
            }
            else
            {
                if ((angle > 135.0f) && (angle < 225.0f))
                {
                    Debug.Log("左に動く");
                    left = true;
                }
                else
                {
                    Debug.Log("下に動く");
                    down = true;
                }
            }
        }

            //歩くアニメーション(rightフラグがtrueなら動く、falseなら動かない)
            anim.SetBool("run 0", right);
            anim.SetBool("run 1", left);
            anim.SetBool("run 2", up);
            anim.SetBool("run 3", down);

    }


    // 左クリックしたオブジェクトを取得する関数(2D)
    private GameObject getrightObject()
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


    void SpriteInitialize()
    {
        //------------------------------------------------------------------------//
        // 取得した際の画像変更のために、それぞれのオブジェクトを取得しておく     //
        //------------------------------------------------------------------------//

        floors = GameObject.FindGameObjectsWithTag("floor");
        beds = GameObject.FindGameObjectsWithTag("Bed");
        sidetables = GameObject.FindGameObjectsWithTag("sidetable");
        desks = GameObject.FindGameObjectsWithTag("desk");
        chests = GameObject.FindGameObjectsWithTag("chest");
        toolboxs = GameObject.FindGameObjectsWithTag("toolbox");
        doors = GameObject.FindGameObjectsWithTag("door");

        // このobjectのSpriteRendererを取得
        FloorSpriteRenderer = floors[0].GetComponent<SpriteRenderer>();
        BedSpriteRenderer = beds[0].GetComponent<SpriteRenderer>();
        SidetableSpriteRenderer = sidetables[0].GetComponent<SpriteRenderer>();
        DeskSpriteRenderer = desks[0].GetComponent<SpriteRenderer>();
        ChestSpriteRenderer = chests[0].GetComponent<SpriteRenderer>();
        ToolboxSpriteRenderer = toolboxs[0].GetComponent<SpriteRenderer>();
        DoorSpriteRenderer = doors[0].GetComponent<SpriteRenderer>();

    }


    //  性別によって画像を変える
    void SpriteGender()
    {
        switch (gender)
        {
            //Boy
            case 0:
                FloorSpriteRenderer.sprite = FloorBoy;
                BedSpriteRenderer.sprite = BedBoy;
                SidetableSpriteRenderer.sprite = SidetableBoy;
                DeskSpriteRenderer.sprite = DeskBoy;
                ChestSpriteRenderer.sprite = ChestBoy;
                ToolboxSpriteRenderer.sprite = ToolboxBoy;
                DoorSpriteRenderer.sprite = DoorBoy;
                break;

            //Girl
            case 1:
                FloorSpriteRenderer.sprite = FloorLady;
                BedSpriteRenderer.sprite = BedLady;
                SidetableSpriteRenderer.sprite = SidetableLady;
                DeskSpriteRenderer.sprite = DeskLady;
                ChestSpriteRenderer.sprite = ChestLady;
                ToolboxSpriteRenderer.sprite = ToolboxLady;
                DoorSpriteRenderer.sprite = DoorLady;

                break;
        }


    }

    //箪笥のアニメーション
    void ChestChangeSprite()
    {
        chestTime++;
        if (chestTime >= 30)
        {
            if (gender == 0)
            {
                ChestSpriteRenderer.sprite = Chest2Put;
            }
            else if (gender == 1)
            {
                ChestSpriteRenderer.sprite = Chest2PutLady;
            }

        }
    }

    //ツールボックスのアニメーション
    void ToolboxChangeSprite()
    {
        toolboxTime++;
        if (toolboxTime >= 30)
        {
            if (gender == 0)
            {
                ToolboxSpriteRenderer.sprite = Toolbox2Put;
            }
            else if (gender == 1)
            {
                ToolboxSpriteRenderer.sprite = Toolbox2PutLady;
            }

        }
    }
}
