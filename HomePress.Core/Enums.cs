using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomePress
{

    public enum Gender
    {
        Female = 0, Male = 1
    }

    public enum UserStatus
    {
        Inactive = 0, Active = 1, Banned = 2
    }

    public enum UserTypes
    {
        Admin = 1, Agent = 2, Client = 3
    }

    public enum PropertyStatus
    {
        Draft = 1, Media = 2, Rejected = 3, Review = 4, Published = 99, Sold = 100, Archived = 101
    }

    public enum PropertyTypes
    {
        Apartment = 1, Villa = 2, Office = 3, Shop = 4
    }

    public enum PropertyCategories
    {
        Buy = 1, Rent = 2, ShortTermRent = 3
    }

    public enum PricePrefix
    {
        EUR = 1, USD = 2, TL = 3
    }

}
