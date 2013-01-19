# JSHint plugin for Resharper

A fork on [JSLint plugin for Resharper](http://resharperjslint.codeplex.com/) that uses 
[JSHint](http://www.jshint.com/) for JavaScript linting.

## Requirements
 - [Resharper 7.0](http://www.jetbrains.com/resharper/),
 - [Resharper 7.0 SDK](http://download.jetbrains.com/resharper/ReSharperSDK-7.1.96.msi) for development

## Installation
 - Get the project,
 - Build it,
 - Copy `Resharper.JSHint.dll` to `C:\Program Files (x86)\JetBrains\ReSharper\v7.1\Bin\Plugins\` or wherever
   your Resharper plugins live

## Differences to JSHint plugin
 - Apparantly uses JSHint,
 - Additional configuration option is avaliable to pass the path to JSON formatted JSHint configuration.
   If left empty or file has not been found, the configuration is taken from the UI

## Known problems and incompatibilities
 - Acceptance tests fail, probably due to the incompatibilities between options and output messages between
   JSLint and JSHint,
 - Didn't even try to run UI tests project,
 - Options screen still contains original JSLint option names and values. This means that if you don't use
   JSON configuration a lot of thing may not work as expected. Example of incompatible option is `white`,
   which `true` in jsLint is same as `false` in JSHint,
 - Removed installer due to the constant nagging from Visual Studio that it cannot load the project

## Credits where credits are due
 - [JSLint plugin for Resharper](http://resharperjslint.codeplex.com) by Lars-Erik Aabech, licensed under
   [MIT license](http://resharperjslint.codeplex.com/license), is probably father to the most of the code I
   blantantly ripped off and run `s/JSLint/JSHint/g` on it,
 - [JSHint](http://www.jshint.com) by Anton Kovalyov, licensed under
   [this license](https://raw.github.com/jshint/jshint/master/LICENSE)
 - [Resharper](http://www.jetbrains.com/resharper/) by JetBrains
