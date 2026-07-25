using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArtHouse.Models.Enums
{
    //  OrderStatus is implemented as an enum instead of a string
    //to improve type safety, prevent typo-related bugs,
    //provide IntelliSense support, and make future
    //maintenance and extension much easier.

    public enum OrderStatus
    {
        Pending = 0,
        Completed = 1,
        Cancelled = 2
    }
}