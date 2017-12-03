/* The size of these files were reduced. The following function fixes all references. */
var $customMSCore = JSIL.GetAssembly("mscorlib");
var $customSys = JSIL.GetAssembly("System");
var $customSysConf = JSIL.GetAssembly("System.Configuration");
var $customSysCore = JSIL.GetAssembly("System.Core");
var $customSysNum = JSIL.GetAssembly("System.Numerics");
var $customSysXml = JSIL.GetAssembly("System.Xml");
var $customSysSec = JSIL.GetAssembly("System.Security");

if (typeof (contentManifest) !== "object") { contentManifest = {}; };
contentManifest["Fusee.Engine.Player.Web.contentproj"] = [
    ["Script",	"Fusee.Base.Core.Ext.js",	{  "sizeBytes": 1273 }],
    ["Script",	"Fusee.Base.Imp.Web.Ext.js",	{  "sizeBytes": 13393 }],
    ["Script",	"opentype.js",	{  "sizeBytes": 166330 }],
    ["Script",	"Fusee.Engine.Imp.Graphics.Web.Ext.js",	{  "sizeBytes": 110669 }],
    ["Script",	"Fusee.Xene.Ext.js",	{  "sizeBytes": 1441 }],
    ["Script",	"Fusee.Xirkit.Ext.js",	{  "sizeBytes": 44215 }],
    ["Script",	"SystemExternals.js",	{  "sizeBytes": 11976 }],
    ["Image",	"Assets/FuseeAnim.gif",	{  "sizeBytes": 221729 }],
    ["Image",	"Assets/FuseeSpinning.gif",	{  "sizeBytes": 19491 }],
    ["Image",	"Assets/FuseeText.png",	{  "sizeBytes": 4009 }],
    ["File",	"Assets/Lato-Black.ttf",	{  "sizeBytes": 114588 }],
    ["File",	"Assets/Model.fus",	{  "sizeBytes": 70107 }],    ["File",    "Assets/AboutFuseeAssets.txt", {  "sizeBytes": 196 }],
    ["File",    "Assets/Dummy.txt", {  "sizeBytes": 0 }],

    ];