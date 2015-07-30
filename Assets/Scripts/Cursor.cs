using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {
    [SerializeField]
    private Texture2D activeTexture;
    [SerializeField]
    private Texture2D moveTexture;
    [SerializeField]
    private Texture2D attackTexture;
    [SerializeField]
    private Texture2D magicTexture;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void Start () {
        UnityEngine.Cursor.SetCursor( activeTexture, hotSpot, cursorMode );
    }

    public void SetStandardCursor () {
        CancelInvoke();
        UnityEngine.Cursor.SetCursor( activeTexture, hotSpot, cursorMode );
    }

    public void SetMoveCursor () {
        CancelInvoke();
        UnityEngine.Cursor.SetCursor( moveTexture, hotSpot, cursorMode );
        CursorInvoke();
    }

    public void SetAttackCursor () {
        CancelInvoke();
        UnityEngine.Cursor.SetCursor( attackTexture, hotSpot, cursorMode );
        CursorInvoke();
    }

    public void SetMagicCursor () {
        CancelInvoke();
        UnityEngine.Cursor.SetCursor( magicTexture, hotSpot, cursorMode );
    }

    private void CancelCursorInvoke () {
        CancelInvoke( "SetStandardCursor" );
    }

    private void CursorInvoke () {
        Invoke( "SetStandardCursor", 0.4f );
    }

}
