// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Windows.Documents;

namespace System.Windows.Xps.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    internal class ReachDocumentReferenceCollectionSerializer :
                   ReachSerializer
    {
        /// <summary>
        /// Creates new serializer for a DocumentReferenceCollection
        /// </summary>
        /// <param name="manager">serialization manager for this seriaizer</param>
        public
        ReachDocumentReferenceCollectionSerializer(
            PackageSerializationManager manager
            ):
        base(manager)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        internal
        override
        void
        PersistObjectData(
            SerializableObjectContext   serializableObjectContext
            )
        {
            ArgumentNullException.ThrowIfNull(serializableObjectContext);

            // get DocumentReferenceCollection
            System.Collections.Generic.IEnumerable<DocumentReference> enumerableObject = serializableObjectContext.TargetObject as System.Collections.Generic.IEnumerable<DocumentReference>;

            if (enumerableObject == null)
            {
                throw new XpsSerializationException(SR.Format(SR.MustBeOfType, "serializableObjectContext.TargetObject", typeof(System.Collections.Generic.IEnumerable<DocumentReference>)));
            }

            SerializeDocumentReferences(serializableObjectContext);
        }

        /// <summary>
        /// This is being called to serialize the DocumentReference items
        /// contained within the colleciton
        /// </summary>
        private
        void
        SerializeDocumentReferences(
            SerializableObjectContext   serializableObjectContext
            )
        {
            //
            // Serialize each DocumentReference in DocumentReferenceColleciton
            //
            foreach (object documentReference in (System.Collections.Generic.IEnumerable<DocumentReference>)serializableObjectContext.TargetObject)
            {
                if (documentReference != null)
                {
                    // Serialize the current item
                    SerializeDocumentReference(documentReference);
                }
            }
        }


        /// <summary>
        ///     Called to serialize a single DocumentReference
        /// </summary>
        private 
        void
        SerializeDocumentReference(
            object documentReference
            )
        {
            ReachSerializer serializer = SerializationManager.GetSerializer(documentReference);

            if(serializer!=null)
            {
                serializer.SerializeObject(documentReference);
            }
            else
            {
                // should we throw if this is not a DocumentReference or just not do anything?
                throw new XpsSerializationException(SR.ReachSerialization_NoSerializer);
            }
        }
    };
}
