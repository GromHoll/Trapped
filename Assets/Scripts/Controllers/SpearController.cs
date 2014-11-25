using UnityEngine;
using System.Collections;

public class SpearController : MonoBehaviour {

    public static readonly string STATUS_KEY = "Status";

    private Animator aminator;
    private SpearCell cell;

    public void Start() {
        aminator = GetComponent<Animator>();
    }

    public void Update() {
        aminator.SetBool(STATUS_KEY, getStatus());
    }

    public void SetCell(SpearCell cell) {
        this.cell = cell;
    }

    private bool getStatus() {
        return cell != null ? cell.IsDeadly() : false;
    }
}
