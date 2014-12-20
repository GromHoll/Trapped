﻿using System;

namespace TrappedGame.Utils {
    class Validate {

        public static void CheckArgument(bool condition, string message = "Argument is not valid"){
            if (!condition) throw new ArgumentException(message);
        }

        // TODO change to NPE exceptions
        public static void NotNull(Object obj, string message = "Argument should be not null") {
            if (obj == null) throw new ArgumentException(message);
        }

    }
}
