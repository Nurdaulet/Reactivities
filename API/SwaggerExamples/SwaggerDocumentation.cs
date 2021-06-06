
namespace API.SwaggerExamples
{
    public static class SwaggerDocumentation
    {
        public const string UnauthorizedDescriptionMessage = "Indicates that users need to be authorized in order to access it";
        public static class CategoriesConstants
        {
            public const string SuccessfulGetRequestMessage = "Returns all categories with their subcategories";
            public const string BadRequestDescriptionMessage = "Indicates that there are not any categories";
        }
        public static class ItemConstants
        {
            public const string NotFoundDescriptionMessage = "Indicates that item does not exist";
            public const string BadRequestDescriptionMessage = "Indicates that the provided data is invalid or something else went wrong";

            public const string SuccessfulGetRequestMessage = "Returns all items";
            public const string SuccessfulGetRequestWithIdDescriptionMessage = "Successfully found item and returns it";

            public const string SuccessfulPostRequestDescriptionMessage = "Creates item successfully and returns the Id of the item";
            public const string SuccessfulPutRequestDescriptionMessage = "Item is updated successfully";
            public const string SuccessfulDeleteRequestDescriptionMessage = "Indicates that item is deleted successfully";
        }
        public static class PictureConstants
        {
            public const string BadRequestDescriptionMessage =
                "Indicates that picture either does not exist or user does not have permission to delete it";

            public const string SuccessfulGetRequestDescriptionMessage =
                "Pictures were uploaded successfully and their corresponding data will be returned";

            public const string SuccessfulDeleteRequestDescriptionMessage = "Picture is deleted successfully";

            public const string SuccessfulGetPictureDetailsRequestDescriptionMessage = "Pictures were retrieved successfully and returned";
        }
    }
}
