/*
    ============================================================================

    Module Name:        JSONFixupEngine.cs

    Namespace Name:     LineBreakFixupsDemo

    Class Name:         JSONFixupEngine

    Synopsis:           This class implements a data-driven engine to correct
                        key names in JSON strings that are invalid C# variable
                        names, and converts named keys into array elements that
                        use the value that was intended to be its key as the
                        value of the first field in each array element, each of
                        which corresponds to a property of the class into which
                        it is transformed by the Newtonsoft JSON deserializer.

    Remarks:            This class relies on a list of string pairs that are
                        kept in a TAB delimited text file that is embedded into
                        the defining assembly. This array could be externalized
                        by deriving a class from this one, and overriding its
                        public constructor.

    Author:             David A. Gray

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

    Date       Version Author Synopsis
    ---------- ------- ------ -------------------------------------------------
    2019/05/13 1.0     DAG    This is the first version.

    2019/05/22 1.1     DAG    Add the flower box and a three-clause BSD license.

    2019/05/28 1.2     DAG    Clean up the flower box.
                              The code is otherwase unchanged.
    ============================================================================
*/

using System;
using System.Text;

using WizardWrx;
using WizardWrx.AnyCSV;
using WizardWrx.Core;
using WizardWrx.EmbeddedTextFile;


namespace LineBreakFixupsDemo
{
    class JSONFixupEngine
    {
        /// <summary>
        /// Marking the default constructor as private ensures that all object
        /// instances are fully initialized when their constructor returns.
        /// </summary>
        private JSONFixupEngine ( )
        {
        }   // private JSONFixupEngine prevents construction of uninitialized instances


        /// <summary>
        /// String argument <paramref name="pstrFixupsPairsResourceFileName"/>
        /// guarantees that the constructed instance is initialized.
        /// </summary>
        /// <param name="pstrFixupsPairsResourceFileName">
        /// String containing the unqualified name of the embedded resource file
        /// from which the private StringFixups.StringFixup array is initialized
        /// </param>
        public JSONFixupEngine ( string pstrFixupsPairsResourceFileName )
        {
            _fIsFirstPass = true;
            StringFixups.StringFixup [ ] stringFixups = LoadStringFixups ( pstrFixupsPairsResourceFileName );
            _responseStringFixups = new StringFixups ( stringFixups );
        }   // public JSONFixupEngine constructor returns an initialized instance


        /// <summary>
        /// The first of two fixup passes applies the string pairs that comprise
        /// the private StringFixups.StringFixup array to the raw JSON string,
        /// which yields a string in which all key names are valid C# variable
        /// names.
        /// </summary>
        /// <param name="pstrRawResponse">
        /// String containing raw response returned by the REST API that pushed
        /// the StringFixups class to its limits, and proved its worth.
        /// </param>
        /// <returns>
        /// The returned string contains key names that are valid C# variable
        /// names.
        /// </returns>
        public string ApplyFixups_Pass_1 ( string pstrRawResponse )
        {
            return _responseStringFixups.ApplyFixups ( pstrRawResponse );
        }   // public string ApplyFixups_Pass_1


        /// <summary>
        /// A second pass is required to transform the TimeSeriesDaily array
        /// into a true array.
        /// </summary>
        /// <param name="pstrFixedUp_Pass_1">
        /// As its name implies, the input to this routine is the output from
        /// ApplyFixups_Pass_1.
        /// </param>
        /// <returns>
        /// The returned JSON string is formatted in such a way that the
        /// TimeSeriesDaily node can be treated as a first class C# array.
        /// </returns>
        public string ApplyFixups_Pass_2 ( string pstrFixedUp_Pass_1 )
        {   // This method references and updates instance member __fIsFirstPass.
            const string TSD_LABEL_ANTE = "\"TimeSeriesDaily\": {";             // Ante: "TimeSeriesDaily": {
            const string TSD_LABEL_POST = "\"Time_Series_Daily\" : [";          // Post: "Time_Series_Daily": [

            const string END_BLOCK_ANTE = "}\n    }\n}";
            const string END_BLOCK_POST = "}\n    ]\n}";

            const int DOBULE_COUNTING_ADJUSTMENT = MagicNumbers.PLUS_ONE;       // Deduct one from the length to account for the first character occupying the position where copying begins.

            _fIsFirstPass = true;                                              // Re-initialize the First Pass flag.

            StringBuilder builder1 = new StringBuilder ( pstrFixedUp_Pass_1.Length * MagicNumbers.PLUS_TWO );

            builder1.Append (
                pstrFixedUp_Pass_1.Replace (
                    TSD_LABEL_ANTE ,
                    TSD_LABEL_POST ) );
            int intLastMatch = builder1.ToString ( ).IndexOf ( TSD_LABEL_POST )
                               + TSD_LABEL_POST.Length
                               - DOBULE_COUNTING_ADJUSTMENT;

            while ( intLastMatch > ListInfo.INDEXOF_NOT_FOUND )
            {
                intLastMatch = FixNextItem (
                    builder1 ,
                    intLastMatch );
            }   // while ( intLastMatch > ListInfo.INDEXOF_NOT_FOUND )

            //  ----------------------------------------------------------------
            //  Close the array by replacing the last French brace with a square
            //  bracket.
            //  ----------------------------------------------------------------

            builder1.Replace (
                END_BLOCK_ANTE ,
                END_BLOCK_POST );

            return builder1.ToString ( );
        }   // public string ApplyFixups_Pass_2


        /// <summary>
        /// This routine changes the next node into a well-formed array element.
        /// </summary>
        /// <param name="pbuilder">
        /// StringBuilder containing string to evaluate and transform
        /// </param>
        /// <param name="pintLastMatch">
        /// Position of last match, from which to resume scanning for string
        /// FIRST_ITEM_BREAK_ANTE.
        /// </param>
        /// <returns>
        /// The return value is the position where the next occurrence of
        /// string FIRST_ITEM_BREAK_ANTE was found, which is eventually
        /// ListInfo.INDEXOF_NOT_FOUND. Since the StringBuilder is a reference,
        /// when string FIRST_ITEM_BREAK_ANTE is found, it is replaced with
        /// string SUBSEQUENT_ITEM_BREAK_ANTE.
        /// </returns>
        private int FixNextItem (
            StringBuilder pbuilder ,
            int pintLastMatch )
        {   // This method references private instance member _fIsFirstPass several times.
            const string FIRST_ITEM_BREAK_ANTE = "[\n        \"";               // Ante: },\n        "
            const string SUBSEQUENT_ITEM_BREAK_ANTE = "},\n        \"";         // Ante: },\n        "

            string strInput = pbuilder.ToString ( );
            int intMatchPosition = strInput.IndexOf (
                _fIsFirstPass
                    ? FIRST_ITEM_BREAK_ANTE
                    : SUBSEQUENT_ITEM_BREAK_ANTE ,
                pintLastMatch );

            if ( intMatchPosition > ListInfo.INDEXOF_NOT_FOUND )
            {
                return FixThisItem (
                    strInput ,
                    intMatchPosition ,
                    _fIsFirstPass
                        ? FIRST_ITEM_BREAK_ANTE.Length
                        : SUBSEQUENT_ITEM_BREAK_ANTE.Length ,
                    pbuilder );
            }   // TRUE (At least one match remains.) block, if ( intMatchPosition > ListInfo.INDEXOF_NOT_FOUND )
            else
            {
                return ListInfo.INDEXOF_NOT_FOUND;
            }   // FALSE (All matches have been found.) block, if ( intMatchPosition > ListInfo.INDEXOF_NOT_FOUND )
        }   // private int FixNextItem


        /// <summary>
        /// After it finds and replaces its string, FixNextItem calls upon this
        /// routine to look just past it for string FIRST_ITEM_BREAK_POST, and
        /// replace it with SUBSEQUENT_ITEM_BREAK_POST.
        /// </summary>
        /// <param name="pstrInput">
        /// This routine receives the string copy made by FixNextItem, so that
        /// it can reinitialize the StringBuilder before it starts appending
        /// text from it.
        /// </param>
        /// <param name="pintMatchPosition">
        /// The character position where FixNextItem found its match becomes the
        /// starting point for the search performed by this routine.
        /// </param>
        /// <param name="pintMatchLength">
        /// The length of the string matched by FixNextItem is fed into this
        /// routine, which adds it to <paramref name="pintMatchPosition"/> to
        /// determine where to begin its own search.
        /// </param>
        /// <param name="psbOut">
        /// Although it is initialized and repopulated, passing around a
        /// StringBuilder is simpler than using an out parameter.
        /// </param>
        /// <returns></returns>
        private int FixThisItem (
            string pstrInput ,
            int pintMatchPosition ,
            int pintMatchLength ,
            StringBuilder psbOut )
        {
            const string FIRST_ITEM_BREAK_POST = "\n        {\n            \"Activity_Date\": \"";        // Post: },\n        {\n        {\n        "Activity_Date": "
            const string SUBSEQUENT_ITEM_BREAK_POST = ",\n        {\n            \"Activity_Date\": \"";    // Post: },\n        {\n        {\n        "Activity_Date": "

            const int DATE_TOKEN_LENGTH = 11;
            const int DATE_TOKEN_SKIP_CHARS = DATE_TOKEN_LENGTH + 3;

            int intSkipOverMatchedCharacters = pintMatchPosition + pintMatchLength;

            psbOut.Clear ( );

            psbOut.Append ( pstrInput.Substring (
                ListInfo.SUBSTR_BEGINNING ,
                ArrayInfo.OrdinalFromIndex ( pintMatchPosition ) ) );
            psbOut.Append ( _fIsFirstPass
                ? FIRST_ITEM_BREAK_POST
                : SUBSEQUENT_ITEM_BREAK_POST );
            psbOut.Append ( pstrInput.Substring (
                intSkipOverMatchedCharacters ,
                DATE_TOKEN_LENGTH ) );
            psbOut.Append ( SpecialCharacters.COMMA );
            psbOut.Append ( pstrInput.Substring ( intSkipOverMatchedCharacters + DATE_TOKEN_SKIP_CHARS ) );

            int rintSearchResumePosition = pintMatchPosition
                                           + ( _fIsFirstPass
                                                    ? FIRST_ITEM_BREAK_POST.Length
                                                    : SUBSEQUENT_ITEM_BREAK_POST.Length );
            _fIsFirstPass = false;     // Putting this here allows execution to be unconditional.

            return ArrayInfo.OrdinalFromIndex ( rintSearchResumePosition );
        }   // private int FixThisItem


        /// <summary>
        /// This private static method encapsulates the work of loading an
        /// embedded text file resource that consists of tab delimited strings,
        /// and parsing them into pairs of strrings.
        /// </summary>
        /// <param name="pstrEmbeddedResourceName">
        /// This string is expected to contain the unqualified name of the
        /// embedded text file resource, which is easy to grab from the Solution
        /// Explorer. This routine appends the extension, which must be .txt.
        /// </param>
        /// <returns>
        /// The return value is an array of StringFixups.StringFixup objects,
        /// each consisting of a pair of strings, one being a string for which
        /// to search, while its companion is the replacement string.
        /// </returns>
        private static StringFixups.StringFixup [ ] LoadStringFixups ( string pstrEmbeddedResourceName )
        {
            const string LABEL_ROW = @"JSON	VS";
            const string TSV_EXTENSION = @".txt";

            const int STRING_PER_RESPONSE = ArrayInfo.ARRAY_FIRST_ELEMENT;
            const int STRING_FOR_JSONCONVERTER = STRING_PER_RESPONSE + ArrayInfo.NEXT_INDEX;
            const int EXPECTED_FIELD_COUNT = STRING_FOR_JSONCONVERTER + ArrayInfo.NEXT_INDEX;

            string strEmbeddResourceFileName = string.Concat (
                pstrEmbeddedResourceName ,
                TSV_EXTENSION );
            string [ ] astrAllMapItems = Readers.LoadTextFileFromEntryAssembly ( strEmbeddResourceFileName );
            Parser parser = new Parser (
                CSVParseEngine.DelimiterChar.Tab ,
                CSVParseEngine.GuardChar.DoubleQuote ,
                CSVParseEngine.GuardDisposition.Strip );
            StringFixups.StringFixup [ ] rFunctionMaps = new StringFixups.StringFixup [ ArrayInfo.IndexFromOrdinal ( astrAllMapItems.Length ) ];

            for ( int intI = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                      intI < astrAllMapItems.Length ;
                      intI++ )
            {
                if ( intI == ArrayInfo.ARRAY_FIRST_ELEMENT )
                {
                    if ( astrAllMapItems [ intI ] != LABEL_ROW )
                    {
                        throw new Exception (
                            string.Format (
                                Properties.Resources.ERRMSG_CORRUPTED_EMBBEDDED_RESOURCE_LABEL ,
                                new string [ ]
                                {
                                    strEmbeddResourceFileName ,                 // Format Item 0: internal resource {0}
                                    LABEL_ROW ,                                 // Format Item 1: Expected value = {1}
                                    astrAllMapItems [ intI ] ,                  // Format Item 2: Actual value   = {2}
                                    Environment.NewLine                         // Format Item 3: Platform-specific newline
                                } ) );
                    }   // if ( astrAllMapItems[intI] != LABEL_ROW )
                }   // TRUE (label row sanity check 1 of 2) block, if ( intI == ArrayInfo.ARRAY_FIRST_ELEMENT )
                else
                {
                    string [ ] astrFields = parser.Parse ( astrAllMapItems [ intI ] );

                    if ( astrFields.Length == EXPECTED_FIELD_COUNT )
                    {
                        rFunctionMaps [ ArrayInfo.IndexFromOrdinal ( intI ) ] = new StringFixups.StringFixup (
                            astrFields [ STRING_PER_RESPONSE ] ,
                            astrFields [ STRING_FOR_JSONCONVERTER ] );
                    }   // TRUE (anticipated outcome) block, if ( astrFields.Length == EXPECTED_FIELD_COUNT )
                    else
                    {
                        throw new Exception (
                            string.Format (
                                Properties.Resources.ERRMSG_CORRUPTED_EMBEDDED_RESOURCE_DETAIL ,
                                new object [ ]
                                {
                                    intI ,                                      // Format Item 0: Detail record {0}
                                    strEmbeddResourceFileName ,                 // Format Item 1: internal resource {1}
                                    EXPECTED_FIELD_COUNT ,                      // Format Item 2: Expected field count = {2}
                                    astrFields.Length ,                         // Format Item 3: Actual field count   = {3}
                                    astrAllMapItems [ intI ] ,                  // Format Item 4: Actual record        = {4}
                                    Environment.NewLine                         // Format Item 5: Platform-specific newline
                                } ) );
                    }   // FALSE (unanticipated outcome) block, if ( astrFields.Length == EXPECTED_FIELD_COUNT )
                }   // FALSE (detail row) block, if ( intI == ArrayInfo.ARRAY_FIRST_ELEMENT )
            }   // for ( int intI = ArrayInfo.ARRAY_FIRST_ELEMENT ; intI < astrAllMapItems.Length ; intI++ )

            return rFunctionMaps;
        }   // private static StringFixups.StringFixup [ ] GetSStringFixups


        private bool _fIsFirstPass;
        private StringFixups _responseStringFixups;
    }   // class JSONFixupEngine
}   // partial namespace LineBreakFixupsDemo