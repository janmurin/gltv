using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Transactions;
using GLTV.Extensions;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLTV.Models
{
    public class TvItemEditViewModel
    {
        public TvItemEditViewModel()
        {
            TypeDropdownItems = Utils.Types;
            LocationCheckboxes = new CheckBoxList();
        }

        public TvItemEditViewModel(TvItem item) : this()
        {
            TvItem = item;
            LocationCheckboxes.LocationBanskaBystrica =
                item.Locations.Any(x => x.Location == Location.BanskaBystrica);
            LocationCheckboxes.LocationKosice =
                item.Locations.Any(x => x.Location == Location.Kosice);
            LocationCheckboxes.LocationZilina =
                item.Locations.Any(x => x.Location == Location.Zilina);
        }

        [TvItemValidation(ErrorMessage = "StartTime is after EndTime")]
        public TvItem TvItem { get; set; }

        public List<SelectListItem> TypeDropdownItems { get; internal set; }

        [Display(Name = "Locations")]
        public CheckBoxList LocationCheckboxes { get; set; }
    }

    [ValidateAtLeastOneChecked(ErrorMessage = "At least one location must be checked.")]
    public class CheckBoxList
    {
        public bool LocationKosice { get; set; }
        public bool LocationBanskaBystrica { get; set; }
        public bool LocationZilina { get; set; }
    }

    public class ValidateAtLeastOneCheckedAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Type type = value.GetType();
            IEnumerable<PropertyInfo> checkBoxeProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.PropertyType == typeof(bool));

            foreach (PropertyInfo checkBoxProperty in checkBoxeProperties)
            {
                var isChecked = (bool)checkBoxProperty.GetValue(value);
                if (isChecked)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(base.ErrorMessageString);
        }
    }

    public class TvItemValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            TvItem item = (TvItem)value;

            if (DateTime.Compare(item.StartTime, item.EndTime) < 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(base.ErrorMessageString);
        }
    }
}
