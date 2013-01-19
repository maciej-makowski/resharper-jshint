/*jslint browser: true */
/*global alert*/

aGlobal = "";

aGlobalFunction();

alert("no semicolon")

aGlobal;	// overridden by resharper

alert("double") alert(" no semi");	// overridden by resharper

var i, x;
i = 2, x = 3;
