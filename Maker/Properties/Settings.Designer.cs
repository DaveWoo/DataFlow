﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Maker.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("using DataFlow.Model;\r\nusing DataFlow.Model;\r\nusing System;\r\nusing System.Collect" +
            "ions.Generic;\r\nusing System.Linq;\r\nusing System.Xml.Linq;\r\n\r\nnamespace DataFlow." +
            "Factories\r\n{\r\n\tinternal class $entityType$Factory : IBuilder\r\n\t{\r\n\t\t#region Prep" +
            "are basic data\r\n\r\n\t\tprivate $DBEntities$ entitiesDbContext = null;\r\n\t\tprivate IE" +
            "numerable<XElement> dataFromDB = null;\r\n\r\n\t\tpublic $entityType$Factory(IEnumerab" +
            "le<XElement> dataFromDB)\r\n\t\t{\r\n\t\t\tentitiesDbContext = new $DBEntities$();\r\n\t\t\tth" +
            "is.dataFromDB = dataFromDB;\r\n\t\t}\r\n\r\n\t\t#endregion Prepare basic data\r\n\r\n\t\t#region" +
            " Interface implements\r\n\r\n\t\tpublic void ReadEntityFileToDB()\r\n\t\t{\r\n\t\t\ttry\r\n\t\t\t{\r\n" +
            "\t\t\t\tvar primary = Utility.Deserialize<$entityType$>(typeof($entityType$).Name) a" +
            "s $entityType$[];\r\n\r\n\t\t\t\tforeach (var item in primary)\r\n\t\t\t\t{\r\n\t\t\t\t\tif (entities" +
            "DbContext.$entitySet$.Any($entityKey$))\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t//todo\r\n\t\t\t\t\t}\r\n\t\t\t\t\telse" +
            "\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\tentitiesDbContext.$entitySet$.Add(item);\t//todo\r\n\t\t\t\t\t\tentitiesD" +
            "bContext.SaveChanges();\r\n\t\t\t\t\t}\r\n\t\t\t\t}\r\n\t\t\t}\r\n\t\t\tcatch (Exception)\r\n\t\t\t{\r\n\t\t\t\tth" +
            "row;\r\n\t\t\t}\r\n\t\t}\r\n\r\n\t\tpublic void CreateEntityFileToDestination()\r\n\t\t{\r\n\t\t\ttry\r\n\t" +
            "\t\t{\r\n\t\t\t\tList<$entityType$> entity = new List<$entityType$>();\r\n\t\t\t\tforeach (var" +
            " item in entitiesDbContext.$entitySet$)\r\n\t\t\t\t{\r\n\t\t\t\t\tif (dataFromDB.Any($entityK" +
            "ey2$))\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\tentity.Add(item);\r\n\t\t\t\t\t}\r\n\t\t\t\t}\r\n\r\n\t\t\t\tUtility.Serialize<" +
            "$entityType$>(entity, typeof($entityType$).Name);\r\n\t\t\t}\r\n\t\t\tcatch (Exception)\r\n\t" +
            "\t\t{\r\n\t\t\t\tthrow;\r\n\t\t\t}\r\n\t\t}\r\n\r\n\t\t#endregion Interface implements\r\n\t}\r\n}")]
        public string FactoriesTemplates {
            get {
                return ((string)(this["FactoriesTemplates"]));
            }
            set {
                this["FactoriesTemplates"] = value;
            }
        }
    }
}
