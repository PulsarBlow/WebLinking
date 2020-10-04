namespace WebLinking.Samples.SimpleApi.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using Properties;

    public class GetValueCollectionParameters
    {
        [Range(
            0,
            int.MaxValue,
            ErrorMessageResourceName =
                nameof(Resources.VALIDATIONERROR_PARAMETERS_OFFSET_OUTOFRANGE),
            ErrorMessageResourceType = typeof(Resources))]
        public int Offset { get; set; }

        [Range(
            1,
            100,
            ErrorMessageResourceName =
                nameof(Resources.VALIDATIONERROR_PARAMETERS_LIMIT_OUTOFRANGE),
            ErrorMessageResourceType = typeof(Resources))]
        public int Limit { get; set; } = 10;
    }
}
