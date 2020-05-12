using Portfolio.Shared.DataModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Portfolio.Components.Classes
{
    public static class ViewHelpers
    {
        public static int GetMaxLengthForSubmissionModelProperty(string property)
        {
            var strLenAttr = typeof(SubmissionModel).GetProperty(property)
                                                    .GetCustomAttributes(typeof(StringLengthAttribute), false)
                                                    .Cast<StringLengthAttribute>()
                                                    .SingleOrDefault();

            return strLenAttr != null ? strLenAttr.MaximumLength : -1;
        }
    }
}
