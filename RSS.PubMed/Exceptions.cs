using System;
using System.Runtime.Serialization;

namespace RSS.PubMed
{
    public class Exceptions
    {
        #region PubMedElementNotFoundException
        public class PubMedElementNotFoundException : Exception
        {
            public PubMedElementNotFoundException()
            {

            }

            public PubMedElementNotFoundException(string message) : base(message)
            {

            }

            public PubMedElementNotFoundException(string message, Exception inner) : base(message, inner)
            {

            }

            protected PubMedElementNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
            {

            }
        }
        #endregion

        #region PubMedAttributeNotFoundException
        public class PubMedAttributeNotFoundException : Exception
        {
            public PubMedAttributeNotFoundException()
            {

            }

            public PubMedAttributeNotFoundException(string message) : base(message)
            {

            }

            public PubMedAttributeNotFoundException(string message, Exception inner) : base(message, inner)
            {

            }

            protected PubMedAttributeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
            {

            }
        }
        #endregion 
    }
}