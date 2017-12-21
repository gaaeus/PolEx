using SQLite;
using System;
using System.Collections.Generic;

namespace PolEX.Model
{
    /// <summary>
    /// Categories to be used by the system. These are the standard divisors of information to be used to process information
    /// </summary>
    [Serializable]
    [Table("Category")]
    public class Category : BindableBase
    {
        #region Properties

        private UInt64? _category_id;
        private String _category_identifier;
        private String _category_name;
        private String _category_description;
        private UInt64 _parent_category_id;
        private ICollection<Category> _subcategories;

        [Column("category_id")]
        [PrimaryKey, AutoIncrement, Indexed]
        public UInt64? category_id
        {
            get { return category_id; }
            set { SetProperty(ref _category_id, value, "category_id"); }
        }

        [Column("category_identifier")]
        public String CategoryIdentifier
        {
            get { return _category_identifier; }
            set { SetProperty(ref _category_identifier, value, "CategoryIdentifier"); }
        }

        [Column("category_name")]
        public String CategoryName
        {
            get { return _category_name; }
            set { SetProperty(ref _category_name, value, "CategoryName"); }
        }

        [Column("category_description")]
        public String CategoryDescription
        {
            get { return _category_description; }
            set { SetProperty(ref _category_description, value, "CategoryDescription"); }
        }

        [Column("parent_category_id")]
        public UInt64 parent_category_id
        {
            get { return _parent_category_id; }
            set { SetProperty(ref _parent_category_id, value, "parent_category_id"); }
        }

        public ICollection<Category> SubCategories
        {
            get { return _subcategories; }
            set {
                ICollection<Category> localcollection = _subcategories;
                SetProperty(ref localcollection, value, "SubCategories");
            }
        }

        #endregion

        #region Constructor

        public Category() { }

        public Category(UInt64? category_id, String category_identifier, String category_name, String category_description, UInt64 parent_category_id, ICollection<Category> SubCategories)
        {
            this._category_id = category_id;
            this._category_identifier = category_identifier;
            this._category_name = category_name;
            this._category_description = category_description;
            this._parent_category_id = parent_category_id;
            this._subcategories = SubCategories;
        }

        #endregion
    }
}
