using System;
using System.Collections.Generic;

namespace ChessButCool
{
    public class Triple<X, Y, Z>
    {
        X value1;
        Y value2;
        Z value3;

        bool noVal3;

        public void SetValue(X value1, Y value2, Z value3)
        {
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
            noVal3 = false;
        }
        public void SetValue(X value1, Y value2)
        {
            this.value1 = value1;
            this.value2 = value2;
            noVal3 = true;
        }

        public void SetValue(X value1)
        {
            this.value1 = value1;
            noVal3 = true;
        }

        public bool GetnoVal3()
        {
            return noVal3;
        }

        public X Value1 
        {
            get { return this.value1; }
            set { value1 = value; }
        }

        public Y Value2
        {
            get { return this.value2; }
            set { value2 = value; }
        }

        public Z Value3
        {
            get { return this.value3; }
            set 
            {
                noVal3 = false; 
                value3 = value; 
            }
        }
    }
}