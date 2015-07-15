using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGINTB_Builder.Lib.Exceptions
{
    public class GinTubXmlException : Exception
    {
        #region MEMBER PROPERTIES

        public string Operation { get; private set;}

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public GinTubXmlException(string operation) :
            base()
        {
            Operation = operation;
        }

        public GinTubXmlException(string operation, Exception innerException) :
            base(string.Format("Exception thrown by failed operation.", innerException))
        {
            Operation = operation;
        }

        #endregion

        #endregion

    }
}
