using System;
using System.Collections.Generic;

namespace ChessButCool
{
    public class Pair<X, Y>
    {
        X value1;
        Y value2;
        bool noVal2;

        public void SetValue(X value1, Y value2)
        {
            this.value1 = value1;
            this.value2 = value2;
            noVal2 = false;
        }

        public void SetValue(X value1)
        {
            this.value1 = value1;
            noVal2 = true;
        }

        public bool GetnoVal2()
        {
            return noVal2;
        }

        public X Value1 
        {
            get { return this.value1; }
            set { value1 = value; }
        }

        public Y Value2
        {
            get { return this.value2; }
            set 
            {
                noVal2 = false; 
                value2 = value; 
            }
        }
        
    }
}
