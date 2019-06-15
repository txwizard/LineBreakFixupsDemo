/*
    ============================================================================

    Namespace:			LineBreakFixupsDemo

    Class Name:			SortableSettingsProperty

	File Name:			SortableSettingsProperty.cs

    Synopsis:			By inheriting SettingsProperty, explicitly implementing
                        the IComparable interface, and overriding the three
                        virtual methods of its ultimate base class, Object, this
                        class provides a mechanism for sorting SettingsProperty
                        objects by name.

    Author:				David A. Gray

	License:            Copyright (C) 2019, David A. Gray.
						All rights reserved.

                        Redistribution and use in source and binary forms, with
                        or without modification, are permitted provided that the
                        following conditions are met:

                        *   Redistributions of source code must retain the above
                            copyright notice, this list of conditions and the
                            following disclaimer.

                        *   Redistributions in binary form must reproduce the
                            above copyright notice, this list of conditions and
                            the following disclaimer in the documentation and/or
                            other materials provided with the distribution.

                        *   Neither the name of David A. Gray, nor the names of
                            his contributors may be used to endorse or promote
                            products derived from this software without specific
                            prior written permission.

                        THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
                        CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
                        WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
                        WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
                        PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
                        David A. Gray BE LIABLE FOR ANY DIRECT, INDIRECT,
                        INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
                        (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
                        SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
                        PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
                        ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
                        LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
                        ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
                        IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version By  Synopsis
    ---------- ------- --- -----------------------------------------------------
	2019/06/06 1.0     DAG Class created
    ============================================================================
*/

using System;

using System.Configuration;


namespace LineBreakFixupsDemo
{
    public class SortableSettingsProperty : SettingsProperty, IComparable<SortableSettingsProperty>
    {
        /// <summary>
        /// Since this constructor is never called, it is safe to have it call a
        /// base constructor that essentially initializes an empty object.
        /// </summary>
        private SortableSettingsProperty ( )
            : base (
                  string.Empty ,                            // string                       name
                  typeof ( string ) ,                       // Type                         propertType
                  null ,                                    // SettingsProvider             provider
                  false ,                                   // bool                         isReadOnly
                  string.Empty ,                            // object                       defaultValue
                  SettingsSerializeAs.String ,              // SettingsSerializeAs          serializeAs
                  null ,                                    // SettingsAttributeDictionary  attributes
                  true ,                                    // bool                         throwOnErrorDeserializing
                  true )                                    // bool                         throwOnErrorSerializing
        {
        }   // private SortableSettingsProperty (1 of 3)


        /// <summary>
        /// Construct an instance from the name of an existing property. I am
        /// not certain how this might be used, unless, perhaps, the method that
        /// copies a collection into an array uses it.
        /// </summary>
        /// <param name="name">
        /// Specifies the name of an existing SettingsProperty object
        /// </param>
        public SortableSettingsProperty (
            string name
            ) : base ( name )
        {
        }   // public SortableSettingsProperty (2 of 3)


        /// <summary>
        /// Construct an instance in which every property is fully specified.
        /// </summary>
        /// <param name="name">
        /// The name of the SettingsProperty object
        /// </param>
        /// <param name="propertyType">
        /// the type of SettingsProperty object
        /// </param>
        /// <param name="provider">
        /// A SettingsProvider object to use for persistence
        /// </param>
        /// <param name="isReadOnly">
        /// A Boolean value specifying whether the SettingsProperty object is
        /// read-only
        /// </param>
        /// <param name="defaultValue">
        /// The default value of the SettingsProperty object
        /// </param>
        /// <param name="serializeAs">
        /// A SettingsSerializeAs object
        /// 
        /// This object is an enumeration used to set the serialization scheme
        /// for storing application settings.
        /// </param>
        /// <param name="attributes">
        /// A SettingsAttributeDictionary object
        /// </param>
        /// <param name="throwOnErrorDeserializing">
        /// A Boolean value specifying whether an error will be thrown when the
        /// property is unsuccessfully deserialized
        /// </param>
        /// <param name="throwOnErrorSerializing">
        /// A Boolean value specifying whether an error will be thrown when the
        /// property is unsuccessfully serialized
        /// </param>
        public SortableSettingsProperty (
                string name ,
                Type propertyType ,
                SettingsProvider provider ,
                bool isReadOnly ,
                object defaultValue ,
                SettingsSerializeAs serializeAs ,
                SettingsAttributeDictionary attributes ,
                bool throwOnErrorDeserializing ,
                bool throwOnErrorSerializing )
            : base (
                  name ,                                    // string                       name
                  propertyType ,                            // Type                         propertType,
                  provider ,                                // SettingsProvider             provider
                  isReadOnly ,                              // bool                         isReadOnly
                  defaultValue ,                            // object                       defaultValue
                  serializeAs ,                             // SettingsSerializeAs          serializeAs
                  attributes ,                              // SettingsAttributeDictionary  attributes
                  throwOnErrorDeserializing ,               // bool                         throwOnErrorDeserializing
                  throwOnErrorSerializing )                 // bool                         throwOnErrorSerializing
        {
        }   // public SortableSettingsProperty (3 of 3)


        /// <summary>
        /// Make instances sortable by their respective Name properties.
        /// </summary>
        /// <param name="other">
        /// The SortableSettingsProperty against which to compare the current
        /// instance
        /// </param>
        /// <returns>
        /// The return value is as specified in the IComparable interface
        /// specification.
        /// </returns>
        int IComparable<SortableSettingsProperty>.CompareTo ( SortableSettingsProperty other )
        {
            return this.Name.CompareTo ( other.Name );
        }   // int IComparable<SortableSettingsProperty>.CompareTo


        /// <summary>
        /// Test the equality of two instances by way of the values of their
        /// respective Name properties.
        /// </summary>
        /// <param name="obj">
        /// This object must be another SortableSettingsProperty instance.
        /// </param>
        /// <returns>
        /// TRUE if the Name properties of the two objects are equal, else FALSE
        /// </returns>
        public override bool Equals ( object obj )
        {
            if ( obj is SortableSettingsProperty )
            {
                SortableSettingsProperty other = obj as SortableSettingsProperty;
                return this.Name.Equals ( other.Name );
            }
            else
            {
                return false;
            }
        }   // public override bool Equals


        /// <summary>
        /// Return the hash code of the Name property as a proxy for the hash
        /// code of the instance.
        /// </summary>
        /// <returns>
        /// The hash code of the Name property
        /// </returns>
        public override int GetHashCode ( )
        {
            return this.Name.GetHashCode ( );
        }   // public override int GetHashCode


        /// <summary>
        /// Return a string representation that incorporates the three most used
        /// properties of a setting, its Name, Type, and Default Value.
        /// </summary>
        /// <returns>
        /// Return a formatted representation of the Name, Type and DefaultValue
        /// properties of the instance.
        /// </returns>
        public override string ToString ( )
        {   // For now, keep the format string with the class.
            return string.Format (
                @"Name = {0}, Type = {1}, Default Value = {2}" ,
                Name ,
                PropertyType ,
                DefaultValue );
        }   // public override string ToString
    }   // public class SortableSettingsProperty
}   // namespace LineBreakFixupsDemo