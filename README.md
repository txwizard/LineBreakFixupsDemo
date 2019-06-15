# LineBreakFixupsDemo README

This repository is the companion to two articles that will soon be published on
The Code Project. When they are published, the URLs will follow this paragraph.

## Background

The original objective of the article was to demonstrate the technique that I
devised for efficiently transforming the JSON string returned by a REST API into
something that can be fed into the Newtonsoft JSON deserializer to produce an
object, and to the Visual Studio code editor to produce the strongly typed class
into which to deserialize it.

Preparations for that article included cloning the GitHub repository into a new
local repositiry, which exposed a nasty surprise; regardless of what I did to
coerce the character-mode Git client to do otherwise, it downloaded the file
that contained the JSON string, with its Unix line breaks, replacing them with
Windows line breaks. Since the second phase of the conversion depended upon Unix
line breaks, this broke the program.

While I know that programs exist that can replace Unix line breaks with Windows
line breaks, and vice versa, I decided to create one of my own, in the spirit of
my very fast AnyCSV delimited text parser.

## Using the Code

To work around the line break conversion issue described above, and provide a
ready-made set of binaries to run, the GitHub repository contains two ZIP
archives that must be extracted after you clone the repository.

|Archive Name |Archive Description                                                                                                                                                            |
|-------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|Binaries.zip |This archive contains the binary (compiled, executable) files created by the build process. These comprise the `/bin` (output) and `/obj` (intermediate) output directories created by the build engine.                                                                                                                                  |
|Test_Data.zip|This archive contains two versions of the file that stores the JSON string, one containing the original Unix line breaks, and the other with the Windows line breaks substituted by the Git client, along with a Microsoft Excel workbook that contains imported reports of application settings and string resources, among other things.|

Follow these step-by-step instructions to prepare the repository for use. These
instructions assume that you have the Git command line tools, and that they are
configured to work with `cmd.exe`.

1. Open a command prompt into the directory (folder) in which you want to create
   your repository.

2. Enter the following command to download the repository.

        `git clone https://github.com/txwizard/LineBreakFixupsDemo.git`

   The above command creates a new directory, `LineBreakFixupsDemo`, into which
   it copies the contents of the repository.

3. Change to the repository directory. The simplest form of the console command
   to do this is as follows.

   		`cd LineBreakFixupsDemo`

4. The next step assumes that your preferred archiver is 7-zip. Unlike its
   commercial rival (WinZip), the command line tools come "in the box," and are
   automatically installed. If so, enter the following command; otherwise, skip
   to step 5.

   			`7z x Test_Data.zip`

   The output should look like the following.

		`7-Zip 18.05 (x64) : Copyright (c) 1999-2018 Igor Pavlov : 2018-04-30`

		`Scanning the drive for archives:`
		`1 file, 34914 bytes (35 KiB)`

		`Extracting archive: Test_Data.zip`
		`--`
		`Path = Test_Data.zip`
		`Type = zip`
		`Physical Size = 34914`

		`Everything is Ok`

		`Folders: 1`
		`Files: 3`
		`Size:       105111`
		`Compressed: 34914`

   Skip to step 7.

5. Though there are numerous hacks for unzipping with the command line without
   the benefit of third party tools, such as <https://stackoverflow.com/questions/28043589/how-can-i-compress-zip-and-uncompress-unzip-files-and-folders-with-bat></https://stackoverflow.com/questions/28043589/how-can-i-compress-zip-and-uncompress-unzip-files-and-folders-with-bat>
   and many others, the quickest way to unzip a file is by way of the File
   Explorer. Right click the archive, `Test_Data.zip`, and select "extract all files."

6. Selecting the "extract all files" item, displays a dialog box that prompts
   for a destination folder. The default displayed therein should suffice. For
   example, when the repository folder is `C:\temp\LineBreakFixupsDemo`, the prompt
   displays the following directory (folder).

   		`C:\temp\LineBreakFixupsDemo\Test_Data`

   Click the __Extract__ button to complete the task.

7. Repeat steps 4 through 6 for `Binaries.zip`.

The repository is ready for use, and you can see the program in action by double-clicking
`[RepositoryDirectoryName]\LineBreakFixupsDemo\bin\Release\LineBreakFixupsDemo.exe`, where
`[RepositoryDirectoryName]` is the absolute (fully qualfied) name of the directory in which
you created the repository clone.

Alternatively, you can execute the command line described above in the command prompt window
that you opened to use the Git command line interface.