using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Encapsulation class for all the config file extension types.
    /// </summary>
    public sealed class ConfigurationFile
    {
        /// <summary>
        /// Defaunt extension for the root config folder
        /// </summary>
        public const string FolderExtension = ".Sepura.Config";
        /// <summary>
        /// Data model file
        /// </summary>
        public const string UITranslationDir = "UITranslations";
        public const string UIDir = "UI";
        public const string DefaultCulture = "default";
        public const string DataModelFileExtension = ".DataModel.Config";
        //public const string DataStorageModelFileExtension = ".DataStorageModel.Config";
        public const string DataTierModelCollectionFileExtension = ".DataTiers.Config";
        public const string UserInterfaceFileExtension = ".UI.Config";
        public const string UserInterfaceTranslationFileExtension = ".UI.Lang.{0}.csv";
        public const string BusinessRuleFileExtension = ".Rule.Config";
        public const string DataModelValidationRuleFileExtension = ".DataModelValidation.Config";
        public const string QueryFileExtension = ".Query.Config";
        public const string QueryIndexExtension = ".QueryIndex.SQL";
        public const string DataModelFileName = "SepuraDataModel" + DataModelFileExtension;

        public const string EntityDeletionBehavioursFileName = "SepuraDataModel.EntityDeleteBehaviours.Config";

        //public const string DataStorageModelFileName = "SepuraDataModel" + DataStorageModelFileExtension;
        public const string DataTierModelCollectionFileName = "SepuraDataModel" + DataTierModelCollectionFileExtension;
        public const string DataModelValidationRuleFuleName = "SepuraDataModel" + DataModelValidationRuleFileExtension;
        public const string StoryboardFileExtension = ".Storyboard.Config";
        /// <summary>
        /// Data package definition
        /// </summary>
        public const string DataPackageFileExtension = ".DataPackage";
        /// <summary>
        /// Site map extension
        /// </summary>
        public const string SiteMapFilename = "Sepura.SiteMap";
    }
}
