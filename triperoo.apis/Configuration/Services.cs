﻿using Funq;
using core.places.services;
using core.hotels.services;

namespace triperoo.apis.Configuration
{
    public class Services
    {
        public static void Register(Container container)
        {
            container.RegisterAutoWiredAs<PlaceService, IPlaceService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<HotelService, IHotelService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<HotelPriceService, IHotelPriceService>().ReusedWithin(ReuseScope.Container);
        }
    }
}