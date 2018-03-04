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
        public EventService(ApplicationDbContext context, IHostingEnvironment env, SignInManager<ApplicationUser> signInManager)
            : base(context, env, signInManager)
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

            _context.Add(logEvent);
            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public List<LogEvent> FetchLogEventsAsync()
        {
            List<LogEvent> events = _context.LogEvent
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

            return events;
        }

        public Task AddClientEventAsync(string source, ClientEventType type, string message, int? itemId)
        {
            ClientEvent clientEvent = new ClientEvent();
            clientEvent.Source = source;
            clientEvent.Message = message;
            clientEvent.TvItemFileId = itemId;
            clientEvent.TimeInserted = DateTime.Now;
            clientEvent.Type = type;

            _context.Add(clientEvent);
            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public List<ClientEvent> FetchClientEventsAsync()
        {
            List<ClientEvent> events = _context.ClientEvent
                .Include(x => x.TvItemFile)
                .OrderByDescending(x => x.TimeInserted)
                .ToList();

            return events;
        }
    }
}
