using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace GLTV.Services
{
    public class EventService : ServiceBase, IEventService
    {
        public EventService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
            : base(context, signInManager)
        {
        }

        public Task AddLogEventAsync(string author, LogEventType type, string message, int? itemId)
        {
            LogEvent logEvent = new LogEvent();
            logEvent.Author = author;
            logEvent.Message = message;
            logEvent.TvItemId = itemId;
            logEvent.TimeInserted = DateTime.Now;
            logEvent.Type = type;

            Context.Add(logEvent);
            Context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<List<LogEvent>> FetchLogEventsAsync()
        {
            List<LogEvent> events = Context.LogEvent
                .Include(x => x.TvItem)
                .OrderByDescending(x => x.TimeInserted)
                .ToList();

            foreach (LogEvent logEvent in events)
            {
                if (logEvent.Type == LogEventType.ItemDelete)
                {
                    logEvent.Message = $"User deleted item [{logEvent.TvItem.GetDetailHyperlink(false)}] with id [{logEvent.TvItemId}].";
                }

                if (logEvent.Type == LogEventType.ItemUpdate)
                {
                    logEvent.Message = $"User updated item [{logEvent.TvItem.GetDetailHyperlink(false)}] with id [{logEvent.TvItemId}].";
                }

                if (logEvent.Type == LogEventType.ItemDeleteFiles)
                {
                    logEvent.Message = $"User deleted files for item [{logEvent.TvItem.GetDetailHyperlink(false)}] with id [{logEvent.TvItemId}].";
                }

                if (logEvent.Type == LogEventType.ItemInsert)
                {
                    logEvent.Message = $"User inserted new item [{logEvent.TvItem.GetDetailHyperlink(false)}] with id [{logEvent.TvItemId}].";
                }

                if (logEvent.Type == LogEventType.AnonymousDetails)
                {
                    logEvent.Message = $"Displayed anonymous details for item [{logEvent.TvItem.GetDetailHyperlink(false)}] with id [{logEvent.TvItemId}].";
                }
            }

            return Task.FromResult(events);
        }

        public Task AddClientEventAsync(string source, ClientEventType type, string message, int? itemId)
        {
            ClientEvent clientEvent = new ClientEvent
            {
                Source = source,
                Message = message,
                TvItemFileId = itemId,
                TimeInserted = DateTime.Now,
                Type = type
            };

            Context.Add(clientEvent);
            Context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<List<ClientEvent>> FetchClientEventsAsync()
        {
            List<ClientEvent> events = Context.ClientEvent
                .Include(x => x.TvItemFile)
                .Where(x => x.Type != ClientEventType.ProgramRequest)
                .OrderByDescending(x => x.TimeInserted)
                .Skip(0)
                .Take(200)
                .ToList();

            foreach (ClientEvent logEvent in events)
            {
                if (logEvent.Type == ClientEventType.ImageRequest || logEvent.Type == ClientEventType.VideoRequest)
                {
                    logEvent.Message = $"Request for file {logEvent.TvItemFile.GetDetailHyperlink()}";
                }
            }

            return Task.FromResult(events);
        }

        public Task<List<ClientEvent>> FetchClientsLastProgramRequestAsync()
        {
            List<ClientEvent> events = Context.ClientEvent
                .Include(x => x.TvItemFile)
                .Where(x => x.Type == ClientEventType.ProgramRequest)
                .OrderByDescending(x => x.TimeInserted)
                .Skip(0)
                .Take(20000)
                .ToList();

            IEnumerable<string> sources = events.Select(x => x.Source).ToList().Distinct();
            List<ClientEvent> requestEvents = new List<ClientEvent>();

            foreach (string source in sources)
            {
                ClientEvent clientEvent = events.First(x => x.Source.Equals(source));
                var location = "unknown";
                if (clientEvent.Message.LastIndexOf(" ", StringComparison.Ordinal) > 0)
                {
                    location = clientEvent.Message.Substring(
                        clientEvent.Message.LastIndexOf(" ", StringComparison.Ordinal)).Trim();
                }
                clientEvent.Source += " (" + location +")";
                requestEvents.Add(clientEvent);
            }


            return Task.FromResult(requestEvents);

        }
    }
}
