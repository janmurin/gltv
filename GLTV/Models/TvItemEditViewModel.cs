using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using GLTV.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLTV.Models
{
    public class TvItemEditViewModel
    {
        public TvItemEditViewModel()
        {
            TypeDropdownItems = Utils.Types;
        }

        public TvItem TvItem { get; set; }

        public List<SelectListItem> TypeDropdownItems { get; internal set; }

        [Display(Name = "Locations")]
        public CheckBoxList LocationCheckboxes { get; set; }

        [Required(ErrorMessage = "At least one file is required.")]
        public List<IFormFile> Files { get; set; }
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
}
