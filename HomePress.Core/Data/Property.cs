using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace HomePress.Core.Data
{


    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int PropertyID { get; set; }

        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public PricePrefix PricePrefix { get; set; }
        public PropertyTypes PropertyType { get; set; }
        public PropertyStatus PropertyStatus { get; set; }
        public PropertyCategories Category { get; set; }

        public List<string>? Tags { get; set; }

        public List<Photo>? Photos { get; set; }

        public double RentDeposite { get; set; }

        public string? CountryId { get; set; }
        public string? StateId { get; set; }
        public string? CityId { get; set; }
        public string? DistrictId { get; set; }
        public string? PropertyAddress { get; set; }
        public string? MapLAT { get; set; }
        public string? MapLNG { get; set; }

        public string? BuildingDeliverDate { get; set; }
        public int BuildingBuildYear { get; set; }
        public int BuildingFloors { get; set; }
        public int BuildingUnits { get; set; }
        public int BuildingDistanceToBus { get; set; }
        public int BuildingDistanceToBeach { get; set; }
        public int BuildingDistanceToCityCenter { get; set; }
        public int BuildingDistanceToShoppingCenter { get; set; }
        public int BuildingDistanceToHospital { get; set; }
        public bool BuildingElevator { get; set; }
        public bool BuildingDoorman { get; set; }
        public bool BuildingSecurityCamera { get; set; }
        public bool BuildingGenerator { get; set; }
        public bool BuildingParkingLot { get; set; }
        public bool BuildingCentralTV { get; set; }
        public bool BuildingCommunalWifi { get; set; }
        public bool BuildingCommunalOutdoorSpace { get; set; }
        public bool BuildingChildrenPlayGround { get; set; }
        public bool BuildingFitnessCenter { get; set; }
        public bool BuildingSwimmingPool { get; set; }
        public bool BuildingAquapark { get; set; }
        public bool BuildingTurkishBath { get; set; }
        public bool BuildingIndoorSwimmingPool { get; set; }
        public bool BuildingMassageRoom { get; set; }
        public bool BuildingSauna { get; set; }
        public bool BuildingJacuzzi { get; set; }
        public bool BuildingSteamRoom { get; set; }
        public string? BuildingOtherFeatures { get; set; }



        public int UnitFloor { get; set; }
        public int UnitBedrooms { get; set; }
        public int UnitBathrooms { get; set; }
        public int UnitBalcony { get; set; }
        public int UnitMeterSq { get; set; }
        public List<string>? UnitFacades { get; set; }
        public bool UnitBrandNew { get; set; }
        public bool UnitSecondHanded { get; set; }
        public bool UnitWhiteGoods { get; set; }
        public bool UnitWaterHeater { get; set; }
        public bool UnitAirConditioner { get; set; }
        public bool UnitFurnished { get; set; }
        public bool UnitWifi { get; set; }
        public bool UnitPrivateParking { get; set; }
        public bool UnitSauna { get; set; }
        public bool UnitJacuzzi { get; set; }
        public bool UnitSeaView { get; set; }
        public bool UnitDuplex { get; set; }
        public bool UnitPenthouse { get; set; }
        public bool UnitLuxury { get; set; }
        public bool UnitCreditAvailable { get; set; }
        public bool UnitExchangeAvailable { get; set; }
        public string? UnitOtherFeatures { get; set; }


        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime PublishedAt { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedAt { get; set; }

    }

    public class Photo
    {
        public string? OriginalUrl { get; set; }
        public string? Width256 { get; set; }
        public string? Width512 { get; set; }
        public string? Width1024 { get; set; }
        public string? Width1920 { get; set; }
    }

    public enum PropertyStatus
    {
        Draft, Pending, Review, Published, SoldOut, Archived
    }

    public enum PropertyTypes
    {
        Apartment, Villa, Office, Shop
    }

    public enum PropertyCategories
    {
        Buy, Rent, ShortTermRent
    }

    public enum PricePrefix
    {
        EUR, USD, TL
    }



}
