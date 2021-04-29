using System;
using System.Collections.Generic;

namespace ChessButCool
{
    public class Triple<X, Y, Z>
    {
        private X value1;
        private Y value2;
        private Z value3;

        private bool noVal3;

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

        public bool NoVal3
        {
            get { return this.noVal3; }
            set { noVal3 = value; }
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
