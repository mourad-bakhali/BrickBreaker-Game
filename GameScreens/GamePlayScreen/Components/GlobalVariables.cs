using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickBreaker
{
    public static class GlobalVariables
    {
        public static int level = 1;
        public static int score = 0;
        public static int money = 0;

        public static float damage=0f;
        public static float resistance = 100f;
        public static float releaseObjSpeed = 0.08f;
        public static float speedFactor = 0.10f;
    }
}
