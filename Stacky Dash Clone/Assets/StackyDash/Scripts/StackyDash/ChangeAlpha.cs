using UnityEngine;
using UnityEngine.UI;



public static class ChangeAlpha
{
    public static void SetAlpha(Image level_index, float alpha)
    {
        if (level_index != null)
        {
            Color __alpha = level_index.color;
            __alpha.a = alpha;
            level_index.color = __alpha;
        }

    }
}
