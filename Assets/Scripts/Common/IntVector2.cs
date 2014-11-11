using System;

public struct IntVector2 {
    public int y;
    public int x;

    public IntVector2(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public void Set(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static bool operator == (IntVector2 left, IntVector2 right) {
        return left.x == right.x && left.y == right.y;
    }

    public static bool operator != (IntVector2 left, IntVector2 right) {
        return left.x != right.x || left.y != right.y;
    }

    public override bool Equals(object other) {
        if (!(other is IntVector2)) {
            return false;
        }
        IntVector2 vector = (IntVector2) other;
        return this.x.Equals(vector.x) && this.y.Equals(vector.y);
    }
    
    public override int GetHashCode() {
        return this.x.GetHashCode () ^ this.y.GetHashCode () << 2;
    }

}