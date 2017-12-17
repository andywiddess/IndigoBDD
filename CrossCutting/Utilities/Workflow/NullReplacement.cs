using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Workflow
{
    public static class NullReplacement
    {


        private static NullClass nullclass = new NullClass();

        public static NullClass Null
        {
            get
            { return nullclass; }
        }


        public class NullClass
        {

        }

    }



}
