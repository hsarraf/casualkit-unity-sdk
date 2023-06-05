using UnityEngine;
using CasualKit.Quick;
using CasualKit.Quick.View;


public class NodeView1 : QuickView
{
    public class Pos : GenericData<Pos>
    {
        public float _x, _y, _z;
        public Vector3 ToVector3() => new Vector3(_x, _y, _z);
    }

    public override void OnRecieved(object data)
    {
        Pos pos = Pos.Load(data);
        transform.position = pos.ToVector3();
        Debug.Log(pos._x + ", " + pos._y + ", " + pos._z);
    }

    private void Update()
    {
        if (Owner) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Pos p = new Pos { _x = Random.Range(-10, 10), _y = Random.Range(-10, 10), _z = Random.Range(-10, 10) };
                Broadcast(p);
                transform.position = p.ToVector3();
            }
        }
    }

}
