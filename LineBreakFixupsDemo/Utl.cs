/*
    ============================================================================

    Namespace:			LineBreakFixupsDemo

    Class Name:			Utl

	File Name:			Utl.cs

    Synopsis:			This static class exposes helper methods used by unit
                        test class LineEndingFixupTests.

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
	2019/06/15 1.0     DAG Class created from Visual Studio template and
                           populated with working routines copied from other
                           projects
    ============================================================================
*/

using System;

using System.IO;
using WizardWrx;


namespace LineBreakFixupsDemo
{
    class Utl
    {
        /// <summary>
        /// Assemble an absolute file name from the location of the executing
        /// assembly, a directory path that is specified relative to it, and the
        /// unqualified name to assign to it, specified by <paramref name="pstrFileNamePerAppSettings"/>.
        /// </summary>
        /// <param name="pstrFileNamePerAppSettings">
        /// String containing the unqualified name to assign to a file
        /// </param>
        /// <returns>
        /// The return value is the absolute name of a file that is assembled by
        /// combining the names of the directory from which the executing
        /// assembly was loaded and another directory name specified relative to
        /// it with the unqualified name specified in <paramref name="pstrFileNamePerAppSettings"/>.
        /// </returns>
        public static string AssembleAbsoluteFileName ( string pstrFileNamePerAppSettings )
        {
            string strAssemblyDirectoryName = Path.GetDirectoryName ( System.Reflection.Assembly.GetExecutingAssembly ( ).Location );
            string strTestDataDirectoryName = Properties.Settings.Default.DATA_DIRECTORY_NAME;
            string strAbsoluteInputFileName = Path.Combine (
                new string [ ]
                {
                    strAssemblyDirectoryName,
                    strTestDataDirectoryName,
                    pstrFileNamePerAppSettings
                } );
            return strAbsoluteInputFileName;
        }   // public static string AssembleAbsoluteFileName


        /// <summary>
        /// Format date/time and integral types for printing, passing all other
        /// types through as is, by way of their ToString methods.
        /// </summary>
        /// <param name="pstrStringFromJSON">
        /// The string from the deserialized JSON object is converted to a safer
        /// type, then fed through its ToString method.
        /// </param>
        /// <returns>
        /// The return value is a formatted string, suitable for display on a
        /// report.
        /// </returns>
        internal static string Beautify ( string pstrStringFromJSON )
        {
            object objOfType = ConvertToAppropriateType ( pstrStringFromJSON );

            if ( objOfType is DateTime )
            {
                DateTime dtmObjAsDate = ( DateTime ) objOfType;
                return SysDateFormatters.ReformatSysDate ( dtmObjAsDate , SysDateFormatters.RFD_YYYY_MM_DD );
            }   // TRUE (The input is a DateTime.) block, if ( objOfType is DateTime )
            else if ( objOfType is long )
            {
                long lngObjAsLongInteger = ( long ) objOfType;
                return lngObjAsLongInteger.ToString ( NumericFormats.NUMBER_PER_REG_SETTINGS_0D );
            }   // TRUE (The input is a Long Integer.) block, else if ( objOfType is long )
            else
            {
                return objOfType.ToString ( );
            }   // FALSE block covering if ( objOfType is DateTime ) AND else if ( objOfType is long )
        }   // internal static string Beautify


        /// <summary>
        /// Increment <paramref name="pintTestNumber"/>, then log a console
        /// message that uses <paramref name="pstrMethodName"/> to identify the
        /// test.
        /// </summary>
        /// <param name="pstrMethodName">
        /// The newer tests use this parameter directly, while the older ones
        /// look it up in a static dictionary that allows it to avoid using
        /// Reflection to get it.
        /// <para>
        /// To invoke the newer method, set <paramref name="pfIgnoreClassTestMap"/>
        /// to TRUE.
        /// </para>
        /// </param>
        /// <param name="pintTestNumber">
        /// The test number is passed in by reference, so that the routine can
        /// be put in charge of incrementing it. Before the first call, this
        /// parameter must be initialized to zero; since this is the default
        /// initial value for an integer, explicit initialization is recommended
        /// but optional.
        /// </param>
        internal static void BeginTest (
            string pstrMethodName ,
            ref int pintTestNumber )
        {
            Console.WriteLine (
                Properties.Resources.MSG_BEGIN ,            // Message Template
                ++pintTestNumber ,                          // Format Item 0 = Test Number - Increment, then print
                pstrMethodName ,                            // Format Item 1 = Method Name per WizardWrx.DiagnosticInfo, which gathers the information at compile time
                Environment.NewLine );                      // Format Item 2 = Newline
        }   // BeginTest method


        /// <summary>
        /// Create a report of the contents of the deserialized response in
        /// <paramref name="timeSeriesDailyResponse"/>.
        /// </summary>
        /// <param name="timeSeriesDailyResponse">
        /// The populated TimeSeriesDailyResponse instance returned by the JSON
        /// deserializer.
        /// </param>
        internal static void ConsumeResponse (
            string pstrReportFileName ,
            TimeSeriesDailyResponse timeSeriesDailyResponse )
        {
            Console.WriteLine (
                Properties.Resources.MSG_RESPONSE_METADATA ,                    // Format control string
                new object [ ]
                {
                    timeSeriesDailyResponse.Meta_Data.Information ,             // Format item 0: Information   = {0}
                    timeSeriesDailyResponse.Meta_Data.Symbol ,                  // Format Item 1: Symbol        = {1}
                    timeSeriesDailyResponse.Meta_Data.LastRefreshed ,           // Format Item 2: LastRefreshed = {2}
                    timeSeriesDailyResponse.Meta_Data.OutputSize ,              // Format Item 3: OutputSize    = {3}
                    timeSeriesDailyResponse.Meta_Data.TimeZone ,                // Format Item 4: TimeZone      = {4}
                    timeSeriesDailyResponse.Time_Series_Daily.Length ,          // Format Item 5: Detail Count  = {5}
                    Environment.NewLine                                         // Format Item 6: Platform-dependent newline
                } );

            string strAbsoluteInputFileName = AssembleAbsoluteFileName ( pstrReportFileName );

            using ( StreamWriter swTimeSeriesDetail = new StreamWriter ( strAbsoluteInputFileName ,
                                                                         FileIOFlags.FILE_OUT_CREATE ,
                                                                         System.Text.Encoding.ASCII ,
                                                                         MagicNumbers.CAPACITY_08KB ) )
            {
                string strLabelRow = Properties.Resources.MSG_RESPONSE_DETAILS_LABELS.ReplaceEscapedTabsInStringFromResX ( );
                swTimeSeriesDetail.WriteLine ( strLabelRow );
                string strDetailRowFormatString = ReportHelpers.DetailTemplateFromLabels ( strLabelRow );

                for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                          intJ < timeSeriesDailyResponse.Time_Series_Daily.Length ;
                          intJ++ )
                {
                    Time_Series_Daily daily = timeSeriesDailyResponse.Time_Series_Daily [ intJ ];
                    swTimeSeriesDetail.WriteLine (
                        strDetailRowFormatString ,
                        new object [ ]
                        {
                            ArrayInfo.OrdinalFromIndex ( intJ ) ,               // Format Item 0: Item
                            Beautify ( daily.Activity_Date) ,                   // Format Item 1: Activity_Date
                            Beautify ( daily.Open ) ,                           // Format Item 2: Open
                            Beautify ( daily.High ) ,                           // Format Item 3: High
                            Beautify ( daily.Low ) ,                            // Format Item 4: Low
                            Beautify ( daily.Close ) ,                          // Format Item 5: Close
                            Beautify ( daily.AdjustedClose ) ,                  // Format Item 6: AdjustedClose
                            Beautify ( daily.Volume ) ,                         // Format Item 7: Volume
                            Beautify ( daily.DividendAmount ) ,                 // Format Item 8: DividendAmount
                            Beautify ( daily.SplitCoefficient )                 // Format Item 9: SplitCoefficient
                        } );
                }   // for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ; intJ < timeSeriesDailyResponse.Time_Series_Daily.Length ; intJ++ )
            }   // using ( StreamWriter swTimeSeriesDetail = new StreamWriter ( strAbsoluteInputFileName , FileIOFlags.FILE_OUT_CREATE , System.Text.Encoding.ASCII , MagicNumbers.CAPACITY_08KB ) )

            Console.WriteLine (
                ShowFileDetails (                                               // Print the returned string.
                    Properties.Resources.FILE_LABEL_CONTENT_REPORT ,            // string pstrLabel
                    strAbsoluteInputFileName ,                                  // string pstrFileName
                    true ,                                                      // bool   pfPrefixWithNewline = false
                    false ) );                                                  // bool   pfSuffixWithNewline = true
        }   // private static void ConsumeResponse


        /// <summary>
        /// Convert a string to the type that most faithfully represents its
        /// contents.
        /// </summary>
        /// <param name="pstrStringFromJSON">
        /// String to convert
        /// </param>
        /// <returns>
        /// Contents of <paramref name="pstrStringFromJSON"/> parsed into the
        /// most appropriate representation from among DateTime, Integer, and
        /// Double, in that order of preference
        /// </returns>
        internal static object ConvertToAppropriateType ( string pstrStringFromJSON )
        {
            DateTime dtmTemp;

            if ( DateTime.TryParse ( pstrStringFromJSON , out dtmTemp ) )
            {
                return dtmTemp;
            }   // TRUE (Input value is a DateTime.) block, if ( DateTime.TryParse ( pstrStringFromJSON , out dtmTemp ) )
            else
            {
                long lngTemp;

                if ( long.TryParse ( pstrStringFromJSON , out lngTemp ) )
                {
                    return lngTemp;
                }   // TRUE (Input value is a Long Integer.) block, if ( long.TryParse ( pstrStringFromJSON , out lngTemp ) )
                else
                {
                    double dblTemp;

                    if ( double.TryParse ( pstrStringFromJSON , out dblTemp ) )
                    {
                        return dblTemp;
                    }   // TRUE (Input value is a Double Precision floating point number.) block, if ( double.TryParse ( pstrStringFromJSON , out dblTemp ) )
                    else
                    {
                        return pstrStringFromJSON;
                    }   // FALSE (Input value is of another type) block, if ( double.TryParse ( pstrStringFromJSON , out dblTemp ) )
                }   // FALSE (Input value is of another type.) block, if ( long.TryParse ( pstrStringFromJSON , out lngTemp ) )
            }   // FALSE (Input value is of another type.) block, if ( DateTime.TryParse ( pstrStringFromJSON , out dtmTemp ) )
        }   // internal static object ConvertToAppropriateType


        /// <summary>
        /// Read the raw JSON string from a text file, which is expected to be
        /// stored in a text file which is fully qualified by application config
        /// values JSON_INPUT_FILE_NAME and DATA_DIRECTORY_NAME.
        /// </summary>
        /// <returns>
        /// If the method succeeds, the return value is the sample JSON string
        /// to be converted. Since it can throw an I/O exception, this method
        /// must be called from whthin a Try block.
        /// </returns>
        public static string GetRawJSONString ( string pstrRESTResponseFileName )
        {
            string strAbsoluteInputFileName = AssembleAbsoluteFileName ( pstrRESTResponseFileName );

            Console.WriteLine (
                ShowFileDetails (                                               // Print the returned string.
                    Properties.Resources.FILE_LABEL_INPUT ,                     // string pstrLabel
                    strAbsoluteInputFileName ) );                               // string pstrFileName
            return File.ReadAllText ( strAbsoluteInputFileName );               // string pstrEndOfLastLine = SpecialStrings.EMPTY_STRING )
        }   // private static string GetRawJSONString


        /// <summary>
        /// Preserve the contents of a string by writing it into a file.
        /// </summary>
        /// <param name="pstrPreserveThisResult">
        /// String containing text to be preserved in the file identified by
        /// <paramref name="pstrOutputFileNamePerSettings"/>
        /// </param>
        /// <param name="pstrOutputFileNamePerSettings">
        /// String containing the unqualified name to give to the file into
        /// which the string specified by <paramref name="pstrPreserveThisResult"/>
        /// should be written
        /// </param>
        /// <param name="pstrLabelForReportMessage">
        /// String containing the label to use as the prefix on the report about
        /// the output file specified by <paramref name="pstrOutputFileNamePerSettings"/>
        /// after the string specified by <paramref name="pstrLabelForReportMessage"/>
        /// is written into it
        /// </param>
        public static void PreserveResult (
            string pstrPreserveThisResult ,
            string pstrOutputFileNamePerSettings ,
            string pstrLabelForReportMessage )
        {
            string strAbsoluteInputFileName = AssembleAbsoluteFileName ( pstrOutputFileNamePerSettings );

            File.WriteAllText ( strAbsoluteInputFileName , pstrPreserveThisResult );

            Console.WriteLine (
                ShowFileDetails (                                               // Print the returned string.
                    pstrLabelForReportMessage ,                                 // string pstrLabel
                    strAbsoluteInputFileName ) );                               // string pstrFileName
        }   // private static void PreserveResult


        /// <summary>
        /// Use a FileInfo object to assemble and report details about a file.
        /// This method wraps the FileInfo extenson method of the same name with
        /// a call to the FileInfo constructor, passing the file name specified
        /// by <paramref name="pstrFileName"/>.
        /// </summary>
        /// <param name="pstrLabel">
        /// Label for report about file <paramref name="pstrFileName"/>, passed
        /// into the extension method
        /// </param>
        /// <param name="pstrFileName">
        /// File about which to compose report labeled per <paramref name="pstrLabel"/>
        /// </param>
        /// <param name="pstrEndOfLastLine">
        /// Optional termination string for report, defaulting to the empty
        /// string, but overrideable with e. g., a newline to follow the report
        /// with a blank line, passed into the extension method
        /// </param>
        /// <param name="pfPrefixWithNewline">
        /// If TRUE, the whole string returned with a platform-dependent newline.
        /// Otherwise, the string begins with the label specified by parameter
        /// pstrLabel, if any, followed by the absolute file name. The default
        /// value is FALSE. This argument is passed into the extension method.
        /// </param>
        /// <param name="pfSuffixWithNewline">
        /// If TRUE, the returned string ends with a platform-dependent newline.
        /// Otherwise, the string ends with the attributes property message. The
        /// default value is TRUE. This argument is passed into the extension
        /// method.
        /// </param>
        /// <returns>
        /// The return value is a string for use as a console or log message.
        /// </returns>
        /// <remarks>
        /// Other than hiding the FileInfo object and allowing it to go out of
        /// scope when it returns, this method adds nothing of value, especially
        /// now that I discovered, in another project, that it is possible to
        /// chain an extnsion method call onto a constructor. This technique can
        /// eliminate this method entirely, but I decided that the addtional
        /// time investment was unjustified for this demonstration program. The
        /// technique is demonstrated, however, in the test stand program for my
        /// WizardWrx .NET API library constellation.
        /// </remarks>
        public static string ShowFileDetails (
            string pstrLabel ,
            string pstrFileName ,
            bool pfPrefixWithNewline = false ,
            bool pfSuffixWithNewline = true )
        {
            FileInfo info = new FileInfo ( pstrFileName );
            return info.ShowFileDetails (                                       // Print the returned string.
                FileInfoExtensionMethods.FileDetailsToShow.Everything ,         // FileDetailsToShow penmFileDetailsToShow = FileDetailsToShow.Everything
                pstrLabel ,                                                     // string            pstrLabel             = null
                pfPrefixWithNewline ,                                           // bool              pfPrefixWithNewline   = false
                pfSuffixWithNewline );                                          // bool              pfSuffixWithNewline   = false
        }   // private static string ShowFileDetails


        internal static int TestDone (
            int pintFinalStatusCode ,
            int pintTestNumber )
        {
            Console.WriteLine (
                Properties.Resources.MSG_DONE ,                                 // Message Template
                pintTestNumber ,                                                // Format Item 0 = Test Number
                pintFinalStatusCode ,                                           // Format Item 1 = Final Status Code
                Environment.NewLine );                                          // Format Item 2 = Newline
            return pintFinalStatusCode;
        }   // TestDone method
    }   // class Utl
}   // partial namespace LineBreakFixupsDemo