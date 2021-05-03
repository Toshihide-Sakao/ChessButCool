using System;
using System.Collections.Generic;

namespace ChessButCool
{
    public class Pair<X, Y>
    {
        private X value1;
        private Y value2;
        private bool noVal2;

        // Sets value of 1 and 2
        public void SetValue(X value1, Y value2)
        {
            this.value1 = value1;
            this.value2 = value2;
            noVal2 = false;
        }

        // Sets value of 1 only
        public void SetValue(X value1)
        {
            this.value1 = value1;
            noVal2 = true;
        }

        // gets if there is no value2
        public bool GetnoVal2()
        {
            return noVal2;
        }

        // Properties
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
                // sets no vlue2 to false;
                noVal2 = false;
                value2 = value;
            }
        }
    }
}
