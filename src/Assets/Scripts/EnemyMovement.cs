using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public GameObject Player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<NavMeshAgent>().destination = Player.transform.position;
	}
}
