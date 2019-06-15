/*
    ============================================================================

    Namespace:			LineBreakFixupsDemo

    Class Name:			JSONDeserializationUseCase

	File Name:			JSONDeserializationUseCase.cs

    Synopsis:			This class models the data for a series of JSON object
                        deserialization unit tests.

    Remarks:            The properties are defined alphabetically by name, and
                        their names correspond to the undecorated names of the
                        PerformJSONTransofmration parameters, except the first
                        parameter, which is managed by a local scalar integer.

                        Since the collection is maintained in an array, which is
                        never sorted, the only unusual features of this class
                        are its ToString method override and its single public
                        constructor, coupled with a private constructor, which
                        guarantees that all instances are initialized.

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
	2019/06/14 1.0     DAG Class created from Visual Studio template and
                           populated with working routines copied from other
                           projects
    ============================================================================
*/


namespace LineBreakFixupsDemo
{
    internal class JSONDeserializationUseCase
    {
        private JSONDeserializationUseCase ( )
        {
        }   // private JSONDeserializationUseCase constructor (1 of 2)

        internal JSONDeserializationUseCase (
            string pstrTestReportLabel ,
            string pstrRESTResponseFileName ,
            bool pfConvertLineEndings ,
            string pstrIntermediateFileName ,
            string pstrFinalOutputFileName ,
            string pstrResponseObjectFileName )
        {
            TestReportLabel = pstrTestReportLabel;
            RESTResponseFileName = pstrRESTResponseFileName;
            ConvertLineEndings = pfConvertLineEndings;
            IntermediateFileName = pstrIntermediateFileName;
            FinalOutputFileName = pstrFinalOutputFileName;
            ResponseObjectFileName = pstrResponseObjectFileName;
        }   // internal JSONDeserializationUseCase (2 of 2)


        internal bool ConvertLineEndings
        {
            get; private set;
        }   // internal bool ConvertLineEndings


        internal string FinalOutputFileName
        {
            get; private set;
        }   // internal string FinalOutputFileName read-only property


        internal string IntermediateFileName
        {
            get; private set;
        }   // internal string IntermediateFileName read-only property


        internal string ResponseObjectFileName
        {
            get; private set;
        }   // internal string ResponseObjectFileName read-only property


        internal string RESTResponseFileName
        {
            get; private set;
        }   // internal string RESTResponseFileName read-only property


        internal string TestReportLabel
        {
            get; private set;
        }   // internal string TestReportLabel


        public override string ToString ( )
        {
            return string.Format (
                Properties.Resources.JSON_DESERIALIZER_TOSTRING_TEMPLATE ,
                new object [ ]
                {
                    RESTResponseFileName ,                  // Format Item 0: RESTResponseFileName = {0},
                    ConvertLineEndings ,                    // Format Item 1: ConvertLineEndings = {1},
                    IntermediateFileName ,                  // Format Item 2: IntermediateFileName = {2},
                    FinalOutputFileName ,                   // Format Item 3: FinalOutputFileName = {3},
                    ResponseObjectFileName                  // Format Item 4: ResponseObjectFileName = {4}
                } );
        }   // public override string ToString method
    }   // internal class JSONDeserializationUseCase
}   // partial namespace LineBreakFixupsDemo