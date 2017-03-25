﻿using library.events.dtos;
using System.Net.Http;
using System.Configuration;
using ServiceStack.Text;

namespace library.events.services
{
    public class EventService : IEventService
    {
        public EventDto ReturnEventsByLocation(string location, int pageSize, int pageNumber)
        {
            var url = string.Format(ConfigurationManager.AppSettings["events.eventful.url"], location, pageSize, pageNumber);

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<EventDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }
    }
}
