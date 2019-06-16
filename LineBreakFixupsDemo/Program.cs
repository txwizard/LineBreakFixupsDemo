/*
    ============================================================================

    Namespace:			LineBreakFixupsDemo

    Class Name:			Program

	File Name:			Program.cs

    Synopsis:			This class implements the entry point of a console mode
                        assembly.

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
	2019/06/16 1.0     DAG Class created from Visual Studio template
    ============================================================================
*/

using System;

using System.Configuration;
using System.IO;

using WizardWrx;
using WizardWrx.ConsoleAppAids3;


namespace LineBreakFixupsDemo
{
    class Program
    {
        const bool CONVERT_TO_UNIX_LINE_BREAKS = true;
        const bool PRESERVE_EXISTING_LINE_BREAKS = false;

        enum SelectedTest
        {
            LineBreaks ,
            AppSettingsList ,
            StringResourceList ,
            TransformJSONString ,
        }   // enum SelectedTest

        static ConsoleAppStateManager s_smThisApp = ConsoleAppStateManager.GetTheSingleInstance ( );

        static JSONDeserializationUseCase [ ] s_aDeserializationUseCases =
        {                                 // TestReportLabel = pstrTestReportLabel;     RESTResponseFileName = pstrFinalOutputFileName;            ConvertLineEndings = pfConvertLineEndings;   IntermediateFileName = pstrIntermediateFileName                               FinalOutputFileName = pstrFinalOutputFileName;                         ResponseObjectFileName = pstrResponseObjectFileName;
            new JSONDeserializationUseCase ( Properties.Resources.MSG_TEST_4_PROLOGUE , Properties.Settings.Default.JSON_INPUT_FILE_NAME_WINDOWS , CONVERT_TO_UNIX_LINE_BREAKS                , Properties.Settings.Default.JSON_INTERMEDIATE_FILE_NAME_WINDOWS_TRANSFORMED , Properties.Settings.Default.JSON_FINAL_FILE_NAME_WINDOWS_TRANSFORMED , Properties.Settings.Default.JSON_CONTENTS_REPORT_FILE_NAME_WINDOWS_TRANSFORMED ) ,
            new JSONDeserializationUseCase ( Properties.Resources.MSG_TEST_5_PROLOGUE , Properties.Settings.Default.JSON_INPUT_FILE_NAME_WINDOWS , PRESERVE_EXISTING_LINE_BREAKS              , Properties.Settings.Default.JSON_INTERMEDIATE_FILE_NAME_WINDOWS_RAW         , Properties.Settings.Default.JSON_FINAL_FILE_NAME_WINDOWS_RAW         , Properties.Settings.Default.JSON_CONTENTS_REPORT_FILE_NAME_WINDOWS_RAW         ) ,
            new JSONDeserializationUseCase ( Properties.Resources.MSG_TEST_6_PROLOGUE , Properties.Settings.Default.JSON_INPUT_FILE_NAME_UNIX    , PRESERVE_EXISTING_LINE_BREAKS              , Properties.Settings.Default.JSON_INTERMEDIATE_FILE_NAME_UNIX                , Properties.Settings.Default.JSON_FINAL_FILE_NAME_UNIX                , Properties.Settings.Default.JSON_CONTENTS_REPORT_FILE_NAME_UNIX                ) ,
        };  // static JSONDeserializationUseCase [ ] s_aDeserializationUseCases


        /// <summary>
        /// This routine is the entry point of the entry assembly.
        /// </summary>
        /// <param name="args">
        /// When specified, the args array contains a single entry, which must
        /// match the string representation of a member of the SelectedTest
        /// enumeration. When the string corresponds to the string
        /// representation of a SelectedTest member, the corresponding test is
        /// performed. When no argument is given, all tests are performed. When
        /// the specified argument is invalid, the error is reported, and
        /// nothing else happens.
        /// </param>
        static void Main ( string [ ] args )
        {
            int intTestNumber = ListInfo.LIST_IS_EMPTY;

            s_smThisApp.DisplayBOJMessage ( );
            s_smThisApp.BaseStateManager.AppExceptionLogger.OptionFlags = s_smThisApp.BaseStateManager.AppExceptionLogger.OptionFlags | WizardWrx.DLLConfigurationManager.ExceptionLogger.OutputOptions.Stack;

            var varAppSettings = Properties.Settings.Default;
            var varAppStrings = Properties.Resources.ResourceManager.GetResourceSet (
                System.Globalization.CultureInfo.CurrentCulture ,               // System.Globalization.CultureInfo culture
                false ,                                                         // bool                             createIfNotExists
                true );                                                         // bool                             tryParents

            try
            {
                if ( RunThisTest ( SelectedTest.LineBreaks , args ) )
                {
                    s_smThisApp.BaseStateManager.AppReturnCode = LineEndingFixupTests.Exercise ( ref intTestNumber );
                }   // if ( RunThisTest ( SelectedTest.LineBreaks , args ) )

                if ( RunThisTest ( SelectedTest.AppSettingsList , args ) )
                {
                    intTestNumber = ListAppSettings ( intTestNumber );
                }   // if ( RunThisTest ( SelectedTest.AppSettingsList , args ) )

                if ( RunThisTest ( SelectedTest.StringResourceList , args ) )
                {
                    intTestNumber = ListEmbeddedResources ( intTestNumber );
                }   // if ( RunThisTest ( SelectedTest.StringResourceList , args ) )

                if ( RunThisTest ( SelectedTest.TransformJSONString , args ) )
                {
                    for ( int intUseCaseIndex = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                          intUseCaseIndex < s_aDeserializationUseCases.Length ;
                          intUseCaseIndex++ )
                    {
                        try
                        {
                            JSONDeserializationUseCase deserializationUseCase = s_aDeserializationUseCases [ intUseCaseIndex ];
                            intTestNumber = PerformJSONTransofmration (
                                intTestNumber ,                                             // int    pintTestNumber                OK
                                deserializationUseCase.ConvertLineEndings ,                 // bool   pfConvertLineEndings          OK
                                deserializationUseCase.TestReportLabel ,                    // string pstrTestReportLabel           OK
                                deserializationUseCase.RESTResponseFileName ,               // string pstrRESTResponseFileName      OK
                                deserializationUseCase.IntermediateFileName ,               // string pstrIntermediateFileName      OK
                                deserializationUseCase.FinalOutputFileName ,                // string pstrFinalOutputFileName       OK
                                deserializationUseCase.ResponseObjectFileName );            // string pstrResponseObjectFileName    OK
                        }
                        catch ( Exception exAll )
                        {
                            s_smThisApp.BaseStateManager.AppExceptionLogger.ReportException ( exAll );
                        }
                    }   // for ( int intUseCaseIndex = ArrayInfo.ARRAY_FIRST_ELEMENT ; intUseCaseIndex < s_aDeserializationUseCases.Length ; intUseCaseIndex++ )
                }   // if ( RunThisTest ( SelectedTest.TransformJSONString , args ) )
            }
            catch ( InvalidOperationException exInvOp )
            {
                if ( exInvOp.Message == Properties.Resources.ERRMSG_CMDARG_IS_INVALID )
                {
                    s_smThisApp.ErrorExit ( MagicNumbers.ERROR_RUNTIME + MagicNumbers.PLUS_ONE );
                }   // TRUE (anticpated outcome) block, if ( exInvOp.Message == Properties.Resources.ERRMSG_CMDARG_IS_INVALID )
                else
                {
                    s_smThisApp.BaseStateManager.AppExceptionLogger.ReportException ( exInvOp );
                    s_smThisApp.ErrorExit ( MagicNumbers.ERROR_RUNTIME );
                }   // FALSE (unanticpated outcome) block, if ( exInvOp.Message == Properties.Resources.ERRMSG_CMDARG_IS_INVALID )
            }
            catch ( Exception exAll )
            {
                s_smThisApp.BaseStateManager.AppExceptionLogger.ReportException ( exAll );
                s_smThisApp.ErrorExit ( MagicNumbers.ERROR_RUNTIME );
            }

            //s_smThisApp.DisplayEOJMessage ( );            // Since s_smThisApp.NormalExit displays this message, this statement is redundant.
            s_smThisApp.NormalExit ( ConsoleAppStateManager.NormalExitAction.WaitForOperator );
        }   // static void Main


        /// <summary>
        /// When called by the main routine, this routine generates two lists of
        /// the application settings defined in the application configuration
        /// file associated with the entry assembly. The first list is displayed
        /// on the console, while the second is written into a file as a set of
        /// tab delimited list of records.
        /// </summary>
        /// <param name="pintTestNumber">
        /// The sequential test number is passed into this routine, which
        /// increments it, and returns the new value.
        /// </param>
        /// <returns>
        /// The return value is <paramref name="pintTestNumber"/> incremented by
        /// one.
        /// </returns>
        private static int ListAppSettings ( int pintTestNumber )
        {
            Utl.BeginTest (
                Properties.Resources.MSG_TEST_2_PROLOGUE ,
                ref pintTestNumber );

            //  ----------------------------------------------------------------
            //  Load the settings into a SettingsPropertyCollection object, then
            //  enumerate it with two objectives.
            //
            //  1)  Show that the items returned by the enumerator are unsorted.
            //  2)  Fill an array of SortableSettingsProperty objects.
            //  ----------------------------------------------------------------

            SettingsPropertyCollection spcMySettings = Properties.Settings.Default.Properties;
            int intTotalItems = spcMySettings.Count;
            Console.WriteLine (
                Properties.Resources.MSG_SETTINGS_ENUMERATION_HEADING ,         // Format control string
                intTotalItems ,                                                 // Format Item 0
                Environment.NewLine );                                          // Format Item 1
            SortableSettingsProperty [ ] asspSortableSettings = new SortableSettingsProperty [ intTotalItems ];
            int intItenNumber = ListInfo.LIST_IS_EMPTY;

            foreach ( SettingsProperty setting in spcMySettings )
            {   // Do everything in one pass, using the third constructor, since the single-argument constructor left most of the fields uninitialized.
                asspSortableSettings [ intItenNumber ] = new SortableSettingsProperty (
                    setting.Name ,                                              // string                       name
                    setting.PropertyType ,                                      // Type                         propertType
                    setting.Provider ,                                          // SettingsProvider             provider
                    true ,                                                      // bool                         isReadOnly
                    setting.DefaultValue ,                                      // object                       defaultValue
                    setting.SerializeAs ,                                       // SettingsSerializeAs          serializeAs
                    setting.Attributes ,                                        // SettingsAttributeDictionary  attributes
                    true ,                                                      // bool                         throwOnErrorDeserializing
                    true );                                                     // bool                         throwOnErrorSerializing
                Console.WriteLine (
                    Properties.Resources.MSG_APP_SETTINGS_DETAIL_TEMPLATE ,     // Format control string
                    ++intItenNumber ,                                           // Format Item 0: {0,2} of
                    intTotalItems ,                                             // Format Item 1: of {1,2}:
                    setting.Name ,                                              // Format Item 2: Name = {2}
                    setting.DefaultValue );                                     // Format Item 3: Value = {3}
            }   // foreach ( SettingsProperty setting in spcMySettings )

            //  ----------------------------------------------------------------
            //  Sort the array, then iterate it by way of a conventional FOR
            //  loop, listing the name, type, and default value of each setting.
            //  This listing is ordered by setting name.
            //  ----------------------------------------------------------------

            Array.Sort ( asspSortableSettings );
            Console.WriteLine (
                Properties.Resources.MSG_SORTED_APP_SETTINGS_LIST_HEADING ,     // Format control string
                intTotalItems ,                                                 // Format Item 0: Enumerate the {0}
                Environment.NewLine );                                          // Format Item 1: sorted settings:{1}
            intItenNumber = ListInfo.LIST_IS_EMPTY;

            string strAppSettingsReportFileName = Utl.AssembleAbsoluteFileName ( Properties.Settings.Default.APP_SETTINGS_REPORT_FILENAME );
            using ( StreamWriter swAppSettings = new StreamWriter ( strAppSettingsReportFileName ,
                                                                    FileIOFlags.FILE_OUT_CREATE ,
                                                                    System.Text.Encoding.ASCII ,
                                                                    MagicNumbers.CAPACITY_08KB ) )
            {
                string strDetailRowFormatString = GenerateDetailFormatStringFromLabelRow ( swAppSettings );

                for ( int intIndex = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                          intIndex < intTotalItems ;
                          intIndex++ )
                {
                    SortableSettingsProperty settingsProperty = asspSortableSettings [ intIndex ];
                    Console.WriteLine (
                        Properties.Resources.MSG_APP_SETTINGS_SORTED_ITEM ,     // Format control string
                        ++intItenNumber ,                                       // Format Item 0: {0,2} of
                        intTotalItems ,                                         // Format Item 1: of {1,2}:
                        settingsProperty.Name ,                                 // Format Item 2: Name = {2}
                        settingsProperty.PropertyType.FullName ,                // Format Item 3: Type = {2}
                        settingsProperty.DefaultValue );                        // Format Item 4: Value = {4}
                    swAppSettings.WriteLine (
                        strDetailRowFormatString ,                              // The generated format control string consists entirely of format items and TAB characters.
                        intItenNumber ,                                         // The item number was incremented just before the console line was assembled.
                        settingsProperty.Name ,
                        settingsProperty.PropertyType.FullName ,
                        settingsProperty.DefaultValue );
                }   // for ( int intIndex = ArrayInfo.ARRAY_FIRST_ELEMENT ; intIndex < intTotalItems ; intIndex++ )
            }   // using ( StreamWriter swAppSettings = new StreamWriter ( Utl.AssembleAbsoluteFileName ( Properties.Settings.Default.APP_SETTINGS_REPORT ) , FileIOFlags.FILE_OUT_CREATE , System.Text.Encoding.ASCII , MagicNumbers.CAPACITY_08KB ) )

            Console.WriteLine (
                Utl.ShowFileDetails (
                    Properties.Resources.FILE_LABEL_APP_SETTINGS_LIST ,         // string pstrLabel
                    strAppSettingsReportFileName ,                              // string pstrFileName
                    true ,                                                      // bool   pfPrefixWithNewline = false
                    false ) );                                                  // bool   pfSuffixWithNewline = true
            s_smThisApp.BaseStateManager.AppReturnCode = Utl.TestDone (
                MagicNumbers.ERROR_SUCCESS ,
                pintTestNumber );
            return pintTestNumber;
        }   // private static int ListAppSettings


        /// <summary>
        /// When called by the main routine, this routine generates two lists of
        /// the string resources embedded in the executing assembly. The first
        /// list is displayed on the console, while the second is written into a
        /// file as a set of tab delimited list of records.
        /// </summary>
        /// <param name="pintTestNumber">
        /// The sequential test number is passed into this routine, which
        /// increments it, and returns the new value.
        /// </param>
        /// <returns>
        /// The return value is <paramref name="pintTestNumber"/> incremented by
        /// one.
        /// </returns>
        private static int ListEmbeddedResources ( int pintTestNumber )
        {
            Utl.BeginTest (
                Properties.Resources.MSG_TEST_3_PROLOGUE ,
                ref pintTestNumber );
            string strAppSettingsReportFileName = Utl.AssembleAbsoluteFileName (
                Properties.Settings.Default.EMBEDDED_RESOURCES_REPORT_FILENAME );

            using ( StreamWriter writer = new StreamWriter ( strAppSettingsReportFileName ,
                                                             FileIOFlags.FILE_OUT_CREATE ,
                                                             System.Text.Encoding.Unicode ,
                                                             MagicNumbers.CAPACITY_08KB ) )
            {
                WizardWrx.AssemblyUtils.SortableManagedResourceItem.ListResourcesInAssemblyByName (
                    System.Reflection.Assembly.GetExecutingAssembly ( ) ,
                    writer );
            }   // using ( StreamWriter writer = new StreamWriter ( strAppSettingsReportFileName , FileIOFlags.FILE_OUT_CREATE , System.Text.Encoding.Unicode , MagicNumbers.CAPACITY_08KB ) )

            Console.WriteLine (
                Utl.ShowFileDetails (
                    Properties.Resources.FILE_LABEL_EMBEDD3ED_RESOUCES_REPORT , // string pstrLabel
                    strAppSettingsReportFileName ,                              // string pstrFileName
                    true ,                                                      // bool   pfPrefixWithNewline = false
                    false ) );                                                  // bool   pfSuffixWithNewline = true
            s_smThisApp.BaseStateManager.AppReturnCode = Utl.TestDone (
                MagicNumbers.ERROR_SUCCESS ,
                pintTestNumber );
            return pintTestNumber;
        }   // private static int ListEmbeddedResources


        /// <summary>
        /// Generate the format control string for the detail row from the label
        /// row, and write the label row into the output file before it boes out
        /// of scope.
        /// </summary>
        /// <param name="swAppSettings">
        /// Pass in a reference to the StreamWriter that was created for the TSV
        /// report.
        /// </param>
        /// <returns>
        /// The return value is the string through which to write detail rows.
        /// </returns>
        private static string GenerateDetailFormatStringFromLabelRow ( StreamWriter swAppSettings )
        {
            string strLabelRow = Properties.Resources.LBL_APP_SETTINGS_LIST.ReplaceEscapedTabsInStringFromResX ( );
            swAppSettings.WriteLine ( strLabelRow );
            return ReportHelpers.DetailTemplateFromLabels ( strLabelRow );
        }   // private static string GenerateDetailFormatStringFromLabelRow


        /// <summary>
        /// Perform the first JSON transformation exercise.
        /// </summary>
        /// <param name="pintTestNumber">
        /// This integer is the number assigned to the last test that ran, which
        /// is fed to Utl.BeginTest, along with <paramref name="pstrTestReportLabel"/>,
        /// which increments it. Because the value is passed by reference, Utl.BeginTest
        /// need not return it to preserve the new value.
        /// </param>
        /// <param name="pfConvertLineEndings">
        /// Specify TRUE to have line endings in input file
        /// <paramref name="pstrRESTResponseFileName"/> converted to Unix line
        /// breaks, or FALSE to preserve whatever is there, which will be
        /// Windows line breaks.
        /// </param>
        /// <param name="pstrTestReportLabel">
        /// Specify the string to apply as a label to the test report. Utility
        /// method Utl.BeginTest receives this value, along with
        /// <paramref name="pintTestNumber"/>.
        /// </param>
        /// <param name="pstrRESTResponseFileName">
        /// Pass in a string that contains the unqualified name of the file that
        /// contains the JSON response returned by the REST API.
        /// </param>
        /// <param name="pstrIntermediateFileName">
        /// Pass in a string that contains the unqualified name of the file that
        /// will store the output of the first transformation of the data in the
        /// file named by <paramref name="pstrRESTResponseFileName"/>.
        /// </param>
        /// <param name="pstrFinalOutputFileName">
        /// Pass in a string that contains the unqualified name of the file that
        /// will store the output of the second transformation of the data in
        /// the file named by <paramref name="pstrRESTResponseFileName"/>.
        /// </param>
        /// <param name="pstrResponseObjectFileName">
        /// Pass in a string that contains the unqualified name of the file that
        /// will store the output of the report of member values of the 
        /// deserialized object constructed from the transformed data read from
        /// the file named by <paramref name="pstrRESTResponseFileName"/>.
        /// </param>
        /// <returns>
        /// This routine calls Utl.BeginTest, which increments the test number.
        /// The new value is returned so that it can be fed to the next test.
        /// </returns>
        /// <remarks>
        /// The argument list could be simplified by passing in a reference to
        /// the JSONDeserializationUseCase object from which all but the first
        /// parameter is extracted. It wasn't because I perfected the test from
        /// a set of discrete values; the JSONDeserializationUseCase objec came
        /// much later, and I saw no point in changing the signature for a
        /// demonstration program.
        /// </remarks>
        private static int PerformJSONTransofmration (
            int pintTestNumber ,
            bool pfConvertLineEndings ,
            string pstrTestReportLabel ,
            string pstrRESTResponseFileName ,
            string pstrIntermediateFileName ,
            string pstrFinalOutputFileName ,
            string pstrResponseObjectFileName )
        {
            Utl.BeginTest (
                pstrTestReportLabel ,
                ref pintTestNumber );

            string strRawResponse = pfConvertLineEndings
                                    ? Utl.GetRawJSONString ( pstrRESTResponseFileName ).UnixLineEndings ( )
                                    : Utl.GetRawJSONString ( pstrRESTResponseFileName );
            JSONFixupEngine engine = new JSONFixupEngine ( @"TIME_SERIES_DAILY_ResponseMap" );
            string strFixedUp_Pass_1 = engine.ApplyFixups_Pass_1 ( strRawResponse );

            Utl.PreserveResult (
                strFixedUp_Pass_1 ,                                             // string pstrPreserveThisResult
                pstrIntermediateFileName ,                                      // string pstrOutputFileNamePerSettings
                Properties.Resources.FILE_LABEL_INTERMEDIATE );                 // string pstrLabelForReportMessage

            string strFixedUp_Pass_2 = engine.ApplyFixups_Pass_2 ( strFixedUp_Pass_1 );

            Utl.PreserveResult (
                strFixedUp_Pass_2 ,                                             // string pstrPreserveThisResult
                pstrFinalOutputFileName ,                                       // string pstrOutputFileNamePerSettings
                Properties.Resources.FILE_LABEL_FINAL );                        // string pstrLabelForReportMessage

            //  ------------------------------------------------------------
            //  TimeSeriesDailyResponse
            //  ------------------------------------------------------------

            Utl.ConsumeResponse (
                pstrResponseObjectFileName ,
                Newtonsoft.Json.JsonConvert.DeserializeObject<TimeSeriesDailyResponse> (
                    strFixedUp_Pass_2 ) );

            s_smThisApp.BaseStateManager.AppReturnCode = Utl.TestDone (
                MagicNumbers.ERROR_SUCCESS ,
                pintTestNumber );
            return pintTestNumber;
        }   // private static int PerformJSONTransofmration


        /// <summary>
        /// 
        /// </summary>
        /// <param name="penmSelectedTest">
        /// This member of the SelectedTest enumeration identifies the test that
        /// is up for consideration.
        /// </param>
        /// <param name="pastrCmdArgs">
        /// The command line argument list is an array of zero or more strings,
        /// each of which is a command line argument.
        /// </param>
        /// <returns>
        /// The return value is TRUE when <paramref name="penmSelectedTest"/>
        /// is the first, or only, command line argument, or when no command
        /// line arguments were given. Otherwise, a message is displayed on the
        /// console, and the return value is FALSE.
        /// </returns>
        private static bool RunThisTest (
            SelectedTest penmSelectedTest ,
            string [ ] pastrCmdArgs )
        {
            if ( pastrCmdArgs.Length == ArrayInfo.ARRAY_IS_EMPTY )
            {
                return true;
            }   // TRUE (No arguments were given on the command line, the degenerate case.) block, if ( pastrCmdArgs.Length == ArrayInfo.ARRAY_IS_EMPTY )
            else
            {
                try
                {
                    SelectedTest selected = pastrCmdArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ].EnumFromString<SelectedTest> ( );
                    return penmSelectedTest == selected;
                }
                catch ( InvalidOperationException )
                {
                    string strMsgTpl = Properties.Resources.ERRMSG_CMDARG_IS_INVALID;
                    WizardWrx.ConsoleStreams.ErrorMessagesInColor.RGBWriteLine (
                        s_smThisApp.BaseStateManager.AppExceptionLogger.ErrorMessageColors.MessageForegroundColor ,
                        s_smThisApp.BaseStateManager.AppExceptionLogger.ErrorMessageColors.MessageBackgroundColor ,
                        strMsgTpl ,
                        pastrCmdArgs [ ArrayInfo.ARRAY_FIRST_ELEMENT ].QuoteString ( ) );
                    throw new InvalidOperationException ( strMsgTpl );
                }
                catch ( Exception exAllOthers )
                {
                    throw exAllOthers;
                }
            }   // FALSE (At least one argument was given on the command line.) block, if ( pastrCmdArgs.Length == ArrayInfo.ARRAY_IS_EMPTY )
        }   // private static bool RunThisTest
    }   // class Program
}   // partial namespace LineBreakFixupsDemo